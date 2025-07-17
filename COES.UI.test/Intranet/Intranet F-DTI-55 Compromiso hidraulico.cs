using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("21.06.24: CNalvarte indica que esta prueba no aplica")]
    public class Intranet_FDTI55_Compromiso_hidraulico
    {
        //21.06: Carlos indica que esta prueba no aplica

        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        //[Test, Order(1)]
        public void Intranet_FDTI55_Compromiso_hidraulico_Ingresar_a_la_opcion()
        {
            //driver = Test_Suite.SetUpClass();
            //FuncionesRecurrentes.loginIntranet(driver);
            //driver.FindElement(By.LinkText("Coordinación")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Yupana Continuo"), driver, true);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Compromiso Hidráulico"), driver, true);
            //FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI55_Compromiso_hidraulico_Consulta_registros()
        {
            //FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);
            //FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        //[Test, Order(3)]
        public void Intranet_FDTI55_Compromiso_hidraulico_Nuevo_registro()
        {
            //try
            //{
            //    var agregarEsVisible = driver.FindElement(By.Id("btnEditarDataSC")).Displayed;
            //    if (agregarEsVisible)
            //    {
            //        FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnEditarDataSC"), driver, true);
            //    }
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tabla_SC']/tbody[1]/tr[1]/td[2]/input[1]"), driver, true);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnGuardarDataSC"), driver, true);
            //    FuncionesRecurrentes.WaitFor(2);
            //    Assert.AreEqual("¿Desea guardar la información?", CloseAlertAndGetItsText());
            //    FuncionesRecurrentes.WaitFor();
            //    Assert.AreEqual("Los datos se enviaron correctamente. ", CloseAlertAndGetItsText());
            //    FuncionesRecurrentes.WaitFor();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        //[Test, Order(4)]
        public void Intranet_FDTI55_Compromiso_hidraulico_Edicion_registro()
        {
            //try
            //{
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnEditarDataSC"), driver, true);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tabla_SC']/tbody[1]/tr[5]/td[2]/input[1]"), driver, true);
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnGuardarDataSC"), driver, true);
            //    FuncionesRecurrentes.WaitFor(1);
            //    Assert.AreEqual("¿Desea guardar la información?", CloseAlertAndGetItsText());
            //    FuncionesRecurrentes.WaitFor(2);
            //    Assert.That(CloseAlertAndGetItsText().StartsWith("Los datos se enviaron correctamente. "),
            //        "El mensaje no coincide con el texto esperado");
            //    FuncionesRecurrentes.WaitFor(2);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        //[Test, Order(5)]
        public void Intranet_FDTI55_Compromiso_hidraulico_Gestionar_centrales()
        {
            //try
            //{
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnGestionarCentrales"), driver, true);
            //}
            //catch (Exception ex)
            //{
            //    //Assert.Fail(ex.Message);
            //    Console.WriteLine(ex.Message);
            //}
            //finally {
            //    driver.Quit();
            //}
        }

        //private string CloseAlertAndGetItsText()
        //{
            //bool acceptNextAlert = true;
            //try
            //{
            //    IAlert alert = driver.SwitchTo().Alert();
            //    string alertText = alert.Text;
            //    if (acceptNextAlert)
            //    {
            //        alert.Accept();
            //    }
            //    else
            //    {
            //        alert.Dismiss();
            //    }
            //    return alertText;
            //}
            //finally
            //{
            //    acceptNextAlert = true;
            //}
        //}
    }
}