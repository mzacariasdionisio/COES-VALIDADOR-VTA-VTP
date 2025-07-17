
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;
using System.IO;
using OpenQA.Selenium.Support.Extensions;
using System.Threading;

namespace Extranet
{
    public class FuncionesRecurrentes
    {
        public static string FilePath = "C:\\Test\\Descargas";
        //public static string issSite = "https://preprodcloud.coes.org.pe/EXTRANET-2024-003285//webform/Account/login.aspx";
        //public static string issSite = "https://preprodcloud.coes.org.pe/EXTRANET-CAMPANIA//webform/Account/login.aspx";

        //Local
        //public static string issSite = "https://pruebascloud.coes.org.pe/EXTRANET-ITE-2-I06CALCULOPORCENTAJESPTOANUAL2024//webform/Account/login.aspx";
        //public static string userSelenium = "selenium";
        //public static string pwdSelenium = "selenium";

        //producción
        public static string issSite = Environment.GetEnvironmentVariable("IssSite") + "//webform/Account/login.aspx";
        public static string userSelenium = Environment.GetEnvironmentVariable("userSelenium");
        public static string pwdSelenium = Environment.GetEnvironmentVariable("pwdSelenium");

        internal static void loginExtranet(IWebDriver driver)
        {
            Console.WriteLine(issSite);
            driver.Navigate().GoToUrl(url: issSite);
            WaitForElementToBeVisible(By.Id("MainContent_TextBoxUserLogin"), driver, true);
            driver.FindElement(By.Id("MainContent_TextBoxUserLogin")).SendKeys(userSelenium);
            WaitForElementToBeVisible(By.Id("MainContent_TextBoxUserPassword"), driver, true);
            driver.FindElement(By.Id("MainContent_TextBoxUserPassword")).SendKeys(pwdSelenium);
            driver.FindElement(By.Id("MainContent_ButtonLogin")).Click();
            WaitForElementToBeVisible(By.XPath("//div[@class='form-title']"),driver);
            Assert.That(driver.FindElement(By.CssSelector(".form-title")).Text, Is.EqualTo("BIENVENIDO A LA EXTRANET COES"));
        }

        internal static void EliminarYCrearCarpeta()
        {
            if (Directory.Exists(FilePath))
            {
                Directory.Delete(FilePath, true);
                Console.WriteLine("Carpeta eliminada con éxito.");
            }
            //Crea la carpeta si no existe 
            Directory.CreateDirectory(FilePath);
            Console.WriteLine("Carpeta creada con éxito.");
        }

        internal static void VerificarArchivoDescargado()
        {
            Thread.Sleep(10000);
            string[] files = Directory.GetFiles(FilePath);
            if (files.Length> 0)
            {
                string fileName = Path.GetFileName(files[0]);
                Console.WriteLine("Nombre del archivo descargado: " + fileName);
            }
            else
            {
                Console.WriteLine("No existe archivo");
            }
        }

        /*
         * Método para navegar a una fecha específica en un calendario de selección de fechas.
         * 
         * Parámetros:
         * - Options: Cadena que indica la opción de navegación. Puede ser "LunesSemanaAnterior" o "MesAnteriorDia1" o "PrimerDíaMesActual".
         *              Por default selecciona el primer día del mes actual.
         * - driver: Objeto que implementa la interfaz IWebDriver, representa el navegador web utilizado para la automatización.
         */
        /*internal static void NavegarAFecha(string Options, IWebDriver driver)
        {
            string ruta = "//table[@class='dp_daypicker dp_body']//td[contains(., '1')]";
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                wait.Until(_driver => driver.FindElement(By.CssSelector(".Zebra_DatePicker_Icon")).Displayed);
            }
            driver.FindElement(By.CssSelector(".Zebra_DatePicker_Icon")).Click();
            if (Options == "LunesSemanaAnterior")
            {
                DateTime today = DateTime.Now;

                DateTime lastMonday = today.AddDays(-(int)today.DayOfWeek - 6);

                ruta = "//table[@class='dp_daypicker dp_body']//td[contains(., '" + lastMonday.Day + "')]";
                if (lastMonday.Month < today.Month)
                {
                    driver.FindElement(By.CssSelector(".dp_previous")).Click();
                }
            }
            else if (Options == "MesAnteriorDia1")
            {
                driver.FindElement(By.CssSelector(".dp_previous")).Click();
            }

            driver.FindElement(By.XPath(ruta)).Click();
        }*/
        internal static void NavegarAFecha(string Options, IWebDriver driver, By by = null)
        {
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

            else if (Options == "Hoy")
            {
                fecha = today.ToString("dd/MM/yyyy");
                Console.WriteLine("Hoy");
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

        /*
         * Método para esperar a que un elemento sea visible en una página web.
         * 
         * Parámetros:
         * - locator: Objeto By que identifica el elemento que se espera que sea visible.
         * - driver: Objeto que implementa la interfaz IWebDriver.
         * - click: Valor booleano opcional que indica si se debe hacer clic en el elemento una vez que sea visible. Por defecto, es false.
         */
        internal static void WaitForElementToBeVisible(By locator, IWebDriver driver, bool click = false)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var elementos = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
            if (click) { elementos.Click(); }
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
         * Método para seleccionar una opción de un dropdown en una página web.
         * 
         * Parámetros:
         * - locator: Objeto By que identifica el elemento del dropdown.
         * - driver: Objeto que implementa la interfaz IWebDriver.
         * - option: Cadena que representa la opción que se desea seleccionar en el dropdown.
         */
        internal static void SelectOptionfromDropdown(By locator, IWebDriver driver, string option, bool porIndice = false, int indice = 0)
        {/*
            WaitForElementToBeVisible(locator, driver, true);
            var dropdown = driver.FindElement(locator);
            if (porIndice)
            {
                SelectElement SelectOptions = new SelectElement(dropdown);
                SelectOptions.SelectByIndex(indice);
            }
            else
            {
                dropdown.FindElement(By.XPath("//option[. = '" + option + "']")).Click();
            }*/
        
            WaitForElementToBeVisible(locator, driver, true);
            SelectElement selectElement = new SelectElement(driver.FindElement(locator));
            if (porIndice)
            {
                selectElement.SelectByIndex(indice);
            }
            else
            {
                selectElement.SelectByText(option);
            }

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

        internal static void WaitFor(int seconds = 5)
        {
            Thread.Sleep(seconds*1000);
        }
}
}