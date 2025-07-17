using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("No aplica")]
    public class Intranet_FDTI66_Lineas_transformadores
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }


        //[Test, Order(1)]
        public void Intranet_FDTI66_Lineas_transformadores_Ingresar_a_la_opcion()

        {
            //driver = Test_Suite.SetUpClass();

            //FuncionesRecurrentes.loginIntranet(driver);

            //driver.FindElement(By.LinkText("Costos Marginales Nodales")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Equivalencia de Códigos"), driver, true);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Líneas y Transformadores"), driver, true);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI66_Lineas_transformadores_Nueva_Linea()
        {
            //try
            //{
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnNuevoLinea']"), driver);
            //    driver.FindElement(By.XPath("//input[@id='btnNuevoLinea']")).Click();

            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbEmpresaLinea']"), driver);
            //    {
            //        var dropdown = driver.FindElement(By.XPath("//select[@id='cbEmpresaLinea']"));
            //        driver.FindElement(By.XPath("//select[@id='cbEmpresaLinea']")).Click();
            //        dropdown.FindElement(By.CssSelector("*:nth-child(2)")).Click();
            //    }

            //    driver.FindElement(By.XPath("//select[@id='cbEquipoLinea']")).Click();
            //    {
            //        var dropdown = driver.FindElement(By.XPath("//select[@id='cbEquipoLinea']"));
            //        SelectElement SelectOptions = new SelectElement(dropdown);
            //        SelectOptions.SelectByIndex(1);
            //    }

            //    driver.FindElement(By.XPath("//input[@name='Nodobarra1']")).Click();
            //    driver.FindElement(By.XPath("//input[@name='Nodobarra1']")).SendKeys("PRUEBA");
            //    driver.FindElement(By.XPath("//input[@name='Nodobarra2']")).Click();
            //    driver.FindElement(By.XPath("//input[@name='Nodobarra2']")).SendKeys("PRUEBA");
            //    driver.FindElement(By.XPath("//input[@name='Nombretna1']")).Click();
            //    driver.FindElement(By.XPath("//input[@name='Nombretna1']")).SendKeys("PRUEBA");
            //    driver.FindElement(By.XPath("//input[@value='Grabar']")).Click();

            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id=\"loading\"]/div[2]"), driver);
            //    {
            //        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //        var elementos = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"loading\"]/div[2]")));
            //    }

            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            //    Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("La operación se realizó correctamente"));
            //    driver.Quit();
            //}
            //catch
            //{
            //    driver.Quit();
            //    Assert.IsTrue(true, "False positive");
            //}
        }

        //[Test, Order(3)]
        public void Intranet_FDTI66_Lineas_transformadores_Edicion_registro()
        {
            //try
            //{
            //    {
            //        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //        var elementos = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"loading\"]/div[2]")));
            //    }
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//img[@src='/AppIntranetApi/Content/Images/btn-edit.png'])[1]"), driver, true);

            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//div[@id='popupEdicion'])[1]"), driver, true);

            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@name='Nodobarra1']"), driver, true);
            //    driver.FindElement(By.XPath("//input[@name='Nodobarra1']")).Clear();
            //    driver.FindElement(By.XPath("//input[@name='Nodobarra1']")).SendKeys("PRUEBAX");
            //    driver.FindElement(By.XPath("//input[@value=\'Grabar\']")).Click();

            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            //    Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("La operación se realizó correctamente"));
            //    driver.Quit();
            //}
            //catch
            //{
            //    driver.Quit();
            //    Assert.IsTrue(true, "False positive");
            //}
        }

        //[Test, Order(4)]
        public void Intranet_FDTI66_Lineas_transformadores_Eliminar_registro()
        {
            //try
            //{
            //    FuncionesRecurrentes.WaitFor(5);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id=\"tablaLinea\"]/tbody[1]/tr"), driver);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id=\"tablaLinea\"]/tbody[1]/tr[1]/td[1]/a[2]/img[1]"), driver, true);

            //    IAlert alert = driver.SwitchTo().Alert();
            //    alert.Accept();
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id=\"loading\"]/div[2]"), driver);
            //    {
            //        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //        var elementos = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"loading\"]/div[2]")));
            //    }
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);

            //    Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("La operación se realizó correctamente."));
            //    driver.Quit();
            //}
            //catch
            //{
            //    driver.Quit();
            //    Assert.IsTrue(true, "False positive");
            //}
        }
    }
}