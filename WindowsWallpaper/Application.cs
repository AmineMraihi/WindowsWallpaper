using System;
using System.IO;
using System.Threading.Tasks;

namespace WindowsWallpaper
{
    public class Application
    {
        private IBingImageService _bingImageService;
        private const string BingServerImageUrl = "https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US";


        public Application(IBingImageService bingImageService)
        {
            _bingImageService = bingImageService;
        }
        public void Run()
        {
            Task<string> responseString = _bingImageService.GetImageFromServer(BingServerImageUrl);

            //wait response
            responseString.Wait();

            //convert the return object to BingImage object
            BingImage myBingImage = _bingImageService.ConvertToBingImage(responseString);

            //get image
            Task<byte[]> myImage = _bingImageService.GetByte(myBingImage.Url);
            myImage.Wait();


            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            path = string.Concat(path, "/");
            string pathWithoutSpecialCaracters = _bingImageService.RemoveSpecialCharacters(myBingImage.Title);

            File.WriteAllBytes(string.Format("{0},{1},{2}", path, pathWithoutSpecialCaracters, ".png"), myImage.Result);
        }
    }
}
