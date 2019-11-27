using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenovationBot.Models;

namespace RenovationBot
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
            services.AddTransient<TelegramContext>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"This is Telegram bot host. In the latest version you can see interface here." +
                    $"<a href=\"//www.free-kassa.ru/\"><img src=\"//www.free-kassa.ru/img/fk_btn/9.png\" title=\"Прием платежей на сайте\"></a>");
            });
            Bot.GetBotClientAsync().Wait();
        }
    }
}
