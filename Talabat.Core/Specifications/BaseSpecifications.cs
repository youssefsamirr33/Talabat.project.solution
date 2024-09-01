using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
	public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>>? Criteria { get; set; } = null!;
		public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderByAsync { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set ; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsEnablePagination { get; set; }

        public BaseSpecifications() // When getall criteria set null 
        {
            
        }

        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
        }

        private protected void AddOrderByAsync(Expression<Func<T, object>> expression)
        {
            OrderByAsync = expression;
        }

        private protected void AddOrderByDesc(Expression<Func<T, object>> expression)
        {
            OrderByDesc = expression;
        }

        private protected void ApllyPagination(int skip , int take)
        {
            IsEnablePagination = true;
            Skip = skip;
            Take = take;
        }
    }
}
