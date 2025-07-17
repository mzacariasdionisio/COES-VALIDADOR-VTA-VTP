using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI25_Datos_congestion
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        //public static string issSite = Environment.GetEnvironmentVariable("IssSite");

        [Test, Order(1)]
        public void Intranet_FDTI25_Datos_congestion_Ingresar_A_La_Opción()

        {
            driver = Test_Suite.SetUpClass();
            FuncionesRecurrentes.loginIntranet(driver);
            driver.FindElement(By.LinkText("Eventos")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Horas de Operación"), driver, true);
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                IAlert alert = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                if (alert != null)
                {
                    FuncionesRecurrentes.WaitFor(2);
                    alert.Accept();
                    // Cambiar fecha a ayer
                    FuncionesRecurrentes.NavegarAFecha("LunesSemanaAnterior", driver);

                    // Reintentar clic en "Consultar"
                    driver.FindElement(By.Id("btnConsultar")).Click();

                    FuncionesRecurrentes.InvisibilityLoading(driver);
                }
            }
            catch (WebDriverTimeoutException)
            {
                //Si hay datos , se continua con la prueba
            }
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Congestión"), driver, true);

            FuncionesRecurrentes.InvisibilityLoading(driver);
            
        }

        [Test, Order(2)]
        public void Intranet_FDTI25_Datos_congestion_Registro_datos()
        {
            driver.SwitchTo().Frame(0);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(5);
            driver.FindElement(By.XPath("//div[@id=\'contenedor\']/div/div/div/div/table/tbody/tr/td")).Click();
            FuncionesRecurrentes.dobleClick(By.XPath("//div[@id='contenedor']/div/div/div/div/table/tbody/tr/td[2]"), driver);
            FuncionesRecurrentes.SelectOptionFromList(By.ClassName("select2-results"), driver, 1, "li");
            FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//div[@id='contenedor']/div/div/div/div/table/tbody/tr/td[3]"), driver, "01:00", By.XPath("//div[@id='contenedor']/div[8]/textarea"));
            FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//div[@id='contenedor']/div/div/div/div/table/tbody/tr/td[4]"), driver, "02:00", By.XPath("//div[@id='contenedor']/div[8]/textarea"));

            // doble click para que aparezca el TextArea, con el primer click pierde el valor de la celda por eso click en 5 y luego en 4
            Actions builder = new Actions(driver);
            FuncionesRecurrentes.WaitFor(5);
            builder.DoubleClick(driver.FindElement(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[5]"))).Perform();
            builder.DoubleClick(driver.FindElement(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[4]"))).Perform();
            driver.FindElement(By.XPath("//div[@id='contenedor']/div[8]/textarea")).SendKeys("TEST1");

            if (driver.FindElement(By.Id("btnGrabar")).Displayed)
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnGrabar']"), driver, true);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                /*FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensaje"), driver);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                Assert.That(driver.FindElement(By.XPath("(//div[@id='mensaje'])[1]")).Text.StartsWith("La operación se realizó con éxito."),
                    "El mensaje no coincide con el texto esperado");*/
            }
        }

        [Test, Order(3)]
        public void Intranet_FDTI25_Datos_congestion_Eliminar_registro()

        {
            try
            {
                Actions actions = new Actions(driver);

                IWebElement elemento = driver.FindElement(By.XPath("//div[@id='contenedor']/div/div/div/div/table/tbody/tr/td[4]"));

                actions.ContextClick(elemento).Perform();

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='icon-eliminar']"), driver, true);

                //driver.Quit();
            }
            catch (Exception e)
            {
                //driver.Quit();
                //Assert.True(true);//Assert.IsTrue(false, "Error eliminando fila " + e.InnerException.Message);
                //Assert.True(false);//Assert.IsTrue(false, "Error eliminando fila " + e.InnerException.Message);
                Assert.Fail(e.Message);
            }
            finally
            {
                driver.Quit();
            }  
        }
    }
}


