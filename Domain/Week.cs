using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Week
    {
        [Key]
        public int Id { get; set; }
        public byte Number { get; set; }
        public List<Day> Days { get; set; }
    }
}