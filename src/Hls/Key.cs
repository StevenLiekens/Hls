using System.Collections.Generic;

namespace Hls
{
    public class Key
    {
        public byte[] IV { get; set; }

        public EncryptionMethod Method { get; set; }

        public System.Uri Uri { get; set; }

        public string KeyFormat { get; set; }

        public IList<int> KeyFormatVersions { get; set; } = new List<int>();
    }
}
