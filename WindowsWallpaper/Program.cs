using Autofac;
using System.Net.Http;
namespace WindowsWallpaper
{
    static class Program
    {

        private static IContainer CompositionRoot()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<Application>();
            builder.RegisterType<HttpClient>().SingleInstance();
            builder.RegisterType<BingImageService>().As<IBingImageService>();
            return builder.Build();
        }

        static void Main(string[] args)
        {
            CompositionRoot().Resolve<Application>().Run();

        }
    }
}
