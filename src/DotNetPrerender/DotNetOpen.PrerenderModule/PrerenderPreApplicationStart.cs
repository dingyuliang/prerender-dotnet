using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOpen.PrerenderModule
{
    /// <summary>
    /// if you want to load this prerender http module dynamically, you can use this method in assembly.cs
    /// 
    /// </summary>
    public static class PrerenderPreApplicationStart
    {
        public const string StartMethodName = "Prestart";
        static bool UsePrestart = !bool.FalseString.Equals(ConfigurationManager.AppSettings[Constants.AppSetting_UsePrestartForPrenderModule], StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// used to configure for PreApplicationStart.
        /// i.e. [assembly: PreApplicationStartMethod(typeof(PrerenderPreApplicationStart), "Start")]
        /// </summary>
        public static void Prestart()
        {
            if (UsePrestart)
            {
                DynamicModuleUtility.RegisterModule(typeof(PrerenderHttpModule));
            }
        }
    }
}
