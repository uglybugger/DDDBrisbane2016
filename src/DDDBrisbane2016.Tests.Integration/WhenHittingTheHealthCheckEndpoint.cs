using System.Net.Http;
using System.Threading.Tasks;
using ConfigInjector.QuickAndDirty;
using DDDBrisbane2016.Tests.Integration.AppSettings;
using NUnit.Framework;

namespace DDDBrisbane2016.Tests.Integration
{
    public class WhenHittingTheHealthCheckEndpoint
    {
        [Test]
        public async Task WeShouldGetASuccessfulResponse()
        {
            var healthCheckEndpoint = DefaultSettingsReader.Get<WebSiteBaseUrl>();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(healthCheckEndpoint);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}