using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.CompensacionRSF.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CompensacionRSF;
using COES.Servicios.Aplicacion.CompensacionRSF.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.SistemasTransmision.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Controllers
{
    public class CompensacionOpController : BaseController
    {
        // GET: /CompensacionRSF/CompensacionOp/

        public CompensacionOpController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }
        
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        CompensacionRSFAppServicio servicioCompensacionRsf = new CompensacionRSFAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        BarraUrsAppServicio servicioURS = new BarraUrsAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            CompensacionOPModel model = new CompensacionOPModel();
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            {
                pericodi = model.ListaPeriodos[0].PeriCodi;
            }
            Log.Info("Entidad Periodo - GetByIdPeriodo");
            model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
            Log.Info("Lista de Versiones - ListVcrRecalculos");
            model.ListaRecalculo = this.servicioCompensacionRsf.ListVcrRecalculos(pericodi); //Ordenado en descendente
            if (model.ListaRecalculo.Count > 0 && vcrecacodi == 0)
            {
                vcrecacodi = (int)model.ListaRecalculo[0].Vcrecacodi;
            }

            if (pericodi > 0 && vcrecacodi > 0)
            {
                Log.Info("Entidad de Versiones - GetByIdVcrRecalculoView");
                model.EntidadRecalculo = this.servicioCompensacionRsf.GetByIdVcrRecalculoView(pericodi, vcrecacodi);
            }
            else
            {
                model.EntidadRecalculo = new VcrRecalculoDTO();
            }
            model.Pericodi = pericodi;
            model.Vcrecacodi = vcrecacodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model); 
        }

        [HttpPost]
        public JsonResult ExportarCompOp(int pericodi = 0, int vcrecacodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                Log.Info("Exportar información - GenerarFormatoVcrCompOp");
                string file = this.servicioCompensacionRsf.GenerarFormatoVcrCompOp(pericodi, vcrecacodi, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [HttpPost]
        public JsonResult ProcesarArchivoCO(string sarchivo , int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            CompensacionOPModel model = new CompensacionOPModel();
            model.sMensaje = "";
            model.sError = "";
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();

            try
            {
                //Elimina información de la tabla VCR_CMPENSOPER
                Log.Info("Elimina información de la tabla VCR_CMPENSOPER - DeleteVcrCmpensoper");
                this.servicioCompensacionRsf.DeleteVcrCmpensoper(vcrecacodi);
                Log.Info("entidadRecalculo - GetByIdVcrRecalculo");
                VcrRecalculoDTO entidadRecalculo = this.servicioCompensacionRsf.GetByIdVcrRecalculo(vcrecacodi);

                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(entidadRecalculo.Pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                //Listado de fechas a grabar
                var dates = new List<DateTime>();
                for (var dt = dFecInicio; dt <= dFecFin; dt = dt.AddDays(1))
                {
                    dates.Add(dt);
                }
                
                //Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();
                ds = this.servicioCompensacionRsf.GeneraDataset(path + sarchivo, 1);

                //Lista de URS
                string[] listaURS = new string[(ds.Tables[0].Columns.Count - 2) / 2]; //Le quitamos las 2 primeras columnas y luego entre 2
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 4)
                    {
                        continue;
                    }
                    //iFila = 4 - Empieza con la cabecera en 2 filas.
                    int iAux = 0;
                    for (int i = 0; i < dtRow.ItemArray.Count(); i++)
                    {
                        if (dtRow[i].ToString() != "null" && dtRow[i].ToString().ToUpper() != "FECHA")
                        {
                            listaURS[iAux] = dtRow[i].ToString();
                            iAux++;
                        }
                    }
                    break;
                }
                //Lee todo el contenido del excel y le descontamos 6 filas hasta donde empieza la data
                string[][] data = new string[ds.Tables[0].Rows.Count - 6][];
                iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 6)
                    {
                        continue;
                    }
                    //A partir de aqui esta la data
                    DateTime dVcmpopfecha = dates[iFila - 6];
                    int iColumna = 2; // Donde empieza la data
                    for (int i = 0; i < listaURS.Count(); i++)
                    {
                        //INSERTAR EL REGISTRO
                        model.EntidadCompensacion = new VcrCmpensoperDTO();
                        model.EntidadCompensacion.Gruponomb = listaURS[i].ToString(); //Nombre del URS
                        Log.Info("EntidadBarraURS - GetByNombrePrGrupo");
                        TrnBarraursDTO dtoBarraURS = this.servicioURS.GetByNombrePrGrupo(listaURS[i].ToString());
                        if (dtoBarraURS != null)
                        {
                            model.EntidadCompensacion.Grupocodi = dtoBarraURS.GrupoCodi;
                            model.EntidadCompensacion.Vcmpopcodi = 0;
                            model.EntidadCompensacion.Vcrecacodi = vcrecacodi;
                            model.EntidadCompensacion.Vcmpopusucreacion = User.Identity.Name;
                            model.EntidadCompensacion.Vcmpopfeccreacion = DateTime.Now;
                            model.EntidadCompensacion.Vcmpopfecha = dVcmpopfecha;
                            //OperacionPorRSF
                            model.EntidadCompensacion.Vcmpopporrsf = UtilSistemasTransmision.ValidarNumero(dtRow[iColumna++].ToString());
                            //BajaEficiencia
                            model.EntidadCompensacion.Vcmpopbajaefic = UtilSistemasTransmision.ValidarNumero(dtRow[iColumna++].ToString());
                            //Insertar registro
                            Log.Info("Insertar registro - SaveVcrCmpensoper");
                            this.servicioCompensacionRsf.SaveVcrCmpensoper(model.EntidadCompensacion);
                        }
                    }
                    
                }
                model.sMensaje = "Felicidades, la carga de información fue exitosa para los " + (iFila - 5) + " dias del mes, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }
        }

        /// <summary>
        /// Abrir el archivo
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesCompensacionRSF.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            return File(path, app, sFecha + "_" + file);
        }

        
        /// <summary>
        /// Permite cargar un archivo Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            base.ValidarSesionUsuario();
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = file.FileName;
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
