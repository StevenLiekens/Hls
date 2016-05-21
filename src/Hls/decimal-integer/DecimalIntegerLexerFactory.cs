﻿using System;
using Txt.Core;
using Txt.ABNF;
using Txt.ABNF.Core.DIGIT;

namespace Hls.decimal_integer
{
    public class DecimalIntegerLexerFactory : ILexerFactory<DecimalInteger>
    {
        private readonly ILexerFactory<Digit> digitLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        public DecimalIntegerLexerFactory(
            IRepetitionLexerFactory repetitionLexerFactory,
            ILexerFactory<Digit> digitLexerFactory)
        {
            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }
            if (digitLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(digitLexerFactory));
            }
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.digitLexerFactory = digitLexerFactory;
        }

        public ILexer<DecimalInteger> Create()
        {
            return new DecimalIntegerLexer(repetitionLexerFactory.Create(digitLexerFactory.Create(), 1, 20));
        }
    }
}
