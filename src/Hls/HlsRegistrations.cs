using System.Collections.Generic;
using System.Reflection;
using Txt.Core;

namespace Hls
{
    public class HlsRegistrations : Registrations
    {
        public static IEnumerable<Registration> GetRegistrations(GetInstanceDelegate getInstance)
        {
            return GetRegistrations(typeof(HlsRegistrations).GetTypeInfo().Assembly, getInstance);
        }
    }
}
