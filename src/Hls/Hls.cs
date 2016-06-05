using System;
using System.Collections.Generic;
using Hls.attribute;
using Hls.attribute_list;
using Hls.attribute_value;
using Hls.decimal_floating_point;
using Hls.decimal_integer;
using Hls.decimal_resolution;
using Hls.duration;
using Hls.EXTINF;
using Hls.EXT_X_DISCONTINUITY_SEQUENCE;
using Hls.EXT_X_I_FRAME_STREAM_INF;
using Hls.EXT_X_KEY;
using Hls.EXT_X_MEDIA_SEQUENCE;
using Hls.EXT_X_STREAM_INF;
using Hls.EXT_X_TARGETDURATION;
using Hls.EXT_X_VERSION;
using Hls.hexadecimal_sequence;
using Hls.playlist;
using Hls.quoted_string;
using Hls.signed_decimal_floating_point;
using SimpleInjector;
using Txt.Core;
using Txt.ABNF;
using Uri;
using Registration = Txt.Core.Registration;

namespace Hls
{
    public class Hls
    {
        private readonly ILexer<Playlist> playlistLexer;

        public Hls(ILexer<Playlist> playlistLexer)
        {
            if (playlistLexer == null)
            {
                throw new ArgumentNullException(nameof(playlistLexer));
            }
            this.playlistLexer = playlistLexer;
        }

        public static Hls CreateDefault()
        {
            var container = new Container();
            var registrations = new List<Registration>();
            registrations.AddRange(AbnfRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(UriRegistrations.GetRegistrations(container.GetInstance));
            registrations.AddRange(HlsRegistrations.GetRegistrations(container.GetInstance));
            foreach (var registration in registrations)
            {
                if (registration.Implementation != null)
                {
                    container.RegisterSingleton(registration.Service, registration.Implementation);
                }
                if (registration.Instance != null)
                {
                    container.RegisterSingleton(registration.Service, registration.Instance);
                }
                if (registration.Factory != null)
                {
                    container.RegisterSingleton(registration.Service, registration.Factory);
                }
            }
            container.Verify();
            return container.GetInstance<Hls>();
        }

        public PlaylistFile Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (text == string.Empty)
            {
                throw new ArgumentException("Argument is an empty string.", text);
            }
            ReadResult<Playlist> result;
            using (var src = new StringTextSource(text))
            using (var scanner = new TextScanner(src))
            {
                result = playlistLexer.Read(scanner);
            }
            if (!result.Success)
            {
                throw new InvalidOperationException();
            }
            var extVersionParser = new ExtVersionParser();
            var extTargetDurationParser = new ExtTargetDurationParser();
            var durationParser = new DurationParser();
            var extMediaSequenceParser = new ExtMediaSequenceParser();
            var hexadecimalSequenceParser = new HexadecimalSequenceParser();
            var decimalIntegerParser = new DecimalIntegerParser();
            var decimalResolutionParser = new DecimalResolutionParser(decimalIntegerParser);
            var decimalFloatingPointParser = new DecimalFloatingPointParser();
            var signedDecimalFloatingPointParser = new SignedDecimalFloatingPointParser(decimalFloatingPointParser);
            var quotedStringParser = new QuotedStringParser();
            var attributeValueParser = new AttributeValueParser(hexadecimalSequenceParser, decimalResolutionParser, decimalFloatingPointParser, signedDecimalFloatingPointParser, decimalIntegerParser, quotedStringParser);
            var attributeParser = new AttributeParser(attributeValueParser);
            var attributeListParser = new AttributeListParser(attributeParser);
            var extKeyParser = new ExtKeyParser(attributeListParser);
            var extStreamInfParser = new ExtStreamInfParser(attributeListParser);
            var extInfParser = new ExtInfParser(durationParser);
            var extIFrameStreamInfParser = new ExtIFrameStreamInfParser(attributeListParser);
            var extDiscontinuitySequenceParser = new ExtDiscontinuitySequenceParser(decimalIntegerParser);
            var walker = new PlaylistWalker(extVersionParser, extTargetDurationParser, extMediaSequenceParser, extKeyParser, extStreamInfParser, extInfParser, extIFrameStreamInfParser, extDiscontinuitySequenceParser);
            result.Element.Walk(walker);
            return walker.Result;
        }
    }
}
