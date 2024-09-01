using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
	internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery , ISpecifications<TEntity> spec)
		{
			var query = InputQuery;  // _dbcontext.set<T>()
			if (spec.Criteria is not null)
				query = query.Where(spec.Criteria);

			if (spec.OrderByAsync is not null)
				query = query.OrderBy(spec.OrderByAsync);
			else if (spec.OrderByDesc is not null)
				query = query.OrderByDescending(spec.OrderByDesc);


			if (spec.IsEnablePagination)
				query = query.Skip(spec.Skip).Take(spec.Take);

			// includes 
			//1- include 1
			//2- include 2
			// ....

			query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
					// _dbcontext.set<T>().include(P => P.category)
					// _dbcontext.set<T>().include(P => P.category).include(p => p.brand)
			return query;
		}
	}
}
