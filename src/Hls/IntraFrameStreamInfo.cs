using System;
using System.Collections.Generic;

namespace Hls
{
    public class IntraFrameStreamInfo
    {
        public IList<Rendition> AlternativeVideo { get; set; } = new List<Rendition>();

        public int? AverageBandwidth { get; set; }

        public int Bandwidth { get; set; }

        public IList<string> Codecs { get; set; } = new List<string>();

        public float? Framerate { get; set; }

        public Tuple<int, int> Resolution { get; set; }

        public Uri Uri { get; set; }

        public string Video { get; set; }
    }
}
