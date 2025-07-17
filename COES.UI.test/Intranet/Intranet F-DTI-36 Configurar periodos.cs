
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI36_Configurar_periodos
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI36_Configurar_periodos_Ingresar_a_la_opcion()

        {
            driver = Test_Suite.SetUpClass();
            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Evaluación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Costo de Oportunidad"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("1. Configurar periodos"), driver, true);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbAnioFiltro']"), driver);
            FuncionesRecurrentes.NavegarAnnioDrowpdown(By.XPath("//select[@id='cbAnioFiltro']"), driver);
            driver.FindElement(By.XPath("//input[@value='Consultar']")).Click();
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }
        
        [Test, Order(2)]
        public void Intranet_FDTI36_Configurar_periodos_Edicion_registro()
        {
            FuncionesRecurrentes.WaitFor(5);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[3]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/a[1]/img[1]"), driver, true);
            FuncionesRecurrentes.WaitFor(5);
            FuncionesRecurrentes.dobleClickAndEdit(By.Id("txtDescripcion"), driver, "PRUEBA", By.Id("txtDescripcion"));

            driver.FindElement(By.XPath("//input[@value='Grabar']")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito'][contains(.,'Los datos se grabaron correctamente.')]"), driver);
        }

        [Test, Order(3)]
        public void Intranet_FDTI36_Configurar_periodos_Eliminar_registro()
        {
            FuncionesRecurrentes.WaitFor(2);
            FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//input[@type='search']"), driver, "enero", By.XPath("//input[@type='search']"));
            FuncionesRecurrentes.WaitFor(1);
            string clase = driver.FindElement(By.XPath("//table[@id='tabla']/tbody[1]/tr[1]/td[1]")).GetAttribute("class");
            if (clase.Contains("dataTables_empty"))
            {
                Console.WriteLine("Sin registros para eliminar.");
            }
            else
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[1]/tbody[1]/tr[1]/td[1]/a[@title='Eliminar']/img[1]"), driver, true);
                FuncionesRecurrentes.WaitFor(2);
                driver.SwitchTo().Alert().Accept();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito'][contains(.,'El registro se eliminó correctamente.')]"), driver);
            }
        }

        [Test, Order(4)]
        public void Intranet_FDTI36_Configurar_periodos_Nuevo_registro()
        {
            FuncionesRecurrentes.WaitFor(2);
            FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//input[@type='search']"), driver, "enero", By.XPath("//input[@type='search']"));
            FuncionesRecurrentes.WaitFor(2);
            string clase = driver.FindElement(By.XPath("//table[@id='tabla']/tbody[1]/tr[1]/td[1]")).GetAttribute("class");
            if (clase.Contains("dataTables_empty"))
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Nuevo']"), driver, true);
                FuncionesRecurrentes.WaitFor(1);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-message'][contains(.,'Por favor complete los datos')]"), driver);
                FuncionesRecurrentes.NavegarAnnioDrowpdown(By.Id("cbAnio"), driver);
                FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbMes"), driver, "ENERO");
                FuncionesRecurrentes.dobleClickAndEdit(By.Id("txtDescripcion"), driver, "ENERO "+DateTime.Now.Year, By.Id("txtDescripcion"));
                FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEstado"), driver, "Activo");

                driver.FindElement(By.XPath("//input[@value='Grabar']")).Click();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito'][contains(.,'Los datos se grabaron correctamente.')]"), driver);
            }
        }

        [Test, Order(5)]
        public void Intranet_FDTI36_Configurar_periodos_Visualizar_reprogramas()
        {
            try
            {
                FuncionesRecurrentes.WaitFor(2);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[3]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/a[5]/img[1]"), driver, true);
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

