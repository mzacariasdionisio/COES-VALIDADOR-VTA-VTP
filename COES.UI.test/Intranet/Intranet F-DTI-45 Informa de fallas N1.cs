using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System.Threading;


namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI45_InformedefallasN1
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }


        [Test, Order(1)]
        public void Intranet_FDTI45_InformedefallasN1_Ingresar_A_La_Opcion()

        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Eventos")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Informe de Fallas N1"), driver);
            driver.FindElement(By.LinkText("Informe de Fallas N1")).Click();

            //las 2 útimas lineas se consideran por ITSE:
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Informe de Fallas N1"), driver, true);
            //FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Intranet_FDTI45_InformedefallasN1_Consulta_de_registros()

        {
            //try
            //{
                FuncionesRecurrentes.WaitFor(5);
                //FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.Id("FechaDesde"));
                DateTime firstDayMonth = DateTime.Now.AddMonths(-20).AddDays(-DateTime.Now.Day + 1);
                FuncionesRecurrentes.NavegarAFecha("FechaPersonalizada", driver, By.Id("FechaDesde"), firstDayMonth.ToString("dd/MM/yyyy"));

                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value=\'Buscar\']"), driver);
                driver.FindElement(By.XPath("//input[@value=\'Buscar\']")).Click();

                //las 2 últimas lineas se consideran por ITSE:
                //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Buscar']"), driver, true);
            //}
            //catch
            //{
            //    Assert.True(true);
            //}
        }

        [Test, Order(3)]
        public void Intranet_FDTI45_InformedefallasN1_Edicion_de_registro()
        {
            //try
            //{
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//img[@src=\'/AppIntranetApi/Content/Images/btn-edit.png\']"), driver);
            //driver.FindElement(By.XPath("//img[@src=\'/AppIntranetApi/Content/Images/btn-edit.png\']")).Click();
            //FuncionesRecurrentes.dobleClick(By.XPath("//input[@name='EvenCorrmem']"), driver);
            //driver.FindElement(By.XPath("//input[@name=\'EvenCorrmem\']")).SendKeys("10");
            //driver.FindElement(By.XPath("//input[@value=\'Grabar\']")).Click();
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[contains(@class,\'action-exito\')]"), driver);
            //Assert.That(driver.FindElement(By.XPath("//div[contains(@class,\'action-exito\')]")).Text, Is.EqualTo("La operación se realizó con éxito."));
            //driver.FindElement(By.XPath("//input[@id=\'btnCancelar\']")).Click();
            //}
            //catch
            //{
            //    Assert.True(true);
            //}

            //ITSE:
            FuncionesRecurrentes.WaitFor(5);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tablaArea']/tbody/tr[1]/td[1]/a[5]/img[1]"), driver, true);
            Thread.Sleep(500);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            Thread.Sleep(2000);
            FuncionesRecurrentes.dobleClick(By.XPath("//input[@name='EvenCorrmem']"), driver);
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//input[@name='EvenCorrmem']")).SendKeys("10");
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//input[@value='Grabar']")).Click();
            Thread.Sleep(1000);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[contains(@class,'action-exito')]"), driver);
            Thread.Sleep(500);
            Assert.That(driver.FindElement(By.XPath("//div[contains(@class,'action-exito')]")).Text, Is.EqualTo("La operación se realizó con éxito."));
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//input[@id='btnCancelar']")).Click();
        }

        [Test, Order(4)]
        public void Intranet_FDTI45_InformedefallasN1_Eliminar_registro()
        {
            //try
            //{
            //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//img[@src=\'/AppIntranetApi/Content/Images/btn-cancel.png\']"), driver);
            //driver.FindElement(By.XPath("//img[@src=\'/AppIntranetApi/Content/Images/btn-cancel.png\']")).Click();

            ////#alerta y aceptar
            //IAlert alert = driver.SwitchTo().Alert();
            //alert.Accept();
            //}
            //catch
            //{
            //    Assert.True(true);
            //}

            //ITSE:
            FuncionesRecurrentes.WaitFor(5);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@id='tablaArea']/tbody/tr[1]/td[1]/a[6]/img[1]"), driver, true);
            FuncionesRecurrentes.WaitFor(5);
            driver.SwitchTo().Alert().Accept();
            FuncionesRecurrentes.WaitFor(5);
        }

        [Test, Order(5)]
        public void Intranet_FDTI45_InformedefallasN1_Logs_de_envio()

        {
            try
            {
                //FuncionesRecurrentes.InvisibilityLoading(driver);
                //FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value=\'Log de Envíos\']"), driver);
                //FuncionesRecurrentes.EliminarYCrearCarpeta();
                //driver.FindElement(By.XPath("//input[@value=\'Log de Envíos\']")).Click();
                //FuncionesRecurrentes.WaitFor(5);
                //FuncionesRecurrentes.VerificarArchivoDescargado();
                //FuncionesRecurrentes.WaitFor(5);
                ////driver.Quit();
                
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Log de Envíos']"), driver);
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                driver.FindElement(By.XPath("//input[@value='Log de Envíos']")).Click();
                FuncionesRecurrentes.VerificarArchivoDescargado();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);

                //driver.Quit();
                //Assert.True(true);//Assert.IsTrue(false, "Error al descargar la información: " + e.InnerException.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}

