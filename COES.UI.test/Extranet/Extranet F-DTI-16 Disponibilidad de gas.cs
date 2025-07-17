
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;
using System.Linq;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI16_Disponibilidad_gas:Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI16_Disponibilidad_gas_Ingresar_a_la_opcion()
        {
            driver.FindElement(By.LinkText("Post Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("IEOD"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("IDCC-G"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Disponibilidad de Gas"), driver, true);
            // Seleccionar la empresa (EGESUR)
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id=\'cbEmpresa\']"), driver, true);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "EGESUR");
        }

        [Test, Order(2)]
        public void Extranet_FDTI16_Disponibilidad_gas_Consultar_registros()
        {
            FuncionesRecurrentes.InvisibilityLoading(driver);       
            //FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnConsultar"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector(".ht_master > .wtHolder"), driver);
            
            FuncionesRecurrentes.InvisibilityLoading(driver);

            var editarEsVisible = driver.FindElement(By.XPath("//a[@id='btnEditarEnvio']//div[@class='content-item-action']")).Displayed;
            if (editarEsVisible)
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnEditarEnvio']//div[@class='content-item-action']"), driver, true);
            }
            else { Console.WriteLine("Botón editar no es visible."); }
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(3)]
        public void Extranet_FDTI16_Disponibilidad_gas_Agregar_registros()
        {
            FuncionesRecurrentes.NavegarAFecha("Hoy", driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnConsultar"), driver, true);
            FuncionesRecurrentes.WaitFor(2);
            var agregarEsVisible = driver.FindElement(By.CssSelector("#btnAgregarFila img")).Displayed;
            if (agregarEsVisible)
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector("#btnAgregarFila img"), driver, true);

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[2]/td[3]"), driver);
                var elementis = driver.FindElements(By.Id("detalleFormato"));
                Assert.True(elementis.Count > 0, "Tabla no encontrada");

                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[2]/td[3]"), driver, "25", By.XPath("//textarea[@class='handsontableInput']"));

                driver.FindElement(By.CssSelector(".ht_master > .wtHolder")).Click();
                driver.FindElement(By.CssSelector("#btnEnviarDatos img")).Click();
                driver.SwitchTo().Alert().Accept();

                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensajeEvento"), driver);
                Assert.That(driver.FindElement(By.Id("mensajeEvento")).Text.StartsWith("Los datos se enviaron correctamente"),
                    "El mensaje no coincide con el texto esperado");
            }
            else
            {
                Console.WriteLine("No se encontró el botón agregar.");
            }
        }

        [Test, Order(4)]
        public void Extranet_FDTI16_Disponibilidad_gas_Eliminar_registro()
        {
            try
            {
                var elementosIniciales = driver.FindElements(By.XPath("//table[@class='htCore']/tbody/tr")).Count();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnEditarEnvio']//div[@class='content-item-action']"), driver, true);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.CssSelector(".btn"), driver, true);
                driver.SwitchTo().Alert().Accept();

                driver.FindElement(By.CssSelector(".ht_master > .wtHolder")).Click();
                driver.FindElement(By.CssSelector("#btnEnviarDatos img")).Click();
                driver.SwitchTo().Alert().Accept();
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensajeEvento"), driver);
                Assert.That(driver.FindElement(By.Id("mensajeEvento")).Text.StartsWith("Los datos se enviaron correctamente"),
                    "El mensaje no coincide con el texto esperado");

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
                var elementos = driver.FindElements(By.XPath("//table[@class='htCore']/tbody/tr")).Count();
                Console.WriteLine("Elementos tabla inicio {0} y fin {1}", elementosIniciales, elementos);
                Assert.True(elementos<elementosIniciales, "Numero de filas diferente a lo esperado"); 
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
