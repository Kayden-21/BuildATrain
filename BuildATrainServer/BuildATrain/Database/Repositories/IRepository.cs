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
        Task<bool> InsertPlayerTrainAsync(string locomotiveSize, int locomotiveType, string locomotiveName, int numFuelCars, int numPassengerCars, int numCargoCars, string email);
        Task<Attributes> GetAttributesByAttributeIdAsync(int attributeId);
        Task<bool> UpdateCarCountAsync(int trainId, CarType carType, int count, string email);
        Task<bool> PreformPurchase(string email, int attributeId, decimal currentWallet);
    }
}
