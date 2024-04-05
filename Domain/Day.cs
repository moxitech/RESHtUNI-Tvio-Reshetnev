using SQLite;
using SQLiteNetExtensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Day
    {
        public string DayName { get; set; }

        public Week Week { get; set; }

        public List<Lesson> Lessons { get; set; }
    }
}
