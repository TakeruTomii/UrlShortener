using System.Reflection;
using System.Resources;

namespace UrlShortenerApp.Shared
{
    public class ErrorMessageService : IErrorMessageService
    {
        private readonly ResourceManager resourceManager;

        public ErrorMessageService()
        {
            resourceManager = new ResourceManager(
                SharedConstants.ERROR_RESOURCE_FILE_LOCATION,
                Assembly.GetExecutingAssembly());
        }
        public string GetErrorMessage(string key)
        {
            return resourceManager.GetString(key);
        }
    }
}
