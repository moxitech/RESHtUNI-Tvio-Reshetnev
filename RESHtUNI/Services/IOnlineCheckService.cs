
namespace RESHtUNI.Services
{
    public interface IOnlineCheckService
    {
        Task<bool> CheckOnlineStatus();
    }
}