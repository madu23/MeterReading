using AutoMapper;
using ENSEK.Application.Interfaces;
using ENSEK.Application.Models;
using ENSEK.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Service.Implementation.Services
{
    public class UploadService : IUploadService
    {
        private readonly ILogger _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IMeterReadingRepository _meterReadingRepository;

        public UploadService(
            ILogger<UploadService> logger, 
            IAccountRepository accountRepository, 
            IMapper mapper, 
            IMeterReadingRepository meterReadingRepository
            )
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _meterReadingRepository = meterReadingRepository;
        }
        public async Task<BaseResponse<MeterReadingResponse>> ImportMeterReading(IEnumerable<MeterReadingModel> meterReadings)
        {
            var response = new BaseResponse<MeterReadingResponse> { ResultData = new MeterReadingResponse() };
            List<MeterReading> meterReadingList = new List<MeterReading>();

            // validate each meter reading data
            foreach (var data in meterReadings)
            {
                if (String.IsNullOrEmpty(data.MeterReadValue))
                {
                    // add to the count of failed record
                    response.ResultData.Failed++;
                    continue;
                }
                var validData = UInt32.TryParse(data.MeterReadValue, out var value);
                if (!validData)
                {
                    // add to the count of failed record
                    response.ResultData.Failed++;
                    continue;
                }
                
                // check if the meter reading is for a valid account
                var isValidAccount = _accountRepository.Find(Int32.Parse(data.AccountId));
                if(isValidAccount == null)
                {
                    // add to the count of failed record
                    response.ResultData.Failed++;
                    continue;
                }
                // check if meter reading record is a new record
                var readingDate = DateTime.Parse(data.MeterReadingDateTime);
                var isNewRecord = await _meterReadingRepository.ValidReading(Int32.Parse(data.AccountId), readingDate);
                //var dataExist = _meterReadingRepository.GetBy(m => m.AccountId == Int32.Parse(data.AccountId) && m.MeterReadingDate >= readingDate).FirstOrDefault();
                if (!isNewRecord)
                {
                    // add to the count of failed record
                    response.ResultData.Failed++;
                    continue;
                }
                var meterReadingRecord = ProcessMeterReading(data);
                meterReadingList.Add(meterReadingRecord);

            }
            await _meterReadingRepository.InsertRangeAsync(meterReadingList);
            await _meterReadingRepository.UnitOfWork.SaveChangesAsync();
            response.ResultData.Successful = meterReadingList.Count();
            response.Succeeded = true;
            response.Message = "Successfully processed files";
            return response;
        }

        private MeterReading ProcessMeterReading(MeterReadingModel model)
        {
            if(model != null)
            {
                var newAccount = _mapper.Map<MeterReadingModel, MeterReading>(model);
                newAccount.MeterReadingDate = DateTime.Parse(model.MeterReadingDateTime);
                newAccount.Id = Guid.NewGuid().ToString();
                newAccount.CreatedOn = DateTime.Now;
                newAccount.CreatedBy = "System";
                return newAccount;
            }
            return null;
        }
    }
}
