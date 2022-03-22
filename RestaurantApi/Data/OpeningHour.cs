using System.Collections.Generic;

namespace RestaurantApi.Data
{
    public class OpeningHour
    {
        public string Day { get; set; }
        public List<Dictionary<string, string>> OpeningHours { get; set; }
    }
}
