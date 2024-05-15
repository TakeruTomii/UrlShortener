using UrlShortenerApp.ShortenUrl.Model;

namespace UrlShortenerApp.ShortenUrl
{
    public interface IUrlShortenerService
    {
        Uri ShortenUrl(in CreateShortUrlConfiguration config);

        Uri FetchOriginalUrl(in ShortUrl shortUrl);
    }
}
