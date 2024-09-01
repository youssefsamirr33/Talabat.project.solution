using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
	public interface IGenaricRepository<T> where T : BaseEntity
	{
		Task<IReadOnlyList<T>> GetAsync();
		Task<T?> GetByIdAsync(int id);

		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
		Task<T?> GetWithSpecAsync(ISpecifications<T> spec);

		Task<int> GetCountWithSpecAsync(ISpecifications<T> spec);

		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}
