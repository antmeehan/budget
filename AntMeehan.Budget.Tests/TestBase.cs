using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

using AntMeehan.Budget.WebApi;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;

namespace AntMeehan.Budget.Tests
{

    public class TestBase : IDisposable
    {
        private readonly Guid _databaseUniqueId;

        public TestBase()
        {
            _databaseUniqueId = Guid.NewGuid();
            using (var context = new TestBudgetContext(_databaseUniqueId))
            {
                Console.WriteLine($"EnsureCreated database {_databaseUniqueId}");
                context.Database.EnsureCreated();
            }

            Server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(collection =>
                {
                    collection.AddScoped<BudgetContext>(_ => new TestBudgetContext(_databaseUniqueId));
                }));
        }

        protected TestServer Server { get; }

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T content)
        {
            var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(content);
            return await Server.CreateClient().PostAsync(requestUri, new StringContent(jsonContent));
        }

        void IDisposable.Dispose()
        {
            using (var context = new TestBudgetContext(_databaseUniqueId))
            {
                Console.WriteLine($"Deleting database {_databaseUniqueId}");
                context.Database.EnsureDeleted();
            }
        }
    }

    public class TestBudgetContext : BudgetContext
    {
        private readonly Guid _databaseUniqueId;

        public TestBudgetContext(Guid databaseUniqueId)
        {
            _databaseUniqueId = databaseUniqueId;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename=./{_databaseUniqueId:N}.db");
        }
    }
}