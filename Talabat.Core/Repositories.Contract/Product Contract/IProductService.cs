using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications.Product_Specification;

namespace Talabat.Core.Repositories.Contract.Product_Contract
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync(ProductSpecPrams prams);

        Task<int> GetCountAsync(ProductSpecPrams prams);

        Task<Product?> GetProductAsync(int productId);

        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();
    }
}
