using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Milabowl.Infrastructure.Contexts;
using NUnit.Framework;

namespace Milabowl.Test.BaseTestClasses
{
    public class DbBaseTest
    {
        protected FantasyContext FantasyContext;

        public DbBaseTest()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var contextBuilder = new DbContextOptionsBuilder<FantasyContext>(
                new DbContextOptions<FantasyContext>()
            ).UseSqlServer(configuration.GetConnectionString("Milabowl"));

            this.FantasyContext = new FantasyContext(contextBuilder.Options);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.FantasyContext.Dispose();
        }
    }
}
