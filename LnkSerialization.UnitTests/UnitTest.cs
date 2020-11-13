using System.IO;
using Xunit;

namespace LnkSerialization.UnitTests
{
    public class UnitTest
    {
        [Fact]
        public void TestSerializeAllLinksAsShortcuts()
        {
            var lnkFiles = Directory.GetFiles("Resources/", "*.lnk");
            foreach (var lnkFile in lnkFiles) {
                Core.LnkSerialization.SerializeAsShortcutJson(lnkFile);    
            }
        }

        [Fact]
        public void TestSerializeMyLinks()
        {
            Core.LnkSerialization.SerializeLinkFolder("Resources/", "");
        }

        [Fact]
        public void TestRestoreMyLinks()
        {
            Core.LnkSerialization.DeserializeLinksToFolder("", "Output/");
        }

        [Fact]
        public void TestSerializeMyProductionLinks()
        {
            Core.LnkSerialization.SerializeLinkFolder(@"C:\Dev\settings\shortcuts\", @"C:\Dev\settings\shortcuts\shorcuts.json");
        }

        [Fact]
        public void TestSerializeMyProductionLinksAsShortcuts()
        {
            var lnkFiles = Directory.GetFiles(@"C:\Dev\settings\shortcuts\", "*.lnk");
            foreach (var lnkFile in lnkFiles) {
                Core.LnkSerialization.SerializeAsShortcutJson(lnkFile);    
            }
        }
    }
}
