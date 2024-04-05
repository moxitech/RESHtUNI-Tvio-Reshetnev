using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TransferTypes
{
    public class TransferGroup
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return $"timetable/{Name}/{Id}/false";
        }
    }
}
