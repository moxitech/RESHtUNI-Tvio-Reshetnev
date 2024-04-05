using SQLite;
using SQLiteNetExtensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Week
    {
        public byte Number { get; set; }
        public List<Day> Days { get; set; }
    }
}