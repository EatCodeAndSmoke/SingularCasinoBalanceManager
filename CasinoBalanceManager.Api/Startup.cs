
using CasinoBalanceManager.Api.Swagger;
using CasinoBalanceManager.Application.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CasinoBalanceManager.Api {
    public class Startup {

        private readonly IConfiguration _config;

        public Startup(IConfiguration config) {
            this._config = config;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddCasinoBalanceManagerServices();

            var swaggerOptions = new SwaggerOptions();
            _config.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            services.AddSingleton(swaggerOptions);
            services.AddSwaggerGen(config => {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "Singular Casino Manager", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, [FromServices] SwaggerOptions options) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();


            app.UseSwagger(config => {
                config.RouteTemplate = options.JsonRoute;
            });

            app.UseSwagger();

            app.UseSwaggerUI(config => {
                config.SwaggerEndpoint(options.UIEndpoint, options.Description);
            });

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
