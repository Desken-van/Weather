using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TG.Models
{
    public class Weather
    {
        public static async Task<string> GetResponse(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = await streamReader.ReadToEndAsync();
            }
            return response;
        }
        public static async  Task<WeatherResponse> GetWeather()
        {
            string response = await GetResponse(AppSettings.Url);
            WeatherResponse weather = JsonConvert.DeserializeObject<WeatherResponse>(response);
            return weather;
        }
        public static async Task<WeatherResponse> GetWeatherHour()
        {
            string response = await GetResponse(AppSettings.HourUrl);
            WeatherResponseList weather = JsonConvert.DeserializeObject<WeatherResponseList>(response);
            WeatherResponse weatherResponse = GetHourWeather(weather);     
            return weatherResponse;
        }
        public static async Task<WeatherResponseList> GetWeatherWeek()
        {
            string response = await GetResponse(AppSettings.HourUrl);
            WeatherResponseList weather = JsonConvert.DeserializeObject<WeatherResponseList>(response);
            WeatherResponseList weatherResponse = GetWeekWeather(weather);
            return weatherResponse;
        }
        public static DateTime GetDatedate(string datestr)
        {
            datestr = datestr.Replace("-", " ");
            datestr = datestr.Replace(":", " ");
            List<int> list = new List<int>(datestr.Split(' ').Select(int.Parse));
            DateTime date = new DateTime(list[0], list[1], list[2], list[3], list[4], list[5]);
            return date;
        }
        public static WeatherResponse GetHourWeather(WeatherResponseList weather)
        {
            DateTime now = DateTime.Now;
            List<DateTime> timelist = new List<DateTime>();
            for (int i = 0; i < weather.List.Count; i++)
            {
                DateTime time = GetDatedate(weather.List[i].Dt_txt);
                if ((time.Year == now.Year) && (time.Month == now.Month) && (time.Day == now.Day))
                {
                    timelist.Add(time);
                }
            }
            DateTime cur = new DateTime();
            if (timelist.Count == 1)
            {
                cur = timelist[0];
            }
            else
            {
                cur = timelist[1];
            }
            WeatherResponse weatherResponse = new WeatherResponse();
            for (int i = 0; i < weather.List.Count; i++)
            {
                DateTime time = GetDatedate(weather.List[i].Dt_txt);
                if (time == cur)
                {
                    weatherResponse.Name = weather.City.Name;
                    weatherResponse.Main = weather.List[i].Main;
                }
            }
            return weatherResponse;       
        }
        public static WeatherResponseList GetWeekWeather(WeatherResponseList weather)
        {
            DateType currentdate = new DateType();
            DateType newdate = new DateType();
            DateTime currenttime = GetDatedate(weather.List[0].Dt_txt);
            currentdate.Year = currenttime.Year;
            currentdate.Month = currenttime.Month;
            currentdate.Day = currenttime.Day;
            float sum = weather.List[0].Main.Temp;
            int count = 1;
            List<WeatherList> total = new List<WeatherList>();
            for (int i = 1; i < weather.List.Count; i++)
            {
              DateTime newtime = GetDatedate(weather.List[i].Dt_txt);
              newdate.Year = newtime.Year;
              newdate.Month = newtime.Month;
              newdate.Day = newtime.Day;
                if (currentdate.Year == newdate.Year && currentdate.Day == newdate.Day && currentdate.Month == newdate.Month)
                {
                 sum += weather.List[i].Main.Temp;
                        count += 1;
                }
                else if(i == weather.List.Count - 1)
                {
                    TemparatureInfo temp = new TemparatureInfo() { Temp = sum / count };
                    WeatherList weath = new WeatherList() { Main = temp,Dt_txt = $"{currentdate.Year}.{currentdate.Month}.{currentdate.Day}"};
                    total.Add(weath);
                }
                else
                {
                  TemparatureInfo temp = new TemparatureInfo() { Temp = sum / count };
                  WeatherList weath = new WeatherList() { Main = temp, Dt_txt = $"{currentdate.Year}.{currentdate.Month}.{currentdate.Day}"};
                    total.Add(weath);
                  currentdate.Year = newdate.Year;
                  currentdate.Month = newdate.Month;
                  currentdate.Day = newdate.Day;
                    sum = weather.List[i].Main.Temp;
                   count = 1;
                }           
            }
            WeatherResponseList weekResponse = new WeatherResponseList()
            {
                City = weather.City,
                List = total,            
            };
            return weekResponse;
        }
    }
}
