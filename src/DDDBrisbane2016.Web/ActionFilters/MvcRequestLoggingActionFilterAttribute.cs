using System;
using System.Diagnostics;
using System.Web.Mvc;
using Serilog;
using Serilog.Events;
using ThirdDrawer.Extensions.ClassExtensionMethods;

namespace DDDBrisbane2016.Web.ActionFilters
{
    public class MvcRequestLoggingActionFilterAttribute : ActionFilterAttribute
    {
        private ILogger _logger;
        private readonly Stopwatch _stopwatch;

        public MvcRequestLoggingActionFilterAttribute(ILogger logger)
        {
            _logger = logger;
            _stopwatch = Stopwatch.StartNew();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);


            var request = filterContext.HttpContext.Request;

            var actionDescriptor = filterContext.ActionDescriptor as ReflectedActionDescriptor;
            if (actionDescriptor != null)
            {
                var methodInfo = actionDescriptor.MethodInfo;
                _logger = _logger
                    .ForContext("ControllerType", methodInfo.DeclaringType?.FullName)
                    .ForContext("ControllerAction", methodInfo.Name);
            }
            _logger.Debug(
                "HTTP {HttpMethod} to {RequestUrl} starting.",
                request.HttpMethod,
                request.Url.Coalesce(u => u.AbsolutePath, null));
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var elapsed = _stopwatch.Elapsed;

            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;

            var exception = filterContext.Exception;
            if (exception == null)
            {
                var statusCode = response?.StatusCode ?? 0;

                _logger.Debug(
                    "HTTP {HttpStatusCode} {HttpMethod} to {RequestUrl} action executed in {ElapsedTime}",
                    statusCode,
                    request.HttpMethod,
                    request.Url.Coalesce(u => u.AbsolutePath, null),
                    elapsed);
            }
            else
            {
                const int statusCode = 500; // response.StatusCode will still be a 200 here until the filter pipeline has completed
                _logger.Error(exception,
                    "HTTP {HttpStatusCode} {HttpMethod} to {RequestUrl} action failed in {ElapsedTime}: {ErrorMessage}",
                    statusCode,
                    request.HttpMethod,
                    request.Url.Coalesce(u => u.AbsolutePath, null),
                    _stopwatch.Elapsed,
                    elapsed);
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            _stopwatch.Stop();

            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;
            var statusCode = response?.StatusCode ?? 0;
            var logEventLevel = LogEventLevelHelper.CalculateLogEventLevel(_stopwatch.Elapsed, statusCode);

            var exception = filterContext.Exception;
            if (exception == null)
            {
                _logger.Write(logEventLevel,
                    "HTTP {HttpStatusCode} {HttpMethod} to {RequestUrl} completed in {ElapsedTime}",
                    statusCode,
                    request.HttpMethod,
                    request.Url.Coalesce(u => u.AbsolutePath, null),
                    _stopwatch.Elapsed);
            }
            else
            {
                _logger.Error(exception,
                    "HTTP {HttpStatusCode} {HttpMethod} to {RequestUrl} failed in {ElapsedTime}: {ErrorMessage}",
                    statusCode,
                    request.HttpMethod,
                    request.Url.Coalesce(u => u.AbsolutePath, null),
                    _stopwatch.Elapsed,
                    exception.Message);
            }
        }

        internal static class LogEventLevelHelper
        {
            private static readonly TimeSpan _requestDurationWarningThreshold = TimeSpan.FromSeconds(2);

            public static LogEventLevel CalculateLogEventLevel(TimeSpan elapsedTime, int statusCode)
            {
                if (statusCode >= 500) return LogEventLevel.Error;
                if (statusCode >= 400) return LogEventLevel.Warning;
                if (elapsedTime >= _requestDurationWarningThreshold) return LogEventLevel.Warning;
                return LogEventLevel.Verbose;
            }
        }
}
}