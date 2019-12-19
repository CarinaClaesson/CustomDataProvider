
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace CustomDataProvider
{
    /// <summary>
    /// Represents Rockets from the SpaceX API
    /// </summary>
    public class Rocket
    {
        public string rocket_name { get; set; }
    }

    /// <summary>
    /// Represents Links from the SpaceX API
    /// </summary>
    public class Links
    {
        public string mission_patch { get; set; }
        public string mission_patch_small { get; set; }
        public string reddit_campaign { get; set; }
        public string reddit_launch { get; set; }
        public string reddit_recovery { get; set; }
        public string reddit_media { get; set; }
        public string presskit { get; set; }
        public string article_link { get; set; }
        public string wikipedia { get; set; }
        public string video_link { get; set; }
        public string youtube_id { get; set; }
        public List<object> flickr_images { get; set; }
    }

    /// <summary>
    /// Represents Launches from the SpaceX API
    /// </summary>
    public class Launch
    {
        public Rocket rocket { get; set; }
        public int flight_number { get; set; }
        public string mission_name { get; set; }
        public string launch_year { get; set; }
        public DateTime launch_date_utc { get; set; }
        public Links links { get; set; }

        public Entity getLaunchAsEntity(ITracingService tracingService)
        {
            Entity entity = new Entity("cr8d8_launch");

            // Transform int unique value to Guid
            var id = flight_number;
            var uniqueIdentifier = CDPHelper.IntToGuid(id);
            tracingService.Trace("Flight Number: {0} transformed into Guid: {1}", flight_number, uniqueIdentifier);

            // Map data to entity
            entity["cr8d8_launchid"] = uniqueIdentifier;
            entity["cr8d8_name"] = mission_name;
            entity["cr8d8_id"] = flight_number;
            entity["cr8d8_rocket"] = rocket.rocket_name;
            entity["cr8d8_launchyear"] = launch_year;
            entity["cr8d8_launchdate"] = launch_date_utc;
            entity["cr8d8_missionpatch"] = links.mission_patch;
            entity["cr8d8_presskit"] = links.presskit;
            entity["cr8d8_videolink"] = links.video_link;
            entity["cr8d8_wikipedia"] = links.wikipedia;

            return entity;
        }
    }
}