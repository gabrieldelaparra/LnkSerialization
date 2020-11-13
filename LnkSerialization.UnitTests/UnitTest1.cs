using System.IO;
using Xunit;

namespace LnkSerialization.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Core.LnkSerialization.ReadLnk("Resources/ie.lnk");
            var lnkFiles = Directory.GetFiles("Resources/", "*.lnk");
            foreach (var lnkFile in lnkFiles) {
                Core.LnkSerialization.SerializeJson(lnkFile);    
            }
        }

        [Fact]
        public void TestSerializeMyLinks()
        {
            Core.LnkSerialization.SerializeLinkFolder("Resources/");
        }

        [Fact]
        public void TestRestoreMyLinks()
        {
            Core.LnkSerialization.RestoreLinks("Output/");
        }
    }
}
