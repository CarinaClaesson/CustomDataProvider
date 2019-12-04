using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace CustomDataProvider
{
    public class TechPortRetrieveMultiplePlugin : IPlugin
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

                                    // TODO - Should test the GUID-INT translation separately in console app
                                    // Translate id to Guid
                                    var id = projDetails.id;
                                    var uniqueIdentifier = CDPHelper.Int2Guid(id);

                                    entity["cc_techport_projectid"] = uniqueIdentifier;
                                    entity["cc_name"] = projDetails.title;
                                    entity["cc_id"] = projDetails.id;
                                    ec.Entities.AddRange(entity);

                                    // TODO: Add more fields to Virtual Entity (config) and here
                                    // Description would be cool to have!
                                    //   projDetails.startDate;
                                    //   projDetails.status;
                                    //   projDetails.description;
                                    //   projDetails.endDate;
                                    //   projDetails.benefits;
                                    //   projDetails.id;
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

}
