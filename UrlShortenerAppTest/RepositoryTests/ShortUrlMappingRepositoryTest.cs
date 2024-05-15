using Moq;
using UrlShortenerApp.Shared;
using UrlShortenerApp.ShortenUrl;
using UrlShortenerApp.ShortenUrl.Helper;
using UrlShortenerApp.ShortenUrl.Model;

namespace UrlShortenerAppTest.RepositoryTests
{
    [TestClass]
    public class ShortUrlMappingRepositoryTest
    {
        private Uri originalUrlValue;
        private ShortUrl shortUrl;
        private OriginalUrl originalUrl;
        private Mock<IShortPathGenerator> mockShortPathGenerator;
        private Mock<IErrorMessageService> mockErrorMessages;

        [TestInitialize]
        public void Initialize()
        {
            var shortUrlPath = TestConstants.SHORT_PATH_1;
            var shortUrlOrigin = new Uri(TestConstants.SHORT_URL_ORIGIN);
            originalUrlValue = new Uri(TestConstants.ORIGINAL_URL);

            mockShortPathGenerator = new Mock<IShortPathGenerator>();
            mockShortPathGenerator
                .Setup(x => x.GenerateShortPath())
                .Returns(shortUrlPath);
            mockErrorMessages = new Mock<IErrorMessageService>();
            mockErrorMessages
                .Setup(x => x.GetErrorMessage(SharedConstants.ERROR_RESOURCE_NOT_EXISTS))
                .Returns(string.Empty);

            shortUrl = ShortUrl.CreateShortUrlFromShorUrlPath(
                shortUrlOrigin,
                shortUrlPath,
                mockErrorMessages.Object,
                mockShortPathGenerator.Object);
            originalUrl = OriginalUrl.CreateOriginalUrl(
                originalUrlValue.AbsoluteUri,
                mockErrorMessages.Object);
        }

        [TestMethod]
        public void FetchOriginalUrl_CannotFetchOriginalUrl_ThrowKeyNotFoundException()
        {
            var repository = new ShortUrlMappingRepository(mockErrorMessages.Object);

            Assert.ThrowsException<KeyNotFoundException>(
                () => repository.FetchOriginalUrl(shortUrl));
        }

        [TestMethod]
        public void FetchOriginalUrl_FetchSavedOriginalUrl_ReturnSavedUrl()
        {
            var repository = new ShortUrlMappingRepository(mockErrorMessages.Object);
            repository.SaveShortUrl(shortUrl, originalUrl);
            var res = repository.FetchOriginalUrl(shortUrl);

            Assert.AreEqual(originalUrlValue.AbsoluteUri, res.AbsoluteUri);
        }

        [TestMethod]
        public void FetchShortUrl_FetchSavedShortUrl_ReturnSavedUrl()
        {
            var repository = new ShortUrlMappingRepository(mockErrorMessages.Object);
            repository.SaveShortUrl(shortUrl, originalUrl);
            var res = repository.FetchShortUrl(originalUrl);

            Assert.AreEqual(shortUrl.GetShortUrl().AbsoluteUri, res.AbsoluteUri);
        }

        [TestMethod]
        public void IsDuplicateShortUrl_GivenDuplicatedUrl_ReturnTrue()
        {
            var repository = new ShortUrlMappingRepository(mockErrorMessages.Object);
            repository.SaveShortUrl(shortUrl, originalUrl);
            var res = repository.IsDuplicateShortUrl(shortUrl);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void IsDuplicateShortUrl_NotDuplicatedUrl_ReturnFalse()
        {
            var repository = new ShortUrlMappingRepository(mockErrorMessages.Object);
            var res = repository.IsDuplicateShortUrl(shortUrl);

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void IsRegisteredOriginalUrl_GivenRegisteredUrl_ReturnTrue()
        {
            var repository = new ShortUrlMappingRepository(mockErrorMessages.Object);
            repository.SaveShortUrl(shortUrl, originalUrl);
            var res = repository.IsRegisteredOriginalUrl(originalUrl);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void IsRegisteredOriginalUrl_NotRegisteredUrl_ReturnTrue()
        {
            var repository = new ShortUrlMappingRepository(mockErrorMessages.Object);
            var res = repository.IsRegisteredOriginalUrl(originalUrl);

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void SaveShortUrl_GivenPair_CanFetchOriginalUrl()
        {
            var repository = new ShortUrlMappingRepository(mockErrorMessages.Object);
            repository.SaveShortUrl(shortUrl, originalUrl);
            var res = repository.FetchOriginalUrl(shortUrl);

            Assert.AreEqual(originalUrlValue.AbsoluteUri, res.AbsoluteUri);
        }
    }
}
