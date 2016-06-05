using System;
using System.Collections.Generic;

namespace Hls
{
    public class PlaylistFile
    {
        public IList<IntraFrameStreamInfo> IntraFrameStreamsInfo { get; set; } = new List<IntraFrameStreamInfo>();

        public IList<MediaSegment> MediaSegments { get; set; } = new List<MediaSegment>();

        public PlaylistType PlaylistType { get; set; }

        public TimeSpan? TargetDuration { get; set; }

        public IList<VariantStream> VariantStreams { get; set; } = new List<VariantStream>();

        public int? Version { get; set; }
    }
}
