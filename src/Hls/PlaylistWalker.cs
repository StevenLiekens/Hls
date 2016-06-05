using System;
using System.Collections.Generic;
using System.Linq;
using Hls.EOL;
using Hls.EXTINF;
using Hls.EXT_X_DISCONTINUITY;
using Hls.EXT_X_DISCONTINUITY_SEQUENCE;
using Hls.EXT_X_ENDLIST;
using Hls.EXT_X_I_FRAME_STREAM_INF;
using Hls.EXT_X_KEY;
using Hls.EXT_X_MEDIA;
using Hls.EXT_X_MEDIA_SEQUENCE;
using Hls.EXT_X_STREAM_INF;
using Hls.EXT_X_TARGETDURATION;
using Hls.EXT_X_VERSION;
using Hls.playlist;
using Txt.Core;
using Uri.URI_reference;

namespace Hls
{
    public class PlaylistWalker : Walker
    {
        private readonly IParser<ExtDiscontinuitySequence, int> discontinuitySequenceParser;

        private readonly IParser<ExtIFrameStreamInf, IntraFrameStreamInfo> iframeStreamInfParser;

        private readonly IParser<ExtInf, Tuple<TimeSpan, string>> infoParser;

        private readonly IParser<ExtKey, Key> keyParser;

        private readonly IParser<ExtMedia, Rendition> mediaParser;

        private readonly IParser<ExtMediaSequence, int> mediaSequenceParser;

        private readonly IParser<ExtStreamInf, StreamInfo> streamInfParser;

        private readonly IParser<ExtTargetDuration, TimeSpan> targetDurationParser;

        private readonly IParser<ExtVersion, int> versionParser;

        private int discontinuitySequence;

        private Key key;

        private MediaSegment mediaSegment;

        private int sequence;

        private VariantStream variantStream;

        private List<VariantStream> variantStreams { get; set; } = new List<VariantStream>();

        private List<MediaSegment> mediaSegments { get; set; } = new List<MediaSegment>();

        private List<IntraFrameStreamInfo> intraFrameStreamsInfo { get; set; } = new List<IntraFrameStreamInfo>();

        private List<Rendition> renditions { get; set; } = new List<Rendition>();


        public PlaylistWalker(
            IParser<ExtVersion, int> versionParser,
            IParser<ExtTargetDuration, TimeSpan> targetDurationParser,
            IParser<ExtMediaSequence, int> mediaSequenceParser,
            IParser<ExtKey, Key> keyParser,
            IParser<ExtStreamInf, StreamInfo> streamInfParser,
            IParser<ExtInf, Tuple<TimeSpan, string>> infoParser,
            IParser<ExtIFrameStreamInf, IntraFrameStreamInfo> iframeStreamInfParser,
            IParser<ExtDiscontinuitySequence, int> discontinuitySequenceParser,
            IParser<ExtMedia, Rendition> mediaParser)
        {
            this.versionParser = versionParser;
            this.targetDurationParser = targetDurationParser;
            this.mediaSequenceParser = mediaSequenceParser;
            this.keyParser = keyParser;
            this.streamInfParser = streamInfParser;
            this.infoParser = infoParser;
            this.iframeStreamInfParser = iframeStreamInfParser;
            this.discontinuitySequenceParser = discontinuitySequenceParser;
            this.mediaParser = mediaParser;
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
        }

        public void Enter(ExtDiscontinuitySequence discontinuitySequence)
        {
            if (Result.PlaylistType == PlaylistType.Unknown)
            {
                Result.PlaylistType = PlaylistType.Media;
            }
            else if (Result.PlaylistType == PlaylistType.Master)
            {
                throw new InvalidOperationException("EXT-X-DISCONTINUITY-SEQUENCE is not valid in a Master Playlist");
            }
            if (mediaSegment != null)
            {
                throw new InvalidOperationException(
                    "The EXT-X-DISCONTINUITY-SEQUENCE tag MUST appear before the first Media Segment in the Playlist.");
            }
            if (this.discontinuitySequence != 0)
            {
                throw new InvalidOperationException(
                    "The EXT-X-DISCONTINUITY-SEQUENCE tag MUST appear before any EXT-X-DISCONTINUITY tag.");
            }
        }

