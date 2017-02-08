using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOpen.PrerenderModule.Configuration
{
    public class PrerenderConfigurationSection : ConfigurationSection
    {
        #region Const
        public const string SectionName = "prerender";
        public const string ServerUrlPropertyName = "ServiceUrl";
        public const string TokenPropertyName = "Token";
        public const string CrawlerUserAgentPatternPropertyName = "CrawlerUserAgentPattern";
        public const string WhiteListPatternPropertyName = "WhiteListPattern";
        public const string BlackListPatternPropertyName = "BlackListPattern";
        public const string AdditionalExtensionPatternPropertyName = "AdditionalExtensionPattern";
        public const string ProxyUrlPropertyName = "ProxyUrl";
        public const string ProxyPortPropertyName = "ProxyPort";
        #endregion

        #region Ctor
        public PrerenderConfigurationSection()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// get or set Prerender service URL
        /// </summary>
        [ConfigurationProperty(ServerUrlPropertyName, IsRequired = true, DefaultValue = Constants.PrerenderIOServiceUrl)]
        public string ServiceUrl
        {
            get { return (string)this[ServerUrlPropertyName]; }
            set
            { this[ServerUrlPropertyName] = value; }
        }

        /// <summary>
        /// get or set Prerender service token
        /// </summary>
        [ConfigurationProperty(TokenPropertyName)]
        public string Token
        {
            get { return (string)this[TokenPropertyName]; }
            set { this[TokenPropertyName] = value; }
        }

        /// <summary>
        /// get or set cralwer user agent pattern, which is used to identity whether the request comes from crawlers
        /// </summary>
        [ConfigurationProperty(CrawlerUserAgentPatternPropertyName, DefaultValue = Constants.CrawlerUserAgentPattern)]
        public string CrawlerUserAgentPattern
        {
            get { return (string)this[CrawlerUserAgentPatternPropertyName]; }
            set { this[CrawlerUserAgentPatternPropertyName] = value; }
        }

        /// <summary>
        /// Get or set the white list of prerender; If the request URL is in white list, it will use prerender logic.
        /// If the value is EMPTY, nothing will be affected;
        /// </summary>
        [ConfigurationProperty(WhiteListPatternPropertyName)]
        public string WhiteListPattern
        {
            get { return (string)this[WhiteListPatternPropertyName]; }
            set { this[WhiteListPatternPropertyName] = value; }
        }

        /// <summary>
        /// Get or set the black list of prerender; If the request is in black list, it will not use prerender logic.
        /// If the value is EMPTY, nothing will be affected;
        /// </summary>
        [ConfigurationProperty(BlackListPatternPropertyName)]
        public string BlackListPattern
        {
            get { return (string)this[BlackListPatternPropertyName]; }
            set { this[BlackListPatternPropertyName] = value; }
        }

        /// <summary>
        /// Get or set the additional extension pattern which will be ignored prerender logic.
        /// By default, there are some extensions we will ignore.
        /// </summary>
        [ConfigurationProperty(AdditionalExtensionPatternPropertyName)]
        public string AdditionalExtensionPattern
        {
            get { return (string)this[AdditionalExtensionPatternPropertyName]; }
            set { this[AdditionalExtensionPatternPropertyName] = value; }
        }
        
        /// <summary>
        /// Get or set the proxy URL.
        /// </summary>
        [ConfigurationProperty(ProxyUrlPropertyName)]
        public string ProxyUrl
        {
            get { return (string)this[ProxyUrlPropertyName]; }
            set { this[ProxyUrlPropertyName] = value; }
        }

        /// <summary>
        /// Get or set the proxy port, only effective when ProxyUrl is NOT EMPTY.
        /// By default, it's 80;
        /// </summary>
        [ConfigurationProperty(ProxyPortPropertyName, DefaultValue = Constants.DefaultPort)]
        public int ProxyPort
        {
            get { return (int)this[ProxyPortPropertyName]; }
            set { this[ProxyPortPropertyName] = value; }
        }
        #endregion

        #region GetSection
        /// <summary>
        /// get "prerender" section
        /// </summary>
        /// <returns></returns>
        public static PrerenderConfigurationSection GetSection()
        {
            return ConfigurationManager.GetSection(SectionName) as PrerenderConfigurationSection;
        }
        #endregion
    }
}
