using BuildATrain.Common;
using BuildATrain.Database.Models;
using BuildATrain.Models.Game;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace BuildATrain.Database.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _entitySet;

        public Repository(DbContext context)
        {
            _context = context;
            _entitySet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entitySet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _entitySet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _entitySet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _entitySet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TrainModel>> GetPlayerTrainsByEmailAsync(string email)
        {
            var emailParam = new SqlParameter("@Email", SqlDbType.NVarChar) { Value = email };
            var result = await _context.Set<TrainModel>()
                .FromSqlRaw("EXEC GetPlayerTrainsByEmail @Email", emailParam)
                .ToListAsync();

            return result;
        }

        public async Task InsertPlayerTrainAsync(string locomotiveSize, string locomotiveName, int numFuelCars, int numPassengerCars, int numCargoCars, string username)
        {
            var locomotiveSizeParam = new SqlParameter("@LocomotiveSize", SqlDbType.VarChar) { Value = locomotiveSize };
            var locomotiveNameParam = new SqlParameter("@LocomotiveName", SqlDbType.VarChar) { Value = locomotiveName };
            var numFuelCarsParam = new SqlParameter("@NumFuelCars", SqlDbType.Int) { Value = numFuelCars };
            var numPassengerCarsParam = new SqlParameter("@NumPassengerCars", SqlDbType.Int) { Value = numPassengerCars };
            var numCargoCarsParam = new SqlParameter("@NumCargoCars", SqlDbType.Int) { Value = numCargoCars };
            var usernameParam = new SqlParameter("@Username", SqlDbType.VarChar) { Value = username };

            await _context.Database.ExecuteSqlRawAsync("EXEC InsertPlayerTrain @LocomotiveSize, @LocomotiveName, @NumFuelCars, @NumPassengerCars, @NumCargoCars, @Username",
                locomotiveSizeParam, locomotiveNameParam, numFuelCarsParam, numPassengerCarsParam, numCargoCarsParam, usernameParam);
        }

        public async Task<Attributes> GetAttributesByAttributeIdAsync(string attributeId)
        {
            var attributeIdParam = new SqlParameter("@AttributeId", SqlDbType.NVarChar) { Value = attributeId };
            var result = await _context.Set<Attributes>()
                .FromSqlRaw("EXEC GetAttributesById @AttributeId", attributeIdParam)
                .FirstAsync();

            return result;
        }

        public async Task<bool> UpdateCarCountAsync(int trainId, CarType carType, int count)
        {
            var train = await _entitySet
                .OfType<TrainModel>()
                .FirstOrDefaultAsync(t => t.TrainId == trainId);

            if (train == null)
            {
                return false;
            }

            switch (carType)
            {
                case CarType.Passenger:
                    train.NumPassengerCars += count;
                    break;
                case CarType.Cargo:
                    train.NumCargoCars += count;
                    break;
                case CarType.Fuel:
                    train.NumFuelCars += count;
                    break;
                default:
                    return false;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
