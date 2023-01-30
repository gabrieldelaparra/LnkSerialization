using System.IO;

using Wororo.Utilities;

using Xunit;

namespace LnkSerialization.UnitTests
{
    public class UnitTest
    {
        [Fact]
        public void TestRestoreMyLinks()
        {
            Core.LnkSerialization.DeserializeLinksToFolder("", "Output/");
        }

        [Fact]
        public void TestSerializeAllLinksAsShortcuts()
        {
            var lnkFiles = Directory.GetFiles("Resources/", "*.lnk");
            foreach (var lnkFile in lnkFiles)
            {
                Core.LnkSerialization.SerializeAsShortcutJson(lnkFile);
            }
        }

        [Fact]
        public void TestLinkRunAsAdmin()
        {
            var adminModel = new Core.LinkModel("Resources/ie-admin.lnk");
            Assert.True(adminModel.RunAsAdmin);
            var noAdminModel = new Core.LinkModel("Resources/ie.lnk");
            Assert.False(noAdminModel.RunAsAdmin);
        }

        [Fact]
        public void TestLinkHasArgument()
        {
            var argsModel = new Core.LinkModel("Resources/ie-argument.lnk");
            Assert.True(argsModel.Args.Length > 0);
            var noArgsModel = new Core.LinkModel("Resources/ie.lnk");
            Assert.True(noArgsModel.Args.Length == 0);
        }

        [Fact]
        public void TestSerializeNonWorkingLinkAsShortcuts()
        {
            Core.LnkSerialization.SerializeAsShortcutJson("Resources/dir-non-working.lnk");
        }

        [Fact]
        public void TestSerializeAndDeserializeNonWorkingLinkAsShortcuts()
        {
            Core.LnkSerialization.SerializeAsShortcutJson("Resources/dir-non-working.lnk");
            var lnkModel = new Core.LinkModel("Resources/dir-non-working.lnk");
            JsonSerialization.SerializeJson(lnkModel, "dir-non-working-AsLnkModel.json");
            var outputFolder = "FromLnkModel/";
            outputFolder.CreatePathIfNotExists();
            lnkModel.ToLinkFile(outputFolder);
        }

        [Fact]
        public void TestSerializeWorkingLinkAsShortcuts()
        {
            Core.LnkSerialization.SerializeAsShortcutJson("Resources/dir-working.lnk");
        }

        [Fact]
        public void TestSerializeMyLinks()
        {
            Core.LnkSerialization.SerializeLinkFolder("Resources/");
        }

        //[Fact]
        //public void TestSerializeMyProductionLinks()
        //{
        //    Core.LnkSerialization.SerializeLinkFolder(@"C:\Apps\settings\shortcuts\", @"C:\Apps\settings\shortcuts.json");
        //}

        //[Fact]
        //public void TestDeserializeMyProductionLinks()
        //{
        //    Core.LnkSerialization.DeserializeLinksToFolder(@"C:\Apps\settings\shortcuts.json",
        //        @"C:\Apps\settings\output\");
        //}

        //    [Fact]
        //    public void TestSerializeMyProductionLinksAsShortcuts()
        //    {
        //        var lnkFiles = Directory.GetFiles(@"C:\Apps\settings\shortcuts\", "*.lnk");
        //        foreach (var lnkFile in lnkFiles)
        //        {
        //            Core.LnkSerialization.SerializeAsShortcutJson(lnkFile);
        //        }
        //    }
    }
}