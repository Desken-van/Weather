using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TG.Models.Commands;

namespace TG.Models
{
    public static class AppSettings
    {
        public static string Key { get; set; } = "1596068759:AAGyM--Nu2TJF7K4DfZdNe4EBlwt_HRC6oU";
        public static string Url { get; set; } = "http://api.openweathermap.org/data/2.5/weather?q=Odesa&units=metric&appid=0827fcc12ca8886fe1b044c70ff37bf1";

        public static string HourUrl { get; set; } = "http://api.openweathermap.org/data/2.5/forecast?q=Odesa&units=metric&appid=0827fcc12ca8886fe1b044c70ff37bf1";
public static List<Command> GetCommand()
        {
            List<Command> list = new List<Command>();
            list.Add(new StartCommand());
            list.Add(new WeatherCommand());
            return list;
        }
    }
}
