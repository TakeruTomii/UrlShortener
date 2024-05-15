using UrlShortenerApp.ShortenUrl.Helper;

namespace UrlShortenerApp.ShortenUrl.Model
{
    public class CreateShortUrlConfiguration
    {
        public OriginalUrl OriginalUrl { get; set; }
        public Uri ShortUrlOrigin { get; set; }
        public IShortPathGenerator ShortPathGenerator { get; set; }
    }
}
