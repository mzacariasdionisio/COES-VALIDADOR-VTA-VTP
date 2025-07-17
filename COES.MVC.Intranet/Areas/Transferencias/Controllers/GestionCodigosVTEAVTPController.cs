using COES.Dominio.DTO.Enum;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.PMPO.Controllers;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Transferencias;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class GestionCodigosVTEAVTPController : BaseController
    {

        SeguridadServicioClient servicio = new SeguridadServicioClient();
        // GET: /Transferencias/GestionCodigosVTEAVTP/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            UserDTO usuario = Session[DatosSesion.SesionUsuario] as UserDTO;

            EmpresaModel modelEmpGen = new EmpresaModel();
            //modelEmpGen.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas().OrderBy(x => x.EmprNombre).ToList();
            modelEmpGen.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCoReSoGen();
            EmpresaModel modelEmpCli = new EmpresaModel();
            modelEmpCli.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            //modelEmpCli.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCoReSoCli();
            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaInterCoReSo();
            modelBarr.ListaBarrasSum = (new BarraAppServicio()).ListarBarraSuministro();

            TipoContratoModel modelTipoCont = new TipoContratoModel();
            modelTipoCont.ListaTipoContrato = (new TipoContratoAppServicio()).ListTipoContrato();

            TipoUsuarioModel modelTipoUsu = new TipoUsuarioModel();
            modelTipoUsu.ListaTipoUsuario = (new TipoUsuarioAppServicio()).ListTipoUsuario();

            /*****************************************************************************************/
            var genemprcodi = Session["genemprcodi"];
            var clicodi = Session["clicodi"];
            var periCodi = Session["periCodi"];
            /*****************************************************************************************/
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodoDeclaracion = (new PeriodoDeclaracionAppServicio()).GetListaCombobox();
            if (genemprcodi == null)
            {
                ViewBag.EMPRCODI2 = new SelectList(modelEmpGen.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            }
            else
            {
                ViewBag.EMPRCODI2 = new SelectList(modelEmpGen.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE", genemprcodi);
            }
            if (clicodi == null)
            {
                ViewBag.CLICODI2 = new SelectList(modelEmpCli.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            }
            else
            {
                ViewBag.CLICODI2 = new SelectList(modelEmpCli.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE", clicodi);
            }
            ViewBag.BARRCODI2 = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");
            ViewBag.BARRCODI3 = new SelectList(modelBarr.ListaBarrasSum, "BARRCODI", "BARRNOMBRE");
            ViewBag.TIPOCONTCODI2 = new SelectList(modelTipoCont.ListaTipoContrato, "TIPOCONTCODI", "TIPOCONTNOMBRE");
            ViewBag.TIPOUSUACODI2 = new SelectList(modelTipoUsu.ListaTipoUsuario, "TIPOUSUACODI", "TIPOUSUANOMBRE");
            ViewBag.ESTCODSOL = new SelectList(new Funcion().ObtenerEstados(), "Value", "Text");
            ViewBag.ESTCODSOLAPR = new SelectList(new Funcion().ObtenerEstadosSolitudPendienteAprobacion(), "Value", "Text");
            if (periCodi == null)
            {
                ViewBag.PERIODO2 = new SelectList(modelPeriodo.ListaPeriodoDeclaracion, "PeridcCodi", "PeridcNombre", modelPeriodo.ListaPeriodoDeclaracion.FirstOrDefault().PeridcCodi);
            }
            else
            {
                ViewBag.PERIODO2 = new SelectList(modelPeriodo.ListaPeriodoDeclaracion, "PeridcCodi", "PeridcNombre", periCodi);
            }

            #region "Histórico"           
            TempData["EMPRCODI2"] = new SelectList(modelEmpGen.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            TempData["CLICODI2"] = new SelectList(modelEmpCli.ListaEmpresas, "EMPRCODI", "EMPRNOMBRE");
            TempData["BARRCODI2"] = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");
            TempData["TIPOCONTCODI2"] = new SelectList(modelTipoCont.ListaTipoContrato, "TIPOCONTCODI", "TIPOCONTNOMBRE");
            TempData["TIPOUSUACODI2"] = new SelectList(modelTipoUsu.ListaTipoUsuario, "TIPOUSUACODI", "TIPOUSUANOMBRE");

            CodigoRetiroModel model = new CodigoRetiroModel
            {
                bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name)
            };
            #endregion "Histórico"

            return View();
        }

        //POST
        [HttpPost]
        public ActionResult Lista(int? genemprcodi, int? clicodi, int? tipocont, int? tipousu, int? barrcodi, int? barrcodisum, string coresoestado, string coregecodvteavtp, int periCodi, int nroPagina, string coresoestapr, string base64)
        {
            Session["genemprcodi"] = genemprcodi;
            Session["clicodi"] = clicodi;
            Session["periCodi"] = periCodi;
            //ViewBag.NroPagina = nroPagina;
            ViewBag.NroPagina = 1;
            PeriodoDeclaracionDTO objPeriodo = new PeriodoDeclaracionAppServicio().GetBydId((int)periCodi);

            string base64Comparar = null;
            if (base64 != null)
            {

                #region valida formatos iguales
                ResultadoDTO<List<CodigoRetiroDTO>> objCodigosExportado = new CodigoRetiroAppServicio().ListarGestionCodigosExportarVTEAVTP(base.UserName, genemprcodi, clicodi, tipocont,
                                                                                                                                            tipousu, barrcodi, barrcodisum, coresoestado, coregecodvteavtp,
                                                                                                                                            null, null, periCodi, 1, int.MaxValue);
                List<CodigoRetiroDTO> paramSolicitud = objCodigosExportado.Data as List<CodigoRetiroDTO>;
                base64Comparar = new SolicitudCodigoAppServicio().ExportarDatosCodigoRetiroIntranet("", paramSolicitud);
                #endregion valida formatos iguales
            }

            ResultadoDTO<List<CodigoRetiroDTO>> rsCodigoRetiro = new ResultadoDTO<List<CodigoRetiroDTO>>();
            rsCodigoRetiro = new CodigoRetiroAppServicio().ListarGestionCodigosVTEAVTP(base.UserName, genemprcodi, clicodi, tipocont, tipousu, barrcodi,
                barrcodisum, coresoestado, coresoestapr, coregecodvteavtp, null, null,
                periCodi,
                nroPagina,
                int.MaxValue,
                base64,
                base64Comparar);

            if (rsCodigoRetiro.EsCorrecto == -2)
                return Json(rsCodigoRetiro);

            CodigoRetiroModel model = new CodigoRetiroModel();
            model.ListaCodigoRetiro = rsCodigoRetiro.Data;
            model.sError = rsCodigoRetiro.Mensaje;
            model.PeriCodi = periCodi;
            model.estadoPeriodo = objPeriodo.PeridcEstado;
            model.SolicitudCambio = coresoestapr;
            //Funcion.PageSize);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SaveAgruparGrilla(List<TrnPotenciaContratadaDTO> parametro)
        {
            ResultadoDTO<int> resultado = new SolicitudCodigoAppServicio().GenerarPotenciasAgrupadas(base.UserName, parametro, true);

            return Json(resultado);
        }
        [HttpPost]
        public ActionResult DesagruparPotencias(List<TrnPotenciaContratadaDTO> parametro)
        {
            ResultadoDTO<int> resultado = new SolicitudCodigoAppServicio().DesagruparPotencias(base.UserName, parametro, true);

            return Json(resultado);
        }

        [HttpPost]
        public ActionResult ExportarInformacion(int? genemprcodi, int? clicodi, int? tipocont, int? tipousu, int? barrcodi, int? barrcodisum, string coresoestado, string coregecodvteavtp, int periCodi)
        {
            PeriodoDeclaracionDTO objPeriodo = new PeriodoDeclaracionAppServicio().GetBydId((int)periCodi);

            ResultadoDTO<ArchivoBaseDTO> resultado = new ResultadoDTO<ArchivoBaseDTO>();
            ResultadoDTO<List<CodigoRetiroDTO>> rsCodigoRetiro = new ResultadoDTO<List<CodigoRetiroDTO>>();
            rsCodigoRetiro = new CodigoRetiroAppServicio().ListarGestionCodigosExportarVTEAVTP(base.UserName, genemprcodi, clicodi, tipocont, tipousu,
                                                                                                barrcodi, barrcodisum, coresoestado, coregecodvteavtp,
                                                                                                null, null, periCodi, 1, int.MaxValue);

            #region exportacion
            CodigoRetiroModel model = new CodigoRetiroModel
            {
                ListaCodigoRetiro = rsCodigoRetiro.Data.ToList()
            };

            #endregion exportacion

            string nombre = "CodigoRetiro";

            Session["TMP_ARCHIVO_2"] = model.ListaCodigoRetiro;
            resultado.Data = new ArchivoBaseDTO
            {
                archivoBase64 = new SolicitudCodigoAppServicio().ExportarDatosCodigoRetiroIntranet(nombre, model.ListaCodigoRetiro),
                nombreArchivo = string.Format("{0}{1}{2}{3}{4}_{5}.xlsx", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Second, nombre)
            };

            #region AuditoriaProceso
            VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO
            {
                Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion,
                Estdcodi = (int)EVtpEstados.BajarFormato,
                Audproproceso = "Exportar informacion de codigos.",
                Audprodescripcion = "Intranet - Se exporta data en excel del periodo " + objPeriodo.PeridcNombre,
                Audprousucreacion = base.UserName,
                Audprofeccreacion = DateTime.Now
            };
            new AuditoriaProcesoAppServicio().save(objAuditoria);
            #endregion AuditoriaProceso

            return Json(resultado);
        }


        [HttpPost]
        public ActionResult Paginado(int periCodi, int? genemprcodi, int? clicodi, int? tipocont, int? tipousu, int? barrcodi, int? barrcodisum, string coresoestado, string coregecodvteavtp, string fechaIni, string fechaFin, int paginadoActual = 1)
        {
            ViewBag.paginadoActual = paginadoActual;

            CodigoRetiroModel model = new CodigoRetiroModel();
            model.IndicadorPagina = false;
            model.NroRegistros = new CodigoRetiroAppServicio().ObtenerNroRegistrosGestionCodigosVTEAVTP(periCodi, genemprcodi, clicodi, tipocont, tipousu, barrcodi, barrcodisum, coresoestado, coregecodvteavtp, string.IsNullOrEmpty(fechaIni) ? null : (DateTime?)DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture), string.IsNullOrEmpty(fechaFin) ? null : (DateTime?)DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture));
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

        [HttpGet]
        public ActionResult Edit(int peridcCodi, int id = 0, int pagina = 1)
        {
            ViewBag.NroPagina = pagina;
            ViewBag.CodigoRolUsuario = ConstantesGestionCodigosVTEAVTP.IdRolCodigoVTEA;

            PeriodoDeclaracionDTO objPeriodo = new PeriodoDeclaracionAppServicio().GetBydId(peridcCodi);

            CodigoRetiroModel model = new CodigoRetiroModel();
            model.Entidad = (new CodigoRetiroAppServicio()).GetByIdGestionCodigosVTEAVTP(id, peridcCodi);
            model.Entidad.PeridcCodi = peridcCodi;
            model.Entidad.PeridcEstado = objPeriodo.PeridcEstado;

            ViewBag.BARRSUMI2 = new SelectList((new BarraAppServicio()).ListBarras(), "BARRCODI", "BARRNOMBRE");


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CodigoRetiroModel parametro)
        {
            string agenteUsuario = parametro.Entidad.CoesUserName;
            string correoAgente = "";

            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            parametro.Entidad.SoliCodiRetiCodigo = (new Funcion()).CorregirCodigo(parametro.Entidad.SoliCodiRetiCodigo);

            CodigoRetiroModel model = new CodigoRetiroModel();
            model.Entidad = (new CodigoRetiroAppServicio()).GetCodigoRetiroByCodigo(parametro.Entidad.SoliCodiRetiCodigo);

            if (model.Entidad != null && model.Entidad.SoliCodiRetiCodi != 0 && parametro.Entidad.SoliCodiRetiCodi != model.Entidad.SoliCodiRetiCodi)
            {
                resultado.EsCorrecto = (int)EnumResultado.error;
                resultado.Mensaje = "El Código de Entrega [" + model.Entidad.SoliCodiRetiCodigo + "], ya se encuentra registrado";
            }
            else
            {
                parametro.Entidad.CoesUserName = User.Identity.Name;
                parametro.Entidad.SoliCodiRetiEstado = parametro.Entidad.EstAbrev == ConstantesGestionCodigosVTEAVTP.Pendiente ? ConstantesGestionCodigosVTEAVTP.PendienteAprobacionVTP : ConstantesGestionCodigosVTEAVTP.Activo; //Codigo Asignado
                if (parametro.Entidad.SoliCodiRetiEstado == ConstantesGestionCodigosVTEAVTP.Activo)
                {
                    // correoAgente = this.servicio.ObtenerUsuarioPorLogin(agenteUsuario).UserEmail;
                    //correoAgente = ConfigurationManager.AppSettings["CorreoPruebas"].ToString();
                }
                parametro.Entidad.SoliCodiRetiFechaInicio = DateTime.ParseExact(parametro.Solicodiretifechainicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                parametro.Entidad.SoliCodiRetiFechaFin = DateTime.ParseExact(parametro.Solicodiretifechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                if (parametro.Entidad.SoliCodiRetiEstado == "ACT")
                    parametro.Entidad.UsuarioAgenteRegistro = this.servicio.ObtenerUsuarioPorLogin(parametro.Entidad.Seinusername).UserEmail;

                resultado = (new CodigoRetiroAppServicio()).AprobarRechazarSolicitud(parametro.Entidad);
                if (resultado.Data > 0)
                {
                    TempData["sMensajeExito"] = "La información ha sido correctamente registrada";



                    resultado.EsCorrecto = (int)EnumResultado.correcto;
                    resultado.Mensaje = "Se ha registrado correctamente. Sera redireccionado a la lista de solicitudes.";
                }
            }
            return Json(resultado);
        }

        [HttpPost]
        public ActionResult EnviarDatos(List<TrnPotenciaContratadaDTO> parametro)
        {
            string userName = base.UserName;

            ResultadoDTO<int> resultado = new SolicitudCodigoAppServicio().GenerarCargaDatosExcel(userName, parametro, true);
            return Json(resultado);
        }

        public ActionResult _AddAbreviatura(EmpresaModel model)
        {
            return PartialView(new AbreviaturaAgregarModel
            {
                Emprcodi = model.IdEmpresa,
                Emprnomb = model.nombreEmpresa
            });
        }
        [HttpPost]
        public ActionResult _AddAbreviatura(AbreviaturaAgregarModel model)
        {
            ResultadoDTO<COES.Dominio.DTO.Transferencias.EmpresaDTO> result = (new EmpresaAppServicio().SaveUpdateAbreaviatura(
                new COES.Dominio.DTO.Transferencias.EmpresaDTO
                {
                    EmprAbrevCodi = model.EmprAbrevCodi,
                    EmprCodi = model.Emprcodi
                }));

            return Json(result);
        }

        [HttpPost]
        public ActionResult ListarPotenciasContratadas(int coresocodi, int periCodi)
        {
            List<TrnPotenciaContratadaDTO> resultado = new SolicitudCodigoAppServicio().ListaPotenciaContratadas(coresocodi, periCodi);
            return Json(resultado);

        }

        [HttpPost]
        public ActionResult ListarEnvios(int periCodi)
        {
            SolicitudCodigoModel model = new SolicitudCodigoModel();
            model.ListarEnvios = new TransferenciasAppServicio().ListarEnvios(periCodi);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult ObtenerPeriodo(int idPeriodo)
        {
            ResultadoDTO<PeriodoDeclaracionDTO> resultado = new ResultadoDTO<PeriodoDeclaracionDTO>();
            resultado.Data = new PeriodoDeclaracionAppServicio().GetBydId(idPeriodo);
            return Json(resultado);
        }
        [HttpPost]
        public ActionResult SaveSuministrosAsignados(CodigoRetiroModel parametro)
        {
            parametro.Entidad.CoesUserName = base.UserName ?? Guid.NewGuid().ToString().Substring(0, 4);
            parametro.Entidad.SoliCodiRetiFechaInicio = DateTime.ParseExact(parametro.Solicodiretifechainicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            parametro.Entidad.SoliCodiRetiFechaFin = DateTime.ParseExact(parametro.Solicodiretifechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            /*
             * Validar si codigo ya existe en algún envio
             */
            ResultadoDTO<int> resultado = new ResultadoDTO<int>();

            if (!string.IsNullOrEmpty(parametro.CodigoAnterior))
            {
                if (parametro.CodigoAnterior.Trim() != parametro.Entidad.SoliCodiRetiCodigo.Trim())
                {
                    var result = (new CodigoRetiroAppServicio()).ValidarExisteCodigoEnEnvios(parametro.Entidad.SoliCodiRetiCodigo);
                    if (result)
                    {
                        resultado.EsCorrecto = result ? (int)EnumResultado.error : 1;
                        resultado.Mensaje = "No se puede cambiar el código dado que pertenece a un proceso de carga y valorización.";
                        return Json(resultado);
                    }
                }
            }

            resultado.Data = new CodigoRetiroAppServicio().SaveSuministrosAsignados(parametro.Entidad);
            resultado.EsCorrecto = (int)EnumResultado.correcto;
            TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
            resultado.Mensaje = "Se ha registrado correctamente. Sera redireccionado a la lista de solicitudes.";
            return Json(resultado);

        }

        [HttpPost]
        public ActionResult AprobarSuministrosAsignados(CodigoRetiroModel parametro)
        {
            parametro.Entidad.CoesUserName = base.UserName ?? Guid.NewGuid().ToString().Substring(0, 4);
            parametro.Entidad.SoliCodiRetiFechaInicio = DateTime.ParseExact(parametro.Solicodiretifechainicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            parametro.Entidad.SoliCodiRetiFechaFin = DateTime.ParseExact(parametro.Solicodiretifechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            /*
             * Validar si codigo ya existe en algún envio
             */
            ResultadoDTO<int> resultado = new ResultadoDTO<int>();

            if (!string.IsNullOrEmpty(parametro.CodigoAnterior))
            {
                if (parametro.CodigoAnterior.Trim() != parametro.Entidad.SoliCodiRetiCodigo.Trim())
                {
                    var result = (new CodigoRetiroAppServicio()).ValidarExisteCodigoEnEnvios(parametro.Entidad.SoliCodiRetiCodigo);
                    if (result)
                    {
                        resultado.EsCorrecto = result ? (int)EnumResultado.error : 1;
                        resultado.Mensaje = "No se puede cambiar el código dado que pertenece a un proceso de carga y valorización.";
                        return Json(resultado);
                    }
                }
            }

            resultado.Data = new CodigoRetiroAppServicio().AprobarSuministrosAsignados(parametro.Entidad);
            resultado.EsCorrecto = (int)EnumResultado.correcto;
            TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
            resultado.Mensaje = "Se ha registrado correctamente. Sera redireccionado a la lista de solicitudes.";
            return Json(resultado);

        }

        [HttpPost]
        public ActionResult RechazarSuministrosAsignados(CodigoRetiroModel parametro)
        {
            parametro.Entidad.CoesUserName = base.UserName ?? Guid.NewGuid().ToString().Substring(0, 4);
            parametro.Entidad.SoliCodiRetiFechaInicio = DateTime.ParseExact(parametro.Solicodiretifechainicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            parametro.Entidad.SoliCodiRetiFechaFin = DateTime.ParseExact(parametro.Solicodiretifechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            /*
             * Validar si codigo ya existe en algún envio
             */
            ResultadoDTO<int> resultado = new ResultadoDTO<int>();

            /*if (!string.IsNullOrEmpty(parametro.CodigoAnterior))
            {
                if (parametro.CodigoAnterior.Trim() != parametro.Entidad.SoliCodiRetiCodigo.Trim())
                {
                    var result = (new CodigoRetiroAppServicio()).ValidarExisteCodigoEnEnvios(parametro.Entidad.SoliCodiRetiCodigo);
                    if (result)
                    {
                        resultado.EsCorrecto = result ? (int)EnumResultado.error : 1;
                        resultado.Mensaje = "No se puede cambiar el código dado que pertenece a un proceso de carga y valorización.";
                        return Json(resultado);
                    }
                }
            }*/

            resultado.Data = new CodigoRetiroAppServicio().RechazarSuministrosAsignados(parametro.Entidad);
            resultado.EsCorrecto = (int)EnumResultado.correcto;
            TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
            resultado.Mensaje = "Se ha registrado correctamente. Sera redireccionado a la lista de solicitudes.";
            return Json(resultado);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rechazar(CodigoRetiroModel parametro)
        {

            if (parametro.Entidad.EstAbrev == ConstanteEstados.Pendiente)
                parametro.Entidad.SoliCodiRetiCodigo = string.Empty;

            string agenteUsuario = parametro.Entidad.CoesUserName;


            ResultadoDTO<int> resultado = new ResultadoDTO<int>();

            parametro.Entidad.Seinusername = agenteUsuario;
            parametro.Entidad.CoesUserName = User.Identity.Name;
            parametro.Entidad.SoliCodiRetiEstado = ConstantesGestionCodigosVTEAVTP.Rechazado;
            //parametro.Entidad.SoliCodiRetiCodigo = string.Empty;

            resultado = (new CodigoRetiroAppServicio()).AprobarRechazarSolicitud(parametro.Entidad);
            if (resultado.Data > 0)
            {
                TempData["sMensajeExito"] = "La solicitud ha sido rechazada";


                resultado.EsCorrecto = (int)EnumResultado.correcto;
                resultado.Mensaje = "Se ha rechazado correctamente. Sera redireccionado a la lista de solicitudes.";
            }

            return Json(resultado);
        }


        [HttpPost]
        public ActionResult Baja(int iCoReSoCodi)
        {
            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            resultado.Data = new CodigoRetiroAppServicio().SaveDarBajaSuministro(iCoReSoCodi, User.Identity.Name);
            resultado.EsCorrecto = (int)EnumResultado.correcto;
            resultado.Mensaje = "Se realizó la baja para la solicitud.";
            return Json(resultado);
        }

        [HttpPost]
        public ActionResult BajaVTP(int iCoReSoCodi, int iCoregeCodi)
        {
            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            resultado.Data = new CodigoRetiroAppServicio().SaveDarBajaSuministroVTP(iCoReSoCodi, iCoregeCodi, User.Identity.Name);
            resultado.EsCorrecto = (int)EnumResultado.correcto;
            resultado.Mensaje = "Se realizó la baja del codigo VTP.";
            return Json(resultado);
        }

        #region "Histórico"
        [HttpPost]
        public PartialViewResult PaginadoHis(string nombreEmp, string tipousu, string tipocont, string barr, string clinomb, string fechaInicio, string fechaFin, string Solicodiretiobservacion, string radiobtn, string codretiro)
        {
            CodigoRetiroModel model = new CodigoRetiroModel
            {
                IndicadorPagina = false
            };

            string estado = "";

            if (nombreEmp.Equals("--Seleccione--") || nombreEmp.Equals(""))
                nombreEmp = null;
            if (tipousu.Equals("--Seleccione--"))
                tipousu = null;
            if (tipocont.Equals("--Seleccione--"))
                tipocont = null;
            if (barr.Equals("--Seleccione--"))
                barr = null;
            if (clinomb.Equals("--Seleccione--"))
                clinomb = null;


            DateTime? dtfi = null;
            if (string.IsNullOrEmpty(fechaInicio))
            {
                dtfi = null;
            }
            else
            {
                dtfi = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            DateTime? dtff = null;
            if (string.IsNullOrEmpty(fechaFin))
            {
                dtff = null;
            }
            else
            {
                dtff = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (radiobtn != null)
            {
                if (radiobtn.Equals("TODOS")) estado = null;
                else if (radiobtn.Equals("CON")) estado = "ASI";
                else if (radiobtn.Equals("SIN")) estado = "GEN";
            }

            model.NroRegistros = (new CodigoRetiroAppServicio()).ObtenerNroFilasCodigoRetiro(nombreEmp, tipousu, tipocont, barr, clinomb, dtfi, dtff, Solicodiretiobservacion, estado, codretiro);
            TempData["tdListaCodigoRetiro"] = (new CodigoRetiroAppServicio()).BuscarCodigoRetiro(nombreEmp, tipousu, tipocont, barr, clinomb, dtfi, dtff, Solicodiretiobservacion, estado, codretiro, 1, model.NroRegistros);
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

        [HttpPost]
        public ActionResult ListaHis(string nombreEmp, string tipousu, string tipocont, string barr, string clinomb, string fechaInicio, string fechaFin, string Solicodiretiobservacion, string radiobtn, string codretiro, int NroPagina)
        {
            string estado = "";
            var ConOSinCodigo = "";

            if (nombreEmp.Equals("--Seleccione--") || nombreEmp.Equals(""))
                nombreEmp = null;
            if (tipousu.Equals("--Seleccione--"))
                tipousu = null;
            if (tipocont.Equals("--Seleccione--"))
                tipocont = null;
            if (barr.Equals("--Seleccione--"))
                barr = null;
            if (clinomb.Equals("--Seleccione--"))
                clinomb = null;

            DateTime? dtfi = null;
            if (string.IsNullOrEmpty(fechaInicio))
            {
                dtfi = null;
            }
            else
            {
                dtfi = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            DateTime? dtff = null;
            if (string.IsNullOrEmpty(fechaFin))
            {
                dtff = null;
            }
            else
            {
                dtff = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (radiobtn != null)
            {
                if (radiobtn.Equals("TODOS")) estado = null;
                else if (radiobtn.Equals("CON"))
                {
                    //estado = "ASI";
                    estado = null; //Para que traiga todo y al final solo considere los que tiene codigo
                    ConOSinCodigo = "C";
                }
                else if (radiobtn.Equals("SIN")) estado = "PAP";
            }

            CodigoRetiroModel model = new CodigoRetiroModel
            {
                ListaCodigoRetiro = (new CodigoRetiroAppServicio()).BuscarCodigoRetiro(nombreEmp, tipousu, tipocont, barr, clinomb, dtfi, dtff, Solicodiretiobservacion, estado, codretiro, NroPagina, Funcion.PageSizeCodigoRetiro)
            };

            if (string.IsNullOrEmpty(ConOSinCodigo))
            {
                foreach (var x in model.ListaCodigoRetiro)
                {
                    if (x.SoliCodiRetiCodigo == null)
                        x.SoliCodiRetiCodigo = "Sin asignar";
                }
            }

            switch (ConOSinCodigo)
            {
                case "C":
                    model.ListaCodigoRetiro.RemoveAll(m => m.SoliCodiRetiCodigo == null);
                    break;
            }


            model.bEditar = (new Funcion()).ValidarPermisoEditar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            model.bEliminar = (new Funcion()).ValidarPermisoEliminar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        public ActionResult ViewHis(int id = 0)
        {
            CodigoRetiroModel model = new CodigoRetiroModel();
            model.Entidad = (new CodigoRetiroAppServicio()).GetByIdCodigoRetiro(id);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult GenerarExcelHis()
        {
            int indicador = 1;
            string estado = "ASI";
            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

                CodigoRetiroModel model = new CodigoRetiroModel();
                if (TempData["tdListaCodigoRetiro"] != null)
                    model.ListaCodigoRetiro = (List<CodigoRetiroDTO>)TempData["tdListaCodigoRetiro"];
                else
                    model.ListaCodigoRetiro = (new CodigoRetiroAppServicio()).ListCodigoRetiro(estado);

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCodigoRetiroExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCodigoRetiroExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE RETIRO SOLICITADOS";
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "CLIENTE";
                        ws.Cells[5, 4].Value = "RUC CLIENTE";
                        ws.Cells[5, 5].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 6].Value = "INICIO OPERACIÓN";
                        ws.Cells[5, 7].Value = "FIN OPERACIÓN";
                        ws.Cells[5, 8].Value = "TIPO CONTRATO";
                        ws.Cells[5, 9].Value = "TIPO USUARIO";
                        ws.Cells[5, 10].Value = "DESCRIPCION";
                        ws.Cells[5, 11].Value = "CÓDIGO RETIRO";

                        rg = ws.Cells[5, 2, 5, 11];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaCodigoRetiro)
                        {
                            ws.Cells[row, 2].Value = (item.EmprNombre != null) ? item.EmprNombre.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.CliNombre != null) ? item.CliNombre.ToString() : string.Empty;
                            ws.Cells[row, 4].Value = (item.CliRuc != null) ? item.CliRuc.ToString() : string.Empty;
                            ws.Cells[row, 5].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                            ws.Cells[row, 6].Value = (item.SoliCodiRetiFechaInicio != null) ? item.SoliCodiRetiFechaInicio.Value.ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[row, 7].Value = (item.SoliCodiRetiFechaFin != null) ? item.SoliCodiRetiFechaFin.Value.ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[row, 8].Value = (item.TipoContNombre != null) ? item.TipoContNombre.ToString() : string.Empty;
                            ws.Cells[row, 9].Value = (item.TipoUsuaNombre != null) ? item.TipoUsuaNombre.ToString() : string.Empty;
                            ws.Cells[row, 10].Value = (item.SoliCodiRetiDescripcion != null) ? item.SoliCodiRetiDescripcion.ToString() : string.Empty;
                            ws.Cells[row, 11].Value = (item.SoliCodiRetiCodigo != null) ? item.SoliCodiRetiCodigo.ToString() : string.Empty;
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 11];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 11);
                        rg = ws.Cells[5, 2, row, 11];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirExcelHis()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteCodigoRetiroExcel;
            return File(path, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCodigoRetiroExcel);
        }

        [HttpPost]
        public ActionResult ValidarExisteCodigoVTAEnvios(string Codigo)
        {
            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            var result = (new CodigoRetiroAppServicio()).ValidarExisteCodigoEnEnvios(Codigo);
            resultado.EsCorrecto = result ? (int)EnumResultado.error : 1;
            return Json(resultado);
        }

        #endregion "Histórico"
    }
}