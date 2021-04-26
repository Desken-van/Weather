using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TG.Models.Commands;

namespace TG.Models.Settings
{
    public interface IControllerService
    {
        Task<Message> GetMessage(int id, string text, List<Command> commands, TelegramBotClient botClient);
        User GetUser(Data.User argument);
        void StatusNotif(int id, TelegramBotClient botClient);
        void UpdateNotif(int id, TelegramBotClient botClient);
        void DeleteNotif(int id, TelegramBotClient botClient);
        IEnumerable<UserResponse> SortList(IEnumerable<UserResponse> listusers, string sort, string range);
    }
}
