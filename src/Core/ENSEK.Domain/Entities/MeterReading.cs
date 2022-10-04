using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Domain.Entities
{
    public class MeterReading : BaseEntity
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDate { get; set; }
        public int MeterReadValue { get; set; }
        public Account Account { get; set; }
    }
}
