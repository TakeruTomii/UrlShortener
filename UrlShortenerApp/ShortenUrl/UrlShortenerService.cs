using UrlShortenerApp.ShortenUrl.Helper;
using UrlShortenerApp.ShortenUrl.Model;

namespace UrlShortenerApp.ShortenUrl
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly IShortUrlMappingRepository repository;

        public UrlShortenerService(IShortUrlMappingRepository shortUrlMappingRepository)
        {
            repository = shortUrlMappingRepository;
        }

        public Uri ShortenUrl(in CreateShortUrlConfiguration config)
        {
            var originalUrl = config.OriginalUrl;

            if (repository.IsRegisteredOriginalUrl(originalUrl))
            {
                return repository.FetchShortUrl(originalUrl);
            }

            var shortUrl = GenerateShortUrl(
                config.ShortUrlOrigin,
                config.ShortPathGenerator);

            repository.SaveShortUrl(shortUrl, originalUrl);

            return shortUrl.GetShortUrl();
        }

        public Uri FetchOriginalUrl(in ShortUrl shortUrl)
        {
            return repository.FetchOriginalUrl(shortUrl);
        }

        private ShortUrl GenerateShortUrl(
            in Uri shortUrlOrigin,
            in IShortPathGenerator shortPathGenerator)
        {
            var shortUrl = ShortUrl.CreateShortUrl(
                shortUrlOrigin,
                shortPathGenerator);

            while (repository.IsDuplicateShortUrl(shortUrl))
            {
                shortUrl = shortUrl.UpdateShortUrlPath();
            }

            return shortUrl;
        }
    }
}
