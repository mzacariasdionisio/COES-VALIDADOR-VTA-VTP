
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI27_Reporte_costos_incrementales
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        

        [Test, Order(1)]
        public void Intranet_FDTI27_Reporte_costos_incrementales_Ingresar_A_La_Opcion()
        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Coordinación")).Click();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Reporte de Costos Incrementales"), driver, true);

        }
        [Test, Order(2)]
        public void Intranet_FDTI27_Reporte_costos_incrementales_Consulta_de_registros()
        {
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("Fecha"));
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnConsultar"), driver, true);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector(".search-content"), driver);
        }

        [Test, Order(3)]
        public void Intranet_FDTI27_Reporte_costos_incrementales_Exportar_registros()
        {
            try
            {
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                FuncionesRecurrentes.WaitFor(5);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector("td > a > img"), driver, true);

                FuncionesRecurrentes.WaitFor(5);

                FuncionesRecurrentes.VerificarArchivoDescargado();
                FuncionesRecurrentes.WaitFor(5);
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