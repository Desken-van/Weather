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
    public class CallBack2
    {
        public static async Task Execute(Message message, TelegramBotClient botClient)
        {
            var keyboard = new InlineKeyboardMarkup(
                                                   new InlineKeyboardButton[][]
                                                   {
                                                            new [] {
                                                                InlineKeyboardButton.WithCallbackData("<-","callback3"),
                                                            },
                                                   }
                                               );
            WeatherResponseList response = await Weather.GetWeatherWeek();
            string tabl = $"-----------------------------------------------------------------------" +
                        $"\n| {response.List[0].Dt_txt} | {response.List[1].Dt_txt} | {response.List[2].Dt_txt} | {response.List[3].Dt_txt} |" +
                        $"\n-----------------------------------------------------------------------" +
                        $"\n|    {Math.Round(Convert.ToDouble(response.List[0].Main.Temp),2)}°C    |    {Math.Round(Convert.ToDouble(response.List[1].Main.Temp), 2)}°C   |    {Math.Round(Convert.ToDouble(response.List[2].Main.Temp), 2)}°C    |     {Math.Round(Convert.ToDouble(response.List[3].Main.Temp), 2)}°C     |" +
                        $"\n-----------------------------------------------------------------------";
            await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId,tabl, ParseMode.Default, false, keyboard);
        }
    }
}
