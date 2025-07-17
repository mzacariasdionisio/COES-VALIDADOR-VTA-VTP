using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System.Linq;
using OpenQA.Selenium.Interactions;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI58_SorteoPR25
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        

       [Test, Order(1)]
        public void Intranet_FDTI58_SorteoPR25_Ingresar_A_La_Opcion()
        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Coordinación")).Click();
            driver.FindElement(By.LinkText("Sorteo PR25")).Click();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tablaArea']"), driver);
            var elementos = driver.FindElements(By.XPath("//table[@id='tablaArea']/tbody/tr")).Count();
            Console.WriteLine("Elementos tabla 1:" + elementos);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tablaHistorico']"), driver);
            elementos = driver.FindElements(By.XPath("//table[@id='tablaHistorico']/tbody/tr")).Count();
            Console.WriteLine("Elementos tabla 2:" + elementos);
            
        }

        [Test, Order(2)]
        public void Intranet_FDTI58_SorteoPR25_Eliminar_registro()
        {
            try
            {
                FuncionesRecurrentes.WaitFor();
                
                var elementos = driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[5]/div[1]/table[1]/tbody[1]/tr[1]/td[2]")).Text;

				FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id='btnEliminarArea']/img"), driver, true);
				FuncionesRecurrentes.WaitFor();

                var elementos2 = driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[5]/div[1]/table[1]/tbody[1]/tr[1]/td[2]")).Text;

                Assert.That(elementos, Is.Not.EqualTo(elementos2));
                //driver.Quit();
            }
            catch (Exception ex)
            {
                //Assert.IsTrue(true, "False positive");
				Assert.Fail(ex.Message);
                //driver.Quit();
            }
			finally
			{
				driver.Quit();
			}
        }
    }
}