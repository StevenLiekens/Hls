using System;
using Hls.duration;
using Hls.EXTINF;
using Hls.EXT_X_KEY;
using Hls.EXT_X_MEDIA_SEQUENCE;
using Hls.EXT_X_STREAM_INF;
using Hls.EXT_X_TARGETDURATION;
using Hls.EXT_X_VERSION;
using Hls.playlist;
using Hls.title;
using Txt.Core;
using Uri.URI;

namespace Hls
{
    public class PlaylistWalker : Walker
    {
        private readonly IParser<Duration, TimeSpan> durationParser;

        private readonly IParser<ExtKey, Key> keyParser;

        private readonly IParser<ExtMediaSequence, int> mediaSequenceParser;

        private readonly IParser<ExtStreamInf, StreamInfo> streamInfParser;

        private readonly IParser<ExtTargetDuration, TimeSpan> targetDurationParser;

        private readonly IParser<ExtVersion, int> versionParser;

        private Key key;

        private MediaSegment mediaSegment;

        private int sequence;

        private VariantStream variantStream;

        public PlaylistWalker(
            IParser<ExtVersion, int> versionParser,
            IParser<ExtTargetDuration, TimeSpan> targetDurationParser,
            IParser<Duration, TimeSpan> durationParser,
            IParser<ExtMediaSequence, int> mediaSequenceParser,
            IParser<ExtKey, Key> keyParser,
            IParser<ExtStreamInf, StreamInfo> streamInfParser)
        {
            this.versionParser = versionParser;
            this.targetDurationParser = targetDurationParser;
            this.durationParser = durationParser;
            this.mediaSequenceParser = mediaSequenceParser;
            this.keyParser = keyParser;
            this.streamInfParser = streamInfParser;
        }

        public PlaylistFile Result { get; private set; }

        public void Enter(Playlist playlist)
        {
            Result = new PlaylistFile();
        }

        public void Enter(ExtInf inf)
        {
            if (Result.PlaylistType == PlaylistType.Unknown)
            {
                Result.PlaylistType = PlaylistType.Media;
            }
            else if (Result.PlaylistType == PlaylistType.Master)
            {
                throw new InvalidOperationException("EXTINF is not valid in a Master Playlist");
            }
            mediaSegment = new MediaSegment();
        }

        public void Enter(ExtStreamInf streamInf)
        {
            if (Result.PlaylistType == PlaylistType.Unknown)
            {
                Result.PlaylistType = PlaylistType.Master;
            }
            else if (Result.PlaylistType == PlaylistType.Media)
            {
                throw new InvalidOperationException("EXT-X-STREAM-INF is not valid in a Media Playlist");
            }
        }

        public void Enter(ExtMediaSequence mediaSequence)
        {
            if (mediaSegment != null)
            {
                throw new InvalidOperationException(
                    "The EXT-X-MEDIA-SEQUENCE tag MUST appear before the first Media Segment in the Playlist.");
            }
        }

        public void Exit(Playlist playlist)
        {
            Result.Complete();
        }

        public void Exit(UniformResourceIdentifier uri)
        {
            switch (Result.PlaylistType)
            {
                case PlaylistType.Master:
                    Result.VariantStreams.Add(variantStream);
                    break;
                case PlaylistType.Media:
                    mediaSegment.Sequence = sequence++;
                    mediaSegment.Key = key;
                    Result.MediaSegments.Add(mediaSegment);
                    break;
                case PlaylistType.Unknown:
                    throw new InvalidOperationException();
            }
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
            if (mediaSegment == null)
            {
                throw new InvalidOperationException();
            }
            mediaSegment.Duration = durationParser.Parse(duration);
            return false;
        }

        public bool Walk(Title title)
        {
            if (mediaSegment == null)
            {
                throw new InvalidOperationException();
            }
            mediaSegment.Title = title.Text;
            return false;
        }

        public bool Walk(UniformResourceIdentifier uri)
        {
            switch (Result.PlaylistType)
            {
                case PlaylistType.Master:
                    variantStream.Location = new System.Uri(uri.Text, UriKind.RelativeOrAbsolute);
                    break;
                case PlaylistType.Media:
                    mediaSegment.Location = new System.Uri(uri.Text, UriKind.RelativeOrAbsolute);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return false;
        }

        public bool Walk(ExtMediaSequence mediaSequence)
        {
            sequence = mediaSequenceParser.Parse(mediaSequence);
            return false;
        }

        public bool Walk(ExtKey key)
        {
            this.key = keyParser.Parse(key);
            return false;
        }

        public bool Walk(ExtStreamInf streamInf)
        {
            variantStream = new VariantStream
            {
                StreamInfo = streamInfParser.Parse(streamInf)
            };
            return false;
        }
    }
}
