
using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;


namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI40_Historico_SP7
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }


        [Test, Order(1)]
        public void Intranet_FDTI40_Historico_SP7_Ingresar_a_la_opcion()

        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Coordinación")).Click();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Tiempo Real SP7"), driver, true);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Histórico SP7"), driver, true);

        }

        [Test, Order(2)]
        public void Intranet_FDTI40_Historico_SP7_Consulta_registros()

        {
            //try
            //{
                FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.NavegarAFecha(
                "FechaPersonalizada",
                driver,
                By.XPath("//input[@id='txtFechaDesde']"),
                DateTime.Now.AddMonths(-1).AddDays(-DateTime.Now.Day + 1).ToString("yyyy-MM-dd")
);

            FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='ss-single-selected']"), driver, true);
                FuncionesRecurrentes.WaitFor(5);
                FuncionesRecurrentes.SelectOptionFromList(By.XPath("//div[@class='ss-content ss-open']//div[@class='ss-list']"), driver, 1, "div");
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='ss-multi-selected']"), driver, true);
                FuncionesRecurrentes.WaitFor(5);
                FuncionesRecurrentes.SelectOptionFromList(By.XPath("//div[@class='ss-content ss-open']//div[@class='ss-list']"), driver, 1, "div");
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnBuscar']"), driver, true);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[normalize-space()='Lista de Valores']"), driver, true);
            //}
            //catch (Exception e)
            //{
            //    Assert.True(true);
            //}
        }

        [Test, Order(3)]
        public void Intranet_FDTI40_Historico_SP7_Exportar_registros()

        {
            //try
            //{
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnExportar']"), driver, true);
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.VerificarArchivoDescargado();
            FuncionesRecurrentes.InvisibilityLoading(driver);
            //}
            //catch (Exception e)
            //{

            //    Assert.True(true);
            //}
        }


        [Test, Order(4)]
        public void Intranet_FDTI40_Historico_SP7_Exportar_registros_FormatoCSV()

        {
            try
            {
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnExportarCSV']"), driver, true);
                //FuncionesRecurrentes.EliminarYCrearCarpeta();
                FuncionesRecurrentes.VerificarArchivoDescargado();
                //FuncionesRecurrentes.InvisibilityLoading(driver);

                //driver.Quit();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                //driver.Quit();
                //Assert.True(true);//Assert.IsTrue(false, "Error al descargar la información: " + e.InnerException.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}

