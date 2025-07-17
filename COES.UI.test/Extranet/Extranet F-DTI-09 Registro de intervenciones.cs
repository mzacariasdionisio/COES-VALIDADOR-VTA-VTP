
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Extranet
{
    [TestFixture]
    public class Extranet_FDTI09_Registro_de_intervenciones:Clase_Base
    {
        [Test, Order(1)]
        public void Extranet_FDTI09_Registro_de_intervenciones_Ingresar_a_la_opcion()
        {
            driver.FindElement(By.LinkText("Operación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Intervenciones"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Registro"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Extranet_FDTI09_Registro_de_intervenciones_Registrar_informacion()
        {
            FuncionesRecurrentes.WaitFor(2);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("tipoProgramacion"), driver, "EJECUTADOS");
            FuncionesRecurrentes.InvisibilityLoading(driver);
            driver.FindElement(By.XPath("//img[@title='Registrar información']")).Click();
            FuncionesRecurrentes.WaitFor(3);
            driver.FindElement(By.XPath("//a[@id='IntervencionesNuevo']/div/img")).Click();
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(4);
            driver.FindElement(By.XPath("//table[@id='tablaDatos']/thead[1]/tr[2]/td[6]/input[6]")).Click();
            FuncionesRecurrentes.WaitFor(2);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresaLinea"), driver, "ACEROS AREQUIPA");
            FuncionesRecurrentes.WaitFor(4);
            driver.FindElement(By.XPath("(//table[@class='tabla-formulario']//td)[2]")).Click();
            //driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Equipo'])[4]/following::td[1]")).Click();

            FuncionesRecurrentes.WaitFor(3);

            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cboTipoEventoPopup"), driver, "MANTENIMIENTO PREVENTIVO");
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cboClaseProgramacionPopup"), driver, "PROGRAMADO");

            FuncionesRecurrentes.dobleClickAndEdit(By.Id("descrip"), driver, DateTime.Now.ToString("s"), By.Id("descrip"));
            FuncionesRecurrentes.WaitFor(3);
            driver.FindElement(By.Id("btnActualizar")).Click();
            FuncionesRecurrentes.WaitFor();
            Assert.AreEqual("¡La Información se grabó correctamente!", CloseAlertAndGetItsText());

            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(3)]
        public void Extranet_FDTI09_Registro_de_intervenciones_Generar_reporte()
        {
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//div[@class='content-item-action']//img)[3]"), driver, true);
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(4)]
        public void Extranet_FDTI09_Registro_de_intervenciones_Edicion_registro()
        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//img[@title='Editar informacion del registro']"), driver, true);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitFor(3);
                FuncionesRecurrentes.dobleClickAndEdit(By.Id("descrip"), driver, "PRUEBA"+DateTime.Now.ToString("s"), By.XPath("//textarea[@id='descrip']"));

                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnActualizar"), driver, true);
                FuncionesRecurrentes.WaitFor();
                Assert.AreEqual("¡La Información se actualizó correctamente!", CloseAlertAndGetItsText());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        private string CloseAlertAndGetItsText()
        {
            bool acceptNextAlert = true;
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
