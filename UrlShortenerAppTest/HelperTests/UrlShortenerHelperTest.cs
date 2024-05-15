using Moq;
using UrlShortenerApp.Shared;
using UrlShortenerApp.ShortenUrl.Helper;

namespace UrlShortenerAppTest.HelperTests
{
    [TestClass]
    public class UrlShortenerHelperTest
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
        [DataRow("http://www.example.com")]
        [DataRow("https://www.example.com")]
        [DataRow("https://www.example.com:10080")]
        [DataRow("https://www.example.com/")]
        [DataRow("https://www.example.com/path")]
        [DataRow("https://www.example.com:10080/path")]
        [DataRow("https://www.example.com:10080/path?key=value")]
        [DataRow("https://www.example.com:10080/path?key1=value1&key1=value1")]
        [DataRow("https://www.example.com:10080/path#examples")]
        [DataRow("https://www.example.com:10080/%E3%81%82")]
        public void ValidatedUrl_GivenValidUrl_JustReturn(string url)
        {
            UrlShortenerHelper.ValidateUrl(url, mockErrorMessages.Object);

            mockErrorMessages
                .Verify(x => x.GetErrorMessage(SharedConstants.ERROR_INVALID_FORMAT),
                Times.Never());
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("qwerty")]
        [DataRow("https:/www.example.com")]
        [DataRow("http://example..com")]
        [DataRow("https://www.example.com:80:8080")]
        [DataRow("https://www.example.com:70000")]

        public void ValidatedUrl_GivenInvalidUrl_ThrowUriFormatException(string url)
        {
            Assert.ThrowsException<UriFormatException>(
                () => UrlShortenerHelper.ValidateUrl(url, mockErrorMessages.Object));
        }
    }
}
