using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.ApplicationContext;

namespace Talabat.Repository.Genaric.Repository
{
	public class GenraicRepository<T> : IGenaricRepository<T> where T : BaseEntity
	{
		private readonly StoreDbContext _context;

		public GenraicRepository(StoreDbContext context)
        {
			_context = context;
		}

	

		public async Task<IReadOnlyList<T>> GetAsync()
		{
			return await _context.Set<T>().AsNoTracking().ToListAsync();
		}
			

		public async Task<T?> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}


		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).AsNoTracking().ToListAsync();
		}

		public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).FirstOrDefaultAsync();
		}


		public async Task<int> GetCountWithSpecAsync(ISpecifications<T> spec)
			=> await ApplySpecifications(spec).CountAsync();

        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
		{
			return SpecificationsEvaluator<T>.GetQuery(_context.Set<T>(), spec);
		}

        public void Add(T entity)
			=> _context.Set<T>().Add(entity);

        public void Update(T entity)
			=> _context.Set<T>().Update(entity);

        public void Delete(T entity)
			=> _context.Set<T>().Remove(entity);
    }
}
