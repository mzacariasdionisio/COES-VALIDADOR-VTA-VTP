using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI23_Demanda:Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI23_Demanda_Ingresar_A_La_Opcion()
        {
            driver.FindElement(By.LinkText("Demanda")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Demanda NUEVO"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(4);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "ACEROS AREQUIPA");
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Extranet_FDTI23_Demanda_Descarga_plantilla()
        {
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnDescargarFormato']//div[1]"), driver, true);
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.VerificarArchivoDescargado();
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//strong[contains(text(),'Los datos se descargaron correctamente')]"), driver);
        }

        [Test, Order(3)]
        public void Extranet_FDTI23_Demanda_Importar_plantilla_carga()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnSelectExcel0']//div[@class='content-item-action']"), driver);
            //driver.FindElement(By.XPath("//input[@type='file']")).SendKeys("C:\\Test\\Descargas\\23_carga.xlsx");
            driver.FindElement(By.XPath("//input[@type='file']")).SendKeys(FilePathFmtos + "\\23_carga.xlsx");
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);

            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(1);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text.StartsWith("Los datos se cargaron correctamente en el excel web"));

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            string valor11 = driver.FindElement(By.XPath(("//table[@class='htCore']/tbody[1]/tr[8]/td[2]"))).Text;
            string valor33 = driver.FindElement(By.XPath(("//table[@class='htCore']/tbody[1]/tr[9]/td[2]"))).Text;
            string valor55 = driver.FindElement(By.XPath(("//table[@class='htCore']/tbody[1]/tr[10]/td[2]"))).Text;

            Assert.True(valor11 == "11.000");
            Assert.True(valor33 == "22.000"); // 22 se encuentra en el Excel proporcionado
            Assert.True(valor55 == "55.000");
            Console.WriteLine("Valores {0}, {1}, {2}", valor11, valor33, valor55);
            string value = driver.FindElement(By.Id("mensajeEvento")).Text;
            Assert.That(value.StartsWith("Los datos se cargaron correctamente"));
        }

        [Test, Order(4)]
        public void Extranet_FDTI23_Demanda_Grabar_registros()
        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id=\"detalleFormato30\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[8]/td[2]"), driver);

                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id=\"detalleFormato30\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[8]/td[2]"), driver, "1", By.XPath("//textarea[@class='handsontableInput']"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id=\"detalleFormato30\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[9]/td[2]"), driver, "2", By.XPath("//textarea[@class='handsontableInput']"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id=\"detalleFormato30\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[10]/td[2]"), driver, "3", By.XPath("//textarea[@class='handsontableInput']"));

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[1]/div[1]/div[6]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[8]/td[2]"), driver, true);
                driver.FindElement(By.XPath("//span[normalize-space()='Enviar']")).Click();
                driver.SwitchTo().Alert().Accept();

                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensajeEvento"), driver);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                Assert.That(driver.FindElement(By.XPath("(//div[@id='mensaje'])[1]")).Text.StartsWith("Los datos se enviaron correctamente"));
            }
            catch
            {
                Assert.IsTrue(false, "Error enviando datos");
            }
        }
    }
}
