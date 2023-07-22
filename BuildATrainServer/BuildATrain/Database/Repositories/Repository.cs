using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

        public async Task<T> GetAttributesById(int id)
        {
            var idParameter = new SqlParameter("@Id", SqlDbType.Int) { Value = id };

            var entities = await _context.Set<T>()
                .FromSqlRaw("EXEC GetAttributesById @Id", idParameter)
                .ToListAsync();

            return entities.FirstOrDefault();
        }
    }
}
