using System.Text.RegularExpressions;
using UrlShortenerApp.ShortenUrl;
using UrlShortenerApp.ShortenUrl.Helper;

namespace UrlShortenerAppTest.HelperTests
{
    [TestClass]
    public class ShortPathGeneratorTest
    {
        [TestMethod]
        public void GenerateShortPath_Execute_ReturnValidShortPath()
        {
            var generator = new ShortPathGenerator();
            var res = generator.GenerateShortPath();

            Assert.AreEqual(ShortenUrlConstants.SHORT_PATH_LENGTH, res.Count());
            Assert.IsTrue(Regex.IsMatch(res, ShortenUrlConstants.REGEX_ALPHANUMERIC));
        }
    }
}
