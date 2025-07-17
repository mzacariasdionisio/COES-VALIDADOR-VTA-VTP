
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI75_Generacion_Rer
    {
        //static string FilePathFmtos = "C:\\Test\\Formatos";
        static string FilePathFmtos = "C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes";

        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI75_Generacion_Rer_Ingresar_a_la_opcion()
        {
           //try { 
            driver = Test_Suite.SetUpClass();
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.loginIntranet(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Programación"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("YUPANA"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Generación Rer"), driver, true);
            driver.FindElement(By.LinkText("Generación Rer")).SendKeys(Keys.Enter);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            //driver.Quit();
            //}
            //catch
            //{
            //    driver.Quit();
            //    Assert.IsTrue(true, "False positive");
            //}
        }

        [Test, Order(2)]
        public void Intranet_FDTI75_Generacion_Rer_Consulta_registros()
        {
            //try { 
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-message'][contains(.,'Por favor seleccione versión, horizonte y fecha')]"), driver);
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);
            driver.FindElement(By.Id("btnConsultar")).Click();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-message'][contains(.,'Información RER actualizada por el administrador')]"), driver);
            //    driver.Quit();
            //}
            //catch
            //{
            //    driver.Quit();
            //    Assert.IsTrue(true, "False positive");
            //}
        }

        [Test, Order(3)]
        public void Intranet_FDTI75_Generacion_Rer_Descargar_plantilla()
        {
            //try { 
                driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/a[1]/div[1]/img[1]")).Click();
                FuncionesRecurrentes.WaitFor(5);
                FuncionesRecurrentes.VerificarArchivoDescargado();
                FuncionesRecurrentes.WaitFor(10);
            //    driver.Quit();
            //}
            //catch
            //{
            //    driver.Quit();
            //    Assert.IsTrue(true, "False positive");
            //}
        }

        [Test, Order(4)]
        public void Intranet_FDTI75_Generacion_Rer_Importar_formato()
        {
            //try { 
                driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/a[1]/div[1]/img[1]"));
                //driver.FindElement(By.XPath("(//span[text()='Importar formato']/following::input)[1]")).SendKeys("C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes\\75_carga.xlsx");
                driver.FindElement(By.XPath("(//span[text()='Importar formato']/following::input)[1]")).SendKeys(FilePathFmtos + "\\75_carga.xlsx");

                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitFor(2);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensaje"), driver);
                Assert.That(driver.FindElement(By.XPath("//table[@class='htCore']/tbody[1]/tr[6]/td[2]")).Text, Is.EqualTo("10.000"));
                //Assert.That(driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[6]/td[3]")).Text, Is.EqualTo("20.000"));
                Assert.That(driver.FindElement(By.XPath("//table[@class='htCore']/tbody[1]/tr[7]/td[2]")).Text, Is.EqualTo("30.000"));
                //Assert.That(driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[7]/td[3]")).Text, Is.EqualTo("40.000"));
            //    driver.Quit();
            //}
            //catch
            //{
            //    driver.Quit();
            //    Assert.IsTrue(true, "False positive");
            //}
        }

        [Test, Order(5)]
        public void Intranet_FDTI75_Generacion_Rer_Grabar_registros()
        {
            try
            {
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[6]/td[3]"),driver,"20", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[7]/td[3]"),driver,"40", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));
                driver.FindElement(By.XPath("//table[@class='htCore']/tbody[1]/tr[7]/td[2]")).Click();
                driver.FindElement(By.XPath("//a[@id='btnEnviarDatos']//div[1]")).Click();
                driver.SwitchTo().Alert().Accept();
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensaje"), driver);
                Assert.That(driver.FindElement(By.Id("mensaje")).Text.StartsWith("Los datos se enviaron correctamente. Código de envío"),
                "El mensaje no coincide con el texto esperado");
                driver.Quit();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                //driver.Quit();
                //Assert.IsTrue(true, "False positive");
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}

