using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Helpers
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSecond;

        public CacheAttribute(int timeToLiveInSecond)
        {
            _timeToLiveInSecond = timeToLiveInSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // cacheResponseService ---> Ask CLR for Creating object from class CacheResponseService by [DI] Exeplicitly
            var ResponsecacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GenirateCacheKeyFromRequest(context.HttpContext.Request);

            var response = await ResponsecacheService.GetCacheResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(response))
            {
                var result = new ContentResult()
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = result;
                return;
            }

            var ActionExecutingContext = await next.Invoke();

            if(ActionExecutingContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
            {

                await ResponsecacheService.CacheResponseAsync(cacheKey, okObjectResult.Value , TimeSpan.FromSeconds(_timeToLiveInSecond));
            }

        }

        private string GenirateCacheKeyFromRequest(HttpRequest request)
        {
            // /api/products?PageIndex=1,PageSize=5,Search=name
            // /api/products --> path for request 
            // PageIndex=1,PageSize=5,Search=name --> Query string [Key value pair] Dictionary

            var stringCacheKey = new StringBuilder();

            stringCacheKey.Append(request.Path); // /api/products

            foreach (var (key, value) in request.Query)
            {
                stringCacheKey.Append($"|{key}-{value}");
                // /api/products|PageIndex-1
                // /api/products|PageIndex-1|PageSize-5
                // /api/products|PageIndex-1|PageSize-5|Search-name
            }

            return stringCacheKey.ToString();
        }
    }
}
