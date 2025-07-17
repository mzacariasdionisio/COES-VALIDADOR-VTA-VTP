
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;
using System.Linq;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI19_Tension_Generacion:Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI19_Tension_Generacion_Ingresar_a_la_opcion()
        {
            driver.FindElement(By.LinkText("Post Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("IEOD"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("IDCC-G"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Tensión de Generación"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(5);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "ENGIE");

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            var elementos = driver.FindElements(By.XPath("//table[@class='htCore']/tbody/tr")).Count();
            Console.WriteLine("Elementos tabla:" + elementos);
            Assert.True(elementos > 3, "numero de elementos de la tabla inferior a lo esperado");
        }

        [Test, Order(2)]
        public void Extranet_FDTI19_Tension_Generacion_Descargar_plantilla()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//div[@class='content-tabla'])[1]"), driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnDescargarFormato']//div[@class='content-item-action']"), driver, true);

            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(3)]
        public void Extranet_FDTI19_Tension_Generacion_Importar_plantilla()
        {
            string valor1 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato1\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[4]/td[2]"))).Text;
            string valor2 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato1\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[5]/td[2]"))).Text;
            string valor3 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato1\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[6]/td[2]"))).Text;

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnSelectExcel1']//div[@class='content-item-action']"), driver);
            //driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[1]/input[1]")).SendKeys("C:\\Test\\Descargas\\19_carga.xlsx");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[1]/input[1]")).SendKeys(FilePathFmtos + "\\19_carga.xlsx");

            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(3);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text.StartsWith("Los datos se cargaron correctamente en el excel web"),
                "El mensaje no coincide con el texto esperado");

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            string valor11 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato1\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[4]/td[2]"))).Text;
            string valor22 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato1\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[5]/td[2]"))).Text;
            string valor33 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato1\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[6]/td[2]"))).Text;

            Assert.True(valor11 == "10.000", "El valor no coincide con el numero esperado (10)");
            Assert.True(valor22 == "20.000", "El valor no coincide con el numero esperado (20)");
            Assert.True(valor33 == "30.000", "El valor no coincide con el numero esperado (30)");
            Console.WriteLine("Valor 1 inicial {0} y final {1}, valor 2 inicial {2} y final {3}", valor1, valor11, valor2, valor22);
        }

        [Test, Order(4)]
        public void Extranet_FDTI19_Tension_Generacion_Enviar_datos()
        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id='detalleFormato1']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[8]/td[2]"), driver);
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id='detalleFormato1']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[4]/td[2]"), driver, "100", By.XPath("//textarea[@class='handsontableInput']"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id='detalleFormato1']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[5]/td[2]"), driver, "50", By.XPath("//textarea[@class='handsontableInput']"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id='detalleFormato1']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[6]/td[2]"), driver, "25", By.XPath("//textarea[@class='handsontableInput']"));
                
                driver.FindElement(By.XPath("//a[@id='btnEnviarDatos']//div[@class='content-item-action']")).Click();
                driver.SwitchTo().Alert().Accept();

                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensajeEvento"), driver);
                Assert.That(driver.FindElement(By.XPath("(//div[@id='mensaje'])[1]")).Text.StartsWith("Los datos se enviaron correctamente."),
                    "El mensaje no coincide con el texto esperado");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
