using System.Text;
using System.Text.RegularExpressions;
using UrlShortenerApp.Shared;
using UrlShortenerApp.ShortenUrl.Helper;

namespace UrlShortenerApp.ShortenUrl.Model
{
    public class ShortUrl
    {
        private readonly string path;
        private readonly Uri origin;
        private readonly IShortPathGenerator generator;

        private ShortUrl(
            in Uri origin,
            in string path,
            in IShortPathGenerator generator)
        {
            this.origin = origin;
            this.path = path;
            this.generator = generator;
        }

        public static ShortUrl CreateShortUrl(
            in Uri shortUrlOrigin,
            in IShortPathGenerator generator)
        {
            return new ShortUrl(
                shortUrlOrigin,
                generator.GenerateShortPath(),
                generator);
        }

        public static ShortUrl CreateShortUrlFromShorUrlPath(
            in Uri shortUrlOrigin,
            in string shortUrlPath,
            in IErrorMessageService errorMessages,
            in IShortPathGenerator generator)
        {
            ValidateShortPath(shortUrlPath, errorMessages);
            return new ShortUrl(shortUrlOrigin, shortUrlPath, generator);
        }

        public ShortUrl UpdateShortUrlPath()
        {
            return new ShortUrl(
                origin,
                generator.GenerateShortPath(),
                generator);
        }

        public Uri GetShortUrl()
        {
            return new Uri(BuildShortUrl());
        }

        private string BuildShortUrl()
        {
            var shortUrl = new StringBuilder();
            shortUrl.Append(origin.AbsoluteUri);
            shortUrl.Append(path);

            return shortUrl.ToString();
        }

        private static void ValidateShortPath(
            in string targetPath,
            in IErrorMessageService errorMessages)
        {
            if (!IsValidShortPath(targetPath))
            {
                var messageTemplate = errorMessages.GetErrorMessage(
                    SharedConstants.ERROR_INVALID_FORMAT);
                var errorMessage = string.Format(
                    messageTemplate,
                    SharedConstants.ERROR_ON_PATH,
                    targetPath);
                throw new FormatException(errorMessage);
            }
        }

        private static bool IsValidShortPath(in string shortPath)
        {
            return !string.IsNullOrEmpty(shortPath) &&
                IsValidShortPathLength(shortPath) &&
                IsAlphaNumeric(shortPath);
        }

        private static bool IsValidShortPathLength(in string shortPath)
        {
            return shortPath.Count() == ShortenUrlConstants.SHORT_PATH_LENGTH;
        }

        private static bool IsAlphaNumeric(in string shortPath)
        {
            return Regex.IsMatch(
                shortPath,
                ShortenUrlConstants.REGEX_ALPHANUMERIC);
        }
    }
}
