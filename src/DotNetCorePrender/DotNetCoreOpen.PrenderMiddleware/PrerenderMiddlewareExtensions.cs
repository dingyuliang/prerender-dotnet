using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DotNetCoreOpen.PrenderMiddleware
{
    public static class PrerenderMiddlewareExtensions
    {
        #region UsePrerender
        /// <summary>
        /// Use Prerender Middleware to prerender JavaScript logic before turn back.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration">Prerender Configuration, if this parameter is NULL, will get the PrerenderConfiguration from ServiceCollection</param>
        /// <returns></returns>
        public static IApplicationBuilder UsePrerender(this IApplicationBuilder app, PrerenderConfiguration configuration = null)
            => app.UseMiddleware<PrerenderMiddleware>(configuration ?? app.ApplicationServices.GetService<IOptions<PrerenderConfiguration>>().Value);
         // => app.Use(next => new PrerenderMiddleware(next, configuration).Invoke);
         // => app.Use(next => context => new PrerenderMiddleware(next, configuration).Invoke(context));  // either way.        
        #endregion

        #region Configuration
        /// <summary>
        /// Add PrerenderConfiguration.json to configuration.
        /// Or you can put the configuration in appsettings.json file either.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="jsonFileName"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddPrerenderConfig(this IConfigurationBuilder builder, string jsonFileName = "PrerenderConfiguration.json")
         => builder.AddJsonFile(jsonFileName, false, true);

        /// <summary>
        /// Configure Section into Service Collections
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <param name="singletonOptions"></param>
        public static void ConfigureSection<TOptions>(this IServiceCollection serviceCollection, IConfiguration configuration, bool singletonOptions = true)
            where TOptions : class, new()
        {
            serviceCollection.Configure<TOptions>(configuration.GetSection(typeof(TOptions).Name));

            if (singletonOptions)
            {
                serviceCollection.AddSingleton<TOptions>(a => a.GetService<IOptions<TOptions>>().Value);
            }
        }
        #endregion        
    }
}
