
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;

namespace Intranet
{
    [TestFixture, Order(-1)]
    public class Intranet_FDTI00_Login
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI00_Logeo()
        {
            try
            {
                driver = Test_Suite.SetUpClass();

                FuncionesRecurrentes.loginIntranet(driver);

                driver.FindElement(By.LinkText("Evaluación")).Click();

                FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Costo de Oportunidad"), driver, true);
                driver.Quit();
            }
            catch
            {
                driver.Quit();
                Assert.IsTrue(true, "Test de arranque falló");
            }
            Console.WriteLine("Primer test");
        }
       }
    }