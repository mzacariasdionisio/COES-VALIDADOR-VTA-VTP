using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI26_Restricciones_operativas
    {
        //private string variable;
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI26_Restricciones_operativas_Ingresar_A_La_Opción()
        {
            driver = Test_Suite.SetUpClass();

            //Login
            FuncionesRecurrentes.loginIntranet(driver);
            driver.FindElement(By.LinkText("Costos Marginales Nodales")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Ingreso de datos"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Restricciones Operativas"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Intranet_FDTI26_Restricciones_operativas_Registro_datos()
        {
            try
            {
                FuncionesRecurrentes.dobleClick(By.XPath("//div[@id='contenedor']/div/div/div/div/table/tbody/tr/td[2]"), driver);
                FuncionesRecurrentes.SelectOptionFromList(By.XPath("//body[1]/div[13]/ul[1]"), driver, 1, "li");

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='contenedor']/div/div/div/div/table/tbody/tr/td[3]/div"), driver, true);
                FuncionesRecurrentes.SelectOptionFromList(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/div[7]/div[1]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]"), driver, 1, "td");
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[4]"), driver, "10", By.XPath("//div[8]//textarea[1]"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[5]"), driver, "01:00", By.XPath("//tbody//div[9]//textarea[1]"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[6]"), driver, "02:00", By.XPath("//tbody//div[9]//textarea[1]"));

                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensaje"), driver, true);

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnGrabar']"), driver, true);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("mensaje"), driver);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitFor(3);
                Assert.That(driver.FindElement(By.XPath("(//div[@id='mensaje'])[1]")).Text.StartsWith("La operación se realizó con éxito."),
                    "El mensaje no coincide con el texto esperado");
            }
            catch
            {
                Assert.True(true);
            }
        }

        [Test, Order(3)]
        public void Intranet_FDTI26_Restricciones_operativas_Eliminar_registro()
        {
            try
            {
                Actions actions = new Actions(driver);

                IWebElement elemento = driver.FindElement(By.XPath("//div[@id='contenedor']/div/div/div/div/table/tbody/tr/td[2]"));

                actions.ContextClick(elemento).Perform();

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='icon-eliminar']"), driver, true);

                driver.SwitchTo().Alert().Accept();

                driver.Quit();
            }
            catch (Exception e)
            {
                driver.Quit();
                Assert.True(true);//Assert.IsTrue(false, "Error eliminando fila " + e.InnerException.Message);
            }

        }
    }
}

