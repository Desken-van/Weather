using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TG.Models.Commands
{
    public class WeatherCommand : Command
    {
        public override string[] Names { get; set; } = new string[] { "weather", "/weather" };
        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var keyboard = new InlineKeyboardMarkup(
                                                   new InlineKeyboardButton[][]
                                                   {
                                                            new [] {
                                                                InlineKeyboardButton.WithCallbackData("Через пару часов","callback1"),
                                                                InlineKeyboardButton.WithCallbackData("На ближайшие дни","callback2")
                                                            },
                                                   }
                                               );
            WeatherResponse response = await Weather.GetWeather();
            //565560272
            await botClient.SendTextMessageAsync(message.Chat.Id,$"Погода в {response.Name}  : {response.Main.Temp} °C", ParseMode.Default, false, false, 0, keyboard);
        }
    }
}
