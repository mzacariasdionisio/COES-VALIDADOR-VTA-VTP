using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("No aplica")]
    public class Intranet_FDTI34_Reporte_de_consistencia
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        //[Test, Order(1)]
        public void Intranet_FDTI34_Reporte_de_consistencia_Ingresar_a_la_opcion()

        {
            //driver = Test_Suite.SetUpClass();

            //FuncionesRecurrentes.loginIntranet(driver);

            //driver.FindElement(By.LinkText("Evaluación")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Cumplimiento Servicio RPF"), driver, true);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Reporte de Consistencia"), driver, true);
            //FuncionesRecurrentes.NavegarAFecha("LunesSemanaAnterior", driver, By.XPath("//input[@id='FechaConsulta']"));

            //driver.FindElement(By.XPath("//input[@id='btnConsultar']")).Click();
        }

        //[Test, Order(2)]
        public void Intranet_FDTI34_Reporte_de_consistencia_Exportar_registros()

        {
            //FuncionesRecurrentes.WaitFor(1);
            //try
            //{
            //    FuncionesRecurrentes.EliminarYCrearCarpeta();
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Exportar']"), driver, true);
            //    FuncionesRecurrentes.VerificarArchivoDescargado();
            //}
            //catch (Exception e)
            //{
            //    Assert.Fail(e.Message);
            //}
            //finally { driver.Quit();
            //}
        }
    }
}