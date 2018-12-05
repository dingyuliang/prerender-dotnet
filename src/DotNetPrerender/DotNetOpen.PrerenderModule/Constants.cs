using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOpen.PrerenderModule
{
    public static class Constants
    {
        #region Const
        public const string PrerenderIOServiceUrl = "http://service.prerender.io/";
        public const int DefaultPort = 80;
        public const string CrawlerUserAgentPattern = "(bingbot)|(googlebot)|(google)|(bing)|(Slurp)|(DuckDuckBot)|(YandexBot)|(baiduspider)|(Sogou)|(Exabot)|(ia_archiver)|(facebot)|(facebook)|(twitterbot)|(rogerbot)|(linkedinbot)|(embedly)|(quora)|(pinterest)|(slackbot)|(redditbot)|(Applebot)|(WhatsApp)|(flipboard)|(tumblr)|(bitlybot)|(Discordbot)";

        public const string EscapedFragment = "_escaped_fragment_";
        public const string HttpProtocol = "http://";
        public const string HttpsProtocol = "https://";
        public const string HttpHeader_XForwardedProto = "X-Forwarded-Proto";
        public const string HttpHeader_XPrerenderToken = "X-Prerender-Token";
        public const string AppSetting_UsePrestartForPrenderModule = "UsePrestartForPrenderModule";
        #endregion
    }
}
