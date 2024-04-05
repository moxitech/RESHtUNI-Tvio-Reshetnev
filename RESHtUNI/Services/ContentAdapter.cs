using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESHtUNI.Services
{
    public class ContentAdapter
    {

        /// <summary>
        /// Обращаемся к базе, и смотрим сохраненные данные
        /// </summary>
        public async Task<GroupTimeTable?> TryGetTimetableFromDB(string groupName)
        {
            return null;
        }

        /// <summary>
        /// Обращаемся к сети, в случае отсутствия данных или очистки базы
        /// </summary>
        public void TryGetOnlineData() 
        {
        
        }


    }
}
