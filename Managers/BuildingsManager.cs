using RenovationBot.Models;
using RenovationBot.Models.ObjectsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RenovationBot.Managers
{
    public class BuildingsManager
    {
        public static async void MainMenu(Models.ObjectsModels.User user, TelegramContext db, Message message, Telegram.Bot.TelegramBotClient botClient)
        {
                var keyboard = new InlineKeyboardMarkup(
                                    new InlineKeyboardButton[]
                                    {
                                            new InlineKeyboardButton{ Text = "Нова будівля", CallbackData = "AddBuilding"}

                                    }
                                );

                var mess = await botClient.SendTextMessageAsync(message.Chat.Id, "Привіт, " + message.Chat.FirstName +
                "! \n\nЦей бот створений для для того, щоб ми якомога швидше могли шукати будівлі. яким потрібна наша допомога. " +
                "\n\nШвидше тисни кнопку, та рятуй прекрасне місто!🦄", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyboard);
        }
        public static async void AddBuilding(TelegramBotClient botClient, TelegramContext db, Message message)
        {
            try
            {
                StateManager.StateUpdate(message.Chat.Id, (int)UserStatesEnum.AddBuildingName);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Введи назву будівлі", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Ошибка: " + ex.Message, Telegram.Bot.Types.Enums.ParseMode.Default);
            }
        }
        public static async void AddBuildingName(TelegramBotClient botClient, TelegramContext db, Message message)
        {
            try
            {
                var building = new Building() { Name = message.Text, ParrentUserId = db.Users.Where(x => x.UserId == message.Chat.Id).Select(x=> x.Id).SingleOrDefault() };
                db.Buildings.Add(building);
                db.SaveChanges();
                StateManager.StateUpdate(message.Chat.Id, (int)UserStatesEnum.AddBuildingAddress, building.Id);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Введи адресу будівлі", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Ошибка: " + ex.Message, Telegram.Bot.Types.Enums.ParseMode.Default);
            }
        }
        public static async void AddBuildingAddress(TelegramBotClient botClient, TelegramContext db, Message message, UserState userState)
        {
            try
            {
                var building = db.Buildings.Where(x => x.Id == userState.BuildingId).SingleOrDefault();
                building.Address = message.Text;
                db.Buildings.Update(building);
                db.SaveChanges();
                StateManager.StateUpdate(message.Chat.Id, (int)UserStatesEnum.AddBuildingComment);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Введи коментар до будівлі", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Ошибка: " + ex.Message, Telegram.Bot.Types.Enums.ParseMode.Default);
            }
        }
        public static async void AddBuildingComment(TelegramBotClient botClient, TelegramContext db, Message message, UserState userState)
        {
            try
            {
                var building = db.Buildings.Where(x => x.Id == userState.BuildingId).SingleOrDefault();
                building.Comment = message.Text;
                db.Buildings.Update(building);
                db.SaveChanges();
                StateManager.StateUpdate(message.Chat.Id, (int)UserStatesEnum.AddBuildingPhoto);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Надсилай фото будівлі, а коли закінчиш напиши команду /save щоб зберегти пост!", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Ошибка: " + ex.Message, Telegram.Bot.Types.Enums.ParseMode.Default);
            }
        }
    }
}
