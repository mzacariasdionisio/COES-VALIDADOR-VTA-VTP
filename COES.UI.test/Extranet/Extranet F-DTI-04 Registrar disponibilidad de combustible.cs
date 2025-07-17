using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI04_Registrar_disponibilidad_combustible: Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI04_Registrar_disponibilidad_combustible_Ingresar_A_La_Opcion()
        {
            driver.FindElement(By.LinkText("Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Reprograma de la operación"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Registrar disponibilidad de combustible"), driver, true);
        }

        [Test, Order(2)]
        public void Extranet_FDTI04_Registrar_disponibilidad_combustible_Grabar_Registros()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("cbEmpresa"),driver);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "PETRAMAS");
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnConsultar"), driver, true);

            FuncionesRecurrentes.InvisibilityLoading(driver);
            
            var editarEsVisible = driver.FindElement(By.XPath("//a[@id='btnEditarEnvio']//div[@class='content-item-action']")).Displayed;
            if(editarEsVisible)
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnEditarEnvio']//div[@class='content-item-action']"), driver, true);
            }

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[3]/td[5]"), driver, true);

            var elements = driver.FindElements(By.Id("detalleFormato"));
            Assert.True(elements.Count > 0,"Tabla no encontrada");

            //JSB:new for
            var elementos = driver.FindElements(By.XPath("//div[@id='detalleFormato']/div[@class='ht_master handsontable']//table[@class='htCore']/tbody/tr")).Count();
            for (int i = 3; i < elementos; i++)
            {
               FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id='detalleFormato']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[" + i + "]/td[5]"), driver, "50", By.XPath("//textarea[@class='handsontableInput']"));
            }
            for (int i = 3; i < elementos; i++)
            {
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id='detalleFormato']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[" + i + "]/td[5]"), driver, "50", By.XPath("//textarea[@class='handsontableInput']"));
            }

            //JSB:FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[3]/td[5]"), driver, "50", By.XPath("//textarea[@class='handsontableInput']"));
            //JSB:driver.FindElement(By.CssSelector(".ht_master > .wtHolder")).Click();
            driver.FindElement(By.XPath("//*[@id='detalleFormato']/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[3]/td[1]")).Click();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnEnviarDatos']/div/img"), driver, true);

            driver.SwitchTo().Alert().Accept();
            
            FuncionesRecurrentes.InvisibilityLoading(driver);
            
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensajeEvento"), driver);
            string xxx = driver.FindElement(By.Id("mensajeEvento")).Text;
            Assert.That(driver.FindElement(By.Id("mensajeEvento")).Text.StartsWith("Los datos se enviaron correctamente"),
                "El mensaje no coincide con el texto esperado");
        }

        [Test, Order(3)]
        public void Extranet_FDTI04_Registrar_disponibilidad_combustible_Descargar_De_Informacion_En_Formato_Excel()
        {
            try
            {
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//span[contains(text(),'Descargar')]"), driver, true);
                FuncionesRecurrentes.VerificarArchivoDescargado();
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
       }
    }