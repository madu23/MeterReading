using ENSEK.Domain.Entities;
using ENSEK.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Application.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
    }
}
