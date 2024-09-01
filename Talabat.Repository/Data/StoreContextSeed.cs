using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;
using Talabat.Repository.ApplicationContext;

namespace Talabat.Repository.Data
{
	public static class StoreContextSeed
	{
		public async static Task SeedAsync(StoreDbContext _context)
		{
			if (_context.ProductBrands.Count() == 0)
			{
				// 1- read data from json file 
				var brandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
				// 2- convert from json to list<productBrand>
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
				if (brands is not null)
				{
					foreach (var brand in brands)
					{
						await _context.Set<ProductBrand>().AddAsync(brand);
					}
					await _context.SaveChangesAsync();

				} 
			}
			if(_context.ProductCategories.Count() == 0)
			{
				var categoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
				var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);
				if(categories is not null)
				{
					foreach(var category in categories)
						await _context.Set<ProductCategory>().AddAsync(category);
					await _context.SaveChangesAsync();
				}
			}
			if (_context.Products.Count() == 0)
			{
				var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productsData);
				if (products is not null)
				{
					foreach (var product in products)
						await _context.Set<Product>().AddAsync(product);
					await _context.SaveChangesAsync();
				}
			}

			if(_context.DeliveryMethods.Count() == 0)
			{
				var deliveryMethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");

				var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodData);

				if(deliveryMethods is not null)
				{
					foreach(var delivery in deliveryMethods)
						await _context.Set<DeliveryMethod>().AddAsync(delivery);

					await _context.SaveChangesAsync();
				}
			}
		}
	}
}
