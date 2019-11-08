using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RenovationBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*using(TelegramContext db = new TelegramContext())
            {
                Models.LearningModels.User user = new Models.LearningModels.User { Id = message.From.Id, Name = message.From.Username, ChatId = chatId, IsAdmin = false };
            }*/
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
