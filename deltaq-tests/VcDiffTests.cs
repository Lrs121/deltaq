using System.IO;
using NUnit.Framework;

namespace deltaq_tests
{
    [TestFixture]
    public class VcDiffTests
    {
        [Test]
        public void VcDiffCreate()
        {
            var buf = new byte[0x1000];

            byte[] outBytes;
            using (var outStream = new MemoryStream())
            {
                deltaq.VcDiff.Create(buf, buf, outStream);
            }
        }
    }
}
