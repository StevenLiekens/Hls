using System;
using Hls.EXTINF;
using Hls.EXT_X_ENDLIST;
using Hls.EXT_X_I_FRAME_STREAM_INF;
using Hls.EXT_X_KEY;
using Hls.EXT_X_MEDIA;
using Hls.EXT_X_MEDIA_SEQUENCE;
using Hls.EXT_X_STREAM_INF;
using Hls.EXT_X_TARGETDURATION;
using Hls.EXT_X_VERSION;
using Hls.ignored_tag;
using Txt.ABNF;
using Txt.Core;

namespace Hls.tag
{
    public class TagLexerFactory : ILexerFactory<Tag>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly ILexer<ExtEndList> extEndListLexer;

        private readonly ILexer<ExtIFrameStreamInf> extIFrameStreamInfLexer;

        private readonly ILexer<ExtInf> extInfLexer;

        private readonly ILexer<ExtKey> extKeyLexer;

        private readonly ILexer<ExtMedia> extMediaLexer;

        private readonly ILexer<ExtMediaSequence> extMediaSequenceLexer;

        private readonly ILexer<ExtStreamInf> extStreamInfLexer;

        private readonly ILexer<ExtTargetDuration> extTargetDurationLexer;

        private readonly ILexer<ExtVersion> extVersionLexer;

        private readonly ILexer<IgnoredTag> ignoredTagLexer;

        public TagLexerFactory(
            IAlternationLexerFactory alternationLexerFactory,
            ILexer<ExtVersion> extVersionLexer,
            ILexer<ExtInf> extInfLexer,
            ILexer<ExtTargetDuration> extTargetDurationLexer,
            ILexer<ExtStreamInf> extStreamInfLexer,
            ILexer<ExtMediaSequence> extMediaSequenceLexer,
            ILexer<IgnoredTag> ignoredTagLexer,
            ILexer<ExtEndList> extEndListLexer,
            ILexer<ExtMedia> extMediaLexer,
            ILexer<ExtIFrameStreamInf> extIFrameStreamInfLexer,
            ILexer<ExtKey> extKeyLexer)
        {
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (extVersionLexer == null)
            {
                throw new ArgumentNullException(nameof(extVersionLexer));
            }
            if (extInfLexer == null)
            {
                throw new ArgumentNullException(nameof(extInfLexer));
            }
            if (extTargetDurationLexer == null)
            {
                throw new ArgumentNullException(nameof(extTargetDurationLexer));
            }
            if (extStreamInfLexer == null)
            {
                throw new ArgumentNullException(nameof(extStreamInfLexer));
            }
            if (extMediaSequenceLexer == null)
            {
                throw new ArgumentNullException(nameof(extMediaSequenceLexer));
            }
            if (ignoredTagLexer == null)
            {
                throw new ArgumentNullException(nameof(ignoredTagLexer));
            }
            if (extEndListLexer == null)
            {
                throw new ArgumentNullException(nameof(extEndListLexer));
            }
            if (extMediaLexer == null)
            {
                throw new ArgumentNullException(nameof(extMediaLexer));
            }
            if (extIFrameStreamInfLexer == null)
            {
                throw new ArgumentNullException(nameof(extIFrameStreamInfLexer));
            }
            if (extKeyLexer == null)
            {
                throw new ArgumentNullException(nameof(extKeyLexer));
            }
            this.alternationLexerFactory = alternationLexerFactory;
            this.extVersionLexer = extVersionLexer;
            this.extInfLexer = extInfLexer;
            this.extTargetDurationLexer = extTargetDurationLexer;
            this.extStreamInfLexer = extStreamInfLexer;
            this.extMediaSequenceLexer = extMediaSequenceLexer;
            this.ignoredTagLexer = ignoredTagLexer;
            this.extEndListLexer = extEndListLexer;
            this.extMediaLexer = extMediaLexer;
            this.extIFrameStreamInfLexer = extIFrameStreamInfLexer;
            this.extKeyLexer = extKeyLexer;
        }

        public ILexer<Tag> Create()
        {
            return new TagLexer(
                alternationLexerFactory.Create(
                    extVersionLexer,
                    extKeyLexer,
                    extInfLexer,
                    extTargetDurationLexer,
                    extStreamInfLexer,
                    extIFrameStreamInfLexer,
                    extMediaSequenceLexer,
                    extEndListLexer,
                    extMediaLexer,
                    ignoredTagLexer));
        }
    }
}
