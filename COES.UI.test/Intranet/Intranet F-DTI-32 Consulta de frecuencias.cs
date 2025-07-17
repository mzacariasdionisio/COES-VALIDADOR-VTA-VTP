
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System.Linq;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI32_Consulta_de_frecuencias
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        
        [Test, Order(1)]
        public void Intranet_FDTI32_Consulta_de_frecuencias_Ingresar_a_la_opcion()
        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Evaluación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Cumplimiento Servicio RPF"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Consulta de frecuencias"), driver, true);

            FuncionesRecurrentes.NavegarAFecha("LunesSemanaAnterior", driver, By.XPath("//input[@id='txtFecha']"));

            driver.FindElement(By.XPath("//input[@id='btnConsultar']")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tabla']"), driver);
            var elementos = driver.FindElements(By.XPath("//table[@id='tabla']/tbody/tr")).Count();
            Console.WriteLine("Elementos tabla:" + elementos);
            Assert.True(elementos > 0, "Tabla no encontrada");
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Intranet_FDTI32_Consulta_de_frecuencias_Descarga_de_datos()
        {
            try
            {
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Descargar datos"), driver, true);
                FuncionesRecurrentes.VerificarArchivoDescargado();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}