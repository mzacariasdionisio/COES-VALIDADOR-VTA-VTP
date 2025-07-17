
using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;


namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI37_3_Insumos_Datos_Señales
    {
        //static string FilePathFmtos = "C:\\Test\\Formatos";
        static string FilePathFmtos = "C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes";

        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }


        [Test, Order(1)]
        public void Intranet_FDTI37_3_Insumos_Datos_Señales_Ingresar_A_La_Opcion()

        {
            driver = Test_Suite.SetUpClass();
            Actions actions = new Actions(driver);
            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Evaluación")).Click();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Costo de Oportunidad"), driver, true);

            actions.MoveToElement(driver.FindElement(By.LinkText("2. Importar insumos"))).Build().Perform();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("2.3 Datos de Señales"), driver, true);

        }

        [Test, Order(2)]
        public void Intranet_FDTI37_3_Insumos_Datos_Señales_Consulta_registros()

        {
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("txtFechaInicio"));

            driver.FindElement(By.XPath("//input[@id='btnConsultar']")).Click();

        }

        [Test, Order(3)]
        public void Intranet_FDTI37_3_Insumos_Datos_Señales_Subir_Archivo()

        {
            try
            {
                FuncionesRecurrentes.InvisibilityLoading(driver);

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnSubirArchivo']"), driver, true);

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='divExportar']"), driver);

                //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnSelectFile']"), driver, true);

                driver.FindElement(By.XPath("//input[@id='btnSelectFile']")).SendKeys(FilePathFmtos + "\\Formato_Señales.xlsx");

                System.Threading.Thread.Sleep(6000);

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='fileInfo']"), driver);

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='mensaje']"), driver);
                //driver.Quit();
            }
            catch (Exception e)
            {
                //driver.Quit();
                //Assert.IsTrue(false, "Error al cargar la información: " + e.InnerException.Message);
                Assert.Fail(e.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}

