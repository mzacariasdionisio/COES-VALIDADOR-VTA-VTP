
using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using NUnit.Framework;
using System.Linq;
using OpenQA.Selenium.Support.UI;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI46_Informe_fallas_N2
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }


        [Test, Order(1)]
        public void Intranet_FDTI46_Informe_fallas_N2_Ingresar_a_la_opcion()
        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Eventos"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Informe de Fallas N2"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Intranet_FDTI46_Informe_fallas_N2_Consulta_registros()

        {
            //Thread.Sleep(5000);
            //ITSE:
            FuncionesRecurrentes.WaitFor();
            //No selecciona el primer día del mes anterior
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmitido"), driver, "--TODOS--");
            FuncionesRecurrentes.NavegarAFecha("FechaPersonalizada", driver, By.Id("FechaDesde"), "01/01/2022");
            driver.FindElement(By.XPath("//input[@value='Buscar']")).Click();

        }

        [Test, Order(3)]
        public void Intranet_FDTI46_Informe_fallas_N2_Edicion_datos()

        {
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tablaArea']"), driver);
            //var elementos = driver.FindElements(By.XPath("//table[@id='tablaArea']/tbody/tr")).Count();
            //Console.WriteLine("Elementos tabla:" + elementos);

            //if (elementos > 0) {
            //    try
            //    {
            //        FuncionesRecurrentes.WaitFor(2);
            //        driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/div[2]/div[2]/table[1]/tbody[1]/tr[1]/td[1]/a[4]/img[1]")).Click();
            //        FuncionesRecurrentes.WaitFor(2);
            //        driver.FindElement(By.XPath("//input[@name='EvenN2Corr']")).Clear();
            //        driver.FindElement(By.XPath("//input[@name='EvenN2Corr']")).SendKeys("10");
            //        driver.FindElement(By.XPath("//input[@value='Grabar']")).Click();
            //        FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[contains(@class,'action-exito')]"), driver);
            //        driver.FindElement(By.XPath("//input[@id='btnCancelar']")).Click();

            //        Intranet_FDTI46_Informe_fallas_N2_Eliminar_registro();
            //        Intranet_FDTI46_Informe_fallas_N2_Log_envios();
            //    }
            //    catch { }
            //}
            //else {
            //    Console.WriteLine("Elementos de la tabla no disponibles para edición.");
            //}
            //driver.Quit();

            //ITSE:
            FuncionesRecurrentes.WaitFor(2);
            driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/div[2]/div[2]/table[1]/tbody[1]/tr[1]/td[1]/a[4]/img[1]")).Click();
            FuncionesRecurrentes.WaitFor(2);
            driver.FindElement(By.XPath("//input[@name='EvenN2Corr']")).Clear();
            driver.FindElement(By.XPath("//input[@name='EvenN2Corr']")).SendKeys("10");
            FuncionesRecurrentes.WaitFor(2);
            driver.FindElement(By.XPath("//input[@value='Grabar']")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[contains(@class,'action-exito')]"), driver);
            FuncionesRecurrentes.WaitFor(2);
            driver.FindElement(By.XPath("//input[@id='btnCancelar']")).Click();
            FuncionesRecurrentes.InvisibilityLoading(driver);

            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmitido"), driver, "--TODOS--");
            FuncionesRecurrentes.WaitFor(2);
            driver.FindElement(By.XPath("//input[@value='Buscar']")).Click();

        }

        [Test, Order(4)]
        public void Intranet_FDTI46_Informe_fallas_N2_Eliminar_registro()

        {
            //try
            //{
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/div[2]/div[2]/table[1]/tbody[1]/tr[1]/td[1]/a[5]/img[1]"), driver, true);
            //    driver.SwitchTo().Alert().Accept();
            //    FuncionesRecurrentes.WaitFor();
            //}
            //catch (Exception e)
            //{
            //    Assert.IsTrue(false, "Error al eliminar el registro: " + e.InnerException.Message);
            //}

            //ITSE:
            new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(d =>
            {
                var modals = d.FindElements(By.CssSelector("div.b-modal"));
                return modals.Count == 0 || modals.All(m => !m.Displayed || m.GetCssValue("opacity") == "0");
            });

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tablaArea']/tbody/tr[1]/td[1]/a[5]/img[1]"), driver, true);
            FuncionesRecurrentes.WaitFor(5);
            driver.SwitchTo().Alert().Accept();
            FuncionesRecurrentes.WaitFor(5);
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(5)]
        public void Intranet_FDTI46_Informe_fallas_N2_Log_envios()
        {
            //try
            //{
            //    FuncionesRecurrentes.EliminarYCrearCarpeta();
            //    driver.FindElement(By.XPath("//input[@value='Log de Envíos']")).Click();
            //    FuncionesRecurrentes.VerificarArchivoDescargado();
            //}
            //catch (Exception e)
            //{
            //    Assert.IsTrue(false, "Error al enviar la información: " + e.InnerException.Message);
            //}

            //ITSE:
            try
            {
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(d =>
                {
                    var modals = d.FindElements(By.CssSelector("div.b-modal"));
                    return modals.Count == 0 || modals.All(m => !m.Displayed || m.GetCssValue("opacity") == "0");
                });
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnDescargarLog']"), driver, true);
                FuncionesRecurrentes.VerificarArchivoDescargado();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}