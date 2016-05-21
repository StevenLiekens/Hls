﻿using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.quoted_string
{
    public sealed class QuotedStringLexer : Lexer<QuotedString>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public QuotedStringLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<QuotedString> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<QuotedString>.FromResult(new QuotedString(result.Element));
            }
            return ReadResult<QuotedString>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
