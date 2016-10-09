using Txt.Core;
using Txt.ABNF;
using Txt.ABNF.Core.VCHAR;
using Txt.ABNF.Core.WSP;
using Xunit;

namespace Hls.comment
{
    public class CommentLexerTest
    {
        [Theory]
        [InlineData("# Hello")]
        public void Read(string expected)
        {
            var factory = new CommentLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                RepetitionLexerFactory.Default,
                AlternationLexerFactory.Default,
                ValueRangeLexerFactory.Default,
                OptionLexerFactory.Default, VisibleCharacterLexerFactory.Default, WhiteSpaceLexerFactory.Default);
            using (var ts = new StringTextSource(expected))
            using (var scanner = new TextScanner(ts))
            {
                var sut = factory.Create();
                var actual = sut.Read(scanner).Text;
                Assert.Equal(expected, actual);
            }
        }
    }
}
