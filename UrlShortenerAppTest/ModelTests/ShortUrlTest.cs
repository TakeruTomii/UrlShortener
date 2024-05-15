using Moq;
using UrlShortenerApp.Shared;
using UrlShortenerApp.ShortenUrl.Helper;
using UrlShortenerApp.ShortenUrl.Model;

namespace UrlShortenerAppTest.ModelTests
{
    [TestClass]
    public class ShortUrlTest
    {
        private Uri shortUrlOrigin;
        private Mock<IShortPathGenerator> mockShortPathGenerator;
        private Mock<IErrorMessageService> mockErrorMessages;

        [TestInitialize]
        public void Initialize()
        {
            shortUrlOrigin = new Uri(TestConstants.SHORT_URL_ORIGIN);
            mockShortPathGenerator = new Mock<IShortPathGenerator>();
            mockErrorMessages = new Mock<IErrorMessageService>();
            mockErrorMessages
                .Setup(x => x.GetErrorMessage(SharedConstants.ERROR_INVALID_FORMAT))
                .Returns(string.Empty);
        }

        [TestMethod]
        public void CreateShortUrl_GivenValidParameters_ReturnInstance()
        {
            var shortUrlPath = TestConstants.SHORT_PATH_1;
            var expectedShortUrl = TestConstants.SHORT_URL_ORIGIN + shortUrlPath;
            mockShortPathGenerator
                .Setup(x => x.GenerateShortPath())
                .Returns(shortUrlPath);

            var shortUrl = ShortUrl.CreateShortUrl(
                shortUrlOrigin,
                mockShortPathGenerator.Object);

            Assert.AreEqual(expectedShortUrl, shortUrl.GetShortUrl().AbsoluteUri);
        }

        [TestMethod]
        public void CreateShortUrlFromShorUrlPath_GivenValidParameters_ReturnInstance()
        {
            var shortUrlPath = TestConstants.SHORT_PATH_1;
            var expectedShortUrl = TestConstants.SHORT_URL_ORIGIN + shortUrlPath;
            mockShortPathGenerator
                .Setup(x => x.GenerateShortPath())
                .Returns(shortUrlPath);

            var shortUrl = ShortUrl.CreateShortUrlFromShorUrlPath(
                shortUrlOrigin,
                shortUrlPath,
                mockErrorMessages.Object,
                mockShortPathGenerator.Object);

            Assert.AreEqual(expectedShortUrl, shortUrl.GetShortUrl().AbsoluteUri);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("1234")]
        [DataRow("123456")]
        [DataRow("A2lo-")]
        public void CreateShortUrlFromShorUrlPath_GivenIValidShortPath_ThrowsFormatExeption(string shortUrlPath)
        {
            var expectedShortUrl = TestConstants.SHORT_URL_ORIGIN + shortUrlPath;
            mockShortPathGenerator
                .Setup(x => x.GenerateShortPath())
                .Returns(shortUrlPath);

            Assert.ThrowsException<FormatException>(
                () => ShortUrl.CreateShortUrlFromShorUrlPath(
                    shortUrlOrigin,
                    shortUrlPath,
                    mockErrorMessages.Object,
                    mockShortPathGenerator.Object));
        }

        [TestMethod]
        public void UpdateShortUrlPath_GivenValidPath_UpdateInstance()
        {
            var shortUrlPath = TestConstants.SHORT_PATH_2;
            var expectedShortPath = TestConstants.SHORT_PATH_1;
            var expectedShortUrl = TestConstants.SHORT_URL_ORIGIN + expectedShortPath;
            mockShortPathGenerator
                .Setup(x => x.GenerateShortPath())
                .Returns(expectedShortPath);

            var shortUrl = ShortUrl.CreateShortUrlFromShorUrlPath(
                shortUrlOrigin,
                shortUrlPath,
                mockErrorMessages.Object,
                mockShortPathGenerator.Object);
            var res = shortUrl.UpdateShortUrlPath();

            Assert.AreEqual(expectedShortUrl, res.GetShortUrl().AbsoluteUri);
        }

        [TestMethod]
        public void GetShortUrl_GivenValidPath_ReturnInstance()
        {
            var shortUrlPath = TestConstants.SHORT_PATH_2;
            var expectedShortUrl = TestConstants.SHORT_URL_ORIGIN + shortUrlPath;
            mockShortPathGenerator
                .Setup(x => x.GenerateShortPath())
                .Returns(shortUrlPath);

            var shortUrl = ShortUrl.CreateShortUrlFromShorUrlPath(
                shortUrlOrigin,
                shortUrlPath,
                mockErrorMessages.Object,
                mockShortPathGenerator.Object);
            var res = shortUrl.GetShortUrl();

            Assert.AreEqual(expectedShortUrl, res.AbsoluteUri);
        }
    }
}
