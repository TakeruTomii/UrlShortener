using UrlShortenerApp.ShortenUrl.Model;

namespace UrlShortenerApp.ShortenUrl
{
    public interface IShortUrlMappingRepository
    {
        public Uri FetchOriginalUrl(in ShortUrl shortUrl);

        public Uri FetchShortUrl(OriginalUrl originalUrl);

        public bool IsDuplicateShortUrl(in ShortUrl shortUrl);

        public bool IsRegisteredOriginalUrl(in OriginalUrl originalUrl);

        public void SaveShortUrl(in ShortUrl shortUrl, in OriginalUrl originalUrl);
    }
}
