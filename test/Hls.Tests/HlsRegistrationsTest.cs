﻿using System.Collections.Generic;
using Hls.playlist;
using SimpleInjector;
using Txt.Core;
using Txt.ABNF;
using UriSyntax;
using Xunit;

namespace Hls
{
    public class HlsRegistrationsTest
    {
        private readonly Container container = new Container();

        [Fact]
        public void GetRegistrations()
        {
            var registrations = new List<Txt.Core.Registration>();
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
        }
    }
}
