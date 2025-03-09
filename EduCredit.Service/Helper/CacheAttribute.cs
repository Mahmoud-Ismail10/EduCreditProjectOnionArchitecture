using EduCredit.Service.Services;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeForSecond;

        public CacheAttribute(int ExpireTimeForSecond)
        {
            _expireTimeForSecond = ExpireTimeForSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheKey = GenerateCacheKey(context.HttpContext.Request);
            var CacheResponse = await cacheService.GetCashAsync(cacheKey);

            if (CacheResponse != null)
            {
                var contentResult = new ContentResult
                {
                    Content = CacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var ExecuteEndpointContext = await next.Invoke();
            if (ExecuteEndpointContext.Result is OkObjectResult result)
            {
                await cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromSeconds(_expireTimeForSecond));
            }

        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
