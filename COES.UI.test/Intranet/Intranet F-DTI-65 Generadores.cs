using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System.Threading;
using System;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("No aplica")]
    public class Intranet_FDTI65_Generadores
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        //[Test, Order(1)]
        public void Intranet_FDTI65_Generadores_Ingresar_A_La_Opcion()
        {
            //driver = Test_Suite.SetUpClass();
            //FuncionesRecurrentes.EliminarYCrearCarpeta();

            //FuncionesRecurrentes.loginIntranet(driver);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Costos Marginales Nodales"), driver, true);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Equivalencia de Códigos"), driver, true);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Generadores"), driver, true);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI65_Generadores_Nuevo_registro()
        {
            //Thread.Sleep(5000);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnNuevo']"), driver);
            //driver.FindElement(By.XPath("//input[@id='btnNuevo']")).Click();
            //FuncionesRecurrentes.WaitFor(5);

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbEmpresaEdit']"), driver);
            //{
            //    var dropdown = driver.FindElement(By.XPath("//select[@id='cbEmpresaEdit']"));

            //    driver.FindElement(By.XPath("//select[@id=\'cbEmpresaEdit\']")).Click();

            //    dropdown.FindElement(By.CssSelector("*:nth-child(2)")).Click();
            //}
            //driver.FindElement(By.XPath("//input[@name=\'Nombarra\']")).Click();
            //driver.FindElement(By.XPath("//input[@name=\'Nombarra\']")).SendKeys("PRUEBA");
            //driver.FindElement(By.XPath("//input[@name=\'Idgener\']")).Click();
            //driver.FindElement(By.XPath("//input[@name=\'Idgener\']")).SendKeys("PRUEBA");
            //driver.FindElement(By.XPath("//input[@name=\'NombreTna\']")).Click();
            //driver.FindElement(By.XPath("//input[@name=\'NombreTna\']")).SendKeys("PRUEBA");
            //driver.FindElement(By.XPath("//select[@name=\'Estado\']")).Click();
            //{
            //    var dropdown = driver.FindElement(By.XPath("//select[@name=\'Estado\']"));
            //    SelectElement SelectOptions = new SelectElement(dropdown);
            //    SelectOptions.SelectByIndex(1);

            //}
            //driver.FindElement(By.XPath("//input[@value=\'Grabar\']")).Click();

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            //Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("Los datos se grabaron correctamente."));
        }

        //[Test, Order(3)]
        public void Intranet_FDTI65_Generadores_Editar_registro()
        {
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[3]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/a[1]/img[1]"), driver);
            //driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[3]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/a[1]/img[1]")).Click();

            //Thread.Sleep(1000);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='txtNomBarra']"), driver);
            //driver.FindElement(By.XPath("//input[@id='txtNomBarra']")).Click();
            //driver.FindElement(By.XPath("//input[@id='txtNomBarra']")).Clear();
            //driver.FindElement(By.XPath("//input[@id='txtNomBarra']")).SendKeys("PRUEBAX");
            //driver.FindElement(By.XPath("//input[@value=\'Grabar\']")).Click();

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            //Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("Los datos se grabaron correctamente."));
        }

        //[Test, Order(4)]
        public void Intranet_FDTI65_Generadores_Eliminar_Registro()
        {
            //try
            //{
            //    FuncionesRecurrentes.WaitFor(5);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[3]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/a[2]/img[1]"), driver);
            //    driver.FindElement(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[3]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/a[2]/img[1]")).Click();
            //    //#falla anterior linea
            //    IAlert alert = driver.SwitchTo().Alert();
            //    alert.Accept();

            //    FuncionesRecurrentes.WaitFor(5);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='action-exito']"), driver);
            //    Assert.That(driver.FindElement(By.XPath("//div[@class='action-exito']")).Text, Is.EqualTo("El registro se eliminó correctamente."));
            //    driver.Quit();
            //}
            //catch (Exception e)
            //{
            //    driver.Quit();
            //    Assert.IsTrue(false, "Error: " + e.InnerException.Message);
            //}
        }
    }
}