using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FinanceApi.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi
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
             services.AddCors(options =>
            {
                // BEGIN01
                options.AddPolicy("AllowAllMethods",
                builder =>
                {
                    builder.WithOrigins("http://localhost:5000", "https://localhost:5000")
                        .AllowAnyMethod();
                });
            });
            services.AddDbContext<UserContext>(u =>
                u.UseInMemoryDatabase("Users"));
            services.AddDbContext<CategoryContext>(c =>
                c.UseInMemoryDatabase("Categories"));
            services.AddDbContext<CategoryBudgetContext>(cb =>
                cb.UseInMemoryDatabase("CategoryBudgets"));
            services.AddDbContext<TransactionContext>(t =>
                t.UseInMemoryDatabase("Transactions"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:5000")
                              .AllowAnyMethod()
                              .AllowCredentials()
                              .AllowAnyHeader());

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
