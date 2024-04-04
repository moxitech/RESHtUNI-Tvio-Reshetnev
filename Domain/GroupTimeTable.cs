using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class GroupTimeTable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Semester { get; set; }
        public string UniversityId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public List<Week> Weeks { get; set; }

        public Week GetWeek(int n)
        {
            
            return Weeks[n - 1];

        }
    }
}
