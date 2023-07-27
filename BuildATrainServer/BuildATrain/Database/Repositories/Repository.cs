using BuildATrain.Common;
using BuildATrain.Database.Models;
using BuildATrain.Models.Game;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<bool> InsertPlayerTrainAsync(string locomotiveSize, int locomotiveType, string locomotiveName, int numFuelCars, int numPassengerCars, int numCargoCars, string email)
        {
            var currentWallet = GetCurrentWalletByEmail(email).Result.CurrentWallet;

            bool isSuccess = await PreformPurchase(email, locomotiveType, currentWallet);

            if (isSuccess)
            {
                var locomotiveSizeParam = new SqlParameter("@LocomotiveSize", SqlDbType.VarChar) { Value = locomotiveSize };
                var locomotiveTypeParam = new SqlParameter("@LocomotiveTypeId", SqlDbType.Int) { Value = locomotiveType };
                var locomotiveNameParam = new SqlParameter("@LocomotiveName", SqlDbType.VarChar) { Value = locomotiveName };
                var numFuelCarsParam = new SqlParameter("@NumFuelCars", SqlDbType.Int) { Value = numFuelCars };
                var numPassengerCarsParam = new SqlParameter("@NumPassengerCars", SqlDbType.Int) { Value = numPassengerCars };
                var numCargoCarsParam = new SqlParameter("@NumCargoCars", SqlDbType.Int) { Value = numCargoCars };
                var emailParam = new SqlParameter("@Email", SqlDbType.VarChar) { Value = email };

                await _context.Database.ExecuteSqlRawAsync("EXEC InsertPlayerTrain @LocomotiveSize, @LocomotiveTypeId, @LocomotiveName, @NumFuelCars, @NumPassengerCars, @NumCargoCars, @Email",
                    locomotiveSizeParam, locomotiveTypeParam, locomotiveNameParam, numFuelCarsParam, numPassengerCarsParam, numCargoCarsParam, emailParam);

                return true;
            }

            return false;
        }

        public async Task<Attributes> GetAttributesByAttributeIdAsync(int attributeId)
        {
            var attributeIdParam = new SqlParameter("@AttributeId", SqlDbType.Int) { Value = attributeId };
            var result = await _context.Set<Attributes>()
                .FromSqlRaw("EXEC GetAttributesById @AttributeId", attributeIdParam)
                .FirstAsync();

            return result;
        }

        public async Task<bool> UpdateCarCountAsync(int trainId, CarType carType, int count, string email)
        {
            var currentWallet = GetCurrentWalletByEmail(email).Result.CurrentWallet;

            bool isSuccess = await PreformPurchase(email, (int)carType, currentWallet);

            if (isSuccess)
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
            }
                return true;
        }

        public async Task<WalletModel> GetCurrentWalletByEmail(string email)
        {
            var emailParam = new SqlParameter("@Email", SqlDbType.NVarChar) { Value = email };
            var result = await _context.Set<WalletModel>()
                .FromSqlRaw("EXEC GetCurrentWalletByEmail @Email", emailParam)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<bool> PreformPurchase(string email, int attributeId, decimal currentWallet)
        {
            var emailParam = new SqlParameter("@Email", SqlDbType.VarChar) { Value = email };
            var attributeIdParam = new SqlParameter("@AttributeId", SqlDbType.Int) { Value = attributeId };
            var currentWalletParam = new SqlParameter("@CurrentWallet", SqlDbType.Decimal) { Value = currentWallet };
            currentWalletParam.Precision = 18;
            currentWalletParam.Scale = 2;

            var successParam = new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

            await _context.Database.ExecuteSqlRawAsync("EXEC PerformPurchaseByAttributeId @Email, @AttributeId, @CurrentWallet OUTPUT, @Success OUTPUT, @Message OUTPUT",
                emailParam, attributeIdParam, currentWalletParam, successParam, messageParam);

            var isSuccess = Convert.ToBoolean(successParam.Value);

            return isSuccess;
        }
    }
}
