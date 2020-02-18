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
        public string details { get; set; }

        public Entity ToEntity(ITracingService tracingService)
        {
            Entity entity = new Entity("cc_spacex_rocket_launch");

            // Transform int unique value to Guid
            var id = flight_number;
            var uniqueIdentifier = CDPHelper.IntToGuid(id);
            tracingService.Trace("Flight Number: {0} transformed into Guid: {1}", flight_number, uniqueIdentifier);

            // Map data to entity
            entity["cc_spacex_rocket_launchid"] = uniqueIdentifier;
            entity["cc_name"] = mission_name;
            entity["cc_flight_number"] = flight_number;
            entity["cc_rocket"] = rocket.rocket_name;
            entity["cc_launch_year"] = launch_year;
            entity["cc_launch_date"] = launch_date_utc;
            entity["cc_mission_patch"] = links.mission_patch;
            entity["cc_presskit"] = links.presskit;
            entity["cc_video_link"] = links.video_link;
            entity["cc_wikipedia"] = links.wikipedia;
            entity["cc_details"] = details;

            return entity;
        }
    }
}