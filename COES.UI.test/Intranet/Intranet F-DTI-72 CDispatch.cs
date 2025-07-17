
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI72_CDispatch
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        

        [Test, Order(1)]
        public void Intranet_FDTI72_CDispatch_Ingresar_a_la_opcion()
        {
            driver = Test_Suite.SetUpClass();
            
            FuncionesRecurrentes.loginIntranet(driver);
            driver.FindElement(By.LinkText("Programación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Despacho"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("CDispatch"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Intranet_FDTI72_CDispatch_Consulta_de_registros()
        {
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("txtFechaini"));
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("txtFechafin"));
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnConsultar"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector(".panel-container"), driver);
        }

        [Test, Order(3)]
        public void Intranet_FDTI72_CDispatch_Exportar_registros()
        {
            try
            {
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector(".panel-container"), driver);
                driver.FindElement(By.CssSelector("#btnExportar span")).Click();
                FuncionesRecurrentes.VerificarArchivoDescargado();
                //driver.Quit();
            }
            catch (Exception e)
            {
                //driver.Quit();
                //Assert.IsTrue(false, "Error al descargar la información: " + e.InnerException.Message);

                Assert.Fail(e.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}