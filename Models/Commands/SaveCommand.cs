using Microsoft.EntityFrameworkCore;
using RenovationBot.Managers.BaseManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RenovationBot.Models.Commands
{
    public class SaveCommand : Command
    {
        public override string Name => @"/save";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(this.Name);
        }

        public override async Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            using (TelegramContext db = new TelegramContext())
            {
                var uState = db.UserStates.Where(x => x.UserId == chatId).SingleOrDefault();
                var build = db.Buildings.Where(x => x.Id == uState.BuildingId).SingleOrDefault();
                if(String.IsNullOrEmpty(build.Link))
                {
                    string[] buildarr = new string[4];
                    buildarr[0] = build.Name;
                    buildarr[1] = build.Address;
                    buildarr[2] = build.Comment;


                    var photos = FilesManager.GetFileByBuilding(build.Id, botClient, db);

                    GoogleAPIauth.UploadNewBuilding(buildarr, photos, botClient);
                }
            }
        }
    }
}
