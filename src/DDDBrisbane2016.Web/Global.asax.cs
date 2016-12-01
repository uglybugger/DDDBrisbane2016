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
using SerilogWeb.Classic.Enrichers;

namespace DDDBrisbane2016.Web
{
    public class MvcApplication : HttpApplication
    {
        private IContainer _container;

        protected void Application_Start()
        {
            var seqServerUri = DefaultSettingsReader.Get<SeqServerUri>();
            var seqApiKey = DefaultSettingsReader.Get<SeqApiKey>();
            var logEventLevel = DefaultSettingsReader.Get<LogEventLevel>();
            var logLevelSwitch = new LoggingLevelSwitch(logEventLevel);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(logLevelSwitch)
                .WriteTo.Seq(seqServerUri.ToString(), apiKey: seqApiKey, controlLevelSwitch: logLevelSwitch)
                .Enrich.FromLogContext()
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
        }

        protected void Application_BeginRequest()
        {
            var request = HttpContext.Current.Request;
            Log.Verbose("Beginning HTTP {HttpMethod} to {HttpRequestUrl}", request.HttpMethod, request.Url);
        }

        protected void Application_EndRequest()
        {
            var request = HttpContext.Current.Request;
            Log.Verbose("Completed HTTP {HttpMethod} to {HttpRequestUrl}", request.HttpMethod, request.Url);
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            if (exception == null) return;

            Log.Error(exception, "An unhandled exception occurred while processing a request");
        }
    }
}