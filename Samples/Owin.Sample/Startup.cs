﻿using Metrics;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin.Metrics;
using System;
using System.Web.Http;

namespace Owin.Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            app.UseCors(CorsOptions.AllowAll);

            Metric.Config
                .WithAllCounters()
                .WithReporting(r => r.WithConsoleReport(TimeSpan.FromSeconds(30)))
                .WithOwin(middleware => app.Use(middleware), config => config
                    .WithRequestMetricsConfig(c => c.RegisterAllMetrics())
                    .WithMetricsEndpoint()
                );

            //app.UseMetrics(Metric.Config.WithAllCounters()
            //    .WithReporting(r => r.WithConsoleReport(TimeSpan.FromSeconds(30))),
            //    owinMetrics => owinMetrics.RegisterAllMetrics());

            var httpConfig = new HttpConfiguration();
            httpConfig.MapHttpAttributeRoutes();
            app.UseWebApi(httpConfig);
        }
    }
}
