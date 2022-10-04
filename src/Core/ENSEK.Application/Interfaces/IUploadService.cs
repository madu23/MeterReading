using ENSEK.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Application.Interfaces
{
    public interface IUploadService
    {
        Task<BaseResponse<MeterReadingResponse>> ImportMeterReading(IEnumerable<MeterReadingModel> data);
    }
}
