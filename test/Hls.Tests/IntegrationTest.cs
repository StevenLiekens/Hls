using System;
using Xunit;

namespace Hls.Tests
{
    public class IntegrationTest
    {
        [Fact]
        public void SimpleMediaPlaylist()
        {
            var data = @"#EXTM3U
#EXT-X-TARGETDURATION:10
#EXTINF:9.009,
http://media.example.com/first.ts
#EXTINF:9.009,
http://media.example.com/second.ts
#EXTINF:3.003,
http://media.example.com/third.ts
#EXT-X-ENDLIST
";
            var parser = Hls.CreateDefault();
            var result = parser.Parse(data);
            Assert.Equal(TimeSpan.FromSeconds(10), result.TargetDuration);
            Assert.Collection(result.MediaSegments,
                first =>
                {
                    Assert.Equal(new TimeSpan(0, 0, 0, 9, 9), first.Duration);
                    Assert.Null(first.Title);
                    Assert.Equal("http://media.example.com/first.ts", first.Location.ToString());
                },
                second =>
                {
                    Assert.Equal(new TimeSpan(0, 0, 0, 9, 9), second.Duration);
                    Assert.Null(second.Title);
                    Assert.Equal("http://media.example.com/second.ts", second.Location.ToString());
                },
                third =>
                {
                    Assert.Equal(new TimeSpan(0, 0, 0, 3, 3), third.Duration);
                    Assert.Null(third.Title);
                    Assert.Equal("http://media.example.com/third.ts", third.Location.ToString());
                });
        }

        [Fact]
        public void LiveMediaPlaylistUsingHttps()
        {
            var data = @"#EXTM3U
#EXT-X-VERSION:3
#EXT-X-TARGETDURATION:8
#EXT-X-MEDIA-SEQUENCE:2680

#EXTINF:7.975,
https://priv.example.com/fileSequence2680.ts
#EXTINF:7.941,
https://priv.example.com/fileSequence2681.ts
#EXTINF:7.975,
https://priv.example.com/fileSequence2682.ts
";
            var parser = Hls.CreateDefault();
            var result = parser.Parse(data);
            Assert.Equal(TimeSpan.FromSeconds(8), result.TargetDuration);
            Assert.Collection(
                result.MediaSegments,
                fileSequence2680 =>
                {
                    Assert.Equal(new TimeSpan(0, 0, 0, 7, 975), fileSequence2680.Duration);
                    Assert.Null(fileSequence2680.Title);
                    Assert.Equal("https://priv.example.com/fileSequence2680.ts", fileSequence2680.Location.ToString());
                    Assert.Equal(2680, fileSequence2680.Sequence);
                },
                fileSequence2681 =>
                {
                    Assert.Equal(new TimeSpan(0, 0, 0, 7, 941), fileSequence2681.Duration);
                    Assert.Null(fileSequence2681.Title);
                    Assert.Equal("https://priv.example.com/fileSequence2681.ts", fileSequence2681.Location.ToString());
                    Assert.Equal(2681, fileSequence2681.Sequence);
                },
                fileSequence2682 =>
                {
                    Assert.Equal(new TimeSpan(0, 0, 0, 7, 975), fileSequence2682.Duration);
                    Assert.Null(fileSequence2682.Title);
                    Assert.Equal("https://priv.example.com/fileSequence2682.ts", fileSequence2682.Location.ToString());
                    Assert.Equal(2682, fileSequence2682.Sequence);
                });
        }

        [Fact]
        public void PlaylistWithEncryptedMediaSegments()
        {
            var data = @"#EXTM3U
#EXT-X-VERSION:3
#EXT-X-MEDIA-SEQUENCE:7794
#EXT-X-TARGETDURATION:15

#EXT-X-KEY:METHOD=AES-128,URI=""https://priv.example.com/key.php?r=52""

#EXTINF:2.833,
http://media.example.com/fileSequence52-A.ts
#EXTINF:15.0,
http://media.example.com/fileSequence52-B.ts
#EXTINF:13.333,
http://media.example.com/fileSequence52-C.ts

#EXT-X-KEY:METHOD=AES-128,URI=""https://priv.example.com/key.php?r=53""

#EXTINF:15.0,
http://media.example.com/fileSequence53-A.ts
";
            var parser = Hls.CreateDefault();
            var result = parser.Parse(data);
            Assert.NotNull(result);
        }

        [Fact]
        public void MasterPlaylist()
        {
            var data = @"#EXTM3U
#EXT-X-STREAM-INF:BANDWIDTH=1280000,AVERAGE-BANDWIDTH=1000000
http://example.com/low.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=2560000,AVERAGE-BANDWIDTH=2000000
http://example.com/mid.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=7680000,AVERAGE-BANDWIDTH=6000000
http://example.com/hi.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=65000,CODECS=""mp4a.40.5""
http://example.com/audio-only.m3u8
";
            var parser = Hls.CreateDefault();
            var result = parser.Parse(data);
            Assert.NotNull(result);
        }

