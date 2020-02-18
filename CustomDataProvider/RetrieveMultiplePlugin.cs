using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace CustomDataProvider
{
    /// <summary>
    /// Retrieves some chosen data for all Launches from the SpaceX API
    /// </summary>
    public class RetrieveMultiplePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var service = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            EntityCollection ec = new EntityCollection();

            tracingService.Trace("Starting to retrieve SpaceX Launch data");

            try
            {
                // Get data about SpaceX Launches
                var webRequest = WebRequest.Create("https://api.spacexdata.com/v3/launches?filter=rocket/rocket_name,flight_number,mission_name,launch_year,launch_date_utc,links,details") as HttpWebRequest;

                if (webRequest == null)
                {
                    return;
                }

                webRequest.ContentType = "application/json";

                using (var s = webRequest.GetResponse().GetResponseStream())
                {
                    using (var sr = new StreamReader(s))
                    {
                        var launchesAsJson = sr.ReadToEnd();
                        var launches = JsonConvert.DeserializeObject<List<Launch>>(launchesAsJson);
                        tracingService.Trace("Total number of Launches: {0}", launches.Count);
                        ec.Entities.AddRange(launches.Select(l => l.ToEntity(tracingService)));
                    }
                }
            }
            catch (Exception e)
            {
                tracingService.Trace("Exception with message: {0}", e.Message);
            }

            // Set output parameter
            context.OutputParameters["BusinessEntityCollection"] = ec;
        }
    }
}