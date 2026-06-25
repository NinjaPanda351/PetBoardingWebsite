using NUnit.Framework;

namespace PawesomePalaceTests
{
    [TestFixture]
    public class BaseTests
    {
        [Test]
        public void BaseTest()
        {
            Assert.That(1 == 1, "Pass condition");
        }
    }
}