using AutomateWashingtonUploads.StaticData;
using Ninject.Modules;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateWashingtonUploads.Dependency
{
    public class DependencyContainer : NinjectModule
    {
        public override void Load()
        {
            Bind<ILoginInfo>().To<LoginInfo>();
            Bind<IWebDriver>().To<FirefoxDriver>();
        }
    }
}