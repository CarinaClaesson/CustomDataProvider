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

            var entity = new Entity("cr8d8_launch");

            tracingService.Trace("Starting to retrieve SpaceX Launch data");

            try
            {
                var guid = context.PrimaryEntityId;
                tracingService.Trace("Guid: {0}", guid);

                // Translate guid into the Flight Number ID
                var flightNumber = CDPHelper.GuidToInt(guid);
                tracingService.Trace("Flight Number: {0}", flightNumber);

                // Now we know which Launch to search for, let us go and do the search
                var webRequest = WebRequest.Create($"\nhttps://api.spacexdata.com/v3/launches/{flightNumber}?filter=rocket/rocket_name,flight_number,mission_name,launch_year,launch_date_utc,links") as HttpWebRequest;

                if (webRequest != null)
                {
                    webRequest.ContentType = "application/json";
                    using (var s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (var sr = new StreamReader(s))
                        {
                            var launchesAsJson = sr.ReadToEnd();
                            tracingService.Trace("Response: {0}", launchesAsJson);

                            launchesAsJson = "{\"Launches\": [" + launchesAsJson + "]}";
                            tracingService.Trace("JSON: {0}", launchesAsJson);

                            var obj = JsonConvert.DeserializeObject<RootObject>(launchesAsJson);

                            if (obj != null)
                            {
                                // Translate id to Guid
                                var uniqueIdentifier = CDPHelper.IntToGuid(flightNumber);

                                // Map Launch data to entity model
                                entity["cr8d8_launchid"] = uniqueIdentifier;
                                entity["cr8d8_name"] = obj.Launches[0].mission_name;
                                entity["cr8d8id"] = obj.Launches[0].flight_number;
                                entity["cr8d8_rocket"] = obj.Launches[0].rocket.rocket_name;
                                entity["cr8d8_launchyear"] = obj.Launches[0].launch_year;
                                entity["cr8d8_launchdate"] = obj.Launches[0].launch_date_utc;
                                entity["cr8d8_missionpatch"] = obj.Launches[0].links.mission_patch;
                                entity["cr8d8_presskit"] = obj.Launches[0].links.presskit;
                                entity["cr8d8_videolink"] = obj.Launches[0].links.video_link;
                                entity["cr8d8_wikipedia"] = obj.Launches[0].links.wikipedia;
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
