using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WindowsWallpaper
{
    public class Application
    {
        private readonly IBingImageService _bingImageService;
        private const string BingServerImageUrl = "https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US";
        const int SetWallpaper = 20;
        const int UpdateIniFile = 0x01;
        const int SendWinIniChange = 0x02;

        public Application(IBingImageService bingImageService)
        {
            _bingImageService = bingImageService;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

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
            string myImageTitleWithoutSpecialCaracters = _bingImageService.RemoveSpecialCharacters(myBingImage.Title);

            File.WriteAllBytes(string.Format("{0}{1}{2}", path, myImageTitleWithoutSpecialCaracters, ".png"), myImage.Result);

            SystemParametersInfo(SetWallpaper, 0, string.Format("{0}{1}{2}", path, myImageTitleWithoutSpecialCaracters, ".png"), UpdateIniFile | SendWinIniChange);
        }
    }
}
