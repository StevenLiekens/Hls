using System;
using System.Collections.Generic;
using SimpleInjector;
using Txt.ABNF;
using UriSyntax;
using Xunit;
using Registration = Txt.Core.Registration;

namespace Hls.Tests
{
    public class IntegrationTest
    {
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
            var container = new Container();
            var registrations = new List<Registration>();
            registrations.AddRange(AbnfRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(UriRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(HlsRegistrations.GetRegistrations(container.GetInstance));
            foreach (var registration in registrations)
            {
                if (registration.Implementation != null)
                    container.RegisterSingleton(registration.Service, registration.Implementation);
                else if (registration.Instance != null)
                    container.RegisterSingleton(registration.Service, registration.Instance);
                else if (registration.Factory != null)
                    container.RegisterSingleton(registration.Service, registration.Factory);
            }
            var parser = container.GetInstance<HlsParser>();
            var result = parser.Parse(data, container.GetInstance<PlaylistWalker>());
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
            var container = new Container();
            var registrations = new List<Registration>();
            registrations.AddRange(AbnfRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(UriRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(HlsRegistrations.GetRegistrations(container.GetInstance));
            foreach (var registration in registrations)
            {
                if (registration.Implementation != null)
                    container.RegisterSingleton(registration.Service, registration.Implementation);
                else if (registration.Instance != null)
                    container.RegisterSingleton(registration.Service, registration.Instance);
                else if (registration.Factory != null)
                    container.RegisterSingleton(registration.Service, registration.Factory);
            }
            var parser = container.GetInstance<HlsParser>();
            var result = parser.Parse(data, container.GetInstance<PlaylistWalker>());
            Assert.Collection(
                result.VariantStreams,
                low =>
                {
                    Assert.Equal(1280000, low.Bandwidth);
                    Assert.Equal(1000000, low.AverageBandwidth);
                    Assert.Empty(low.Codecs);
                    Assert.Null(low.Resolution);
                    Assert.Null(low.Framerate);
                    Assert.Null(low.Audio);
                    Assert.Null(low.Video);
                    Assert.Null(low.Subtitles);
                    Assert.Null(low.ClosedCaptions);
                },
                mid =>
                {
                    Assert.Equal(2560000, mid.Bandwidth);
                    Assert.Equal(2000000, mid.AverageBandwidth);
                    Assert.Empty(mid.Codecs);
                    Assert.Null(mid.Resolution);
                    Assert.Null(mid.Framerate);
                    Assert.Null(mid.Audio);
                    Assert.Null(mid.Video);
                    Assert.Null(mid.Subtitles);
                    Assert.Null(mid.ClosedCaptions);
                },
                hi =>
                {
                    Assert.Equal(7680000, hi.Bandwidth);
                    Assert.Equal(6000000, hi.AverageBandwidth);
                    Assert.Empty(hi.Codecs);
                    Assert.Null(hi.Resolution);
                    Assert.Null(hi.Framerate);
                    Assert.Null(hi.Audio);
                    Assert.Null(hi.Video);
                    Assert.Null(hi.Subtitles);
                    Assert.Null(hi.ClosedCaptions);
                },
                audio_only =>
                {
                    Assert.Equal(65000, audio_only.Bandwidth);
                    Assert.Null(audio_only.AverageBandwidth);
                    Assert.Collection(
                        audio_only.Codecs,
                        codec => Assert.Equal("mp4a.40.5", codec));
                    Assert.Null(audio_only.Resolution);
                    Assert.Null(audio_only.Framerate);
                    Assert.Null(audio_only.Audio);
                    Assert.Null(audio_only.Video);
                    Assert.Null(audio_only.Subtitles);
                    Assert.Null(audio_only.ClosedCaptions);
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
            var container = new Container();
            var registrations = new List<Registration>();
            registrations.AddRange(AbnfRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(UriRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(HlsRegistrations.GetRegistrations(container.GetInstance));
            foreach (var registration in registrations)
            {
                if (registration.Implementation != null)
                    container.RegisterSingleton(registration.Service, registration.Implementation);
                else if (registration.Instance != null)
                    container.RegisterSingleton(registration.Service, registration.Instance);
                else if (registration.Factory != null)
                    container.RegisterSingleton(registration.Service, registration.Factory);
            }
            var parser = container.GetInstance<HlsParser>();
            var result = parser.Parse(data, container.GetInstance<PlaylistWalker>());
            Assert.Collection(
                result.VariantStreams,
                low =>
                {
                    Assert.Equal(1280000, low.Bandwidth);
                    Assert.Collection(low.Codecs, codec => Assert.Equal("...", codec));
                    Assert.Equal("aac", low.Audio);
                    Assert.Equal("low/video-only.m3u8", low.Uri.ToString());
                    Assert.Collection(
                        low.AlternativeAudio,
                        english =>
                        {
                            Assert.Equal(MediaType.Audio, english.Type);
                            Assert.Equal(low.Audio, english.GroupId);
                            Assert.Equal("English", english.Name);
                            Assert.Equal(true, english.Default);
                            Assert.Equal(true, english.AutoSelect);
                            Assert.Equal("en", english.Language);
                            Assert.Equal("main/english-audio.m3u8", english.Uri.ToString());
                        },
                        german =>
                        {
                            Assert.Equal(MediaType.Audio, german.Type);
                            Assert.Equal(low.Audio, german.GroupId);
                            Assert.Equal("Deutsch", german.Name);
                            Assert.Equal(false, german.Default);
                            Assert.Equal(true, german.AutoSelect);
                            Assert.Equal("de", german.Language);
                            Assert.Equal("main/german-audio.m3u8", german.Uri.ToString());
                        },
                        commentary =>
                        {
                            Assert.Equal(MediaType.Audio, commentary.Type);
                            Assert.Equal(low.Audio, commentary.GroupId);
                            Assert.Equal("Commentary", commentary.Name);
                            Assert.Equal(false, commentary.Default);
                            Assert.Equal(false, commentary.AutoSelect);
                            Assert.Equal("en", commentary.Language);
                            Assert.Equal("commentary/audio-only.m3u8", commentary.Uri.ToString());
                        });
                },
                mid =>
                {
                    Assert.Equal(2560000, mid.Bandwidth);
                    Assert.Collection(mid.Codecs, codec => Assert.Equal("...", codec));
                    Assert.Equal("aac", mid.Audio);
                    Assert.Equal("mid/video-only.m3u8", mid.Uri.ToString());
                    Assert.Collection(
                        mid.AlternativeAudio,
                        english =>
                        {
                            Assert.Equal(MediaType.Audio, english.Type);
                            Assert.Equal(mid.Audio, english.GroupId);
                            Assert.Equal("English", english.Name);
                            Assert.Equal(true, english.Default);
                            Assert.Equal(true, english.AutoSelect);
                            Assert.Equal("en", english.Language);
                            Assert.Equal("main/english-audio.m3u8", english.Uri.ToString());
                        },
                        german =>
                        {
                            Assert.Equal(MediaType.Audio, german.Type);
                            Assert.Equal(mid.Audio, german.GroupId);
                            Assert.Equal("Deutsch", german.Name);
                            Assert.Equal(false, german.Default);
                            Assert.Equal(true, german.AutoSelect);
                            Assert.Equal("de", german.Language);
                            Assert.Equal("main/german-audio.m3u8", german.Uri.ToString());
                        },
                        commentary =>
                        {
                            Assert.Equal(MediaType.Audio, commentary.Type);
                            Assert.Equal(mid.Audio, commentary.GroupId);
                            Assert.Equal("Commentary", commentary.Name);
                            Assert.Equal(false, commentary.Default);
                            Assert.Equal(false, commentary.AutoSelect);
                            Assert.Equal("en", commentary.Language);
                            Assert.Equal("commentary/audio-only.m3u8", commentary.Uri.ToString());
                        });
                },
                hi =>
                {
                    Assert.Equal(7680000, hi.Bandwidth);
                    Assert.Collection(hi.Codecs, codec => Assert.Equal("...", codec));
                    Assert.Equal("aac", hi.Audio);
                    Assert.Equal("hi/video-only.m3u8", hi.Uri.ToString());
                    Assert.Collection(
                        hi.AlternativeAudio,
                        english =>
                        {
                            Assert.Equal(MediaType.Audio, english.Type);
                            Assert.Equal(hi.Audio, english.GroupId);
                            Assert.Equal("English", english.Name);
                            Assert.Equal(true, english.Default);
                            Assert.Equal(true, english.AutoSelect);
                            Assert.Equal("en", english.Language);
                            Assert.Equal("main/english-audio.m3u8", english.Uri.ToString());
                        },
                        german =>
                        {
                            Assert.Equal(MediaType.Audio, german.Type);
                            Assert.Equal(hi.Audio, german.GroupId);
                            Assert.Equal("Deutsch", german.Name);
                            Assert.Equal(false, german.Default);
                            Assert.Equal(true, german.AutoSelect);
                            Assert.Equal("de", german.Language);
                            Assert.Equal("main/german-audio.m3u8", german.Uri.ToString());
                        },
                        commentary =>
                        {
                            Assert.Equal(MediaType.Audio, commentary.Type);
                            Assert.Equal(hi.Audio, commentary.GroupId);
                            Assert.Equal("Commentary", commentary.Name);
                            Assert.Equal(false, commentary.Default);
                            Assert.Equal(false, commentary.AutoSelect);
                            Assert.Equal("en", commentary.Language);
                            Assert.Equal("commentary/audio-only.m3u8", commentary.Uri.ToString());
                        });
                },
                main =>
                {
                    Assert.Equal(65000, main.Bandwidth);
                    Assert.Collection(main.Codecs, codec => Assert.Equal("mp4a.40.5", codec));
                    Assert.Equal("aac", main.Audio);
                    Assert.Equal("main/english-audio.m3u8", main.Uri.ToString());
                    Assert.Collection(
                        main.AlternativeAudio,
                        english =>
                        {
                            Assert.Equal(MediaType.Audio, english.Type);
                            Assert.Equal(main.Audio, english.GroupId);
                            Assert.Equal("English", english.Name);
                            Assert.Equal(true, english.Default);
                            Assert.Equal(true, english.AutoSelect);
                            Assert.Equal("en", english.Language);
                            Assert.Equal("main/english-audio.m3u8", english.Uri.ToString());
                        },
                        german =>
                        {
                            Assert.Equal(MediaType.Audio, german.Type);
                            Assert.Equal(main.Audio, german.GroupId);
                            Assert.Equal("Deutsch", german.Name);
                            Assert.Equal(false, german.Default);
                            Assert.Equal(true, german.AutoSelect);
                            Assert.Equal("de", german.Language);
                            Assert.Equal("main/german-audio.m3u8", german.Uri.ToString());
                        },
                        commentary =>
                        {
                            Assert.Equal(MediaType.Audio, commentary.Type);
                            Assert.Equal(main.Audio, commentary.GroupId);
                            Assert.Equal("Commentary", commentary.Name);
                            Assert.Equal(false, commentary.Default);
                            Assert.Equal(false, commentary.AutoSelect);
                            Assert.Equal("en", commentary.Language);
                            Assert.Equal("commentary/audio-only.m3u8", commentary.Uri.ToString());
                        });
                });
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
            var container = new Container();
            var registrations = new List<Registration>();
            registrations.AddRange(AbnfRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(UriRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(HlsRegistrations.GetRegistrations(container.GetInstance));
            foreach (var registration in registrations)
            {
                if (registration.Implementation != null)
                    container.RegisterSingleton(registration.Service, registration.Implementation);
                else if (registration.Instance != null)
                    container.RegisterSingleton(registration.Service, registration.Instance);
                else if (registration.Factory != null)
                    container.RegisterSingleton(registration.Service, registration.Factory);
            }
            var parser = container.GetInstance<HlsParser>();
            var result = parser.Parse(data, container.GetInstance<PlaylistWalker>());
            Assert.Collection(
                result.VariantStreams,
                low =>
                {
                    Assert.Equal(1280000, low.Bandwidth);
                    Assert.Collection(low.Codecs, codec => Assert.Equal("...", codec));
                    Assert.Equal("low", low.Video);
                    Assert.Equal("low/main/audio-video.m3u8", low.Uri.ToString());
                    Assert.Collection(
                        low.AlternativeVideo,
                        main =>
                        {
                            Assert.Equal(MediaType.Video, main.Type);
                            Assert.Equal(low.Video, main.GroupId);
                            Assert.Equal("Main", main.Name);
                            Assert.Equal(true, main.Default);
                            Assert.Equal("low/main/audio-video.m3u8", main.Uri.ToString());
                        },
                        centerfield =>
                        {
                            Assert.Equal(MediaType.Video, centerfield.Type);
                            Assert.Equal(low.Video, centerfield.GroupId);
                            Assert.Equal("Centerfield", centerfield.Name);
                            Assert.Equal(false, centerfield.Default);
                            Assert.Equal("low/centerfield/audio-video.m3u8", centerfield.Uri.ToString());
                        },
                        dugout =>
                        {
                            Assert.Equal(MediaType.Video, dugout.Type);
                            Assert.Equal(low.Video, dugout.GroupId);
                            Assert.Equal("Dugout", dugout.Name);
                            Assert.Equal(false, dugout.Default);
                            Assert.Equal("low/dugout/audio-video.m3u8", dugout.Uri.ToString());
                        });
                },
                mid =>
                {
                    Assert.Equal(2560000, mid.Bandwidth);
                    Assert.Collection(mid.Codecs, codec => Assert.Equal("...", codec));
                    Assert.Equal("mid", mid.Video);
                    Assert.Equal("mid/main/audio-video.m3u8", mid.Uri.ToString());
                    Assert.Collection(
                        mid.AlternativeVideo,
                        main =>
                        {
                            Assert.Equal(MediaType.Video, main.Type);
                            Assert.Equal(mid.Video, main.GroupId);
                            Assert.Equal("Main", main.Name);
                            Assert.Equal(true, main.Default);
                            Assert.Equal("mid/main/audio-video.m3u8", main.Uri.ToString());
                        },
                        centerfield =>
                        {
                            Assert.Equal(MediaType.Video, centerfield.Type);
                            Assert.Equal(mid.Video, centerfield.GroupId);
                            Assert.Equal("Centerfield", centerfield.Name);
                            Assert.Equal(false, centerfield.Default);
                            Assert.Equal("mid/centerfield/audio-video.m3u8", centerfield.Uri.ToString());
                        },
                        dugout =>
                        {
                            Assert.Equal(MediaType.Video, dugout.Type);
                            Assert.Equal(mid.Video, dugout.GroupId);
                            Assert.Equal("Dugout", dugout.Name);
                            Assert.Equal(false, dugout.Default);
                            Assert.Equal("mid/dugout/audio-video.m3u8", dugout.Uri.ToString());
                        });
                },
                hi =>
                {
                    Assert.Equal(7680000, hi.Bandwidth);
                    Assert.Collection(hi.Codecs, codec => Assert.Equal("...", codec));
                    Assert.Equal("hi", hi.Video);
                    Assert.Equal("hi/main/audio-video.m3u8", hi.Uri.ToString());
                    Assert.Collection(
                        hi.AlternativeVideo,
                        main =>
                        {
                            Assert.Equal(MediaType.Video, main.Type);
                            Assert.Equal(hi.Video, main.GroupId);
                            Assert.Equal("Main", main.Name);
                            Assert.Equal(true, main.Default);
                            Assert.Equal("hi/main/audio-video.m3u8", main.Uri.ToString());
                        },
                        centerfield =>
                        {
                            Assert.Equal(MediaType.Video, centerfield.Type);
                            Assert.Equal(hi.Video, centerfield.GroupId);
                            Assert.Equal("Centerfield", centerfield.Name);
                            Assert.Equal(false, centerfield.Default);
                            Assert.Equal("hi/centerfield/audio-video.m3u8", centerfield.Uri.ToString());
                        },
                        dugout =>
                        {
                            Assert.Equal(MediaType.Video, dugout.Type);
                            Assert.Equal(hi.Video, dugout.GroupId);
                            Assert.Equal("Dugout", dugout.Name);
                            Assert.Equal(false, dugout.Default);
                            Assert.Equal("hi/dugout/audio-video.m3u8", dugout.Uri.ToString());
                        });
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
            var container = new Container();
            var registrations = new List<Registration>();
            registrations.AddRange(AbnfRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(UriRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(HlsRegistrations.GetRegistrations(container.GetInstance));
            foreach (var registration in registrations)
            {
                if (registration.Implementation != null)
                    container.RegisterSingleton(registration.Service, registration.Implementation);
                else if (registration.Instance != null)
                    container.RegisterSingleton(registration.Service, registration.Instance);
                else if (registration.Factory != null)
                    container.RegisterSingleton(registration.Service, registration.Factory);
            }
            var parser = container.GetInstance<HlsParser>();
            var result = parser.Parse(data, container.GetInstance<PlaylistWalker>());
            Assert.Collection(
                result.VariantStreams,
                low =>
                {
                    Assert.Equal(1280000, low.Bandwidth);
                    Assert.Null(low.AverageBandwidth);
                    Assert.Empty(low.Codecs);
                    Assert.Null(low.Resolution);
                    Assert.Null(low.Framerate);
                    Assert.Null(low.Audio);
                    Assert.Null(low.Video);
                    Assert.Null(low.Subtitles);
                    Assert.Null(low.ClosedCaptions);
                },
                mid =>
                {
                    Assert.Equal(2560000, mid.Bandwidth);
                    Assert.Null(mid.AverageBandwidth);
                    Assert.Empty(mid.Codecs);
                    Assert.Null(mid.Resolution);
                    Assert.Null(mid.Framerate);
                    Assert.Null(mid.Audio);
                    Assert.Null(mid.Video);
                    Assert.Null(mid.Subtitles);
                    Assert.Null(mid.ClosedCaptions);
                },
                hi =>
                {
                    Assert.Equal(7680000, hi.Bandwidth);
                    Assert.Null(hi.AverageBandwidth);
                    Assert.Empty(hi.Codecs);
                    Assert.Null(hi.Resolution);
                    Assert.Null(hi.Framerate);
                    Assert.Null(hi.Audio);
                    Assert.Null(hi.Video);
                    Assert.Null(hi.Subtitles);
                    Assert.Null(hi.ClosedCaptions);
                },
                audio_only =>
                {
                    Assert.Equal(65000, audio_only.Bandwidth);
                    Assert.Null(audio_only.AverageBandwidth);
                    Assert.Collection(
                        audio_only.Codecs,
                        codec => Assert.Equal("mp4a.40.5", codec));
                    Assert.Null(audio_only.Resolution);
                    Assert.Null(audio_only.Framerate);
                    Assert.Null(audio_only.Audio);
                    Assert.Null(audio_only.Video);
                    Assert.Null(audio_only.Subtitles);
                    Assert.Null(audio_only.ClosedCaptions);
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
            var container = new Container();
            var registrations = new List<Registration>();
            registrations.AddRange(AbnfRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(UriRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(HlsRegistrations.GetRegistrations(container.GetInstance));
            foreach (var registration in registrations)
            {
                if (registration.Implementation != null)
                    container.RegisterSingleton(registration.Service, registration.Implementation);
                else if (registration.Instance != null)
                    container.RegisterSingleton(registration.Service, registration.Instance);
                else if (registration.Factory != null)
                    container.RegisterSingleton(registration.Service, registration.Factory);
            }
            var parser = container.GetInstance<HlsParser>();
            var result = parser.Parse(data, container.GetInstance<PlaylistWalker>());
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
            var container = new Container();
            var registrations = new List<Registration>();
            registrations.AddRange(AbnfRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(UriRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(HlsRegistrations.GetRegistrations(container.GetInstance));
            foreach (var registration in registrations)
            {
                if (registration.Implementation != null)
                    container.RegisterSingleton(registration.Service, registration.Implementation);
                else if (registration.Instance != null)
                    container.RegisterSingleton(registration.Service, registration.Instance);
                else if (registration.Factory != null)
                    container.RegisterSingleton(registration.Service, registration.Factory);
            }
            var parser = container.GetInstance<HlsParser>();
            var result = parser.Parse(data, container.GetInstance<PlaylistWalker>());
            Assert.Equal(TimeSpan.FromSeconds(10), result.TargetDuration);
            Assert.Collection(
                result.MediaSegments,
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
    }
}
