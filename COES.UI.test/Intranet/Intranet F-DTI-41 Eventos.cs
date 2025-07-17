using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("21.06.24: CNalvarte indica que esta prueba no aplica")]
    public class Intranet_FDTI41_Eventos
    {
        //21.06: Carlos indica que esta prueba no aplica
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

       //[Test, Order(1)]
        public void Intranet_FDTI41_Eventos_Ingresar_a_la_opcion()
        {
            //driver = Test_Suite.SetUpClass();
            //FuncionesRecurrentes.loginIntranet64bits(driver);
            //driver.FindElement(By.LinkText("Eventos")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//ul[@id='menu']/li[2]/ul/li/a"), driver, true);
            //FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI41_Eventos_Consulta_registros()
        {
            //FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("FechaDesde"));
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value=\'Buscar\']"), driver);
            //driver.FindElement(By.XPath("//input[@value=\'Buscar\']")).Click();
        }

        //[Test, Order(3)]
        public void Intranet_FDTI41_Eventos_Nuevo_evento()
        {
            //FuncionesRecurrentes.WaitFor(5);
            //driver.FindElement(By.Id("btnNuevo")).Click();
            //FuncionesRecurrentes.InvisibilityLoading(driver);
            //FuncionesRecurrentes.WaitFor(2);
            //FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "ACEROS AREQUIPA");
            //FuncionesRecurrentes.WaitFor(4);
            //driver.FindElement(By.XPath("//table[@id='tabla']/tbody[1]/tr[1]/td[1]")).Click();
            //FuncionesRecurrentes.WaitFor(2);

            //FuncionesRecurrentes.dobleClickAndEdit(By.Id("txtDescripcion"), driver, "PRUEBA", By.Id("txtDescripcion"));
            //FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbSubCausaEvento"), driver, "PROGRAMADO");

            //FuncionesRecurrentes.WaitFor(3);
            //driver.FindElement(By.Id("btnGrabar")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[contains(@class,\'action-exito\')]"), driver);
            //Assert.That(driver.FindElement(By.XPath("//div[contains(@class,\'action-exito\')]")).Text, Is.EqualTo("La operación se realizó con éxito."));
            //driver.FindElement(By.Id("btnCancelar")).Click();
            //FuncionesRecurrentes.WaitFor(3);
        }

        //[Test, Order(4)]
        public void Intranet_FDTI41_Eventos_Nueva_bitacora()
        {
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnBitacora"), driver, true);
            //FuncionesRecurrentes.InvisibilityLoading(driver);
            //FuncionesRecurrentes.WaitFor(4);
            //FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "ACEROS AREQUIPA");
            //FuncionesRecurrentes.WaitFor(4);
            //FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa"), driver, "ACEROS AREQUIPA");
            //FuncionesRecurrentes.WaitFor(4);
            //driver.FindElement(By.XPath("//table[@id='tabla']/tbody[1]/tr[1]/td[1]")).Click();
            //FuncionesRecurrentes.WaitFor(2);
            //FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbSubCausaEvento"), driver, "NO IDENTIFICADO");
            //FuncionesRecurrentes.dobleClickAndEdit(By.Id("txtDescripcion"), driver, "PRUEBA", By.Id("txtDescripcion"));
            //FuncionesRecurrentes.WaitFor(3);
            //driver.FindElement(By.Id("btnGrabar")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[contains(@class,\'action-exito\')]"), driver);
            //Assert.That(driver.FindElement(By.XPath("//div[contains(@class,\'action-exito\')]")).Text, Is.EqualTo("La operación se realizó con éxito."));
            //driver.FindElement(By.Id("btnCancelar")).Click();
            //FuncionesRecurrentes.WaitFor(3);
        }

        //[Test, Order(5)]
        public void Intranet_FDTI41_Eventos_Exportar_registros()
        {
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnExportar"), driver, true);
            //FuncionesRecurrentes.EliminarYCrearCarpeta();
            //FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        //[Test, Order(6)]
        public void Intranet_FDTI41_Eventos_Exportar_detallado()
        {
            //FuncionesRecurrentes.EliminarYCrearCarpeta();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnExportarDetalle"), driver, true);
            //FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        //[Test, Order(7)]
        public void Intranet_FDTI41_Eventos_Editar_registro()
        {
            //driver.FindElement(By.XPath("//table[@id='tabla']/tbody/tr/td[2]/a/img")).Click();
            //FuncionesRecurrentes.WaitFor(3);
            //FuncionesRecurrentes.dobleClickAndEdit(By.Id("txtDescripcion"), driver, "PRUEBAX", By.XPath("//textarea[@id='txtDescripcion']"));

            //FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnGrabar"), driver, true);
            //driver.FindElement(By.Id("btnCancelar")).Click();
            //FuncionesRecurrentes.WaitFor(3);
        }

        //[Test, Order(8)]
        public void Intranet_FDTI41_Eventos_Eliminar_registro()
        {
            //try
            //{
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tabla']/tbody[1]/tr[1]/td[2]/a[2]/img[1]"), driver, true);
            //    Assert.AreEqual("¿Está seguro de realizar esta acción?", CloseAlertAndGetItsText());
            //}
            //catch (Exception ex)
            //{
            //    Assert.Fail(ex.Message);
            //}
            //finally { driver.Quit();
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