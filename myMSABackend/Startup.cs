using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using myMSABackend.Data;
using Microsoft.EntityFrameworkCore;

namespace myMSABackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerDocument(options =>
            {
                options.DocumentName = "myMSABackend";
                options.Version = "V1";

            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "myMSABackend", Version = "v1" });
            });
            services.AddDbContext<IDBContext, myDBContext>(options => options.UseSqlite(Configuration.GetConnectionString("WebAPIConnection")));
            services.AddScoped<IDBRepo, DBRepo>();
            services.AddHttpClient(Configuration["PokemonClientName"], configureClient: client =>
            {
                client.BaseAddress = new Uri(Configuration["PokemonAddress"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
