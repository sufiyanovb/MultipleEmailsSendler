
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultipleEmailsSendler.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MultipleEmailsSendler.Migrations;

namespace MultipleEmailsSendler
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

            var connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDataContext>(options => options.UseSqlServer(connection));
            services.AddControllersWithViews();

            services.AddAutoMapper(typeof(MapperConfig));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });

        }
    }
}
