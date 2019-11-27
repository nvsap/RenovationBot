using RenovationBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RenovationBot.Managers.BaseManagers
{
    public class CallbackQueryManager
    {
        public static async void ChoseCallBackQuery(Telegram.Bot.TelegramBotClient botClient, Update Update)
        {
            try
            {
                using (TelegramContext db = new TelegramContext())
                {
                    var update = Update;
                    var user = db.Users.Where(x => x.UserId == update.CallbackQuery.From.Id).Single();
                    var callBack = update.CallbackQuery;
                    await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);

                    if (callBack.Data == "AddBuilding")
                    {
                       BuildingsManager.AddBuilding(botClient, db, update.CallbackQuery.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
