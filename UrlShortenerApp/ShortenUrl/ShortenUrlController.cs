using Microsoft.AspNetCore.Mvc;
using UrlShortenerApp.Error;
using UrlShortenerApp.Shared;
using UrlShortenerApp.ShortenUrl.Dto;
using UrlShortenerApp.ShortenUrl.Helper;
using UrlShortenerApp.ShortenUrl.Model;

namespace UrlShortenerApp.ShortenUrl
{
    [ApiController]
    public class ShortenUrlController : ControllerBase
    {
        private readonly ILogger<ShortenUrlController> logger;
        private readonly IUrlShortenerService urlShortener;
        private readonly IShortPathGenerator shortPathGenerator;
        private readonly IErrorMessageService errorMessages;
        private readonly Uri shortUrlOrigin;

        public ShortenUrlController(
            ILogger<ShortenUrlController> logger,
            IConfiguration configuration,
            IUrlShortenerService urlShortenerService,
            IErrorMessageService errorMessageService)
        {
            this.logger = logger;
            urlShortener = urlShortenerService;
            errorMessages = errorMessageService;
            shortPathGenerator = new ShortPathGenerator();

            var shortUrlSection = configuration.GetSection(ShortenUrlConstants.SHORT_URL_SECTION);
            var shortUrlOriginValue = shortUrlSection[ShortenUrlConstants.SHORT_URL_ORIGIN];
            UrlShortenerHelper.ValidateUrl(shortUrlOriginValue, errorMessages);
            this.shortUrlOrigin = new Uri(shortUrlOriginValue);
        }

        [Route("/short-url")]
        [HttpPost]
        public ActionResult<ShortUrlMappingDto> ShortenUrl([FromBody] CreateShortUrlDto dto)
        {
            try
            {
                var originalUrl = OriginalUrl.CreateOriginalUrl(dto.OriginalUrl, errorMessages);
                var config = new CreateShortUrlConfiguration()
                {
                    OriginalUrl = originalUrl,
                    ShortUrlOrigin = shortUrlOrigin,
                    ShortPathGenerator = shortPathGenerator,
                };
                var shortenUrl = urlShortener.ShortenUrl(config);
                var res = new ShortUrlMappingDto()
                {
                    OriginalUrl = dto.OriginalUrl,
                    ShortUrl = shortenUrl.AbsoluteUri
                };

                return Ok(res);
            }
            catch (UriFormatException ex)
            {
                ErrorUtilities.LogError(logger, ex);

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{shortPath}")]
        public ActionResult RedirectShortUrl(string shortPath)
        {
            try
            {
                var shortUrl = ShortUrl.CreateShortUrlFromShorUrlPath(
                    shortUrlOrigin,
                    shortPath,
                    errorMessages,
                    shortPathGenerator);
                var originalUrl = urlShortener.FetchOriginalUrl(shortUrl);

                return Redirect(originalUrl.AbsoluteUri);
            }
            catch (FormatException ex)
            {
                ErrorUtilities.LogError(logger, ex);

                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                ErrorUtilities.LogError(logger, ex);

                return NotFound(ex.Message);
            }
        }
    }
}
