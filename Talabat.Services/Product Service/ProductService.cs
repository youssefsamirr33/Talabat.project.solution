using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract.Product_Contract;
using Talabat.Core.Specifications.Product_Specification;

namespace Talabat.Services.Product_Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Product>> GetAllProductsAsync(ProductSpecPrams prams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(prams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            return products;
        }

        public async Task<int> GetCountAsync(ProductSpecPrams prams)
        {
            var countSpec = new ProductWithCountSpecifications(prams);
            var count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec); // spec with filteration only 
            return count;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
            => await _unitOfWork.Repository<ProductBrand>().GetAsync();

        public Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
            => _unitOfWork.Repository<ProductCategory>().GetAsync();


        public async Task<Product?> GetProductAsync(int productId)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(productId);
            var product = await _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);
            return product;
        }
    }
}
