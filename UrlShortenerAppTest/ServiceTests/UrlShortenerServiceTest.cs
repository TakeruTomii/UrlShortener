using Moq;
using UrlShortenerApp.Shared;
using UrlShortenerApp.ShortenUrl;
using UrlShortenerApp.ShortenUrl.Helper;
using UrlShortenerApp.ShortenUrl.Model;

namespace UrlShortenerAppTest.ServiceTests
{
    [TestClass]
    public class UrlShortenerServiceTest
    {
        private Uri shortUrlOrigin;
        private Uri originalUrlValue;
        private ShortUrl shortUrl;
        private OriginalUrl originalUrl;
        private Mock<IShortPathGenerator> mockShortPathGenerator;
        private Mock<IErrorMessageService> mockErrorMessages;
        private Mock<IShortUrlMappingRepository> mockRepository;

        [TestInitialize]
        public void Initialize()
        {
            var shortUrlPath = TestConstants.SHORT_PATH_1;
            shortUrlOrigin = new Uri(TestConstants.SHORT_URL_ORIGIN);
            originalUrlValue = new Uri(TestConstants.ORIGINAL_URL);

            mockShortPathGenerator = new Mock<IShortPathGenerator>();
            mockShortPathGenerator
                .Setup(x => x.GenerateShortPath())
                .Returns(shortUrlPath);
            mockErrorMessages = new Mock<IErrorMessageService>();
            mockRepository = new Mock<IShortUrlMappingRepository>();

            shortUrl = ShortUrl.CreateShortUrlFromShorUrlPath(
                shortUrlOrigin,
                shortUrlPath,
                mockErrorMessages.Object,
                mockShortPathGenerator.Object);
            originalUrl = OriginalUrl.CreateOriginalUrl(
                this.originalUrlValue.AbsoluteUri,
                mockErrorMessages.Object);
        }

        [TestMethod]
        public void ShortenUrl_OriginalUrlRegistered_ReturnExistingShortUrl()
        {
            mockRepository
                .Setup(x => x.IsRegisteredOriginalUrl(originalUrl))
                .Returns(true);
            mockRepository
                .Setup(x => x.FetchShortUrl(originalUrl))
                .Returns(shortUrl.GetShortUrl());

            var config = new CreateShortUrlConfiguration()
            {
                OriginalUrl = originalUrl,
                ShortUrlOrigin = shortUrlOrigin,
                ShortPathGenerator = mockShortPathGenerator.Object
            };

            var service = new UrlShortenerService(mockRepository.Object);
            var res = service.ShortenUrl(config);

            Assert.AreEqual(shortUrl.GetShortUrl().AbsolutePath, res.AbsolutePath);
            mockRepository.Verify(
                x => x.IsRegisteredOriginalUrl(originalUrl),
                Times.Once());
            mockRepository.Verify(
                x => x.FetchShortUrl(originalUrl),
                Times.Once());
        }

        [TestMethod]
        public void ShortenUrl_NewOriginalUrl_SaveNewShortUrl()
        {
            mockRepository
                .Setup(x => x.IsRegisteredOriginalUrl(originalUrl))
                .Returns(false);
            mockRepository
                .Setup(x => x.FetchShortUrl(originalUrl))
                .Returns(shortUrl.GetShortUrl());

            var config = new CreateShortUrlConfiguration()
            {
                OriginalUrl = originalUrl,
                ShortUrlOrigin = shortUrlOrigin,
                ShortPathGenerator = mockShortPathGenerator.Object
            };

            var service = new UrlShortenerService(mockRepository.Object);
            var res = service.ShortenUrl(config);

            Assert.AreEqual(shortUrl.GetShortUrl().AbsoluteUri, res.AbsoluteUri);
            mockRepository.Verify(
                x => x.IsRegisteredOriginalUrl(originalUrl),
                Times.Once());
            mockRepository.Verify(
                x => x.FetchShortUrl(originalUrl),
                Times.Never());
        }

        [TestMethod]
        public void FetchOriginalUrl_ShortUrlRegistered_FetchUrl()
        {
            mockRepository
                .Setup(x => x.FetchOriginalUrl(shortUrl))
                .Returns(originalUrlValue);

            var service = new UrlShortenerService(mockRepository.Object);
            var res = service.FetchOriginalUrl(shortUrl);

            Assert.AreEqual(originalUrlValue.AbsoluteUri, res.AbsoluteUri);
            mockRepository.Verify(x => x.FetchOriginalUrl(shortUrl), Times.Once());
        }

    }
}