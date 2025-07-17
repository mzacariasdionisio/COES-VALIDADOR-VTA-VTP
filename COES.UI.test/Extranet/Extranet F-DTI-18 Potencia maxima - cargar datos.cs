
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;
using System.Linq;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI18_Potencia_maxima_cargar_datos:Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI18_Potencia_maxima_Ingresar_a_la_opcion()
        {
            driver.FindElement(By.LinkText("Post Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Transferencias"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Potencia máxima a retirar"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Cargar datos"), driver, true);
            FuncionesRecurrentes.WaitFor(2);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "AGREGADOS COMERCIALIZADOS S.A.C.");
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnConsultar']"), driver, true);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            var elementos = driver.FindElements(By.XPath("//table[@class='htCore']/tbody/tr")).Count();
            Console.WriteLine("Elementos tabla:" + elementos);
            Assert.True(elementos > 3, "Tabla no encontrada");
        }

        [Test, Order(2)]
        public void Extranet_FDTI18_Potencia_maxima_Descargar_formato()
        {
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnDescargarFormato']//div[@class='content-item-action']"), driver, true);
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(3)]
        public void Extranet_FDTI18_Potencia_maxima_Importar_formato()
        {
            string valor1 = driver.FindElement(By.XPath(("//*[@id='detalleFormato']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[8]/td[2]"))).Text;
            string valor2 = driver.FindElement(By.XPath(("//*[@id='detalleFormato']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[9]/td[2]"))).Text;
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnSelectExcel']//div[@class='content-item-action']"), driver);

            //driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[1]/div[2]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/input[1]")).SendKeys("C:\\Test\\Descargas\\18_carga.xlsx");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[1]/div[2]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/input[1]")).SendKeys(FilePathFmtos + "\\18_carga.xlsx");

            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(3);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            string valor11 = driver.FindElement(By.XPath(("//*[@id='detalleFormato']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[8]/td[2]"))).Text;
            string valor22 = driver.FindElement(By.XPath(("//*[@id='detalleFormato']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[9]/td[2]"))).Text;
            string valor33 = driver.FindElement(By.XPath(("//*[@id='detalleFormato']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[10]/td[2]"))).Text;

            Assert.True(valor11 == "10.000", "El valor no coincide con el numero esperado (10)");
            Assert.True(valor22 == "20.000", "El valor no coincide con el numero esperado (20)");
            Assert.True(valor33 == "30.000", "El valor no coincide con el numero esperado (30)");
            Console.WriteLine("Valor 1 inicial {0} y final {1}, valor 2 inicial {2} y final {3}", valor1, valor11, valor2, valor22);

        }

        [Test, Order(4)]
        public void Extranet_FDTI18_Potencia_maxima_Enviar_datos()
        {
            try
            {
                FuncionesRecurrentes.NavegarAPeriodo(By.Id("txtMes"), driver, "MesSiguiente");
                
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnConsultar']"), driver, true);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id='detalleFormato']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[8]/td[2]"), driver);
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[8]/td[2]"), driver, "1", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[9]/td[2]"), driver, "2", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id='detalleFormato']/div[5]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[2]/td[1]"), driver, true);
                driver.FindElement(By.XPath("//a[@id='btnEnviarDatos']//div[@class='content-item-action']")).Click();
                driver.SwitchTo().Alert().Accept();
                
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
                Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text.StartsWith("Los datos se enviaron correctamente."),
                    "El mensaje no coincide con el texto esperado");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
