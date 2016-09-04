using System.Collections.Generic;
using SimpleInjector;
using Txt.Core;
using Txt.ABNF;
using UriSyntax;
using Xunit;

namespace Hls.comment
{
    public class CommentLexerTest
    {
        private readonly Container container = new Container();

        public CommentLexerTest()
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

        [Theory]
        [InlineData("# Hello")]
        public void Read(string value)
        {
            using (var ts = new StringTextSource(value))
            {
                using (var scanner = new TextScanner(ts))
                {
                    var lexer = container.GetInstance<ILexer<Comment>>();
                    var readResult = lexer.Read(scanner);
                    Assert.NotNull(readResult);
                    Assert.True(readResult.IsSuccess);
                    Assert.Equal(value, readResult.Element.Text);
                }
            }
        }
    }
}
