using System;
using Hls.decimal_floating_point;
using Hls.decimal_integer;
using Hls.decimal_resolution;
using Hls.enumerated_string;
using Hls.hexadecimal_sequence;
using Hls.quoted_string;
using Hls.signed_decimal_floating_point;
using Txt.Core;
using Txt.ABNF;

namespace Hls.attribute_value
{
    public class AttributeValueLexerFactory : ILexerFactory<AttributeValue>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly ILexerFactory<DecimalFloatingPoint> decimalFloatingPointLexerFactory;

        private readonly ILexerFactory<DecimalInteger> decimalIntegerLexerFactory;

        private readonly ILexerFactory<DecimalResolution> decimalResolutionLexerFactory;

        private readonly ILexerFactory<EnumeratedString> enumeratedStringLexerFactory;

        private readonly ILexerFactory<HexadecimalSequence> hexadecimalSequenceLexerFactory;

        private readonly ILexerFactory<QuotedString> quotedStringLexerFactory;

        private readonly ILexerFactory<SignedDecimalFloatingPoint> signedDecimalFloatingPointLexerFactory;

        public AttributeValueLexerFactory(
            IAlternationLexerFactory alternationLexerFactory,
            ILexerFactory<DecimalInteger> decimalIntegerLexerFactory,
            ILexerFactory<HexadecimalSequence> hexadecimalSequenceLexerFactory,
            ILexerFactory<DecimalFloatingPoint> decimalFloatingPointLexerFactory,
            ILexerFactory<SignedDecimalFloatingPoint> signedDecimalFloatingPointLexerFactory,
            ILexerFactory<QuotedString> quotedStringLexerFactory,
            ILexerFactory<EnumeratedString> enumeratedStringLexerFactory,
            ILexerFactory<DecimalResolution> decimalResolutionLexerFactory)
        {
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (decimalIntegerLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalIntegerLexerFactory));
            }
            if (hexadecimalSequenceLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(hexadecimalSequenceLexerFactory));
            }
            if (decimalFloatingPointLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalFloatingPointLexerFactory));
            }
            if (signedDecimalFloatingPointLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(signedDecimalFloatingPointLexerFactory));
            }
            if (quotedStringLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(quotedStringLexerFactory));
            }
            if (enumeratedStringLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(enumeratedStringLexerFactory));
            }
            if (decimalResolutionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalResolutionLexerFactory));
            }
            this.alternationLexerFactory = alternationLexerFactory;
            this.decimalIntegerLexerFactory = decimalIntegerLexerFactory;
            this.hexadecimalSequenceLexerFactory = hexadecimalSequenceLexerFactory;
            this.decimalFloatingPointLexerFactory = decimalFloatingPointLexerFactory;
            this.signedDecimalFloatingPointLexerFactory = signedDecimalFloatingPointLexerFactory;
            this.quotedStringLexerFactory = quotedStringLexerFactory;
            this.enumeratedStringLexerFactory = enumeratedStringLexerFactory;
            this.decimalResolutionLexerFactory = decimalResolutionLexerFactory;
        }

        public ILexer<AttributeValue> Create()
        {
            return
                new AttributeValueLexer(
                    alternationLexerFactory.Create(
                        hexadecimalSequenceLexerFactory.Create(),
                        decimalResolutionLexerFactory.Create(),
                        decimalFloatingPointLexerFactory.Create(),
                        signedDecimalFloatingPointLexerFactory.Create(),
                        decimalIntegerLexerFactory.Create(),
                        quotedStringLexerFactory.Create(),
                        enumeratedStringLexerFactory.Create()));
        }
    }
}
