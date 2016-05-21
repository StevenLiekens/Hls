﻿using System;
using Txt.Core;
using Txt.ABNF;
using Txt.ABNF.Core.VCHAR;
using Txt.ABNF.Core.WSP;

namespace Hls.title
{
    public class TitleLexerFactory : ILexerFactory<Title>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        private readonly ILexerFactory<VisibleCharacter> visibleCharacterLexerFactory;

        private readonly ILexerFactory<WhiteSpace> whiteSpaceLexerFactory;

        public TitleLexerFactory(
            IRepetitionLexerFactory repetitionLexerFactory,
            IAlternationLexerFactory alternationLexerFactory,
            ILexerFactory<VisibleCharacter> visibleCharacterLexerFactory,
            ILexerFactory<WhiteSpace> whiteSpaceLexerFactory)
        {
            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (visibleCharacterLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(visibleCharacterLexerFactory));
            }
            if (whiteSpaceLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(whiteSpaceLexerFactory));
            }
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.alternationLexerFactory = alternationLexerFactory;
            this.visibleCharacterLexerFactory = visibleCharacterLexerFactory;
            this.whiteSpaceLexerFactory = whiteSpaceLexerFactory;
        }

        public ILexer<Title> Create()
        {
            return
                new TitleLexer(
                    repetitionLexerFactory.Create(
                        alternationLexerFactory.Create(
                            visibleCharacterLexerFactory.Create(),
                            whiteSpaceLexerFactory.Create()),
                        1,
                        int.MaxValue));
        }
    }
}
