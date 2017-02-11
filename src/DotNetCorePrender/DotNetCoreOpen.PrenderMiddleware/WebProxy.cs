using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DotNetCoreOpen.PrenderMiddleware
{
    public class WebProxy : IWebProxy
    {
        #region Ctor
        public WebProxy(string url, int port)
        {
            ProxyUri = new Uri($"{url}:{port}");
        }
        #endregion

        #region Properties
        public Uri ProxyUri { get; private set; }
        #endregion

        #region Implement IWebProxy
        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            return ProxyUri;
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }
        #endregion
    }
}
