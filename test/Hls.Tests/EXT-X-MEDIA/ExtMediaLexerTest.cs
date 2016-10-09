using Txt.Core;
using Xunit;

namespace Hls.EXT_X_MEDIA
{
    public class ExtMediaLexerTest
    {
        [Theory]
        [InlineData("#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"aac\",NAME=\"English\",DEFAULT=YES,AUTOSELECT=YES,LANGUAGE=\"en\",URI=\"main/english-audio.m3u8\"")]
        public void Read(string expected)
        {
            using (var textSource = new StringTextSource(expected))
            using (var textScanner = new TextScanner(textSource))
            {
                var sut = ExtMediaLexerFactory.Default.Create();
                var actual = sut.Read(textScanner).Text;
                Assert.Equal(expected, actual);
            }
        }
    }
}
