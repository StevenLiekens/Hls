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
    public class LineLexerFactory : ILexerFactory<Line>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly ILexer<Comment> commentLexer;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexer<Empty> emptyLexer;

        private readonly ILexer<EndOfLine> endOfLineLexer;

        private readonly ILexer<Tag> tagLexer;

        private readonly ILexer<UriReference> uriReferenceLexer;

        public LineLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            IAlternationLexerFactory alternationLexerFactory,
            ILexer<Tag> tagLexer,
            ILexer<Comment> commentLexer,
            ILexer<Empty> emptyLexer,
            ILexer<UriReference> uriReferenceLexer,
            ILexer<EndOfLine> endOfLineLexer)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (tagLexer == null)
            {
                throw new ArgumentNullException(nameof(tagLexer));
            }
            if (commentLexer == null)
            {
                throw new ArgumentNullException(nameof(commentLexer));
            }
            if (emptyLexer == null)
            {
                throw new ArgumentNullException(nameof(emptyLexer));
            }
            if (uriReferenceLexer == null)
            {
                throw new ArgumentNullException(nameof(uriReferenceLexer));
            }
            if (endOfLineLexer == null)
            {
                throw new ArgumentNullException(nameof(endOfLineLexer));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.alternationLexerFactory = alternationLexerFactory;
            this.tagLexer = tagLexer;
            this.commentLexer = commentLexer;
            this.emptyLexer = emptyLexer;
            this.uriReferenceLexer = uriReferenceLexer;
            this.endOfLineLexer = endOfLineLexer;
        }

        public ILexer<Line> Create()
        {
            return
                new LineLexer(
                    concatenationLexerFactory.Create(
                        alternationLexerFactory.Create(tagLexer, commentLexer, emptyLexer, uriReferenceLexer),
                        endOfLineLexer));
        }
    }
}
