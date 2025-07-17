using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("No aplica")]
    public class Intranet_FDTI56_Condiciones_térmicas
    {
        //private string variable;
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        //[Test, Order(1)]
        public void Intranet_FDTI56_Condiciones_térmicas_Ingresar_A_La_Opción()
        {
            //driver = Test_Suite.SetUpClass();

            ////Actions actions = new Actions(driver);

            ////Login
            //FuncionesRecurrentes.loginIntranet(driver);
            //driver.FindElement(By.LinkText("Coordinación")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Yupana Continuo"), driver, true);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Condiciones Térmicas"), driver, true);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI56_Condiciones_térmicas_Nuevo_registro()
        {
            //FuncionesRecurrentes.InvisibilityLoading(driver);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//tbody/tr[1]/td[2]/a[1]/div[1]/img[1]"), driver, true);
            //FuncionesRecurrentes.SelectOptionfromDropdown(By.XPath("//select[@id='cbEmpresa2']"), driver, "BIOENERGIA DEL CHIRA S.A.");
            //FuncionesRecurrentes.WaitFor(2);
            //FuncionesRecurrentes.SelectOptionfromDropdown(By.XPath("//select[@id='cbCentral2']"), driver, "C.T. CAÑA BRAVA");
            //FuncionesRecurrentes.InvisibilityLoading(driver);
            //FuncionesRecurrentes.SelectOptionfromDropdown(By.XPath("//select[@id='cbModoOpGrupo']"), driver, "CANA BRAVA TV1 - BAGAZO");
            //FuncionesRecurrentes.InvisibilityLoading(driver);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnAceptar']"), driver, true);
            //FuncionesRecurrentes.WaitFor(3);
            //IAlert alert = driver.SwitchTo().Alert();
            //alert.Accept();
            //FuncionesRecurrentes.InvisibilityLoading(driver);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//tbody/tr[1]/td[2]/div[2]/div[2]/div[5]/ul[1]/li[1]/a[1]"), driver, true);
            //Assert.That(driver.FindElement(By.XPath("//body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[5]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[1]/div[3]/div[2]/div[1]/table[1]/tbody[1]/tr[1]/td[1]")).Text.Equals("BIOENERGIA DEL CHIRA S.A."));
            //Assert.That(driver.FindElement(By.XPath("//body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[5]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[1]/div[3]/div[2]/div[1]/table[1]/tbody[1]/tr[1]/td[2]")).Text.Equals("C.T. CAÑA BRAVA"));
            //Assert.That(driver.FindElement(By.XPath("//body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[5]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[1]/div[3]/div[2]/div[1]/table[1]/tbody[1]/tr[1]/td[3]")).Text.Equals("CANA BRAVA TV1 - BAGAZO"));
            //FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        //[Test, Order(3)]
        public void Intranet_FDTI56_Condiciones_térmicas_Actualización_automatica()
        {
            //try
            //{
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnActualizar']"), driver, true);
            //    FuncionesRecurrentes.WaitFor(3);
            //    IAlert alert = driver.SwitchTo().Alert();
            //    alert.Accept();
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