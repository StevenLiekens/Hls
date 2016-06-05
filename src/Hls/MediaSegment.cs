﻿using System;

namespace Hls
{
    public class MediaSegment
    {
        public int DiscontinuitySequence { get; set; }

        public TimeSpan Duration { get; set; }

        public Key Key { get; set; }

        public System.Uri Location { get; set; }

        public int Sequence { get; set; }

        public string Title { get; set; }
    }
}
