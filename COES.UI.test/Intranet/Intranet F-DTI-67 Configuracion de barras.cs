using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium.Support.UI;
using System;
using OpenQA.Selenium.Support.Extensions;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("No aplica")]
    public class Intranet_FDTI67_Configuracion_de_barras
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }


        //[Test, Order(1)]
        public void Intranet_FDTI67_Configuracion_de_barras_Ingresar_A_La_Opcion()
        {
            //driver = Test_Suite.SetUpClass();

            //FuncionesRecurrentes.loginIntranet(driver);

            //driver.FindElement(By.LinkText("Costos Marginales Nodales")).Click();
            //driver.ExecuteJavaScript("window.scrollBy(0,250)");
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Configuración"), driver, true);
            //driver.ExecuteJavaScript("window.scrollBy(0,250)");
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Configuración de Barras"), driver, true);
            //FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI67_Configuracion_de_barras_Nuevo_Registro()
        {
            //FuncionesRecurrentes.WaitFor(4);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnNuevo"), driver, true);
            ////driver.FindElement(By.Id("btnNuevo")).Click();

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("txtNodoBarra"), driver);
            //driver.FindElement(By.Id("txtNodoBarra")).SendKeys("PRUEBA");
            //driver.FindElement(By.Id("txtNombreBarra")).SendKeys("WEB");
            //driver.FindElement(By.Id("btnGrabar")).Click();

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            //Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("Los datos se grabaron correctamente."));
        }

        //[Test, Order(3)]
        public void Intranet_FDTI67_Configuracion_de_barras_Editar_Registro()
        {
            //{
            //    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //    var elementos = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"loading\"]/div[2]")));
            //}
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[3]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/a[1]/img[1]"), driver);
            //driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[3]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/a[1]/img[1]")).Click();

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//div[@id='popupRelacion'])[1]"), driver, true);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@name=\'Nodobarra\']"), driver);
            //driver.FindElement(By.XPath("//input[@name=\'Nodobarra\']")).Click();
            //driver.FindElement(By.XPath("//input[@name=\'Nodobarra\']")).Clear();
            //driver.FindElement(By.XPath("//input[@name=\'Nodobarra\']")).SendKeys("TEST2");
            //driver.FindElement(By.XPath("//input[@value=\'Grabar\']")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            //Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("Los datos se grabaron correctamente."));
        }

        //[Test, Order(4)]
        public void Intranet_FDTI67_Configuracion_de_barras_Eliminar_Registro()
        {
            //FuncionesRecurrentes.WaitFor(5);
            //try
            //{
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[3]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/a[2]/img[1]"), driver);
            //    driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[3]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/a[2]/img[1]")).Click();
            //    FuncionesRecurrentes.WaitFor(1);
            //    driver.SwitchTo().Alert().Accept();
            //    FuncionesRecurrentes.WaitFor(3);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            //    Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("El registro se eliminó correctamente."));
            //}
            //catch (Exception e)
            //{
            //    Assert.Fail(e.Message);
            //}
            //finally {
            //    driver.Quit();
            //}
        }
    }
}