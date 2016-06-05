using System;
using Hls.EOL;
using Hls.EXTINF;
using Hls.EXT_X_ENDLIST;
using Hls.EXT_X_KEY;
using Hls.EXT_X_MEDIA_SEQUENCE;
using Hls.EXT_X_STREAM_INF;
using Hls.EXT_X_TARGETDURATION;
using Hls.EXT_X_VERSION;
using Hls.playlist;
using Txt.Core;
using Uri.URI;

namespace Hls
{
    public class PlaylistWalker : Walker
    {
        private readonly IParser<ExtInf, Tuple<TimeSpan, string>> infoParser;

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
            IParser<ExtMediaSequence, int> mediaSequenceParser,
            IParser<ExtKey, Key> keyParser,
            IParser<ExtStreamInf, StreamInfo> streamInfParser,
            IParser<ExtInf, Tuple<TimeSpan, string>> infoParser)
        {
            this.versionParser = versionParser;
            this.targetDurationParser = targetDurationParser;
            this.mediaSequenceParser = mediaSequenceParser;
            this.keyParser = keyParser;
            this.streamInfParser = streamInfParser;
            this.infoParser = infoParser;
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

        public void Enter(ExtTargetDuration targetDuration)
        {
            if (Result.PlaylistType == PlaylistType.Unknown)
            {
                Result.PlaylistType = PlaylistType.Media;
            }
            else if (Result.PlaylistType == PlaylistType.Master)
            {
                throw new InvalidOperationException("EXT-X-TARGETDURATION cannot appear in a Master Playlist.");
            }
        }

        public void Enter(ExtEndList endList)
        {
            if (Result.PlaylistType == PlaylistType.Unknown)
            {
                Result.PlaylistType = PlaylistType.Media;
            }
            else if (Result.PlaylistType == PlaylistType.Master)
            {
                throw new InvalidOperationException("EXT-X-ENDLIST cannot appear in a Master Playlist.");
            }
        }

        public void Enter(ExtMediaSequence mediaSequence)
        {
            if (Result.PlaylistType == PlaylistType.Unknown)
            {
                Result.PlaylistType = PlaylistType.Media;
            }
            else if (Result.PlaylistType == PlaylistType.Master)
            {
                throw new InvalidOperationException("EXT-X-MEDIA-SEQUENCE cannot appear in a Master Playlist.");
            }
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

        public bool Walk(EndOfLine eol)
        {
            return false;
        }

        public bool Walk(ExtMediaSequence mediaSequence)
        {
            sequence = mediaSequenceParser.Parse(mediaSequence);
            return false;
        }

        public bool Walk(ExtEndList mediaSequence)
        {
            return false;
        }

        public bool Walk(ExtKey key)
        {
            this.key = keyParser.Parse(key);
            return false;
        }

        public bool Walk(ExtInf inf)
        {
            var info = infoParser.Parse(inf);
            mediaSegment = new MediaSegment
            {
                Duration = info.Item1,
                Title = info.Item2
            };
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
