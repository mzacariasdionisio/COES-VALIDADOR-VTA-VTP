using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("No aplica")]
    public class Intranet_FDTI69_ConsultasTabulares
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }


        //[Test, Order(1)]
        public void Intranet_FDTI69_Ingresar_A_La_Opcion()
        {
            //driver = Test_Suite.SetUpClass();
            //FuncionesRecurrentes.EliminarYCrearCarpeta();
            //FuncionesRecurrentes.loginIntranet(driver);
            //driver.FindElement(By.LinkText("Programación")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Intervenciones"), driver, true);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Consultas Tabulares"), driver, true);
            //FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI69_Consultar_registros()
        {
            //FuncionesRecurrentes.InvisibilityLoading(driver);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cboTipoProgramacion']"), driver, true);
            //FuncionesRecurrentes.SelectOptionfromDropdown(By.XPath("//select[@id='cboTipoProgramacion']"), driver, "EJECUTADOS");
            //FuncionesRecurrentes.InvisibilityLoading(driver);
            //FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.XPath("//input[@id='InterfechainiD']"));
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector("#btnConsultar span"), driver, true);
            //FuncionesRecurrentes.InvisibilityLoading(driver);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("TablaConsultaIntervencion_wrapper"), driver);
        }

        //[Test, Order(3)]
        public void Intranet_FDTI69_Reporte()
        {
            //try
            //{
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector("#btnExportarExcel .set_size"), driver, true);
            //    FuncionesRecurrentes.InvisibilityLoading(driver);
            //    FuncionesRecurrentes.VerificarArchivoDescargado();
            //    driver.Quit();
            //}
            //catch (Exception e)
            //{
            //    driver.Quit();
            //    Assert.IsTrue(false, "Error al descargar la información: " + e.Message);
            //}
        }
    }
}