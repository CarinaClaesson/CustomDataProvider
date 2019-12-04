using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomDataProvider
{
    public class TechPortRetrievePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            var guid = context.PrimaryEntityId;

            // Translate guid into the TechPort ID
            var TechPortId = CDPHelper.Guid2Int(guid);

            // Now we know which TechPort record to search for, let us go and do the search

            // TODO - Bryt ut så att detta ligger i någon metod som vi anropar både här och i RetrieveMultiple-fallet!
            var webRequest2 = WebRequest.Create($"\nhttps://api.nasa.gov/techport/api/projects/{TechPortId}?api_key=uAPHV2We3eqFJNu2Hl1tvraadSamNxWYOx8A8lvP") as HttpWebRequest;

            Entity entity = new Entity("cc_techport_project");

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


                        // Translate id to Guid
                        var id = projDetails.id;
                        var uniqueIdentifier = CDPHelper.Int2Guid(id);

                        entity["cc_techport_projectid"] = uniqueIdentifier;
                        entity["cc_name"] = projDetails.title;
                        entity["cc_id"] = projDetails.id;

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

            // Set output parameter
            context.OutputParameters["BusinessEntity"] = entity;   
        }
    }
}
