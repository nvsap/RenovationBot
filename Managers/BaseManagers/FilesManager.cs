using RenovationBot.Models;
using RenovationBot.Models.ObjectsModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RenovationBot.Managers.BaseManagers
{
    public class FilesManager
    {
        public static async void SaveFile(TelegramBotClient botClient, Message message, int buildingId)
        {
            if (message.Document != null)
            {
                using (TelegramContext db = new TelegramContext())
                {
                    DataFile LessonsData = new DataFile { FileId = message.Document.FileId, Name = message.Document.FileId + "Build", BuildingId = buildingId };
                    await db.DataFiles.AddAsync(LessonsData);
                    await db.SaveChangesAsync();
                }
            }
            else if (message.Photo != null && message.Photo.Count() > 0)
            {
                using (TelegramContext db = new TelegramContext())
                {
                    DataFile LessonsData = new DataFile { FileId = message.Photo[1].FileId, Name = "Photo" + message.Photo[1].FileId, BuildingId = buildingId };
                    await db.DataFiles.AddAsync(LessonsData);
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                //await botClient.SendTextMessageAsync(message.Chat.Id, "Отправте следующий файл, или введите одну из команд!", Telegram.Bot.Types.Enums.ParseMode.Default);
            }
        }

        public static List<string> GetFileByBuilding(int buildingId, TelegramBotClient botClient, TelegramContext db)
        {
            //botClient.DownloadFileAsync(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\")),  )
            var photos = db.DataFiles.Where(x => x.BuildingId == buildingId).Select(x => x.FileId).ToList<string>();

            return photos;
        }

        public static async void SendFile(TelegramBotClient botClient, Message message, string FileId)
        {
            //var file = await botClient.GetFileAsync(FileId);
            await botClient.SendDocumentAsync(message.Chat.Id, FileId);
            //await botClient.SendDocumentAsync(message.Chat.Id, file);

        }
    }
}
