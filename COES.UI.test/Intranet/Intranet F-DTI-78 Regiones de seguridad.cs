
using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI78_Regiones_seguridad
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI78_Regiones_seguridad_Ingresar_a_la_opcion()
        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);
            driver.FindElement(By.LinkText("Programación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("YUPANA"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Regiones de Seguridad"), driver, true);
            driver.FindElement(By.LinkText("Regiones de Seguridad")).SendKeys(Keys.Enter);
        }

        [Test, Order(2)]
        public void Intranet_FDTI78_Regiones_seguridad_Nuevo_registro()
        {
            try
            {
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitFor(5);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnNuevo']"), driver, true);
                FuncionesRecurrentes.WaitFor(5);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='popupRegion']"), driver);

                var elements = driver.FindElements(By.XPath("//div[@id='popupRegion']"));
                Assert.True(elements.Count > 0);
                driver.FindElement(By.XPath("//input[@id='nombRegion']")).SendKeys("Prueba");
                driver.FindElement(By.XPath("//div[@id='popupRegion']")).Click();
                driver.FindElement(By.XPath("//input[@id='btnGrabar']")).Click();
                driver.SwitchTo().Alert().Accept();
                FuncionesRecurrentes.WaitFor(5);
                Assert.That(driver.SwitchTo().Alert().Text, Is.EqualTo("Se guardó correctamente la Región"));
                //driver.Quit();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                //driver.Quit();
                //Assert.True(true);//Assert.IsTrue(false, "Error al descargar la información: " + e.InnerException.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}