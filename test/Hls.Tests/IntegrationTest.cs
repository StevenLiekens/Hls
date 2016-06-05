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
                    Assert.Equal("http://media.example.com/first.ts", first.Uri.ToString());
                },
                second =>
                {
                    Assert.Equal(new TimeSpan(0, 0, 0, 9, 9), second.Duration);
                    Assert.Null(second.Title);
                    Assert.Equal("http://media.example.com/second.ts", second.Uri.ToString());
                },
                third =>
                {
                    Assert.Equal(new TimeSpan(0, 0, 0, 3, 3), third.Duration);
                    Assert.Null(third.Title);
                    Assert.Equal("http://media.example.com/third.ts", third.Uri.ToString());
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
            Assert.Equal(3, result.Version);
            Assert.Equal(TimeSpan.FromSeconds(8), result.TargetDuration);
            Assert.Collection(
                result.MediaSegments,
                fileSequence2680 =>
                {
                    Assert.Equal(TimeSpan.FromSeconds(7.975), fileSequence2680.Duration);
                    Assert.Null(fileSequence2680.Title);
                    Assert.Equal("https://priv.example.com/fileSequence2680.ts", fileSequence2680.Uri.ToString());
                    Assert.Equal(2680, fileSequence2680.Sequence);
                },
                fileSequence2681 =>
                {
                    Assert.Equal(TimeSpan.FromSeconds(7.941), fileSequence2681.Duration);
                    Assert.Null(fileSequence2681.Title);
                    Assert.Equal("https://priv.example.com/fileSequence2681.ts", fileSequence2681.Uri.ToString());
                    Assert.Equal(2681, fileSequence2681.Sequence);
                },
                fileSequence2682 =>
                {
                    Assert.Equal(TimeSpan.FromSeconds(7.975), fileSequence2682.Duration);
                    Assert.Null(fileSequence2682.Title);
                    Assert.Equal("https://priv.example.com/fileSequence2682.ts", fileSequence2682.Uri.ToString());
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
            Assert.Equal(3, result.Version);
            Assert.Equal(TimeSpan.FromSeconds(15), result.TargetDuration);
            Assert.Collection(
                result.MediaSegments,
                fileSequence52_A =>
                {
                    Assert.Equal(7794, fileSequence52_A.Sequence);
                    Assert.Null(fileSequence52_A.Title);
                    Assert.Equal(TimeSpan.FromSeconds(2.833), fileSequence52_A.Duration);
                    Assert.Equal("http://media.example.com/fileSequence52-A.ts", fileSequence52_A.Uri.ToString());
                    Assert.Equal(EncryptionMethod.AES128, fileSequence52_A.Key.Method);
                    Assert.Equal("https://priv.example.com/key.php?r=52", fileSequence52_A.Key.Uri.ToString());
                    Assert.Null(fileSequence52_A.Key.IV);
                    Assert.Equal("identity", fileSequence52_A.Key.KeyFormat);
                    Assert.Collection(
                        fileSequence52_A.Key.KeyFormatVersions,
                        version => Assert.Equal(1, version));
                },
                fileSequence52_B =>
                {
                    Assert.Equal(7795, fileSequence52_B.Sequence);
                    Assert.Null(fileSequence52_B.Title);
                    Assert.Equal(TimeSpan.FromSeconds(15.0), fileSequence52_B.Duration);
                    Assert.Equal("http://media.example.com/fileSequence52-B.ts", fileSequence52_B.Uri.ToString());
                    Assert.Equal(EncryptionMethod.AES128, fileSequence52_B.Key.Method);
                    Assert.Equal("https://priv.example.com/key.php?r=52", fileSequence52_B.Key.Uri.ToString());
                    Assert.Null(fileSequence52_B.Key.IV);
                    Assert.Equal("identity", fileSequence52_B.Key.KeyFormat);
                    Assert.Collection(
                        fileSequence52_B.Key.KeyFormatVersions,
                        version => Assert.Equal(1, version));
                },
                fileSequence52_C =>
                {
                    Assert.Equal(7796, fileSequence52_C.Sequence);
                    Assert.Null(fileSequence52_C.Title);
                    Assert.Equal(TimeSpan.FromSeconds(13.333), fileSequence52_C.Duration);
                    Assert.Equal("http://media.example.com/fileSequence52-C.ts", fileSequence52_C.Uri.ToString());
                    Assert.Equal(EncryptionMethod.AES128, fileSequence52_C.Key.Method);
                    Assert.Equal("https://priv.example.com/key.php?r=52", fileSequence52_C.Key.Uri.ToString());
                    Assert.Null(fileSequence52_C.Key.IV);
                    Assert.Equal("identity", fileSequence52_C.Key.KeyFormat);
                    Assert.Collection(
                        fileSequence52_C.Key.KeyFormatVersions,
                        version => Assert.Equal(1, version));
                },
                fileSequence53_A =>
                {
                    Assert.Equal(7797, fileSequence53_A.Sequence);
                    Assert.Null(fileSequence53_A.Title);
                    Assert.Equal(TimeSpan.FromSeconds(15.0), fileSequence53_A.Duration);
                    Assert.Equal("http://media.example.com/fileSequence53-A.ts", fileSequence53_A.Uri.ToString());
                    Assert.Equal(EncryptionMethod.AES128, fileSequence53_A.Key.Method);
                    Assert.Equal("https://priv.example.com/key.php?r=53", fileSequence53_A.Key.Uri.ToString());
                    Assert.Null(fileSequence53_A.Key.IV);
                    Assert.Equal("identity", fileSequence53_A.Key.KeyFormat);
                    Assert.Collection(
                        fileSequence53_A.Key.KeyFormatVersions,
                        version => Assert.Equal(1, version));
                });
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
            Assert.Collection(
                result.VariantStreams,
                low =>
                {
                    Assert.Equal(1280000, low.StreamInfo.Bandwidth);
                    Assert.Equal(1000000, low.StreamInfo.AverageBandwidth);
                    Assert.Empty(low.StreamInfo.Codecs);
                    Assert.Null(low.StreamInfo.Resolution);
                    Assert.Null(low.StreamInfo.Framerate);
                    Assert.Null(low.StreamInfo.Audio);
                    Assert.Null(low.StreamInfo.Video);
                    Assert.Null(low.StreamInfo.Subtitles);
                    Assert.Null(low.StreamInfo.ClosedCaptions);
                },
                mid =>
                {
                    Assert.Equal(2560000, mid.StreamInfo.Bandwidth);
                    Assert.Equal(2000000, mid.StreamInfo.AverageBandwidth);
                    Assert.Empty(mid.StreamInfo.Codecs);
                    Assert.Null(mid.StreamInfo.Resolution);
                    Assert.Null(mid.StreamInfo.Framerate);
                    Assert.Null(mid.StreamInfo.Audio);
                    Assert.Null(mid.StreamInfo.Video);
                    Assert.Null(mid.StreamInfo.Subtitles);
                    Assert.Null(mid.StreamInfo.ClosedCaptions);

                },
                hi =>
                {
                    Assert.Equal(7680000, hi.StreamInfo.Bandwidth);
                    Assert.Equal(6000000, hi.StreamInfo.AverageBandwidth);
                    Assert.Empty(hi.StreamInfo.Codecs);
                    Assert.Null(hi.StreamInfo.Resolution);
                    Assert.Null(hi.StreamInfo.Framerate);
                    Assert.Null(hi.StreamInfo.Audio);
                    Assert.Null(hi.StreamInfo.Video);
                    Assert.Null(hi.StreamInfo.Subtitles);
                    Assert.Null(hi.StreamInfo.ClosedCaptions);
                },
                audio_only =>
                {
                    Assert.Equal(65000, audio_only.StreamInfo.Bandwidth);
                    Assert.Null(audio_only.StreamInfo.AverageBandwidth);
                    Assert.Collection(
                        audio_only.StreamInfo.Codecs,
                        codec => Assert.Equal("mp4a.40.5", codec));
                    Assert.Null(audio_only.StreamInfo.Resolution);
                    Assert.Null(audio_only.StreamInfo.Framerate);
                    Assert.Null(audio_only.StreamInfo.Audio);
                    Assert.Null(audio_only.StreamInfo.Video);
                    Assert.Null(audio_only.StreamInfo.Subtitles);
                    Assert.Null(audio_only.StreamInfo.ClosedCaptions);
                });
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
            Assert.Collection(
                result.VariantStreams,
                low =>
                {
                    Assert.Equal(1280000, low.StreamInfo.Bandwidth);
                    Assert.Null(low.StreamInfo.AverageBandwidth);
                    Assert.Empty(low.StreamInfo.Codecs);
                    Assert.Null(low.StreamInfo.Resolution);
                    Assert.Null(low.StreamInfo.Framerate);
                    Assert.Null(low.StreamInfo.Audio);
                    Assert.Null(low.StreamInfo.Video);
                    Assert.Null(low.StreamInfo.Subtitles);
                    Assert.Null(low.StreamInfo.ClosedCaptions);
                },
                mid =>
                {
                    Assert.Equal(2560000, mid.StreamInfo.Bandwidth);
                    Assert.Null(mid.StreamInfo.AverageBandwidth);
                    Assert.Empty(mid.StreamInfo.Codecs);
                    Assert.Null(mid.StreamInfo.Resolution);
                    Assert.Null(mid.StreamInfo.Framerate);
                    Assert.Null(mid.StreamInfo.Audio);
                    Assert.Null(mid.StreamInfo.Video);
                    Assert.Null(mid.StreamInfo.Subtitles);
                    Assert.Null(mid.StreamInfo.ClosedCaptions);
                },
                hi =>
                {
                    Assert.Equal(7680000, hi.StreamInfo.Bandwidth);
                    Assert.Null(hi.StreamInfo.AverageBandwidth);
                    Assert.Empty(hi.StreamInfo.Codecs);
                    Assert.Null(hi.StreamInfo.Resolution);
                    Assert.Null(hi.StreamInfo.Framerate);
                    Assert.Null(hi.StreamInfo.Audio);
                    Assert.Null(hi.StreamInfo.Video);
                    Assert.Null(hi.StreamInfo.Subtitles);
                    Assert.Null(hi.StreamInfo.ClosedCaptions);

                },
                audio_only =>
                {
                    Assert.Equal(65000, audio_only.StreamInfo.Bandwidth);
                    Assert.Null(audio_only.StreamInfo.AverageBandwidth);
                    Assert.Collection(
                        audio_only.StreamInfo.Codecs,
                        codec => Assert.Equal("mp4a.40.5", codec));
                    Assert.Null(audio_only.StreamInfo.Resolution);
                    Assert.Null(audio_only.StreamInfo.Framerate);
                    Assert.Null(audio_only.StreamInfo.Audio);
                    Assert.Null(audio_only.StreamInfo.Video);
                    Assert.Null(audio_only.StreamInfo.Subtitles);
                    Assert.Null(audio_only.StreamInfo.ClosedCaptions);
                });
            Assert.Collection(
                result.IntraFrameStreamsInfo,
                low =>
                {
                    Assert.Equal(86000, low.Bandwidth);
                    Assert.Equal("low/iframe.m3u8", low.Uri.ToString());
                    Assert.Null(low.AverageBandwidth);
                    Assert.Empty(low.Codecs);
                    Assert.Null(low.Resolution);
                    Assert.Null(low.Framerate);
                    Assert.Null(low.Video);
                },
                mid =>
                {
                    Assert.Equal(150000, mid.Bandwidth);
                    Assert.Equal("mid/iframe.m3u8", mid.Uri.ToString());
                    Assert.Null(mid.AverageBandwidth);
                    Assert.Empty(mid.Codecs);
                    Assert.Null(mid.Resolution);
                    Assert.Null(mid.Framerate);
                    Assert.Null(mid.Video);
                },
                hi =>
                {
                    Assert.Equal(550000, hi.Bandwidth);
                    Assert.Equal("hi/iframe.m3u8", hi.Uri.ToString());
                    Assert.Null(hi.AverageBandwidth);
                    Assert.Empty(hi.Codecs);
                    Assert.Null(hi.Resolution);
                    Assert.Null(hi.Framerate);
                    Assert.Null(hi.Video);
                });
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