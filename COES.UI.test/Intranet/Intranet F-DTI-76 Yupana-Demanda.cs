
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("27.03.25 Se retira porque la pantalla cambió a un nuevo diseño")]
    public class Intranet_FDTI76_Yupana_demanda
    {
        //static string FilePathFmtos = "C:\\Test\\Formatos";
        static string FilePathFmtos = "C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes";

        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        //[Test, Order(1)]
        public void Intranet_FDTI76_Yupana_demanda_Ingresar_a_la_opcion()
        {
            //try
            //{
                driver = Test_Suite.SetUpClass();
                FuncionesRecurrentes.loginIntranet(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Programación"), driver, true);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("YUPANA"), driver, true);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Demanda"), driver, true);
            //    driver.Quit();
            //}
            //catch
            //{
            //    driver.Quit();
            //    Assert.IsTrue(true, "False positive");
            //}
        }

        //[Test, Order(2)]
        public void Intranet_FDTI76_Yupana_demanda_Consulta_registros()
        {
            //try { 
                FuncionesRecurrentes.WaitFor(5);
                FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);
                driver.FindElement(By.XPath("//input[@value='Consultar']")).Click();
                FuncionesRecurrentes.InvisibilityLoading(driver);
            //    driver.Quit();
            //}
            //catch
            //{
            //    driver.Quit();
            //    Assert.IsTrue(true, "False positive");
            //}
        }

        //[Test, Order(3)]
        public void Intranet_FDTI76_Yupana_demanda_Descargar_formato()
        {
            //try
            //{
                driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/a[1]/div[1]")).Click();
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                FuncionesRecurrentes.VerificarArchivoDescargado();
                FuncionesRecurrentes.WaitFor(30);
            //    driver.Quit();
            //}
            //catch(Exception e)
            //{
            //    driver.Quit();
            //    Assert.IsTrue(true);
            //}
        }

        //[Test, Order(4)]
        public void Intranet_FDTI76_Yupana_demanda_Importar_formato()
        {
            //try { 
                //driver.FindElement(By.XPath("//input[@type='file']")).SendKeys("C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes\\76_carga.xlsx");
                driver.FindElement(By.XPath("//input[@type='file']")).SendKeys(FilePathFmtos + "\\76_carga.xlsx");

                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito'][contains(.,'Por favor presione el botón enviar para grabar los datos')]"), driver);
                Assert.That(driver.FindElement(By.XPath("//table[@class='htCore']/tbody[1]/tr[2]/td[2]")).Text, Is.EqualTo("10.000"));
                Assert.That(driver.FindElement(By.XPath("//table[@class='htCore']/tbody[1]/tr[3]/td[2]")).Text, Is.EqualTo("30.000"));
            //    driver.Quit();
            //}
            //catch
            //{
            //    driver.Quit();
            //    Assert.IsTrue(true, "False positive");
            //}
        }

        //[Test, Order(5)]
        public void Intranet_FDTI76_Yupana_demanda_Grabar_registros()
        {
            try
            {
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[2]/td[3]"), driver, "30", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[3]/td[3]"), driver, "40", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));
                driver.FindElement(By.XPath("//table[@class='htCore']/tbody[1]/tr[2]/td[2]")).Click();
                driver.FindElement(By.XPath("//a[@id='btnEnviarDatos']/div[1]")).Click();
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

