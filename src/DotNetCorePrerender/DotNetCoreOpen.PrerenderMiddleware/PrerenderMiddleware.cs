using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Extensions;
using System.Threading;

namespace DotNetCoreOpen.PrerenderMiddleware
{
    /// <summary>
    /// .NET Core Middleware for prerendering JavaScript logic for crawlers.
    /// </summary>
    public class PrerenderMiddleware
    {
        #region Static ReadOnly
        const string DefaultIgnoredExtensions = "\\.(vxml|js|css|less|png|jpg|jpeg|gif|pdf|doc|txt|zip|mp3|rar|exe|wmv|doc|avi|ppt|mpg|mpeg|tif|wav|mov|psd|ai|xls|mp4|m4a|swf|dat|dmg|iso|flv|m4v|torrent)$";
        static readonly Encoding DefaultEncoding = Encoding.UTF8;
        #endregion

        #region Fields
        protected readonly RequestDelegate _next = null;
        #endregion

        #region Ctor
        public PrerenderMiddleware(RequestDelegate next, PrerenderConfiguration configuration)
        {
            _next = next;
            Configuration = configuration;
        }
        #endregion

        #region Properties
        public PrerenderConfiguration Configuration { get; private set; }
        #endregion

        #region Invoke
        public async Task Invoke(HttpContext httpContext)
        {
            await Prerender(httpContext);
        }
        #endregion
        
        #region Prerender
        /// <summary>
        /// Prerender logic
        /// </summary>
        /// <param name="context"></param>
        private async Task Prerender(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var response = httpContext.Response;
            var requestFeature = httpContext.Features.Get<IHttpRequestFeature>();
            
            if (IsValidForPrerenderPage(request, requestFeature))
            {
                // generate URL
                var requestUrl = request.GetDisplayUrl();
                // if traffic is forwarded from https://, we convert http:// to https://.
                if (string.Equals(request.Headers[Constants.HttpHeader_XForwardedProto], Constants.Https, StringComparison.OrdinalIgnoreCase)
                 && requestUrl.StartsWith(Constants.HttpProtocol, StringComparison.OrdinalIgnoreCase))
                {
                    requestUrl = Constants.HttpsProtocol + requestUrl.Substring(Constants.HttpProtocol.Length);
                }
                var prerenderUrl = $"{Configuration.ServiceUrl.Trim('/')}/{requestUrl}";

                // use HttpClient instead of HttpWebRequest, as HttpClient has AllowAutoRedirect option.
                var httpClientHandler = new HttpClientHandler() { AllowAutoRedirect = true };
                // Proxy Information
                if (!string.IsNullOrEmpty(Configuration.ProxyUrl) && Configuration.ProxyPort > 0)
                    httpClientHandler.Proxy = new WebProxy(Configuration.ProxyUrl, Configuration.ProxyPort);

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(60);
                    httpClient.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { NoCache = true };
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(Constants.HttpHeader_ContentType, "text/html");
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(Constants.HttpHeader_UserAgent, request.Headers[Constants.HttpHeader_UserAgent].ToString());

                    if (!string.IsNullOrEmpty(Configuration.Token))
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(Constants.HttpHeader_XPrerenderToken, Configuration.Token);

                    using (var webMessage = await httpClient.GetAsync(prerenderUrl))
                    {
                        var text = default(string);
                        try
                        {
                            response.StatusCode = (int)webMessage.StatusCode;
                            foreach (var keyValue in webMessage.Headers)
                            {
                                response.Headers[keyValue.Key] = new StringValues(keyValue.Value.ToArray());
                            }

                            using (var stream = await webMessage.Content.ReadAsStreamAsync())
                            using (var reader = new StreamReader(stream))
                            {
                                webMessage.EnsureSuccessStatusCode();
                                text = reader.ReadToEnd();
                            }
                        }
                        catch (Exception e)
                        {
                            text = e.Message;
                        }
                        await response.WriteAsync(text);
                    }
                }
            }
            else
            {
                await _next.Invoke(httpContext);
            }
        }
        
        private bool IsValidForPrerenderPage(HttpRequest request, IHttpRequestFeature requestFeature)
        {
            var userAgent = request.Headers[Constants.HttpHeader_UserAgent];
            var rawUrl = requestFeature.RawTarget;
            var relativeUrl = request.Path.ToString();
            
            // check if follows google search engine suggestion
            if (request.Query.Keys.Any(a => a.Equals(Constants.EscapedFragment, StringComparison.OrdinalIgnoreCase)))
                return true;

            // check if has user agent
            if (string.IsNullOrEmpty(userAgent))
                return false;

            // check if it's crawler user agent.
            var crawlerUserAgentPattern = string.IsNullOrEmpty(Configuration.CrawlerUserAgentPattern) ? Constants.CrawlerUserAgentPattern : Configuration.CrawlerUserAgentPattern;
            if (string.IsNullOrEmpty(crawlerUserAgentPattern)
             || !Regex.IsMatch(userAgent, crawlerUserAgentPattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase))
                return false;

            // check if the extenion matchs default extension
            if (Regex.IsMatch(relativeUrl, DefaultIgnoredExtensions, RegexOptions.IgnorePatternWhitespace))
                return false;

            if (!string.IsNullOrEmpty(Configuration.AdditionalExtensionPattern) && Regex.IsMatch(relativeUrl, Configuration.AdditionalExtensionPattern, RegexOptions.IgnorePatternWhitespace))
                return false;

            if (!string.IsNullOrEmpty(Configuration.BlackListPattern)
              && Regex.IsMatch(rawUrl, Configuration.BlackListPattern, RegexOptions.IgnorePatternWhitespace))
                return false;

            if (!string.IsNullOrEmpty(Configuration.WhiteListPattern)
              && !Regex.IsMatch(rawUrl, Configuration.WhiteListPattern, RegexOptions.IgnorePatternWhitespace))
                return false;

            return true;

        }
        #endregion
    }
}
