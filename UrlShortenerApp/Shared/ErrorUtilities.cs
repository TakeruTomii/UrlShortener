namespace UrlShortenerApp.Error
{
    public class ErrorUtilities
    {
        public static void LogError(ILogger logger, Exception ex)
        {
            logger.LogError(ex.Message);
            logger.LogError(ex.StackTrace);
        }
    }
}
