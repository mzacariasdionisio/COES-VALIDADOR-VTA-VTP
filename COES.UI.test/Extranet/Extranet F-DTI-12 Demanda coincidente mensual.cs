
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI12_Demanda_coincidente_mensual:Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI12_Demanda_coincidente_mensual_Ingresar_a_la_opcion()
        {
            driver.FindElement(By.LinkText("Post Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Transferencias"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Cálculo de garantías"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Carga de datos"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Demanda coincidente mensual"), driver, true);

            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "BIOENERGIA DEL CHIRA S.A.");
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Seleccionar']"), driver, true);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            var elementos = driver.FindElements(By.XPath("//table[@class='htCore']/tbody/tr")).Count();
            Console.WriteLine("Elementos tabla:" + elementos);
            Assert.True(elementos > 0, "Tabla no encontrada");

        }

        [Test, Order(2)]
        public void Extranet_FDTI12_Demanda_coincidente_mensual_Descarga_de_plantilla()
        {
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnDescargarFormato']//div[@class='content-item-action']"), driver, true);
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(3)]
        public void Extranet_FDTI12_Demanda_coincidente_mensual_Importar_plantilla_de_carga()
        {
            string valorEnero = driver.FindElement(By.XPath(("//table[@class='htCore']/tbody[1]/tr[3]/td[2]"))).Text; //"//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[3]/td[2]"))).Text;
            string valorFebrero = driver.FindElement(By.XPath(("//table[@class='htCore']/tbody[1]/tr[4]/td[2]"))).Text;
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnSelectExcel']//div[@class='content-item-action']"), driver);

            //driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[4]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/table[1]/tbody[1]/tr[1]/td[3]/div[1]/div[1]/input[1]")).SendKeys("C:\\Test\\Formatos\\12_carga.xlsx");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[4]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/table[1]/tbody[1]/tr[1]/td[3]/div[1]/div[1]/input[1]")).SendKeys(FilePathFmtos + "\\12_carga.xlsx");

            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(2);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            string valorEnero2 = driver.FindElement(By.XPath(("//table[@class='htCore']/tbody[1]/tr[3]/td[2]"))).Text;
            string valorFebrero2 = driver.FindElement(By.XPath(("//table[@class='htCore']/tbody[1]/tr[4]/td[2]"))).Text;

            Assert.True(valorEnero2 == "100.000", "El valor no coincide con el numero esperado (100)");
            Assert.True(valorFebrero2 == "200.000", "El valor no coincide con el numero esperado (200)");
            Console.WriteLine("Valor enero inicial {0} y final {1}, valor febrero inicial {2} y final {3}", valorEnero, valorEnero2, valorFebrero, valorFebrero2);

        }

        [Test, Order(4)]
        public void Extranet_FDTI12_Demanda_coincidente_mensual_Grabar_registros()
        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[4]/td[2]"), driver);
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[4]/td[2]"), driver, "300", By.XPath("//textarea[@class='handsontableInput']"));

                driver.FindElement(By.XPath("//span[normalize-space()='Enviar']")).Click();
                driver.SwitchTo().Alert().Accept();
                //string xxx = driver.FindElement(By.XPath("(//div[@id='mensaje'])[1]")).Text;
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensajeEvento"), driver);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                //string yyy = driver.FindElement(By.XPath("(//div[@id='mensaje'])[1]")).Text;
                Assert.That(driver.FindElement(By.XPath("(//div[@id='mensaje'])[1]")).Text.StartsWith("Código de envío"),
                    "El mensaje no coincide con el texto esperado");
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
