using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENSEK.Application.Interfaces;
using ENSEK.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ENSEK.Tasks.StartupTasks
{
    public class SeedDbTask : IStartupTask
    {
        private readonly IServiceProvider _serviceProvider;
        const string LASTNAME = "Test";
        public SeedDbTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await SeedAccountData();
        }
        private async Task SeedAccountData()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var testData = new List<Account>
                    {
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Tommy", LastName = LASTNAME, AccountId = 2344, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Barry", LastName = LASTNAME, AccountId = 2233, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Sally", LastName = LASTNAME, AccountId = 8766, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Jerry", LastName = LASTNAME, AccountId = 2345, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Ollie", LastName = LASTNAME, AccountId = 2346, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Tara", LastName = LASTNAME, AccountId = 2347, CreatedBy=  "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Tammy", LastName = LASTNAME, AccountId = 2348, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Simon", LastName = LASTNAME, AccountId = 2349, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Colin", LastName = LASTNAME, AccountId = 2350, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Gladys", LastName = LASTNAME, AccountId = 2351, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Greg", LastName = LASTNAME, AccountId = 2352, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Tony", LastName = LASTNAME, AccountId = 2353, CreatedBy = "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Arthur", LastName = LASTNAME, AccountId = 2355, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Craig", LastName = LASTNAME, AccountId = 2356, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Laura", LastName = LASTNAME, AccountId = 6776, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Josh", LastName = LASTNAME, AccountId = 4534, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Freya", LastName = LASTNAME, AccountId = 1234, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Noddy", LastName = LASTNAME, AccountId = 1239, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Archie", LastName = LASTNAME, AccountId = 1240, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Lara", LastName = LASTNAME, AccountId = 1241, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Tim", LastName = LASTNAME, AccountId = 1242, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Graham", LastName = LASTNAME, AccountId = 1243, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Tony", LastName = LASTNAME, AccountId = 1244, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Neville", LastName = LASTNAME, AccountId = 1245, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Jo", LastName = LASTNAME, AccountId = 1246, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Jim", LastName = LASTNAME, AccountId = 1247, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                        new Account{ Id = Guid.NewGuid().ToString(), FirstName = "Pam", LastName = LASTNAME, AccountId = 1248, CreatedBy= "DbSeed", CreatedOn = DateTime.Now },
                    };
                    var accountRepository = scope.ServiceProvider.GetRequiredService<IAccountRepository>();

                    foreach (var testAccount in testData)
                    {
                        var account = accountRepository.Find(testAccount.AccountId);
                        if(account == null)
                        {
                            await accountRepository.InsertAsync(testAccount);
                        }
                    }
                    await accountRepository.UnitOfWork.SaveChangesAsync();
                }
                return;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
