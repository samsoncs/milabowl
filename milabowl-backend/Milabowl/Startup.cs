using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Milabowl.Business.Api;
using Milabowl.Business.Import;
using Milabowl.Business.Mappers;

namespace Milabowl
{
    public class Startup
    {
        private readonly string _myAllowSpecificOrigins = "AllowSpecific";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMilaResultsService, MilaResultsService>();
            services.AddScoped<IDataImportBusiness, DataImportBusiness>();
            services.AddScoped<IMilaPointsProcessorService, MilaPointsProcessorService>();
            services.AddScoped<IMilaRuleBusiness, MilaRuleBusiness>();
            services.AddScoped<IDataImportService, DataImportService>();
            services.AddScoped<IMilaResultsBusiness, MilaResultsBusiness>();
            services.AddScoped<IDataImportProvider, DataImportProvider>();
            services.AddScoped<IFantasyMapper, FantasyMapper>();
            //services.AddDbContext<FantasyContext>(optionsBuilder =>
            //{
            //    var connectionString = Configuration.GetConnectionString("Milabowl");
            //    optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            //});
            //services.AddDbContext<FantasyContext>(optionsBuilder =>
            //{
            //    var connectionString = Configuration.GetConnectionString("Milabowl");
            //    optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            //});

            services.AddMemoryCache();

            services.AddCors(options =>
            {
                options.AddPolicy(this._myAllowSpecificOrigins, builder =>
                {
                    builder
                    .WithOrigins("http://localhost:3000", 
                            "https://milabowl-frontend.azurewebsites.net", 
                            "http://milabowl-frontend.azurewebsites.net")
                        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Audience = Configuration["AzureAdB2C:ClientId"];
                opt.Authority = $"https://login.microsoftonline.com/{Configuration["AzureAdB2C:TenantId"]}/v2.0";
            });

            services.AddHttpClient();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(this._myAllowSpecificOrigins);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
