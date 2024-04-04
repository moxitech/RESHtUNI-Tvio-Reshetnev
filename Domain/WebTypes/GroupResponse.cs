using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.WebTypes
{
    /// <summary>
    /// Ответ от сервера групп
    /// </summary>
    public class GroupResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"Группа: {Name} Id: {Id}";
        }

    }
}
