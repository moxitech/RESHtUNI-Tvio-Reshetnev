using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Day
    {
        [Key]
        public int Id { get; set; }
        public string DayName { get; set; }
        public List<Lesson> Lessons { get;set; }
    }
}