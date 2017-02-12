using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreOpen.PrerenderMiddleware
{
    public class PrerenderConfiguration
    {
        #region Properties
        public string ServiceUrl { get; set; }

        /// <summary>
        /// get or set Prerender service token
        /// </summary> 
        public string Token { get; set; }

        /// <summary>
        /// get or set cralwer user agent pattern, which is used to identity whether the request comes from crawlers
        /// </summary> 
        public string CrawlerUserAgentPattern { get; set; } = Constants.CrawlerUserAgentPattern;

        /// <summary>
        /// Get or set the white list of prerender; If the request URL is in white list, it will use prerender logic.
        /// If the value is EMPTY, nothing will be affected;
        /// </summary> 
        public string WhiteListPattern { get; set; }

        /// <summary>
        /// Get or set the black list of prerender; If the request is in black list, it will not use prerender logic.
        /// If the value is EMPTY, nothing will be affected;
        /// </summary> 
        public string BlackListPattern { get; set; }

        /// <summary>
        /// Get or set the additional extension pattern which will be ignored prerender logic.
        /// By default, there are some extensions we will ignore.
        /// </summary> 
        public string AdditionalExtensionPattern { get; set; }

        /// <summary>
        /// Get or set the proxy URL.
        /// </summary>
        public string ProxyUrl { get; set; }

        /// <summary>
        /// Get or set the proxy port, only effective when ProxyUrl is NOT EMPTY.
        /// By default, it's 80;
        /// </summary>
        public int ProxyPort { get; set; } = Constants.DefaultPort;
        #endregion
    }
}
