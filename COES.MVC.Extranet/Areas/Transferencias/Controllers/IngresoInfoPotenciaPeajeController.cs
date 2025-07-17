using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.Transferencias.Models;
using COES.MVC.Extranet.Areas.TransfPotencia.Helper;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using log4net;
using COES.Dominio.DTO.Enum;

namespace COES.MVC.Extranet.Areas.Transferencias.Controllers
{
    public class IngresoInfoPotenciaPeajeController : BaseController
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IngresoInfoPotenciaPeajeController));

        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        CodigoRetiroGeneradoAppServicio servicioCodigoRetiroGenerado = new CodigoRetiroGeneradoAppServicio();
        CodigoConsolidadoAppServicio servicioCodigoConsolidado = new CodigoConsolidadoAppServicio();
        TipoContratoAppServicio servicioTipoContrato = new TipoContratoAppServicio();
        private static string NombreControlador = "IngresoInfoPotenciaPeajeController";
        VariacionEmpresaAppServicio servicioVariacionEmpresa = new VariacionEmpresaAppServicio();
        VariacionCodigoAppServicio servicioVariacionCodigo = new VariacionCodigoAppServicio();
        AuditoriaProcesoAppServicio servicioAuditoria = new AuditoriaProcesoAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        CodigoRetiroRelacionEquivalenciasAppServicio servicioRelacionEquivalencia = new CodigoRetiroRelacionEquivalenciasAppServicio();

        // GET: Transferencias/ValorizacionEnergiaActiva
        public ActionResult Index(int pericodi = 0, int recpotcodi = 0, int pegrcodi = 0)
        {
            base.ValidarSesionUsuario();

            #region Autentificando Empresa

            CodigoRetiroGeneradoModel models = new CodigoRetiroGeneradoModel();
            int iEmprCodi = 0;
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

            List<SeguridadServicio.EmpresaDTO> listTotal = new List<SeguridadServicio.EmpresaDTO>();//UtilTransfPotencia.ObtenerEmpresasPorUsuario(User.Identity.Name);
            List<SeguridadServicio.EmpresaDTO> list = new List<SeguridadServicio.EmpresaDTO>();


            bool accesoEmpresas = true;
            if (accesoEmpresas)
            {
                listTotal = seguridad.ListarEmpresas().OrderBy(x => x.EMPRNOMB).ToList();

            }
            else
            {
                listTotal = this.ListaEmpresas.ToList();
            }


            //- aca debemos hacer jugada para escoger la empresa
            List<TrnInfoadicionalDTO> listaInfoAdicional = (new TransferenciasAppServicio()).ListTrnInfoadicionals();
            List<int> idsEmpresas = listaInfoAdicional.Where(x => x.Emprcodi != null).Select(x => (int)x.Emprcodi).Distinct().ToList();

            foreach (var item in listTotal)
            {
                list.Add(item);
                //ASSETEC 20190111 - comparar contra otros conceptos
                Int16 iEmpcodi = Convert.ToInt16(item.EMPRCODI);
                foreach (TrnInfoadicionalDTO dtoOtroConcepto in listaInfoAdicional)
                {
                    Int16 iOtroConcepto = Convert.ToInt16(dtoOtroConcepto.Emprcodi);
                    if (iEmpcodi == iOtroConcepto)
                    {
                        SeguridadServicio.EmpresaDTO dtoEmpresaConcepto = new SeguridadServicio.EmpresaDTO();
                        dtoEmpresaConcepto.EMPRCODI = Convert.ToInt16(dtoOtroConcepto.Infadicodi);
                        dtoEmpresaConcepto.EMPRNOMB = dtoOtroConcepto.Infadinomb;
                        dtoEmpresaConcepto.TIPOEMPRCODI = Convert.ToInt16(dtoOtroConcepto.Tipoemprcodi);
                        list.Add(dtoEmpresaConcepto);
                    }
                }
                //--------------------------------------------------
            }

            if (accesoEmpresas)
            {
                list.RemoveAll(x => x.EMPRCODI == 67);
            }


            TempData["EMPRNRO"] = list.Count();
            if (list.Count() == 1)
            {
                TempData["EMPRNOMB"] = list[0].EMPRNOMB;
                Session["EmprNomb"] = list[0].EMPRNOMB;
                Session["EmprCodi"] = list[0].EMPRCODI;

                Dominio.DTO.Sic.SiEmpresaDTO empresa = (new COES.Servicios.Aplicacion.General.EmpresaAppServicio()).ObtenerEmpresa(list[0].EMPRCODI);

                if (list[0].EMPRCODI > 0)
                {
                    if (empresa.Emprestado != "A")
                    {
                        TempData["EmprNomb"] = list[0].EMPRNOMB + "  <span style='color:red'>(EN BAJA)</span>";
                    }
                }

                iEmprCodi = Convert.ToInt32(list[0].EMPRCODI);
            }
            else if (Session["EmprCodi"] != null)
            {
                iEmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprCodi);
                TempData["EMPRNOMB"] = dtoEmpresa.EmprNombre;
                Session["EmprNomb"] = dtoEmpresa.EmprNombre;

                Dominio.DTO.Sic.SiEmpresaDTO empresa = (new COES.Servicios.Aplicacion.General.EmpresaAppServicio()).ObtenerEmpresa(iEmprCodi);

                if (iEmprCodi > 0)
                {
                    if (empresa.Emprestado != "A")
                    {
                        TempData["EmprNomb"] = dtoEmpresa.EmprNombre + "  <span style='color:red'>(EN BAJA)</span>";
                    }
                }
            }
            else if (list.Count() > 1)
            {
                TempData["EMPRNOMB"] = "";
                return View();
            }
            else
            {
                //No hay empresa asociada a la cuenta
                TempData["EMPRNOMB"] = "";
                TempData["EMPRNRO"] = -1;
                return View();
            }

            #endregion

            CodigoRetiroGeneradoModel model = new CodigoRetiroGeneradoModel();

            if (pericodi > 0)
            {
                model.ListaPeriodos = new List<PeriodoDTO>();
                model.ListaPeriodos.Add(this.servicioPeriodo.GetByIdPeriodo(pericodi));
                if (model.ListaPeriodos[0] != null)
                {
                    if (model.ListaPeriodos[0].PeriFormNuevo == 0)
                    {
                        return Redirect("~/transfpotencia/peajeegreso/index?pericodi=" + pericodi);
                    }
                }
            }

            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            {
                pericodi = model.ListaPeriodos[0].PeriCodi;
            }
            if (pericodi > 0)
            {
                model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi); //Ordenado en descendente
                if (model.ListaRecalculoPotencia.Count > 0 && recpotcodi == 0)
                { recpotcodi = (int)model.ListaRecalculoPotencia[0].Recpotcodi; }
            }

            if (pericodi > 0 && recpotcodi > 0)
            {
                model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
                if (pegrcodi == 0)
                { model.EntidadPeajeEgreso = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresos(iEmprCodi, pericodi, recpotcodi); }
                else
                { model.EntidadPeajeEgreso = this.servicioTransfPotencia.GetByIdVtpPeajeEgreso(pegrcodi); }
                if (model.EntidadPeajeEgreso == null)
                { model.EntidadPeajeEgreso = new VtpPeajeEgresoDTO(); }
            }
            else
            {
                model.EntidadRecalculoPotencia = new VtpRecalculoPotenciaDTO();
                model.EntidadPeajeEgreso = new VtpPeajeEgresoDTO();
            }
            model.Emprcodi = iEmprCodi;
            model.Pericodi = pericodi;
            model.Recpotcodi = recpotcodi;


            // cambiar al deplegar
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return View(model);
        }

        /// <summary>
        /// Permite cargar un archivo Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            base.ValidarSesionUsuario();
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();

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
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - UploadExcel", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// Permite actualizar o grabar un registro
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmpresaElegida(int EmprCodi)
        {
            if (EmprCodi != 0)
            {
                Session["EmprCodi"] = EmprCodi;
            }
            return RedirectToAction("Index");
        }

        /// Permite seleccionar a la empresa con la que desea trabajar
        [HttpPost]
        public ActionResult EscogerEmpresa()
        {
            base.ValidarSesionUsuario();

            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            List<SeguridadServicio.EmpresaDTO> listTotal = new List<SeguridadServicio.EmpresaDTO>();//
            List<SeguridadServicio.EmpresaDTO> list = new List<SeguridadServicio.EmpresaDTO>();//

            bool accesoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);
            if (accesoEmpresas)
            {
                listTotal = seguridad.ListarEmpresas().OrderBy(x => x.EMPRNOMB).ToList();
            }
            else
            {
                listTotal = this.ListaEmpresas.ToList();
            }


            List<TrnInfoadicionalDTO> listaInfoAdicional = (new TransferenciasAppServicio()).ListTrnInfoadicionals();
            List<int> idsEmpresas = listaInfoAdicional.Where(x => x.Emprcodi != null).Select(x => (int)x.Emprcodi).Distinct().ToList();


            foreach (var item in listTotal)
            {
                list.Add(item);

                //ASSETEC 20190111 - comparar contra otros conceptos
                Int16 iEmpcodi = Convert.ToInt16(item.EMPRCODI);
                foreach (TrnInfoadicionalDTO dtoOtroConcepto in listaInfoAdicional)
                {
                    Int16 iOtroConcepto = Convert.ToInt16(dtoOtroConcepto.Emprcodi);
                    if (iEmpcodi == iOtroConcepto)
                    {
                        SeguridadServicio.EmpresaDTO dtoEmpresaConcepto = new SeguridadServicio.EmpresaDTO();
                        dtoEmpresaConcepto.EMPRCODI = Convert.ToInt16(dtoOtroConcepto.Infadicodi);
                        dtoEmpresaConcepto.EMPRNOMB = dtoOtroConcepto.Infadinomb;
                        dtoEmpresaConcepto.TIPOEMPRCODI = Convert.ToInt16(dtoOtroConcepto.Tipoemprcodi);
                        list.Add(dtoEmpresaConcepto);
                    }
                }
                //--------------------------------------------------
            }

            if (accesoEmpresas)
            {
                list.RemoveAll(x => x.EMPRCODI == 67);
            }


            //UtilTransfPotencia.ObtenerEmpresasPorUsuario(User.Identity.Name);
            BaseModel model = new BaseModel();
            model.ListaEmpresas = new List<EmpresaDTO>();
            foreach (var item in list)
            {
                //if (item.EMPRCODI == 12758)
                //{
                //    item.EMPRCODI = 11567;
                //    item.EMPRNOMB = "STATKRAFT";
                //}

                model.ListaEmpresas.Add(new EmpresaDTO { EmprCodi = item.EMPRCODI, EmprNombre = item.EMPRNOMB });
            }
            return PartialView(model);
        }

        #region Grilla Excel

        /// <summary>
        /// Muestra la grilla excel para el Desarrollo de peajes e ingresos tarifarios
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcel(int pericodi, int recpotcodi, int emprcodi, int pegrcodi = 0)
        {
            base.ValidarSesionUsuario();

            GridExcelModel model = new GridExcelModel();
            CodigoRetiroGeneradoModel modelpe = new CodigoRetiroGeneradoModel();

            try
            {
                #region Armando de contenido
                //Definimos la cabecera como una matriz
                string[] Cabecera1 = { "Codigo", "Cliente", "Barra", "Contrato", "Tipo Usuario", "Precio Potencia S/ /kW-mes", "Potencia Coincidente kW", "Potencia Declarada kW", "Peaje Unitario /KW mes", "Factor pérdida", "Calidad" };

                //Ancho de cada columna
                int[] widths = { 120, 120, 120, 80, 80, 120, 120, 120, 120, 80, 80 };
                object[] columnas = new object[11];

                bool pegrestado = false;
                bool pegrestadocalculo = false;
                bool procesoCorrecto = true;
                model.sEstado = "SI"; //PegrPlazo <- Si entra a liquidación
                                      //Obtener el periodo de recalculo
                modelpe.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotencia(pericodi, recpotcodi);
                //Validamos si el periodo / versión ya esta cerrado 
                //(modelpe.EntidadRecalculoPotencia.Recpotfechalimite < DateTime.Now)
                if (modelpe.EntidadRecalculoPotencia.Recpotestado.Equals("Cerrado"))
                {
                    pegrestado = true; //Deshabilita los botones para que grabe o realice cualquier otra acción
                }
                else
                {   //Consultamos por la fecha limite para el envio de información
                    try
                    {   //Si todo el proceso sale bien 
                        IFormatProvider culture = new System.Globalization.CultureInfo("es-PE", true); // Specify exactly how to interpret the string.
                        string sHora = modelpe.EntidadRecalculoPotencia.Recpothoralimite;
                        double dHora = Convert.ToDouble(sHora.Substring(0, sHora.IndexOf(":")));
                        double dMinute = Convert.ToDouble(sHora.Substring(sHora.IndexOf(":") + 1));
                        DateTime dDiaHoraLimite = Convert.ToDateTime(modelpe.EntidadRecalculoPotencia.Recpotfechalimite);
                        dDiaHoraLimite = dDiaHoraLimite.AddHours(dHora);
                        dDiaHoraLimite = dDiaHoraLimite.AddMinutes(dMinute);
                        if (dDiaHoraLimite < System.DateTime.Now)
                        {
                            model.sEstado = "NO"; //PegrPlazo <- NO entra a liquidación, la Fecha/Hora limite esmenor a la fecha del sistema
                        }
                    }
                    catch (Exception e)
                    {   // Error en la conversión del tipo hora a fecha.
                        string sMensaje = e.ToString();
                    }
                }
                //Obtener las empresas para el dropdown
                var ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR().Select(x => x.EmprNombre).ToList();
                //Obtener las barras para el dropdown
                var ListaBarras = this.servicioBarra.ListBarras().Select(x => x.BarrNombre).ToList();
                //Lista de PeajesEgreso por EmprCodi
                if (pegrcodi == 0)
                { modelpe.EntidadPeajeEgreso = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresos(emprcodi, pericodi, recpotcodi); }
                else
                { modelpe.EntidadPeajeEgreso = this.servicioTransfPotencia.GetByIdVtpPeajeEgreso(pegrcodi); }

                //Se arma la matriz de datos
                string[][] data;
                if (modelpe.EntidadPeajeEgreso != null)
                {
                    if (modelpe.EntidadPeajeEgreso.Pegrestado.Equals("NO"))
                    {
                        //Es un envio que ya esta INACTIVO - no es el ultimo
                        pegrestado = true;  //Deshabilita los botones para que grabe o realice cualquier otra acción
                    }

                }
                modelpe.ListaPeajeEgresoDetalle = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresoDetallesNuevo(pegrcodi, recpotcodi, emprcodi, pericodi);
                if (modelpe.ListaPeajeEgresoDetalle.Any())
                {
                    data = new string[modelpe.ListaPeajeEgresoDetalle.Count() + 1][];
                    data[0] = Cabecera1;
                    int index = 1;
                    model.NumRegistros = modelpe.ListaPeajeEgresoDetalle.Count;
                    decimal? valorPrecioPoteCoinci;
                    foreach (VtpPeajeEgresoDetalleDTO item in modelpe.ListaPeajeEgresoDetalle)
                    {
                        valorPrecioPoteCoinci = item.Pegrdpotecoincidente.ToString() == "" || item.Pegrdpotecoincidente == null ? item.Pegrdpoteegreso : item.Pegrdpotecoincidente;
                        if (valorPrecioPoteCoinci == 0 || valorPrecioPoteCoinci == null
                            || item.Pegrdpotedeclarada.ToString() == "" || item.Pegrdpotedeclarada == null ||
                            item.Pegrdpeajeunitario.ToString() == "" || item.Pegrdpeajeunitario == null)
                        {
                            procesoCorrecto = false;
                        }

                        string[] itemDato = {
                        //item.Coregecodvtp == null ? "" : item.Coregecodvtp.ToString(),
                        item.Codcncodivtp == null ? "" : item.Codcncodivtp.ToString(),
                        item.Emprnomb, item.Barrnombre,
                        item.TipConNombre == null ? "" : item.TipConNombre.ToString(),
                        item.Pegrdtipousuario,
                        item.Pegrdpreciopote.ToString(),
                        valorPrecioPoteCoinci.ToString(),
                        item.Pegrdpotedeclarada.ToString(),
                        item.Pegrdpeajeunitario.ToString(),
                        item.Pegrdfacperdida == null || item.Pegrdfacperdida.ToString() == "" ? "" : item.Pegrdfacperdida.ToString(),
                        item.Pegrdcalidad
                    };
                        data[index] = itemDato;
                        index++;
                    }
                }
                else
                {
                    data = new string[2][];
                    data[0] = Cabecera1;
                    int index = 1;
                    string[] itemDato = { "", "", "", "", "", "", "", "", "", "", "" };
                    data[index] = itemDato;
                }

                ///////////          
                string[] aTipoUsuario = { "Regulado", "Libre", "Gran Usuario" };
                string[] aLicitacion = { "Si", "No" };
                string[] aCalidad = { "Final", "Preliminar" };

                columnas[0] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true,
                };
                columnas[1] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoLista,
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true,
                };
                columnas[2] = new
                {   //Barrcodi - Barrnombre
                    type = GridExcelModel.TipoLista,
                    source = ListaBarras.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true,
                };
                columnas[3] = new
                {   //Licitación
                    type = GridExcelModel.TipoLista,
                    //source = aLicitacion,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                columnas[4] = new
                {   //Rpsctipousuario
                    type = GridExcelModel.TipoLista,
                    //  source = aTipoUsuario,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                columnas[5] = new
                {   //Precio Potencia
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    readOnly = pegrestadocalculo,
                };
                columnas[6] = new
                {   //Potencia Egreso
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    readOnly = pegrestadocalculo,
                };
                columnas[7] = new
                {   //Potencia Calculada
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    readOnly = pegrestadocalculo,
                };
                columnas[8] = new
                {   //Potencia Declarada
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    readOnly = pegrestadocalculo,
                };
                columnas[9] = new
                {   //Barrcodifco - Barrnombrefco
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    readOnly = false,
                };
                columnas[10] = new
                {   //Rpscpoteactiva
                    type = GridExcelModel.TipoTexto,
                    source = "",
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };

                #endregion
                model.bGrabar = pegrestado;
                model.ListaEmpresas = ListaEmpresas.ToArray();
                model.ListaBarras = ListaBarras.ToArray();
                model.ListaLicitacion = aLicitacion.ToArray();
                model.ListaCalidad = aCalidad.ToArray();
                model.ListaTipoUsuario = aTipoUsuario.ToArray();

                model.Data = data;
                model.Widths = widths;
                model.Columnas = columnas;
                model.FixedRowsTop = 1;
                model.FixedColumnsLeft = 1;
                model.procesoCorrecto = procesoCorrecto;

                //ASSETEC 20190219
                if (modelpe.EntidadPeajeEgreso != null)
                    model.Pegrcodi = modelpe.EntidadPeajeEgreso.Pegrcodi;

                return Json(model);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - GrillaExcel", ex);
                return Json(ex);
            }
        }

        #endregion



        /// <summary>
        /// Lee datos desde el archivo excel
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult ProcesarArchivo(string sarchivo, int pegrcodi = 0, int pericodi = 0, int recpotcodi = 0, int emprcodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            CodigoRetiroGeneradoModel modelpe = new CodigoRetiroGeneradoModel();
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
            string sResultado = "1";
            int iRegError = 0;
            string sMensajeError = "";

            string[] aLicitacion = { "LICITACION", "BILATERAL", "AUTOCONSUMO" };

            try
            {
                #region Armando de contenido
                //Definimos la cabecera como una matriz
                string[] Cabecera1 = { "Codigo", "Cliente", "Barra", "Contrato", "Tipo Usuario", "Precio Potencia S/ /kW-mes", "Potencia Coincidente kW", "Potencia Declarada kW", "Peaje Unitario /KW mes", "Factor pérdida", "Calidad" };

                //Ancho de cada columna
                int[] widths = { 120, 120, 120, 100, 100, 100, 100, 100, 100, 80, 80 };
                object[] columnas = new object[11];

                //Obtener las empresas para el dropdown
                var ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR().Select(x => x.EmprNombre).ToList();
                //Obtener las barras para el dropdown
                var ListaBarras = this.servicioBarra.ListBarras().Select(x => x.BarrNombre).ToList();
                bool pegrestado = false;

                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioTransfPotencia.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count - 4][]; // -6 por las primeras filas del encabezado + 2 por las dos cabeceras
                data[0] = Cabecera1;
                int index = 1;
                int iFila = 0;
                int iNumRegistros = 0;
                List<String> lstCodVtp = new List<String>();
                Boolean duplicado = false;
                String codVtpDuplicado = "";
                var listaDetalleCargaGrilla = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresoDetallesNuevo(pegrcodi, recpotcodi, emprcodi, pericodi);


                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 7)
                    {
                        continue;
                    }
                    for (int i = 0; i < lstCodVtp.Count; i++)
                    {
                        if (dtRow[1].ToString() == lstCodVtp[i].ToString())
                        {
                            codVtpDuplicado = dtRow[1].ToString();
                            duplicado = true;
                            break;
                        }
                    }

                    int iNumFila = iFila + 1;

                    if (duplicado)
                    {
                        duplicado = false;
                        sMensajeError += "<br>Fila:" + (iNumFila - 5) + " - El Codigo VTP está duplicado: " + codVtpDuplicado;
                        iRegError++;
                        continue;
                    }
                    //CODIVO VTP
                    string sCodigoVtp = dtRow[1].ToString();
                    VtpCodigoConsolidadoDTO dtoCodigoConsolidado = this.servicioCodigoConsolidado.GetByCodigoVTP(sCodigoVtp);
                    if (dtoCodigoConsolidado == null)
                    {
                        sMensajeError += "<br>No existe el codigo VTP: " + sCodigoVtp;
                        sCodigoVtp = "";
                        iRegError++;
                        continue;
                    }
                    //CLIENTE
                    string sCliente = dtRow[2].ToString();
                    EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(sCliente);
                    if (dtoEmpresa == null)
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No existe el cliente: " + sCliente;
                        sCliente = "";
                        iRegError++;
                        continue;
                    }
                    if (sCliente != dtoCodigoConsolidado.Cliente.Trim())
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No coincide el cliente: " + sCliente + ", debe ser el siguiente cliente: " + dtoCodigoConsolidado.Cliente;
                        sCliente = "";
                        iRegError++;
                        continue;
                    }
                    //BARRA
                    string sBarra = dtRow[3].ToString();
                    BarraDTO dtoBarra = this.servicioBarra.GetByBarra(sBarra);
                    if (dtoBarra == null)
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No existe la barra: " + sBarra;
                        sBarra = "";
                        iRegError++;
                        continue;
                    }
                    if (sBarra.ToUpper().Trim() != dtoCodigoConsolidado.Barra.ToUpper().Trim())
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No coincide la barra: " + sBarra + ", debe ser la siguiente barra: " + dtoCodigoConsolidado.Barra;
                        sCliente = "";
                        iRegError++;
                        continue;
                    }
                    //LICITACION
                    string sLicitacion = dtRow[4].ToString().ToUpper();
                    TipoContratoDTO dtoTipoContrato = this.servicioTipoContrato.GetByNombre(sLicitacion);
                    if (dtoTipoContrato == null)
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No existe el tipo de contrato: " + sLicitacion;
                        sLicitacion = "";
                        iRegError++;
                        continue;
                    }
                    if (sLicitacion.ToUpper().Trim() != dtoCodigoConsolidado.TipConNombre.ToUpper().Trim())
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No coincide el tipo de contrato: " + sLicitacion + ", debe ser el tipo de contrato: " + dtoCodigoConsolidado.TipConNombre;
                        sLicitacion = "";
                        iRegError++;
                        continue;
                    }

                    int existRelacion = listaDetalleCargaGrilla.Where(x => x.Coregecodvtp == dtRow[1].ToString() &&
                    x.Barrnombre?.Trim() == sBarra?.Trim() &&
                    x.Emprnomb?.Trim() == sCliente?.Trim()).Count();

                    if (existRelacion == 0)
                    {
                        sMensajeError += "<br>Fila:" + (iNumFila - 5) + " - La siguiente relacion no se encuentra cargada: " +
                          dtRow[1].ToString() + " " + sCliente + " " + sBarra;
                        sLicitacion = "";
                        iRegError++;
                        continue;
                    }

                    //tipousuario
                    string sTipoUsuario = Convert.ToString(dtRow[5]).ToString().ToUpper();
                    if (sTipoUsuario == "REGULADO")
                    { sTipoUsuario = "Regulado"; }
                    else if (sTipoUsuario == "LIBRE")
                    { sTipoUsuario = "Libre"; }
                    else if (sTipoUsuario == "GRAN USUARIO")
                    { sTipoUsuario = "Gran Usuario"; }
                    else
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No existe el tipo de usuario: " + sTipoUsuario;
                        sTipoUsuario = "";
                        iRegError++;
                        continue;
                    }

                    if (sTipoUsuario.ToUpper().Trim() != dtoCodigoConsolidado.TipUsuNombre.ToUpper().Trim())
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No coincide el tipo de usuario: " + sTipoUsuario.ToUpper() + ", debe ser el siguiente tipo de usuario: " + dtoCodigoConsolidado.TipUsuNombre;
                        sLicitacion = "";
                        iRegError++;
                        continue;
                    }

                    string precioPote = dtRow[6].ToString() == "null" ? "" : dtRow[6].ToString();
                    string poteCoincidente = dtRow[7].ToString() == "null" ? "" : dtRow[7].ToString();
                    string poteDeclarada = dtRow[8].ToString() == "null" ? "" : dtRow[8].ToString();
                    string peajeUnitario = dtRow[9].ToString() == "null" ? "" : dtRow[9].ToString();
                    string factorPerdida = dtRow[10].ToString() == "null" ? "" : dtRow[10].ToString();
                    //
                    decimal dPreciopote = UtilTransfPotencia.ValidarNumero(precioPote);
                    decimal dPotecoincidente = UtilTransfPotencia.ValidarNumero(poteCoincidente);
                    decimal dPotedeclarada = sLicitacion == "AUTOCONSUMO" ? 0 : UtilTransfPotencia.ValidarNumero(poteDeclarada);
                    decimal dPeajeunitario = sLicitacion == "AUTOCONSUMO" ? 0 : UtilTransfPotencia.ValidarNumero(peajeUnitario);
                    poteDeclarada = sLicitacion == "AUTOCONSUMO" ? "" : poteDeclarada;
                    peajeUnitario = sLicitacion == "AUTOCONSUMO" ? "" : peajeUnitario;
                    decimal dFactorperdida = UtilTransfPotencia.ValidarNumero(factorPerdida);
                    string sCalidad = "Preliminar";
                    if (Convert.ToString(dtRow[11]).ToString().ToUpper() == "FINAL")
                    {
                        sCalidad = "Final";
                    }
                    string[] itemDato = {
                        sCodigoVtp,
                        sCliente,
                        sBarra,
                        sLicitacion,
                        sTipoUsuario,
                        precioPote == "" ? "" : dPreciopote == 0 ? "0" : dPreciopote.ToString(),
                        poteCoincidente == "" ? "" : dPotecoincidente == 0 ? "0" : dPotecoincidente.ToString(),
                        poteDeclarada == "" ? "" : dPotedeclarada == 0 ? "0" : dPotedeclarada.ToString(),
                        peajeUnitario == "" ? "" : dPeajeunitario == 0 ? "0" : dPeajeunitario.ToString(),
                        factorPerdida == "" ? "" : dFactorperdida == 0 ? "0" : dFactorperdida.ToString(),
                        sCalidad
                    };
                    data[index] = itemDato;
                    index++;
                    iNumRegistros++;
                    lstCodVtp.Add(dtRow[1].ToString());


                }

                #region AuditoriaProceso

                PeriodoDTO periodoDTO = this.servicioPeriodo.GetByIdPeriodo(pericodi);

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.CargaInformacionVTPExtranet;
                objAuditoria.Estdcodi = (int)EVtpEstados.SubirFormato;
                objAuditoria.Audproproceso = "Importación de data en excel extranet - VTP";
                objAuditoria.Audprodescripcion = "Se importa la data del periodo " + periodoDTO.PeriNombre + " - cantidad de errores - " + iRegError + " - usuario " + base.UserName;
                objAuditoria.Audprousucreacion = base.UserName;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);
                if (auditoria == 0)
                {
                    Logger.Error(NombreControlador + " - Error Save Auditoria - Importar Data Extranet VTP");
                }

                #endregion

                string[] aTipoUsuario = { "Regulado", "Libre", "Gran Usuario" };
                string[] aCalidad = { "Final", "Preliminar" };
                columnas[0] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true,
                };
                columnas[1] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoLista,
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true,
                };
                columnas[2] = new
                {   //Barrcodi - Barrnombre
                    type = GridExcelModel.TipoLista,
                    source = ListaBarras.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true,
                };
                columnas[3] = new
                {   //Licitación
                    type = GridExcelModel.TipoLista,
                    source = aLicitacion,
                    strict = false,
                    correctFormat = true,
                    readOnly = true,
                };
                columnas[4] = new
                {   //Rpsctipousuario
                    type = GridExcelModel.TipoLista,
                    source = aTipoUsuario,
                    strict = false,
                    correctFormat = true,
                    readOnly = true,
                };
                columnas[5] = new
                {   //Precio Potencia
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[6] = new
                {   //Potencia Egreso
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "#,##0.00",
                    readOnly = pegrestado,
                };
                columnas[7] = new
                {   //Potencia Calculada
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "#,##0.00",
                    readOnly = pegrestado,
                };
                columnas[8] = new
                {   //Potencia Declarada
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[9] = new
                {   //Factor pérdida
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = false,
                };
                columnas[10] = new
                {   //Rpscpoteactiva
                    type = GridExcelModel.TipoTexto,
                    source = "",
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                if (!string.IsNullOrEmpty(sMensajeError))
                    sMensajeError += ". Por favor, Haga Click en consultar nuevamente";

                #endregion
                model.bGrabar = pegrestado;
                model.ListaEmpresas = ListaEmpresas.ToArray();
                model.ListaBarras = ListaBarras.ToArray();
                model.ListaLicitacion = aLicitacion.ToArray();
                model.ListaCalidad = aCalidad.ToArray();
                model.ListaTipoUsuario = aTipoUsuario.ToArray();

                model.Data = data;
                model.Widths = widths;
                model.Columnas = columnas;
                model.FixedRowsTop = 1;
                model.FixedColumnsLeft = 1;
                model.RegError = iRegError;
                model.MensajeError = sMensajeError;
                model.NumRegistros = iNumRegistros;
                return Json(model);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - ProcesarArchivo", ex);
                sResultado = ex.Message;
                return Json(sResultado);
            }
        }

        /// <summary>
        /// Permite exportar a un archivo todos los registros en pantalla
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int pericodi = 0, int recpotcodi = 0, int pegrcodi = 0, int emprcodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();

                string file = this.servicioTransfPotencia.GenerarFormatoPeajeEgreso(pericodi, recpotcodi, pegrcodi, emprcodi, formato, pathFile, pathLogo);

                PeriodoDTO periodoDTO = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);

                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.CargaInformacionVTPExtranet;
                objAuditoria.Estdcodi = (int)EVtpEstados.BajarFormato;
                objAuditoria.Audproproceso = "Exportación de data en excel extranet - VTP";
                objAuditoria.Audprodescripcion = "Se exporta data en excel del periodo " + periodoDTO.PeriNombre + " y revisión " + EntidadRecalculoPotencia.Recpotnombre + " - usuario " + base.UserName;
                objAuditoria.Audprousucreacion = base.UserName;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);
                if (auditoria == 0)
                {
                    Logger.Error(NombreControlador + " - Error Save Auditoria - Exportar Data Extranet - VTP");
                }

                #endregion

                return Json(file);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - ExportarData", ex);
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Descarga el archivo
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            return File(path, app, sFecha + "_" + file);
        }


        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public JsonResult GrabarGrillaExcel(int pericodi, int recpotcodi, int emprcodi, string testado, List<string[]> datos)
        {
            base.ValidarSesionUsuario();
            int genemprcodi = emprcodi;
            int pegrcodi = 0;
            int NumRegistros = 0;
            int iRegError = 0;
            int idTipoContrato = 0;
            string sMensajeError = "";
            int pegrdcodi = 0;
            decimal percentVariation = 0;
            bool ceroNegativo = false;
            string emprNomb = "";
            decimal sumaPeajeUnitario = 0;
            List<CodigoPotenciaCoincidenteVTP> lstCodigosVTP = new List<CodigoPotenciaCoincidenteVTP>();

            try
            {
                PeriodoDTO periodoDTO = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                TransfPotencia.Models.PeajeEgresoModel model = new TransfPotencia.Models.PeajeEgresoModel();
                if (testado.Equals(""))
                {
                    testado = "SI";
                    model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotencia(pericodi, recpotcodi);
                    if (model.EntidadRecalculoPotencia != null)
                    {
                        IFormatProvider culture = new System.Globalization.CultureInfo("es-PE", true); // Specify exactly how to interpret the string.
                        string sHora = model.EntidadRecalculoPotencia.Recpothoralimite;
                        double dHora = Convert.ToDouble(sHora.Substring(0, sHora.IndexOf(":")));
                        double dMinute = Convert.ToDouble(sHora.Substring(sHora.IndexOf(":") + 1));
                        DateTime dDiaHoraLimite = Convert.ToDateTime(model.EntidadRecalculoPotencia.Recpotfechalimite);
                        dDiaHoraLimite = dDiaHoraLimite.AddHours(dHora);
                        dDiaHoraLimite = dDiaHoraLimite.AddMinutes(dMinute);
                        if (dDiaHoraLimite < System.DateTime.Now)
                        {
                            testado = "NO"; //PegrPlazo <- NO entra a liquidación, la Fecha/Hora limite esmenor a la fecha del sistema
                        }
                    }
                }

                //Graba Cabezera
                model.Entidad = new VtpPeajeEgresoDTO();
                model.Entidad.Pericodi = pericodi;
                model.Entidad.Recpotcodi = recpotcodi;
                model.Entidad.Emprcodi = genemprcodi;
                model.Entidad.Pegrestado = "SI"; //entra a liquidación
                model.Entidad.Pegrplazo = "S"; //esta en plazo
                model.Entidad.Pegrusucreacion = User.Identity.Name;
                model.Entidad.Pegrfeccreacion = DateTime.Now;
                if (testado.Equals("NO"))
                {
                    model.Entidad.Pegrestado = "NO"; //Se graba, pero no entra a liquidación
                    model.Entidad.Pegrplazo = "N"; //Se graba, pero no esta en plazo
                }

                if (model.Entidad.Pegrestado.Equals("SI"))
                {
                    //Antes de grabar cabezera actualiza los estados de "SI" a "NO"
                    this.servicioTransfPotencia.UpdateByCriteriaVtpPeajeEgreso(genemprcodi, pericodi, recpotcodi);
                }

                //Graba nuevo, vacio sin detalle y es el ultimo dato reportado por el agente
                pegrcodi = this.servicioTransfPotencia.SaveVtpPeajeEgreso(model.Entidad);

                // Obtener el porcentaje por defecto

                VariacionEmpresaModel modelVariacionEmpresa = new VariacionEmpresaModel();
                modelVariacionEmpresa.Entidad = this.servicioVariacionEmpresa.GetDefaultPercentVariationByTipoComp("A");

                // Obtener el porcentaje por empresa

                VariacionEmpresaModel modelVaricionEmpresaEmp = new VariacionEmpresaModel();
                modelVaricionEmpresaEmp.Entidad = this.servicioVariacionEmpresa.GetPercentVariationByEmprCodiAndTipoComp(genemprcodi, "A");

                if (modelVaricionEmpresaEmp.Entidad != null)
                {
                    percentVariation = modelVaricionEmpresaEmp.Entidad.Varempprocentaje;
                }
                else
                {
                    if (modelVariacionEmpresa.Entidad != null)
                    {
                        percentVariation = modelVariacionEmpresa.Entidad.Varempprocentaje;
                    }
                    else
                    {
                        percentVariation = 0;
                    }

                }

                // Obtener la sumatoria de peajes de egreso
                List<VtpPeajeIngresoDTO> ListaPeajeIngreso = this.servicioTransfPotencia.ListVtpPeajeIngresoView(pericodi, recpotcodi);
                foreach (VtpPeajeIngresoDTO item in ListaPeajeIngreso)
                {
                    sumaPeajeUnitario += item.Pingregulado == null ? 0 : Convert.ToDecimal(item.Pingregulado);
                }

                // Obtener el último id de revisión anterior
                VtpPeajeEgresoDTO vtpPeajeEgreso = new VtpPeajeEgresoDTO();
                vtpPeajeEgreso = this.servicioTransfPotencia.GetPreviusPeriod(pericodi - 1, emprcodi);

                //Recorrer matriz para grabar detalle
                //Recorremos la matriz que se inicia en la fila 2

                RecalculoPotenciaModel modelRecalculoPotencia = new RecalculoPotenciaModel();
                modelRecalculoPotencia.Entidad = this.servicioTransfPotencia.GetByIdVtpRecalculoPotencia(pericodi, recpotcodi);

                emprNomb = this.servicioEmpresa.GetByIdEmpresa(emprcodi).EmprNombre;

                for (int j = 1; j < datos.Count(); j++)
                {
                    if (datos[j][0] == null)
                        break;
                    CodigoPotenciaCoincidenteVTP coincidenteVTP = new CodigoPotenciaCoincidenteVTP
                    {
                        codigoVTP = datos[j][0].ToString(),
                        potenciaCoincidente = UtilTransfPotencia.ValidarNumero(datos[j][6].ToString())
                    };
                    lstCodigosVTP.Add(coincidenteVTP);
                }

                for (int f = 1; f < datos.Count(); f++)
                {   //Por Fila
                    if (datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadDetalle = new VtpPeajeEgresoDetalleDTO();
                    model.EntidadDetalle.Pegrcodi = pegrcodi;

                    BarraDTO dtoBarra = new BarraDTO();

                    if (!datos[f][0].Equals(""))
                    {
                        model.EntidadDetalle.Coregecodvtp = datos[f][0].ToString();
                        VtpCodigoConsolidadoDTO dtoCodigoConsolidado = this.servicioCodigoConsolidado.GetByCodigoVTP(datos[f][0].ToString());
                        if (dtoCodigoConsolidado != null)
                        {
                            model.EntidadDetalle.Coregecodi = dtoCodigoConsolidado.Codcncodi;
                        }
                        else
                        {
                            sMensajeError += "<br>Fila:" + (f + 1) + " - No existe el codigo VTP: " + model.EntidadDetalle.Coregecodvtp;
                            iRegError++;
                            break;
                        }
                    }
                    else
                    {
                        sMensajeError += "<br>Fila:" + (f + 1) + " - codigo VTP invalido.";
                        iRegError++;
                        break;
                    }
                    //cliente
                    if (!datos[f][1].Equals(""))
                    {
                        model.EntidadDetalle.Emprnomb = Convert.ToString(datos[f][1]);
                        EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(model.EntidadDetalle.Emprnomb);
                        if (dtoEmpresa != null)
                        {
                            model.EntidadDetalle.Emprcodi = dtoEmpresa.EmprCodi;
                        }
                        else
                        {
                            sMensajeError += "<br>Fila:" + (f + 1) + " - No existe cliente: " + model.EntidadDetalle.Emprnomb;
                            iRegError++;
                            break;
                        }
                    }
                    else
                    {
                        sMensajeError += "<br>Fila:" + (f + 1) + " - cliente invalido.";
                        iRegError++;
                        break;
                    }
                    //barra
                    if (!datos[f][2].Equals(""))
                    {
                        model.EntidadDetalle.Barrnombre = Convert.ToString(datos[f][2]);
                        dtoBarra = this.servicioBarra.GetByBarra(model.EntidadDetalle.Barrnombre);
                        if (dtoBarra != null)
                        {
                            model.EntidadDetalle.Barrcodi = dtoBarra.BarrCodi;
                            model.EntidadDetalle.Barrcodifco = dtoBarra.BarrCodi;
                        }
                        else
                        {
                            sMensajeError += "<br>Fila:" + (f + 1) + " - No existe barra: " + model.EntidadDetalle.Barrnombre;
                            iRegError++;
                            break;
                        }
                    }
                    else
                    {
                        sMensajeError += "<br>Fila:" + (f + 1) + " - barra invalida.";
                        iRegError++;
                        break;
                    }
                    // tipo de contrato
                    if (!datos[f][3].Equals(""))
                    {
                        string sTipoContratoV = Convert.ToString(datos[f][3]).ToString().ToUpper();
                        TipoContratoDTO dtoTipoContrato = this.servicioTipoContrato.GetByNombre(sTipoContratoV);
                        if (dtoTipoContrato == null)
                        {
                            sMensajeError += "<br>Fila:" + (f + 1) + " - No existe tipo de contrato: " + sTipoContratoV;
                            iRegError++;
                            break;
                        }
                        else
                        {
                            idTipoContrato = dtoTipoContrato.TipoContCodi;
                        }
                    }
                    else
                    {
                        sMensajeError += "<br>Fila:" + (f + 1) + " - tipo de contrato invalido.";
                        iRegError++;
                        break;
                    }


                    //tipousuario
                    if (!datos[f][4].Equals(""))
                    {
                        string sTipoUsuario = Convert.ToString(datos[f][4]).ToString().ToUpper();
                        if (sTipoUsuario == "REGULADO")
                        { model.EntidadDetalle.Pegrdtipousuario = "Regulado"; }
                        else if (sTipoUsuario == "LIBRE")
                        { model.EntidadDetalle.Pegrdtipousuario = "Libre"; }
                        else if (sTipoUsuario == "GRAN USUARIO")
                        { model.EntidadDetalle.Pegrdtipousuario = "Gran Usuario"; }
                        else
                        {
                            sMensajeError += "<br>Fila:" + (f + 1) + " - No existe tipo de usuaio: " + sTipoUsuario;
                            iRegError++;
                            break;
                        }
                    }
                    else
                    {
                        sMensajeError += "<br>Fila:" + (f + 1) + " - tipo de usuario invalido.";
                        iRegError++;
                        break;
                    }

                    // AGREGAR UNA COLUMNA QUE ALMACENE EL CONCEPTO DE TIPO USUARIO COMPLETO

                    string sLicitacionAnt = datos[f][3].ToString().ToUpper();
                    if (sLicitacionAnt == "LICITACION")
                        sLicitacionAnt = "Si";
                    else
                        sLicitacionAnt = "No";

                    //licitación
                    //if (!datos[f][3].Equals("") && Convert.ToString(datos[f][3]).ToString().ToUpper() == "SI")
                    //{
                    //    model.EntidadDetalle.Pegrdlicitacion = "Si";
                    //}
                    //else
                    //{
                    //    model.EntidadDetalle.Pegrdlicitacion = "No";
                    //}
                    string sTipoContrato = Convert.ToString(datos[f][3]).ToString().ToUpper();
                    model.EntidadDetalle.Pegrdlicitacion = sLicitacionAnt;
                    model.EntidadDetalle.Pegrdpreciopote = UtilTransfPotencia.ValidarNumero(datos[f][5].ToString());
                    model.EntidadDetalle.Pegrdpoteegreso = UtilTransfPotencia.ValidarNumero(datos[f][6].ToString());
                    model.EntidadDetalle.Pegrdpotecalculada = sTipoContrato == "AUTOCONSUMO" ? 0 : UtilTransfPotencia.ValidarNumero(datos[f][6].ToString());
                    model.EntidadDetalle.Pegrdpotecoincidente = UtilTransfPotencia.ValidarNumero(datos[f][6].ToString());
                    decimal? poteautoconsumo = null;
                    model.EntidadDetalle.Pegrdpotedeclarada = sTipoContrato == "AUTOCONSUMO" ? poteautoconsumo : UtilTransfPotencia.ValidarNumero(datos[f][7].ToString());
                    model.EntidadDetalle.Pegrdpeajeunitario = sTipoContrato == "AUTOCONSUMO" ? poteautoconsumo : UtilTransfPotencia.ValidarNumero(datos[f][8].ToString());
                    model.EntidadDetalle.Pegrdfacperdida = UtilTransfPotencia.ValidarNumero(datos[f][9].ToString());
                    model.EntidadDetalle.Pegrdcalidad = datos[f][10] == null ? "Preliminar" : datos[f][10].ToString();
                    model.EntidadDetalle.TipConCondi = idTipoContrato;
                    model.EntidadDetalle.Pegrdpoteactiva = 0;
                    model.EntidadDetalle.Pegrdpotereactiva = 0;
                    ceroNegativo = model.EntidadDetalle.Pegrdpreciopote == 0 || model.EntidadDetalle.Pegrdpoteegreso == 0 || model.EntidadDetalle.Pegrdpotedeclarada == 0 || model.EntidadDetalle.Pegrdpeajeunitario == 0;
                    //Barra FCO
                    //if (!datos[f][9].Equals(""))
                    //{
                    //    model.EntidadDetalle.Barrnombrefco = Convert.ToString(datos[f][9]);
                    //    BarraDTO dtoBarrafco = this.servicioBarra.GetByBarra(model.EntidadDetalle.Barrnombrefco);
                    //    if (dtoBarrafco != null)
                    //    { model.EntidadDetalle.Barrcodifco = dtoBarrafco.BarrCodi; }
                    //    else
                    //    {
                    //        sMensajeError += "<br>Fila:" + (f + 1) + " - No existe: " + model.EntidadDetalle.Barrnombrefco;
                    //        iRegError++;
                    //        break;
                    //    }
                    //}
                    //else
                    //{
                    //    sMensajeError += "<br>Fila:" + (f + 1) + " - barra FCO invalido.";
                    //    iRegError++;
                    //    break;
                    //}
                    //model.EntidadDetalle.Pegrdpoteactiva = UtilTransfPotencia.ValidarNumero(datos[f][10].ToString());
                    //model.EntidadDetalle.Pegrdpotereactiva = UtilTransfPotencia.ValidarNumero(datos[f][11].ToString());
                    //if (!datos[f][12].Equals("") && Convert.ToString(datos[f][12]).ToString().ToUpper() == "FINAL")
                    //{
                    //    model.EntidadDetalle.Pegrdcalidad = "Final";
                    //}
                    //else
                    //{
                    //    model.EntidadDetalle.Pegrdcalidad = "Preliminar";
                    //}

                    model.EntidadDetalle.Pegrdusucreacion = User.Identity.Name;

                    //Insertar registro
                    pegrdcodi = this.servicioTransfPotencia.SaveVtpPeajeEgresoDetalle(model.EntidadDetalle);

                    #region 1.- Validacion Histórica 

                    int Pegrcodi = vtpPeajeEgreso?.Pegrcodi ?? 0;
                    //decimal porcentajeVariacionDefecto = this.servicioVariacionEmpresa.GetPercentVariationByEmprCodiAndTipoComp(0, "A")?.Varempprocentaje ?? 0;



                    decimal poteCoincidenteOrigin = model.EntidadDetalle?.Pegrdpotecoincidente ?? 0;
                    decimal poteCoincidenteAnterior = 0;

                    VariacionCodigoModel VariacionCodigoDTO = new VariacionCodigoModel();
                    VariacionCodigoDTO.Entidad = this.servicioVariacionCodigo.GetVariacionCodigoByCodVtp(datos[f][0].ToString());

                    TransfPotencia.Models.PeajeEgresoModel modelDetalle = new TransfPotencia.Models.PeajeEgresoModel();
                    modelDetalle.EntidadDetalle = this.servicioTransfPotencia.GetByPegrCodiAndCodVtp(Pegrcodi, datos[f][0].ToString());

                    poteCoincidenteAnterior = modelDetalle.EntidadDetalle?.Pegrdpotecoincidente ?? 0;

                    if (poteCoincidenteOrigin > 0)
                    {
                        decimal porcentaje = (poteCoincidenteOrigin - poteCoincidenteAnterior) / poteCoincidenteOrigin * 100;

                        porcentaje = porcentaje < 0 ? (porcentaje * -1) : porcentaje;
                        porcentaje = Math.Round(porcentaje, MidpointRounding.ToEven);
                        decimal porcentajeCodigo = VariacionCodigoDTO.Entidad != null ? VariacionCodigoDTO.Entidad.VarCodPorcentaje : percentVariation;
                        porcentajeCodigo = Math.Round(porcentajeCodigo, MidpointRounding.ToEven);

                        if (porcentaje > porcentajeCodigo)
                        {
                            VtpValidacionEnvioDTO validacionEnvioDto = new VtpValidacionEnvioDTO();
                            validacionEnvioDto.PegrCodi = pegrcodi;
                            validacionEnvioDto.PegrdCodi = pegrdcodi;
                            validacionEnvioDto.VaenTipoValidacion = "1";
                            validacionEnvioDto.VaenNomCliente = Convert.ToString(datos[f][1]);
                            validacionEnvioDto.VaenCodVtea = "";
                            validacionEnvioDto.VaenCodVtp = datos[f][0].ToString();
                            validacionEnvioDto.VaenBarraTra = "";
                            validacionEnvioDto.VaenBarraSum = Convert.ToString(datos[f][2]);
                            validacionEnvioDto.VaenValorReportado = (decimal)model.EntidadDetalle.Pegrdpotecoincidente;
                            validacionEnvioDto.VaenRevisionAnterior = poteCoincidenteAnterior;
                            validacionEnvioDto.VaenVariacion = porcentaje;
                            validacionEnvioDto.VaenPrecioPotencia = sTipoContrato == "AUTOCONSUMO" ? poteautoconsumo : (decimal)model.EntidadDetalle.Pegrdpreciopote;
                            validacionEnvioDto.VaenPeajeUnitario = sTipoContrato == "AUTOCONSUMO" ? poteautoconsumo : (decimal)model.EntidadDetalle.Pegrdpeajeunitario;
                            int VaenCodi = this.servicioTransfPotencia.SaveVtpValidacionEnvio(validacionEnvioDto);
                        }
                    }

                    #endregion

                    #region 2.- Validacion Energía Activa

                    CodigoPotenciaCoincidenteVTP coincidenteVTPD = lstCodigosVTP.Find(item => item.codigoVTP == datos[f][0].ToString());
                    if (coincidenteVTPD != null)
                    {
                        string codVTP = coincidenteVTPD.codigoVTP;
                        List<CodigoRetiroRelacionDTO> resultadoDetalle = this.servicioRelacionEquivalencia.ListarRelacionCodigoRetiros(1, 20, genemprcodi, null, null, null, null, null, "", codVTP, 0, 0, 0);
                        if (resultadoDetalle.Count > 0)
                        {
                            if (resultadoDetalle[0].ListarRelacion.Count > 0)
                            {
                                String codigoVTEA = resultadoDetalle[0].ListarRelacion[0].Codigovtea;
                                List<CodigoRetiroRelacionDTO> resultadoDetalleVTEA = this.servicioRelacionEquivalencia.ListarRelacionCodigoRetiros(1, 20, genemprcodi, null, null, null, null, null, "", codigoVTEA, 0, 0, 0);
                                if (resultadoDetalleVTEA.Count > 0)
                                {
                                    if (resultadoDetalleVTEA[0].ListarRelacion.Count == 1)
                                    {
                                        if (resultadoDetalleVTEA[0].ListarRelacion[0].Codigovtea != "" && resultadoDetalleVTEA[0].ListarRelacion[0].Codigovtp != "")
                                        {
                                            decimal maximaDemandaVTEA = this.servicioRelacionEquivalencia.GetMaximaDemandaVTEAEmpresa(pericodi, recpotcodi, resultadoDetalleVTEA[0].ListarRelacion[0].Codigovtea, genemprcodi);
                                            maximaDemandaVTEA = maximaDemandaVTEA * 4000;
                                            decimal potenciaCoincidenteVTP = coincidenteVTPD.potenciaCoincidente;

                                            decimal porcentajeVariacion = maximaDemandaVTEA > 0 ? (maximaDemandaVTEA - potenciaCoincidenteVTP) / maximaDemandaVTEA * 100 : 0;
                                            porcentajeVariacion = porcentajeVariacion < 0 ? porcentajeVariacion * -1 : porcentajeVariacion;
                                            porcentajeVariacion = Math.Round(porcentajeVariacion, MidpointRounding.ToEven);
                                            var porcVari = Math.Round(resultadoDetalleVTEA[0].Retrelvari, MidpointRounding.ToEven);
                                            if (porcentajeVariacion > porcVari)
                                            {
                                                VtpValidacionEnvioDTO validacionEnvioDto = new VtpValidacionEnvioDTO();
                                                validacionEnvioDto.PegrCodi = pegrcodi;
                                                validacionEnvioDto.PegrdCodi = pegrdcodi;
                                                validacionEnvioDto.VaenTipoValidacion = "2";
                                                validacionEnvioDto.VaenNomCliente = Convert.ToString(datos[f][1]);
                                                validacionEnvioDto.VaenCodVtea = resultadoDetalleVTEA[0].ListarRelacion[0].Codigovtea;
                                                validacionEnvioDto.VaenCodVtp = resultadoDetalleVTEA[0].ListarRelacion[0].Codigovtp;
                                                validacionEnvioDto.VaenValorVtea = maximaDemandaVTEA;
                                                validacionEnvioDto.VaenValorVtp = potenciaCoincidenteVTP;
                                                validacionEnvioDto.VaenBarraTra = "";
                                                validacionEnvioDto.VaenBarraSum = "";
                                                validacionEnvioDto.VaenValorCoes = 1;
                                                validacionEnvioDto.VaenValorReportado = porcentajeVariacion;
                                                validacionEnvioDto.VaenRevisionAnterior = 0;
                                                validacionEnvioDto.VaenVariacion = resultadoDetalleVTEA[0].Retrelvari;
                                                validacionEnvioDto.VaenPrecioPotencia = 0;
                                                validacionEnvioDto.VaenPeajeUnitario = 0;

                                                int VaenCodi = this.servicioTransfPotencia.SaveVtpValidacionEnvio(validacionEnvioDto);

                                                int index = lstCodigosVTP.FindIndex(item => item.codigoVTP == datos[f][0].ToString());
                                                lstCodigosVTP.RemoveAt(index);
                                            }
                                        }
                                    }
                                    else if (resultadoDetalleVTEA[0].ListarRelacion.Count > 1)
                                    {
                                        decimal maximaDemandaVTEA = 0;
                                        if (lstCodigosVTP.Exists(item => item.codigoVTP == resultadoDetalleVTEA[0].ListarRelacion[0].Codigovtp))
                                        {

                                            var distinctVTEA = resultadoDetalleVTEA[0].ListarRelacion.Select(x => x.Codigovtea).Distinct();
                                            foreach (var vtea in distinctVTEA)
                                            {
                                                if (!string.IsNullOrEmpty(vtea))
                                                {
                                                    maximaDemandaVTEA += this.servicioRelacionEquivalencia.GetMaximaDemandaVTEAEmpresa(pericodi, recpotcodi, vtea, genemprcodi);
                                                }
                                            }

                                            //decimal maximaDemandaVTEA = this.servicioRelacionEquivalencia.GetMaximaDemandaVTEAEmpresa(pericodi, recpotcodi, resultadoDetalleVTEA[0].ListarRelacion[0].Codigovtea, genemprcodi);
                                            maximaDemandaVTEA = maximaDemandaVTEA * 4000;
                                            decimal potenciaCoincidenteVTP = 0;

                                            for (int i = 0; i < resultadoDetalleVTEA[0].ListarRelacion.Count; i++)
                                            {
                                                if (resultadoDetalleVTEA[0].ListarRelacion[i].Codigovtp != "")
                                                {
                                                    var codvtp = resultadoDetalleVTEA[0].ListarRelacion[i].Codigovtp;
                                                    decimal? valor = lstCodigosVTP.Find(item => item.codigoVTP == codvtp)?.potenciaCoincidente;
                                                    potenciaCoincidenteVTP += valor ?? 0;
                                                }
                                            }

                                            decimal porcentajeVariacion = maximaDemandaVTEA > 0 ? Math.Round(((maximaDemandaVTEA - potenciaCoincidenteVTP) / maximaDemandaVTEA) * 100, 4) : 0;

                                            porcentajeVariacion = porcentajeVariacion < 0 ? porcentajeVariacion * -1 : porcentajeVariacion;
                                            porcentajeVariacion = Math.Round(porcentajeVariacion, MidpointRounding.ToEven);
                                            var porcVari = Math.Round(resultadoDetalleVTEA[0].Retrelvari, MidpointRounding.ToEven);

                                            if (porcentajeVariacion > porcVari)
                                            {
                                                for (int i = 0; i < resultadoDetalleVTEA[0].ListarRelacion.Count; i++)
                                                {
                                                    if (resultadoDetalleVTEA[0].ListarRelacion[i].Codigovtea != "")
                                                    {
                                                        var valorVTEA = this.servicioRelacionEquivalencia.GetMaximaDemandaVTEAEmpresa(pericodi, recpotcodi, resultadoDetalleVTEA[0].ListarRelacion[i].Codigovtea, genemprcodi);

                                                        if (lstCodigosVTP.Exists(item => item.codigoVTP == resultadoDetalleVTEA[0].ListarRelacion[i].Codigovtp))
                                                        {
                                                            potenciaCoincidenteVTP = string.IsNullOrEmpty(resultadoDetalleVTEA[0].ListarRelacion[i].Codigovtp) ? 0 : lstCodigosVTP.Find(item => item.codigoVTP == resultadoDetalleVTEA[0].ListarRelacion[i].Codigovtp).potenciaCoincidente;
                                                            VtpValidacionEnvioDTO validacionEnvioDto = new VtpValidacionEnvioDTO();
                                                            validacionEnvioDto.PegrCodi = pegrcodi;
                                                            validacionEnvioDto.PegrdCodi = pegrdcodi;
                                                            validacionEnvioDto.VaenTipoValidacion = "2";
                                                            validacionEnvioDto.VaenNomCliente = Convert.ToString(datos[f][1]);
                                                            validacionEnvioDto.VaenCodVtea = resultadoDetalleVTEA[0].ListarRelacion[i].Codigovtea;
                                                            validacionEnvioDto.VaenCodVtp = resultadoDetalleVTEA[0].ListarRelacion[i].Codigovtp;
                                                            validacionEnvioDto.VaenValorVtea = valorVTEA * 4000;
                                                            validacionEnvioDto.VaenValorVtp = potenciaCoincidenteVTP;
                                                            validacionEnvioDto.VaenBarraTra = "";
                                                            validacionEnvioDto.VaenBarraSum = "";
                                                            validacionEnvioDto.VaenValorCoes = resultadoDetalleVTEA[0].ListarRelacion.Count;
                                                            validacionEnvioDto.VaenValorReportado = porcentajeVariacion;
                                                            validacionEnvioDto.VaenRevisionAnterior = 0;
                                                            validacionEnvioDto.VaenVariacion = resultadoDetalleVTEA[0].Retrelvari;
                                                            validacionEnvioDto.VaenPrecioPotencia = 0;
                                                            validacionEnvioDto.VaenPeajeUnitario = 0;

                                                            _ = this.servicioTransfPotencia.SaveVtpValidacionEnvio(validacionEnvioDto);
                                                            if (!string.IsNullOrEmpty(resultadoDetalleVTEA[0].ListarRelacion[i].Codigovtp))
                                                            {
                                                                int index = lstCodigosVTP.FindIndex(item => item.codigoVTP == resultadoDetalleVTEA[0].ListarRelacion[i].Codigovtp);
                                                                lstCodigosVTP.RemoveAt(index);
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region 3.- Validación Factores de Pérdida

                    if (dtoBarra.BarrFactorPerdida != model.EntidadDetalle.Pegrdfacperdida)
                    {
                        VtpValidacionEnvioDTO validacionEnvioDto = new VtpValidacionEnvioDTO();
                        validacionEnvioDto.PegrCodi = pegrcodi;
                        validacionEnvioDto.PegrdCodi = pegrdcodi;
                        validacionEnvioDto.VaenTipoValidacion = "3";
                        validacionEnvioDto.VaenNomCliente = Convert.ToString(datos[f][1]);
                        validacionEnvioDto.VaenCodVtea = "";
                        validacionEnvioDto.VaenCodVtp = datos[f][0].ToString();
                        validacionEnvioDto.VaenBarraTra = "";
                        validacionEnvioDto.VaenBarraSum = Convert.ToString(datos[f][2]);
                        validacionEnvioDto.VaenValorCoes = dtoBarra.BarrFactorPerdida;
                        validacionEnvioDto.VaenValorReportado = model.EntidadDetalle.Pegrdfacperdida;
                        validacionEnvioDto.VaenRevisionAnterior = 0;
                        validacionEnvioDto.VaenVariacion = 0;
                        validacionEnvioDto.VaenPrecioPotencia = sTipoContrato == "AUTOCONSUMO" ? poteautoconsumo : (decimal)model.EntidadDetalle.Pegrdpreciopote;
                        validacionEnvioDto.VaenPeajeUnitario = sTipoContrato == "AUTOCONSUMO" ? poteautoconsumo : (decimal)model.EntidadDetalle.Pegrdpeajeunitario;

                        int VaenCodi = this.servicioTransfPotencia.SaveVtpValidacionEnvio(validacionEnvioDto);
                    }

                    #endregion

                    #region 4.- Validación Precio Potencia

                    if (modelRecalculoPotencia.Entidad != null)
                    {
                        decimal precioObtenidoReCalculo = (decimal)modelRecalculoPotencia.Entidad.Recpotpreciopoteppm;
                        decimal factorPorPrecioReCalculo = Math.Round(precioObtenidoReCalculo * dtoBarra.BarrFactorPerdida, 2);
                        if (model.EntidadDetalle.Pegrdpreciopote < precioObtenidoReCalculo || model.EntidadDetalle.Pegrdpreciopote < factorPorPrecioReCalculo)
                        {
                            VtpValidacionEnvioDTO validacionEnvioDto = new VtpValidacionEnvioDTO();
                            validacionEnvioDto.PegrCodi = pegrcodi;
                            validacionEnvioDto.PegrdCodi = pegrdcodi;
                            validacionEnvioDto.VaenTipoValidacion = "4";
                            validacionEnvioDto.VaenNomCliente = Convert.ToString(datos[f][1]);
                            validacionEnvioDto.VaenCodVtea = "";
                            validacionEnvioDto.VaenCodVtp = datos[f][0].ToString();
                            validacionEnvioDto.VaenBarraTra = "";
                            validacionEnvioDto.VaenBarraSum = Convert.ToString(datos[f][2]);
                            validacionEnvioDto.VaenValorCoes = modelRecalculoPotencia.Entidad.Recpotpreciopoteppm;
                            validacionEnvioDto.VaenValorReportado = 0;
                            validacionEnvioDto.VaenRevisionAnterior = 0;
                            validacionEnvioDto.VaenVariacion = 0;
                            validacionEnvioDto.VaenPrecioPotencia = (decimal)model.EntidadDetalle.Pegrdpreciopote;
                            validacionEnvioDto.VaenPeajeUnitario = sTipoContrato == "AUTOCONSUMO" ? poteautoconsumo : (decimal)model.EntidadDetalle.Pegrdpeajeunitario;

                            int VaenCodi = this.servicioTransfPotencia.SaveVtpValidacionEnvio(validacionEnvioDto);
                        }
                    }

                    #endregion

                    #region 5.- Validación Peaje Unitario

                    if (modelRecalculoPotencia.Entidad != null)
                    {
                        decimal precioPeajeCalculado = Math.Round(sumaPeajeUnitario * dtoBarra.BarrFactorPerdida, 3);
                        if (model.EntidadDetalle.Pegrdpeajeunitario < sumaPeajeUnitario || model.EntidadDetalle.Pegrdpeajeunitario < precioPeajeCalculado)
                        {
                            if (sTipoContrato != "AUTOCONSUMO")
                            {
                                VtpValidacionEnvioDTO validacionEnvioDto = new VtpValidacionEnvioDTO();
                                validacionEnvioDto.PegrCodi = pegrcodi;
                                validacionEnvioDto.PegrdCodi = pegrdcodi;
                                validacionEnvioDto.VaenTipoValidacion = "5";
                                validacionEnvioDto.VaenNomCliente = Convert.ToString(datos[f][1]);
                                validacionEnvioDto.VaenCodVtea = "";
                                validacionEnvioDto.VaenCodVtp = datos[f][0].ToString();
                                validacionEnvioDto.VaenBarraTra = "";
                                validacionEnvioDto.VaenBarraSum = Convert.ToString(datos[f][2]);
                                // validacionEnvioDto.VaenValorCoes = sumaPeajeUnitario;
                                validacionEnvioDto.VaenValorCoes = model.EntidadDetalle.Pegrdpeajeunitario;
                                validacionEnvioDto.VaenValorReportado = 0;
                                validacionEnvioDto.VaenRevisionAnterior = 0;
                                validacionEnvioDto.VaenVariacion = 0;
                                validacionEnvioDto.VaenPrecioPotencia = (decimal)model.EntidadDetalle.Pegrdpreciopote;
                                validacionEnvioDto.VaenPeajeUnitario = sTipoContrato == "AUTOCONSUMO" ? poteautoconsumo : (decimal)model.EntidadDetalle.Pegrdpeajeunitario;

                                int VaenCodi = this.servicioTransfPotencia.SaveVtpValidacionEnvio(validacionEnvioDto);
                            }
                        }
                    }

                    #endregion

                    NumRegistros++;
                }
                model.Pegrcodi = pegrcodi;
                model.sFecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").ToString();

                #region EnviarNotificacionAgente

                VtpPeajeEgresoDTO peajeEgresoNoti = new VtpPeajeEgresoDTO
                {
                    Pericodi = pericodi,
                    Recpotcodi = recpotcodi,
                    Pegrcodi = pegrcodi,
                    Pegrfeccreacion = DateTime.Now
                };

                PeriodoDTO periCodi = this.servicioPeriodo.GetByIdPeriodo(pericodi);

                SeguridadServicio.UserDTO userDto = this.seguridad.ObtenerUsuarioPorLogin(User.Identity.Name);

                TransfPotencia.Models.PeajeEgresoModel model2 = new TransfPotencia.Models.PeajeEgresoModel();
                model2.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotencia(pericodi, recpotcodi);

                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                this.servicioTransfPotencia.EnviarCorreoNotificacionEnvioExtranet(peajeEgresoNoti, emprNomb, periCodi.PeriNombre, model2.EntidadRecalculoPotencia.Recpotnombre, userDto.UserEmail, pathFile, pathLogo);

                #endregion

                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.CargaInformacionVTPExtranet;
                objAuditoria.Estdcodi = (int)EVtpEstados.EnviarDatos;
                objAuditoria.Audproproceso = "Se realizó el envío de información desde extranet - VTP";
                objAuditoria.Audprodescripcion = "Se realizó el envío con el código " + periodoDTO.PeriNombre + " - data con errores : " + (ceroNegativo ? "Si" : "No") + " - usuario " + base.UserName;
                objAuditoria.Audprousucreacion = base.UserName;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);
                if (auditoria == 0)
                {
                    Logger.Error(NombreControlador + " - Error Save Auditoria - Realizar envío data Extranet");
                }

                #endregion

                if (!sMensajeError.Equals(""))
                {
                    model.sError = sMensajeError;
                    //No se graba ningun detalle para que este vacio
                    this.servicioTransfPotencia.DeleteVtpPeajeEgresoDetalle(pegrcodi);
                }
                else
                {
                    model.sError = "";
                    model.NumRegistros = NumRegistros;
                    model.sPlazo = model.Entidad.Pegrplazo;
                }

                return Json(model);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - GrabarGrillaExcel", ex);
                TransfPotencia.Models.PeajeEgresoModel model = new TransfPotencia.Models.PeajeEgresoModel();
                model.Pegrcodi = pegrcodi;
                model.sError = ex.Message; //"-1"
                model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
                return Json(model);
            }
        }

        /// <summary>
        /// Permite eliminar todos los registros del periodo y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatos(int pericodi, int recpotcodi, int emprcodi, int pegrcodi = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";
            try
            {
                TransfPotencia.Models.PeajeEgresoModel model = new TransfPotencia.Models.PeajeEgresoModel();

                //Graba Cabezera
                model.Entidad = new VtpPeajeEgresoDTO();
                model.Entidad.Pericodi = pericodi;
                model.Entidad.Recpotcodi = recpotcodi;
                model.Entidad.Emprcodi = emprcodi;
                model.Entidad.Pegrestado = "SI";
                model.Entidad.Pegrusucreacion = User.Identity.Name;

                model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
                //Antes de grabar cabezera actualiza los estados de "SI" a "NO"
                this.servicioTransfPotencia.UpdateByCriteriaVtpPeajeEgreso(emprcodi, pericodi, recpotcodi);

                //Graba nuevo
                pegrcodi = this.servicioTransfPotencia.SaveVtpPeajeEgreso(model.Entidad);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - EliminarDatos", ex);
                sResultado = ex.Message; //"-1";
            }

            return Json(sResultado);

        }


        /// Permite seleccionar a la empresa con la que desea trabajar
        [HttpPost]
        public ActionResult ListaEnvios(int pericodi = 0, int recpotcodi = 0, int emprcodi = 0)
        {
            base.ValidarSesionUsuario();
            TransfPotencia.Models.PeajeEgresoModel model = new TransfPotencia.Models.PeajeEgresoModel();
            model.ListaPeajeEgreso = this.servicioTransfPotencia.ListVtpPeajeEgresosView(emprcodi, pericodi, recpotcodi);
            model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            return PartialView(model);
        }


        /// <summary>
        /// Permite listas las validaciones después del envío
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        public ActionResult ListaValidaciones(int pegrcodi = 0)
        {
            base.ValidarSesionUsuario();
            ValidacionEnvioModel model = new ValidacionEnvioModel();
            model.ListaValidacionEnvio = this.servicioTransfPotencia.GetValidacionEnvioByPegrcodi(pegrcodi);
            List<VtpValidacionEnvioDTO> lstVal = model.ListaValidacionEnvio.Where(x => x.VaenTipoValidacion == "4").ToList();
            List<VtpValidacionEnvioDTO> lstValPeaje = model.ListaValidacionEnvio.Where(x => x.VaenTipoValidacion == "5").ToList();
            model.PrecioPotenciaRevision = lstVal.Count > 0 ? Convert.ToDecimal(lstVal[0].VaenValorCoes) : 0;
            model.PeajeUnitarioRegulado = lstValPeaje.Count > 0 ? Convert.ToDecimal(lstValPeaje[0].VaenValorCoes) : 0;
            if (model.ListaValidacionEnvio != null)
            {
                model.Historica = model.ListaValidacionEnvio.Exists(x => x.VaenTipoValidacion == "1");
                model.EnergiaActiva = model.ListaValidacionEnvio.Exists(x => x.VaenTipoValidacion == "2");
                model.FactorPerdida = model.ListaValidacionEnvio.Exists(x => x.VaenTipoValidacion == "3");
                model.PrecioPotencia = model.ListaValidacionEnvio.Exists(x => x.VaenTipoValidacion == "4");
                model.PeajeUnitario = model.ListaValidacionEnvio.Exists(x => x.VaenTipoValidacion == "5");
            }
            model.ListaValidacionEnvio = model.ListaValidacionEnvio.OrderBy(x => x.VaenCodi).ToList();
            for (int i = 0; i < model.ListaValidacionEnvio.Count; i++)
            {
                if (model.ListaValidacionEnvio[i].VaenTipoValidacion == "2")
                {
                    var index = model.ListaValidacionEnvio.FindIndex(item => item.PegrdCodi == model.ListaValidacionEnvio[i].PegrdCodi && item.VaenTipoValidacion == "2");
                    var listVTEA = model.ListaValidacionEnvio.Where(m => m.PegrdCodi == model.ListaValidacionEnvio[i].PegrdCodi && m.VaenTipoValidacion == "2")
                        .Select(m => new VtpValidacionEnvioDTO
                        {
                            VaenCodVtea = m.VaenCodVtea
                        }).ToList();

                    var distinct = listVTEA.Select(x => x.VaenCodVtea).Distinct().ToList();
                    if (distinct.Count > 1)
                    {
                        var validVTEA = model.ListaValidacionEnvio.Where(m => m.PegrdCodi == model.ListaValidacionEnvio[i].PegrdCodi && m.VaenTipoValidacion == "2");
                        foreach (var cust in validVTEA)
                        {
                            cust.VaenValorAgrupamiento = 2;
                        }
                    }

                    model.ListaValidacionEnvio[index].VaenValorAgrupamiento = distinct.Count > 1 ? 3 : 1;


                }
            }
            //var item3 = model.ListaValidacionEnvio[0];
            //foreach (var item2 in model.ListaValidacionEnvio)
            //{
            //    if(item2.VaenTipoValidacion == "2")
            //    {
            //        if(item2.VaenValorCoes == 2)
            //        {
            //            if(item3.VaenCodVtea == item2.VaenCodVtea)
            //            {
            //                item2.VaenValorReportado = 1;
            //            }else
            //            {
            //                item3 = item2;
            //            }
            //        }
            //    }
            //}

            return PartialView(model);
        }

        /// <summary>
        /// Permite exportar a un archivo todas las validaciones
        /// </summary>
        /// <param name="pegrcodi">Código del Envío</param>
        /// <param name="formato">Formato del Archivo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDataValidaciones(int pericodi = 0, int recpotcodi = 0, int pegrcodi = 0, int emprcodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string file = "";
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();

                EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByIdEmpresa(emprcodi);
                if (dtoEmpresa != null)
                {
                    file = this.servicioTransfPotencia.GenerarFormatoValidacionesEnvio(pericodi, recpotcodi, pegrcodi, dtoEmpresa.EmprNombre, formato, pathFile, pathLogo);

                }
                else
                {
                    file = "";
                }
                return Json(file);

            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - ExportarDataValidaciones", ex);
                return Json(ex.Message);
            }
        }

    }
}