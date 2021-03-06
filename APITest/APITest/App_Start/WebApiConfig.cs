﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;
using APITest.MessageHandlers;
namespace APITest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            //var cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);

            // Allows cors for from webapi endpoints. 
            //var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors();
            config.MessageHandlers.Add(new CorsHandler());    

            //config.EnableCors();
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }
    }
}
