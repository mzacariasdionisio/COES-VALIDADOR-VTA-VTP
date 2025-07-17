using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using System;
using OpenQA.Selenium.Support.Extensions;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI37_1_Configuracion
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI37_1_Configuracion_Ingresar_a_la_opción()
        {
            driver = Test_Suite.SetUpClass();
            FuncionesRecurrentes.loginIntranet64bits(driver);

            driver.FindElement(By.LinkText("Evaluación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Costo de Oportunidad"), driver, true);
            Actions actions = new Actions(driver);
            actions.MoveToElement(driver.FindElement(By.LinkText("2. Importar insumos"))).Build().Perform();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("2.1 Configuración"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor();
        }

        [Test, Order(2)]
        public void Intranet_FDTI37_1_Configuracion_Consulta_de_registros()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbPeriodo']"), driver);
            FuncionesRecurrentes.NavegarAFechaDrowpdown(By.XPath("//select[@id='cbPeriodo']"), driver);
            FuncionesRecurrentes.WaitFor(1);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbVersion"), driver, "", true, 1);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Consultar']"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(3)]
        public void Intranet_FDTI37_1_Configuracion_Editar_registro()
        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tablaURS']/tbody[1]/tr[1]/td[1]/a[1]/img[1]"), driver, true);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='txtInicioHabilitacion']"), driver);
                FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.XPath("//input[@id='txtInicioHabilitacion']"));
                FuncionesRecurrentes.Otrasfechas("Ultimodiames", driver, By.XPath("//input[@id='txtFinHabilitacion']"));

                FuncionesRecurrentes.dobleClick(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[2]"), driver);
                FuncionesRecurrentes.WaitFor(1);
                driver.FindElement(By.XPath("/html[1]/body[1]/div[15]/ul[1]/li[1]/div[text()='Por unidad']")).Click();

                FuncionesRecurrentes.dobleClick(By.XPath("//td[@class='estilocombo']"), driver);
                FuncionesRecurrentes.WaitFor(1);
                driver.FindElement(By.XPath("/html[1]/body[1]/div/ul[1]/li/div[text()='Por unidad']")).Click();

                driver.ExecuteJavaScript("window.scrollBy(0,250)");
                FuncionesRecurrentes.dobleClick(By.XPath("//div[@id='excelEquipo']//table[@class='htCore']/tbody[1]/tr[1]/td[2]"), driver);
                FuncionesRecurrentes.WaitFor(1);
                driver.FindElement(By.XPath("/html[1]/body[1]/div/ul/li/div[text()='Si']")).Click();

                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//div[@id='excelEquipo']//table[@class='htCore']/tbody[1]/tr[1]/td[3]"),
                    driver, "1", By.XPath("//div[@class='handsontableInputHolder']//textarea[1]"));
                driver.FindElement(By.XPath("//td[@class='estilocombo']")).Click();
                driver.FindElement(By.XPath("//input[@id='btnGrabarConfiguracion']")).Click();

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
                Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("La configuración se grabó satisfactoriamente."),
                    "No se encontró el mensaje de éxito.");
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