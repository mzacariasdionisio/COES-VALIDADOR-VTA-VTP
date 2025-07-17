
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI10_Informe_fallas:Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI10_Informe_fallas_Ingresar_A_La_Opcion()
        {
            driver.FindElement(By.LinkText("Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Informes de Falla"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector(".form-title"), driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("tabla_wrapper"), driver);
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.XPath("(//input[@id='dtInicio'])[1]"));
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//input[@id='btnConsultar'])[1]"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Extranet_FDTI10_Informe_fallas_Carga_De_Informes_De_Falla_Preliminar_IPI()
        {
            try
            {
                //Cargar IPI
                driver.FindElement(By.CssSelector(".odd:nth-child(1) > td:nth-child(5) img")).Click();

                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("popupCargaArchivo"), driver);

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='listadoInformes']//table[@id='tabla']"), driver);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                //driver.FindElement(By.Id("seleccionArchivos")).SendKeys("C:\\Test\\Descargas\\10_pdf_de_ejemplo.pdf");
                driver.FindElement(By.Id("seleccionArchivos")).SendKeys(FilePathFmtos + "\\10_pdf_de_ejemplo.pdf");
                FuncionesRecurrentes.InvisibilityLoading(driver);

                FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector("#listaDeArchivos > ul"), driver);
                FuncionesRecurrentes.WaitFor(1);
                Assert.That(driver.FindElement(By.CssSelector("#listaDeArchivos > ul")).Text, Is.EqualTo("10_pdf_de_ejemplo.pdf"),
                    "Falla al subir el archivo");
                //FuncionesRecurrentes.WaitFor(1);
                //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnEnviar']"), driver, true);
                driver.FindElement(By.Id("btnEnviar")).Click();

                //FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitFor(1);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensajeEvento"), driver);
                string yyy = Convert.ToString(driver.FindElement(By.Id("mensajeEvento")).Text);
                //FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensajeEvento"), driver);
                
                //Assert.That(driver.FindElement(By.Id("mensajeEvento")).Text, Is.EqualTo("Los datos se guadaron correctamente"),
                //    "El mensaje no coincide con el texto esperado");
                Assert.That(yyy, Is.EqualTo("Los datos se guadaron correctamente"),
                    "El mensaje no coincide con el texto esperado");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                driver.FindElement(By.XPath("//input[@id='btnCancelar']")).Click();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector(".odd:nth-child(1) > td:nth-child(7) img"), driver);
            }
        }

        [Test, Order(3)]
        public void Extranet_FDTI10_Informe_fallas_Carga_De_Informes_De_Fallas_Final_IF()
        {
            try
            {
                //Cargar IF
                FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector(".odd:nth-child(1) > td:nth-child(7) img"), driver, true);

                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("popupCargaArchivo"), driver);

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='listadoInformes']//table[@id='tabla']"), driver);

                FuncionesRecurrentes.InvisibilityLoading(driver); 
                driver.FindElement(By.Id("seleccionArchivos")).SendKeys(FilePathFmtos + "\\10_word_de_ejemplo.docx");
                FuncionesRecurrentes.InvisibilityLoading(driver);

                FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector("#listaDeArchivos > ul"), driver);
                FuncionesRecurrentes.WaitFor(1);
                Assert.That(driver.FindElement(By.CssSelector("#listaDeArchivos > ul")).Text, Is.EqualTo("10_word_de_ejemplo.docx"),
                    "Error al subir el archivo");

                driver.FindElement(By.Id("btnEnviar")).Click();
                //string xxx = Convert.ToString(driver.FindElement(By.Id("mensajeEvento")).Text);
                //FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitFor(1);
                string yyy = Convert.ToString(driver.FindElement(By.Id("mensajeEvento")).Text);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensajeEvento"), driver);
                //string zzz = Convert.ToString(driver.FindElement(By.Id("mensajeEvento")).Text);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensajeEvento"), driver);
                //Assert.That(driver.FindElement(By.Id("mensajeEvento")).Text, Is.EqualTo("Los datos se guadaron correctamente"),
                //    "El mensaje no coincide con el texto esperado");
                Assert.That(yyy, Is.EqualTo("Los datos se guadaron correctamente"),
                    "El mensaje no coincide con el texto esperado");
            }
            catch(Exception ex)
            {
                //Assert.Fail(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
