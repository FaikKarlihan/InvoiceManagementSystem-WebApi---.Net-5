using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using PaymentApi.Context;
using PaymentApi.Repositories;
using PaymentApi.Services;
using PaymentApi.Validators;
using WebApi.Middlewares;
using WebApi.Services;

namespace PaymentApi
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
            services.AddControllers()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ConfirmPaymentDtoValidator>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentApi", Version = "v1" });
            });
            services.AddHttpClient();
            // MongoContext tekil olacak
            services.AddSingleton<MongoContext>();
        
            // IMongoDatabase direkt Context içinden alınacak (artık refleksiyon yok!)
            services.AddSingleton(sp => sp.GetRequiredService<MongoContext>().Database);
        
            // Repository ve Service’ler
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<PaymentService>();
            services.AddSingleton<IloggerService, ConsoleLogger>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentApi v1"));
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseCustomExceptionMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
