using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsWallpaper
{
    public interface IBingImageService
    {
        Task<string> GetImageFromServer(string imageUrl);
        BingImage ConvertToBingImage(Task<string> myResponseString);
        Task<byte[]> GetByte(string imageUrl);
        string RemoveSpecialCharacters(string str);
    }

    public class BingImageService : IBingImageService
    {
        private HttpClient _httpClient;

        public BingImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public BingImage ConvertToBingImage(Task<string> myResponseString)
        {
            var parsedReponse = JValue.Parse(myResponseString.Result.ToString());
            var myImage = parsedReponse.SelectToken("images");
            return myImage.ToObject<BingImage[]>().FirstOrDefault();
        }

        public async Task<byte[]> GetByte(string imageUrl)
        {
            byte[] responseByteArray = new byte[0];
            var response = await _httpClient.GetAsync("https://www.bing.com/" + imageUrl);
            if (response.IsSuccessStatusCode)
            {
                responseByteArray = await response.Content.ReadAsByteArrayAsync();
            }
            return responseByteArray;
        }

        public async Task<string> GetImageFromServer(string imageUrl)
        {
            string responseString = string.Empty;
            var response = await _httpClient.GetAsync(imageUrl);
            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
            }
            return responseString;
        }

        public string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
    }
}
