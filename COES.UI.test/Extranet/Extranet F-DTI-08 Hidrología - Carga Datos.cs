using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI08_Hidrologia_Carga_Datos : Clase_Base
    {

        [Test, Order(1)]
        public void Extranet_FDTI08_Ingresar_A_La_Opcion()
        {

            driver.FindElement(By.LinkText("Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Hidrología"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Carga Datos"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Extranet_FDTI08_Descarga_Plantilla()
        {

            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "ENGIE");
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Seleccionar']"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnDescargarFormato']//div[1]"), driver, true);
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.VerificarArchivoDescargado();

        }

        [Test, Order(3)]
        public void Extranet_FDTI08_Importar_plantilla_carga()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnSelectExcel']//div[@class='content-item-action']"), driver);
            //driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[4]/div[1]/table[1]/tbody[1]/tr[2]/td[1]/div[1]/table[1]/tbody[1]/tr[1]/td[3]/div[1]/div[1]/input[1]")).SendKeys("C:\\Test\\Descargas\\08_carga.xlsx");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[4]/div[1]/table[1]/tbody[1]/tr[2]/td[1]/div[1]/table[1]/tbody[1]/tr[1]/td[3]/div[1]/div[1]/input[1]")).SendKeys(FilePathFmtos+"\\08_carga.xlsx");
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(3);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            string valor11 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[4]/td[2]"))).Text;
            string valor12 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[5]/td[2]"))).Text;
            FuncionesRecurrentes.WaitFor(3);
            Assert.True(valor11 == "11.000", "El valor no coincide con el numero esperado (11)");
            Assert.True(valor12 == "12.000", "El valor no coincide con el numero esperado (12)");
            Console.WriteLine("Valores {0}, {1}", valor11, valor12);

        }


        [Test, Order(4)]
        public void Extranet_FDTI08_Grabar_registros()
        {
            try
            {

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[4]/td[2]"), driver, "10", By.XPath("//textarea[@class='handsontableInput']"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[5]/td[2]"), driver, "20", By.XPath("//textarea[@class='handsontableInput']"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[6]/td[2]"), driver, "30", By.XPath("//textarea[@class='handsontableInput']"));
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']/tbody[1]/tr[7]/td[2]"), driver, true);
                FuncionesRecurrentes.WaitFor(2);
                driver.FindElement(By.XPath("//a[@id='btnEnviarDatos']//div[@class='content-item-action']")).Click();
                driver.SwitchTo().Alert().Accept();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='mensaje']"), driver);
                string valor = driver.FindElement(By.XPath(("//div[@id='mensaje']"))).Text;
                FuncionesRecurrentes.WaitFor(2);
                Assert.That(driver.FindElement(By.XPath("//div[@id='mensaje']")).Text.StartsWith("Código de envío:"),
                    "El mensaje no coincide con el texto esperado");

            }
            catch (Exception e)
            {
                driver.Quit();
                Assert.IsTrue(false, "Fallo el envio de información " + e.InnerException.Message);
            }
        }
    }
}
