using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using WindowsWallpaper;

namespace UnitTests
{
    [TestClass]
    public class WindowsWallpaperTests
    {
        [TestMethod]
        public void SingleInstanceHttpClientExpected()
        {
            //Arrange
            HttpClient firstHttpClient = CompositionRoot().Resolve<HttpClient>();
            HttpClient secondHttpClient = CompositionRoot().Resolve<HttpClient>();

            //Act

            //Assert
            Assert.AreSame(firstHttpClient, secondHttpClient);
        }

        private IContainer CompositionRoot()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Application>();
            builder.RegisterType<HttpClient>().InstancePerLifetimeScope();
            builder.RegisterType<BingImageService>().As<IBingImageService>();
            return builder.Build();
        }
    }
}
