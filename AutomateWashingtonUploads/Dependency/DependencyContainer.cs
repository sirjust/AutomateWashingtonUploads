using AutomateWashingtonUploads.StaticData;
using Ninject.Modules;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace AutomateWashingtonUploads.Dependency
{
    public class DependencyContainer : NinjectModule
    {
        public override void Load()
        {
            Bind<ILoginInfo>().To<LoginInfo>();
            Bind<IWebDriver>().To<FirefoxDriver>();
            Bind<IUploader>().To<Uploader>();
        }
    }
}