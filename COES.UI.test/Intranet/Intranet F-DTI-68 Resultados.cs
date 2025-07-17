using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("No aplica")]
    public class Intranet_FDTI68_Resultados
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        //[Test, Order(1)]
        public void Intranet_FDTI68_Resultados_Ingresar_A_La_Opcion()
        {
            //driver = Test_Suite.SetUpClass();
            //FuncionesRecurrentes.EliminarYCrearCarpeta();

            //FuncionesRecurrentes.loginIntranet64bits(driver);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Costos Marginales Nodales"), driver, true);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Resultados"), driver, true);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI68_Resultados_Exportacion_masiva()
        {
            //FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);
            //FuncionesRecurrentes.WaitFor(5);
            //driver.FindElement(By.XPath("//input[@id='txtFecha']")).Click();
            //FuncionesRecurrentes.WaitFor(5);
            //driver.FindElement(By.XPath("//input[@value='Consultar']")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//img[contains(@title,'Muestra los resultados por barra')])[1]"), driver);
        }

        //[Test, Order(3)]
        public void Intranet_FDTI68_Resultados_Editar_registro()
        {
            //try
            //{
            //    driver.FindElement(By.XPath("//input[@value='Exportar Masivo']")).Click();
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito'][contains(.,'Seleccione el rango de fechas a exportar:')]"), driver);
            //    FuncionesRecurrentes.NavegarAFecha("", driver, By.XPath("//input[@id='txtExportarDesde']"));
            //    //driver.FindElement(By.Id("//input[@id='txtExportarDesde']")).Click();
            //    //FuncionesRecurrentes.WaitFor(10);
            //    driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[2]/div[4]/input[1]")).Click();
            //    FuncionesRecurrentes.VerificarArchivoDescargado();
            //    FuncionesRecurrentes.WaitFor(40);
            //    driver.Quit();
            //}
            //catch
            //{
            //    driver.Quit();
            //    Assert.IsTrue(true, "False positive");
            //}
        }
    }
}