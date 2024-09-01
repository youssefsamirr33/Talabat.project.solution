using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Repositories.Contract.Product_Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specification;
using Talabat.Services.Cache_service;

namespace Talabat.APIs.Controllers
{
	public class ProductsController : BaseApiController
	{
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        

        public ProductsController(IProductService productService ,
			IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

		[CacheAttribute(600)]  // Action Filter 
		// EndPoint : GET : /api/products
		[HttpGet]
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetAllProducts([FromQuery]ProductSpecPrams prams)
		{
			var products = await _productService.GetAllProductsAsync(prams);
			var ProductMapped = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
			var count = await _productService.GetCountAsync(prams);
            return Ok(new Pagination<ProductToReturnDto>(prams.PageSize , prams.PageIndex , count , ProductMapped));
		}

		// GET : /api/Products/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var product = await _productService.GetProductAsync(id);
			if (product is null)
				return NotFound(new ApiResponse(404));
			return Ok(_mapper.Map<Product , ProductToReturnDto>(product));
		}

		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
		{
			var brand = await _productService.GetBrandsAsync();
			if (brand is null)
				return BadRequest(new ApiResponse(400));
			return Ok(brand);
		}

		[HttpGet("categories")]
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetAllCategories()
		{
			var categories = await _productService.GetCategoriesAsync();
			if (categories is null)
				return BadRequest(new ApiResponse(400));
			return Ok(categories);
		}
    }
}
