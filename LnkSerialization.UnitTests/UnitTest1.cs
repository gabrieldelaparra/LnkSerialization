using System;

using Xunit;

namespace LnkSerialization.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Core.LnkSerialization.ReadLnk("Resources/ie.lnk");
        }
    }
}
