using System;
using System.Collections.Generic;
using Hls.playlist;
using JetBrains.Annotations;
using SimpleInjector;
using Txt.ABNF;
using Txt.Core;
using UriSyntax;
using Registration = Txt.Core.Registration;

namespace Hls
{
    public class Hls
    {
        private readonly ILexer<Playlist> playlistLexer;

        private readonly PlaylistWalker walker;

        public Hls(ILexer<Playlist> playlistLexer, [NotNull] PlaylistWalker walker)
        {
            if (playlistLexer == null)
            {
                throw new ArgumentNullException(nameof(playlistLexer));
            }
            if (walker == null)
            {
                throw new ArgumentNullException(nameof(walker));
            }
            this.playlistLexer = playlistLexer;
            this.walker = walker;
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
            container.Register<PlaylistWalker>();
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
            IReadResult<Playlist> result;
            using (var src = new StringTextSource(text))
            using (var scanner = new TextScanner(src))
            {
                result = playlistLexer.Read(scanner);
            }
            if (!result.Success)
            {
                throw new InvalidOperationException();
            }
            result.Element.Walk(walker);
            return walker.Result;
        }
    }
}
