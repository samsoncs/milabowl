using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Milabowl.Infrastructure.Contexts;
using Milabowl.Utils;
using NUnit.Framework;

namespace Milabowl.Test.Integration
{
    public abstract class ControllerTestBase
    {
        private const string TEST_AUTHENTICATION_SCHEME = "Test";

        protected WebApplicationFactory<Startup> Factory;
        protected HttpClient HttpClient;
        protected FantasyContext FantasyContext;
        private IServiceScope _scope;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            this.Factory = this.GetApplicationFactory();
            this.Factory.Server.PreserveExecutionContext = true;
            this.HttpClient = this.Factory.CreateClient();
            this._scope =  this.Factory.Services.CreateScope();
            this.FantasyContext = this._scope.ServiceProvider.GetRequiredService<FantasyContext>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this.HttpClient.Dispose();
            this.Factory.Dispose();
            this.FantasyContext.Dispose();
            this._scope.Dispose();
        }

        private WebApplicationFactory<Startup> GetApplicationFactory()
        {
            var projectDir = Directory.GetCurrentDirectory();
            var appsettingsPath = Path.Combine(projectDir, "appsettings.json");

            return new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => builder.ConfigureAppConfiguration((context, config) =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddAuthentication(TEST_AUTHENTICATION_SCHEME)
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                TEST_AUTHENTICATION_SCHEME, options => { });

                        var descriptor = services
                            .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<FantasyContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<FantasyContext>(optionsBuilder =>
                        {
                            var connectionString = Startup.Configuration.GetConnectionString("Milabowl");
                            optionsBuilder.UseSqlServer(connectionString);
                        });

                    });

                    builder.ConfigureTestServices(this.AddMockServices);

                    config.AddJsonFile(appsettingsPath);
                })
            );
        }

        public virtual void AddMockServices(IServiceCollection services) { }
    }

    class TestAuthHandler: AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test user"), new Claim(ClaimTypes.Role, ApplicationRoles.MilaAdmin) };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
