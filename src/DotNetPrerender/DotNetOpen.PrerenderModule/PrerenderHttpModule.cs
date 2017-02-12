using DotNetOpen.PrerenderModule.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DotNetOpen.PrerenderModule
{
    /// <summary>
    /// Prerender Http Module: prerender HTML for crawlers.
    /// </summary>
    public class PrerenderHttpModule : IHttpModule
    {
        #region Static ReadOnly
        static string DefaultIgnoredExtensions = "\\.vxml|js|css|less|png|jpg|jpeg|gif|pdf|doc|txt|zip|mp3|rar|exe|wmv|doc|avi|ppt|mpg|mpeg|tif|wav|mov|psd|ai|xls|mp4|m4a|swf|dat|dmg|iso|flv|m4v|torrent";
        static readonly PrerenderConfigurationSection Configuration = PrerenderConfigurationSection.GetSection();
        static readonly Encoding DefaultEncoding = Encoding.UTF8; 
        #endregion

        #region Fields
        HttpApplication context;
        #endregion

        #region Implement IHttpModule
        /// <summary>
        /// init
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            this.context = context; 
            context.BeginRequest += context_BeginRequest;
        }

        /// <summary>
        /// dispose
        /// </summary>
        public void Dispose()
        {
        }
        #endregion

        #region Begin Request
        protected void context_BeginRequest(object sender, EventArgs e)
        {
            try
            {
                Prerender(context);
            }
            catch (Exception exception)
            {               
                Debug.Write(exception.ToString());
            }
        }
        #endregion

        #region Prerender
        /// <summary>
        /// Prerender logic
        /// </summary>
        /// <param name="context"></param>
        private void Prerender(HttpApplication context)
        {
            var httpContext = context.Context;
            var request = httpContext.Request;
            var response = httpContext.Response;
            if (ShouldPrerenderPage(request))
            {
                // generate URL
                var requestUrl = request.Url.AbsoluteUri;
                // if traffic is forwarded from https://, we convert http:// to https://.
                if (string.Equals(request.Headers[Constants.HttpHeader_XForwardedProto], Constants.HttpsProtocol, StringComparison.InvariantCultureIgnoreCase)
                 && requestUrl.StartsWith(Constants.HttpProtocol, StringComparison.InvariantCultureIgnoreCase))
                {
                    requestUrl = Constants.HttpsProtocol + requestUrl.Substring(Constants.HttpProtocol.Length);
                }
                var prerenderUrl = $"{Configuration.ServiceUrl.Trim('/')}/{requestUrl}";

                // create request
                var webRequest = (HttpWebRequest)WebRequest.Create(prerenderUrl);
                webRequest.Method = "GET";
                webRequest.UserAgent = request.UserAgent;
                webRequest.AllowAutoRedirect = false;
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.ContentType = "text/html";

                // Proxy Information
                if (!string.IsNullOrEmpty(Configuration.ProxyUrl) && Configuration.ProxyPort > 0)
                    webRequest.Proxy = new WebProxy(Configuration.ProxyUrl, Configuration.ProxyPort);

                if (!string.IsNullOrEmpty(Configuration.Token))
                    webRequest.Headers.Add(Constants.HttpHeader_XPrerenderToken, Configuration.Token);

                try
                {
                    // Get the web response and read content etc. if successful
                    var webResponse = (HttpWebResponse)webRequest.GetResponse();
                    WriteToResponse(response, webResponse);
                }
                catch (WebException e)
                {
                    // Handle response WebExceptions for invalid renders (404s, 504s etc.) - but we still want the content 
                    WriteToResponse(response, e.Response as HttpWebResponse);
                }

                response.Flush();
                context.CompleteRequest();
            }
        }

        /// <summary>
        /// Write WebResponse to HttpResponse
        /// </summary>
        /// <param name="response"></param>
        /// <param name="webResponse"></param>
        private void WriteToResponse(HttpResponse response, HttpWebResponse webResponse)
        {
            response.StatusCode = (int)webResponse.StatusCode;
            foreach (string key in webResponse.Headers.Keys)
            {
                response.Headers[key] = webResponse.Headers[key];
            }

            using (var reader = new StreamReader(webResponse.GetResponseStream(), DefaultEncoding))
            {
                response.Write(reader.ReadToEnd());
            }
        }

        private bool ShouldPrerenderPage(HttpRequest request)
        {
            var userAgent = request.UserAgent;
            var url = request.Url;
            var rawUrl = request.RawUrl;
            var relativeUrl = request.AppRelativeCurrentExecutionFilePath;

            // check if follows google search engine suggestion
            if (request.QueryString.AllKeys.Any(a => a.Equals(Constants.EscapedFragment, StringComparison.InvariantCultureIgnoreCase)))
                return true;

            // check if has user agent
            if (string.IsNullOrEmpty(userAgent))
                return false;

            // check if it's crawler user agent.
            var crawlerUserAgentPattern = Configuration.CrawlerUserAgentPattern ?? Constants.CrawlerUserAgentPattern;
            if (string.IsNullOrEmpty(crawlerUserAgentPattern)
             || !Regex.IsMatch(userAgent, crawlerUserAgentPattern, RegexOptions.IgnorePatternWhitespace))
                return false;
            
            // check if the extenion matchs default extension
            if (Regex.IsMatch(relativeUrl, DefaultIgnoredExtensions, RegexOptions.IgnorePatternWhitespace))
                return false;

            if (!string.IsNullOrEmpty(Configuration.WhiteListPattern)
              && Regex.IsMatch(rawUrl, Configuration.WhiteListPattern, RegexOptions.IgnorePatternWhitespace))
                return true;

            if (!string.IsNullOrEmpty(Configuration.BlackListPattern)
              && Regex.IsMatch(rawUrl, Configuration.BlackListPattern, RegexOptions.IgnorePatternWhitespace))
                return false;

            return true;

        }
        #endregion
         
    }
}
