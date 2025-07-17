using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI42_UnidadesRegulacionSecundaria
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI42_UnidadesRegulacionSecundaria_Ingresar_a_la_opcion()
        {
            driver = Test_Suite.SetUpClass();
            FuncionesRecurrentes.loginIntranet(driver);
            FuncionesRecurrentes.WaitFor(2);
            driver.FindElement(By.LinkText("Eventos")).Click();
            FuncionesRecurrentes.WaitFor(2);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//ul[@id='menu']/li[2]/ul/li[2]/a"), driver);
            FuncionesRecurrentes.WaitFor(2);
            FuncionesRecurrentes.dobleClick(By.XPath("//ul[@id='menu']/li[2]/ul/li[2]/a"), driver);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-message'][contains(.,'Complete los datos')]"), driver);
        }

        [Test, Order(2)]
        public void Intranet_FDTI42_UnidadesRegulacionSecundaria_Consulta_registros()
        {
            //try
            //{
                FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);
                driver.FindElement(By.XPath("//input[@id='txtFecha']")).Click();
                driver.FindElement(By.XPath("//td[contains(@class,'dp_selected')]")).Click();
				FuncionesRecurrentes.InvisibilityLoading(driver);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        [Test, Order(3)]
        public void Intranet_FDTI42_UnidadesRegulacionSecundaria_Edicion_datos()
        {
            //try
			//{ 
				FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[4]/td[7]"), driver, "10.000", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));
				FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[4]/td[8]"), driver, "20.000", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));
				FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[5]/td[7]"), driver, "30.000", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));
				FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[5]/td[8]"), driver, "40.000", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));
				
				driver.FindElement(By.XPath("//input[@id='btnGrabar']")).Click();
				
				FuncionesRecurrentes.InvisibilityLoading(driver);
				
				FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito'][contains(.,'Los datos se grabaron correctamente. Se modificaron 1 datos.')]"), driver);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        [Test, Order(4)]
        public void Intranet_FDTI42_UnidadesRegulacionSecundaria_exportar_registros()
        {
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            //try
			//{ 
				driver.FindElement(By.Id("btnExportar")).Click();
				FuncionesRecurrentes.VerificarArchivoDescargado();
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        [Test, Order(5)]
        public void Intranet_FDTI42_UnidadesRegulacionSecundaria_importar_registros()
        {
            //try
			//{
				FuncionesRecurrentes.Otrasfechas("", driver);
                FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);
            
                driver.FindElement(By.XPath("//input[@value='Importar']")).Click();
				
				driver.FindElement(By.Id("btnImportarYupana")).Click();
				
				FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-alert'][contains(.,'Se han actualizado las columnas con cabecera de color naranja. Por favor revise antes de grabar')]"), driver);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        [Test, Order(6)]
        public void Intranet_FDTI42_UnidadesRegulacionSecundaria_generar_XML()
        {
            try
            {
				driver.FindElement(By.Id("btnExportarXML")).Click();
				FuncionesRecurrentes.InvisibilityLoading(driver);
				
				FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnExportarAGC"), driver, true);
				
				//FuncionesRecurrentes.WaitFor(5);
                //IAlert alert = driver.SwitchTo().Alert();
                //alert.Accept();
                FuncionesRecurrentes.WaitFor();
                driver.SwitchTo().Alert().Accept();
            }
            catch (Exception ex)
            {
                //Assert.Fail(ex.Message);// es debido a que no realizó los pasos anteriores
                // quizá debido a la ejecución anterior y no se puede eliminar el registro
                //Console.WriteLine(ex.Message);
				
				Assert.Fail(ex.Message);
                //driver.Quit();
            }
            finally
			{
				driver.Quit();
			}
        }
    }
}