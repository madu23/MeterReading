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
    public class MeterReadingRepository : BaseRepository<MeterReading>, IMeterReadingRepository
    {
        public MeterReadingRepository(EnsekContext context) : base(context)
        {

        }

        public Task<bool> ValidReading(int accountId, DateTime readDate)
        {
            var records = GetBy(m => m.AccountId == accountId).OrderByDescending(r => r.MeterReadingDate).FirstOrDefault();
            if (records == null)
            {
                return Task.FromResult(true);
            }
            
            return Task.FromResult(readDate > records.MeterReadingDate);
        }
    }
}
