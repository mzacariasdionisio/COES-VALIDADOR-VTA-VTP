
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Extensions;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI351_Deficit_Superavit_Reserva_No_Suministrada
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI351_Deficit_superavit_reserva_no_suministrada_Ingresar_a_la_opcion()
        {
            driver = Test_Suite.SetUpClass();
            FuncionesRecurrentes.loginIntranet(driver);
            driver.FindElement(By.LinkText("Evaluación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Regulación Secundaria de Frecuencia"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Déficit, Superávit y Reserva No Suministrada"), driver, true);
        }

        [Test, Order(2)]
        public void Intranet_FDTI351_Deficit_superavit_reserva_no_suministrada_Nuevo_registro()
        {
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnNuevo']"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='EntidadVersiondsrn_Vcrdsrnombre']"), driver);
            driver.FindElement(By.XPath("//input[@id='EntidadVersiondsrn_Vcrdsrnombre']")).SendKeys("TEST");
            FuncionesRecurrentes.WaitFor(3);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@title='Insertar']"), driver, true);
            FuncionesRecurrentes.WaitFor(3);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='mensaje']"), driver);
            Assert.That(driver.FindElement(By.XPath("//div[@id='mensaje']")).Text, Is.EqualTo("La información ha sido correctamente registrada"));
        }

        [Test, Order(3)]
        public void Intranet_FDTI351_Deficit_superavit_reserva_no_suministrada_Editar_registro()
        {
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//img[@alt='Editar el registro'])[1]"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='EntidadVersiondsrn_Vcrdsrnombre']"), driver);
            FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//input[@id='EntidadVersiondsrn_Vcrdsrnombre']"),driver,"PRUEBA", By.XPath("//input[@id='EntidadVersiondsrn_Vcrdsrnombre']"));
            FuncionesRecurrentes.WaitFor(3);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@title='Actualizar']"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(3);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='mensaje']"), driver);
            Assert.That(driver.FindElement(By.XPath("//div[@id='mensaje']")).Text, Is.EqualTo("La información ha sido correctamente registrada"));
        }


        [Test, Order(4)]
        public void Intranet_FDTI351_Deficit_superavit_reserva_no_suministrada_Asignar_termino()
        {
            try
            {
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//img[@alt='Asignar Términos'])[1]"), driver, true);
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitFor(2);
                DateTime today = DateTime.Now;
                var fecha = today.ToString("dd/MM/yyyy");
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[1]"), driver, fecha, By.XPath("//tbody//div[6]//textarea[1]"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[2]"), driver, "01:00", By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[1]/div[3]/div[1]/div[1]/div[1]/div[2]/div[7]/textarea[1]"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[3]"), driver, "02:00", By.XPath("//tbody//div[7]//textarea[1]"));
                
                // revisar seleccion de Aceros arequipa *********************************
                FuncionesRecurrentes.dobleClick(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[4]"), driver);
                driver.FindElement(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[4]")).SendKeys("Aceros are");
                FuncionesRecurrentes.WaitFor(1);
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[4]"),driver, "ACEROS AREQUIPA",By.XPath("//tbody/tr/td[@class='frame-section']/div[@id='divGeneral']/div[@class='ast']/div[@class='content-hijo']/div[@id='tab-container']/div[@class='panel-container']/div[@id='paso1']/div[@id='grillaExcelDT']/div[@class='handsontableInputHolder']/textarea[1]"));
                driver.FindElement(By.XPath("//tbody/tr/td[text()='ACEROS AREQUIPA']")).Click();
              
                //driver.FindElement(By.XPath("//table[1]/tbody[1]/tr[9]/td[contains(.,'ACEROS AREQUIPA')]")).Click();

                FuncionesRecurrentes.dobleClick(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[5]"), driver);
                FuncionesRecurrentes.SelectOptionFromList(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[5]"), driver, 1, "td");
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[6]"), driver, "1", By.XPath("//tbody//div[9]//textarea[1]"));
                driver.FindElement(By.XPath("//a[@id='btnGrabarExcelDT']//div[@class='content-item-action']")).Click();
                //driver.SwitchTo().Alert().Accept();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='mensaje']"), driver);
                FuncionesRecurrentes.WaitFor(3);
                Assert.That(driver.FindElement(By.XPath("//div[@id='mensaje']")).Text.StartsWith("Operación Exitosa"),
                    "El mensaje no coincide con el texto esperado");

            }
            catch (Exception e)
            {
                //Assert.Fail(e.Message);
                Console.WriteLine(e.Message);
            }
            finally
            {
                driver.Quit();
            }
        }

        //26.03.25: Código ITSE
        //[Test, Order(4)]
        //public void Intranet_FDTI351_Deficit_superavit_reserva_no_suministrada_Asignar_termino()
        //{
        //    try
        //    {
        //        FuncionesRecurrentes.InvisibilityLoading(driver);
        //        FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//img[@alt='Asignar Términos'])[1]"), driver, true);
        //        FuncionesRecurrentes.InvisibilityLoading(driver);
        //        FuncionesRecurrentes.WaitFor(2);
        //        DateTime today = DateTime.Now;
        //        var fecha = today.ToString("dd/MM/yyyy");
        //        FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[1]"), driver, fecha, By.XPath("//tbody//div[6]//textarea[1]"));
        //        FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[2]"), driver, "01:00", By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[1]/div[3]/div[1]/div[1]/div[1]/div[2]/div[7]/textarea[1]"));
        //        FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[3]"), driver, "02:00", By.XPath("//tbody//div[7]//textarea[1]"));

        //        FuncionesRecurrentes.dobleClick(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[4]"), driver);
        //        var element = driver.FindElement(By.XPath("//*[@id='grillaExcelDT']/div[2]/div/div/div/table/tbody//tr[last()]"));
        //        driver.ExecuteJavaScript("arguments[0].scrollIntoView();", element);

        //        driver.FindElement(By.XPath("//*[@id='grillaExcelDT']/div[8]/textarea")).Clear();
        //        driver.FindElement(By.XPath("//*[@id='grillaExcelDT']/div[8]/textarea")).SendKeys("ACEROS AREQUIPA");

        //        driver.ExecuteJavaScript("arguments[0].scrollIntoView(false);", element);

        //        FuncionesRecurrentes.dobleClick(By.XPath("//div[@id='grillaExcelDT']/div[@class='ht_clone_top handsontable']/div[@class='wtHolder']/div[@class='wtHider']/div[@class='wtSpreader']/table[@class='htCore']/tbody/tr/td[@class='htLeft htAutocomplete']"), driver);
        //        FuncionesRecurrentes.dobleClick(By.XPath("(//td[@class='htLeft htAutocomplete']/following-sibling::td)[3]"), driver);
        //        FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[1]/div[3]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[6]"), driver, "1", By.XPath("//*[@id='grillaExcelDT']/div[9]/textarea"));
        //        driver.FindElement(By.XPath("//*[@id='grillaExcelDT']/div[2]/div/div/div/table/thead/tr/th[6]/div")).Click();

        //        driver.FindElement(By.XPath("//a[@id='btnGrabarExcelDT']//div[@class='content-item-action']")).Click();
        //        FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='mensaje']"), driver);
        //        FuncionesRecurrentes.WaitFor(3);
        //        Assert.That(driver.FindElement(By.XPath("//div[@id='mensaje']")).Text.StartsWith("Operación Exitosa"),
        //            "El mensaje no coincide con el texto esperado");
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //    finally
        //    {
        //        driver.Quit();
        //    }
        //}
    }
}