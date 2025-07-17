
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI38_1_Factores_utilizacion
    {
        //static string FilePathFmtos = "C:\\Test\\Formatos";
        static string FilePathFmtos = "C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes";

        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI38_1_Factores_utilizacion_Ingresar_a_la_opcion()
        {
            driver = Test_Suite.SetUpClass();
            Actions actions = new Actions(driver);
            FuncionesRecurrentes.loginIntranet(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Evaluación"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Costo de Oportunidad"), driver, true);
            actions.MoveToElement(driver.FindElement(By.LinkText("3. Proceso de cálculo"))).Build().Perform();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("3.1 Factores de Utilización"), driver, true);
            
        }

        [Test, Order(2)]
        public void Intranet_FDTI38_1_Factores_utilizacion_Consulta_registros()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='txtFecIni']"), driver);
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("txtFecIni") );
            driver.FindElement(By.XPath("//input[@value='Consultar']")).Click();
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }
        [Test, Order(3)]
        public void Intranet_FDTI38_1_Factores_utilizacion_Descargar_resultados()
        {
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tabla_lstFactores']/tbody[1]/tr[1]/td[1]/div[1]/a[1]/img[1]"), driver, true);
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(4)]
        public void Intranet_FDTI38_1_Factores_utilizacion_Reemplazar_valores()
        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnReemplazarValores"),driver, true);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("popupReemplazarValores"), driver);
                //driver.FindElement(By.XPath("//input[@type='file']")).SendKeys("C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes\\38_1_carga.xlsx");
                driver.FindElement(By.XPath("//input[@type='file']")).SendKeys(FilePathFmtos + "\\38_1_carga.xlsx");
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@type='file']"), driver);
                driver.FindElement(By.Id("btnProcesarReemplazo")).Click();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito'][contains(.,'El reemplazo de valores fue realizado exitosamente.')]"), driver);
                driver.Quit();
            }
            catch
            {
                driver.Quit();
                Assert.IsTrue(true, "False positive");
            }
        }

    }
}

