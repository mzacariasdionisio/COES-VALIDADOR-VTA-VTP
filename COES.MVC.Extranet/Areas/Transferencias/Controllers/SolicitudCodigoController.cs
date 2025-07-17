using COES.Dominio.DTO.Enum;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.Transferencias.Helper;
using COES.MVC.Extranet.Areas.Transferencias.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using log4net;
using COES.Servicios.Aplicacion.Factory;

namespace COES.MVC.Extranet.Areas.Transferencias.Controllers
{
    public class SolicitudCodigoController : BaseController
    {
        private static string NombreControlador = "SolicitudCodigoController";
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EnvioInformacionController));

        GestionarCodigosAgrupadosHelper objGenerarCodigoAgrupado = null;
        BarraAppServicio servicioBarra = new BarraAppServicio();
        TransferenciasAppServicio servicioTransferencia = new TransferenciasAppServicio();
        SolicitudCodigoAppServicio servicioSolicitudCodigo = new SolicitudCodigoAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        EmpresaAppServicio servicio = new EmpresaAppServicio();
        // GET: Transferencias/SolicitudCodigo
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            List<SeguridadServicio.EmpresaDTO> listTotal = Funcion.ObtenerEmpresasPorUsuario(User.Identity.Name);
            List<SeguridadServicio.EmpresaDTO> list = new List<SeguridadServicio.EmpresaDTO>();

            //- aca debemos hacer jugada para escoger la empresa
            List<TrnInfoadicionalDTO> listaInfoAdicional = this.servicioTransferencia.ListTrnInfoadicionals();
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


            TempData["EMPRNRO"] = list.Count();
            if (list.Count() == 1)
            {
                TempData["EMPRNOMB"] = list[0].EMPRNOMB;
                Session["EmprNomb"] = list[0].EMPRNOMB;
                Session["EmprCodi"] = list[0].EMPRCODI;
            }
            else if (Session["EmprCodi"] != null)
            {
                int iEmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprCodi);
                TempData["EMPRNOMB"] = dtoEmpresa.EmprNombre;
                Session["EmprNomb"] = dtoEmpresa.EmprNombre;
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

            EmpresaModel modelCliente = new EmpresaModel();
            modelCliente.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCoReSoCli();

            BarraModel modelBarra = new BarraModel();
            modelBarra.ListaBarras = (new BarraAppServicio()).ListaInterCoReSo();

            TipoContratoModel modelTipoCont = new TipoContratoModel();
            modelTipoCont.ListaTipoContrato = (new TipoContratoAppServicio()).ListTipoContrato();

            TipoUsuarioModel modelTipoUsu = new TipoUsuarioModel();
            modelTipoUsu.ListaTipoUsuario = (new TipoUsuarioAppServicio()).ListTipoUsuario();


            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodoDeclaracion = (new PeriodoDeclaracionAppServicio()).GetListaCombobox();
            /*****************************************************************************************/
            var clicodi = Session["clicodi"];
            var pericodi = Session["pericodi"];
            /*****************************************************************************************/
            if (clicodi == null)
            {
                TempData["CLICODI2"] = new SelectList(modelCliente.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            }
            else
            {
                TempData["CLICODI2"] = new SelectList(modelCliente.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE", clicodi);
            }

            TempData["BARRCODI2"] = new SelectList(modelBarra.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");
            TempData["TIPOCONTCODI2"] = new SelectList(modelTipoCont.ListaTipoContrato, "TIPOCONTCODI", "TIPOCONTNOMBRE");
            TempData["TIPOUSUACODI2"] = new SelectList(modelTipoUsu.ListaTipoUsuario, "TIPOUSUACODI", "TIPOUSUANOMBRE");
            if (pericodi == null)
            {
                TempData["PERIODO2"] = new SelectList(modelPeriodo.ListaPeriodoDeclaracion, "PeridcCodi", "PeridcNombre", modelPeriodo.ListaPeriodoDeclaracion.FirstOrDefault().PeridcCodi);
            }
            else
            {
                TempData["PERIODO2"] = new SelectList(modelPeriodo.ListaPeriodoDeclaracion, "PeridcCodi", "PeridcNombre", pericodi);
            }
            TempData["DETALLE"] = "";
            Session["ELIMINAR"] = "";
            Session["ListarCodigoRetiro"] = null;

            return View();
        }

        /// Permite cargar los la lista
        //POST
        [HttpPost]
        public ActionResult Lista(int? clicodi, string pericodiNombre, string nombre, string tipoUsu, string tipoCont, string barrTran, string cliNomb, int? pericodi, string fechaInicio, string fechaFin, int nroPagina, string nombreArchivo, string base64)
        {
            Session["clicodi"] = clicodi;
            Session["pericodi"] = pericodi;

            ResultadoDTO<List<SolicitudCodigoDetalleDTO>> resultadoExcel = null;

            if (string.IsNullOrEmpty(base.UserName))
            {
                resultadoExcel = new ResultadoDTO<List<SolicitudCodigoDetalleDTO>>();
                resultadoExcel.EsCorrecto = -10;
                resultadoExcel.Mensaje = "Se ha perdido la sesión, por favor inicie sesión nuevamente";

                return Json(resultadoExcel);
            }

            if (Session["EmprCodi"] != null)
            {
                int iEmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprCodi);
                nombre = dtoEmpresa.EmprNombre;

                if (tipoUsu.Equals("--Seleccione--"))
                    tipoUsu = null;
                if (tipoCont.Equals("--Seleccione--"))
                    tipoCont = null;
                if (barrTran.Equals("--Seleccione--"))
                    barrTran = null;
                if (cliNomb.Equals("--Seleccione--"))
                    cliNomb = null;

                DateTime? dtfi = null;
                if (string.IsNullOrEmpty(fechaInicio))
                {
                    dtfi = null;
                }
                else
                {
                    dtfi = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaInicio);
                }

                DateTime? dtff = null;
                if (string.IsNullOrEmpty(fechaFin))
                {

                    dtff = null;
                }
                else
                {
                    dtff = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaFin);
                }

                SolicitudCodigoModel model = new SolicitudCodigoModel();


                List<SolicitudCodigoDTO> listaSolicitud = new List<SolicitudCodigoDTO>();
                string Solicodiretiobservacion = null;
                string estado = null;

                if (base64 != null)
                {
                    #region valida formatos iguales

                    //listaSolicitud = this.servicioSolicitudCodigo.ListarExportacionCodigoRetiro(nombre, tipoUsu, tipoCont, barrTran, cliNomb, dtfi, dtff, null, null, pericodi, nroPagina
                    //                  , int.MaxValue).ToList();
                    listaSolicitud = this.servicioSolicitudCodigo.ListarCodigoRetiro(nombre, tipoUsu, tipoCont, barrTran, cliNomb, dtfi, dtff, Solicodiretiobservacion, estado, pericodi, nroPagina, int.MaxValue)
                                                            .OrderBy(o => o.EmprNombre)
                                                            .OrderBy(o => o.CliNombre)
                                                            .OrderBy(o => o.FechaInicio)
                                                            .ToList();
                    //model.ListaCodigoRetiro = this.generarAgrupacion(listaSolicitud, null);

                    var codigoRetirosAuxiliar = this.generarAgrupacion(listaSolicitud, null);


                    #region "Agrupaciones"
                    int IndiceGrupo = 1;
                    foreach (var item in codigoRetirosAuxiliar)
                    {
                        int IndexDetalle = -1;
                        foreach (var detalle in item.ListaCodigoRetiroDetalle)
                        {
                            IndexDetalle++;
                            if (item.OmitirFilaVTA.Equals(0))
                            {
                                item.EsAgrupado = 0;
                                item.IndiceGrupo = IndiceGrupo;
                                if (detalle.PotenciaContratadaDTO.TipoAgrupacion != null
                                    && detalle.PotenciaContratadaDTO.TipoAgrupacion.Equals("AGRVTP"))
                                {
                                    if (item.ListaCodigoRetiroDetalle.Count > 1)
                                    {
                                        var existeVTP = item.ListaCodigoRetiroDetalle.FirstOrDefault();
                                        if (existeVTP != null)
                                        {
                                            if (item.EsAgrupado.Equals(0))
                                                item.EsAgrupado = 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //No omite fila, pero buscamos si ya tiene idParent
                                var existeParent = codigoRetirosAuxiliar.Where(x => x.SoliCodiRetiCodi == item.SoliCodiRetiCodiVTAParent)
                                                                        .FirstOrDefault();
                                if (existeParent != null)
                                {
                                    if (existeParent.EsAgrupado.Equals(0)) //Si el existe y no esta considerado como agrupado lo cambiamos a agrupado
                                        existeParent.EsAgrupado = 1;

                                    item.EsAgrupado = existeParent.EsAgrupado;
                                    item.IndiceGrupo = existeParent.IndiceGrupo;
                                }
                            }
                        }

                        IndiceGrupo++;
                    }

                    /*Agrupado por Estado : Pendiente | Activos - (Otros) */
                    codigoRetirosAuxiliar = codigoRetirosAuxiliar.OrderByDescending(p => p.abrevEstadoVTA.Equals("PAP") || p.abrevEstadoVTA.Equals("PVT"))
                                                                 .ThenByDescending(p => p.abrevEstadoVTA.Equals("PAP") || p.abrevEstadoVTA.Equals("PVT"))
                                                                 .ThenByDescending(p => p.abrevEstadoVTA.Equals("ACT"))
                                                                 .ThenByDescending(p => p.abrevEstadoVTA.Equals("BAJ"))
                                                                 .ThenByDescending(p => p.abrevEstadoVTA.Equals("REC"))
                                                                 .ThenByDescending(p => p.abrevEstadoVTA.Equals("SBJ"))
                                                                 /*Desagrupados(0) y Agrupados(1)*/
                                                                 .ThenBy(p => p.EmprNombre)
                                                                 .ThenBy(p => p.CliNombre)
                                                                 .ThenBy(p => p.FechaInicio)
                                                                 .ThenByDescending(p => p.EsAgrupado.Equals(0))
                                                                 .ThenByDescending(p => p.EsAgrupado.Equals(1))
                                                                 .ToList();

                    #endregion "Agrupaciones"

                    //model.ListaCodigoRetiro = this.generarAgrupacion(listaSolicitud, null);
                    model.ListaCodigoRetiro = codigoRetirosAuxiliar;
                    string base64Comparar = this.servicioSolicitudCodigo.ExportarDatosCodigoRetiro(nombre,
                    model.ListaCodigoRetiro);
                    #endregion valida formatos iguales

                    resultadoExcel = this.servicioSolicitudCodigo.ListarCodigoRetiroFromExcel(base64, base64Comparar, 1);
                    if (resultadoExcel.EsCorrecto == -1)
                        model.sError = resultadoExcel.Mensaje;
                    if (resultadoExcel.EsCorrecto == -2)
                        return Json(resultadoExcel);

                }


                //if (base64 != null)
                //{
                //   List<SolicitudCodigoDTO> paramSolicitud = this.servicioSolicitudCodigo.ListarCodigoRetiro(nombre, tipoUsu, tipoCont, barrTran, cliNomb, dtfi, dtff, null, null, pericodi, nroPagina
                //, int.MaxValue);

                //    List<SolicitudCodigoDTO> paramSolicitud = Session["TMP_ARCHIVO"] as List<SolicitudCodigoDTO>;

                //    List<SolicitudCodigoDTO> objSolicitudes = this.generarAgrupacion(paramSolicitud, null);

                //    string base64Comparar = this.servicioSolicitudCodigo.ExportarDatosCodigoRetiro(nombre, objSolicitudes.Where(x => (x.abrevEstadoVTA == "ACT" || x.abrevEstadoVTA == "PAP")
                //&& (x.abrevEstadoVTP == "ACT" || x.abrevEstadoVTP == "PAP" || string.IsNullOrEmpty(x.abrevEstadoVTP))).ToList());

                //    ResultadoDTO<List<SolicitudCodigoDetalleDTO>> resultadoComparar = this.servicioSolicitudCodigo.ListarCodigoRetiroFromExcel(base64Comparar, 1);

                //    string jsonSerializeActual = new JavaScriptSerializer().Serialize(resultadoExcel.Data);
                //    string jsonSerializeComparar = new JavaScriptSerializer().Serialize(resultadoComparar.Data);
                //    if (!jsonSerializeActual.Contains(jsonSerializeComparar))
                //        return Json(new { Mensaje = "El archivo adjuntado no coincide con la información en la grilla." });
                //}

                model.Entidad = new SolicitudCodigoDTO();

                model.Entidad.PeridcCodi = (int)pericodi;
                model.Entidad.PeridcNombre = pericodiNombre;



               
                List<SolicitudCodigoPotenciaContratadaDTO> listaPotencias = new List<SolicitudCodigoPotenciaContratadaDTO>();
                listaSolicitud = new List<SolicitudCodigoDTO>();
                //if (Session["ListarCodigoRetiro"] == null)
                var resultado = this.servicioSolicitudCodigo.ListarCodigoRetiro(nombre, tipoUsu, tipoCont, barrTran, cliNomb, dtfi, dtff, Solicodiretiobservacion, estado, pericodi, nroPagina
                    , int.MaxValue);
                listaSolicitud = resultado;

                ////========================================================================================================
                //objGenerarCodigoAgrupado = new GestionarCodigosAgrupadosHelper(listaSolicitud);
                ////-========================================================================================================

                var resultadoAgrupado = this.generarAgrupacion(listaSolicitud, resultadoExcel);
                model.ListaCodigoRetiro = resultadoAgrupado;

                return PartialView(model);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult ListarEnvios(int periCodi)
        {
            SolicitudCodigoModel model = new SolicitudCodigoModel();
            model.ListarEnvios = new TransferenciasAppServicio().ListarEnvios(periCodi);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ListarPotenciasContratadas(int coresocodi, int periCodi)
        {
            List<TrnPotenciaContratadaDTO> resultado = new SolicitudCodigoAppServicio().ListaPotenciaContratadas(coresocodi, periCodi);
            return Json(resultado);

        }
        [HttpPost]
        public PartialViewResult Paginado(string nombre, string tipoUsu, string tipoCont, string barTran, string cliNomb, int? pericodi, string fechaInicio, string fechaFin)
        {
            if (tipoUsu.Equals("--Seleccione--"))
                tipoUsu = null;
            if (tipoCont.Equals("--Seleccione--"))
                tipoCont = null;
            if (barTran.Equals("--Seleccione--"))
                barTran = null;
            if (cliNomb.Equals("--Seleccione--"))
                cliNomb = null;

            DateTime? dtfi = null;
            if (string.IsNullOrEmpty(fechaInicio))
            {
                dtfi = null;
            }
            else
            {
                dtfi = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaInicio);
            }

            DateTime? dtff = null;
            if (string.IsNullOrEmpty(fechaFin))
            {

                dtff = null;
            }
            else
            {
                dtff = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaFin);
            }
            SolicitudCodigoModel model = new SolicitudCodigoModel();
            model.IndicadorPagina = false;

            if (Session["EmprCodi"] != null)
            {
                int iEmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprCodi);
                nombre = dtoEmpresa.EmprNombre;

                string Solicodiretiobservacion = null;
                string estado = null;

                List<SolicitudCodigoDTO> listaSolicitud = new List<SolicitudCodigoDTO>();


                model.NroRegistros = this.servicioSolicitudCodigo.ObtenerNroFilasCodigoRetiro(nombre, tipoUsu, tipoCont, barTran, cliNomb, dtfi, dtff, Solicodiretiobservacion, estado, pericodi);

                if (model.NroRegistros > Funcion.NroPageShow)
                {
                    int pageSize = Funcion.PageSizeCodigoEntrega;
                    int nroPaginas = (model.NroRegistros % pageSize == 0) ? model.NroRegistros / pageSize : model.NroRegistros / pageSize + 1;
                    model.NroPaginas = nroPaginas;
                    model.NroMostrar = Funcion.NroPageShow;
                    model.IndicadorPagina = true;
                }

                return PartialView(model);
            }
            return PartialView(null);
        }

        /// Permite seleccionar a la empresa con la que desea trabajar
        [HttpPost]
        public ActionResult EscogerEmpresa()
        {
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            List<TrnInfoadicionalDTO> listaInfoAdicional = this.servicioTransferencia.ListTrnInfoadicionals();
            List<int> idsEmpresas = listaInfoAdicional.Where(x => x.Emprcodi != null).Select(x => (int)x.Emprcodi).Distinct().ToList();

            List<SeguridadServicio.EmpresaDTO> list = Funcion.ObtenerEmpresasPorUsuario(User.Identity.Name, idsEmpresas);

            EmpresaModel model = new EmpresaModel();
            List<EmpresaDTO> lista = new List<EmpresaDTO>();
            foreach (var item in list)
            {
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(item.EMPRCODI);
                lista.Add(dtoEmpresa);

                //ASSETEC 20190111 - comparar contra otros conceptos
                Int16 iEmpcodi = Convert.ToInt16(item.EMPRCODI);
                foreach (TrnInfoadicionalDTO dtoOtroConcepto in listaInfoAdicional)
                {
                    Int16 iOtroConcepto = Convert.ToInt16(dtoOtroConcepto.Emprcodi);
                    if (iEmpcodi == iOtroConcepto)
                    {
                        EmpresaDTO dtoEmpresaConcepto = new EmpresaDTO();
                        dtoEmpresaConcepto.EmprCodi = Convert.ToInt16(dtoOtroConcepto.Infadicodi);
                        dtoEmpresaConcepto.EmprNombre = dtoOtroConcepto.Infadinomb;
                        dtoEmpresaConcepto.TipoEmprCodi = Convert.ToInt16(dtoOtroConcepto.Tipoemprcodi);
                        lista.Add(dtoEmpresaConcepto);
                    }
                }
                //--------------------------------------------------
            }

            model.ListaEmpresas = lista.OrderBy(x => x.EmprNombre).ToList();
            return PartialView(model);
        }

        /// Permite actualizar o grabar un registro
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmpresaElegida(int EmprCodi)
        {
            //ASSETEC 20190111
            if (EmprCodi != 0)
            {
                Session["EmprCodi"] = EmprCodi;
            }
            return RedirectToAction("Index");
        }

        /// Permite mostrar el formulario para un nuevo registro
        public ActionResult New(int peridcCodi, string peridcNombre)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            SolicitudCodigoModel modelo = new SolicitudCodigoModel();
            modelo.Entidad = new SolicitudCodigoDTO();
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }

            modelo.Entidad.PeridcCodi = peridcCodi;
            modelo.Entidad.PeridcNombre = peridcNombre;
            modelo.Entidad.SoliCodiRetiCodi = 0;
            modelo.Entidad.SoliCodiRetiCodi = 0;
            //modelo.Solicodiretifechainicio = DateTime.Now.ToString("dd/MM/yyyy");
            modelo.Solicodiretifechafin = DateTime.Now.ToString("dd/MM/yyyy");

            EmpresaModel modelCliente = new EmpresaModel();
            modelCliente.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            TempData["CLICODI2"] = modelCliente;

            BarraModel modelBarra = new BarraModel();

            modelBarra.ListaBarras = this.servicioBarra.ListaBarraTransferencia();

            TempData["BARRCODI2"] = modelBarra;

            TipoContratoModel modelTipoCont = new TipoContratoModel();
            modelTipoCont.ListaTipoContrato = (new TipoContratoAppServicio()).ListTipoContrato();
            TempData["TIPOCONTCODI2"] = new SelectList(modelTipoCont.ListaTipoContrato, "TIPOCONTCODI", "TIPOCONTNOMBRE");
            TipoUsuarioModel modelTipoUsu = new TipoUsuarioModel();
            modelTipoUsu.ListaTipoUsuario = (new TipoUsuarioAppServicio()).ListTipoUsuario();
            TempData["TIPOUSUACODI2"] = new SelectList(modelTipoUsu.ListaTipoUsuario, "TIPOUSUACODI", "TIPOUSUANOMBRE");
            TempData["EMPRNOMB"] = Session["EmprNomb"];

            BarraModel modelBarraSum = new BarraModel();
            modelBarraSum.ListaBarras = this.servicioBarra.ListBarras();
            TempData["BARRCODISUM"] = modelBarraSum;

            PeriodoDeclaracionDTO objPeriodo = new PeriodoDeclaracionAppServicio().GetBydId(peridcCodi);
            modelo.Entidad.PeridcEstado = objPeriodo.PeridcEstado;

            string dtFechaInicio = (objPeriodo.PeridcMes < 10 ? "0" + objPeriodo.PeridcMes : objPeriodo.PeridcMes.ToString()) + "/01/" + objPeriodo.PeridcAnio;
            modelo.Solicodiretifechainicio = Convert.ToDateTime(dtFechaInicio).ToString("dd/MM/yyyy");

            modelo.SolicodiretifechainicioValida = Convert.ToDateTime(dtFechaInicio).ToString("dd/MM/yyyy");

            return PartialView(modelo);
        }

        /// Permite listar las barras de suministro por barra de trasferencia seleccionada
        [HttpPost]
        public ActionResult ListaSuministro(string bartran, string barsum, string nombarrsum, string tipo, string idGenerado)
        {
            int idbarr = (string.IsNullOrEmpty(bartran)) ? 0 : Convert.ToInt32(bartran);
            int idbarrsum = (string.IsNullOrEmpty(barsum)) ? 0 : Convert.ToInt32(barsum);
            SolicitudCodigoModel model = new SolicitudCodigoModel();
            model.ListaRelacionBarras = TempData["DETALLE"] as List<BarraRelacionDTO>;

            //A: agrega relación de barras
            if (tipo == "A")
            {
                if (model.ListaRelacionBarras != null)
                {
                    bool existe = model.ListaRelacionBarras.Exists(x => x.BareBarrCodiSum == idbarrsum);
                    if (!existe)
                    {
                        BarraRelacionDTO oBarRel = new BarraRelacionDTO();
                        oBarRel.BareBarrCodiTra = idbarr;
                        oBarRel.BareBarrCodiSum = idbarrsum;
                        oBarRel.BarrNombSum = nombarrsum;
                        oBarRel.BareEstado = Funcion.EstadoActivo;
                        oBarRel.IdGenerado = idGenerado;
                        model.ListaRelacionBarras.Add(oBarRel);
                    }
                }


            }
            //D: eliminar relación de barras
            else if (tipo == "D")
            {
                if (model.ListaRelacionBarras != null)
                {
                    BarraRelacionDTO obj = new BarraRelacionDTO();
                    if (idGenerado != "")
                        obj = model.ListaRelacionBarras.Where(x => x.IdGenerado == idGenerado).FirstOrDefault();
                    else
                        obj = model.ListaRelacionBarras.LastOrDefault(x => x.BareBarrCodiSum == idbarrsum);
                    if (obj != null)
                    {
                        model.ListaRelacionBarras.Remove(obj);
                    }
                }

            }
            //Quitar todas las barras de suministro
            else if (tipo == "B")
            {
                model.ListaRelacionBarras = new List<BarraRelacionDTO>();
            }
            //Listar relación de barras
            else
            {
                model.ListaRelacionBarras = this.servicioBarra.ListaRelacion(idbarr);
            }

            foreach (var item in model.ListaRelacionBarras)
            {
                item.NroRegistros = (item.NroRegistros == 0) ? 1 : item.NroRegistros;
            }
            TempData["DETALLE"] = model.ListaRelacionBarras;
            return PartialView(model);
        }


        //AQUI

        [HttpPost]
        public ActionResult ActualizarIdGenerado(List<BarraRelacionDTO> param)
        { 
            if (param != null)
            {
                SolicitudCodigoModel model = new SolicitudCodigoModel();
                model.ListaRelacionBarras = TempData["DETALLE"] as List<BarraRelacionDTO>;
                foreach (var item in param.Where(x => x.NroRegistros >= 0))
                {
                    var findEntidad = model.ListaRelacionBarras.Find(x => x.BareBarrCodiSum == item.BareCodi);
                    findEntidad.NroRegistros = item.NroRegistros;
                    findEntidad.IdGenerado = item.IdGenerado;
                }

                TempData["DETALLE"] = model.ListaRelacionBarras;

            }

            return Json(1);
        }


        [HttpPost]
        public ActionResult ActualizarIdGeneradoEdit(List<BarraRelacionDTO> param)
        {

            if (param != null)
            {
                SolicitudCodigoModel model = new SolicitudCodigoModel();
                model.ListaCodigoRetiroDetalle = TempData["DETALLE"] as List<SolicitudCodigoDetalleDTO>;
                foreach (var item in param)
                {
                    var findEntidad = model.ListaCodigoRetiroDetalle[item.IndexLista];
                    findEntidad.IdGenerado = item.IdGenerado;
                }
                TempData["DETALLE"] = model.ListaCodigoRetiroDetalle;

            }

            return Json(1);
        }



        /// Permite actualizar o grabar un registro
        //POST
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Save(SolicitudCodigoModel modelo)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            if (ModelState.IsValid)
            {
                if (modelo.Entidad.SoliCodiRetiObservacion != null)
                {
                    modelo.Entidad.SoliCodiRetiFechaRegistro = DateTime.Now;
                    modelo.Entidad.SoliCodiRetiCodigo = null;
                    modelo.Entidad.SoliCodiRetiEstado = Funcion.EstadoPendiente;

                    DateTime dtFechaInicio = DateTime.ParseExact(modelo.Solicodiretifechainicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime dtFechaInicioValida = DateTime.ParseExact(modelo.SolicodiretifechainicioValida, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                    if (dtFechaInicio< dtFechaInicioValida)
                    {
                        modelo.sError = "Verificar que la fecha de inicio corresponda al periodo actual de declaración";
                        TempData["sMensajeValidacionFecha"] = "Verificar que la fecha de inicio corresponda al periodo actual de declaración";
                    } else
                    {
                        if (modelo.Solicodiretifechainicio != "" && modelo.Solicodiretifechainicio != null)
                            modelo.Entidad.SoliCodiRetiFechaInicio = DateTime.ParseExact(modelo.Solicodiretifechainicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        if (modelo.Solicodiretifechafin != "" && modelo.Solicodiretifechafin != null)
                            modelo.Entidad.SoliCodiRetiFechaFin = DateTime.ParseExact(modelo.Solicodiretifechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        //CAPTURAR DEL LOGUEO USUASEIN
                        modelo.Entidad.UsuaCodi = User.Identity.Name;
                        modelo.Entidad.UserEmail = this.seguridad.ObtenerUsuarioPorLogin(User.Identity.Name).UserEmail;
                        if (Session["EmprCodi"] != null)
                        {
                            modelo.Entidad.EmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());

                            string strDetalle = modelo.Entidad.SoliCodiRetiObservacion;
                            string[] aDetalle = strDetalle.Split(',');

                            List<SolicitudCodigoDetalleDTO> lSolDetalle = new List<SolicitudCodigoDetalleDTO>();
                            for (int i = 0; i < aDetalle.Length; i++)
                            {
                                SolicitudCodigoDetalleDTO oSolDetalle = new SolicitudCodigoDetalleDTO();
                                oSolDetalle.Barracodisum = Convert.ToInt32(aDetalle[i].Split('_')[0].ToString());
                                oSolDetalle.Coresdnumregistro = Convert.ToInt32(aDetalle[i].Split('_')[1].ToString());
                                oSolDetalle.Barracoditra = modelo.Entidad.BarrCodi;
                                oSolDetalle.Codigovtp = null;
                                oSolDetalle.Coresdusuarioregistro = modelo.Entidad.UsuaCodi;
                                lSolDetalle.Add(oSolDetalle);
                            }

                            if (modelo.Entidad.TrnpcTipoCasoAgrupado == "AGRVTP")
                                modelo.Entidad.ListaPotenciaContratadas = new JavaScriptSerializer().Deserialize<List<TrnPotenciaContratadaDTO>>(modelo.ListaPotenciasContratadasVTP);

                            modelo.ListaCodigoRetiroDetalle = lSolDetalle;
                            modelo.Entidad.SoliCodiRetiObservacion = "SOLBAJANO";

                            int id = modelo.Entidad.SoliCodiRetiCodi;
                            //dasaji*
                            modelo.IdcodRetiro = this.servicioSolicitudCodigo.SaveOrUpdateCodigoRetiro(modelo.Entidad, modelo.ListaCodigoRetiroDetalle);
                            if (id == 0)
                            {

                                #region AuditoriaProceso
                                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion;
                                objAuditoria.Estdcodi = (int)EVtpEstados.Pendiente;
                                objAuditoria.Audproproceso = "Gestion de codigos - Extranet";
                                objAuditoria.Audprodescripcion = "Se registro correctamente la solicitud Nro." + modelo.IdcodRetiro;
                                objAuditoria.Audprousucreacion = base.UserName;
                                objAuditoria.Audprofeccreacion = DateTime.Now;
                                new AuditoriaProcesoAppServicio().save(objAuditoria);
                                #endregion
                            }
                            TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                        }
                        else
                            TempData["sMensajeExito"] = "Lo sentimos, no se ha procesado la información, favor de seleccionar una empresa primero";
                    }

                    
                    if (modelo.IdcodRetiro > 0)
                        return RedirectToAction("Index");
                }

            }
            //Error
            modelo.sError = "No se ha podido almacenar correctamente la información, favor de verificar los datos registrados";

            EmpresaModel modelCliente = new EmpresaModel();
            modelCliente.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            TempData["CLICODI2"] = modelCliente;


            BarraModel modelBarraSum = new BarraModel();
            modelBarraSum.ListaBarras = this.servicioBarra.ListBarras();
            TempData["BARRCODI2"] = modelBarraSum;

            BarraModel modelBarraSum2 = new BarraModel();
            modelBarraSum2.ListaBarras = this.servicioBarra.ListBarras();
            TempData["BARRCODISUM"] = modelBarraSum2;

            TipoContratoModel modelTipoCont = new TipoContratoModel();
            modelTipoCont.ListaTipoContrato = (new TipoContratoAppServicio()).ListTipoContrato();
            TempData["TIPOCONTCODI2"] = new SelectList(modelTipoCont.ListaTipoContrato, "TIPOCONTCODI", "TIPOCONTNOMBRE");
            TipoUsuarioModel modelTipoUsu = new TipoUsuarioModel();
            modelTipoUsu.ListaTipoUsuario = (new TipoUsuarioAppServicio()).ListTipoUsuario();
            TempData["TIPOUSUACODI2"] = new SelectList(modelTipoUsu.ListaTipoUsuario, "TIPOUSUACODI", "TIPOUSUANOMBRE");
            TempData["EMPRNOMB"] = Session["EmprNomb"];


            return PartialView(modelo);
        }

        [HttpPost]
        public ActionResult SaveAgruparGrilla(List<TrnPotenciaContratadaDTO> parametro)
        {

            ResultadoDTO<int> resultado = new ResultadoDTO<int>();

            if (string.IsNullOrEmpty(base.UserName))
            {
                resultado.EsCorrecto = -10;
                resultado.Mensaje = "Se ha perdido la sesión, por favor inicie sesión nuevamente";
            }
            else
                resultado = new SolicitudCodigoAppServicio().GenerarPotenciasAgrupadas(base.UserName, parametro);

            //ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            //objGenerarCodigoAgrupado = new GestionarCodigosAgrupadosHelper(Session["ListarCodigoRetiro"] as List<SolicitudCodigoDTO>);
            //Session["ListarCodigoRetiro"] = objGenerarCodigoAgrupado.AgruparRegistros(User.Identity.Name, 0, parametro);
            return Json(resultado);
        }

        [HttpPost]
        public ActionResult DesagruparPotencias(List<TrnPotenciaContratadaDTO> parametro)
        {
            ResultadoDTO<int> resultado = new SolicitudCodigoAppServicio().DesagruparPotencias(base.UserName, parametro);


            return Json(resultado);
        }

        [HttpPost]
        public ActionResult EnviarDatos(List<TrnPotenciaContratadaDTO> parametro)
        {
            string userName = User.Identity.Name;

            ResultadoDTO<int> resultado = new SolicitudCodigoAppServicio().GenerarCargaDatosExcel(userName, parametro);


            return Json(resultado);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="tipoUsu"></param>
        /// <param name="tipoCont"></param>
        /// <param name="barrTran"></param>
        /// <param name="cliNomb"></param>
        /// <param name="pericodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportarInformacion(string nombre, string tipoUsu, string tipoCont, string barrTran, string cliNomb, int? pericodi, string fechaInicio, string fechaFin, int nroPagina)
        {
            ResultadoDTO<ArchivoBaseDTO> resultado = new ResultadoDTO<ArchivoBaseDTO>();
            if (string.IsNullOrEmpty(base.UserName))
            {
                resultado.EsCorrecto = -10;
                resultado.Mensaje = "Se ha perdido la sesión, por favor inicie sesión nuevamente";
                return Json(resultado);
            }

            if (tipoUsu.Equals("--Seleccione--"))
                tipoUsu = null;
            if (tipoCont.Equals("--Seleccione--"))
                tipoCont = null;
            if (barrTran.Equals("--Seleccione--"))
                barrTran = null;
            if (cliNomb.Equals("--Seleccione--"))
                cliNomb = null;

            DateTime? dtfi = null;
            if (string.IsNullOrEmpty(fechaInicio))
            {
                dtfi = null;
            }
            else
            {
                dtfi = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaInicio);
            }

            DateTime? dtff = null;
            if (string.IsNullOrEmpty(fechaFin))
            {

                dtff = null;
            }
            else
            {
                dtff = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(fechaFin);
            }
            SolicitudCodigoModel model = new SolicitudCodigoModel();

            if (Session["EmprCodi"] != null)
            {
                int iEmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprCodi);
                nombre = dtoEmpresa.EmprNombre;

                string Solicodiretiobservacion = null;
                string estado = null;

                List<SolicitudCodigoPotenciaContratadaDTO> listaPotencias = new List<SolicitudCodigoPotenciaContratadaDTO>();
                List<SolicitudCodigoDTO> listaSolicitud = new List<SolicitudCodigoDTO>();

                listaSolicitud = this.servicioSolicitudCodigo.ListarCodigoRetiro(nombre, tipoUsu, tipoCont, barrTran, cliNomb, dtfi, dtff, Solicodiretiobservacion, estado, pericodi, nroPagina, int.MaxValue)
                                                             .OrderBy(o => o.EmprNombre)
                                                             .OrderBy(o => o.CliNombre)
                                                             .OrderBy(o => o.FechaInicio)
                                                             .ToList();                

                //listaSolicitud = this.servicioSolicitudCodigo.ListarExportacionCodigoRetiro(nombre, tipoUsu, tipoCont, barrTran, cliNomb, dtfi, dtff, null, null, pericodi, nroPagina, int.MaxValue)
                //                                             //.OrderBy(o => o.EmprNombre)
                //                                             //.OrderBy(o => o.CliNombre)
                //                                             //.OrderBy(o => o.FechaInicio)
                //                                             .ToList();
                
                var codigoRetirosAuxiliar = this.generarAgrupacion(listaSolicitud, null);
                

                #region "Agrupaciones"
                int IndiceGrupo = 1;              
                foreach (var item in codigoRetirosAuxiliar)
                {
                    int IndexDetalle = -1;
                    foreach (var detalle in item.ListaCodigoRetiroDetalle)
                    {
                        IndexDetalle++;
                        if (item.OmitirFilaVTA.Equals(0))
                        {
                            item.EsAgrupado = 0;                            
                            item.IndiceGrupo = IndiceGrupo;
                            if (detalle.PotenciaContratadaDTO.TipoAgrupacion != null 
                                && detalle.PotenciaContratadaDTO.TipoAgrupacion.Equals("AGRVTP"))
                            {
                                if (item.ListaCodigoRetiroDetalle.Count > 1)
                                {
                                    var existeVTP = item.ListaCodigoRetiroDetalle.FirstOrDefault();
                                    if (existeVTP != null)
                                    {
                                        if (item.EsAgrupado.Equals(0))
                                            item.EsAgrupado = 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //No omite fila, pero buscamos si ya tiene idParent
                            var existeParent = codigoRetirosAuxiliar.Where(x => x.SoliCodiRetiCodi == item.SoliCodiRetiCodiVTAParent)
                                                                    .FirstOrDefault();
                            if (existeParent != null)
                            {
                                if (existeParent.EsAgrupado.Equals(0)) //Si el existe y no esta considerado como agrupado lo cambiamos a agrupado
                                    existeParent.EsAgrupado = 1;

                                item.EsAgrupado = existeParent.EsAgrupado;
                                item.IndiceGrupo = existeParent.IndiceGrupo;
                            }
                        }
                    }

                    IndiceGrupo++;
                }

                /*Agrupado por Estado : Pendiente | Activos - (Otros) */
                codigoRetirosAuxiliar = codigoRetirosAuxiliar.OrderByDescending(p => p.abrevEstadoVTA.Equals("PAP") || p.abrevEstadoVTA.Equals("PVT"))
                                                             .ThenByDescending(p => p.abrevEstadoVTA.Equals("PAP") || p.abrevEstadoVTA.Equals("PVT"))
                                                             .ThenByDescending(p => p.abrevEstadoVTA.Equals("ACT"))
                                                             .ThenByDescending(p => p.abrevEstadoVTA.Equals("BAJ"))
                                                             .ThenByDescending(p => p.abrevEstadoVTA.Equals("REC"))
                                                             .ThenByDescending(p => p.abrevEstadoVTA.Equals("SBJ"))
                                                             /*Desagrupados(0) y Agrupados(1)*/
                                                             .ThenBy(p => p.EmprNombre)
                                                             .ThenBy(p => p.CliNombre)
                                                             .ThenBy(p => p.FechaInicio)
                                                             .ThenByDescending(p => p.EsAgrupado.Equals(0))
                                                             .ThenByDescending(p => p.EsAgrupado.Equals(1))
                                                             .ToList();

                #endregion "Agrupaciones"

                model.ListaCodigoRetiro = codigoRetirosAuxiliar;

                PeriodoDeclaracionDTO objPeriodo = new PeriodoDeclaracionAppServicio().GetBydId((int)pericodi);

                Session["TMP_ARCHIVO"] = model.ListaCodigoRetiro;

                resultado.Data = new ArchivoBaseDTO();
                resultado.Data.archivoBase64 = this.servicioSolicitudCodigo.ExportarDatosCodigoRetiro(nombre, model.ListaCodigoRetiro);
                resultado.Data.nombreArchivo = string.Format("{0}{1}{2}{3}{4}_{5}.xlsx", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Second, nombre);

                #region AuditoriaProceso


                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion;
                objAuditoria.Estdcodi = (int)EVtpEstados.BajarFormato;
                objAuditoria.Audproproceso = "Exportar informacion de codigos.";
                objAuditoria.Audprodescripcion = "Se exporta data en excel del periodo " + objPeriodo.PeridcNombre + " - usuario " + User.Identity.Name;
                objAuditoria.Audprousucreacion = User.Identity.Name;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = new AuditoriaProcesoAppServicio().save(objAuditoria);
                if (auditoria == 0)
                {
                    Logger.Error(NombreControlador + " - Error Save AuditoriaA");
                }

                #endregion

            }
            return Json(resultado);
        }


        public ActionResult solicitudCliente(string empresaGeneradora)
        {
            ViewBag.empresaGeneradora = empresaGeneradora;
            return PartialView();
        }

        [HttpPost]
        public ActionResult solicitudCliente(SolicitudCodigoModel modelo)
        {
            bool resultado = new SolicitudCodigoAppServicio().EnviarCorreoNotificacionSolicitudCliente(modelo.RazonSocial, modelo.RucCliente, modelo.Comentario, modelo.EmpresaGeneradora);
            return Json(resultado);
        }

        /// <summary>
        /// Permite obtener los datos de SUNAT
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult obtenerdatos(string ruc)
        {
            object resultado;
            bool json = false;
            try
            {
                BeanEmpresa empresa = this.servicio.ConsultarPorRUC(ruc);
                if (empresa == null)
                    resultado = -2;
                else
                {
                    if (string.IsNullOrEmpty(empresa.RUC))
                    {
                        resultado = -2; //- RUC no Existe            
                    }
                    else
                    {
                        if (empresa.Estado.ToUpper().Trim() != "ACTIVO")
                            resultado = -3; //- RUC de Baja            
                        else
                        {
                            json = true;
                            resultado = empresa;
                        }
                    }
                }
                //json = false;
                //resultado = -1;
            }
            catch
            {
                resultado = -1; //- Error en el proceso
            }

            if (!json)
            {
                if ((int)resultado == -1 || (int)resultado == -2 || (int)resultado == -3)
                {
                    BeanEmpresa empresa = (new EmpresaAppServicio()).ConsultarPorRUC(ruc);
                    if (string.IsNullOrEmpty(empresa.RazonSocial))
                        resultado = -2;
                    else
                        resultado = empresa;

                }
            }

            return Json(resultado); //- RUC no Existe            
        }
        /// Permite  cargar la vista (detalle) de un registro
        public ActionResult View(int peridcCodi, string peridcNombre, int id = 0, int tipo = 0)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            SolicitudCodigoModel model = new SolicitudCodigoModel();

            if (id != 0)
            {
                model.Entidad = (new SolicitudCodigoAppServicio()).GetByIdCodigoRetiro(id, peridcCodi);
                if (model.Entidad.SoliCodiRetiCodigo == null)
                    model.Entidad.SoliCodiRetiCodigo = "PENDIENTE";

                if (model.Entidad.ListaPotenciaContratadas.Count == 0)
                {
                    model.Entidad.TrnpcTipoCasoAgrupado = "AGRVTA";
                }

                model.Entidad.PeridcCodi = peridcCodi;
                model.Entidad.PeridcNombre = peridcNombre;
                List<BarraDTO> modelBarraSum = new List<BarraDTO>();
                modelBarraSum = this.servicioBarra.ListBarras();
                TempData["BARRCODISUM"] = modelBarraSum;

                model.ListaCodigoRetiroDetalle = this.servicioSolicitudCodigo.ListarDetalle(id).OrderBy(x => x.Coregecodigo).ToList();
                TempData["DETALLE"] = model.ListaCodigoRetiroDetalle;
                TempData["EMPRNOMB"] = Session["EmprNomb"];
            }

            if (tipo == 1)
            {
                //Error
                model.sError = "No se ha podido almacenar correctamente la información, favor de verificar los datos registrados";
            }

            PeriodoDeclaracionDTO objPeriodo = new PeriodoDeclaracionAppServicio().GetBydId(peridcCodi);
            model.Entidad.PeridcEstado = objPeriodo.PeridcEstado;

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ListaDetalle(string idbarrsum, string nombarrsum, string numreg, string tipo, int? idcod, string idGenerado)
        {
            SolicitudCodigoModel model = new SolicitudCodigoModel();
            List<SolicitudCodigoDetalleDTO> lEliminados = new List<SolicitudCodigoDetalleDTO>();
            int id = (string.IsNullOrEmpty(idbarrsum)) ? 0 : Convert.ToInt32(idbarrsum);
            int nro = (string.IsNullOrEmpty(numreg)) ? 0 : Convert.ToInt32(numreg);

            model.ListaCodigoRetiroDetalle = TempData["DETALLE"] as List<SolicitudCodigoDetalleDTO>;
            //A: agregar a la lista de detalle en memoria
            if (tipo.Equals("A"))
            {
                SolicitudCodigoDetalleDTO oDetalle = new SolicitudCodigoDetalleDTO();
                oDetalle.Coresdcodi = 0;
                oDetalle.Coregecodigo = 0;
                oDetalle.Barracodisum = id;
                oDetalle.Barranomsum = nombarrsum;
                oDetalle.Codigovtp = "";
                oDetalle.IdGenerado = idGenerado;
                oDetalle.coregeestado = Funcion.EstadoPendiente;
                model.ListaCodigoRetiroDetalle.Add(oDetalle);
            }
            else //agreagr a la lista de eliminados en memoria
            {
                if ((Session["ELIMINAR"] as List<SolicitudCodigoDetalleDTO>) != null)
                {
                    lEliminados = Session["ELIMINAR"] as List<SolicitudCodigoDetalleDTO>;
                }

                SolicitudCodigoDetalleDTO obj = new SolicitudCodigoDetalleDTO();

                if (idcod == 0)
                {
                    obj = model.ListaCodigoRetiroDetalle.Where(x => x.IdGenerado == idGenerado).FirstOrDefault();
                }
                else
                {
                    obj = model.ListaCodigoRetiroDetalle.Where(x => x.Coregecodigo == idcod).FirstOrDefault();

                }


                if (obj == null && string.IsNullOrEmpty(idGenerado))
                    obj = model.ListaCodigoRetiroDetalle.Where(x => x.Barracodisum == Convert.ToInt32(idbarrsum)).FirstOrDefault();


                if (obj != null && obj.Coregecodigo > 0)
                {
                    lEliminados.Add(obj);
                    Session["ELIMINAR"] = lEliminados;
                }

                model.ListaCodigoRetiroDetalle.Remove(obj);
            }

            TempData["DETALLE"] = model.ListaCodigoRetiroDetalle;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ObtenerPeriodo(int idPeriodo)
        {
            ResultadoDTO<PeriodoDeclaracionDTO> resultado = new ResultadoDTO<PeriodoDeclaracionDTO>();
            resultado.Data = new PeriodoDeclaracionAppServicio().GetBydId(idPeriodo);
            return Json(resultado);
        }
        /// Permite actualizar o grabar un registro
        //POST
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Update(SolicitudCodigoModel modelo)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            List<SolicitudCodigoDetalleDTO> lNuevos = new List<SolicitudCodigoDetalleDTO>();
            List<SolicitudCodigoDetalleDTO> lEliminados = new List<SolicitudCodigoDetalleDTO>();
            List<TrnPotenciaContratadaDTO> lPotenciasContratadas = new List<TrnPotenciaContratadaDTO>();
            bool blnSolicitudCambioPotencia = false;

            if (modelo.Entidad.SoliCodiRetiObservacion != null)
            {
                string usuario = User.Identity.Name;
                string strDetNuevos = modelo.Entidad.SoliCodiRetiObservacion;
                string[] aDetNuevos = strDetNuevos.Split(',');


                for (int i = 0; i < aDetNuevos.Length; i++)
                {
                    SolicitudCodigoDetalleDTO oSolDetalle = new SolicitudCodigoDetalleDTO();
                    oSolDetalle.Barracodisum = Convert.ToInt32(aDetNuevos[i].Split('_')[1].ToString());
                    oSolDetalle.indexBarra = Convert.ToInt32(aDetNuevos[i].Split('_')[3].ToString());
                    oSolDetalle.Barracoditra = modelo.Entidad.BarrCodi;
                    oSolDetalle.Codigovtp = null;
                    oSolDetalle.Coresdusuarioregistro = usuario;
                    oSolDetalle.Coresocodi = modelo.Entidad.SoliCodiRetiCodi;
                    oSolDetalle.Coresdnumregistro = 1;
                    oSolDetalle.coregeestado = Funcion.EstadoPendiente;

                    var existeSuministro = (TempData["DETALLE"] as List<SolicitudCodigoDetalleDTO>).
                                            FirstOrDefault(x => x.Barracodisum == oSolDetalle.Barracodisum);

                    if (existeSuministro != null)
                    {
                        oSolDetalle.Coresdcodi = existeSuministro.Coresdcodi;
                    }
                    else
                    {
                        oSolDetalle.Coresdcodi = 0;
                    }

                    lNuevos.Add(oSolDetalle);
                }
            }

            lEliminados = Session["ELIMINAR"] as List<SolicitudCodigoDetalleDTO>;
            if (lEliminados != null)
            {
                foreach (var item in lEliminados)
                {
                    item.Coresdusuarioregistro = User.Identity.Name;
                    item.coregeestado = Funcion.EstadoInactivo;
                    // Si esta eliminado lo desactiva
                    this.servicioSolicitudCodigo.DesactivarPotenciasPorBarrSum(modelo.Entidad.SoliCodiRetiCodi, item.Coregecodigo, modelo.Entidad.PeridcCodi);
                }
            }

            using (TransactionScope trn = new TransactionScope())
            {
                try
                {
                    SolicitudCodigoDTO objSolicitud = (new SolicitudCodigoAppServicio()).GetByIdCodigoRetiro(modelo.Entidad.SoliCodiRetiCodi, modelo.Entidad.PeridcCodi);

                    #region Registro Potencias
                    modelo.Entidad.UsuaCodi = User.Identity.Name;
                    modelo.Entidad.esPrimerRegistro = modelo.Entidad.esPrimerRegistro ?? 1;
                    if (modelo.Entidad.TrnpcTipoCasoAgrupado == "AGRVTA" && modelo.Entidad.esPrimerRegistro == 1)
                    {
                        if (objSolicitud.TrnpcTipoCasoAgrupado == "AGRVTP")
                            this.servicioSolicitudCodigo.DesactivarPotenciasContratadas(modelo.Entidad.SoliCodiRetiCodi, modelo.Entidad.PeridcCodi);

                        modelo.ListaPotenciasContratadasVTP = string.Empty;
                        modelo.Entidad.TrnPctComeObs = "Potencias  contradas, que fueron registradas a nivela a nivel VTA";
                        if (objSolicitud.SoliCodiRetiEstado=="ACT")
                        {
                            
                            if ((objSolicitud.TrnPctHfpmwFija==modelo.Entidad.TrnPctHfpmwFija) && (objSolicitud.TrnPctHfpmwFijaVariable == modelo.Entidad.TrnPctHfpmwFijaVariable) && (objSolicitud.TrnPctHpmwFija == modelo.Entidad.TrnPctHpmwFija) && (objSolicitud.TrnPctHpmwFijaVariable == modelo.Entidad.TrnPctHpmwFijaVariable) && (objSolicitud.TrnPctTotalmwFija == modelo.Entidad.TrnPctTotalmwFija) && (objSolicitud.TrnPctTotalmwVariable == modelo.Entidad.TrnPctTotalmwVariable))
                            {
                                blnSolicitudCambioPotencia = false;
                            } else
                            {
                                blnSolicitudCambioPotencia = true;
                            }
                            if (blnSolicitudCambioPotencia)
                            {
                                this.servicioSolicitudCodigo.UpdateCodigoRetiroPotenciasContradasVTAAprobar(modelo.Entidad);
                            } else
                            {
                                this.servicioSolicitudCodigo.UpdateCodigoRetiroPotenciasContradasVTA(modelo.Entidad);
                            }
                            
                        } else
                        {
                            this.servicioSolicitudCodigo.UpdateCodigoRetiroPotenciasContradasVTA(modelo.Entidad);
                        }
                       
                    }
                    else if (modelo.Entidad.TrnpcTipoCasoAgrupado == "AGRVTP")
                    {
                        if (objSolicitud.TrnpcTipoCasoAgrupado == "AGRVTA")
                            this.servicioSolicitudCodigo.DesactivarPotenciasContratadas(modelo.Entidad.SoliCodiRetiCodi, modelo.Entidad.PeridcCodi);                        
                    }

                    modelo.Entidad.ListaPotenciaContratadas = new JavaScriptSerializer().Deserialize<List<TrnPotenciaContratadaDTO>>(modelo.ListaPotenciasContratadasVTP);

                    #endregion Registro Potencias


                    int result = 0;
                    if (lEliminados != null || lNuevos.Count > 0 || (modelo.Entidad.ListaPotenciaContratadas != null && modelo.Entidad.ListaPotenciaContratadas.Count > 0))
                    {
                        if (objSolicitud.SoliCodiRetiEstado == "ACT")
                        {
                            if (objSolicitud.ListaPotenciaContratadas.Count==modelo.Entidad.ListaPotenciaContratadas.Count)
                            {
                                int contador = 0;
                                foreach (var item in objSolicitud.ListaPotenciaContratadas)
                                {
                                    if ((item.TrnPctHfpMwFija== modelo.Entidad.ListaPotenciaContratadas[contador].TrnPctHfpMwFija) && (item.TrnPctHfpMwFijaVariable == modelo.Entidad.ListaPotenciaContratadas[contador].TrnPctHfpMwFijaVariable) && (item.TrnPctHpMwFija == modelo.Entidad.ListaPotenciaContratadas[contador].TrnPctHpMwFija) && (item.TrnPctHpMwFijaVariable == modelo.Entidad.ListaPotenciaContratadas[contador].TrnPctHpMwFijaVariable) && (item.TrnPctTotalMwFija == modelo.Entidad.ListaPotenciaContratadas[contador].TrnPctTotalMwFija) && (item.TrnPctTotalMwVariable == modelo.Entidad.ListaPotenciaContratadas[contador].TrnPctTotalMwVariable))
                                    {
                                        //blnSolicitudCambioPotencia = false;
                                    } else
                                    {
                                        blnSolicitudCambioPotencia = true;
                                    }
                                    contador++;
                                }
                            } else
                            {
                                blnSolicitudCambioPotencia = true;
                            }
                            
                        }
                        result = this.servicioSolicitudCodigo.UpdateCodigoRetiro(modelo.Entidad, lNuevos, lEliminados, blnSolicitudCambioPotencia);
                    }
                    else
                    {
                        result = 1;
                    }

                    #region Update TRNTIPOPOTENCIA

                    SolicitudCodigoDTO solcodDTO = new SolicitudCodigoDTO
                    {
                        PeridcCodi = modelo.Entidad.PeridcCodi,
                        CoresoCodi = modelo.Entidad.SoliCodiRetiCodi,
                        TrnpcTipoPotencia = modelo.Entidad.TrnpcTipoCasoAgrupado == "AGRVTA" ? 1 : 2
                    };

                    this.servicioSolicitudCodigo.UpdateTipPotCodConsolidadoPeriodo(solcodDTO);
                    this.servicioSolicitudCodigo.UpdateTipPotCodCodigoRetiro(solcodDTO);

                    #endregion

                    trn.Complete();

                    if (result > 0)
                    {
                        trn.Dispose();
                        if (blnSolicitudCambioPotencia)
                        {
                            TempData["sMensajeExito"] = "Su solicitud de cambio, ha sido registrada satisfactoriamente, en espera de aprobación.";
                        } else
                        {
                            TempData["sMensajeExito"] = "La información ha sido correctamente actualizada";
                        }
                        
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    trn.Dispose();
                }

            }


            if (modelo.Entidad.SoliCodiRetiCodigo == null)
            {
                modelo.Entidad.SoliCodiRetiCodigo = "PENDIENTE";

            }

            return RedirectToAction("View", "SolicitudCodigo", new { id = modelo.Entidad.SoliCodiRetiCodi, tipo = 1 });
        }

        /// Permite controlar un registro mediante estados
        /// en este caso delete significa que esta solicitandose de baja = SOLBAJAPEN
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public string Delete(int id = 0)
        //{
        //    SolicitudCodigoModel model = new SolicitudCodigoModel();

        //    if (id != 0)
        //    {
        //        model.Entidad = this.servicioSolicitudCodigo.GetByIdCodigoRetiro(id);

        //        if (model.Entidad.SoliCodiRetiEstado == "GEN" || model.Entidad.SoliCodiRetiEstado == Funcion.EstadoActivo)
        //        {
        //            model.Entidad.SoliCodiRetiObservacion = "SOLBAJAOK";
        //            model.Entidad.SoliCodiretiFechaSolBaja = DateTime.Now.Date;
        //            model.Entidad.SoliCodiRetiFechaBaja = DateTime.Now.Date;
        //            model.Entidad.CoesUserName = User.Identity.Name;
        //            model.Entidad.SoliCodiRetiUsuRegistro = User.Identity.Name;
        //            model.Entidad.SoliCodiRetiEstado = Funcion.EstadoSolicitudBaja;
        //            int result = this.servicioSolicitudCodigo.SolicitarBajarVTEA(model.Entidad);
        //            if (result > 0)
        //            {
        //                return "true";
        //            }
        //            else
        //            {
        //                return "false";
        //            }
        //        }
        //    }

        //    return "false";
        //}

        /// Permite controlar un registro mediante estados
        /// en este caso delete significa que esta solicitandose de baja = SOLBAJAPEN
        [HttpPost, ActionName("Deletevtp")]
        [ValidateAntiForgeryToken]
        public string Deletevtp(int id = 0)
        {
            CodigoGeneradoDTO oCodigo = new CodigoGeneradoDTO();
            if (id != 0)
            {
                oCodigo = this.servicioSolicitudCodigo.GetByIdCodigoGenerado(id);

                if (oCodigo.Coregeestado == Funcion.EstadoActivo)
                {
                    oCodigo.Coregeestado = Funcion.EstadoSolicitudBaja;
                    oCodigo.Coregeusuregistro = User.Identity.Name;
                    int result = this.servicioSolicitudCodigo.SolicitarBajarVTP(oCodigo);
                    if (result > 0)
                    {
                        return "true";
                    }
                    else
                    {
                        return "false";
                    }
                }

            }

            return "false";
        }

        #region Privates    
        private List<SolicitudCodigoDTO> generarAgrupacion(List<SolicitudCodigoDTO> listaSolicitud, ResultadoDTO<List<SolicitudCodigoDetalleDTO>> resultadoExcel)
        {
            //Funcion.PageSizeCodigoRetiro
            List<SolicitudCodigoDTO> listaSolicitudFormado = new List<SolicitudCodigoDTO>();

            var listaSolicitudFiltro = listaSolicitud.Select(x => x.SoliCodiRetiCodi).Distinct();

            foreach (var codSolicitud in listaSolicitudFiltro)
            {
                var solicitud = listaSolicitud.Where(x => x.SoliCodiRetiCodi == codSolicitud).ToList();

                SolicitudCodigoDTO cabecera = new SolicitudCodigoDTO();
                cabecera = solicitud[0];

                cabecera.SoliCodiRetiCodiVTAParent = solicitud[0].SoliCodiRetiCodi;
                List<SolicitudCodigoDetalleDTO> listaDetalle = new List<SolicitudCodigoDetalleDTO>();
                int index = 0;

                foreach (var item in solicitud)
                {
                    SolicitudCodigoDetalleDTO detalle = new SolicitudCodigoDetalleDTO
                    {
                        Barranomsum = item.BarraNomBarrSum,
                        coregeestado = item.SoliCodiRetiEstadoVTP,
                        Coregecodigo = item.Codretgencodi,

                        PotenciaContratadaDTO = new SolicitudCodigoPotenciaContratadaDTO
                        {
                            CoresoCodigo = item.CoresoCodiPotcn,
                            CoregeCodigo = item.CoregeCodiPotcn,
                            TipoAgrupacion = item.TipCasaAbrev,
                            CodigoAgrupacion = item.TrnpCagrp,
                            PotenciaContrTotalFija = item.TrnPctTotalmwFija,
                            PotenciaContrHPFija = item.TrnPctHpmwFija,
                            PotenciaContrHFPFija = item.TrnPctHfpmwFija,
                            PotenciaContrTotalVar = item.TrnPctTotalmwVariable,
                            PotenciaContrHPVar = item.TrnPctHpmwFijaVariable,
                            PotenciaContrHFPVar = item.TrnPctHfpmwFijaVariable
                        }
                    };
                    detalle.PotenciaContratadaDTO.PotenciaContrHFPVar = item.TrnPctHfpmwFijaVariable;
                    detalle.PotenciaContratadaDTO.NumeroOrden = item.TrnpcNumordm;
                    //obs.2---
                    detalle.PotenciaContratadaDTO.PotenciaContrObservacion = item.TrnPctComeObs;


                    if (item.SoliCodiRetiEstadoVTP == Funcion.EstadoActivo)
                    {
                        detalle.Codigovtp = item.SoliCodiRetiCodigoVTP;
                    }
                    else if (item.SoliCodiRetiEstadoVTP == Funcion.EstadoPendiente || item.SoliCodiRetiEstadoVTP == Funcion.EstadoPendientePVT)
                    {
                        detalle.Codigovtp = "PENDIENTE";
                    }
                    else if (item.SoliCodiRetiEstadoVTP == Funcion.EstadoSolicitudBaja)
                    {
                        detalle.Codigovtp = "SOLICITUD BAJA";
                    }
                    else if (item.SoliCodiRetiEstadoVTP == null)
                    {
                        detalle.coregeestado = "";
                        detalle.Codigovtp = "";
                    }
                    else
                    {
                        detalle.Codigovtp = item.SoliCodiRetiCodigoVTP;
                    }

                    //SIN SON SOLO VTEA FUERA DENTRO DEL VTP

                    detalle.Indice = index;
                    listaDetalle.Add(detalle);
                    index++;
                }
                cabecera.NroRegDetalle = listaDetalle.Count();
                cabecera.ListaCodigoRetiroDetalle = listaDetalle;
                listaSolicitudFormado.Add(cabecera);
            }
            //Armando estructura de Potencias Contratadas
            foreach (var item in listaSolicitudFormado)
            {
                bool finalizar = false;

                if (item.OmitirFilaVTA == 1)
                    continue;

                foreach (var detalle in item.ListaCodigoRetiroDetalle)
                {
                    if (detalle.OmitirFila == 1)
                        continue;

                    //---------------------------------------------------------------------------------------
                    //Si 5 Existen registros
                    //---------------------------------------------------------------------------------------
                    List<SolicitudCodigoDTO> obtenerPotencias = new List<SolicitudCodigoDTO>();
                    if (detalle.PotenciaContratadaDTO.TipoAgrupacion == Constantes.AgrupacionVTP
                        || detalle.PotenciaContratadaDTO.TipoAgrupacion == Constantes.AgrupacionVTA)
                    {
                        int? rowSpan = 0;
                        List<int> omitirFilas = new List<int>();
                        int encontroPrimerRegistro = 0;
                        string Observacion = "";
                        decimal? PotenciaContrTotalFija = 0;
                        decimal? PotenciaContrHPFija = 0;
                        decimal? PotenciaContrHFPFija = 0;
                        decimal? PotenciaContrTotalVar = 0;
                        decimal? PotenciaContrHPVar = 0;
                        decimal? PotenciaContrHFPVar = 0;

                        int PotenciaEsExcel = 0;


                        if (detalle.PotenciaContratadaDTO.TipoAgrupacion == Constantes.AgrupacionVTP)
                        {
                            obtenerPotencias = listaSolicitud.Where(x => x.CoresoCodiPotcn == item.CoresoCodiPotcn
                        && x.TrnpCagrp == detalle.PotenciaContratadaDTO.CodigoAgrupacion
                        ).ToList();
                        }
                        else
                        {

                            //Si es una agrupacion VTA :> Obtiene sus potencias asociadas para cargar en una lista
                            if (detalle.PotenciaContratadaDTO.CodigoAgrupacion is null)
                                break;
                            obtenerPotencias = listaSolicitud.Where(x => x.TrnpCagrp == detalle.PotenciaContratadaDTO.CodigoAgrupacion).OrderBy(c => c.TrnpcNumordm).ToList();

                        }

                        // Obtiene sus potencias asociadas
                        rowSpan = obtenerPotencias.Count;
                        foreach (var itemPotencia in obtenerPotencias)
                        {
                            if (encontroPrimerRegistro > 0)
                            {

                                if (detalle.PotenciaContratadaDTO.TipoAgrupacion == Constantes.AgrupacionVTP)
                                    omitirFilas.Add(itemPotencia.Codretgencodi);
                                else if (detalle.PotenciaContratadaDTO.TipoAgrupacion == Constantes.AgrupacionVTA)
                                    omitirFilas.Add(Convert.ToInt32(itemPotencia.CoresoCodiPotcn));
                            }



                            //---------------------------------------------------------------------------------------
                            //Si es VTP
                            //---------------------------------------------------------------------------------------
                            if (detalle.PotenciaContratadaDTO.TipoAgrupacion == Constantes.AgrupacionVTP)
                            {

                                if (resultadoExcel == null)
                                {
                                    if (itemPotencia.TrnPctExcel == 1)
                                    {
                                        PotenciaContrTotalFija = itemPotencia.TrnPctTotalmwFija;
                                        PotenciaContrHPFija = itemPotencia.TrnPctHpmwFija;
                                        PotenciaContrHFPFija = itemPotencia.TrnPctHfpmwFija;

                                        PotenciaContrTotalVar = itemPotencia.TrnPctTotalmwVariable;
                                        PotenciaContrHPVar = itemPotencia.TrnPctHpmwFijaVariable;
                                        PotenciaContrHFPVar = itemPotencia.TrnPctHfpmwFijaVariable;

                                    }
                                    else
                                    {
                                        PotenciaContrTotalFija += itemPotencia.TrnPctTotalmwFija;
                                        PotenciaContrHPFija += itemPotencia.TrnPctHpmwFija;
                                        PotenciaContrHFPFija += itemPotencia.TrnPctHfpmwFija;

                                        PotenciaContrTotalVar += itemPotencia.TrnPctTotalmwVariable;
                                        PotenciaContrHPVar += itemPotencia.TrnPctHpmwFijaVariable;
                                        PotenciaContrHFPVar += itemPotencia.TrnPctHfpmwFijaVariable;

                                    }

                                }
                                else
                                {

                                    #region Obtiene del archivo Excel
                                    SolicitudCodigoDetalleDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.Coresocodi == item.SoliCodiRetiCodi).FirstOrDefault();
                                    #endregion Obtiene del archivo Excel

                                    if (potenciaVTAExcel != null)
                                    {

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                            PotenciaContrTotalFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                            PotenciaContrHPFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                            PotenciaContrHFPFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                            PotenciaContrTotalVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)
                                            PotenciaContrHPVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                            PotenciaContrHFPVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;

                                        //obs.
                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                            Observacion = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;


                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null
                                )
                                            PotenciaEsExcel = 1;
                                    }
                                }



                            }

                            //---------------------------------------------------------------------------------------
                            //Si es VTA
                            //---------------------------------------------------------------------------------------
                            else if (
                                detalle.PotenciaContratadaDTO.TipoAgrupacion == Constantes.AgrupacionVTA
                                && encontroPrimerRegistro == 0

                                )
                            {
                                if (resultadoExcel != null)
                                {
                                    #region Obtiene del archivo Excel
                                    SolicitudCodigoDetalleDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.Coresocodi == item.SoliCodiRetiCodi).FirstOrDefault();
                                    #endregion Obtiene del archivo Excel

                                    if (potenciaVTAExcel != null)
                                    {

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                            itemPotencia.TrnPctTotalmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                            itemPotencia.TrnPctHpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                            itemPotencia.TrnPctHfpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                            itemPotencia.TrnPctTotalmwVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)
                                            itemPotencia.TrnPctHpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                            itemPotencia.TrnPctHfpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;
                                        //obs.
                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                            itemPotencia.TrnPctComeObs = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                || itemPotencia.TrnPctComeObs != null

                                )
                                            itemPotencia.TrnPctExcel = 1;



                                    }
                                }


                                PotenciaContrTotalFija = itemPotencia.TrnPctTotalmwFija;
                                PotenciaContrHPFija = itemPotencia.TrnPctHpmwFija;
                                PotenciaContrHFPFija = itemPotencia.TrnPctHfpmwFija;
                                PotenciaContrTotalVar = itemPotencia.TrnPctTotalmwVariable;
                                PotenciaContrHPVar = itemPotencia.TrnPctHpmwFijaVariable;
                                PotenciaContrHFPVar = itemPotencia.TrnPctHfpmwFijaVariable;
                                Observacion = itemPotencia.TrnPctComeObs;
                                //EsExcel
                                PotenciaEsExcel = itemPotencia.TrnPctExcel;



                            }

                            encontroPrimerRegistro++;

                        }

                        detalle.PotenciaContratadaDTO.PotenciaContrTotalFija = PotenciaContrTotalFija;
                        detalle.PotenciaContratadaDTO.PotenciaContrHPFija = PotenciaContrHPFija;
                        detalle.PotenciaContratadaDTO.PotenciaContrHFPFija = PotenciaContrHFPFija;
                        detalle.PotenciaContratadaDTO.PotenciaContrTotalVar = PotenciaContrTotalVar;
                        detalle.PotenciaContratadaDTO.PotenciaContrHPVar = PotenciaContrHPVar;
                        detalle.PotenciaContratadaDTO.PotenciaContrHFPVar = PotenciaContrHFPVar;
                        detalle.PotenciaContratadaDTO.PotenciaContrObservacion = Observacion;

                        //EsExcel
                        detalle.PotenciaContratadaDTO.PotenciaEsExcel = PotenciaEsExcel;



                        detalle.PotenciaContratadaDTO.RowSpan = rowSpan;
                        foreach (var itemFormato in listaSolicitudFormado)
                        {
                            IEnumerable<SolicitudCodigoDetalleDTO> detalleEncontrado = null;

                            if (itemFormato.TipCasaAbrev == Constantes.AgrupacionVTP)
                            {
                                detalleEncontrado = itemFormato.ListaCodigoRetiroDetalle.Where(x =>
                                  omitirFilas.Contains(x.Coregecodigo)
                                );
                            }
                            else if (itemFormato.TipCasaAbrev == Constantes.AgrupacionVTA)
                            {
                                //Obs.Omitir 
                                detalleEncontrado = from pot in itemFormato.ListaCodigoRetiroDetalle
                                                    where omitirFilas.Where(f => f == pot.PotenciaContratadaDTO.CoresoCodigo).Count() > 0
                                                    select pot;

                                var aux = detalleEncontrado.ToList();
                                // detalleEncontrado = itemFormato.ListaCodigoRetiroDetalle.Where(x =>
                                //  omitirFilas.Contains((int)(x.PotenciaContratadaDTO.CoresoCodigo ?? 0))
                                //);
                            }

                            if (detalleEncontrado != null)
                            {
                                foreach (var itemDetalle in detalleEncontrado)
                                    itemDetalle.OmitirFila = 1;
                                if (detalleEncontrado.Count() > 0
                                && detalle.PotenciaContratadaDTO.TipoAgrupacion == Constantes.AgrupacionVTA
                                && itemFormato.CoresoCodiPotcn != item.CoresoCodiPotcn)
                                {
                                    itemFormato.OmitirFilaVTA = 1;
                                    itemFormato.SoliCodiRetiCodiVTAParent = (int)item.CoresoCodiPotcn;
                                }
                            }




                        }

                    }
                    else
                        obtenerPotencias = listaSolicitud.Where(x => x.SoliCodiRetiCodi == item.SoliCodiRetiCodi).ToList();


                    //---------------------------------------------------------------------------------------
                    //Si no Existen registros
                    //---------------------------------------------------------------------------------------
                    if (obtenerPotencias.Count > 0
                         && detalle.TipCasaAbrev != Constantes.AgrupacionVTP)
                    {
                        List<int> omitirFilas = new List<int>();
                        int? rowSpan = obtenerPotencias.Count;
                        int encontroPrimerRegistro = 0;
                        SolicitudCodigoDTO potenciaEncontrada = null;
                        foreach (var itemPtcn in obtenerPotencias)
                        {
                            //Potencias registradas a nivel de VTEA
                            //if (itemPtcn.SoliCodiRetiCodi != null
                            // && itemPtcn.CoregeCodiPotcn == null
                            if (itemPtcn.SoliCodiRetiCodi != 0
                                && itemPtcn.Codretgencodi != 0
                                && (itemPtcn.TipCasaAbrev == Constantes.AgrupacionVTA)
                                && encontroPrimerRegistro == 0)
                            {
                                encontroPrimerRegistro++;

                                if (resultadoExcel == null)
                                    potenciaEncontrada = itemPtcn;
                                else
                                {
                                    #region Obtiene del archivo Excel
                                    SolicitudCodigoDetalleDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.Coresocodi == itemPtcn.SoliCodiRetiCodi).FirstOrDefault();
                                    #endregion Obtiene del archivo Excel

                                    potenciaEncontrada = itemPtcn;

                                    //esExcel
                                    if (potenciaVTAExcel != null)
                                    {

                                        potenciaEncontrada.CoresoCodiPotcn = itemPtcn.CoregeCodiPotcn;
                                        potenciaEncontrada.CoregeCodiPotcn = itemPtcn.CoregeCodiPotcn;
                                        potenciaEncontrada.TipCasaAbrev = itemPtcn.TipCasaAbrev;
                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                            potenciaEncontrada.TrnPctTotalmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                            potenciaEncontrada.TrnPctHpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                            potenciaEncontrada.TrnPctHfpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                            potenciaEncontrada.TrnPctTotalmwVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;
                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)
                                            potenciaEncontrada.TrnPctHpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                            potenciaEncontrada.TrnPctHfpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;

                                        //obs.
                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                            potenciaEncontrada.TrnPctComeObs = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                     || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                     || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                     || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                     || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                     || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                     || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null

                                     )
                                            potenciaEncontrada.TrnPctExcel = 1;

                                    }
                                }
                                continue;
                            }
                            //Potencias registradas a nivel de VTP
                            //else if (itemPtcn.CoresoCodiPotcn != null
                            //  && itemPtcn.CoregeCodiPotcn != null
                            else if (itemPtcn.SoliCodiRetiCodi != 0

                            && string.IsNullOrEmpty(itemPtcn.TipCasaAbrev))
                            {
                                if (item.TrnpcTipoPotencia == 2 && itemPtcn.Codretgencodi != 0)
                                {
                                    rowSpan = null;
                                    potenciaEncontrada = null;
                                    break;
                                }
                                else
                                {
                                    if (encontroPrimerRegistro == 0)
                                    {
                                        encontroPrimerRegistro++;
                                        itemPtcn.TipCasaAbrev = Constantes.AgrupacionVTA;
                                        if (resultadoExcel == null)
                                            potenciaEncontrada = itemPtcn;
                                        else
                                        {
                                            #region Obtiene del archivo Excel
                                            SolicitudCodigoDetalleDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.Coresocodi == itemPtcn.SoliCodiRetiCodi).FirstOrDefault();
                                            #endregion Obtiene del archivo Excel

                                            potenciaEncontrada = itemPtcn;

                                            //esExcel
                                            if (potenciaVTAExcel != null)
                                            {
                                                potenciaEncontrada.CoresoCodiPotcn = itemPtcn.CoregeCodiPotcn;
                                                potenciaEncontrada.CoregeCodiPotcn = itemPtcn.CoregeCodiPotcn;
                                                potenciaEncontrada.TipCasaAbrev = itemPtcn.TipCasaAbrev;
                                                if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                                    potenciaEncontrada.TrnPctTotalmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                                if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                                    potenciaEncontrada.TrnPctHpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                                if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                                    potenciaEncontrada.TrnPctHfpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                                if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                                    potenciaEncontrada.TrnPctTotalmwVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;
                                                if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)
                                                    potenciaEncontrada.TrnPctHpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                                if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                                    potenciaEncontrada.TrnPctHfpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;

                                                //obs.
                                                if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                                    potenciaEncontrada.TrnPctComeObs = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;

                                                if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                             || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                             || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                             || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                             || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                             || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                             || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null

                                             )
                                                {
                                                    potenciaEncontrada.TrnpcTipoPotencia = 1;
                                                    potenciaEncontrada.TrnPctExcel = 1;
                                                }

                                            }
                                        }

                                    }
                                    else
                                    {
                                        itemPtcn.OmitirFilaVTA = 1;
                                        omitirFilas.Add(itemPtcn.CoregeCodiPotcn ?? itemPtcn.Codretgencodi);
                                    }
                                }
                            }
                            else
                            {
                                omitirFilas.Add(itemPtcn.CoregeCodiPotcn ?? itemPtcn.Codretgencodi);
                            }
                        }

                        if (potenciaEncontrada != null && rowSpan != null)
                        {
                            SolicitudCodigoPotenciaContratadaDTO itemPotencia = new SolicitudCodigoPotenciaContratadaDTO();
                            itemPotencia.CoresoCodigo = potenciaEncontrada.CoresoCodiPotcn;
                            itemPotencia.CoregeCodigo = potenciaEncontrada.CoregeCodiPotcn;
                            itemPotencia.TipoAgrupacion = potenciaEncontrada.TipCasaAbrev;
                            itemPotencia.PotenciaContrTotalFija = potenciaEncontrada.TrnPctTotalmwFija;
                            itemPotencia.PotenciaContrHPFija = potenciaEncontrada.TrnPctHpmwFija;
                            itemPotencia.PotenciaContrHFPFija = potenciaEncontrada.TrnPctHfpmwFija;
                            itemPotencia.PotenciaContrTotalVar = potenciaEncontrada.TrnPctTotalmwVariable;
                            itemPotencia.PotenciaContrHPVar = potenciaEncontrada.TrnPctHpmwFijaVariable;
                            itemPotencia.PotenciaContrHFPVar = potenciaEncontrada.TrnPctHfpmwFijaVariable;
                            itemPotencia.PotenciaContrObservacion = potenciaEncontrada.TrnPctComeObs;

                            //esExcel
                            itemPotencia.PotenciaEsExcel = potenciaEncontrada.TrnPctExcel;

                            //obs.2
                            itemPotencia.PotenciaContrObservacion = potenciaEncontrada.TrnPctComeObs;
                            itemPotencia.RowSpan = rowSpan;
                            item.OmitirFilaVTA = 0;
                            detalle.PotenciaContratadaDTO = itemPotencia;

                            foreach (var itemFormato in listaSolicitudFormado)
                            {
                                var detalleEncontrado = itemFormato.ListaCodigoRetiroDetalle.Where(x => omitirFilas.Contains(x.Coregecodigo));
                                foreach (var itemDetalle in detalleEncontrado)
                                {
                                    itemDetalle.OmitirFila = 1;
                                }
                            }
                        }
                        else if (rowSpan == null)
                        {
                            var t = detalle.PotenciaContratadaDTO;
                            if (resultadoExcel != null)
                            {
                                #region Obtiene del archivo Excel
                                SolicitudCodigoDetalleDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.Coregecodigo == detalle.Coregecodigo).FirstOrDefault();
                                #endregion Obtiene del archivo Excel
                                //EsExcel
                                if (potenciaVTAExcel != null)
                                {

                                    SolicitudCodigoPotenciaContratadaDTO itemPotencia = new SolicitudCodigoPotenciaContratadaDTO();
                                    itemPotencia.CoresoCodigo = item.SoliCodiRetiCodi;
                                    itemPotencia.CoregeCodigo = detalle.CoregeCodiPotcn;
                                    itemPotencia.TipoAgrupacion = null;

                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                        itemPotencia.PotenciaContrTotalFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                        itemPotencia.PotenciaContrHPFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                        itemPotencia.PotenciaContrHFPFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                        itemPotencia.PotenciaContrTotalVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;

                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)

                                        itemPotencia.PotenciaContrHPVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                        itemPotencia.PotenciaContrHFPVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;

                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                        itemPotencia.PotenciaContrObservacion = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;

                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                        || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                        || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                        || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                        || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                        || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                        || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null
                                        )
                                        itemPotencia.PotenciaEsExcel = 1;

                                    //obs.3
                                    itemPotencia.PotenciaContrObservacion = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;

                                    detalle.PotenciaContratadaDTO = itemPotencia;
                                }
                            }
                        }

                    }
                }
            }


            foreach (var x in listaSolicitudFormado)
            {
                if (x.SoliCodiRetiEstado == "ASI" && x.SoliCodiRetiObservacion.Equals("SOLBAJAOK"))
                {
                    //x.SoliCodiRetiEstado = Funcion.EstadoBaja;
                    //x.EstadoDescripcionVTEA = "Baja";
                    x.SoliCodiRetiEstado = Funcion.EstadoActivo;
                    x.EstadoDescripcionVTEA = "Activo";
                }
                else if (x.SoliCodiRetiEstado == "ASI" && x.SoliCodiRetiObservacion.Equals("SOLBAJANO"))
                {
                    x.SoliCodiRetiEstado = Funcion.EstadoActivo;
                    x.EstadoDescripcionVTEA = "Activo";
                }
                else if (x.SoliCodiRetiEstado == "GEN" && x.SoliCodiRetiObservacion.Equals("SOLBAJANO"))
                {
                    x.SoliCodiRetiEstado = Funcion.EstadoPendiente;
                    x.EstadoDescripcionVTEA = "Pendiente de aprobación";
                }
                else if (x.SoliCodiRetiEstado == Funcion.EstadoPendiente || x.SoliCodiRetiEstado == Funcion.EstadoPendientePVT)
                {
                    x.EstadoDescripcionVTEA = "Pendiente de aprobación";
                }

                if (x.SoliCodiRetiCodigo == null)
                {
                    x.SoliCodiRetiCodigo = "PENDIENTE";

                }

            }        

            return listaSolicitudFormado;
        }


        #endregion Privates

    }
}