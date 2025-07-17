using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.CompensacionRSF.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CompensacionRSF;
using COES.Servicios.Aplicacion.CompensacionRSF.Helper;
using COES.Servicios.Aplicacion.CostoOportunidad;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
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
    public class DespachoURSController : BaseController
    {
        // GET: /CompensacionRSF/DespachoURS/

        public DespachoURSController()
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
        CostoOportunidadAppServicio servicioCostOport = new CostoOportunidadAppServicio();
        BarraUrsAppServicio servicioURS = new BarraUrsAppServicio();

        public ActionResult Index(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            DespachoURSModel model = new DespachoURSModel();
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
                Log.Info("EntidadRecalculo - GetByIdVcrRecalculoView"); 
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

        /// <summary>
        /// Permite listar la información relacionada a la Reserva Asignada
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista(int vcrecacodi = 0)
        {
            DespachoURSModel model = new DespachoURSModel();
            model.ListaDespacho = this.servicioCompensacionRsf.ListVcrDespachourss(vcrecacodi);
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, User.Identity.Name);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Permite exportar a un archivo excel la lista de la Reserva Asignada
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDURS(int pericodi = 0)
        {
            base.ValidarSesionUsuario();
            string result = "-1";
            try
            {
                DespachoURSModel model = new DespachoURSModel();
                if (pericodi > 0)
                {
                    Log.Info("Entidad Periodo - GetByIdPeriodo");
                    model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                    string sMes = model.EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;

                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    var sFechaFin = System.DateTime.DaysInMonth(model.EntidadPeriodo.AnioCodi, model.EntidadPeriodo.MesCodi) + "/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    DateTime fecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fecFin = DateTime.ParseExact(sFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    string pathFile = ConfigurationManager.AppSettings[ConstantesCompensacionRSF.ReporteDirectorio].ToString();
                    string nombreArchivo = "RptCostoOportunidad.xls";
                    Log.Info("Exportar información - GenerarArchivoExcelDespacho");
                    this.servicioCostOport.GenerarArchivoExcelDespacho(fecInicio, pathFile + nombreArchivo);

                    result = "1";
                }
            }
            catch (Exception e)
            {
                string sMensaje = e.Message;
                result = "-1";
            }
            return Json(result);
        }

        /// <summary>
        /// Abrir el archivode de la Reserva Asignada
        /// </summary>
        public virtual ActionResult AbrirArchivo()
        {
            string nombreArchivo = "RptCostoOportunidad.xls";
            string fullPath = ConfigurationManager.AppSettings[ConstantesCompensacionRSF.ReporteDirectorio].ToString() + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        /// <summary>
        /// Permite copiar la información de la base de datos de los Despachos de las URS
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarDURS(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            DespachoURSModel model = new DespachoURSModel();
            model.sError = "";
            model.iNumReg = 0;
            try
            {
                if (pericodi > 0 && vcrecacodi > 0)
                {
                    //Eliminando Todo el proceso de calculo
                    //IMPLEMENTAR PROCEDIMIENTO

                    //Eliminando la información del periodo
                    Log.Info("Eliminando la información - DeleteVcrDespachours");
                    this.servicioCompensacionRsf.DeleteVcrDespachours(vcrecacodi);

                    Log.Info("Entidad Periodo - GetByIdPeriodo");
                    model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                    string sMes = model.EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;

                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    var sFechaFin = System.DateTime.DaysInMonth(model.EntidadPeriodo.AnioCodi, model.EntidadPeriodo.MesCodi) + "/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    DateTime fecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fecFin = DateTime.ParseExact(sFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                    //Se esta copiando tal cual todo el procedimiento del archivo COES.Servicios.Aplicacion.CostoOportunidad.CostoOportunidadAppServicio:
                    //GenerarArchivoExcelDespacho
                    
                    //Para cada fecha, probando fecInicio, luego hacer un bucle hasta fecFin
                    DateTime fecha = fecInicio;
                    while (fecha <= fecFin)
                    {
                        Log.Info("listaReservEjec - GetReservaEjec");
                        var listaReservEjec = this.servicioCostOport.GetReservaEjec(fecha);
                        Log.Info("listaDespachoSin - GetReservaProgramado");
                        var listaDespachoSin = this.servicioCostOport.GetReservaProgramado(fecha, ConstantesCostoOportunidad.LectcodiDespachoSinReserva); // Sin Reserva
                        Log.Info("listaDespacho - GetReservaProgramado");
                        var listaDespacho = this.servicioCostOport.GetReservaProgramado(fecha, ConstantesCostoOportunidad.LectcodiDespachoConReserva); // Con Reserva
                        Log.Info("listaReservProg - GetReservaProgramado");
                        var listaReservProg = this.servicioCostOport.GetReservaProgramado(fecha, ConstantesCostoOportunidad.LectcodiReservaProgramada);
                        List<MeMedicion48DTO> listaReservaEjec = new List<MeMedicion48DTO>();
                        Log.Info("listaCruceReserva - GetListaCruce");
                        var listaCruceReserva = this.servicioCostOport.GetListaCruce(listaReservEjec, listaReservProg, listaReservaEjec, fecha);
                        //Sin Reserva
                        Log.Info("Sin Reserva - GrabarDespacho");
                        this.servicioCompensacionRsf.GrabarDespacho(vcrecacodi, User.Identity.Name, listaReservEjec, listaDespacho, listaDespachoSin, listaReservProg, listaCruceReserva, fecha, 0);
                        // Con Reserva
                        Log.Info("Con Reserva - GrabarDespacho");
                        this.servicioCompensacionRsf.GrabarDespacho(vcrecacodi, User.Identity.Name, listaReservEjec, listaDespacho, listaDespachoSin, listaReservProg, listaCruceReserva, fecha, 1);
                        fecha = fecha.AddDays(1);
                        model.iNumReg++;
                    }
                }
                else
                    model.sError = "Debe seleccionar un periodo y versión correcto";
            }
            catch (Exception e)
            {
                model.sError = e.Message;
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult ProcesarArchivoDU(string sarchivo, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            DespachoURSModel model = new DespachoURSModel();
            model.sMensaje = "";
            model.sError = "";
            string path = ConfigurationManager.AppSettings[ConstantesCompensacionRSF.ReporteDirectorio].ToString();

            try
            {
                this.servicioCompensacionRsf.EliminarCalculo(vcrecacodi);
                //Elimina información de la tabla VCR_DESPACHOURS
                this.servicioCompensacionRsf.DeleteVcrDespachours(vcrecacodi);

                VcrRecalculoDTO entidadRecalculo = this.servicioCompensacionRsf.GetByIdVcrRecalculo(vcrecacodi);

                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(entidadRecalculo.Pericodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime dFecFin = dFecInicio.AddMonths(1);

                //Listado de fechas a grabar
                var dates = new List<DateTime>();
                int iNroFechas = 0;
                for (var dt = dFecInicio; dt < dFecFin; dt = dt.AddMinutes(30))
                {
                    dates.Add(dt);
                    iNroFechas++;
                }

                //Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();
                ds = this.servicioCompensacionRsf.GeneraDataset(path + sarchivo, 1);

                //Lista de URS
                string[] listaURS = new string[(ds.Tables[0].Columns.Count - 2)]; //Le quitamos las 2 primeras columnas
                int[] listaURSGrupoCodi = new int[(ds.Tables[0].Columns.Count - 2)];
                string[] listaCentral = new string[(ds.Tables[0].Columns.Count - 2)]; 
                
                int iNroCabeceras = 0;
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 4)
                    {
                        continue;
                    }
                    //iFila = 4 - Empieza con la cabecera URS.
                    if (iFila == 4)
                    {
                        iNroCabeceras = 0;
                        for (int i = 0; i < dtRow.ItemArray.Count(); i++)
                        {
                            if (dtRow[i].ToString() != "null" && dtRow[i].ToString().ToUpper() != "HORA" && dtRow[i].ToString() != "")
                            {
                                listaURS[iNroCabeceras] = dtRow[i].ToString();
                                TrnBarraursDTO dtoBarraURS = this.servicioURS.GetByNombrePrGrupo(listaURS[iNroCabeceras].ToString());
                                if (dtoBarraURS != null)
                                    listaURSGrupoCodi[iNroCabeceras] = dtoBarraURS.GrupoCodi;
                                iNroCabeceras++;
                            }
                        }
                        continue;
                    }
                    //iFila = 5 - Empieza con la cabecera Central.
                    if (iFila == 5)
                    {
                        iNroCabeceras = 0;
                        for (int i = 0; i < dtRow.ItemArray.Count(); i++)
                        {
                            if (dtRow[i].ToString() != "null" && dtRow[i].ToString() != "")
                            {
                                listaCentral[iNroCabeceras] = dtRow[i].ToString();
                                iNroCabeceras++;
                            }
                        }
                    }
                    break;
                }
                //Lee todo el contenido del excel y le descontamos 6 filas hasta donde empieza la data
                iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 6)
                    {
                        continue;
                    }
                    if (iFila > iNroFechas + 5)
                    {
                        break;
                    }
                    //A partir de aqui esta la data
                    DateTime dVcmpopfecha = dates[iFila - 6];
                    int iColumna = 2; // Donde empieza la dat
                    for (int i = 0; i < iNroCabeceras; i++)
                    {
                        //INSERTAR EL REGISTRO
                        model.EntidadDespacho = new VcrDespachoursDTO();
                        TrnBarraursDTO dtoBarraTrnURS = this.servicioURS.GetByIdGrupoCodiTRN(listaURSGrupoCodi[i]);
                        if (dtoBarraTrnURS != null)
                        {
                            model.EntidadDespacho.Vcdurscodi = 0;
                            model.EntidadDespacho.Vcrecacodi = vcrecacodi;
                            if (i < iNroCabeceras / 2)
                            {
                                model.EntidadDespacho.Vcdurstipo = "S";
                            }                            
                            else
                            {
                                model.EntidadDespacho.Vcdurstipo = "C";
                            }
                            //if (i == listaURS.Count() / 2)
                            //{
                            //    iColumna += 4;
                            //}
                            //Insertar registro
                            model.EntidadDespacho.Grupocodi = dtoBarraTrnURS.GrupoCodi;
                            model.EntidadDespacho.Equicodi = dtoBarraTrnURS.EquiCodi;
                            model.EntidadDespacho.Emprcodi = dtoBarraTrnURS.EmprCodi;
                            model.EntidadDespacho.Vcdursusucreacion = User.Identity.Name;
                            model.EntidadDespacho.Vcdursfeccreacion = DateTime.Now;
                            model.EntidadDespacho.Vcdursfecha = dVcmpopfecha;
                            model.EntidadDespacho.Gruponomb = listaURS[i].ToString();
                            model.EntidadDespacho.Vcdursdespacho = UtilCompensacionRSF.ValidarNumero(dtRow[iColumna].ToString());
                            this.servicioCompensacionRsf.SaveVcrDespachours(model.EntidadDespacho);
                            iColumna++;
                        }
                    }
                }
                model.sMensaje = "Felicidades, la carga de información fue exitosa para los " + (iFila - 5) + " registros, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }
        }

        /// <summary>
        /// Permite cargar un archivo Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            base.ValidarSesionUsuario();
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesCompensacionRSF.ReporteDirectorio].ToString();

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
