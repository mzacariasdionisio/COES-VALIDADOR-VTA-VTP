
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI29_Volumenes_caudales_CMgCP
    {
        //static string FilePathFmtos = "C:\\Test\\Formatos";
        static string FilePathFmtos = "C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes";

        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }

        [Test, Order(1)]
        public void Intranet_FDTI29_Volumenes_caudales_CMgCP_Ingresar_a_la_opcion()
        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);
            driver.FindElement(By.LinkText("Mediciones")).Click();
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Hidrología"), driver, true);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Volúmenes y Caudales CMgCP"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);

            FuncionesRecurrentes.SelectOptionfromDropdown(By.Id("cbEtapa"), driver, "Postoperativa");
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnConsultar"), driver, true);
            FuncionesRecurrentes.InvisibilityLoading(driver);
        }

        [Test, Order(2)]
        public void Intranet_FDTI29_Volumenes_caudales_CMgCP_Descarga_de_plantilla()
        {
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnDescargarF']//div[@class='content-item-action']"), driver, true);
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(3)]
        public void Intranet_FDTI29_Volumenes_caudales_CMgCP_Importar_plantilla_de_carga()
        {
            string valor1 = driver.FindElement(By.XPath(("//body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/div[2]/div[2]/div[4]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]"))).Text;
            string valor2 = driver.FindElement(By.XPath(("//body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/div[2]/div[2]/div[4]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[3]"))).Text;
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//a[@id='btnImportarF']//div[@class='content-item-action']"), driver);

            //driver.FindElement(By.XPath("//table[@class='table-search']/tbody[1]/tr[1]/td[3]/div[1]/input[1]")).SendKeys("C:\\CoesDevOps\\PruebasRutaCritica\\FrameworkCoes\\29_carga.xlsx");
            driver.FindElement(By.XPath("//table[@class='table-search']/tbody[1]/tr[1]/td[3]/div[1]/input[1]")).SendKeys( FilePathFmtos+ "\\29_carga.xlsx");

            FuncionesRecurrentes.InvisibilityLoading(driver);
            FuncionesRecurrentes.WaitFor(3);
            FuncionesRecurrentes.WaitForElementToBeVisible(By.XPath("//table[@class='htCore']"), driver);
            string valor11 = driver.FindElement(By.XPath(("//body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/div[2]/div[2]/div[4]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]"))).Text;
            string valor22 = driver.FindElement(By.XPath(("//body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/div[2]/div[2]/div[4]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[3]"))).Text;
            string valor33 = driver.FindElement(By.XPath(("//body[1]/div[4]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[2]/div[2]/div[2]/div[1]/div[2]/div[2]/div[4]/div[1]/div[1]/div[1]/div[1]/table[1]/tbody[1]/tr[1]/td[4]"))).Text;

            Assert.True(valor11 == "10.000", "El valor no coincide con el numero esperado (10)");
            Assert.True(valor22 == "10.000", "El valor no coincide con el numero esperado (20)");
            Assert.True(valor33 == "30.000", "El valor no coincide con el numero esperado (30)");
            Console.WriteLine("Valor 1 inicial {0} y final {1}, valor 2 inicial {2} y final {3}", valor1, valor11, valor2, valor22);
        }

        [Test, Order(4)]
        public void Intranet_FDTI29_Volumenes_caudales_CMgCP_Grabar_registros()
        {
            try
            {
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[2]"), driver, "11", By.XPath("//textarea[@class='handsontableInput']"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[3]"), driver, "12", By.XPath("//textarea[@class='handsontableInput']"));
                FuncionesRecurrentes.dobleClickAndEdit(By.XPath("//table[@class='htCore']/tbody[1]/tr[1]/td[4]"), driver, "13", By.XPath("//textarea[@class='handsontableInput']"));

                driver.FindElement(By.XPath("(//div[@class='content-item-action'])[3]")).Click();
                driver.SwitchTo().Alert().Accept();
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally { driver.Quit(); }
        }
    }
}
