using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Intranet
{
    [TestFixture]
    [Category("Ejecutar")]
    public class Intranet_FDTI49_Horas_operacion
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        
        [Test, Order(1)]
        public void Intranet_FDTI49_Horas_operacion_Ingresar_a_la_opcion()
        {
            driver = Test_Suite.SetUpClass();
            FuncionesRecurrentes.loginIntranet(driver);
            driver.FindElement(By.LinkText("Eventos")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Horas de Operación"), driver, true);
            //Logica para cuando no hay datos en BD
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                IAlert alert = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                if (alert != null)
                {
                    FuncionesRecurrentes.WaitFor(2);
                    alert.Accept();
                    // Cambiar fecha a ayer
                    FuncionesRecurrentes.NavegarAFecha("LunesSemanaAnterior", driver);

                    // Reintentar clic en "Consultar"
                    driver.FindElement(By.Id("btnConsultar")).Click();

                    FuncionesRecurrentes.InvisibilityLoading(driver);
                }
            }
            catch (WebDriverTimeoutException)
            {
                //Si hay datos , se continua con la prueba
            }
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Intranet_FDTI49_Horas_operacion_Consulta_registros()
        {
            try
            {
                FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnConsultar']"), driver, true);
                FuncionesRecurrentes.InvisibilityLoading(driver);
            }
            catch
            {
                driver.SwitchTo().Alert().Accept();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnConsultar']"), driver, true);
                FuncionesRecurrentes.InvisibilityLoading(driver);
            }
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//ul[@class='etabs']/li[@class='tab'][1]/a[@href='#vistaListado']"), driver, true);
        }

        [Test, Order(3)]
        public void Intranet_FDTI49_Horas_operacion_Nuevo_registro()
        {
            IWebElement element = driver.FindElement(By.XPath("//table[@id='reporteHO']/tbody/tr[1]/td[2]/a[@class='eli_hop']/img"));
            driver.ExecuteJavaScript("arguments[0].click();", element);
            driver.SwitchTo().Alert().Accept();
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(1);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnAgregarHoraOperacionEms']//div[@class='content-item-action']"), driver, true);
            FuncionesRecurrentes.WaitFor(2);

            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEmpresa2"), driver, "AGRO INDUSTRIAL PARAMONGA S.A.");
            FuncionesRecurrentes.WaitFor(1);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbCentral2"), driver, "C.T. PARAMONGA");
            FuncionesRecurrentes.WaitFor(1);

            driver.FindElement(By.CssSelector(".ms-choice > span")).Click();
            driver.FindElement(By.XPath("//label[contains(.,' PARAMONGA - BAGAZO')]")).Click();
            
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbTipoOp"), driver, "POR TENSION");
            FuncionesRecurrentes.dobleClickAndEdit(By.Id("txtEnParaleloH"), driver, "11:11:11", By.Id("txtEnParaleloH"));
            FuncionesRecurrentes.dobleClickAndEdit(By.Id("txtFueraParaleloH"), driver, "22.22:22", By.Id("txtFueraParaleloH"));
            FuncionesRecurrentes.dobleClickAndEdit(By.Id("TxtDescripcion"), driver, "PRUEBA", By.Id("TxtDescripcion"));

            driver.FindElement(By.Id("btnAceptar2")).Click();
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(4)]
        public void Intranet_FDTI49_Horas_operacion_Descargar_manual_usuario()
        {
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnManual"), driver, true);
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }
        
        [Test, Order(5)]
        public void Intranet_FDTI49_Horas_operacion_Reporte()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnReporte"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("txtFechaInicio"));
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("txtFechaFin"));
            driver.FindElement(By.Id("btnBuscar")).Click();
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(6)]
        public void Intranet_FDTI49_Horas_operacion_Reporte_exportar()
        {
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnExportar"), driver, true);
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }
        /*
        [Test, Order(7)]
        public void Intranet_FDTI49_Horas_operacion_Reporte_osinergmin()
        {
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnExportarOsinergmin"), driver, true);
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }*/

        [Test, Order(7)]
        public void Intranet_FDTI49_Horas_operacion_Edicion_registros()
        {
            try
            {
                driver.FindElement(By.LinkText("Eventos")).Click();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Horas de Operación"), driver, true);
                //Logica para cuando no hay datos en BD
                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                    IAlert alert = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                    if (alert != null)
                    {
                        FuncionesRecurrentes.WaitFor(2);
                        alert.Accept();
                        // Cambiar fecha a ayer
                        FuncionesRecurrentes.NavegarAFecha("LunesSemanaAnterior", driver);

                        // Reintentar clic en "Consultar"
                        driver.FindElement(By.Id("btnConsultar")).Click();

                        FuncionesRecurrentes.InvisibilityLoading(driver);
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    //Si hay datos , se continua con la prueba
                }
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnConsultar']"), driver, true);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//ul[@class='etabs']/li[@class='tab'][1]/a[@href='#vistaListado']"), driver, true);
                IWebElement element = driver.FindElement(By.XPath("//img[@alt='Editar registro']"));
                driver.ExecuteJavaScript("arguments[0].click();", element);

                FuncionesRecurrentes.WaitFor(1);
                FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbTipoOp"), driver, "POR MANIOBRAS");
                FuncionesRecurrentes.dobleClickAndEdit(By.Id("TxtDescripcion"), driver, "TEST", By.Id("TxtDescripcion"));
                driver.FindElement(By.Id("btnAceptar2")).Click();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}