using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.CompensacionRSF.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CompensacionRSF;
using COES.Servicios.Aplicacion.CompensacionRSF.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
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
    public class MedicionBornesController : BaseController
    {
        // GET: /CompensacionRSF/MedicionBornes/

        public MedicionBornesController()
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
        ConsultaMedidoresAppServicio servicioMedidores = new ConsultaMedidoresAppServicio();
        CentralGeneracionAppServicio servicioCentral = new CentralGeneracionAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();

        public ActionResult Index(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            MedicionBornesModel model = new MedicionBornesModel();
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
        /// Muestra la lista de Unidades considerados en el cargo del incumplimiento
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <param name="vcrecacodi">Versión de cálculo</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista(int pericodi = 0, int vcrecacodi = 0)
        {
            MedicionBornesModel model = new MedicionBornesModel();
            //Lista todas la lista de la tabla VCR_MEDBORNECARGOINCP incluido los Nombres de Empresa, Central y Unidad
            Log.Info("ListaMedBorneCargoIncp - ListVcrMedbornecargoincps");
            model.ListaMedBorneCargoIncp = this.servicioCompensacionRsf.ListVcrMedbornecargoincps(vcrecacodi); 
            return PartialView(model);
        }

        /// <summary>
        /// Permite exportar a un archivo excel la lista de Medidores de Bornes de Generación
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarMB(int pericodi = 0)
        {
            base.ValidarSesionUsuario();
            string result = "-1";
            try
            {
                ReservaAsignadaModel model = new ReservaAsignadaModel();
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

                    string empresas = Constantes.ParametroDefecto;
                    string tiposGeneracion = Constantes.ParametroDefecto;
                    string tiposEmpresa = "1,2,3,4,5";
                    int central = 1;
                    string parametros = "1";
                    int tipo = 1;

                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    string file = NombreArchivo.ReporteMedidoresHorizontal;
                    bool flag = (User.Identity.Name == Constantes.UsuarioAnonimo) ? false : true;
                    Log.Info("Exportar información - GenerarArchivoExportacion");
                    this.servicioMedidores.GenerarArchivoExportacion(fecInicio, fecFin, tiposEmpresa, empresas, tiposGeneracion, central, parametros, path, file, tipo, flag);
                    result = file;
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
        /// Abrir el archivode de Medidores de Bornes de Generación
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            return File(path, app, sFecha + "_" + file);
        }

        /// <summary>
        /// Permite copiar la información de la base de datos de los Medidores de Bornes de Generación
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <param name="vcrecacodi">Versión de cálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarMB(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            MedicionBornesModel model = new MedicionBornesModel();
            model.sError = "";
            model.iNumReg = 0;
            try
            {
                if (pericodi > 0 && vcrecacodi > 0)
                {
                    //Eliminando Todo el proceso de calculo
                    //IMPLEMENTAR PROCEDIMIENTO

                    //Eliminando la información del periodo
                    Log.Info("Eliminar registro - DeleteVcrUnidadexonerada");
                    this.servicioCompensacionRsf.DeleteVcrUnidadexonerada(vcrecacodi);
                    Log.Info("Eliminar registro - DeleteVcrMedbornecargoincp");
                    this.servicioCompensacionRsf.DeleteVcrMedbornecargoincp(vcrecacodi);
                    Log.Info("Eliminar registro - DeleteVcrMedborne");
                    this.servicioCompensacionRsf.DeleteVcrMedborne(vcrecacodi);

                    Log.Info("Entidad Periodo - GetByIdPeriodo");
                    model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                    string sMes = model.EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;

                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    var sFechaFin = System.DateTime.DaysInMonth(model.EntidadPeriodo.AnioCodi, model.EntidadPeriodo.MesCodi) + "/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    DateTime fecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fecFin = DateTime.ParseExact(sFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    //Obtenemos la informacion de los Medidores de Bornes de Generación
                    string empresas = Constantes.ParametroDefecto;
                    string tiposGeneracion = Constantes.ParametroDefecto;
                    int central = 1;
                    Log.Info("Insertar registro - GrabarMedidorBorne");
                    model.iNumReg = this.servicioCompensacionRsf.GrabarMedidorBorne(vcrecacodi, User.Identity.Name, 1, empresas, central, tiposGeneracion, fecInicio, fecFin);
                    //Grabamos las Unidades considerados en el cargo del incumplimiento
                    this.servicioCompensacionRsf.GrabarUnidadesCargoIncumplimiento(vcrecacodi, User.Identity.Name);
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
        public JsonResult ProcesarArchivoMB(string sarchivo, int pericodi, int vcrecacodi)
        {
            base.ValidarSesionUsuario();
            List<VcrMedborneDTO> ListaResultadoGrabar = new List<VcrMedborneDTO>();
            ReservaAsignadaModel model = new ReservaAsignadaModel();
            model.sMensaje = "";
            model.sError = "";
            string path = ConfigurationManager.AppSettings[ConstantesCompensacionRSF.ReporteDirectorio].ToString();

            try
            {
                //Eliminando la información del periodo
                Log.Info("Eliminar registro - DeleteVcrUnidadexonerada");
                this.servicioCompensacionRsf.DeleteVcrUnidadexonerada(vcrecacodi);
                Log.Info("Eliminar registro - DeleteVcrMedbornecargoincp");
                this.servicioCompensacionRsf.DeleteVcrMedbornecargoincp(vcrecacodi);
                Log.Info("Eliminar registro - DeleteVcrMedborne");
                this.servicioCompensacionRsf.DeleteVcrMedborne(vcrecacodi);

                //Valores del periodo
                Log.Info("Entidad Periodo - GetByIdPeriodo");
                model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                string sMes = model.EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;

                var sFechaInicio = "01/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime dFecFin = dFecInicio.AddMonths(1);

                //#region Plantilla exportada a la fecha
                //string empresas = Constantes.ParametroDefecto;
                //string tiposGeneracion = Constantes.ParametroDefecto;
                //string tiposEmpresa = "1,2,3,4,5";

                //List<MeMedicion96DTO> listActiva = this.servicioCompensacionRsf.listActivaMB(dFecInicio, dFecFin, empresas, tiposGeneracion, tiposEmpresa);
                //#endregion

                //Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();
                ds = this.servicioCompensacionRsf.GeneraDataset(path + sarchivo, 1);

                int iFila = 0;
                int iAux = 0;
                decimal dNumero;
                //Lee todo el contenido del excel y le descontamos 6 filas hasta donde empieza la data
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    if (iAux < 9)
                    {
                        iAux++; 
                        continue;
                    }
                    //VALIDAR SI LA COLUMNA YA ESTA AL FINAL
                    if (!Decimal.TryParse(dtRow[1].ToString(), out dNumero))
                    {
                        //fin de archivo
                        //break;
                    }
                    //IDENTIFICAMOS EMPRESA, CENTRAL, UNIDAD Y PUNTO DE MEDICIÓN EN LA LISTA ACTIVA
                    VcrMedborneDTO dtoMedborne = new VcrMedborneDTO();
                    dtoMedborne.Vcrecacodi = vcrecacodi;
                    DateTime dFecha;
                    try
                    {
                        dFecha = DateTime.ParseExact(dtRow[1].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        break;
                    }
                    //DateTime.FromOADate(double.Parse(dtRow[1].ToString()));
                    dtoMedborne.Vcrmebfecha = dFecha;
                    dtoMedborne.Vcrmebptomed = dtRow[2].ToString(); //Identificador de la tabla PtoMedicion
                    //if (dtoMedborne.Vcrmebptomed == "437")
                    //{
                    //    dtoMedborne.Vcrmebptomed = "437";
                    //}
                    //SI_EMPRESA---------------------------------------------------------------------
                    string sEmprnomb = dtRow[3].ToString();
                    COES.Dominio.DTO.Transferencias.EmpresaDTO dtoEmpresa = new COES.Dominio.DTO.Transferencias.EmpresaDTO();
                    Log.Info("Entidad empresa - GetByNombre");
                    dtoEmpresa = this.servicioEmpresa.GetByNombre(sEmprnomb);
                    if (dtoEmpresa == null)
                    {
                        model.sError += "<br>El siguiente Empresa no existe: " + dtoEmpresa.EmprNombre;
                        continue;
                        //return Json(model);
                    }
                    dtoMedborne.Emprcodi = dtoEmpresa.EmprCodi;
                    int iEnelGreen = 11395;
                    if (dtoMedborne.Emprcodi == iEnelGreen)
                        dtoMedborne.Emprcodi = 13783; //ENEL GREEN POWER PERU S.A. [B] -> ENEL GREEN POWER PERU S.A.C [A] //sNombre = listActiva[iFila].Emprnomb;
                    //---------------------------------------------------------------------
                    //CENTRAL---------------------------------------------------------------------
                    string sCentral = dtRow[4].ToString();
                    string sUnidad = dtRow[5].ToString();
                    if (sCentral.Equals(sUnidad))
                    {
                        EqEquipoDTO dtoCentral = this.servicioCentral.GetByCentGeneNombreEquipoCenUni(sCentral, -1);
                        if (dtoCentral == null)
                        {
                            model.sError = "No existe la Central/Unidad " + sCentral;
                            return Json(model);
                        }
                        dtoMedborne.Equicodicen = dtoCentral.Equicodi;
                        dtoMedborne.Equicodiuni = dtoCentral.Equicodi;
                    }
                    else
                    {
                        Log.Info("EntidadCentral - GetByCentGeneNomb");
                        //CentralGeneracionDTO dtoCentral = this.servicioEquipo.GetByCentGeneNomb(Central);
                        EqEquipoDTO dtoCentral = this.servicioCentral.GetByCentGeneNombreEquipo(sCentral, -1);
                        if (dtoCentral == null)
                        {
                            model.sError = "No existe la Central " + sCentral;
                            return Json(model);
                        }
                        dtoMedborne.Equicodicen = dtoCentral.Equicodi;
                        if (dtoMedborne.Equicodicen == -1)
                        {
                            continue;
                        }
                        Log.Info("EntidadUnidad - GetByCentGeneNomb");
                        EqEquipoDTO dtoUnidad = this.servicioCentral.GetByCentGeneUniNombreEquipo(sUnidad, (int)dtoCentral.Equicodi, -1);
                        if (dtoUnidad == null)
                        {
                            model.sError = "No existe la Unidad " + sUnidad + " de la Central " + sCentral;
                            return Json(model);
                        }
                        dtoMedborne.Equicodiuni = dtoUnidad.Equicodi;
                    }

                    decimal dVcrMebPotenciaMededia = 0;
                    for (int k = 7; k <= 102; k++)
                    {
                        //Excel -> H9:7  CV9:102
                        string resultado = dtRow[k].ToString().Trim();
                        if (resultado != null)
                        {
                            dVcrMebPotenciaMededia += decimal.Parse(resultado, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint); // Convert.ToDecimal(resultado);
                        }
                    }
                    dtoMedborne.Vcrmebpotenciamed = dVcrMebPotenciaMededia / 4; //  (sum(96) Interv de Potencia / 4) = TOTAL ENERGIA ACTIVA  (expresado en MWh)
                                                                                //202012
                    dtoMedborne.Vcrmebpotenciamedgrp = dVcrMebPotenciaMededia / 96;  // (sum(96) Interv de Potencia / 96) = POTENCIA MEDIA DE GRUPO  (expresado en potencia)
                    dtoMedborne.Vcrmebpresencia = 0;
                    if (dtoMedborne.Vcrmebpotenciamedgrp > 0)
                    {
                        dtoMedborne.Vcrmebpresencia = 1;
                    }
                    
                    dtoMedborne.Vcrmebusucreacion = User.Identity.Name;
                    ListaResultadoGrabar.Add(dtoMedborne);
                    //this.servicioCompensacionRsf.SaveVcrMedborne(dtoMedborne);

                    iFila++;
                }
                this.servicioCompensacionRsf.SaveVcrMedborneBulk(ListaResultadoGrabar);
                //Complementamos la grabación
                this.servicioCompensacionRsf.GrabarGrandesUsuarios(vcrecacodi, dFecInicio, User.Identity.Name);

                //Grabamos las Unidades considerados en el cargo del incumplimiento
                this.servicioCompensacionRsf.GrabarUnidadesCargoIncumplimiento(vcrecacodi, User.Identity.Name);
                model.sMensaje = "Felicidades, la carga de información fue exitosa para los " + (iFila) + " registros, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
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

        /// <summary>
        /// Permite actualizar la unidades que son considerados en el cargo del incumplimiento
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <param name="vcrecacodi">Versión de cálculo</param>
        /// <param name="items">Lista de Ids</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarListaCargo(int pericodi = 0, int vcrecacodi = 0, string items = "")
        {
            base.ValidarSesionUsuario();
            MedicionBornesModel model = new MedicionBornesModel();
            model.sError = "";
            model.iNumReg = 0;
            try
            {
                if (pericodi > 0 && vcrecacodi > 0)
                {
                    //Actualizamos la información, colocando en NO a todas las unidades
                    Log.Info("Actualizar registro - UpdateVcrMedbornecargoincpVersionNO");
                    this.servicioCompensacionRsf.UpdateVcrMedbornecargoincpVersionNO(vcrecacodi, User.Identity.Name);

                    //Actualizamos la información, colocando en SI, solo a la lista de items seleccionados
                    string[] Ids = items.Split(ConstantesAppServicio.CaracterComa);
                    foreach (string Id in Ids)
                    {
                        int iVcmbcicodi = Convert.ToInt32(Id);
                        if (iVcmbcicodi > 0)
                        {
                            Log.Info("Actualizar registro - UpdateVcrMedbornecargoincpVersionSI");
                            this.servicioCompensacionRsf.UpdateVcrMedbornecargoincpVersionSI(vcrecacodi, User.Identity.Name, iVcmbcicodi);
                            model.iNumReg++;
                        }
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

    }
}
