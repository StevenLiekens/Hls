using System.Collections.Generic;
using SimpleInjector;
using Txt.ABNF;
using Uri;

namespace Hls
{
    public class TestBase
    {
        protected readonly Container Container = new Container();

        public TestBase()
        {
            var registrations = new List<Txt.Core.Registration>();
            registrations.AddRange(AbnfRegistrations.GetRegistrations(Container.GetInstance));
            registrations.AddRange(UriRegistrations.GetRegistrations(Container.GetInstance));
            registrations.AddRange(HlsRegistrations.GetRegistrations(Container.GetInstance));
            foreach (var registration in registrations)
            {
                if (registration.Implementation != null)
                {
                    Container.RegisterSingleton(registration.Service, registration.Implementation);
                }
                if (registration.Instance != null)
                {
                    Container.RegisterSingleton(registration.Service, registration.Instance);
                }
                if (registration.Factory != null)
                {
                    Container.RegisterSingleton(registration.Service, registration.Factory);
                }
            }
            Container.Verify();
        }
    }
}