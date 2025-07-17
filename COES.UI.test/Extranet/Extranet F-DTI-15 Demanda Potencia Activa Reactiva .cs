
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI15_Demanda_con_potencia_activa_y_reactiva:Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI15_Demanda_con_potencia_activa_y_reactiva_Ingresar_a_la_opcion()
        {
            driver.FindElement(By.LinkText("Post Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("IEOD"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("IDDC-D"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Demanda con Potencia Activa y Reactiva"), driver, true);
            
            FuncionesRecurrentes.WaitFor(5);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            // Seleccionar la empresa MINERA ANTAMINA
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id=\'cbEmpresa\']"), driver, true);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "MINERA ANTAMINA");
        }

        [Test, Order(2)]
        public void Extranet_FDTI15_Demanda_con_potencia_activa_y_reactiva_Descarga_de_plantilla()
        {
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnDescargarFormato']//div[@class='content-item-action']"), driver, true);

            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(3)]
        public void Extranet_FDTI15_Demanda_con_potencia_activa_y_reactiva_Importar_plantilla_de_carga()
        {           
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnSelectExcel0']//div[@class='content-item-action']"), driver);

            //driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[1]/input[1]")).SendKeys("C:\\Test\\Descargas\\15_carga.xlsx");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[1]/input[1]")).SendKeys(FilePathFmtos + "\\15_carga.xlsx");

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("loading"), driver);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            FuncionesRecurrentes.WaitFor(2);
            Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text.StartsWith("Los datos se cargaron correctamente en el excel web"),
                "El mensaje no coincide con el texto esperado");

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            string valor11 = driver.FindElement(By.XPath(("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[1]/div[1]/div[6]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[8]/td[2]"))).Text;
            string valor22 = driver.FindElement(By.XPath(("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[1]/div[1]/div[6]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[9]/td[2]"))).Text;
            string valor33 = driver.FindElement(By.XPath(("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[1]/div[1]/div[6]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[10]/td[2]"))).Text;

            Assert.True(valor11 == "1.000", "El valor no coincide con el numero esperado (1)");
            Assert.True(valor22 == "3.000", "El valor no coincide con el numero esperado (3)");
            Assert.True(valor33 == "5.000", "El valor no coincide con el numero esperado (5)");
            Console.WriteLine("Valores {0}, {1}, {2}", valor11, valor22, valor33);
            string value = driver.FindElement(By.XPath("(//div[@id='mensajeEvento'])[1]")).Text;
            Assert.That(value.StartsWith("Los datos se cargaron correctamente"), "El mensaje no coincide con el texto esperado");
        }

        [Test,Order(4)]
        public void Extranet_FDTI15_Demanda_con_potencia_activa_y_reactiva_Grabar_registros()
        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id=\"detalleFormato30\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[10]/td[2]"), driver);

                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id=\"detalleFormato30\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[10]/td[2]"), driver, "1", By.XPath("//textarea[@class='handsontableInput']"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id=\"detalleFormato30\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[11]/td[2]"), driver, "2", By.XPath("//textarea[@class='handsontableInput']"));

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("/html[1]/body[1]/div[1]/div[4]/section[1]/div[1]/div[2]/div[1]/div[1]/div[6]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[8]/td[2]"),driver,true); 
                driver.FindElement(By.XPath("//span[normalize-space()='Enviar']")).Click();
                driver.SwitchTo().Alert().Accept();

                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensajeEvento"), driver);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                Assert.That(driver.FindElement(By.XPath("(//div[@id='mensaje'])[1]")).Text.StartsWith("Los datos se enviaron correctamente"),
                    "El mensaje no coincide con el texto esperado");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
