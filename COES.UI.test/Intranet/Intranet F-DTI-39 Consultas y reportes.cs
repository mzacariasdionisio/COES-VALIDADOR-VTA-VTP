
using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;



namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI39_Consultas_reportes
    {
        //private string variable;
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI39_Consultas_reportes_Ingresar_a_la_opcion()

        {

            driver = Test_Suite.SetUpClass();

            //Login
            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Evaluación")).Click();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Costo de Oportunidad"), driver);

            driver.FindElement(By.LinkText("Costo de Oportunidad")).Click();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("4. Consultas y reportes"), driver);

            driver.FindElement(By.LinkText("4. Consultas y reportes")).Click();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[contains(@id,\'cbPeriodo\')]"), driver);

            driver.FindElement(By.XPath("//select[contains(@id,\'cbPeriodo\')]")).Click();
            {
                var dropdown = driver.FindElement(By.XPath("//select[contains(@id,\'cbPeriodo\')]"));
                dropdown.FindElement(By.XPath("//option[. = 'Diciembre.2023']")).Click();
            }
            driver.FindElement(By.XPath("//select[contains(@id,\'cbVersion\')]")).Click();
            {
                var dropdown = driver.FindElement(By.XPath("//select[contains(@id,\'cbVersion\')]"));
                dropdown.FindElement(By.XPath("//option[. = 'Final Mensual']")).Click();
            }

            driver.FindElement(By.XPath("//input[contains(@value,\'Consultar\')]")).Click();

        }

        [Test, Order(2)]
        public void Intranet_FDTI39_Consultas_reportes_Exportar_registros()

        {
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            {
                var elements = driver.FindElements(By.Id("//div[contains(text(),'Procesando...')]"));
                Assert.True(elements.Count == 0);
            }

            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("divResultado"), driver);
            FuncionesRecurrentes.WaitFor(5);
            driver.FindElement(By.Id("divResultado")).Click();

            var exportarButtonLocator = By.XPath("//input[contains(@value,'Exportar')]");

            new WebDriverWait(driver, TimeSpan.FromSeconds(30))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(exportarButtonLocator));

            driver.FindElement(exportarButtonLocator).Click();

            FuncionesRecurrentes.WaitFor(5);
            FuncionesRecurrentes.VerificarArchivoDescargado();

        }
        [Test, Order(3)]
        public void Intranet_FDTI39_Consultas_reportes_Publicacion_de_registros()

        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[contains(@value,\'Publicación\')]"), driver);
                driver.FindElement(By.XPath("//input[contains(@value,\'Publicación\')]")).Click();

                FuncionesRecurrentes.WaitFor(10);

                FuncionesRecurrentes.VerificarArchivoDescargado();

                driver.Quit();
            }
            catch (Exception e)
            {
                driver.Quit();
                Assert.IsTrue(false, "Error al descargar la información: " + e.InnerException.Message);
            }
        }

        }
}

