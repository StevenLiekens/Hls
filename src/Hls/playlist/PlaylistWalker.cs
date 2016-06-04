using System;
using Hls.duration;
using Hls.EXTINF;
using Hls.EXT_X_MEDIA_SEQUENCE;
using Hls.EXT_X_TARGETDURATION;
using Hls.EXT_X_VERSION;
using Hls.title;
using Txt.Core;
using Uri.URI;

namespace Hls.playlist
{
    public class PlaylistWalker : Walker
    {
        private readonly IParser<Duration, TimeSpan> durationParser;

        private readonly IParser<ExtTargetDuration, TimeSpan> targetDurationParser;

        private readonly IParser<ExtVersion, int> versionParser;

        private readonly IParser<ExtMediaSequence, int> mediaSequenceParser;

        private int sequence = 0;

        private MediaSegment currentSegment;

        public PlaylistWalker(
            IParser<ExtVersion, int> versionParser,
            IParser<ExtTargetDuration, TimeSpan> targetDurationParser,
            IParser<Duration, TimeSpan> durationParser,
            IParser<ExtMediaSequence, int> mediaSequenceParser)
        {
            this.versionParser = versionParser;
            this.targetDurationParser = targetDurationParser;
            this.durationParser = durationParser;
            this.mediaSequenceParser = mediaSequenceParser;
        }

        public PlaylistFile Result { get; private set; }

        public void Enter(Playlist playlist)
        {
            Result = new PlaylistFile();
        }

        public void Enter(ExtInf inf)
        {
            currentSegment = new MediaSegment();
        }

        public void Exit(Playlist playlist)
        {
            Result.Complete();
        }

        public bool Walk(ExtVersion version)
        {
            Result.Version = versionParser.Parse(version);
            return false;
        }

        public bool Walk(ExtTargetDuration targetDuration)
        {
            Result.TargetDuration = targetDurationParser.Parse(targetDuration);
            return false;
        }

        public bool Walk(Duration duration)
        {
            currentSegment.Duration = durationParser.Parse(duration);
            return false;
        }

        public bool Walk(Title title)
        {
            currentSegment.Title = title.Text;
            return false;
        }

        public bool Walk(UniformResourceIdentifier uri)
        {
            currentSegment.Location = new System.Uri(uri.Text, UriKind.RelativeOrAbsolute);
            return false;
        }

        public void Enter(ExtMediaSequence mediaSequence)
        {
            if (currentSegment != null)
            {
                throw new InvalidOperationException("The EXT-X-MEDIA-SEQUENCE tag MUST appear before the first Media Segment in the Playlist.");
            }
        }

        public bool Walk(ExtMediaSequence mediaSequence)
        {
            sequence = mediaSequenceParser.Parse(mediaSequence);
            return false;
        }

        public void Exit(UniformResourceIdentifier uri)
        {
            currentSegment.Sequence = sequence++;
            Result.Segments.Add(currentSegment);
        }
    }
}
