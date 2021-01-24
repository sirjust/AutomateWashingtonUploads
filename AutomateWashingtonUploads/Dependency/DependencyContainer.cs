using AutomateWashingtonUploads.Helpers;
using AutomateWashingtonUploads.StaticData;
using Ninject.Modules;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomateWashingtonUploads.Dependency
{
    public class DependencyContainer : NinjectModule
    {
        public override void Load()
        {
            Bind<ILoginInfo>().To<LoginInfo>();
            Bind<IWebDriver>().To<ChromeDriver>();
            Bind<IUploader>().To<Uploader>();
            Bind<IErrorHelper>().To<ErrorHelper>();
            Bind<ILogger>().To<Logger>();
            Bind<IEmailHelper>().To<EmailHelper>();
            Bind<IValidationHelper>().To<ValidationHelper>();
        }
    }
}