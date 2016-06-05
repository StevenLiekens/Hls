using System;
using System.Collections.Generic;
using System.Linq;
using Hls.attribute_list;
using Txt.Core;

namespace Hls.EXT_X_MEDIA
{
    public class ExtMediaParser : Parser<ExtMedia, Rendition>
    {
        private readonly IParser<AttributeList, IDictionary<string, object>> attributeListParser;

        private readonly HashSet<string> inStreamValues = new HashSet<string>
        {
            "CC1",
            "CC2",
            "CC3",
            "CC4",
            "SERVICE1",
            "SERVICE2",
            "SERVICE3",
            "SERVICE4",
            "SERVICE5",
            "SERVICE6",
            "SERVICE7",
            "SERVICE8",
            "SERVICE9",
            "SERVICE10",
            "SERVICE11",
            "SERVICE12",
            "SERVICE13",
            "SERVICE14",
            "SERVICE15",
            "SERVICE16",
            "SERVICE17",
            "SERVICE18",
            "SERVICE19",
            "SERVICE20",
            "SERVICE21",
            "SERVICE22",
            "SERVICE23",
            "SERVICE24",
            "SERVICE25",
            "SERVICE26",
            "SERVICE27",
            "SERVICE28",
            "SERVICE29",
            "SERVICE30",
            "SERVICE31",
            "SERVICE32",
            "SERVICE33",
            "SERVICE34",
            "SERVICE35",
            "SERVICE36",
            "SERVICE37",
            "SERVICE38",
            "SERVICE39",
            "SERVICE40",
            "SERVICE41",
            "SERVICE42",
            "SERVICE43",
            "SERVICE44",
            "SERVICE45",
            "SERVICE46",
            "SERVICE47",
            "SERVICE48",
            "SERVICE49",
            "SERVICE50",
            "SERVICE51",
            "SERVICE52",
            "SERVICE53",
            "SERVICE54",
            "SERVICE55",
            "SERVICE56",
            "SERVICE57",
            "SERVICE58",
            "SERVICE59",
            "SERVICE60",
            "SERVICE61",
            "SERVICE62",
            "SERVICE63"
        };

        public ExtMediaParser(IParser<AttributeList, IDictionary<string, object>> attributeListParser)
        {
            if (attributeListParser == null)
            {
                throw new ArgumentNullException(nameof(attributeListParser));
            }
            this.attributeListParser = attributeListParser;
        }

        protected override Rendition ParseImpl(ExtMedia value)
        {
            var rendition = new Rendition();
            var values = attributeListParser.Parse((AttributeList)value[1]);
            object tmp;
            if (!values.TryGetValue(@"TYPE", out tmp))
            {
                throw new InvalidOperationException("Every EXT-X-MEDIA tag MUST include the TYPE attribute.");
            }
            switch ((string)tmp)
            {
                case @"AUDIO":
                    rendition.Type = MediaType.Audio;
                    break;
                case @"VIDEO":
                    rendition.Type = MediaType.Video;
                    break;
                case @"SUBTITLES ":
                    rendition.Type = MediaType.Subtitles;
                    break;
                case @"CLOSED-CAPTIONS ":
                    rendition.Type = MediaType.ClosedCaptions;
                    break;
                default:
                    throw new InvalidOperationException(
                        "Valid strings for the TYPE attribute are AUDIO, VIDEO, SUBTITLES and CLOSED-CAPTIONS.");
            }
            if (values.TryGetValue(@"URI", out tmp))
            {
                if (rendition.Type == MediaType.ClosedCaptions)
                {
                    throw new InvalidOperationException(
                        @"If the TYPE is CLOSED-CAPTIONS, the URI attribute MUST NOT be present.");
                }
                rendition.Uri = new System.Uri((string)tmp, UriKind.RelativeOrAbsolute);
            }
            if (!values.TryGetValue(@"GROUP-ID", out tmp))
            {
                throw new InvalidOperationException("Every EXT-X-MEDIA tag MUST include the GROUP-ID attribute.");
            }
            rendition.GroupId = (string)tmp;
            if (values.TryGetValue(@"LANGUAGE", out tmp))
            {
                rendition.Language = (string)tmp;
            }
            if (values.TryGetValue(@"ASSOC-LANGUAGE", out tmp))
            {
                rendition.AssociatedLanguage = (string)tmp;
            }
            if (!values.TryGetValue(@"NAME", out tmp))
            {
                throw new InvalidOperationException("Every EXT-X-MEDIA tag MUST include the NAME attribute.");
            }
            rendition.Name = (string)tmp;
            if (values.TryGetValue(@"DEFAULT", out tmp))
            {
                switch ((string)tmp)
                {
                    case @"YES":
                        rendition.Default = true;
                        break;
                    case @"NO":
                        rendition.Default = false;
                        break;
                    default:
                        throw new InvalidOperationException("Valid strings for the DEFAULT attribute are YES and NO.");
                }
            }
            if (values.TryGetValue(@"AUTOSELECT", out tmp))
            {
                switch ((string)tmp)
                {
                    case @"YES":
                        rendition.AutoSelect = true;
                        break;
                    case @"NO":
                        if (rendition.Default)
                        {
                            throw new InvalidOperationException(
                                "The AUTOSELECT attribute MUST be YES if the value of the DEFAULT attribute is YES.");
                        }
                        rendition.AutoSelect = false;
                        break;
                    default:
                        throw new InvalidOperationException(
                            "Valid strings for the AUTOSELECT attribute are YES and NO.");
                }
            }
            if (values.TryGetValue(@"FORCED", out tmp))
            {
                if (rendition.Type != MediaType.Subtitles)
                {
                    throw new InvalidOperationException(
                        "The FORCED attribute MUST NOT be present unless the TYPE is SUBTITLES.");
                }
                switch ((string)tmp)
                {
                    case @"YES":
                        rendition.AutoSelect = true;
                        break;
                    case @"NO":
                        rendition.AutoSelect = false;
                        break;
                    default:
                        throw new InvalidOperationException("Valid strings for the FORCED attribute are YES and NO.");
                }
            }
            if (values.TryGetValue(@"INSTREAM-ID", out tmp))
            {
                var id = (string)tmp;
                if (!inStreamValues.Contains(id))
                {
                    throw new InvalidOperationException("Valid strings for the INSTREAM-ID attribute are CC1, CC2, CC3, CC4 or SERVICEn where n MUST be an integer between 1 and 63 (e.g. SERVICE3 or SERVICE42).");
                }
                rendition.InStreamId = id;
            }
            else
            {
                if (rendition.Type == MediaType.ClosedCaptions)
                {
                    throw new InvalidOperationException(
                        "The INSTREAM-ID attribute MUST be present when the TYPE is CLOSED-CAPTIONS.");
                }
            }
            if (values.TryGetValue(@"CHARACTERISTICS", out tmp))
            {
                rendition.Characteristics = ((string)tmp).Split(',').Select(s => s.Trim()).ToList();
            }
            return rendition;
        }
    }
}
