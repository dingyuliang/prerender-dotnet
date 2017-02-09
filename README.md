# prerender.io Middlewares for ASP.NET MVC, ASP.NET Core, IIS
This project is to provide the prerender.io middlewares for ASP.NET MVC, ASP.NET Core, IIS.

## src/DotNetPrerender is the prerender.io middleware for ASP.NET MVC

* Download from Nuget: Install-Package DotNetOpen.PrerenderModule   
* There are two different ways to configure PrerenderHttpModule   
      * Option 1: Use UsePrestartForPrenderModule app setting. Once UsePrestartForPrenderModule is true, it means we will use PreApplicationStartMethodAttribute to dynamically load the http module.
      * Option 2: Use Web.config to configure PrerenderHttpModule (set UsePrestartForPrenderModule = false), please make sure you use Integrated Mode for application pool.

## src/DotNetCorePrerender is the prerender.io middleware for ASP.NET Core
## src/IIS is the prerender.io configuration guide for IIS

