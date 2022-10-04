using ENSEK.Application.Mappers;
using ENSEK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Application.Models
{
    public class AccountModel : IMapFromVmToEntity<Account> 
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
