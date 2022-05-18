using Api.Common;
using Api.Filters;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Api
{
    /// <summary>
    ///     Application startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Initializes startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        ///     The configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     Configures the services.
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => { options.Filters.Add<ApiExceptionFilterAttribute>(); });

            services.AddHttpContextAccessor();
            services.AddApplication(Configuration);
            services.AddInfrastructure(Configuration);
            services.AddSwagger();
            services.AddJwtAuth(Configuration);
            services.AddInternalServices(Configuration);
        }

        /// <summary>
        ///     Configures the application
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder</param>
        /// <param name="env">Web host environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/MatePortalSpecification/swagger.json", "MatePortal");
                c.RoutePrefix = string.Empty;
            });

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}