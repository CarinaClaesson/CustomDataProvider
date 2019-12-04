using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace CustomDataProvider
{
    public class TechportRetrieveMultiplePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            EntityCollection ec = new EntityCollection();

            // Get data from Techport
            var webRequest = WebRequest.Create("https://api.nasa.gov/techport/api/projects?api_key=uAPHV2We3eqFJNu2Hl1tvraadSamNxWYOx8A8lvP") as HttpWebRequest;
            if (webRequest == null)
            {
                return;
            }

            webRequest.ContentType = "application/json";

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var techportProjectsAsJson = sr.ReadToEnd();
                    var obj = JsonConvert.DeserializeObject<RootObject>(techportProjectsAsJson);
                    var projects = obj.projects;
                    Console.WriteLine(projects.totalCount);
                    var techportProjects = projects.projects;
                    foreach (var techportProject in techportProjects)
                    {
                        // For each id - make a new API call to recieve more info
                        var webRequest2 = WebRequest.Create($"\nhttps://api.nasa.gov/techport/api/projects/{techportProject.id}?api_key=uAPHV2We3eqFJNu2Hl1tvraadSamNxWYOx8A8lvP") as HttpWebRequest;

                        if (webRequest2 != null)
                        {
                            webRequest2.ContentType = "application/json";
                            using (var s2 = webRequest2.GetResponse().GetResponseStream())
                            {
                                using (var sr2 = new StreamReader(s2))
                                {
                                    var detailsAsJson = sr2.ReadToEnd();
                                    var obj2 = JsonConvert.DeserializeObject<RootObject2>(detailsAsJson);
                                    var projDetails = obj2.project;

                                    // Map TechPort data to entity model
                                    Entity entity = new Entity("cc_techport_project");
                                    entity["cc_techport_projectid"] = Guid.NewGuid();
                                    entity["cc_name"] = projDetails.title;
                                    entity["cc_id"] = projDetails.id;
                                    ec.Entities.AddRange(entity);

                                    //   Console.WriteLine(projDetails.startDate);
                                    //   Console.WriteLine(projDetails.status);
                                    //   Console.WriteLine(projDetails.description);
                                    //   Console.WriteLine(projDetails.endDate);
                                    //   Console.WriteLine(projDetails.benefits);
                                    //   Console.WriteLine(projDetails.id);
                                }
                            }
                        }
                    }
                }
            }

            // Set output parameter

            context.OutputParameters["BusinessEntityCollection"] = ec;
        }
    }

    public class TechportRetrievePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            var guid = context.PrimaryEntityId;

            // Hur skall man få till sin retrieve?
            // Hur veta vilken post som skall hämtas?
            // I exemplet i docs har de ingen retrieve bara retrieve multiple?
            // De har även en som returnerar en query med det är motsvarande när jag hämtar med http?





            // Fetch data from another source
            /*
            var item = SensorMeasurementRepository.Get(context.PrimaryEntityId);

            // Map 3rd party data to entity model
            Entity entity = new Entity(context.PrimaryEntityName);
            entity["fic_name"] = item.Name;
            entity["fic_temp"] = item.Temperature;

            // Set output parameter
            context.OutputParameters["BusinessEntity"] = entity;

    */

            // Get the Techport ID so that we can fetch the fields needed


            // Go and get the data from another source
            // Request to get the data
            // 

            // Map Techport data to entity model

            // Fetch data from another source
            //  var item = SensorMeasurementRepository.Get(context.PrimaryEntityId);

            // Map 3rd party data to entity model
            Entity entity = new Entity(context.PrimaryEntityName);
            // entity["fic_name"] = item.Name;
            //  entity["fic_temp"] = item.Temperature;

            // Set output parameter
            context.OutputParameters["BusinessEntity"] = entity;
        }

    }
}
