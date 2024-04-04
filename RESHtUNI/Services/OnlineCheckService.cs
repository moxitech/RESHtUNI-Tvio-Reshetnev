using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RESHtUNI.Services
{
    public class OnlineCheckService : IOnlineCheckService
    {
        public async Task<bool> CheckOnlineStatus()
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync("www.google.com");
                return (reply.Status == IPStatus.Success);
            }
            catch (PingException)
            {
                return false;
            }
        }
    }
}
