using deltaq.VcDiff;
using System.IO;
using Xunit;

namespace deltaq_tests
{
    public class VcDiffTests
    {
        [Fact]
        public void VcDiffParse()
        {
            const string file = @"X:\Storage\oTTW\Installer\bin\Debug\resources\hk.delta";

            var vcFile = new VcDeltaFile(File.OpenRead(file));
        }
    }
}
