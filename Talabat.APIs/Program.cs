using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Repositories.Contract.Auth.contract;
using Talabat.Core.Repositories.Contract.Order_contract;
using Talabat.Core.Repositories.Contract.Payment_Contract;
using Talabat.Core.Repositories.Contract.Product_Contract;
using Talabat.Repository;
using Talabat.Repository._Identity;
using Talabat.Repository._Identity.DataSeed;
using Talabat.Repository.ApplicationContext;
using Talabat.Repository.Basket_Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Genaric.Repository;
using Talabat.Services.Auth;
using Talabat.Services.Cache_service;
using Talabat.Services.Order.Services;
using Talabat.Services.Payment_Service;
using Talabat.Services.Product_Service;

namespace Talabat.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Configuration Services
			// Add services to the container.

			builder.Services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			});
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<StoreDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefualtConnection"));
			});

			builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
			});

			// Allow DI for Redis 
			builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
			{
				var connection = builder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});

			builder.Services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheService));
			builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
			builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));
			builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
			builder.Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
			//builder.Services.AddScoped(typeof(IGenaricRepository<>) ,typeof(GenraicRepository<>));
			builder.Services.AddAutoMapper(typeof(MappingProfile));
			builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
			builder.Services.AddScoped(typeof(IAuthService) , typeof(AuthService));

			// Allow DI for 3 main servecies for Identity and allow DI for IUserStore -- CraeteUser
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
							.AddEntityFrameworkStores<ApplicationIdentityDbContext>();

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				// Authentication handler [validate token for jwt]
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = builder.Configuration["JWT:Validissuer"],
					ValidateAudience = true ,
					ValidAudience = builder.Configuration["JWT:Aud"],
					ValidateIssuerSigningKey = true ,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"])),
					ValidateLifetime = true ,
					ClockSkew = TimeSpan.Zero
                };
			});

			// generation response For validation errors [Factory]
			builder.Services.Configure<ApiBehaviorOptions>(Options =>
			{
				Options.InvalidModelStateResponseFactory = (ActionContext) =>
				{
					var errors = ActionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
														 .SelectMany(p => p.Value.Errors)
														 .Select(E => E.ErrorMessage)
														 .ToList();
					var response = new ApiValidationResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(response);
				};
			});

			#endregion

			var app = builder.Build();

			using var scope = app.Services.CreateScope(); // With life time scoped
			var services = scope.ServiceProvider;
			var _dbcontext = services.GetRequiredService<StoreDbContext>(); // ASk ClR For Creating object Exeplicitly
			var _IdentityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();
			var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await _dbcontext.Database.MigrateAsync();
				await StoreContextSeed.SeedAsync(_dbcontext);

				await _IdentityDbContext.Database.MigrateAsync();
				await ApplicationIdentityContextSeed.UserIdentitySeed(userManager);
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "an error has been occured during Apply the Migration");
				
			}

			#region Configure

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseMiddleware<ExceptionMiddleWare>();

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseStaticFiles();

			app.MapControllers(); 
			#endregion

			app.Run();
		}
	}
}
