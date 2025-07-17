using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using OpenQA.Selenium.Support.Extensions;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI30_Consulta_De_Datos_Diario
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI30_Consulta_de_datos_diario_Ingresar_a_la_opcion()
        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Programación")).Click();
            driver.ExecuteJavaScript("window.scrollBy(0,250)");
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("YUPANA"), driver, true);
            FuncionesRecurrentes.WaitFor(2);
            driver.ExecuteJavaScript("window.scrollBy(0,250)");
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Consulta de Datos – Diario"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);

            //FuncionesRecurrentes.NavegarAFecha("Ayer", driver, By.Id("txtFecha"));
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("txtFecha"));
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(2);
            driver.FindElement(By.Id("cbTipoCaso")).Click();
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbTipoCaso"), driver, "Solo oficiales");
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbTipoCaso"), driver, "Todos");
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//select[@id='cbTopologia']"), driver);
            FuncionesRecurrentes.WaitFor(2);

            FuncionesRecurrentes.dobleClick(By.Id("cbTopologia"), driver);
            FuncionesRecurrentes.WaitFor(2);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbTopologia"), driver, "", true, 1);
            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbTipoInformacion"), driver, "Aportes Embalse");
            FuncionesRecurrentes.WaitFor(2);
            driver.FindElement(By.Id("btnConsultar")).Click();

            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("tablaListado_wrapper"), driver);
        }

        [Test, Order(2)]
        public void Intranet_FDTI30_Consulta_de_datos_diario_Exportar_datos()
        {
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            driver.FindElement(By.Id("btnExportar")).Click();
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(3)]
        public void Intranet_FDTI30_Consulta_de_datos_diario_Reporte_resumen()
        {
            try
            {
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                driver.FindElement(By.Id("btnReporte")).Click();
                FuncionesRecurrentes.VerificarArchivoDescargado();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally { driver.Quit(); }
        }
    }
}