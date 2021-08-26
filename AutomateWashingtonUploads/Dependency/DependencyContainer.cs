using AutomateWashingtonUploads.Data;
using AutomateWashingtonUploads.Helpers;
using AutomateWashingtonUploads.StaticData;
using Ninject.Modules;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace AutomateWashingtonUploads.Dependency
{
    public class DependencyContainer : NinjectModule
    {
        public override void Load()
        {
            // This should suppress Chrome logs where it says a usb device isn't functioning
            var ls = new List<string> { "enable-logging" };
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddExcludedArguments(ls);
            var driver = new ChromeDriver(options: chromeOptions);

            Bind<ILoginInfo>().To<LoginInfo>();
            Bind<IWebDriver>().ToConstant(driver);
            Bind<IUploader>().To<Uploader>();
            Bind<IErrorHelper>().To<ErrorHelper>();
            Bind<ILogger>().To<Logger>();
            Bind<IEmailHelper>().To<EmailHelper>();
            Bind<IValidationHelper>().To<ValidationHelper>();
            Bind<ICompletionRepository>().To<CompletionRepository>();
        }
    }
}