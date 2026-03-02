using LocaCraftAPI.Models;

namespace LocaCraftAPI.Services
{
    public interface IInseeService
    {
        public Task<List<InseeIndexModel>> GetIndexAsync(int lastNIndex = 20);
    }
}
