using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("21.06.24: CNalvarte indica que esta prueba no aplica")]
    public class Intranet_FDTI48_Consulta_de_mantenimientos
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        //[Test, Order(1)]
        public void Intranet_FDTI48_Consulta_de_mantenimientos_Ingresar_a_la_opcion()
        {
            //driver = Test_Suite.SetUpClass();

            //FuncionesRecurrentes.loginIntranet(driver);

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Eventos"), driver, true);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Consulta de Mantenimientos"), driver, true);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI48_Consulta_de_mantenimientos_Consulta_de_registros()
        {
            //FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.XPath("//input[@id='FechaDesde']"));
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnBuscar"), driver, true);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id=\'tabla_wrapper\']"), driver);
        }

        //[Test, Order(3)]
        public void Intranet_FDTI48_Consulta_de_mantenimientos_Exportar_reporte()
        {
            //FuncionesRecurrentes.EliminarYCrearCarpeta();
            //driver.FindElement(By.XPath("//input[@id='btnExpotar']")).Click();
            //FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        //[Test, Order(4)]
        public void Intranet_FDTI48_Consulta_de_mantenimientos_Ver_graficos()
        {
            //try
            //{
            //    driver.FindElement(By.Id("btnGrafico")).Click();
            //    FuncionesRecurrentes.InvisibilityLoading(driver);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector(".panel-container"), driver);

            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Gráfico 1"), driver, true);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Gráfico 2"), driver, true);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Gráfico 3"), driver, true);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Gráfico 4"), driver, true);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Gráfico 5"), driver, true);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Gráfico 6"), driver, true);
            //}
            //catch (Exception e)
            //{
            //    Assert.Fail(e.Message);
            //}
            //finally
            //{
            //    driver.Quit();
            //}
        }
    }
}