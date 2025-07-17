using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("No aplica")]
    public class Intranet_FDTI52_Reporte_cumplimiento
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }


        //[Test, Order(1)]
        public void Intranet_FDTI52_Reporte_cumplimiento_Ingresar_A_La_Opcion()
        {
            //driver = Test_Suite.SetUpClass();

            ////Login
            //FuncionesRecurrentes.loginIntranet(driver);

            //driver.FindElement(By.LinkText("Coordinación")).Click();

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Reprograma diario de la operación"), driver, true);

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Reporte de cumplimiento"), driver, true);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI52_Reporte_cumplimiento_Consulta_de_registros()
        {
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cboEstado']"), driver);

            //driver.FindElement(By.XPath("//select[@id='cboEstado']")).Click();

            //{
            //    var dropdown = driver.FindElement(By.XPath("//select[@id='cboEstado']"));
            //    SelectElement SelectOptions = new SelectElement(dropdown);
            //    SelectOptions.SelectByIndex(1);

            //}
            //driver.FindElement(By.XPath("//input[@id=\'btnBuscar\']")).Click();

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id=\'tabla\']"), driver);
        }

        //[Test, Order(3)]
        public void Intranet_FDTI52_Reporte_cumplimiento_Exportar_registros()
        {
            //try
            //{
            //    FuncionesRecurrentes.EliminarYCrearCarpeta();

            //    driver.FindElement(By.XPath("//input[@id='btnExportar']")).Click();

            //    FuncionesRecurrentes.WaitFor(10);

            //    FuncionesRecurrentes.VerificarArchivoDescargado();
            //    driver.Quit();

            //}
            //catch (Exception e)
            //{
            //    driver.Quit();
            //    Assert.IsTrue(false, "Error al descargar la información: " + e.InnerException.Message);
            //}
        }
    }
}