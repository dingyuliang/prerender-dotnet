# ASP.NET MVC, ASP.NET Core, IIS Middlewares for prerender.io
This project is to provide the prerender.io middlewares for ASP.NET MVC, ASP.NET Core, IIS. 

There are several blog posts which explain why we need to use prerender to improve JavaScript SEO and how to implement it on different levels.
* [Use Prerender to improve AngularJS SEO](http://netopensource.com/use-prerender-improve-angularjs-seo/)

  * Application Level (i.e.  ASP.NET MVC middleware for prerender.io and ASP.NET Core middleware for prerender.io)
  * Server Container Level (i.e. IIS configuration guide for prerender.io)
  * Network Level
  
* [Setup Prerender Service for JavaScript SEO](http://netopensource.com/setup-prerender-service-javascript-seo/)

* [Prerender Implementation Best Practice](http://netopensource.com/prerender-implementation-best-practice/)

## src/DotNetPrerender is the ASP.NET MVC middleware for prerender.io

   Please follow [Prerender Middleware for ASP.NET](https://github.com/dingyuliang/prerender-dotnet/wiki/Prerender-Middleware-for-ASP.NET)
   
   Download from Nuget: 
```
Install-Package DotNetOpen.PrerenderModule  
```

## src/DotNetCorePrerender is the ASP.NET Core middleware for prerender.io 

   Please follow [Prerender Middleware for ASP.NET Core](https://github.com/dingyuliang/prerender-dotnet/wiki/Prerender-Middleware-for-ASP.NET-Core)
   
   Download from Nuget: 
```
Install-Package DotNetCoreOpen.PrenderMiddleware  
```

## src/IIS is the IIS configuration guide for prerender.io
   
   Please follow [Prerender Configuration in IIS](https://github.com/dingyuliang/prerender-dotnet/wiki/Prerender-Configuration-in-IIS)

