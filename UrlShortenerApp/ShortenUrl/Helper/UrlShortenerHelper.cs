using UrlShortenerApp.Shared;

namespace UrlShortenerApp.ShortenUrl.Helper
{
    public class UrlShortenerHelper
    {
        public static void ValidateUrl(
            in string url,
            in IErrorMessageService errorMessages)
        {
            if (!IsValidUrl(url))
            {
                var messageTemplate = errorMessages.GetErrorMessage(
                    SharedConstants.ERROR_INVALID_FORMAT);
                var errorMessage = string.Format(
                    messageTemplate,
                    SharedConstants.ERROR_ON_URL,
                    url);
                throw new UriFormatException(errorMessage);
            }
        }

        private static bool IsValidUrl(in string targetUrl)
        {
            return Uri.IsWellFormedUriString(targetUrl, UriKind.Absolute);
        }
    }
}
