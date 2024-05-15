using UrlShortenerApp.Shared;
using UrlShortenerApp.ShortenUrl.Helper;

namespace UrlShortenerApp.ShortenUrl.Model
{
    public class OriginalUrl
    {
        private readonly string url;

        private OriginalUrl(in string url)
        {
            this.url = url;
        }

        public static OriginalUrl CreateOriginalUrl(
            in string url,
            in IErrorMessageService errorMessages)
        {
            UrlShortenerHelper.ValidateUrl(url, errorMessages);
            return new OriginalUrl(url);
        }

        public Uri GetOriginalUrl()
        {
            return new Uri(url);
        }
    }
}
