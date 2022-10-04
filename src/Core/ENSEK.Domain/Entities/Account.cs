using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Domain.Entities
{
    public class Account : BaseEntity
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<MeterReading> MeterReadings { get; set; }
    }
}
