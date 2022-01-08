using System;
using System.Net.Http;
using System.Threading.Tasks;
using MyPomodoro.Application.Interfaces.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyPomodoro.Infrastructure.Persistence.Services
{
    public class DateTimeService : IDateTimeService
    {
        public async Task<DateTime> GetLocalTime(string ipAddress)
        {
            DateTime localtime = DateTime.Now;
            if (!string.IsNullOrEmpty(ipAddress))
            {
                HttpClient client = new HttpClient();
                string url = $"https://geo.ipify.org/api/v1?apiKey=at_MrHGKuFmIXqQ7I8ylR2f7HP5qs6Pp&ipAddress={ipAddress.Trim()}";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                if (responseBody.Contains("timezone"))
                {
                    var data = (JObject)JsonConvert.DeserializeObject(responseBody);
                    string timeZone = data["location"]["timezone"].Value<string>();
                    if (!string.IsNullOrEmpty(timeZone.Trim()))
                    {
                        string operation = timeZone[0].ToString();
                        int begin = timeZone.IndexOf(timeZone[0]);
                        int end = timeZone.LastIndexOf(':');
                        string hour = timeZone.Substring(begin + 1, end - 1);
                        string minute = timeZone.Substring(end + 1);
                        localtime = DateTime.UtcNow.AddHours(Convert.ToInt32(operation + hour)).AddMinutes(Convert.ToInt32(operation + minute));
                    }
                }
            }
            return await Task.FromResult(localtime);
        }
    }
}