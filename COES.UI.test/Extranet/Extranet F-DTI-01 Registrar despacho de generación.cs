using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using System;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI01_Registrar_despacho_generación : Clase_Base
    {

        [Test, Order(1)]
        public void Extranet_FDTI01_Ingresar_A_La_Opcion()
        {
            driver.FindElement(By.LinkText("Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Reprograma de la operación"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Registra despacho de generación"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Extranet_FDTI01_Grabar_registros()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("cbEmpresa"), driver);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "ENGIE");
            FuncionesRecurrentes.InvisibilityLoading(driver);
            for (int i = 4; i <= 13; i++)
            {
                string xpath = $"//table[@class='htCore']/tbody[1]/tr[{i}]/td[2]";
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath(xpath), driver, "1", By.XPath("//textarea[@class='handsontableInput']"));
            }
            driver.FindElement(By.XPath("//a[@id='btnEnviarDatos']//div[@class='content-item-action']")).Click();
            driver.SwitchTo().Alert().Accept();
            FuncionesRecurrentes.WaitFor();
            /* string valorB6 = driver.FindElement(By.XPath(("//body[1]/div[9]/div[2]/div[2]/div[1]/div[2]/table[1]/tbody[1]/tr[1]/td[1]"))).Text;
            string valorBlanco = driver.FindElement(By.XPath(("//body[1]/div[9]/div[2]/div[2]/div[1]/div[2]/table[1]/tbody[1]/tr[1]/td[3]"))).Text;
            FuncionesRecurrentes.WaitFor(5);
            Assert.True(valorB6 == "B6", "El valor no coincide con el texto esperado (B6)");
            Assert.True(valorBlanco == "BLANCO.", "El valor no coincide con el texto esperado (blanco)");
            Console.WriteLine("Valores: {0}, {1}", valorB6, valorBlanco);*/
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='validaciones']//span[@class='button b-close']"), driver, true);
        }

        [Test, Order(3)]
        public void Extranet_FDTI01_Descargar_información_formato_Excel()
        {
            try
            {

                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnDescargarFormato']//div[1]"), driver, true);
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                FuncionesRecurrentes.VerificarArchivoDescargado();
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//strong[contains(text(),'Los datos se descargaron correctamente')]"), driver);

            }
            catch (Exception e)
            {
                driver.Quit();
                Assert.IsTrue(false, "Descarga de archivo incorrecta " + e.Message);
            }
        }
    }
}