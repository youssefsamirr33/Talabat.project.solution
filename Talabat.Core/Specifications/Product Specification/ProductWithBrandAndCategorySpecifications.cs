using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specification
{
	public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
	{
        public ProductWithBrandAndCategorySpecifications(ProductSpecPrams prams) 
            : base(p =>
                  (!prams.brandId.HasValue || p.BrandId == prams.brandId ) 
                  &&
                  (!prams.categoryId.HasValue || p.CategoryId == prams.categoryId)
                  &&
                  (string.IsNullOrEmpty(prams.Search) || p.Name.ToLower().Contains(prams.Search) )
                  )
		{
			AddIncludes();

            // sort 
            if (!string.IsNullOrEmpty(prams.Sort))
            {
                // sort 
                switch (prams.Sort)
                {
                    case "priceAsync":
                        AddOrderByAsync(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderByAsync(p => p.Name);
                        break;

                }
            }
            else
                AddOrderByAsync(P => P.Name);


            // products => 100
            // pageSize => 10
            // pageIndex => 5

            // skip(40).take(10)
            // skip(40)  ===> 10 * (5-1) = 40 --> pageSize * (pageIndex -1)

            ApllyPagination(prams.PageSize * (prams.PageIndex -1) , prams.PageSize);

        }

		public ProductWithBrandAndCategorySpecifications(int id)
            : base(P =>P.Id == id) 
        {
			AddIncludes();
		}

		private protected void AddIncludes()
		{
			Includes.Add(p => p.Brand);
			Includes.Add(p => p.Category);
		}
	}
}
