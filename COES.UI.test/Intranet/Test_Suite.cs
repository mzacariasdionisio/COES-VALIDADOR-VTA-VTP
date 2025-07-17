using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;

namespace Intranet
{
    internal class Test_Suite
    {
        public static WebDriver driver;
        public static string FilePath = "C:\\Test\\Descargas";
        //public static string FilePathFmtos = "C:\\Test\\Formatos";
        //"C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes"

        internal static IWebDriver SetUpClass()
        {

            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", FilePath);
			chromeOptions.AddUserProfilePreference("profile.password_manager_leak_detection", false);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Size = new System.Drawing.Size(1000, 1000);//(974, 1039);
            //estaba comentado
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            return driver;
        }
    }
}