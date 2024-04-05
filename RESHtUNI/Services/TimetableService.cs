using Domain;
using HtmlAgilityPack;
using RESHtUNI.Data;
using System.Net;
using System.Text.RegularExpressions;
namespace RESHtUNI.Services
{
    public class TimetableService : ITimeTableService
    {

        // Получает расписание Заданной группы по сети 
        public async Task<GroupTimeTable> GetTimeTableByGroup(string groupName, string id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://timetable.pallada.sibsau.ru/timetable/");
            request.Headers.Add("authority", "timetable.pallada.sibsau.ru");
            request.Headers.Add("accept", "*/*");
            request.Headers.Add("accept-language", "ru,en;q=0.9");
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("cookie", "session_id=59ae30cafb7ff21fd3d6103bf1511c875e4a1540; frontend_lang=ru_RU; frontend_lang=ru_RU; session_id=59ae30cafb7ff21fd3d6103bf1511c875e4a1540; session_id=59ae30cafb7ff21fd3d6103bf1511c875e4a1540");
            request.Headers.Add("pragma", "no-cache");
            request.Headers.Add("referer", "https://timetable.pallada.sibsau.ru/timetable/");
            request.Headers.Add("sec-ch-ua", "\"Not_A Brand\";v=\"8\", \"Chromium\";v=\"120\", \"YaBrowser\";v=\"24.1\", \"Yowser\";v=\"2.5\"");
            request.Headers.Add("sec-ch-ua-mobile", "?0");
            request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            request.Headers.Add("sec-fetch-dest", "script");
            request.Headers.Add("sec-fetch-mode", "no-cors");
            request.Headers.Add("sec-fetch-site", "same-origin");
            request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 YaBrowser/24.1.0.0 Safari/537.36");
            var content = new MultipartFormDataContent();
            content.Add(new StringContent($" {groupName}"), "query");
            content.Add(new StringContent($" {id}"), "id");
            request.Content = content;
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if (response is { StatusCode: HttpStatusCode.Redirect, Headers: { Location: { } } })
            {
                request = new HttpRequestMessage(HttpMethod.Get, response.Headers.Location);
                response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            }
            //FIXME TODO :: HERE
            var timeTableObject = await ParseTimeTable(await response.Content.ReadAsStringAsync(), groupName, id);
            TestDataController.TimeTable = timeTableObject;
            return timeTableObject;
        }

        public async Task<GroupTimeTable> ParseTimeTable(string HtmlString, string groupName, string id)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(HtmlString);
            var daysOfWeek1 = htmlDocument.DocumentNode.SelectNodes("//*[@id=\"week_1_tab\"]/div[*]");
            var daysOfWeek2 = htmlDocument.DocumentNode.SelectNodes("//*[@id=\"week_2_tab\"]/div[*]");
            GroupTimeTable timeTable = new GroupTimeTable();
            timeTable.Name = groupName;
            timeTable.UniversityId = id;
            timeTable.Date = DateTime.Now;
            timeTable.Weeks = new() { };

            if (daysOfWeek1 != null)
            {
                var counter = 1;
                Week week = new();
                week.Days = new();
                week.Number = 1;
                foreach (var day in daysOfWeek1)
                {
                    Day _ = new Day();
                    string dayOfWeek = day.SelectSingleNode(".//div[contains(@class, 'header')]/div")?.InnerText;
                    _.DayName = dayOfWeek.Trim();
                    _.Lessons = new();
                    var lessCounter = 0;
                    var scheduleLines = day.SelectNodes($"//*[@id=\"week_1_tab\"]/div[{counter}]/div[2]/div[*]");
                    if (scheduleLines != null)
                    {
                        foreach (var line in scheduleLines)
                        {
                            string time = line.SelectSingleNode(".//div[contains(@class, 'hidden-xs')]")?.InnerText;
                            string discipline = line.SelectSingleNode(".//span[contains(@class, 'name')]")?.InnerText;
                            string teacher = line.SelectSingleNode($".//li/i[contains(@class, 'fa-user')]/parent::li/a")?.InnerText;
                            string audithory = line.SelectSingleNode($"//li/i[contains(@class, 'fa fa-compass')]/parent::li/a")?.InnerText;
                            string subgroup = line.SelectSingleNode($".//li/i[contains(@class, 'fa fa-paperclip')]/parent::li")?.InnerText;

                            var lesson = new Lesson()
                            {
                                Auditorium = Regex.Replace(audithory, @"\s+", " ").Trim(),
                                Name = discipline.Trim(),
                                Teacher = Regex.Replace(teacher, @"\s+", " ").Trim(),
                                Time = time.Trim(),
                            };
                            if (subgroup != null)
                                lesson.SubGroup = Regex.Replace(subgroup, @"\s+", " ").Trim();
                            _.Lessons.Add(lesson);
                            lessCounter += 1;
                            if (lessCounter >= scheduleLines.Count)
                            {
                                counter += 1;
                                break;
                            }
                        }
                    }
                    week.Days.Add(_);
                }
                timeTable.Weeks.Add(week);
            }
            if (daysOfWeek2 != null)
            {
                var counter = 1;
                Week week = new();
                week.Days = new();
                week.Number = 2;
                foreach (var day in daysOfWeek2)
                {
                    Day _ = new Day();
                    string dayOfWeek = day.SelectSingleNode(".//div[contains(@class, 'header')]/div")?.InnerText;
                    _.DayName = dayOfWeek.Trim();
                    _.Lessons = new();
                    var lessCounter = 0;
                    var scheduleLines = day.SelectNodes($"//*[@id=\"week_2_tab\"]/div[{counter}]/div[2]/div[*]");

                    if (scheduleLines != null)
                    {
                        foreach (var line in scheduleLines)
                        {
                            // Получаем время и дисциплину
                            string time = line.SelectSingleNode(".//div[contains(@class, 'hidden-xs')]")?.InnerText;
                            string discipline = line.SelectSingleNode(".//span[contains(@class, 'name')]")?.InnerText;
                            string teacher = line.SelectSingleNode($".//li/i[contains(@class, 'fa-user')]/parent::li/a")?.InnerText;
                            string audithory = line.SelectSingleNode($"//li/i[contains(@class, 'fa fa-compass')]/parent::li/a")?.InnerText;
                            string subgroup = line.SelectSingleNode($".//li/i[contains(@class, 'fa fa-paperclip')]/parent::li")?.InnerText;
                            var lesson = new Lesson()
                            {
                                Auditorium = Regex.Replace(audithory, @"\s+", " ").Trim(),
                                Name = discipline.Trim(),
                                Teacher = Regex.Replace(teacher, @"\s+", " ").Trim(),
                                Time = time.Trim(),
                            };
                            if (subgroup != null)
                                lesson.SubGroup = Regex.Replace(subgroup, @"\s+", " ").Trim();
                            _.Lessons.Add(lesson);
                            lessCounter += 1;
                            if (lessCounter >= scheduleLines.Count)
                            {
                                counter += 1;
                                break;
                            }
                        }
                    }
                    week.Days.Add(_);
                }
                timeTable.Weeks.Add(week);
            }
            return timeTable;
        }


        


    }
}
