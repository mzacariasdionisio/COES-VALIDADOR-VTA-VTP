using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Linq;
using System;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("No aplica")]
    public class Intranet_FDTI63_Reportes_Osinergmin
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }


        //[Test, Order(1)]
        public void Intranet_FDTI63_Reportes_Osinergmin_Ingresar_a_la_opcion()
        {
            //driver = Test_Suite.SetUpClass();

            //FuncionesRecurrentes.loginIntranet(driver);

            //driver.FindElement(By.LinkText("Osinergmin")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Informes Osinergmin (Mant. y Horas Operac.)"), driver, true);

            //FuncionesRecurrentes.WaitFor(5);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI63_Reportes_Osinergmin_Consulta_de_registros()
        {
            //FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);

            //FuncionesRecurrentes.WaitFor(5);

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tb_info']"), driver);
            //var elementos = driver.FindElements(By.XPath("//table[@id='tb_info']/tbody/tr")).Count();
            //Console.WriteLine("Elementos de la tabla: " + elementos);
        }

        //[Test, Order(3)]
        public void Intranet_FDTI63_Reportes_Osinergmin_Exportar_registros()
        {
            //FuncionesRecurrentes.EliminarYCrearCarpeta();
            //try
            //{
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnExportar"), driver);
            //    driver.FindElement(By.Id("btnExportar")).Click();
            //    FuncionesRecurrentes.WaitFor(10);
            //    FuncionesRecurrentes.VerificarArchivoDescargado();
            //}
            //catch (Exception e)
            //{
            //    Assert.Fail(e.Message);
            //}
            //finally {
            //    driver.Quit();
            //}
        }
    }
}