using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TG.Interface;
using TG.Models.Settings;

namespace TG.Models.Commands
{
    public class RegisterCommand : Command
    {
        private IControllerService controllerService { get; set; }
        IUserRepository repo;
        public RegisterCommand(IUserRepository r,IControllerService service)
        {
            repo = r;
            controllerService = service;
        }
        public override string[] Names { get; set; } = new string[] { "register", "/register" };
        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            await repo.Add(message.From.FirstName, message.From.LastName, message.From.Username, message.Chat.Id);
            await botClient.SendTextMessageAsync(message.Chat.Id, "Вы были зарегистрированы");
            Data.User user = await repo.GetTG(message.Chat.Id);
            controllerService.StatusNotif(user.Id, botClient);
        }
    }
}
