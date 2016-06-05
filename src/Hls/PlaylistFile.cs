using System;
using System.Collections.Generic;

namespace Hls
{
    public class PlaylistFile
    {
        private bool complete;

        public IList<MediaSegment> MediaSegments { get; } = new List<MediaSegment>();

        public PlaylistType PlaylistType { get; set; }

        public TimeSpan? TargetDuration { get; set; }

        public IList<VariantStream> VariantStreams { get; set; } = new List<VariantStream>();

        public IList<IntraFrameStreamInfo> IntraFrameStreamsInfo { get; set; } = new List<IntraFrameStreamInfo>();

        public int? Version { get; set; }

        public void Complete()
        {
            complete = true;
        }
    }
}
