namespace Linn.Tax.Resources
{
    using System.Collections.Generic;

    public class FraudPreventionMetadataResource
    {
        public bool DoNotTrack { get; set; }

        public int WindowWidth { get; set; }

        public int WindowHeight { get; set; }

        public List<string> BrowserPlugins { get; set; }

        public string UserAgentString { get; set; }

        public string Username { get; set; }

        public List<string> LocalIps { get; set; }

        public string LocalIpsTimestamp { get; set; }

        public int ScreenWidth { get; set; }

        public int ScreenHeight { get; set; }

        public int ScalingFactor { get; set; }

        public int ColourDepth { get; set; }

        public int TimezoneOffset { get; set; }
    }
}
