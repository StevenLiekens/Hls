using System.Collections.Generic;
using System.Reflection;
using Txt.Core;

namespace Hls
{
    public class HlsRegistrations : Registrations
    {
        public static IEnumerable<Registration> GetRegistrations(GetInstanceDelegate getInstance)
        {
            return new List<Registration>(GetRegistrations(typeof(HlsRegistrations).GetTypeInfo().Assembly, getInstance))
            {
                new Registration(typeof(PlaylistWalker), typeof(PlaylistWalker)),
                new Registration(typeof(HlsParser), typeof(HlsParser))
            };
        }
    }
}
