# ASP.NET MVC, ASP.NET Core, IIS Middlewares for prerender.io
This project is to provide the prerender.io middlewares for ASP.NET MVC, ASP.NET Core, IIS. 

There are two blog posts which explain why we need to use prerender to improve JavaScript SEO and how to implement it on different levels.
* [Use Prerender to improve AngularJS SEO](http://netopensource.com/use-prerender-improve-angularjs-seo/)

  * Application Level  
  * Server Container Level  
  * Network Level
  
* [Setup Prerender Service for JavaScript SEO](http://netopensource.com/setup-prerender-service-javascript-seo/)

## src/DotNetPrerender is the ASP.NET MVC middleware for prerender.io

### Requirements
* .NET Framework 4.6.2
* Microsoft.Web.Infrastructure (>= 1.0.0)

### User Guide
* Download from Nuget: 
```
Install-Package DotNetOpen.PrerenderModule  
```
* Configure PrerenderHttpModule in your ASP.NET or ASP.NET MVC project, there are two ways to configure:   
      * Option 1: Use UsePrestartForPrenderModule app setting. Once UsePrestartForPrenderModule is true, it means we will use PreApplicationStartMethodAttribute to dynamically load the http module.
      * Option 2: Use Web.config to configure PrerenderHttpModule (set UsePrestartForPrenderModule = false), please make sure you use Integrated Mode for application pool.

### Sample code
You can download the sample project from: https://github.com/dingyuliang/prerender-dotnet/tree/master/src/DotNetPrerender/DotNetOpen.PrerenderModule.Mvc

## src/DotNetCorePrerender is the ASP.NET Core middleware for prerender.io 

### Requirements
* .NETStandard 1.6

### User Guide
* Download from Nuget: 
```
Install-Package DotNetCoreOpen.PrenderMiddleware  
```
* Configuration File 
      Add PrerenderConfiguration.json in your ASP.NET Core project by following below content format:
        ```
	{
	  "PrerenderConfiguration": {
	    "ServiceUrl": "http://localhost:3000",
	    "Token": null,
	    "CrawlerUserAgentPattern": null,
	    "WhiteListPattern": null,
	    "BlackListPattern": "lib|css|js",
	    "AdditionalExtensionPattern": null,
	    "ProxyUrl": null,
	    "ProxyPort": 80
	  }
	}
        ```
* Add code in startup.cs
      ** Step 1: Use AddPrerenderConfig() in ConfigurationBuilder
      
        ```
        var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                // Prerender Step 1: Add Prerender configuration Json file.
                .AddPrerenderConfig() 
                .AddEnvironmentVariables();
	```
	
      ** Step 2: Configure Configuration Option in ServiceCollections
      
        ```
	    // Prerender Step 2: Add Options.
            services.AddOptions();
            services.ConfigureSection<PrerenderConfiguration>(Configuration);
        ```
	
      ** Step 3: Configure Prerender Middleware in IApplicationBuilder
      
        ```
            // Prerender Step 3: UsePrerender, before others.
            app.UsePrerender();	
        ```
      
### Sample code
You can download the sample project from: https://github.com/dingyuliang/prerender-dotnet/tree/master/src/DotNetCorePrender/DotNetCoreOpen.PrenderMiddleware.Mvc


## src/IIS is the IIS configuration guide for prerender.io

### User Guide

1. Install Application Requesst Routing (ARR) in IIS
   * Configure Application Request Routing Proxy Setting. (IIS server node -> Right Panel Application Requst Routing -> Server Proxy Settings)
   * Enable Proxy.  
      Http version: Pass through.
2. Install URL Rewrite Module 
3. Configure URL Rewrite in web.config as below. 
   This configuration worked sometimes, but sometime didn't work, it might be the URL rewrite cache issue. 
   
```
   <rewrite>
            <rules>
                <rule name="RewriteSEO">
                    <match url="^((?!(bundles|scripts)).)*$" ignoreCase="true" />
			<conditions logicalGrouping="MatchAny">  
                        <add input="{QUERY_STRING}" matchType="Pattern" ignoreCase="true" pattern="_escaped_fragment_="/>
                        <add input="{HTTP_USER_AGENT}" matchType="Pattern" ignoreCase="true" pattern="(baiduspider)|(facebookexternalhit)|(twitterbot)|(rogerbot)|(linkedinbot)|(embedly)|(quora link preview)|(showyoubot)|(outbrain)|(pinterest)|(google\\.com)|(slackbot)|(vkShare)|(W3C_Validator)|(redditbot)|(Applebot)|(WhatsApp)|(flipboard)|(tumblr)|(bitlybot)|(SkypeUriPreview)|(nuzzel)|(Discordbot)|(Google Page Speed)|(x\\-bufferbot)"/>
                    </conditions>
                    <action type="Rewrite" url="http://service.prerender.io/http://{HTTP_HOST}{URL}" appendQueryString="false"/>
		    <serverVariables>
			 <set name="HTTP_X_PRERENDER_TOKEN" value="{Your Token}" />
	            </serverVariables>
                </rule>
            </rules>
    </rewrite> 
```

### Sample code
You can download the sample project from: https://github.com/dingyuliang/prerender-dotnet/tree/master/src/IIS

