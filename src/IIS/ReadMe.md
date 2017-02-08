#Instructions

Note: this test won't work on your local.

1. Install Application Requesst Routing (ARR) in IIS
   * Configure Application Request Routing Proxy Setting. (IIS server node -> Right Panel Application Requst Routing -> Server Proxy Settings)
   * Enable Proxy.  
      Http version: Pass through.
2. Install URL Rewrite Module 
3. Configure URL Rewrite in web.config as below. 
   This configuration worked sometimes, but sometime didn't work, it might be the URL rewrite cache issue. 
   
 <cite>
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
   </cite>
