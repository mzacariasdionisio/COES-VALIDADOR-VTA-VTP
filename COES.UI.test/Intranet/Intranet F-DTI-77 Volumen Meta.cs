
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using OpenQA.Selenium.Support.UI;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI77_Volumen_Meta
    {
        //static string FilePathFmtos = "C:\\Test\\Formatos";
        static string FilePathFmtos = "C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes";

        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI77_Volumen_Meta_Ingresar_a_la_opcion()
        {
            driver = Test_Suite.SetUpClass();
            FuncionesRecurrentes.loginIntranet(driver);
            driver.FindElement(By.LinkText("Programación")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("YUPANA"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Volumen Meta"), driver, true);
            driver.FindElement(By.LinkText("Volumen Meta")).SendKeys(Keys.Enter);
        }

        [Test, Order(2)]
        public void Intranet_FDTI77_Volumen_Meta_Consulta_registros()
        {
            //try
            //{
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.WaitFor(5);
                FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver, By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/div[1]/table[1]/tbody[1]/tr[1]/td[4]/div[1]/span[1]/input[1]"));
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//input[@id='btnConsultar']"), driver, true);
            //}
            //catch (Exception e)
            //{

            //    Assert.True(true);
            //}
        }

        [Test, Order(3)]
        public void Intranet_FDTI77_Volumen_Meta_Descargar_formato()
        {
            //try
            //{
                FuncionesRecurrentes.InvisibilityLoading(driver);
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                FuncionesRecurrentes.WaitFor(5);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("/html[1]/body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/a[1]/div[1]"), driver, true);
                FuncionesRecurrentes.VerificarArchivoDescargado();
            //}
            //catch (Exception e)
            //{

            //    Assert.True(true);
            //}
        }

        [Test, Order(4)]
        public void Intranet_FDTI77_Volumen_Meta_Importar_formato()
        {
            //try
            //{
                FuncionesRecurrentes.WaitFor(5);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnSelectExcel']//div[@class='content-item-action']"), driver);
                //driver.FindElement(By.XPath("(//span[text()='Importar formato']/following::input)[1]")).SendKeys("C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes\\77_carga.xlsx");
                driver.FindElement(By.XPath("(//span[text()='Importar formato']/following::input)[1]")).SendKeys(FilePathFmtos + "\\77_carga.xlsx");
                FuncionesRecurrentes.WaitFor(5);
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@class='ht_master handsontable']//div[@class='wtHolder']"), driver);
                string valor10 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[3]/td[2]"))).Text;
                string valor30 = driver.FindElement(By.XPath(("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[4]/td[2]"))).Text;
                FuncionesRecurrentes.WaitFor(5);
                Assert.True(valor10 == "10.000", "El valor no coincide con el numero esperado (10)");
                Assert.True(valor30 == "30.000", "El valor no coincide con el numero esperado (30)");
                Console.WriteLine("Valores {0}, {1}", valor10, valor30);
                FuncionesRecurrentes.WaitFor(3);
                Assert.That(driver.FindElement(By.XPath("//div[@id='mensaje']")).Text, Is.EqualTo("Por favor presione el botón enviar para grabar los datos"),
                        "El mensaje no coincide con el texto esperado");
            //}
            //catch (Exception e)
            //{

            //    Assert.True(true);
            //}
        }

        [Test, Order(5)]
        public void Intranet_FDTI77_Volumen_Meta_Grabar_registros()
        {
            try
            {

                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[3]/td[3]"), driver, "20", By.XPath("//textarea[@class='handsontableInput']"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[4]/td[3]"), driver, "40", By.XPath("//textarea[@class='handsontableInput']"));
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//*[@id=\"detalleFormato\"]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[3]/td[2]"), driver, true);
                driver.FindElement(By.XPath("//a[@id='btnEnviarDatos']//div[@class='content-item-action']")).Click();
                driver.SwitchTo().Alert().Accept();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//div[@id='mensaje']"), driver);
                FuncionesRecurrentes.WaitFor(3);
                Assert.That(driver.FindElement(By.XPath("//div[@id='mensaje']")).Text.StartsWith("Los datos se enviaron correctamente"),
                    "El mensaje no coincide con el texto esperado");

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                //Assert.True(true);//Console.WriteLine(e.Message);
            }
            finally { driver.Quit(); }
        }
    }
}