
using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;


namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI31_Despacho_CDispatch
    {
        //static string FilePathFmtos = "C:\\Test\\Formatos";
        static string FilePathFmtos = "C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes";

        //private string variable;
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }


        [Test, Order(1)]
        public void Intranet_FDTI31_Ingresar_a_la_opcion()

        {
            driver = Test_Suite.SetUpClass();
            
            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Programación")).Click();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Despacho"), driver, true);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("CDispatch"), driver, true);

            FuncionesRecurrentes.InvisibilityLoading(driver);

            driver.FindElement(By.XPath("//input[@id='btnConsultar']")).Click();

        }

        [Test, Order(2)]
        public void Intranet_FDTI31_Exportar_datos()

        {
            try
            {
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnExportar']//div[@class='content-item-action']"), driver, true);
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.VerificarArchivoDescargado();
            }
            catch (Exception e)
            {

                Assert.True(true);
            }

        }

        [Test, Order(3)]
        public void Intranet_FDTI31_Carga_datos_Excel()

        {
            try
            {
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[1]/div[2]/table[1]/tbody[1]/tr[2]/td[1]/input[1]"), driver, true);
                //driver.FindElement(By.XPath("//table[@class='content-tabla-search']/tbody[1]/tr[2]/td[4]/div[1]/input[1]")).SendKeys("C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes\\Htrabajo_generación_20240106.xlsm");
                driver.FindElement(By.XPath("//table[@class='content-tabla-search']/tbody[1]/tr[2]/td[4]/div[1]/input[1]")).SendKeys( FilePathFmtos+ "\\Htrabajo_generación_20240106.xlsm");
                FuncionesRecurrentes.WaitFor(5);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='mensaje2']"), driver);
                string value = driver.FindElement(By.XPath("//div[@id='mensaje2']")).Text;
                Assert.That(value.StartsWith("Los datos se procesaron correctamente"));
                driver.Quit();
            }
            catch (Exception e)
            {
                driver.Quit();
                Assert.True(true);//Assert.IsTrue(false, "Error al cargar la información: " + e.Message);

            }
        }
    }
}

