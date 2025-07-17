using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI37_2_Insumos_datosYUPANA
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI37_2_Insumos_datosYUPANA_Ingresar_a_la_opcion()

        {
            driver = Test_Suite.SetUpClass();
            Actions actions = new Actions(driver);

            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Evaluación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Costo de Oportunidad"), driver);
            driver.FindElement(By.LinkText("Costo de Oportunidad")).Click();
            actions.MoveToElement(driver.FindElement(By.LinkText("2. Importar insumos"))).Build().Perform();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("2.2 Datos YUPANA"), driver);
            driver.FindElement(By.LinkText("2.2 Datos YUPANA")).Click();
        }

        [Test, Order(2)]
        public void Intranet_FDTI37_2_Insumos_datosYUPANA_Consulta_de_registros()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbPeriodo']"), driver);
            FuncionesRecurrentes.NavegarAFechaDrowpdown(By.XPath("//select[@id='cbPeriodo']"), driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbVersion']"), driver, true);

            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbVersion"), driver, "", true, 1);
            
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Consultar']"), driver);
            driver.FindElement(By.XPath("//input[@value='Consultar']")).Click();
        }

        [Test, Order(3)]
        public void Intranet_FDTI37_2_Insumos_datosYUPANA_Copiar_datos()
        {
            FuncionesRecurrentes.InvisibilityLoading(driver);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Copiar datos']"), driver);
            driver.FindElement(By.XPath("//input[@value='Copiar datos']")).Click();

            driver.SwitchTo().Alert().Accept();

            FuncionesRecurrentes.InvisibilityLoading(driver);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("Los datos se importaron correctamente. Ahora puede calcular los costos de oportunidad, ingresar a la opción de procesamiento."));
        }

        [Test, Order(4)]
        public void Intranet_FDTI37_2_Insumos_datosYUPANA_Exportar_registros()
        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Exportar']"), driver);
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                driver.FindElement(By.XPath("//input[@value='Exportar']")).Click();

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