        [Fact]
        public void MasterPlaylistWithIFrames()
        {
            var data = @"#EXTM3U
#EXT-X-STREAM-INF:BANDWIDTH=1280000
low/audio-video.m3u8
#EXT-X-I-FRAME-STREAM-INF:BANDWIDTH=86000,URI=""low/iframe.m3u8""
#EXT-X-STREAM-INF:BANDWIDTH=2560000
mid/audio-video.m3u8
#EXT-X-I-FRAME-STREAM-INF:BANDWIDTH=150000,URI=""mid/iframe.m3u8""
#EXT-X-STREAM-INF:BANDWIDTH=7680000
hi/audio-video.m3u8
#EXT-X-I-FRAME-STREAM-INF:BANDWIDTH=550000,URI=""hi/iframe.m3u8""
#EXT-X-STREAM-INF:BANDWIDTH=65000,CODECS=""mp4a.40.5""
audio-only.m3u8
";
            var parser = Hls.CreateDefault();
            var result = parser.Parse(data);
            Assert.NotNull(result);
        }

        [Fact]
        public void MasterPlaylistWithAlternativeAudio()
        {
            var data = @"#EXTM3U
#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=""aac"",NAME=""English"",DEFAULT=YES,AUTOSELECT=YES,LANGUAGE=""en"",URI=""main/english-audio.m3u8""
#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=""aac"",NAME=""Deutsch"",DEFAULT=NO,AUTOSELECT=YES,LANGUAGE=""de"",URI=""main/german-audio.m3u8""
#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=""aac"",NAME=""Commentary"",DEFAULT=NO,AUTOSELECT=NO,LANGUAGE=""en"",URI=""commentary/audio-only.m3u8""
#EXT-X-STREAM-INF:BANDWIDTH=1280000,CODECS=""..."",AUDIO=""aac""
low/video-only.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=2560000,CODECS=""..."",AUDIO=""aac""
mid/video-only.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=7680000,CODECS=""..."",AUDIO=""aac""
hi/video-only.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=65000,CODECS=""mp4a.40.5"",AUDIO=""aac""
main/english-audio.m3u8
";
            var parser = Hls.CreateDefault();
            var result = parser.Parse(data);
            Assert.NotNull(result);
        }

        [Fact]
        public void MasterPlaylistWithAlternativeVideo()
        {
            var data = @"#EXTM3U
#EXT-X-MEDIA:TYPE=VIDEO,GROUP-ID=""low"",NAME=""Main"",DEFAULT=YES,URI=""low/main/audio-video.m3u8""
#EXT-X-MEDIA:TYPE=VIDEO,GROUP-ID=""low"",NAME=""Centerfield"",DEFAULT=NO,URI=""low/centerfield/audio-video.m3u8""
#EXT-X-MEDIA:TYPE=VIDEO,GROUP-ID=""low"",NAME=""Dugout"",DEFAULT=NO,URI=""low/dugout/audio-video.m3u8""

#EXT-X-STREAM-INF:BANDWIDTH=1280000,CODECS=""..."",VIDEO=""low""
low/main/audio-video.m3u8

#EXT-X-MEDIA:TYPE=VIDEO,GROUP-ID=""mid"",NAME=""Main"",DEFAULT=YES,URI=""mid/main/audio-video.m3u8""
#EXT-X-MEDIA:TYPE=VIDEO,GROUP-ID=""mid"",NAME=""Centerfield"",DEFAULT=NO,URI=""mid/centerfield/audio-video.m3u8""
#EXT-X-MEDIA:TYPE=VIDEO,GROUP-ID=""mid"",NAME=""Dugout"",DEFAULT=NO,URI=""mid/dugout/audio-video.m3u8""

#EXT-X-STREAM-INF:BANDWIDTH=2560000,CODECS=""..."",VIDEO=""mid""
mid/main/audio-video.m3u8

#EXT-X-MEDIA:TYPE=VIDEO,GROUP-ID=""hi"",NAME=""Main"",DEFAULT=YES,URI=""hi/main/audio-video.m3u8""
#EXT-X-MEDIA:TYPE=VIDEO,GROUP-ID=""hi"",NAME=""Centerfield"",DEFAULT=NO,URI=""hi/centerfield/audio-video.m3u8""
#EXT-X-MEDIA:TYPE=VIDEO,GROUP-ID=""hi"",NAME=""Dugout"",DEFAULT=NO,URI=""hi/dugout/audio-video.m3u8""

#EXT-X-STREAM-INF:BANDWIDTH=7680000,CODECS=""..."",VIDEO=""hi""
hi/main/audio-video.m3u8
";
            var parser = Hls.CreateDefault();
            var result = parser.Parse(data);
            Assert.NotNull(result);
        }
    }
}