using ENSEK.Application.Mappers;
using ENSEK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Application.Models
{
    public class MeterReadingModel : IMapFromVmToEntity<MeterReading>
    {
        public string AccountId { get; set; }
        public string MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }
}
