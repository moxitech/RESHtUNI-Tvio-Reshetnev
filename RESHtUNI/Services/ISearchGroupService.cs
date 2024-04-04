using Domain.WebTypes;

namespace RESHtUNI.Services
{
    public interface ISearchGroupService
    {
        Task<List<GroupResponse>?> GetFullGroupNameFromServer(string name);
    }
}