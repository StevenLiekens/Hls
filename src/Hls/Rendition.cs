using System.Collections.Generic;

namespace Hls
{
    public class Rendition
    {
        public string AssociatedLanguage { get; set; }

        public bool AutoSelect { get; set; }

        public IList<string> Characteristics { get; set; }

        public bool Default { get; set; }

        public bool Forced { get; set; }

        public string GroupId { get; set; }

        public string InStreamId { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }

        public MediaType Type { get; set; }

        public System.Uri Uri { get; set; }
    }
}
