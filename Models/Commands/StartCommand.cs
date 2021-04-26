using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TG.Models.Commands
{
    public class StartCommand : Command
    {
        public override string[] Names { get; set; } = new string[] { "start", "/start"};
        public override async Task Execute(Message message,TelegramBotClient botClient)
        {
            //565560272
            string str = "------------------------------------------------------------------------------------------------" +
                       "\n            Приветсвую дружище.Я бот погоды!                  " +
                       "\n  Для использования команд нужно зарегестрироваться в базе!!!!" +
                       "\n------------------------------------------------------------------------------------------------" +
                        "\n Команды для работы : "+                        
                       "\n /register - регистрация  в базе данных"+
                       "\n /weather - оповещатель погоды";
            await botClient.SendTextMessageAsync(message.Chat.Id, str);
        }
    }
}
