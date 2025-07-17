using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI38_2_Despacho_conosin_reserva
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI38_2_Despacho_conosin_reserva_Generadores_Ingresar_a_la_opcion()
        {
            driver = Test_Suite.SetUpClass();
            Actions actions = new Actions(driver);
            FuncionesRecurrentes.loginIntranet(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Evaluación"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Costo de Oportunidad"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("3. Proceso de cálculo"), driver, true);
        }

        [Test, Order(2)]
        public void Intranet_FDTI38_2_Despacho_conosin_reserva_Consulta_registros()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbPeriodo']"), driver);
            FuncionesRecurrentes.NavegarAFechaDrowpdown(By.XPath("//select[@id='cbPeriodo']"), driver);
            FuncionesRecurrentes.WaitFor(1);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbVersion"), driver, "", true, 1);
            driver.FindElement(By.XPath("//input[@value='Consultar']")).Click();
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='tab-container']/ul[1]/li[1]/a"), driver);
            driver.FindElement(By.XPath("//div[@id='tab-container-resultados']/ul[1]/li[1]/a")).Click();
            driver.FindElement(By.XPath("//div[@id='tab-container-resultados']/ul[1]/li[2]/a")).Click();
            driver.FindElement(By.XPath("//div[@id='tab-container']/ul[1]/li[2]/a")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='tab-container-datos']/ul/li[1]"), driver);
            driver.FindElement(By.XPath("//div[@id='tab-container-datos']/ul/li[2]")).Click();
            driver.FindElement(By.XPath("//div[@id='tab-container-datos']/ul/li[3]")).Click();
            driver.FindElement(By.XPath("//div[@id='tab-container-datos']/ul/li[4]")).Click();
            driver.FindElement(By.XPath("//div[@id='tab-container-datos']/ul/li[5]")).Click();
            driver.FindElement(By.XPath("//div[@id='tab-container-datos']/ul/li[6]")).Click();
            driver.FindElement(By.XPath("//div[@id='tab-container']/ul[1]/li[1]/a")).Click();
        }

        [Test, Order(3)]
        public void Intranet_FDTI38_2_Despacho_conosin_reserva_Exportar_proceso_calculo()
        {
            if (driver.FindElement(By.Id("btnExportarResultado")).Displayed)
            {
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnExportarResultado"), driver, true);
                FuncionesRecurrentes.VerificarArchivoDescargado();
            }
            else
            {
                Console.WriteLine("No se encontró el botón Exportar");
            }
        }

        [Test, Order(4)]
        public void Intranet_FDTI38_2_Despacho_conosin_reserva_Exportar_consulta_insumos()
        {
            try
            {
                driver.FindElement(By.XPath("//div[@id='tab-container']/ul[1]/li[2]/a")).Click();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnExportarInsumo"), driver, true);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.VerificarArchivoDescargado();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}