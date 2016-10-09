using Txt.Core;

namespace Hls.hexadecimal_sequence
{
    public class HexadecimalSequenceParserFactory : ParserFactory<HexadecimalSequence, byte[]>
    {
        static HexadecimalSequenceParserFactory()
        {
            Default = new HexadecimalSequenceParserFactory();
        }

        public static IParserFactory<HexadecimalSequence, byte[]> Default { get; }

        public override IParser<HexadecimalSequence, byte[]> Create()
        {
            return new HexadecimalSequenceParser();
        }
    }
}
