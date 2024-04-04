using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESHtUNI.Services
{
    internal interface ITimeTableService
    {
        public Task<GroupTimeTable> GetTimeTableByGroup(string groupName, string id);

    }
}
