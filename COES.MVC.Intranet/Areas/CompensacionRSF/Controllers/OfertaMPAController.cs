using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.CompensacionRSF.Models;
using COES.MVC.Intranet.Areas.Subastas.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CompensacionRSF;
using COES.Servicios.Aplicacion.CompensacionRSF.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Subastas;
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
    public class OfertaMPAController : BaseController
    {
        // GET: /CompensacionRSF/OfertaMPA/

        public OfertaMPAController()
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
        SubastasAppServicio servicioSubasta = new SubastasAppServicio();
        BarraUrsAppServicio servicioPrGrupo = new BarraUrsAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        BarraUrsAppServicio servicioBarraUrs = new BarraUrsAppServicio();

        public ActionResult Index(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            OfertaMPAModel model = new OfertaMPAModel();
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            {
                pericodi = model.ListaPeriodos[0].PeriCodi;
            }
            Log.Info("Entidad Periodo - GetByIdPeriodo");
            model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
            string sMes = model.EntidadPeriodo.MesCodi.ToString();
            if (sMes.Length == 1) sMes = "0" + sMes;
            model.sFechaEnvioIni = "01/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
            model.sFechaEnvioFin = System.DateTime.DaysInMonth(model.EntidadPeriodo.AnioCodi, model.EntidadPeriodo.MesCodi) + "/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;

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
        /// Permite copiar la información de la base de datos de la Oferta Diaria
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarOD(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            ReservaAsignadaModel model = new ReservaAsignadaModel();
            model.sError = "";
            model.iNumReg = 0;
            try
            {
                if (pericodi > 0 && vcrecacodi > 0)
                {
                    //Eliminando Todo el proceso de calculo
                    Log.Info("Executar proceso - EliminarCalculo");
                    string sBorrar = this.servicioCompensacionRsf.EliminarCalculo(vcrecacodi);
                    if (!sBorrar.Equals("1"))
                    {
                        model.sError = "Lo sentimos, No se pudo eliminar el proceso de cálculo: " + sBorrar;
                        return Json(model);
                    }

                    //Eliminando la información del periodo
                    Log.Info("Eliminar registro - DeleteVcrOferta");
                    this.servicioCompensacionRsf.DeleteVcrOferta(vcrecacodi);

                    Log.Info("Entidad Periodo - GetByIdPeriodo");
                    model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                    string sMes = model.EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;

                    #region PRIMER PASO - Importar data de SGOCOES
                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    var sFechaFin = System.DateTime.DaysInMonth(model.EntidadPeriodo.AnioCodi, model.EntidadPeriodo.MesCodi) + "/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    DateTime fecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fecFin = DateTime.ParseExact(sFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                    //Obtenemos la informacion de la Oferta Diaria
                    int empresaCodi = -1; //Defecto
                    int userCode = -1;
                    int tipoOferta = 1;
                    string oferCodi = "-1";
                    int opcionReporte = 1;
                    string listUrs = "-1";
                    List<TrnBarraursDTO> ListaBarraURS = this.servicioBarraUrs.ListPrGrupo();
                    foreach (var urs in ListaBarraURS)
                    {
                        listUrs = listUrs + ", " + urs.GrupoCodi.ToString();
                    }
                    //Log.Info("ListaOferta - ListSmaOfertasInterna");
                    //List<SmaOfertaDTO> lsOferta = this.servicioSubasta.ListSmaOfertasInterna(1, fecInicio, fecFin, -1, -1, ConstantesValores.estadoActivo, -1, "-1");
                    Log.Info("ListaOferta - ListaConsultaOferta");
                    List<SmaOfertaDTO> listaOferta = this.servicioSubasta.ListaConsultaOferta(opcionReporte, tipoOferta, fecInicio, fecFin, userCode, oferCodi, empresaCodi, listUrs, ConstantesSubasta.FuenteExtranet);
                    //List<SmaOfertaDTO> lsOfertaA = lsOferta.Where(x => x.Oferestado == "A").ToList();
                    foreach (SmaOfertaDTO dtoSmaOferta in listaOferta)
                    {
                        VcrOfertaDTO dtoOferta = new VcrOfertaDTO();
                        dtoOferta.Vcrecacodi = vcrecacodi;
                        //SI_EMPRESA
                        COES.Dominio.DTO.Transferencias.EmpresaDTO dtoEmpresa = new COES.Dominio.DTO.Transferencias.EmpresaDTO();
                        Log.Info("Entidad empresa - GetByNombre");
                        dtoEmpresa = this.servicioEmpresa.GetByNombre(dtoSmaOferta.Emprnomb);
                        if (dtoEmpresa == null)
                        {
                            model.sError += "<br>El siguiente Empresa no existe: " + dtoEmpresa.EmprNombre;
                            continue;
                            //return Json(model);
                        }
                        dtoOferta.Emprcodi = dtoEmpresa.EmprCodi;
                        //PR_GRUPO
                        Log.Info("Entidad Barraurs - GetByNombrePrGrupo");
                        TrnBarraursDTO dtoBarraurs = this.servicioPrGrupo.GetByNombrePrGrupo(dtoSmaOferta.Ursnomb.Trim());
                        if (dtoBarraurs == null)
                        {
                            model.sError += "<br>La siguiente URS no existe: " + dtoSmaOferta.Ursnomb;
                            continue;
                            //return Json(model);
                        }
                        dtoOferta.Grupocodi = dtoBarraurs.GrupoCodi;
                        dtoOferta.Gruponomb = dtoBarraurs.GrupoNomb;
                        //FW_USER
                        Log.Info("Entidad User - GetByFwUserByNombre");
                        VcrOfertaDTO dtoUser = this.servicioCompensacionRsf.GetByFwUserByNombre(dtoSmaOferta.Username);
                        dtoOferta.Usercode = -1;
                        if (dtoUser != null)
                            dtoOferta.Usercode = dtoUser.Usercode;

                        dtoOferta.Vcrofecodigoenv = dtoSmaOferta.Ofercodenvio;
                        dtoOferta.Vcrofefecha = ((DateTime)dtoSmaOferta.Oferfechainicio).Date;
                        
                        var horaminutoInicio = dtoSmaOferta.Ofdehorainicio.Split(':');
                        DateTime dfechabaseInicio = new DateTime(dtoOferta.Vcrofefecha.Value.Year, dtoOferta.Vcrofefecha.Value.Month, dtoOferta.Vcrofefecha.Value.Day, Convert.ToInt32(horaminutoInicio[0]), Convert.ToInt32(horaminutoInicio[1]), 0);
                        dtoOferta.Vcrofehorinicio = dfechabaseInicio;

                        var horaminutoFin = dtoSmaOferta.Ofdehorafin.Split(':');
                        DateTime dfechabaseFin = new DateTime(dtoOferta.Vcrofefecha.Value.Year, dtoOferta.Vcrofefecha.Value.Month, dtoOferta.Vcrofefecha.Value.Day, Convert.ToInt32(horaminutoFin[0]), Convert.ToInt32(horaminutoFin[1]), 0);
                        dtoOferta.Vcrofehorfinal = dfechabaseFin;
                        
                        dtoOferta.Vcrofemodoperacion = dtoSmaOferta.OferlistMODes;
                        dtoOferta.Vcrofepotofertada = dtoSmaOferta.Repopotofer;
                        dtoOferta.Vcrofeprecio = Convert.ToDecimal(dtoSmaOferta.Repoprecio);
                        dtoOferta.Vcrofeusucreacion = User.Identity.Name;
                        dtoOferta.Vcrofetipocarga = Convert.ToInt32(dtoSmaOferta.Ofdetipo);
                        Log.Info("Insertar registro - SaveVcrOferta");
                        this.servicioCompensacionRsf.SaveVcrOferta(dtoOferta);
                        //model.iNumReg++;
                    }
                    #endregion
                    //ASSETEC 20190115
                    //Vamos a quedarnos con la ultima oferta del día para una URS
                    List<VcrOfertaDTO> ListaOfeFinal = new List<VcrOfertaDTO>();
                    #region SEGUNDO PASO - Limpiar de duplicados y Solo una oferta por URS en el día
                    List<VcrOfertaDTO> ListaOfertas = this.servicioCompensacionRsf.ListVcrOfertasSinDuplicados(vcrecacodi);
                    //La lista ya esta sin duplicados.
                    int iGrupoCodi = 0;
                    DateTime dVcrOfeFecha = DateTime.MinValue;
                    string sVcrOfeCodigoEnv = "";
                    foreach (VcrOfertaDTO dtoOferta in ListaOfertas)
                    {
                        bool bInsertar = false;
                        if (dtoOferta.Grupocodi != iGrupoCodi)
                        {   // && dtoOferta.Vcrofefecha != dVcrOfeFecha && dtoOferta.Vcrofecodigoenv != sVcrOfeCodigoEnv
                            //Nueva oferta
                            iGrupoCodi = dtoOferta.Grupocodi;
                            dVcrOfeFecha = (DateTime)dtoOferta.Vcrofefecha;
                            sVcrOfeCodigoEnv = dtoOferta.Vcrofecodigoenv;
                            bInsertar = true;
                        }
                        else if (dtoOferta.Vcrofefecha != dVcrOfeFecha)
                        {   // Entonces: dtoOferta.Grupocodi == iGrupoCodi //&& dtoOferta.Vcrofecodigoenv != sVcrOfeCodigoEnv
                            //Nueva oferta
                            dVcrOfeFecha = (DateTime)dtoOferta.Vcrofefecha;
                            sVcrOfeCodigoEnv = dtoOferta.Vcrofecodigoenv;
                            bInsertar = true;
                        }
                        else if (dtoOferta.Vcrofecodigoenv == sVcrOfeCodigoEnv)
                        {   //Entonces: dtoOferta.Grupocodi == iGrupoCodi && dtoOferta.Vcrofefecha == dVcrOfeFecha && 
                            //Nueva oferta, en otro horario de inicio
                            bInsertar = true;
                            
                        }
                        if (bInsertar)
                        {
                            VcrOfertaDTO dtoOfeFinal = this.servicioCompensacionRsf.GetByCriteriaVcrOferta(vcrecacodi, iGrupoCodi, dVcrOfeFecha, sVcrOfeCodigoEnv, (DateTime)dtoOferta.Vcrofehorinicio, dtoOferta.Vcrofetipocarga);
                            ListaOfeFinal.Add(dtoOfeFinal);
                        }
                    }
                    #endregion
                    #region TERCER PASO - Insertamos las ofertas finales
                    //Limpiamos la tabla para el recalculo para almacenar las ofertas finales
                    Log.Info("Eliminar registro - DeleteVcrOferta"); 
                    this.servicioCompensacionRsf.DeleteVcrOferta(vcrecacodi);
                    //Insertamos las ofertas finales
                    foreach(VcrOfertaDTO dto in ListaOfeFinal)
                    {
                        this.servicioCompensacionRsf.SaveVcrOferta(dto);
                        model.iNumReg++;
                    }
                    #endregion
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
        public JsonResult ExportarReporte(int empresacodi, int tipooferta, string oferfechaenvio, string oferfechaenviofin, int usercode, int opcion)
        {
            OfertaModel modelResult = new OfertaModel();

            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                int empresaCodi = empresacodi;
                int userCode = usercode;
                int tipoOferta = tipooferta;
                string fechaString = oferfechaenvio;
                string fechafinString = oferfechaenviofin;
                int oferCodi = -1;
                int opcionReporte = opcion;
                string listUrs = "-1";
                List<TrnBarraursDTO> ListaBarraURS = this.servicioBarraUrs.ListPrGrupo();
                foreach (var urs in ListaBarraURS)
                {
                    listUrs = listUrs + ", " + urs.GrupoCodi.ToString();
                }

                DateTime? fecha = (fechaString == null) ? (DateTime?)null : DateTime.ParseExact(fechaString, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime? fechafin = (fechafinString == null) ? (DateTime?)null : DateTime.ParseExact(fechafinString, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nombreArchivo = NombreArchivo.ReporteOfertasDiarias;

                this.servicioSubasta.GenerarArchivoExcelConsultaOferta(ruta, nombreArchivo, opcionReporte, tipoOferta, fecha.Value, fechafin.Value, userCode, oferCodi, empresaCodi, listUrs);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                modelResult.Resultado = -1;
                modelResult.Mensaje = ex.Message;
                modelResult.Detalle = ex.StackTrace;
            }

            return Json(modelResult);
        }


        [HttpPost]
        public JsonResult ProcesarArchivoOD(string sarchivo, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            ReservaAsignadaModel model = new ReservaAsignadaModel();
            model.sMensaje = "";
            model.sError = "";
            string path = ConfigurationManager.AppSettings[ConstantesCompensacionRSF.ReporteDirectorio].ToString();

            try
            {
                //Eliminando Todo el proceso de calculo
                Log.Info("Executar proceso - EliminarCalculo");
                string sBorrar = this.servicioCompensacionRsf.EliminarCalculo(vcrecacodi);
                if (!sBorrar.Equals("1"))
                {
                    model.sError = "Lo sentimos, No se pudo eliminar el proceso de cálculo: " + sBorrar;
                    return Json(model);
                }

                //Eliminando la información del periodo
                Log.Info("Eliminar registro - DeleteVcrOferta");
                this.servicioCompensacionRsf.DeleteVcrOferta(vcrecacodi);
                int[] aTipoCarga = { 0, ConstantesCompensacionRSF.TipoCargaSubir, ConstantesCompensacionRSF.TipoCargaBajar };
                int[] aNroTipoCarga = { 0, 0, 0 };

                VcrRecalculoDTO entidadRecalculo = this.servicioCompensacionRsf.GetByIdVcrRecalculo(vcrecacodi);
                //Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();

                for (int iHoja = 1; iHoja <= 2; iHoja++)
                {
                    ds = this.servicioCompensacionRsf.GeneraDataset(path + sarchivo, iHoja);

                    int iFila = 0;
                    int iAux = 0;
                    decimal dNumero;
                    //Lee todo el contenido del excel y le descontamos 6 filas hasta donde empieza la data
                    foreach (DataRow dtRow in ds.Tables[0].Rows)
                    {
                        if (iAux < 5)
                        {
                            iAux++;
                            continue;
                        }
                        //VALIDAR SI LA COLUMNA YA ESTA AL FINAL
                        if (!Decimal.TryParse(dtRow[5].ToString(), out dNumero))
                        {
                            //fin de archivo
                            break;
                        }
                        iFila++;
                        //Leemos una fila
                        VcrOfertaDTO dtoOferta = new VcrOfertaDTO();
                        dtoOferta.Vcrecacodi = vcrecacodi;
                        //SI_EMPRESA
                        dtoOferta.Emprcodi = -1;
                        //PR_GRUPO
                        string sUrsnomb = dtRow[2].ToString();
                        Log.Info("Entidad Barraurs - GetByNombrePrGrupo");
                        TrnBarraursDTO dtoBarraurs = this.servicioPrGrupo.GetByNombrePrGrupo(sUrsnomb);
                        if (dtoBarraurs == null)
                        {
                            model.sError += "<br>La siguiente URS no existe: " + sUrsnomb;
                            continue;
                            //return Json(model);
                        }
                        dtoOferta.Grupocodi = dtoBarraurs.GrupoCodi;
                        dtoOferta.Gruponomb = dtoBarraurs.GrupoNomb;
                        //FW_USER
                        dtoOferta.Usercode = -1;
                        dtoOferta.Vcrofecodigoenv = "-";
                        string sVcrofefecha = dtRow[1].ToString().Trim();
                        DateTime dVcrofefecha = DateTime.ParseExact(sVcrofefecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        dtoOferta.Vcrofefecha = dVcrofefecha;

                        var horaminutoInicio = sVcrofefecha + " " + dtRow[3].ToString().Trim();
                        DateTime dfechabaseInicio = DateTime.ParseExact(horaminutoInicio, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                        dtoOferta.Vcrofehorinicio = dfechabaseInicio;

                        var horaminutoFin = sVcrofefecha + " " + dtRow[4].ToString().Trim();
                        DateTime dfechabaseFin = DateTime.ParseExact(horaminutoFin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                        dtoOferta.Vcrofehorfinal = dfechabaseFin;

                        dtoOferta.Vcrofemodoperacion = "";
                        dtoOferta.Vcrofepotofertada = Convert.ToDecimal(dtRow[5].ToString().Trim());
                        dtoOferta.Vcrofeprecio = Convert.ToDecimal(dtRow[6].ToString().Trim());
                        dtoOferta.Vcrofetipocarga = aTipoCarga[iHoja]; //ConstantesCompensacionRSF.TipoCargaSubir / TipoCargaBajar
                        dtoOferta.Vcrofeusucreacion = User.Identity.Name;
                        Log.Info("Insertar registro - SaveVcrOferta");
                        this.servicioCompensacionRsf.SaveVcrOferta(dtoOferta);
                    }
                    aNroTipoCarga[iHoja] = iFila;
                }
                
                model.sMensaje = "Felicidades, la carga de información fue exitosa para " + (aNroTipoCarga[ConstantesCompensacionRSF.TipoCargaSubir]) + " reg.subida y " + (aNroTipoCarga[ConstantesCompensacionRSF.TipoCargaBajar]) + " reg.bajada, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
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
