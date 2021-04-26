using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TG.Models.CallBacks
{
    public class CallBack3
    {
        public static async Task Execute(Message message, TelegramBotClient botClient)
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
            await botClient.EditMessageTextAsync(message.Chat.Id,message.MessageId, $"Погода в {response.Name}  : {response.Main.Temp} °C", ParseMode.Default, false, keyboard);
        }
    }
}
