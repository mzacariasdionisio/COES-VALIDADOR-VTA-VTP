using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Intranet
{
    //[TestFixture]
    //[Ignore("21.06.24: CNalvarte indica que esta prueba no aplica")]
    public class Intranet_FDTI70_Consultas_cruzadas
    {
        //21.06: Carlos indica que esta prueba no aplica

        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        //[Test, Order(1)]
        public void Intranet_FDTI70_Consultas_cruzadas_Ingresar_a_la_opcion()
        {
            //driver = Test_Suite.SetUpClass();
            //FuncionesRecurrentes.loginIntranet(driver);
            //driver.FindElement(By.LinkText("Programación")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Intervenciones"), driver, true);
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Consultas Cruzadas"), driver, true);
            //FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        //[Test, Order(2)]
        public void Intranet_FDTI70_Consultas_cruzadas_Consulta_registros()
        {
            //FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("TipoProgramacion"), driver, "EJECUTADOS");
            //FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("Entidad_Interfechaini"));
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnConsultar']//div[@class='content-item-action']"), driver, true);
        }

        //[Test, Order(3)]
        public void Intranet_FDTI70_Consultas_cruzadas_Nuevo_registro()
        {
            //try
            //{
            //    FuncionesRecurrentes.InvisibilityLoading(driver);
            //    FuncionesRecurrentes.NavegarAFecha("Ayer", driver, By.Id("Entidad_Interfechaini"));
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnNuevo']//img[@class='set_size']"), driver, true);
            //    FuncionesRecurrentes.InvisibilityLoading(driver);

            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnBuscarEquipoPopup']"), driver, true);
            //    FuncionesRecurrentes.WaitFor(2);

            //    driver.FindElement(By.XPath("(//button[@type='button'])[12]")).Click();
            //    driver.FindElement(By.XPath("//div[@id='searchContentEquipo']/table/tbody/tr/td[2]/div/div/ul/li[contains(.,' ACEROS AREQUIPA')]/label")).Click();

            //    FuncionesRecurrentes.WaitFor(4);
            //    driver.FindElement(By.XPath("(//table[@class='tabla-formulario']//td)[1]")).Click();
            //    FuncionesRecurrentes.WaitFor(2);

            //    FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cboTipoEventoPopup"), driver, "MANTENIMIENTO PREVENTIVO");
            //    FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cboClaseProgramacionPopup"), driver, "PROGRAMADO");
            //    FuncionesRecurrentes.dobleClickAndEdit(By.Id("descrip"), driver, DateTime.Now.ToString("s"), By.Id("descrip"));

            //    driver.FindElement(By.Id("btnActualizar")).Click();
            //    FuncionesRecurrentes.WaitFor();
            //    Assert.AreEqual("¡La Información se grabó correctamente!", CloseAlertAndGetItsText());
            //    FuncionesRecurrentes.WaitFor(2);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        //[Test, Order(4)]
        public void Intranet_FDTI70_Consultas_cruzadas_Reporte()
        {
            //try
            //{
            //    FuncionesRecurrentes.EliminarYCrearCarpeta();
            //    FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("//a[@id='btnExportarExcel']//div[@class='content-item-action']"), driver, true);
            //    FuncionesRecurrentes.VerificarArchivoDescargado();
            //}
            //catch (Exception ex)
            //{
            //    //Assert.Fail(ex.Message);// es debido a que no realizó los pasos anteriores
            //    // quizá debido a la ejecución anterior y no se puede eliminar el registro
            //    Console.WriteLine(ex.Message);
            //}
            //finally
            //{
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