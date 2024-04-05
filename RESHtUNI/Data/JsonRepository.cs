using Domain;
using Domain.TransferTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESHtUNI.Data
{
    public class JsonRepository
    {
        private readonly string filepath = Path.Combine(FileSystem.AppDataDirectory, "Timetables.json");
        
        async Task Init()
        {
            if (!File.Exists(filepath))
            {
                var f = File.Create(filepath);
                f.Close();
            }
        }

        /// <summary>
        /// Возвращает все закешированные группы
        /// </summary>
        /// <returns></returns>
        public async Task<TransferGroup> GetGroups()
        {
            await Init();
            string json = await File.ReadAllTextAsync(filepath);
            var table = JsonConvert.DeserializeObject<GroupTimeTable>(json) ?? new GroupTimeTable();
            return new TransferGroup() { Id = table.UniversityId, Name = table.Name };
        }

        public async Task<GroupTimeTable?> GetGroupTimetableAsync(string GroupName)
        {
            await Init();
            string json = await File.ReadAllTextAsync(filepath);
            var table = JsonConvert.DeserializeObject<GroupTimeTable>(json);

            if (table.Name == GroupName)
            {
                return table;
            }
            
            return null;
        }

        public async Task<int> SaveTimeTableAsync(GroupTimeTable timetable)
        {
            await Init();
            string json = JsonConvert.SerializeObject(timetable, Formatting.Indented);
            File.WriteAllText(filepath, json);
            return 0;
        }
        
        public async Task<int> DeleteTimeTableAsync()
        {
            await Init();
            return 0;
            
        }
    }
}
