using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System.Linq;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI62_CDispatch
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        
        
        [Test, Order(1)]
        public void Intranet_FDTI62_CDispatch_Ingresar_A_La_Opcion()
        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Coordinación"), driver, true);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("CDispatch"), driver, true);

            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Intranet_FDTI62_CDispatch_Consulta_De_Registros()
        {
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("txtFechaini"));
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("txtFechafin"));

            driver.FindElement(By.Id("btnConsultar")).Click();
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tb_info']"), driver);
            var elementos = driver.FindElements(By.XPath("//table[@id='tb_info']/tbody/tr")).Count();
            Console.WriteLine("Elementos de la tabla: " + elementos);
        }

        [Test, Order(3)]
        public void Intranet_FDTI62_CDispatch_Exportar_Registros()
        {
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            driver.FindElement(By.CssSelector("#btnExportar span")).Click();
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(4)]
        public void Intranet_FDTI62_CDispatch_Detalle_Adicional()
        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("check_detalleAdicional"), driver, true);

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tb_info']"), driver);
                var elementos = driver.FindElements(By.XPath("//table[@id='tb_info']/tbody/tr")).Count();
                Console.WriteLine("Elementos de la tabla: " + elementos);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally { driver.Quit(); }

        }
    }
}