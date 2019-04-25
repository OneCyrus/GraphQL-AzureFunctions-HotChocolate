using GraphQLAzureFunctions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(WebJobsExtensionStartup))]
namespace GraphQLAzureFunctions
{
    public class WebJobsExtensionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<InjectConfiguration>();
        }
    }
}
