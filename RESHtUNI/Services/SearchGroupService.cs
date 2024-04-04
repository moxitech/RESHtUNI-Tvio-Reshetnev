using Domain.WebTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESHtUNI.Services
{
    public class SearchGroupService : ISearchGroupService
    {
        //Получает вероятные группы и их ID
        public async Task<List<GroupResponse>?> GetFullGroupNameFromServer(string name)
        {
            var searchContent = name;
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://timetable.pallada.sibsau.ru/ajax/search_schedule");
            request.Headers.Add("authority", "timetable.pallada.sibsau.ru");
            request.Headers.Add("accept", "application/json, text/javascript, */*; q=0.01");
            request.Headers.Add("accept-language", "ru,en;q=0.9");
            request.Headers.Add("cache-control", "no-cache");
            //request.Headers.Add("content-type", "application/json");
            request.Headers.Add("origin", "https://timetable.pallada.sibsau.ru");
            request.Headers.Add("pragma", "no-cache");
            request.Headers.Add("referer", "https://timetable.pallada.sibsau.ru/timetable/");
            request.Headers.Add("sec-ch-ua", "\"Not_A Brand\";v=\"8\", \"Chromium\";v=\"120\", \"YaBrowser\";v=\"24.1\", \"Yowser\";v=\"2.5\"");
            request.Headers.Add("sec-ch-ua-mobile", "?0");
            request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            request.Headers.Add("sec-fetch-dest", "empty");
            request.Headers.Add("sec-fetch-mode", "cors");
            request.Headers.Add("sec-fetch-site", "same-origin");
            request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 YaBrowser/24.1.0.0 Safari/537.36");
            request.Headers.Add("x-requested-with", "XMLHttpRequest");
            request.Headers.Add("Cookie", "session_id=59ae30cafb7ff21fd3d6103bf1511c875e4a1540");
            var content = new StringContent("{\r\n    \"jsonrpc\": \"2.0\",\r\n    \"method\": \"call\",\r\n    \"params\": {\r\n        \"query\": " +
                $" \"{searchContent}\" " +
                "\r\n    },\r\n    \"id\": 1\r\n}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();

            Dictionary<string, object> jsonDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
            List<GroupResponse> groups;
            // Извлечение значения из массива "result"
            if (jsonDict.ContainsKey("result"))
            {
                string result = jsonDict["result"].ToString();
                List<Dictionary<string, object>> resultList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(result);
                groups = new List<GroupResponse>();
                foreach (var item in resultList)
                {
                    GroupResponse newGroup = new();
                    bool locker = false;
                    foreach (var kvp in item)
                    {
                        if (kvp.Key == "name")
                        {
                            if (kvp.Value.ToString().Length > 9)
                            {
                                locker = true;
                                continue;
                            }
                            newGroup.Name = (string)kvp.Value;
                        }
                        if (kvp.Key == "id")
                        {
                            if (locker)
                            {
                                locker = false;
                                continue;
                            }
                            newGroup.Id = Convert.ToInt32(kvp.Value);
                            groups.Add(newGroup);
                        }
                    }
                }
                foreach (GroupResponse group in groups)
                {
                    Console.WriteLine(group.ToString());
                }
            }
            else
            {
                throw new Exception("Ошибка парсинга групп при поиске!");
            }
            return groups;
        }


    }
}
