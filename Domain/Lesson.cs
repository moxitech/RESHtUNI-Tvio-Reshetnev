using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }
        public string Time { get; set; }
        public string Name { get; set; }
        public string? SubGroup { get; set; }
        public string Teacher { get; set; }
        public string Auditorium { get; set; }
    }
}