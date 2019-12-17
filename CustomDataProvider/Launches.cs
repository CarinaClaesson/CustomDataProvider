
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
    /// Represents Links retrieved from the SpaceX API
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
    /// Represents Launches retrieved from the SpaceX API
    /// </summary>
    public class Launch
    {
        public Rocket rocket { get; set; }
        public int flight_number { get; set; }
        public string mission_name { get; set; }
        public string launch_year { get; set; }
        public DateTime launch_date_utc { get; set; }
        public Links links { get; set; }
    }

    /// <summary>
    /// Represents the Root Object of the JSON retrieved from the SpaceX API
    /// </summary>
    public class RootObject
    {
        public List<Launch> Launches { get; set; }
    }
}