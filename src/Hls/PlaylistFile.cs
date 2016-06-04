using System;
using System.Collections.Generic;

namespace Hls
{
    public class PlaylistFile
    {
        private bool complete;

        public IList<MediaSegment> MediaSegments { get; } = new List<MediaSegment>();

        public TimeSpan? TargetDuration { get; set; }

        public int? Version { get; set; }

        public void Complete()
        {
            complete = true;
        }
    }
}
