using UrlShortenerApp.Shared;
using UrlShortenerApp.ShortenUrl.Model;

namespace UrlShortenerApp.ShortenUrl
{
    public class ShortUrlMappingRepository : IShortUrlMappingRepository
    {
        private readonly Dictionary<string, string> shortUrlMap;
        private readonly IErrorMessageService errorMessages;

        public ShortUrlMappingRepository(IErrorMessageService errorMessageService)
        {
            shortUrlMap = new Dictionary<string, string>();
            errorMessages = errorMessageService;
        }

        public Uri FetchOriginalUrl(in ShortUrl shortUrl)
        {
            var shortUrlValue = shortUrl.GetShortUrl().AbsoluteUri;
            if (!CanFetchRedirectUrl(shortUrl))
            {
                var messageTemplate = errorMessages.GetErrorMessage(
                    SharedConstants.ERROR_RESOURCE_NOT_EXISTS);
                var errorMessage = string.Format(
                    messageTemplate,
                    shortUrlValue);
                throw new KeyNotFoundException(errorMessage);
            }
            var originalUrl = shortUrlMap[shortUrlValue];

            return new Uri(originalUrl);
        }

        public Uri FetchShortUrl(OriginalUrl originalUrl)
        {
            var value = originalUrl.GetOriginalUrl().AbsoluteUri;
            var item = shortUrlMap.First(item => item.Value == value);

            return new Uri(item.Key);
        }

        public bool IsDuplicateShortUrl(in ShortUrl shortUrl)
        {
            var key = shortUrl.GetShortUrl().AbsoluteUri;
            return shortUrlMap.Keys.Contains(key);
        }

        public bool IsRegisteredOriginalUrl(in OriginalUrl originalUrl)
        {
            var value = originalUrl.GetOriginalUrl().AbsoluteUri;
            return shortUrlMap.Values.Contains(value);
        }

        public void SaveShortUrl(in ShortUrl shortUrl, in OriginalUrl originalUrl)
        {
            var key = shortUrl.GetShortUrl().AbsoluteUri;
            var value = originalUrl.GetOriginalUrl().AbsoluteUri;
            shortUrlMap.Add(key, value);
        }

        private bool CanFetchRedirectUrl(in ShortUrl shortUrl)
        {
            var key = shortUrl.GetShortUrl().AbsoluteUri;
            return shortUrlMap.Keys.Contains(key);
        }
    }
}
