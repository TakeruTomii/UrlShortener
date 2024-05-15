using Moq;
using UrlShortenerApp.Shared;
using UrlShortenerApp.ShortenUrl.Model;

namespace UrlShortenerAppTest.ModelTests
{
    [TestClass]
    public class OriginalUrlTest
    {
        private Mock<IErrorMessageService> mockErrorMessages;

        [TestInitialize]
        public void Initialize()
        {
            mockErrorMessages = new Mock<IErrorMessageService>();
            mockErrorMessages
                .Setup(x => x.GetErrorMessage(SharedConstants.ERROR_INVALID_FORMAT))
                .Returns(string.Empty);
        }

        [TestMethod]
        public void CreateOriginalUrl_GivenValidUrl_ReturnInstance()
        {
            var url = TestConstants.ORIGINAL_URL;

            var res = OriginalUrl.CreateOriginalUrl(url, mockErrorMessages.Object);
            Assert.AreEqual(url, res.GetOriginalUrl().AbsoluteUri);
        }

        [TestMethod]
        public void CreateOriginalUrl_GivenInalidUrl_ThrowUriFormatExeption()
        {
            var url = "https://www.example.com:80path";

            Assert.ThrowsException<UriFormatException>(
                () => OriginalUrl.CreateOriginalUrl(url, mockErrorMessages.Object));
        }

        [TestMethod]
        public void GetOriginalUrl_GivenValidUrl_RerturnSameUrl()
        {
            var url = TestConstants.ORIGINAL_URL;

            var originalUrl = OriginalUrl.CreateOriginalUrl(url, mockErrorMessages.Object);
            var res = originalUrl.GetOriginalUrl();

            Assert.AreEqual(url, res.AbsoluteUri);
        }
    }
}
