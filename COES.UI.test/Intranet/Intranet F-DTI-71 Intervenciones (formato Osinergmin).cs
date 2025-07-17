using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;


namespace Intranet
{
    //[TestFixture]
    //[Ignore("No aplica")]
    public class Intranet_FDTI71_Intervenciones
    {
        //private string variable;
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }


        //[Test, Order(1)]
        public void Intranet_FDTI71_Intervenciones_Ingresar_a_la_opcion()
        {
            //driver = Test_Suite.SetUpClass();

            ////Login
            //FuncionesRecurrentes.loginIntranet(driver);

            //driver.FindElement(By.LinkText("Programación")).Click();

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Intervenciones"), driver, true);

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Reportes"), driver, true);

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Intervenciones (Formato OSINERGMIN)"), driver, true);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI71_Intervenciones_Consulta_registros()
        {
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='tipoProCodi']"), driver, true);

            //{
            //    var dropdown = driver.FindElement(By.XPath("//select[@id='tipoProCodi']"));
            //    SelectElement SelectOptions = new SelectElement(dropdown);
            //    SelectOptions.SelectByText("EJECUTADOS");
            //}

            //FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.XPath("//input[@id='Entidad_Interfechaini']"));

            //driver.FindElement(By.XPath("//input[@id='Entidad_Interfechafin']")).Click();

            //driver.FindElement(By.XPath("//table[@class='dp_daypicker dp_body']//td[contains(., '1')]")).Click();

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnBuscar']"), driver, true);

            //FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        //[Test, Order(3)]
        public void Intranet_FDTI71_Intervenciones_Imprimir()
        {
            //FuncionesRecurrentes.EliminarYCrearCarpeta();
            //driver.FindElement(By.XPath("//input[@id='btnImprimir']")).Click();
            //FuncionesRecurrentes.InvisibilityLoading(driver);
            //FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        //[Test, Order(4)]
        public void Intranet_FDTI71_Intervenciones_Generar_Excel()
        {
            //try
            //{
            //    FuncionesRecurrentes.EliminarYCrearCarpeta();
            //    driver.FindElement(By.XPath("//input[@id='btnGenerarExcel']")).Click();
            //    FuncionesRecurrentes.InvisibilityLoading(driver);
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