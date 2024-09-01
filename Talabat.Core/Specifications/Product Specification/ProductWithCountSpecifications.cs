using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specification
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product>
    {
        public ProductWithCountSpecifications(ProductSpecPrams prams) 
            :base(p => 
                 (!prams.brandId.HasValue || p.BrandId == prams.brandId)
                 &&
                (!prams.categoryId.HasValue || p.CategoryId == prams.categoryId)
                 &&
                  (string.IsNullOrEmpty(prams.Search) || p.Name.ToLower().Contains(prams.Search))
                 )
        {
            
        }

    }
}
