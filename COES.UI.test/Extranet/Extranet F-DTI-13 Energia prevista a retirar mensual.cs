
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI13_Energia_prevista_a_retirar_mensual:Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI13_Energia_prevista_a_retirar_mensual_Ingresar_a_la_opcion()
        {
            driver.FindElement(By.LinkText("Post Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Transferencias"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Cálculo de garantías"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Carga de datos"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Energía prevista a retirar mensual"), driver, true);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "BIOENERGIA DEL CHIRA S.A.");
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Seleccionar']"), driver, true);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            var elementos = driver.FindElements(By.XPath("//table[@class='htCore']/tbody/tr")).Count();
            Console.WriteLine("Elementos tabla:" + elementos);
            Assert.True(elementos > 0, "Tabla no encontrada");
        }

        [Test, Order(2)]
        public void Extranet_FDTI13_Energia_prevista_a_retirar_mensual_Descarga_de_plantilla()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnDescargarFormato']//img"), driver, true);
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(3)]
        public void Extranet_FDTI13_Energia_prevista_a_retirar_mensual_Importar_plantilla_de_carga()
        {
            string valor1 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[3]/td[2]"))).Text;
            string valor2 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[4]/td[2]"))).Text;
            string valor3 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[5]/td[2]"))).Text;
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnSelectExcel']//img"), driver);

            //driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[4]/div[1]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/table[1]/tbody[1]/tr[1]/td[3]/div[1]/div[1]/input[1]")).SendKeys("C:\\Test\\Descargas\\13_carga.xlsx");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[4]/div[1]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/table[1]/tbody[1]/tr[1]/td[3]/div[1]/div[1]/input[1]")).SendKeys(FilePathFmtos+"\\13_carga.xlsx");

            FuncionesRecurrentes.WaitFor(5);
            driver.SwitchTo().Alert().Accept();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            string valor11 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[3]/td[2]"))).Text;
            string valor22 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[4]/td[2]"))).Text;
            string valor33 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[5]/td[2]"))).Text;

            Assert.True(valor11 == "11.000", "El valor no coincide con el numero esperado (11)");
            Assert.True(valor22 == "22.000", "El valor no coincide con el numero esperado (22)");
            Assert.True(valor33 == "33.000", "El valor no coincide con el numero esperado (33)");
            Console.WriteLine("Valor 1 inicial {0} y final {1}, valor 2 inicial {2} y final {3}", valor1, valor11, valor2, valor22);

        }

        [Test,Order(4)]
        public void Extranet_FDTI13_Energia_prevista_a_retirar_mensual_Grabar_registros()
        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[4]/td[2]"), driver);
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[5]/td[2]"), driver, "44", By.XPath("//textarea[@class='handsontableInput']"));

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[4]/div[1]/div[1]/div[1]/div[1]/div[2]/div[5]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[2]/th[1]/div[1]"),driver,true); 
                driver.FindElement(By.XPath("//span[normalize-space()='Enviar']")).Click();
                driver.SwitchTo().Alert().Accept();

                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensajeEvento"), driver);
                Assert.That(driver.FindElement(By.XPath("(//div[@id='mensaje'])[1]")).Text.StartsWith("Código de envío"),
                    "El mensaje no coincide con el texto esperado");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
