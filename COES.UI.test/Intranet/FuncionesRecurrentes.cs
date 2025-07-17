using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;
using System.IO;
using OpenQA.Selenium.Support.Extensions;
using System.Threading;
using System.Xml.Linq;

namespace Intranet
{
    public class FuncionesRecurrentes
    {
        public static string FilePath  = "C:\\Test\\Descargas";
        //public static string issSite = "https://preprodcloud.coes.org.pe/INTRANET-CAMPANIA//Home/Login";

        //local
        //public static string issSite = "https://pruebascloud.coes.org.pe/INTRANET-ITE-2-I06CALCULOPORCENTAJESPTOANUAL2024//Home/Login";
        //public static string userSelenium = "selenium";
        //public static string pwdSelenium = "selenium";

        //producción
        public static string issSite = Environment.GetEnvironmentVariable("IssSite") + "/Home/Login";
        public static string userSelenium = Environment.GetEnvironmentVariable("userSelenium");
        public static string pwdSelenium = Environment.GetEnvironmentVariable("pwdSelenium");

        internal static void loginIntranet(IWebDriver driver)
        {
            Console.WriteLine(issSite);
            driver.Navigate().GoToUrl(url: issSite); //"https://pruebascloud.coes.org.pe/INTRANET-PRUEBA1-15-04-NET/Home/Login");

            WaitForElementToBeVisible(By.XPath("//input[@id=\'Usuario\']"), driver);
            var elements = driver.FindElements(By.XPath("//input[@id=\'Usuario\']"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.CssSelector(".login-content")).Click();
            driver.FindElement(By.XPath("//input[@id=\'Usuario\']")).SendKeys(userSelenium);
            elements = driver.FindElements(By.XPath("//input[@id=\'Clave\']"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.XPath("//input[@id=\'Clave\']")).SendKeys(pwdSelenium);
            driver.FindElement(By.XPath("//input[@id=\'btnEnviar\']")).Click();
            elements = driver.FindElements(By.CssSelector(".loading-text"));
            Assert.True(elements.Count > 0);

            WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("Bienvenidos a la Intranet SGOCOES"));
        }

        internal static void loginIntranet64bits(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(url: issSite); //"https://pruebascloud.coes.org.pe/INTRANET-PRUEBA1-15-04-NET/home/login");

            WaitForElementToBeVisible(By.XPath("//input[@id=\'Usuario\']"), driver);
            var elements = driver.FindElements(By.XPath("//input[@id=\'Usuario\']"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.CssSelector(".login-content")).Click();
            driver.FindElement(By.XPath("//input[@id=\'Usuario\']")).SendKeys(userSelenium);
            elements = driver.FindElements(By.XPath("//input[@id=\'Clave\']"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.XPath("//input[@id=\'Clave\']")).SendKeys(pwdSelenium);
            driver.FindElement(By.XPath("//input[@id=\'btnEnviar\']")).Click();
            elements = driver.FindElements(By.CssSelector(".loading-text"));
            Assert.True(elements.Count > 0);

            WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("Bienvenidos a la Intranet SGOCOES"));
        }

        internal static void EliminarYCrearCarpeta()
        {
            if (Directory.Exists(FilePath))
            {
                Directory.Delete(FilePath, true);
                Console.WriteLine("Carpeta eliminada con éxito.");
            }
            //Crea la carpeta
            Directory.CreateDirectory(FilePath);
            Console.WriteLine("Carpeta creada con éxito.");
        }

        internal static void VerificarArchivoDescargado()
        {
            Thread.Sleep(10000);
            string[] files = Directory.GetFiles(FilePath);
            if (files.Length > 0)
            {
                string fileName = Path.GetFileName(files[0]);
                Console.WriteLine("Nombre del archivo descargado: " + fileName);
            }
            else
            {
                Console.WriteLine("No existe archivo");
            }
        }

        internal static void NavegarAFecha(string Options, IWebDriver driver, By by = null, string fechaPersonalizada = "dd/MM/yyyy")
        {
            Thread.Sleep(5000);
            if (by == null)
            {
                by = By.Id("txtFecha");
            }
            var element = driver.FindElement(by);

            // Permitir la edición del campo de entrada eliminando el atributo 'readonly' utilizando JavaScript
            driver.ExecuteJavaScript("arguments[0].removeAttribute('readonly')", element);
            WaitForElementToBeVisible(By.CssSelector(".Zebra_DatePicker_Icon"), driver);
            String fecha = "";
            DateTime today = DateTime.Now;
            if (Options == "LunesSemanaAnterior")
            {
                DateTime lastMonday = today.AddDays(-(int)today.DayOfWeek - 6);
                fecha = lastMonday.ToString("dd/MM/yyyy");

            }
            else if (Options == "MesAnteriorDia1")
            {
                DateTime firstDayMonth = today.AddMonths(-1).AddDays(-today.Day + 1);
                fecha = firstDayMonth.ToString("dd/MM/yyyy");
            }
            else if (Options == "Ayer")
            {
                DateTime yesterday = today.AddDays(-1);
                fecha = yesterday.ToString("dd/MM/yyyy");
            }
            else if (Options == "FechaPersonalizada")
            {
                fecha = fechaPersonalizada;
            }
            else
            {
                DateTime firstDayLastMonth = today.AddDays(-today.Day + 1);
                fecha = firstDayLastMonth.ToString("dd/MM/yyyy");
            }
            element.Clear();
            element.SendKeys(fecha);
            driver.ExecuteJavaScript("arguments[0].setAttribute('readonly', 'readonly')", element);
        }

        internal static void Otrasfechas(string Options, IWebDriver driver, By by = null)
        {
            Thread.Sleep(5000);
            if (by == null)
            {
                by = By.Id("txtFecha");
            }
            var element = driver.FindElement(by);

            // Permitir la edición del campo de entrada eliminando el atributo 'readonly' utilizando JavaScript
            driver.ExecuteJavaScript("arguments[0].removeAttribute('readonly')", element);
            WaitForElementToBeVisible(By.CssSelector(".Zebra_DatePicker_Icon"), driver);
            String fecha = "";
            DateTime today = DateTime.Now;
            if (Options == "Ultimodiames")
            {
                DateTime lastDayLastMonth = today.AddDays(-today.Day);
                fecha = lastDayLastMonth.ToString("dd/MM/yyyy");
            }
            else if (Options == "Ayer")
            {
                DateTime lastDayLastMonth = today.AddDays(-1);
                fecha = lastDayLastMonth.ToString("dd/MM/yyyy");
            }
            else if (Options == "Manana")
            {
                DateTime lastDayLastMonth = today.AddDays(+1);
                fecha = lastDayLastMonth.ToString("dd/MM/yyyy hh:mm:ss");
            }
            else {
                fecha = today.ToString("dd/MM/yyyy");
            }
            element.Clear();
            element.SendKeys(fecha);
            driver.ExecuteJavaScript("arguments[0].setAttribute('readonly', 'readonly')", element);
        }

        internal static void WaitForElementToBeVisible(By locator,IWebDriver driver, bool click = false )
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var elementos = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
            if (click) { elementos.Click(); }
        }

        internal static void NavegarAFechaDrowpdown(By locator, IWebDriver driver)
        {
            var Mes = DateTime.Now.AddMonths(-1);
            string NombreMes = Mes.ToString("MMMM.yyyy");
            string MesAnterior = NombreMes.Substring(0, 1).ToUpper() + NombreMes.Substring(1).ToLower();

            SelectOptionfromDropdown(locator, driver, MesAnterior);

        }

        internal static void NavegarAnnioDrowpdown(By locator, IWebDriver driver)
        {
            var Annio = DateTime.Now;
            string NombreAnnio = Annio.ToString("yyyy");
            string AnnioActual = NombreAnnio.Substring(0, 1).ToUpper() + NombreAnnio.Substring(1).ToLower();

            SelectOptionfromDropdown(locator, driver, AnnioActual);

        }

        internal static void NavegarMesDrowpdown(By locator, IWebDriver driver)
        {
            var Mes = DateTime.Now;
            string NombreMes = Mes.ToString("MMMM");
            string MesAnterior = NombreMes.Substring(0, 1).ToUpper() + NombreMes.Substring(1).ToUpper();
            SelectOptionfromDropdown(locator, driver, MesAnterior);

        }

        internal static void SelectOptionfromDropdown(By locator, IWebDriver driver, string option, bool porIndice=false, int indice=0)
        {
            WaitForElementToBeVisible(locator, driver, true);
            var dropdown = driver.FindElement(locator);
            SelectElement SelectOptions = new SelectElement(dropdown);
            if (porIndice)
            {
                SelectOptions.SelectByIndex(indice);
            }
            else{
                SelectOptions.SelectByText(option);
            }
        }

        internal static void InvisibilityLoading(IWebDriver driver)
        {
            var elements = driver.FindElement(By.Id("loading"));
            if (!elements.Displayed)
            {
                WaitForElementToBeVisible(By.Id("loading"), driver, false);
            }
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.Id("loading")));
        }

