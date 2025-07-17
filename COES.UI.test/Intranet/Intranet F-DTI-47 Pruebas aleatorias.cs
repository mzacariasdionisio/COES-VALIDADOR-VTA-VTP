using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI47_Pruebas_aleatorias
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI47_Pruebas_aleatorias_Ingresar_a_la_opcion()
        {
            try
            {
                driver = Test_Suite.SetUpClass();

                FuncionesRecurrentes.loginIntranet(driver);

                driver.FindElement(By.LinkText("Eventos")).Click();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Pruebas aleatorias"), driver, true);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                //driver.Quit();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally {
                driver.Quit();
            }
        }

        //[Test, Order(2)]
        public void Intranet_FDTI47_Pruebas_aleatorias_Nuevo_registro()
        {
            // A partir de la 14:00 se muestra el Botón nuevo
            //try
            //{
            //    realizarRegistro();
            //    Intranet_FDTI47_Pruebas_aleatorias_Edicion_de_registro();
            //    Intranet_FDTI47_Pruebas_aleatorias_Consulta_Registro();
            //}
            //catch (Exception ex)
            //{
            //    if (DateTime.Now.Hour < 14)
            //    {
            //        Assert.True(true);
            //        Console.WriteLine("Botón nuevo no visible, está disponible a partir de las 2 pm.");
            //    }
            //    else
            //    {
            //        Assert.Fail(ex.Message);
            //    }
            //}
            //finally { driver.Quit(); }
        }

        private void realizarRegistro()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnNuevo']"), driver, true);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//select[@id='cbUsuario'])[1]"), driver, true);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbUsuario"), driver, "", true, 1);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnSIC2HOP"), driver, true);
            /* no se puede verificar el color de ese elemento 
            String color = driver.FindElement(By.Id("btnSIC2HOP")).GetCssValue("background-color");
            String[] numbers = color.Replace("rgba(", "").Replace(")", "").Split(',');
            int r = int.Parse(numbers[0].Trim());
            int g = int.Parse(numbers[1].Trim());
            int b = int.Parse(numbers[2].Trim());
            Console.WriteLine("r: " + r + "g: " + g + "b: " + b);*/

            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnCancelar"), driver, true);
            FuncionesRecurrentes.WaitFor(1);
        }

        //[Test, Order(3)]
        public void Intranet_FDTI47_Pruebas_aleatorias_Edicion_de_registro()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//tbody/tr[1]/td[1]/a[2]/img[1]"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnHOP2UT30D"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnCancelar"), driver, true);
        }

        //[Test, Order(4)]
        public void Intranet_FDTI47_Pruebas_aleatorias_Consulta_Registro()
        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//tbody/tr[1]/td[1]/a[1]/img[1]"), driver, true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

    }
}