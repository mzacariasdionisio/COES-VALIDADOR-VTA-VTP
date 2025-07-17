
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI21_PR16_Carga_de_informacion:Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI21_PR16_Carga_de_informacion_Ingresar_a_la_opcion()
        {
            driver.FindElement(By.LinkText("Post Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("PR-16 Medidores de Energía UL y D"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Carga de Información"), driver, true);
            
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(3);
            // Seleccionar la empresa Aceros chilca
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id=\'cbEmpresa\']"), driver, true);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "ACEROS CHILCA S.A.C.");
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnConsultar']"), driver, true);
        }

        [Test, Order(2)]
        public void Extranet_FDTI21_PR16_Carga_de_informacion_Descarga_de_plantilla()
        {
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnDescargarFormato']//div[@class='content-item-action']"), driver, true);

            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(3)]
        public void Extranet_FDTI21_PR16_Carga_de_informacion_Importar_plantilla_de_carga()
        {                                                                                                               
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnSelectExcel']//div[@class='content-item-action']"), driver);

                //driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[1]/div[2]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/table[1]/tbody[1]/tr[1]/td[3]/div[1]/input[1]")).SendKeys("C:\\Test\\Descargas\\21_carga.xlsx");
                driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[1]/div[2]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/table[1]/tbody[1]/tr[1]/td[3]/div[1]/input[1]")).SendKeys(FilePathFmtos+ "\\21_carga.xlsx");

                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("loading"), driver);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
                Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text.StartsWith("Por favor presione el botón enviar para grabar los datos"),
                    "El mensaje no coincide con el texto esperado");

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
                string valor11 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[6]/td[2]"))).Text;
                string valor22 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[7]/td[2]"))).Text;
                string valor33 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[8]/td[2]"))).Text;

                Assert.True(valor11 == "10.000", "El valor no coincide con el numero esperado (10)");
                Assert.True(valor22 == "20.000", "El valor no coincide con el numero esperado (20)");
                Assert.True(valor33 == "30.000", "El valor no coincide con el numero esperado (30)");
                Console.WriteLine("Valores {0}, {1}, {2}", valor11, valor22, valor33);

                Assert.That(driver.FindElement(By.Id("mensaje")).Text.StartsWith("Por favor presione el botón enviar para grabar los datos"),
                    "El mensaje no coincide con el texto esperado");
                driver.FindElement(By.Id("btnEnviarDatos")).Click();
                driver.SwitchTo().Alert().Accept();

                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensaje"), driver);
                FuncionesRecurrentes.InvisibilityLoading(driver);

                string mensaje = driver.FindElement(By.Id("mensaje")).Text;

                bool mensajeEsperado = mensaje.StartsWith("Los datos se enviaron correctamente") ||
                                        mensaje.StartsWith("Plazos del envío: Fuera de plazo");

                Assert.IsTrue(mensajeEsperado, "El mensaje no coincide con el texto esperado: " + mensaje);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