        /*
         * Método para realizar un doble clic en un elemento, abrir un área de texto para edición y enviar texto.
         * 
         * Parámetros:
         * - locator: Objeto By que identifica el elemento en el que se realizará el doble clic.
         * - driver: Objeto que implementa la interfaz IWebDriver.
         * - keys: Cadena que representa el texto que se enviará al área de texto para su edición.
         * - textArea: Objeto By que identifica el área de texto donde se enviará el texto.
         */
        internal static void dobleClickAndEdit(By locator, IWebDriver driver, string keys, By textArea)
        {
            Actions builder = new Actions(driver);
            var celda = driver.FindElement(locator);
            builder.DoubleClick(celda).Perform();
            builder.DoubleClick(celda).Perform();
            var textAreaElement = driver.FindElement(textArea);
            textAreaElement.Clear();
            textAreaElement.SendKeys(keys);
        }

         /*
         * Método para navegar a un período de tiempo específico.
         * 
         * Parámetros:
         * - locator: Objeto By que identifica el elemento de entrada donde se ingresará la fecha.
         * - driver: Objeto que implementa la interfaz IWebDriver.
         * - option: Cadena que indica el tipo de navegación que se realizará. Puede ser "MesSiguiente" o "MesAnterior".
         */
        internal static void NavegarAPeriodo(By locator, IWebDriver driver, string option)
        {
            DateTime today = DateTime.Now;
            var element = driver.FindElement(locator);

            // Permitir la edición del campo de entrada eliminando el atributo 'readonly' utilizando JavaScript
            driver.ExecuteJavaScript("arguments[0].removeAttribute('readonly')", element);

            WaitForElementToBeVisible(locator, driver, false);
            element.Clear();
            string date = "";

            if (option == "MesSiguiente")
            {
                DateTime Monthg = today.AddMonths(1);
                date = Monthg.ToString("MM yyyy");
            }
            else if (option == "MesAnterior")
            {
                DateTime Monthg = today.AddMonths(-1);
                date = Monthg.ToString("MM yyyy");
            }

            element.SendKeys(date);
            driver.ExecuteJavaScript("arguments[0].setAttribute('readonly', 'readonly')", element);
        }

        internal static void WaitFor(int seconds = 5)
        {
            Thread.Sleep(seconds*1000);
        }

        internal static void dobleClick(By locator, IWebDriver driver/*, string keys, By textArea*/)
        {
            Actions builder = new Actions(driver);
            var elemento = driver.FindElement(locator);
            builder.DoubleClick(elemento).Perform();

        }

        internal static void SelectOptionFromList(By locator, IWebDriver driver, int opcion, string tag/*, string keys, By textArea*/)
        {
            var lista = driver.FindElement(locator);
            var opciones = lista.FindElements(By.TagName(tag));
            opciones[opcion].Click();

        }

    }
}