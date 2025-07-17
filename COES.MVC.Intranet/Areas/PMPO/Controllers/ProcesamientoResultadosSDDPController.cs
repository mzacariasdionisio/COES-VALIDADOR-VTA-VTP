using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.PMPO;
using COES.Servicios.Aplicacion.PMPO.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class ProcesamientoResultadosSDDPController : BaseController
    {
        private ProgramacionAppServicio servicio = new ProgramacionAppServicio();

        #region Declaración de variables

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult ProcesarResultados()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            var model = new SDDPModel();
            model.RutaCarga = servicio.GetDirectorioDat();
            model.ListaAnio = servicio.ListarAnio();
            model.ListaPmpoformato = servicio.ListFormatoByArchivo("CSV");

            if (model.ListaAnio.Count > 0) model.ListaMes = servicio.ListarMesxAnio(model.ListaAnio[0].Entero1.Value);

            return View(model);
        }

        [HttpPost]
        public JsonResult ListaMesxAnio(int anio)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                model.ListaMes = servicio.ListarMesxAnio(anio);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult ProcesarArchivos(int pmpericodi, string carpeta)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                base.ValidarSesionJsonResult();

                carpeta = servicio.GetEstructuraRuta(carpeta);

                //Si la generacion es diferente al 100% nos devolvera valor 0 y no permitira el registro
                var estado = servicio.NoExisteGeneracionEnProceso();
                model.Resultado = estado.ToString();

                if (estado == 1) //si no existe proceso ejecutandose
                {
                    List<string> listaMsj = servicio.ValidarCarpetaSDDP(pmpericodi, carpeta);
                    //no existe la carpeta o faltan archivos
                    if (listaMsj.Count > 0)
                    {
                        model.ListaVal = listaMsj;
                        model.Resultado = "-2";
                    }
                    else
                    {
                        //Generacion de version en segundo plano
                        int enviocodi = servicio.GetNuevoEnvioSDDP(pmpericodi, base.UserName);
                        HostingEnvironment.QueueBackgroundWorkItem(token => SaveSDDPSegundoPlano(enviocodi, pmpericodi, carpeta, token));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Procesos segundo plano Generacion de version y exportancion Excel
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaIni2"></param>
        /// <param name="dato"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task SaveSDDPSegundoPlano(int enviocodi, int pmpericodi, string directorioPersonalizado, CancellationToken cancellationToken)
        {
            try
            {
                //Segundo plano Generacion de version
                servicio.GenerarVersionSDDP(enviocodi, pmpericodi, directorioPersonalizado);

                //cambiar a procesado el mes
                var regPeriodo = servicio.GetByIdPmoPeriodo(pmpericodi);
                servicio.ActualizarProcesado(regPeriodo.Pmperifecinimes.Year, regPeriodo.Pmperifecinimes.Month);
            }
            catch (Exception ex)
            {
                MeEnvioDTO reg = servicio.GetByIdMeEnvio(enviocodi);
                reg.Enviodesc = "-1";
                servicio.UpdateMeEnvioDesc(reg);

                //mensajes de error
                servicio.AgregarLog(enviocodi, "Ejecución en segundo plano. "+ ex.ToString(), ConstantesPMPO.TipoEventoLogError, "SISTEMA");

                Log.Error(NameController, ex);
            }
        }

        [HttpPost]
        public JsonResult ListaLogxMes(int pmpericodi)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                model.Resultado = servicio.NoExisteGeneracionEnProceso().ToString();
                model.ListaLog = servicio.ListarLogXMes(pmpericodi, out MeEnvioDTO version);
                model.Envio = version;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult ExportarArchivos(int pmpericodi, int codigoReporte)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                string file = "";
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload;
                int codigoenvio = 0;

                var regPeriodo = servicio.GetByIdPmoPeriodo(pmpericodi);
                DateTime fecha1Mes = regPeriodo.Pmperifecinimes;
                var regEnvio = servicio.GetUltimoEnvioSddp(fecha1Mes);
                if (regEnvio != null) codigoenvio = regEnvio.Enviocodi;
                else
                {
                    throw new ArgumentException("No existe procesamiento para el periodo seleccionado. No está permitido la descarga de información.");
                }

                if (codigoReporte != 18)
                {
                    file = servicio.GenerarReporteIndividual(ruta, codigoenvio, codigoReporte, false);
                }
                else
                {
                    string plantillaFile = ConstantesPMPO.PlantillaSDDP;
                    //string plantillaPath = servicio.GetDirectorioDat();

                    ////validamos que el sistema pueda obtener el archivo (o que en realidad exista)
                    //if (!FileServer.VerificarExistenciaFile("", plantillaFile, plantillaPath))
                    //    throw new Exception("No se encontró la plantilla: " + plantillaPath + " # " + plantillaFile);

                    //string ls_destino = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload;
                    //FileServer.CopiarFileAlterFinalOrigen("", ls_destino, plantillaFile, plantillaPath);

                    file = servicio.GenerarFormatoResultadosPMPO(codigoenvio, AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload, plantillaFile);
                }

                model.Resultado = file;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Descarga el formato
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(int formato, string file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            return File(path, app, file);
        }

        /// <summary>
        /// Permite descargar archivo Excel de Resultados Finales
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult DescargarResultadosSDDP(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload + file;

            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            return File(path, app, string.Format("ResultadosSDDP_{0}.xlsx", sFecha));
        }
    }
}
