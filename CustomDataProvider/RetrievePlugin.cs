using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace CustomDataProvider
{
    /// <summary>
    /// Retrieves some chosen data for one Launch from the SpaceX API
    /// </summary>
    public class RetrievePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var service = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity entity = null;

            tracingService.Trace("Starting to retrieve SpaceX Launch data");

            try
            {
                var guid = context.PrimaryEntityId;
                tracingService.Trace("Guid: {0}", guid);

                // Translate guid into the Flight Number ID
                var flightNumber = CDPHelper.GuidToInt(guid);
                tracingService.Trace("Flight Number: {0}", flightNumber);

                // Now we know which Launch to search for, let us go and do the search
                var webRequest = WebRequest.Create($"\nhttps://api.spacexdata.com/v3/launches/{flightNumber}?filter=rocket/rocket_name,flight_number,mission_name,launch_year,launch_date_utc,links,details") as HttpWebRequest;

                if (webRequest != null)
                {
                    webRequest.ContentType = "application/json";
                    using (var s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (var sr = new StreamReader(s))
                        {
                            var launchAsJson = sr.ReadToEnd();
                            tracingService.Trace("Response: {0}", launchAsJson);

                            var launch = JsonConvert.DeserializeObject<Launch>(launchAsJson);
                            if (launch != null)
                            {
                                entity = launch.ToEntity(tracingService);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                tracingService.Trace("Exception with message: {0}", e.Message);
            }

            // Set output parameter
            context.OutputParameters["BusinessEntity"] = entity;
        }
    }
}
