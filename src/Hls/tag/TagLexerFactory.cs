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
    public class TagLexerFactory : LexerFactory<Tag>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly ILexerFactory<ExtEndList> extEndListLexerFactory;

        private readonly ILexerFactory<ExtIFrameStreamInf> extIFrameStreamInfLexerFactory;

        private readonly ILexerFactory<ExtInf> extInfLexerFactory;

        private readonly ILexerFactory<ExtKey> extKeyLexerFactory;

        private readonly ILexerFactory<ExtMedia> extMediaLexerFactory;

        private readonly ILexerFactory<ExtMediaSequence> extMediaSequenceLexerFactory;

        private readonly ILexerFactory<ExtStreamInf> extStreamInfLexerFactory;

        private readonly ILexerFactory<ExtTargetDuration> extTargetDurationLexerFactory;

        private readonly ILexerFactory<ExtVersion> extVersionLexerFactory;

        private readonly ILexerFactory<IgnoredTag> ignoredTagLexerFactory;

        static TagLexerFactory()
        {
            Default = new TagLexerFactory(
                AlternationLexerFactory.Default,
                ExtVersionLexerFactory.Default.Singleton(),
                ExtInfLexerFactory.Default.Singleton(),
                ExtTargetDurationLexerFactory.Default.Singleton(),
                ExtStreamInfLexerFactory.Default.Singleton(),
                ExtMediaSequenceLexerFactory.Default.Singleton(),
                IgnoredTagLexerFactory.Default.Singleton(),
                ExtEndListLexerFactory.Default.Singleton(),
                ExtMediaLexerFactory.Default.Singleton(),
                ExtIFrameStreamInfLexerFactory.Default.Singleton(),
                ExtKeyLexerFactory.Default.Singleton());
        }

        public TagLexerFactory(
            IAlternationLexerFactory alternationLexerFactory,
            ILexerFactory<ExtVersion> extVersionLexerFactory,
            ILexerFactory<ExtInf> extInfLexerFactory,
            ILexerFactory<ExtTargetDuration> extTargetDurationLexerFactory,
            ILexerFactory<ExtStreamInf> extStreamInfLexerFactory,
            ILexerFactory<ExtMediaSequence> extMediaSequenceLexerFactory,
            ILexerFactory<IgnoredTag> ignoredTagLexerFactory,
            ILexerFactory<ExtEndList> extEndListLexerFactory,
            ILexerFactory<ExtMedia> extMediaLexerFactory,
            ILexerFactory<ExtIFrameStreamInf> extIFrameStreamInfLexerFactory,
            ILexerFactory<ExtKey> extKeyLexerFactory)
        {
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (extVersionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(extVersionLexerFactory));
            }
            if (extInfLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(extInfLexerFactory));
            }
            if (extTargetDurationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(extTargetDurationLexerFactory));
            }
            if (extStreamInfLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(extStreamInfLexerFactory));
            }
            if (extMediaSequenceLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(extMediaSequenceLexerFactory));
            }
            if (ignoredTagLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(ignoredTagLexerFactory));
            }
            if (extEndListLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(extEndListLexerFactory));
            }
            if (extMediaLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(extMediaLexerFactory));
            }
            if (extIFrameStreamInfLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(extIFrameStreamInfLexerFactory));
            }
            if (extKeyLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(extKeyLexerFactory));
            }
            this.alternationLexerFactory = alternationLexerFactory;
            this.extVersionLexerFactory = extVersionLexerFactory;
            this.extInfLexerFactory = extInfLexerFactory;
            this.extTargetDurationLexerFactory = extTargetDurationLexerFactory;
            this.extStreamInfLexerFactory = extStreamInfLexerFactory;
            this.extMediaSequenceLexerFactory = extMediaSequenceLexerFactory;
            this.ignoredTagLexerFactory = ignoredTagLexerFactory;
            this.extEndListLexerFactory = extEndListLexerFactory;
            this.extMediaLexerFactory = extMediaLexerFactory;
            this.extIFrameStreamInfLexerFactory = extIFrameStreamInfLexerFactory;
            this.extKeyLexerFactory = extKeyLexerFactory;
        }

        public static TagLexerFactory Default { get; }

        public override ILexer<Tag> Create()
        {
            return new TagLexer(
                alternationLexerFactory.Create(
                    extVersionLexerFactory.Create(),
                    extKeyLexerFactory.Create(),
                    extInfLexerFactory.Create(),
                    extTargetDurationLexerFactory.Create(),
                    extStreamInfLexerFactory.Create(),
                    extIFrameStreamInfLexerFactory.Create(),
                    extMediaSequenceLexerFactory.Create(),
                    extEndListLexerFactory.Create(),
                    extMediaLexerFactory.Create(),
                    ignoredTagLexerFactory.Create()));
        }
    }
}
