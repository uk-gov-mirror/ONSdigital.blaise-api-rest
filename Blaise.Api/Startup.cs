﻿using System.Web.Http;
using Blaise.Api.Providers;
using Microsoft.Owin.Extensions;
using Owin;
using Unity.WebApi;

namespace Blaise.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.Use((context, next) =>
            {
                context.Response.Headers.Remove("Server");
                return next.Invoke();
            });
            appBuilder.UseStageMarker(PipelineStage.PostAcquireState);

            // Configure Web API for self-host. 
            var config = new HttpConfiguration
            {
                DependencyResolver = new UnityDependencyResolver(
                    UnityProvider.GetConfiguredContainer())
            };

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            appBuilder.UseWebApi(config);
        }
    }
}
