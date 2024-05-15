using System.Text;

namespace UrlShortenerApp.ShortenUrl.Helper
{
    public class ShortPathGenerator : IShortPathGenerator
    {
        public string GenerateShortPath()
        {
            var shortPath = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < ShortenUrlConstants.SHORT_PATH_LENGTH; i++)
            {
                var index = random.Next() % ShortenUrlConstants.PATH_CHARACTER_LENGTH;
                var selectedCharacter = ShortenUrlConstants.SHORT_PATH_CHARACTER_SET[index];
                shortPath = shortPath.Append(selectedCharacter);
            }

            return shortPath.ToString();
        }
    }
}
