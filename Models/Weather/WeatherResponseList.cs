using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TG.Models
{
    public class WeatherResponseList
    {
        public City City { get; set; }
        public List<WeatherList> List { get; set; }
    }
}
