using CsvHelper;
using CsvHelper.Configuration;
using ENSEK.Application.Interfaces;
using ENSEK.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace ENSEK.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUploadService _uploadService;
        public ImportController(IConfiguration configuration, IUploadService uploadService)
        {
            _configuration = configuration;
            _uploadService = uploadService;
        }

        [HttpPost("meter-reading-uploads")]
        [ProducesResponseType(typeof(BaseResponse<MeterReadingResponse>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromForm] IFormFile file)
        {
            using var newMemoryStream = new MemoryStream();
            try
            {
                List<MeterReadingModel> dataSet;
                if(file == null)
                {
                    return BadRequest(new BaseResponse<MeterReadingResponse>
                    {
                        Succeeded = false,
                        Message = "Emtpy file. Please upload a valid file",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        
                    });
                }
                var fileExt = Path.GetExtension(file.FileName);
                if (fileExt != ".csv")
                {
                    return BadRequest(new BaseResponse<MeterReadingResponse>
                    {
                        Succeeded = false,
                        Message = "Bade file format. Please upload a valid csv file",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                    });
                }
                file.CopyTo(newMemoryStream);
                var path = _configuration["Storage:UploadPath"];
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileName = $"{Path.GetRandomFileName()}{fileExt}";
                var filePath = $"{path}\\{fileName}";
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    newMemoryStream.WriteTo(fs);
                    newMemoryStream.Close();
                    fs.Close();
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = false,
                    };
                    using (var reader = new StreamReader(filePath))
                    using (var csv = new CsvReader(reader, config))
                    {
                        var records = csv.GetRecords<MeterReadingModel>();
                        if (records.Any())
                        {
                            var uploadResult = await _uploadService.ImportMeterReading(records.ToList());
                            uploadResult.StatusCode = (int)HttpStatusCode.OK;
                            return Ok(uploadResult);
                        }
                        return BadRequest(new BaseResponse<MeterReadingResponse>
                        {
                            Succeeded = false,
                            Message = "Emtpy file. Please upload a valid file",
                            StatusCode = (int)HttpStatusCode.BadRequest,

                        });
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                newMemoryStream.Close();
            }
        }
    }
}
