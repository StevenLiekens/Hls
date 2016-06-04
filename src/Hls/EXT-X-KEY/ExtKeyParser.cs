using System;
using System.Collections.Generic;
using System.Globalization;
using Hls.attribute_list;
using Txt.Core;

namespace Hls.EXT_X_KEY
{
    public class ExtKeyParser : Parser<ExtKey, Key>
    {
        private readonly IParser<AttributeList, IDictionary<string, object>> attributeListParser;

        public ExtKeyParser(IParser<AttributeList, IDictionary<string, object>> attributeListParser)
        {
            if (attributeListParser == null)
            {
                throw new ArgumentNullException(nameof(attributeListParser));
            }
            this.attributeListParser = attributeListParser;
        }

        protected override Key ParseImpl(ExtKey key)
        {
            var result = new Key();
            var values = attributeListParser.Parse((AttributeList)key[1]);
            object tmp;
            if (!values.TryGetValue(@"METHOD", out tmp))
            {
                throw new InvalidOperationException("The METHOD attribute of EXT-X-KEY is REQUIRED.");
            }
            var method = (string)tmp;
            if (method == @"NONE")
            {
                result.Method = EncryptionMethod.None;
                if (values.Count != 1)
                {
                    throw new InvalidOperationException(
                        "If the encryption method is NONE, other attributes MUST NOT be present.");
                }
                return result;
            }
            if (method == @"AES-128")
            {
                result.Method = EncryptionMethod.AES128;
            }
            else if (method == @"SAMPLE-AES")
            {
                result.Method = EncryptionMethod.SAMPLE_AES;
            }
            if (!values.TryGetValue(@"URI", out tmp))
            {
                throw new InvalidOperationException(
                    "The URI attribute of EXT-X-KEY is REQUIRED for the AES-128 METHOD.");
            }
            result.Uri = new System.Uri((string)tmp, UriKind.RelativeOrAbsolute);
            if (values.TryGetValue(@"IV", out tmp))
            {
                result.IV = (byte[])tmp;
            }
            // This attribute is OPTIONAL; its absence indicates an implicit value of "identity".
            if (values.TryGetValue(@"KEYFORMAT", out tmp))
            {
                result.KeyFormat = (string)tmp;
            }
            else
            {
                result.KeyFormat = @"identity";
            }

            // This attribute is OPTIONAL; if it is not present, its value is considered to be "1".
            if (values.TryGetValue(@"KEYFORMATVERSIONS", out tmp))
            {
                var csv = (string)tmp;
                var versions = csv.Split('/');
                foreach (var version in versions)
                {
                    int v;
                    if (!int.TryParse(version, NumberStyles.None, NumberFormatInfo.InvariantInfo, out v))
                    {
                        throw new FormatException("KEYFORMATVERSIONS");
                    }
                    result.KeyFormatVersions.Add(v);
                }
            }
            else
            {
                result.KeyFormatVersions.Add(1);
            }
            return result;
        }
    }
}
