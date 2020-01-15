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
        private readonly HttpClient _httpClient;

        public BingImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// converts the returned result to BingImage object
        /// </summary>
        /// <param name="myResponseString">the json response as input</param>
        /// <returns>a BingImage object</returns>
        public BingImage ConvertToBingImage(Task<string> myResponseString)
        {
            var parsedReponse = JValue.Parse(myResponseString.Result.ToString());
            var myImage = parsedReponse.SelectToken("images");
            return myImage.ToObject<BingImage[]>().FirstOrDefault();
        }

        /// <summary>
        /// get image file from url of the image
        /// </summary>
        /// <param name="imageUrl">url of the image</param>
        /// <returns>image</returns>
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

        /// <summary>
        /// get the image info from server
        /// </summary>
        /// <param name="imageUrl">imageUrl</param>
        /// <returns>image info as string</returns>
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

        /// <summary>
        /// removes special caracters from image name 
        /// </summary>
        /// <param name="str">image old title</param>
        /// <returns>image title without special caracters</returns>
        public string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
    }
}
