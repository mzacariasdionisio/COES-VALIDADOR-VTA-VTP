
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace Intranet
{
    [TestFixture]
    public class Intranet_FDTI61_Creacion_De_IDCOS
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        

        [Test, Order(1)]
        public void Intranet_FDTI61_Creacion_De_IDCOS_Ingresar_A_La_Opcion()
        {
            driver = Test_Suite.SetUpClass();

            FuncionesRecurrentes.loginIntranet(driver);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Coordinación"), driver, true);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.LinkText("Creacion de IDCOS"), driver, true);

            //FuncionesRecurrentes.WaitFor(5);
            //ITSE:
            FuncionesRecurrentes.WaitFor();
        }

        [Test, Order(2)]
        public void Intranet_FDTI61_Creacion_De_IDCOS_Consulta_De_Registros()
        {
            FuncionesRecurrentes.EliminarYCrearCarpeta();
            FuncionesRecurrentes.NavegarAFecha("MesAnteriorDia1", driver);

            FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnGenerarRestricOp"), driver, true);
            FuncionesRecurrentes.VerificarArchivoDescargado();
        }

        [Test, Order(3)]
        public void Intranet_FDTI61_Creacion_De_IDCOS_Crear_Registros()
        {
            try
            {
                FuncionesRecurrentes.EliminarYCrearCarpeta();
                FuncionesRecurrentes.WaitForElementToBeVisible(By.Id("btnGenerar"), driver, true);
                FuncionesRecurrentes.VerificarArchivoDescargado();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}