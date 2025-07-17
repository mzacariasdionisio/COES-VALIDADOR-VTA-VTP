
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System.Linq;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI60_Informes_Osinergmin
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        

        [Test, Order(1)]
        public void Intranet_FDTI60_Informes_Osinergmin_Ingresar_A_La_Opcion()
        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Coordinación")).Click();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Informes Osinergmin (Mant. y Horas Operac.)"), driver, true);

            //FuncionesRecurrentes.WaitFor(10);
            //ITSE:
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Intranet_FDTI60_Informes_Osinergmin_Consulta_De_Registros()
        {
            //FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbTipo']"), driver);
            //var dropdown = driver.FindElement(By.XPath("//select[@id='cbTipo']"));

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//option[. = 'Estado Operativo 90 días calendario']"), driver, true);

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tb_info']"), driver);
            //var elementos = driver.FindElements(By.XPath("//table[@id='tb_info']/tbody/tr")).Count();
            //Console.WriteLine("Elementos tabla:" + elementos);

            //ITSE:
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbTipo']"), driver);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.XPath("//select[@id='cbTipo']"), driver, "Estado Operativo 90 días calendario");

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tb_info']"), driver);
            var elementos = driver.FindElements(By.XPath("//table[@id='tb_info']/tbody/tr")).Count();
            Console.WriteLine("Elementos tabla:" + elementos);
        }

        [Test, Order(3)]
        public void Intranet_FDTI60_Informes_Osinergmin_Consulta_Por_Tipo1()
        {
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbTipo']"), driver);
            //// Vuelve a definir el dropdown, deberia encontrarlo en la pantalla
            //var dropdown = driver.FindElement(By.XPath("//select[@id='cbTipo']"));

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//option[. = 'Mantenimientos Programados 7d']"), driver);
            //dropdown.FindElement(By.XPath("//option[. = 'Mantenimientos Programados 7d']")).Click();

            //FuncionesRecurrentes.WaitFor(5);

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tb_info']"), driver);
            //var elementos = driver.FindElements(By.XPath("//table[@id='tb_info']/tbody/tr")).Count();
            //Console.WriteLine("Elementos tabla tipo1:" + elementos);

            //ITSE:
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbTipo']"), driver);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.XPath("//select[@id='cbTipo']"), driver, "Mantenimientos Programados 7d");
            FuncionesRecurrentes.WaitFor();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tb_info']"), driver);
            var elementos = driver.FindElements(By.XPath("//table[@id='tb_info']/tbody/tr")).Count();
            Console.WriteLine("Elementos tabla tipo1:" + elementos);
        }

        [Test, Order(4)]
        public void Intranet_FDTI60_Informes_Osinergmin_Consulta_Por_Tipo2()
        {
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbTipo']"), driver);
            //var dropdown = driver.FindElement(By.XPath("//select[@id='cbTipo']"));

            //dropdown.FindElement(By.XPath("//option[. = 'Estado Operativo 30 dias']")).Click();

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tb_info']"), driver);
            //var elementos = driver.FindElements(By.XPath("//table[@id='tb_info']/tbody/tr")).Count();
            //Console.WriteLine("Elementos tabla tipo2:" + elementos);
            //FuncionesRecurrentes.WaitFor(5);

            //ITSE
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbTipo']"), driver);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.XPath("//select[@id='cbTipo']"), driver, "Estado Operativo 30 dias");

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tb_info']"), driver);
            var elementos = driver.FindElements(By.XPath("//table[@id='tb_info']/tbody/tr")).Count();
            Console.WriteLine("Elementos tabla tipo2:" + elementos);
            FuncionesRecurrentes.WaitFor();
        }

        [Test, Order(5)]
        public void Intranet_FDTI60_Informes_Osinergmin_Exportar_Registros()
        {
            //try
            //{
            //    FuncionesRecurrentes.EliminarYCrearCarpeta();
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnExportar']"), driver);
            //    driver.FindElement(By.XPath("//input[@id='btnExportar']")).Click();

            //    FuncionesRecurrentes.VerificarArchivoDescargado();
            //}
            //catch (Exception e)
            //{
            //    Assert.Fail(e.Message);
            //}
            //finally { driver.Quit(); }

            //ITSE:
            try
            {
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnExportar']"), driver, true);

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