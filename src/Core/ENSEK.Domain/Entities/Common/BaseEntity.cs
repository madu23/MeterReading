using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Domain.Entities
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set;}
        public string CreatedBy { get; set; } = null!;
    }
}
