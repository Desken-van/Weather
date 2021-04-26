using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TG.Models
{
    public class WeatherResponse
    {
        public string Name { get; set; }
        public TemparatureInfo Main { get; set; }
    }
}
