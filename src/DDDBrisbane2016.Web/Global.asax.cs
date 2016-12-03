using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using ConfigInjector.QuickAndDirty;
using DDDBrisbane2016.Web.AppSettings;
using Serilog;
using Serilog.Core;
using SerilogWeb.Classic;
using SerilogWeb.Classic.Enrichers;

namespace DDDBrisbane2016.Web
{
    public class MvcApplication : HttpApplication
    {
        private IContainer _container;

        protected void Application_Start()
        {
            //TODO Extract logger configuration (during presentation)
            var seqServerUri = DefaultSettingsReader.Get<SeqServerUri>();
            var seqApiKey = DefaultSettingsReader.Get<SeqApiKey>();
            var logEventLevel = DefaultSettingsReader.Get<LogEventLevel>();
            var logLevelSwitch = new LoggingLevelSwitch(logEventLevel);
            var appPoolId = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

            ApplicationLifecycleModule.RequestLoggingLevel = Serilog.Events.LogEventLevel.Verbose;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(logLevelSwitch)
                .WriteTo.Seq(seqServerUri.ToString(), apiKey: seqApiKey, controlLevelSwitch: logLevelSwitch)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", typeof(MvcApplication).Assembly.GetName().Name)
                .Enrich.WithProperty("ApplicationVersion", typeof(MvcApplication).Assembly.GetName().Version)
                .Enrich.WithProperty("ApplicationPool", appPoolId)
                .Enrich.WithProperty("ProcessName", Process.GetCurrentProcess().ProcessName)
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.With<HttpRequestClientHostIPEnricher>()
                .Enrich.With<HttpRequestNumberEnricher>()
                .Enrich.With<HttpRequestIdEnricher>()
                .Enrich.With<HttpRequestRawUrlEnricher>()
                .Enrich.With<HttpRequestTypeEnricher>()
                .Enrich.With<HttpRequestUrlEnricher>()
                .Enrich.With<HttpRequestUrlReferrerEnricher>()
                .Enrich.With<HttpRequestUserAgentEnricher>()
                .CreateLogger();
            Log.Information("Logger online");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _container = IoC.LetThereBeIoC();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
        }

        protected void Application_End()
        {
            Log.Information("Application shutting down");

            _container?.Dispose();
            _container = null;

            Log.CloseAndFlush();
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            if (exception == null) return;

            Log.Error(exception, "An unhandled exception occurred while processing a request");
        }
    }
}