
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;
using System.Linq;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI07_Generación_RER : Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI07_Ingresar_a_la_opcion()
        {
            driver.FindElement(By.LinkText("Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Generación RER"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Cargar Datos"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "ENGIE");
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            var elementos = driver.FindElements(By.XPath("//table[@class='htCore']/tbody/tr")).Count();
            Console.WriteLine("Elementos tabla:" + elementos);
            Assert.True(elementos > 3, "Tabla no encontrada");
        }

        [Test, Order(2)]
        public void Extranet_FDTI07_Descarga_plantilla_carga()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnDescargarFormato']//div[@class='content-item-action']"), driver, true);
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(3)]
        public void Extranet_FDTI07_Importar_plantilla_carga()
        {

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnSelectExcel1']//div[@class='content-item-action']"), driver);
            //driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[1]/input[1]")).SendKeys("C:\\Test\\Descargas\\07_carga.xlsx");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[1]/input[1]")).SendKeys(FilePathFmtos + "\\07_carga.xlsx");
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(3);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            string valor10 = driver.FindElement(By.XPath(("//div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[5]/td[2]"))).Text;
            string valor20 = driver.FindElement(By.XPath(("//div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[5]/td[3]"))).Text;
            FuncionesRecurrentes.WaitFor(3);
            Assert.True(valor10 == "10.000", "El valor no coincide con el numero esperado (10)");
            Assert.True(valor20 == "20.000", "El valor no coincide con el numero esperado (20)");
            Console.WriteLine("Valores {0}, {1}", valor10, valor20);
            FuncionesRecurrentes.WaitFor(3);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            FuncionesRecurrentes.WaitFor(2);
            Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text.StartsWith("Los datos se cargaron correctamente en el excel web"),
                "El mensaje no coincide con el texto esperado");

        }

        [Test, Order(4)]
        public void Extranet_FDTI07_Grabar_registros()
        {
            try
            {

                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[5]/td[2]"), driver, "30", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[6]/td[2]"), driver, "40", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));
                driver.FindElement(By.XPath("//a[@id='btnEnviarDatos']//div[@class='content-item-action']")).Click();
                driver.SwitchTo().Alert().Accept();
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
                Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text.StartsWith("Los datos se enviaron correctamente."),
                    "El mensaje no coincide con el texto esperado");
            }
            catch
            {
                Assert.IsTrue(false, "Error al enviar la información");
            }
        }
    }
}
