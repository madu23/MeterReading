using ENSEK.Application.Interfaces;
using ENSEK.Database.Context;
using ENSEK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Service.Implementation.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(EnsekContext context) : base(context)
        {

        }
    }
}
