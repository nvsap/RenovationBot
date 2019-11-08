using Microsoft.EntityFrameworkCore;
using RenovationBot.Managers.BaseManagers;
using RenovationBot.Models;
using RenovationBot.Models.ObjectsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RenovationBot.Managers
{
    public class StateManager
    {
        public static void StateUpdate(long userId, int state, int? buildingId = null)
        {
            using (TelegramContext db = new TelegramContext())
            {
                var userState = db.UserStates.Where(x => x.UserId == userId).SingleOrDefault();
                userState.State = state;
                if (buildingId != null)
                    userState.BuildingId = (int)buildingId;
                db.UserStates.Update(userState);
                db.SaveChanges();
            }
        }
        public static void StateControl(Telegram.Bot.TelegramBotClient botClient, Update update)
        {
            using (TelegramContext db = new TelegramContext())
            {
                var userState = db.UserStates.Where(x => x.UserId == update.Message.From.Id).SingleOrDefault();
                ActionStateSelected(userState, db, botClient, update);
            }
        }
        public static void ActionStateSelected(UserState userState,
                                                    TelegramContext db,
                                                    Telegram.Bot.TelegramBotClient botClient,
                                                    Update update)
        {
            switch (userState.State)
            {
                case (int)UserStatesEnum.Empty://Empty
                    break;
                case (int)UserStatesEnum.AddBuildingName://AddCourse
                    BuildingsManager.AddBuildingName(botClient, db, update.Message);
                    break;
                case (int)UserStatesEnum.AddBuildingAddress://CoursesList
                    BuildingsManager.AddBuildingAddress(botClient, db, update.Message, userState);
                    break;
                case (int)UserStatesEnum.AddBuildingComment://AddCourseName
                    BuildingsManager.AddBuildingComment(botClient, db, update.Message, userState);
                    break;
                case (int)UserStatesEnum.AddBuildingPhoto://AddCourseName
                    FilesManager.SaveFile(botClient, update.Message, userState.BuildingId);
                    break;

            }
        }
    }
}
