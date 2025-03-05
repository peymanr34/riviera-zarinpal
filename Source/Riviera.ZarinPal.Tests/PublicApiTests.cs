using System.Threading.Tasks;
using PublicApiGenerator;
using VerifyXunit;
using Xunit;

namespace Riviera.ZarinPal.Tests
{
    public class PublicApiTests
    {
        [Fact]
        public Task PublicApiShouldNotChange()
        {
            var publicApi = typeof(V1.ZarinPalService).Assembly
                .GeneratePublicApi();

            return Verifier.Verify(publicApi)
                .UniqueForTargetFrameworkAndVersion();
        }
    }
}
