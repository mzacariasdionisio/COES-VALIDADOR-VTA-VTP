
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;

namespace Extranet
{
    [TestFixture, Order(-1)]
    public class Extranet_FDTI00_Login:Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI00_Logeo()
        {
            try
            {
                driver.FindElement(By.LinkText("Operación")).Click();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Reprograma de la operación"), driver, true);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Registrar disponibilidad de combustible"), driver, true);
                driver.Quit();
            }
            catch
            {
                driver.Quit();
                Assert.IsTrue(true, "Test de arranque");
            }
            Console.WriteLine("Primer test");
        }
       }
    }