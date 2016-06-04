using System;

namespace Hls
{
    public class MediaSegment
    {
        public TimeSpan Duration { get; set; }

        public System.Uri Location { get; set; }

        public string Title { get; set; }
    }
}
