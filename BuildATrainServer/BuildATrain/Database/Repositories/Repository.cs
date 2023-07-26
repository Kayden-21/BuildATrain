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

        public async Task<Attributes> GetAttributesById(int id)
        {
            var idParameter = new SqlParameter("@Id", SqlDbType.Int) { Value = id };

            var entities = await _context.Set<T>()
                .FromSqlRaw("EXEC GetAttributesById @Id", idParameter)
                .ToListAsync();

            return entities.FirstOrDefault();
        }

        public async Task<TrainModel?> GetTrainByUsernameAndLocomotiveNameAsync(string username, string locomotiveName)
        {
            return await _entitySet
            .OfType<TrainModel>()
                .FirstOrDefaultAsync(t => t.Username == username && t.LocomotiveName == locomotiveName);
        }

        public async Task InsertPlayerTrainAsync(string locomotiveSize, int numFuelCars, int numPassengerCars, int numCargoCars, string username)
        {
            var locomotiveSizeParam = new SqlParameter("@LocomotiveSize", SqlDbType.VarChar) { Value = locomotiveSize };
            var numFuelCarsParam = new SqlParameter("@NumFuelCars", SqlDbType.Int) { Value = numFuelCars };
            var numPassengerCarsParam = new SqlParameter("@NumPassengerCars", SqlDbType.Int) { Value = numPassengerCars };
            var numCargoCarsParam = new SqlParameter("@NumCargoCars", SqlDbType.Int) { Value = numCargoCars };
            var usernameParam = new SqlParameter("@Username", SqlDbType.VarChar) { Value = username };

            await _context.Database.ExecuteSqlRawAsync("EXEC InsertPlayerTrain @LocomotiveSize, @NumFuelCars, @NumPassengerCars, @NumCargoCars, @Username",
                locomotiveSizeParam, numFuelCarsParam, numPassengerCarsParam, numCargoCarsParam, usernameParam);
        }
    }
}
