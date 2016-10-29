using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

using AntMeehan.Budget.WebApi;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http.Headers;

namespace AntMeehan.Budget.Tests
{

    public class TestBase : IDisposable
    {
        private readonly Guid _databaseUniqueId;
        private readonly Lazy<TestServer> _testServer;

        public TestBase()
        {
            _databaseUniqueId = Guid.NewGuid();
            using (var context = new TestBudgetContext(_databaseUniqueId))
            {
                Console.WriteLine($"EnsureCreated database {_databaseUniqueId}");
                context.Database.EnsureCreated();
            }

            _testServer = new Lazy<TestServer>(() => new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(collection =>
                {
                    collection.AddScoped<BudgetContext>(_ => new TestBudgetContext(_databaseUniqueId));
                })));
        }

        protected TestServer Server { get { return _testServer.Value; } }

        protected Action<IServiceCollection> AdditionalSerivceConfiguration = collection => { };

        protected T SubsituteAndConfigure<T>() where T : class
        {
            var sub = NSubstitute.Substitute.For<T>();
            AdditionalSerivceConfiguration += collection => { collection.AddScoped<T>(_ => sub); };

            return sub;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T content)
        {
            var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(content);
            var requestContent = new StringContent(jsonContent);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await Server.CreateClient().PostAsync(requestUri, requestContent);
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