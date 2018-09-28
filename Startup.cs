using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebAPIAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAPIAssignment
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
            //Setting up db context, so that model can be used throughout the application
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("TodoList"));

            //Adding CORS
            services.AddCors(o => o.AddPolicy("AuthToAll", builder =>
            {
                //As the name suggests, allow response from any origin
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));    

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Use the above added cors service
            app.UseCors("AuthToAll");
            app.UseMvc();
        }
    }
}
