# prerender.io Middlewares for ASP.NET MVC, ASP.NET Core, IIS
This project is to provide the prerender.io middlewares for ASP.NET MVC, ASP.NET Core, IIS. 

There are two blog posts which explains why we need to use prerender to improve JavaScript SEO and how to implement it on different level.
* [Use Prerender to improve AngularJS SEO](http://netopensource.com/use-prerender-improve-angularjs-seo/)
* [Setup Prerender Service for JavaScript SEO](http://netopensource.com/setup-prerender-service-javascript-seo/)

## src/DotNetPrerender is the prerender.io middleware for ASP.NET MVC

### Requirements
* .NET Framework 4.6.2
* Microsoft.Web.Infrastructure (>= 1.0.0)

### User Guide
* Download from Nuget: Install-Package DotNetOpen.PrerenderModule   
* Configure PrerenderHttpModule in your ASP.NET or ASP.NET MVC project, there are two ways to configure:   
      * Option 1: Use UsePrestartForPrenderModule app setting. Once UsePrestartForPrenderModule is true, it means we will use PreApplicationStartMethodAttribute to dynamically load the http module.
      * Option 2: Use Web.config to configure PrerenderHttpModule (set UsePrestartForPrenderModule = false), please make sure you use Integrated Mode for application pool.

### Sample code
You can download the sample project from: https://github.com/dingyuliang/prerender-dotnet/tree/master/src/DotNetPrerender/DotNetOpen.PrerenderModule.Mvc

## src/DotNetCorePrerender is the prerender.io middleware for ASP.NET Core
## src/IIS is the prerender.io configuration guide for IIS

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

