using System;
using System.Collections.Generic;
using Hls.playlist;
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

        public Hls(ILexer<Playlist> playlistLexer)
        {
            if (playlistLexer == null)
            {
                throw new ArgumentNullException(nameof(playlistLexer));
            }
            this.playlistLexer = playlistLexer;
        }

        public static Container GetContainer()
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
            return container;
        }

        public PlaylistFile Parse(string text, PlaylistWalker walker)
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
