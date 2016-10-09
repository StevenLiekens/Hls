using System;
using Hls.comment;
using Hls.empty;
using Hls.EOL;
using Hls.tag;
using Txt.ABNF;
using Txt.Core;
using UriSyntax.URI_reference;

namespace Hls.line
{
    public class LineLexerFactory : LexerFactory<Line>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly ILexerFactory<Comment> commentLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<Empty> emptyLexerFactory;

        private readonly ILexerFactory<EndOfLine> endOfLineLexerFactory;

        private readonly ILexerFactory<Tag> tagLexerFactory;

        private readonly ILexerFactory<UriReference> uriReferenceLexerFactory;

        static LineLexerFactory()
        {
            Default = new LineLexerFactory(
                ConcatenationLexerFactory.Default,
                AlternationLexerFactory.Default,
                TagLexerFactory.Default.Singleton(),
                CommentLexerFactory.Default.Singleton(),
                EmptyLexerFactory.Default.Singleton(),
                UriReferenceLexerFactory.Default.Singleton(),
                EndOfLineLexerFactory.Default.Singleton());
        }

        public LineLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            IAlternationLexerFactory alternationLexerFactory,
            ILexerFactory<Tag> tagLexerFactory,
            ILexerFactory<Comment> commentLexerFactory,
            ILexerFactory<Empty> emptyLexerFactory,
            ILexerFactory<UriReference> uriReferenceLexerFactory,
            ILexerFactory<EndOfLine> endOfLineLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (tagLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(tagLexerFactory));
            }
            if (commentLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(commentLexerFactory));
            }
            if (emptyLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(emptyLexerFactory));
            }
            if (uriReferenceLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(uriReferenceLexerFactory));
            }
            if (endOfLineLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(endOfLineLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.alternationLexerFactory = alternationLexerFactory;
            this.tagLexerFactory = tagLexerFactory;
            this.commentLexerFactory = commentLexerFactory;
            this.emptyLexerFactory = emptyLexerFactory;
            this.uriReferenceLexerFactory = uriReferenceLexerFactory;
            this.endOfLineLexerFactory = endOfLineLexerFactory;
        }

        public static LineLexerFactory Default { get; }

        public override ILexer<Line> Create()
        {
            return
                new LineLexer(
                    concatenationLexerFactory.Create(
                        alternationLexerFactory.Create(
                            tagLexerFactory.Create(),
                            commentLexerFactory.Create(),
                            emptyLexerFactory.Create(),
                            uriReferenceLexerFactory.Create()),
                        endOfLineLexerFactory.Create()));
        }
    }
}
