﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Repositories.Contract
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response , TimeSpan timeToLive);

        Task<string?> GetCacheResponseAsync(string cacheKey);    
    }
}
