using AutoMapper;
using ENSEK.Application.Interfaces;
using ENSEK.Application.Models;
using ENSEK.Database.Context;
using ENSEK.Domain.Entities;
using ENSEK.Domain.Interfaces;
using ENSEK.Service.Implementation.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK.Test
{
    // [TestFixture]
    public class ServiceTest
    {
        private List<Account> accounts;
        private List<MeterReading> MeterReading;
        //public ServiceTest()
        //{


        //}

        [SetUp]
        public void SetUp()
        {
            SeedAccount();
            MeterReading = new List<MeterReading>();
        }

        [Test]
        public async Task Valid_MeterReading_ShouldBe_Saved()
        {
            // Arrange
            var ctx = new Mock<EnsekContext>();
            var logger = new Mock<ILogger<UploadService>>();
            var accountRepo = new Mock<IAccountRepository>();
            var meterReadingRepo = new Mock<IMeterReadingRepository>();
            var mapper = new Mock<IMapper>();
            var uow = new Mock<IUnitOfWork>();

            var input = new MeterReadingModel { AccountId = "2344", MeterReadingDateTime = "4/22/2019  9:24:00 AM", MeterReadValue = "1002" };

            var list = new List<MeterReadingModel>();
            list.Add(input);

            var record = new MeterReading 
            { 
                AccountId = Int32.Parse(input.AccountId), 
                MeterReadingDate = DateTime.Parse(input.MeterReadingDateTime), 
                MeterReadValue = Int32.Parse(input.MeterReadValue), 
                Id = "", CreatedBy = "", 
                CreatedOn = DateTime.Now 
            };

            var newMeterReading = new List<MeterReading>();
            newMeterReading.Add(record);

            // Setup
            mapper.Setup(x => x.Map<MeterReadingModel, MeterReading>(It.IsAny<MeterReadingModel>())).Returns(record);

            accountRepo.Setup(x => x.Find(Int32.Parse("2344"))).Returns(GetAccountBy(Int32.Parse
                (input.AccountId)));

            accountRepo.Setup(x => x.GetAll()).Returns(GetAll());

            meterReadingRepo.Setup(x => x.GetBy(m => m.AccountId == It.IsAny<int>() && m.MeterReadingDate >= It.IsAny<DateTime>())).Returns(GetMeterReadingBy(Int32.Parse(input.AccountId), DateTime.Parse(input.MeterReadingDateTime)));

            meterReadingRepo.Setup(x => x.ValidReading(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(IsValidReading(Int32.Parse(input.AccountId), DateTime.Parse(input.MeterReadingDateTime)));

            meterReadingRepo.Setup(x => x.UnitOfWork).Returns(ctx.Object);

            uow.Setup(x => x.SaveChangesAsync(default)).Returns(SaveChanges(record));

            meterReadingRepo.Setup(x => x.InsertRangeAsync(It.IsAny<List<MeterReading>>())).Returns(InsertMeterReading(newMeterReading));

            // Act
            var uploadService = new UploadService(logger.Object, accountRepo.Object, mapper.Object, meterReadingRepo.Object);
            var SUT = uploadService;
            var response = SUT.ImportMeterReading(list).Result;
            var successCount = response.ResultData.Successful;
            var failureCout = response.ResultData.Failed;

            // Assert
            Assert.AreEqual(1, successCount);
            Assert.AreEqual(0, failureCout);
        }


        [Test]
        public async Task Ensure_NewRead_Is_Older_Than_Existing_Read()
        {
            // Arrange
            var meterReadingRepo = new Mock<IMeterReadingRepository>();
            MeterReading.Add(new MeterReading { AccountId = 2350 , MeterReadingDate = DateTime.Parse("4/22/2019  12:25:00 PM"), MeterReadValue = 5684 });

            var newRead = new MeterReading { AccountId = 2350, MeterReadingDate = DateTime.Parse("4/22/2019  12:28:00 PM"), MeterReadValue = 1002 };
            
            // Setup
            meterReadingRepo.Setup(x => x.GetAll()).Returns(GetAllMeterReading());
            meterReadingRepo.Setup(x => x.ValidReading(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(IsValidReading(newRead.AccountId, newRead.MeterReadingDate));

            var SUT = meterReadingRepo.Object;

            // Act
            var expected = await SUT.ValidReading(newRead.AccountId, newRead.MeterReadingDate);

            // Assert
            Assert.IsTrue(expected);
        }

        private Account GetAccountBy(int accountId)
        {
            var account = accounts.Where(x => x.AccountId == accountId).SingleOrDefault();
            return account;
        }

        private IQueryable<Account> GetAll()
        {
            return accounts.AsQueryable();
        }

        private IQueryable<MeterReading> GetAllMeterReading()
        {
            return MeterReading.AsQueryable();
        }

        private void SeedAccount()
        {
            accounts = new List<Account>
            {
                new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Tommy", LastName = "Test", AccountId = 2344},
                new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Barry", LastName = "Test", AccountId = 2233 },
                new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Sally", LastName = "Test", AccountId = 8766 },
                new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Jerry", LastName = "Test", AccountId = 2345 }
            };
        }

        private Task InsertMeterReading(List<MeterReading> meterReadings)
        {
            MeterReading.AddRange(meterReadings);
            return Task.CompletedTask;
        }
        private Task<bool> SaveChanges(MeterReading record)
        {
            MeterReading.Add(record);
            return Task.FromResult(true);
        }
        private IEnumerable<MeterReading> GetMeterReadingBy(int accountId, DateTime meterReadingDateTime)
        {
            var records = MeterReading.Where(r => r.AccountId == accountId && r.MeterReadingDate >= meterReadingDateTime).ToList();
            return records;
        }
        private Task<bool> IsValidReading(int accountId, DateTime readDate)
        {
            if (!MeterReading.Any())
            {
                return Task.FromResult(true);
            }
            var record = MeterReading.Where(x => x.AccountId == accountId).OrderByDescending(r => r.MeterReadingDate).FirstOrDefault();
            if (record == null)
            {
                return Task.FromResult(true);
            }
            var truthy = readDate > record.MeterReadingDate;
            return Task.FromResult(truthy);
        }
    }
}