        public void Enter(ExtDiscontinuity discontinuity)
        {
            if (Result.PlaylistType == PlaylistType.Unknown)
            {
                Result.PlaylistType = PlaylistType.Media;
            }
            else if (Result.PlaylistType == PlaylistType.Master)
            {
                throw new InvalidOperationException("EXT-X-DISCONTINUITY is not valid in a Master Playlist");
            }
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

        public void Enter(ExtMedia media)
        {
            if (Result.PlaylistType == PlaylistType.Unknown)
            {
                Result.PlaylistType = PlaylistType.Master;
            }
            else if (Result.PlaylistType == PlaylistType.Media)
            {
                throw new InvalidOperationException("EXT-X-MEDIA is not valid in a Media Playlist");
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

        public void Enter(ExtIFrameStreamInf iFrameStreamInf)
        {
            if (Result.PlaylistType == PlaylistType.Unknown)
            {
                Result.PlaylistType = PlaylistType.Master;
            }
            else if (Result.PlaylistType == PlaylistType.Media)
            {
                throw new InvalidOperationException("EXT-X-I-FRAME-STREAM-INF cannot appear in a Media Playlist.");
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
            Result.MediaSegments = mediaSegments;
            Result.VariantStreams = variantStreams;
            Result.IntraFrameStreamsInfo = intraFrameStreamsInfo;
        }

        public void Exit(UriReference uri)
        {
            switch (Result.PlaylistType)
            {
                case PlaylistType.Master:
                    variantStreams.Add(variantStream);
                    if (variantStream.StreamInfo.Audio != null)
                    {
                        variantStream.StreamInfo.AlternativeAudio = renditions.FindAll(x => (x.Type == MediaType.Audio) && (x.GroupId == variantStream.StreamInfo.Audio));
                    }
                    if (variantStream.StreamInfo.Video != null)
                    {
                        variantStream.StreamInfo.AlternativeVideo = renditions.FindAll(x => (x.Type == MediaType.Video) && (x.GroupId == variantStream.StreamInfo.Video));
                    }
                    if (variantStream.StreamInfo.Subtitles != null)
                    {
                        variantStream.StreamInfo.AlternativeSubtitles = renditions.FindAll(x => (x.Type == MediaType.Subtitles) && (x.GroupId == variantStream.StreamInfo.Subtitles));
                    }
                    if (variantStream.StreamInfo.ClosedCaptions != null)
                    {
                        variantStream.StreamInfo.AlternativeClosedCaptions = renditions.FindAll(x => (x.Type == MediaType.Video) && (x.GroupId == variantStream.StreamInfo.ClosedCaptions));
                    }
                    break;
                case PlaylistType.Media:
                    mediaSegment.Sequence = sequence++;
                    mediaSegment.DiscontinuitySequence = discontinuitySequence;
                    mediaSegment.Key = key;
                    mediaSegments.Add(mediaSegment);
                    break;
                case PlaylistType.Unknown:
                    throw new InvalidOperationException("A URI-line MUST NOT appear before the first playlist tag.");
            }
        }

        public bool Walk(ExtVersion version)
        {
            Result.Version = versionParser.Parse(version);
            return false;
        }

        public bool Walk(ExtIFrameStreamInf iFrameStreamInf)
        {
            var streamInfo = iframeStreamInfParser.Parse(iFrameStreamInf);
            if (streamInfo.Video != null)
            {
                streamInfo.AlternativeVideo = renditions.FindAll(x => (x.Type == MediaType.Video) && (x.GroupId == streamInfo.Video));
            }
            intraFrameStreamsInfo.Add(streamInfo);
            return false;
        }

        public bool Walk(ExtTargetDuration targetDuration)
        {
            Result.TargetDuration = targetDurationParser.Parse(targetDuration);
            return false;
        }

        public bool Walk(ExtDiscontinuitySequence discontinuitySequence)
        {
            this.discontinuitySequence = discontinuitySequenceParser.Parse(discontinuitySequence);
            return false;
        }

        public bool Walk(ExtDiscontinuity discontinuity)
        {
            discontinuitySequence++;
            return false;
        }

        public bool Walk(ExtMedia media)
        {
            renditions.Add(mediaParser.Parse(media));
            return false;
        }

        public bool Walk(UriReference uri)
        {
            switch (Result.PlaylistType)
            {
                case PlaylistType.Master:
                    variantStream.Uri = new System.Uri(uri.Text, UriKind.RelativeOrAbsolute);
                    break;
                case PlaylistType.Media:
                    mediaSegment.Uri = new System.Uri(uri.Text, UriKind.RelativeOrAbsolute);
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
