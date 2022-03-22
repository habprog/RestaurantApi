using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestaurantApi.Data;
using System;
using System.Collections.Generic;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpeningHourController : ControllerBase
    {

        [HttpPost]
        public IActionResult AddOpeningHour(List<OpeningHour> openh)
        {
            string day = "";
            string openT = "";
            int openCount = 0;
            int closeCount = 0;
            string closeT = "";
            List<string> jOut = new();
            string outputTemp = "";
            foreach (OpeningHour open in openh)
            {
                day = open.Day;
                switch (day.ToLower())
                {
                    case "monday":
                    case "tuesday":
                    case "wednesday":
                    case "thursday":
                    case "friday":
                    case "saturday":
                    case "sunday":
                        if (open.OpeningHours.Count > 0)
                        {
                            foreach (var item in open.OpeningHours)
                            {

                                foreach (KeyValuePair<string, string> kvp in item)
                                {
                                    if (kvp.Key.ToLower() == "type" && kvp.Value.ToLower() == "open")
                                    {
                                        openCount++;
                                    }

                                    if (kvp.Key.ToLower() == "type" && kvp.Value.ToLower() == "close")
                                    {
                                        closeCount++;
                                    }

                                    if (openCount != 0)
                                    {
                                        if (kvp.Key.ToLower() == "value")
                                        {
                                            openT = kvp.Value;
                                        }
                                    }

                                    if (closeCount != 0)
                                    {
                                        if (kvp.Key.ToLower() == "value")
                                        {
                                            closeT = kvp.Value;
                                        }
                                    }

                                }

                                openCount = 0;
                                closeCount = 0;

                            }
                        }
                        else
                        {
                            openT = "";
                        }

                        if (openT != "" && closeT != "")
                        {

                            outputTemp = $"{day}: {UnixTimeStampToDateTime(Convert.ToDouble(openT)).ToShortTimeString()}" +
                                $" - {UnixTimeStampToDateTime(Convert.ToDouble(closeT)).ToShortTimeString()}";
                        }
                        else if (openT != "" && closeT == "")
                        {
                            outputTemp = $"{day}: {UnixTimeStampToDateTime(Convert.ToDouble(openT)).ToShortTimeString()}";
                        }
                        else if (openT == "")
                        {
                            outputTemp = $"{day}: Closed";
                        }
                        break;
                    default:
                        return BadRequest("Invalid days of the week");
                        break;
                }

                Console.WriteLine(outputTemp);
                jOut.Add(outputTemp);
                openT = "";
                closeT = "";
                
            }
            return Ok(jOut);
        }


        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }


    }
}
