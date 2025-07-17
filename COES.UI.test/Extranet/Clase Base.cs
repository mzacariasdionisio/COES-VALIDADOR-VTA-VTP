
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using NUnit.Framework;

namespace Extranet
{
    public class Clase_Base
    {
        public static WebDriver driver;
        public static string FilePath = "C:\\Test\\Descargas";
        //local
        //public static string FilePathFmtos = "C:\\Test\\Formatos";
        //producción
        public static string FilePathFmtos = "C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes";
        
        
        [OneTimeSetUp]
        public void open() 
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", FilePath);
            chromeOptions.AddUserProfilePreference("profile.password_manager_leak_detection", false);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Size = new System.Drawing.Size(974, 1039);
            driver.Manage().Window.Maximize();
            FuncionesRecurrentes.loginExtranet(driver);
        }

        [OneTimeTearDown] 
        public void close() {
            driver.Quit();
        }
    }
}