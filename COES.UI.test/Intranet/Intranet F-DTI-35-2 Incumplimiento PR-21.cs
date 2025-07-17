
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;


namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI35_2_Incumplimiento_PR_21 
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI35_2_Incumplimiento_PR_21_Ingresar_a_la_opcion()

        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);

            driver.FindElement(By.LinkText("Evaluación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Regulación Secundaria de Frecuencia"), driver);
            driver.FindElement(By.LinkText("Regulación Secundaria de Frecuencia")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Incumplimiento PR-21"), driver);
            driver.FindElement(By.LinkText("Incumplimiento PR-21")).Click();
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }
        
        [Test, Order(2)]
        public void Intranet_FDTI35_2_Incumplimiento_PR_21_Nuevo_Registro()

        {

            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[contains(@id,'btnNuevo')]"), driver);
            driver.FindElement(By.XPath("//input[contains(@id,'btnNuevo')]")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='EntidadIncumpPR21_Vcrincnombre']"), driver);
            driver.FindElement(By.XPath("//input[@id='EntidadIncumpPR21_Vcrincnombre']")).SendKeys("TEST");
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Insertar']"), driver);
            driver.FindElement(By.XPath("//input[@value='Insertar']")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='mensaje']"), driver);
            Assert.That(driver.FindElement(By.XPath("//div[@id='mensaje']")).Text, Is.EqualTo("La información ha sido correctamente registrada"));
            FuncionesRecurrentes.WaitFor(5);

        }

        [Test, Order(3)]
        public void Intranet_FDTI35_2_Incumplimiento_PR_21_Editar_registro()

        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//img[@alt='Editar el registro'])[1]"), driver);
            driver.FindElement(By.XPath("(//img[@alt='Editar el registro'])[1]")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='EntidadIncumpPR21_Vcrincnombre']"), driver);
            driver.FindElement(By.XPath("//input[@id='EntidadIncumpPR21_Vcrincnombre']")).Clear();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='EntidadIncumpPR21_Vcrincnombre']"), driver);
            driver.FindElement(By.XPath("//input[@id='EntidadIncumpPR21_Vcrincnombre']")).SendKeys("PRUEBA");
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@value='Actualizar']"), driver);
            driver.FindElement(By.XPath("//input[@value='Actualizar']")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='mensaje']"), driver);
            Assert.That(driver.FindElement(By.XPath("//div[@id='mensaje']")).Text, Is.EqualTo("La información ha sido correctamente registrada"));

        }

        [Test, Order(4)]
        public void Intranet_FDTI35_2_Incumplimiento_PR_21_Eliminar_registro()
        {
            try
            {
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("(//img[@alt='Eliminar el registro'])[1]"), driver);
                driver.FindElement(By.XPath("(//img[@alt='Eliminar el registro'])[1]")).Click();
                FuncionesRecurrentes.WaitFor(5);
                //#alerta y aceptar
                IAlert alert = driver.SwitchTo().Alert();
                alert.Accept();
                //las 2 lineas de arriba se pueden resumir en: driver.SwitchTo().Alert().Accept();
                FuncionesRecurrentes.WaitFor(5);
                //driver.Quit();
            }
            catch (Exception e)
            {
                //driver.Quit();
                //Assert.IsTrue(false, "Error al terminar: " + e.InnerException.Message);
                Assert.Fail(e.Message);
            }
            finally
            {
                driver.Quit();
            }

        }
    }
}

