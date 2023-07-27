using BuildATrain.Common;
using BuildATrain.Database.Models;
using BuildATrain.Models.Game;
using System.Diagnostics;

namespace BuildATrain.Database.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<TrainModel>> GetPlayerTrainsByEmailAsync(string email);
        Task InsertPlayerTrainAsync(string locomotiveSize, string locomotiveName, int numFuelCars, int numPassengerCars, int numCargoCars, string username);
        Task<Attributes> GetAttributesByAttributeIdAsync(string attributeId);
        Task<bool> UpdateCarCountAsync(int trainId, CarType carType, int count);
    }
}
