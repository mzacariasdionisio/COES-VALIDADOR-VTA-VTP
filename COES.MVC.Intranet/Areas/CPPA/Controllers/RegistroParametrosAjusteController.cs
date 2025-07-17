using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using COES.MVC.Intranet.Helper;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.MVC.Intranet.Areas.DemandaCP.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System.Web.Script.Serialization;
using System.Globalization;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.Servicios.Aplicacion.DPODemanda;
using COES.Servicios.Aplicacion.DPODemanda.Helper;
using COES.Servicios.Aplicacion.Scada;
using COES.MVC.Intranet.Areas.CPPA.Models;
using COES.Servicios.Aplicacion.CPPA;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using log4net;
using System.Reflection;
using COES.Servicios.Aplicacion.CPPA.Helper;
using static COES.Servicios.Aplicacion.PotenciaFirme.ConstantesPotenciaFirmeRemunerable;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using COES.Dominio.DTO.Transferencias;
using System.ComponentModel.Design;
using DevExpress.Utils.Serializing.Helpers;
using DevExpress.ClipboardSource.SpreadsheetML;
using System.Text;

namespace COES.MVC.Intranet.Areas.CPPA.Controllers
{
    public class RegistroParametrosAjusteController : Controller
    {
        private readonly CPPAAppServicio servicio = new CPPAAppServicio();
        private readonly PronosticoDemandaAppServicio servicio1 = new PronosticoDemandaAppServicio();
        public RegistroParametrosAjusteController()
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
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        
        public ActionResult Index(string fecha, string ajuste, string revision, int idRevision)
        {
            ViewBag.Fecha = fecha;
            ViewBag.Ajuste = ajuste;
            ViewBag.Revision = revision;
            ViewBag.IdRevision = idRevision;
            return View();
        }

        /// <summary>
        /// Interfaz donde se listan las centrales y agregan nuevas
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="ajuste"></param>
        /// <param name="revision"></param>
        /// <param name="idRevision"></param>
        /// <returns></returns>
        public ActionResult ListaCentrales(string fecha, string ajuste, string revision, int idRevision, int cpaEmpresa, string nombEmpresa)
        {
            ViewBag.Fecha = fecha;
            ViewBag.Ajuste = ajuste;
            ViewBag.Revision = revision;
            ViewBag.IdRevision = idRevision;
            ViewBag.IdEmpresa = cpaEmpresa;
            ViewBag.Empresa = nombEmpresa;
            CPPAModel model = new CPPAModel()
            {
                ListCentralesGeneradoras = servicio.ListaCentralesCPPA()
            };

            return View(model);
        }

        public PartialViewResult Participantes(int revision)
        {
            return PartialView();
        }

        public PartialViewResult Generadores()
        {
            CPPAModel model = new CPPAModel()
            {
                ListEmpresasGeneracion = new List<SiEmpresaDTO>()
            };
            return PartialView(model);
        }

        /// <summary>
        /// Filtro de empresas disponibles
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        public JsonResult EmpresasDisponibles(string tipo, int revision)
        {
            CPPAModel model = new CPPAModel();

            switch (tipo)
            {
                case ConstantesCPPA.tipoEmpresaGeneradora:
                    model.ListEmpresasGeneracion = servicio.ListaEmpresasTipoCPPA(revision, tipo);
                    break;
                case ConstantesCPPA.tipoEmpresaDistribuidora:
                    model.ListEmpresasDistribucion = servicio.ListaEmpresasTipoCPPA(revision, tipo);
                    break;
                case ConstantesCPPA.tipoEmpresaUsuarioLibre:
                    model.ListEmpresasLibres = servicio.ListaEmpresasTipoCPPA(revision, tipo);
                    break;
                default:
                    model.ListEmpresasTransmision = servicio.ListaEmpresasTipoCPPA(revision, tipo);
                    break;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        /// <summary>
        /// Lista las una empresas generadoras registradas
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="estado"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public JsonResult ListaEmpresasIntegrantes(int revision, string estado, int tipo)
        {
            CPPAModel model = new CPPAModel();

            switch (tipo)
            {
                case 1:
                    model.ListEmpresasIntegrantes = servicio.ListaEmpresasIntegrantes(revision, estado, $"'{ConstantesCPPA.tipoEmpresaGeneradora}'");
                    break;
                case 2:
                    model.ListEmpresasIntegrantes = servicio.ListaEmpresasIntegrantes(revision, estado, $"'{ConstantesCPPA.tipoEmpresaDistribuidora}'");
                    break;
                case 3:
                    model.ListEmpresasIntegrantes = servicio.ListaEmpresasIntegrantes(revision, estado, $"'{ConstantesCPPA.tipoEmpresaUsuarioLibre}'");
                    break;
                default:
                    model.ListEmpresasIntegrantes = servicio.ListaEmpresasIntegrantes(revision, estado, $"'{ConstantesCPPA.tipoEmpresaTransmisora}'");
                    break;
            }
            return Json(model);
        }

        /// <summary>
        /// Registra una empresa generadora, distribuidora, transmisora y usaurio libre
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="empresa"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public JsonResult RegistraEmpresaIntegrante(int revision, int empresa, int tipo)
        {
            CPPAModel model = new CPPAModel();
            List<CpaEmpresaDTO> lista = new List<CpaEmpresaDTO>();
            try
            {
                switch (tipo)
                {
                    case 1:
                        lista = servicio.ListaEmpresaPorRevisionTipo(revision, ConstantesCPPA.tipoEmpresaGeneradora, empresa).Where(x => x.Cpaempestado == ConstantesCPPA.eActivo).ToList();
                        if (lista.Count > 0)
                        {
                            model.sMensaje = "La empresa ya fue registrada, vuelva a cargar la ventana...";
                            model.sResultado = "2";
                            return Json(model);
                        }
                        servicio.RegistraEmpresaIntegrante(revision, empresa, User.Identity.Name, ConstantesCPPA.tipoEmpresaGeneradora);
                        break;
                    case 2:
                        lista = servicio.ListaEmpresaPorRevisionTipo(revision, ConstantesCPPA.tipoEmpresaDistribuidora, empresa).Where(x => x.Cpaempestado == ConstantesCPPA.eActivo).ToList();
                        if (lista.Count > 0)
                        {
                            model.sMensaje = "La empresa ya fue registrada, vuelva a cargar la ventana...";
                            model.sResultado = "2";
                            return Json(model);
                        }
                        servicio.RegistraEmpresaIntegrante(revision, empresa, User.Identity.Name, ConstantesCPPA.tipoEmpresaDistribuidora);
                        break;
                    case 3:
                        lista = servicio.ListaEmpresaPorRevisionTipo(revision, ConstantesCPPA.tipoEmpresaUsuarioLibre, empresa).Where(x => x.Cpaempestado == ConstantesCPPA.eActivo).ToList();
                        if (lista.Count > 0)
                        {
                            model.sMensaje = "La empresa ya fue registrada, vuelva a cargar la ventana...";
                            model.sResultado = "2";
                            return Json(model);
                        }
                        servicio.RegistraEmpresaIntegrante(revision, empresa, User.Identity.Name, ConstantesCPPA.tipoEmpresaUsuarioLibre);
                        break;
                    default:
                        lista = servicio.ListaEmpresaPorRevisionTipo(revision, ConstantesCPPA.tipoEmpresaTransmisora, empresa).Where(x => x.Cpaempestado == ConstantesCPPA.eActivo).ToList();
                        if (lista.Count > 0)
                        {
                            model.sMensaje = "La empresa ya fue registrada, vuelva a cargar la ventana...";
                            model.sResultado = "2";
                            return Json(model);
                        }
                        servicio.RegistraEmpresaIntegrante(revision, empresa, User.Identity.Name, ConstantesCPPA.tipoEmpresaTransmisora);
                        break;
                }
                model.sMensaje = "Todo correcto";
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }


        /// <summary>
        /// Anula una empresa integrante modificando su estado de Activo a Anulado
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="empresa"></param>
        /// <param name="revision"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public JsonResult AnulaEmpresaIntegrante(int codigo, int empresa, int revision, string tipo)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                object resMessage = servicio.AnulaEmpresaIntegrante(codigo, empresa, revision, tipo, User.Identity.Name);
                model.sMensaje = (string)resMessage.GetType().GetProperty("dataMsg").GetValue(resMessage);
                model.sTipo = (string)resMessage.GetType().GetProperty("typeMsg").GetValue(resMessage);
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Lista las una empresas generadoras registradas
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="revision"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public JsonResult ListaCentralesIntegrantes(int empresa, int revision, string estado)
        {
            CPPAModel model = new CPPAModel();
            model.ListCentralesIntegrantes = servicio.ListaCentralesIntegrantes(empresa, revision, estado);
            return Json(model);
        }

        /// <summary>
        /// Filtro actualizado de las centrales disponibles
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public JsonResult CentralesDisponibles(int revision, int empresa)
        {
            CPPAModel model = new CPPAModel()
            {
                //ListaEmpresasTipoCPPA//ListaCentralesCPPA
                ListCentralesGeneradoras = servicio.ListaCentralesTipoCPPA(revision, empresa)
            };

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        /// <summary>
        /// Registra una central para una empresa integrante
        /// </summary>
        /// <param name="anio">
        /// <param name="empresa">Identificador de la tabla CPA_EMPRESA</param>
        /// <param name="revision"></param>
        /// <param name="empresa"></param>
        /// <param name="ajuste"></param>
        /// <returns></returns>
        public JsonResult RegistraCentralIntegrante(int anio, int empresa, int revision, int central, string ajuste)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                List<CpaCentralDTO> lista = servicio.ListaCentralesPorEmpresaRevison(empresa, revision, central).Where(x => x.Cpacntestado == ConstantesCPPA.eActivo).ToList();
                if (lista.Count > 0)
                {
                    model.sMensaje = "Ya se registro esta central...";
                    model.sResultado = "2";
                    return Json(model);
                }

                CpaCentralDTO validacionCentral = servicio.ListaCentralesPorRevison(revision, central).Where(x => x.Cpacntestado == ConstantesCPPA.eActivo).FirstOrDefault();
                if (validacionCentral != null)
                {
                    model.sMensaje = $"Ya se registro esta central en la empresa {validacionCentral.Emprnomb}";
                    model.sResultado = "2";
                    return Json(model);
                }

                int anioPresupuestal = anio - 1;
                servicio.RegistraCentralIntegrante(anioPresupuestal, empresa, revision, central, ajuste, User.Identity.Name);
                model.sMensaje = "Todo correcto";
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }


        /// <summary>
        /// Anula una empresa integrante modificando su estado de Activo a Anulado
        /// </summary>
        /// <param name="central"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public JsonResult AnulaCentralIntegrante(int central, int empresa)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                servicio.AnulaCentralIntegrante(central, empresa, User.Identity.Name);
                model.sMensaje = "Todo correcto";
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Vista parcial de Empresas Integrantes - Distribuidores
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Distribuidores()
        {
            CPPAModel model = new CPPAModel()
            {
                ListEmpresasDistribucion = new List<SiEmpresaDTO>()
            };
            return PartialView(model);
        }

        /// <summary>
        /// Vista parcial de Empresas Integrantes - Usuarios Libres
        /// </summary>
        /// <returns></returns>
        public PartialViewResult UsuariosLibres()
        {
            CPPAModel model = new CPPAModel()
            {
                ListEmpresasLibres = new List<SiEmpresaDTO>()
            };
            return PartialView(model);
        }

        /// <summary>
        /// Vista parcial de Empresas Integrantes - Transmisoras
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Transmisores()
        {
            CPPAModel model = new CPPAModel()
            {
                ListEmpresasTransmision = new List<SiEmpresaDTO>()
            };
            return PartialView(model);
        }

        public PartialViewResult Centrales(int revision)
        {
            CPPAModel model = new CPPAModel()
            {
                filtroEmpresasIntegrantes = servicio.FiltroEmpresasIntegrantes(revision),
                filtroCentralesIntegrantes = servicio.FiltroCentralesIntegrantes(revision),
                filtroBarrasTransIntegrantes = servicio.FiltroBarrasTransIntegrantes(revision),
                popupfiltroBarrasTransferencia = new List<BarraDTO>(),
                popupfiltroCentralesPMPO = new List<MePtomedicionDTO>(),
                popupfiltroBarrasPMPO =  new List<MePtomedicionDTO>()
            };
            return PartialView(model);
        }

        /// <summary>
        /// Llena el filtro centrales pmpo y lista centrales agregadas(grilla popup)
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        public JsonResult ListaCentralesAgregadas(int empresa, int central)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                model.popupfiltroCentralesPMPO = servicio.ListaCentralesFiltradasPMPO(empresa, central);
                model.popupfiltroBarrasTransferencia = servicio.ListaBarrasTransFormato();
                model.popupfiltroBarrasPMPO = servicio.ListaBarrasPMPO();
                model.popupGrillaCentralesPMPO = servicio.ListCpaCentralPmpobyCentral(central);
                model.sMensaje = "Todo correcto";
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Llena el filtro centrales pmpo y lista centrales agregadas(grilla popup)
        /// </summary>
        /// <param name = "revision" > Identificador de la revision</param>
        /// <param name="ajuste">Ajuste de la revision</param>
        /// <param name="id">Identificador de la tabla CPA_CENTRAL</param>
        /// <param name="transferencia">Identificador de la tabla CPA_CENTRAL</param>
        /// <param name="barraPMPO">Identificador de la tabla CPA_CENTRAL</param>
        /// <param name="centralesPMPO">Identificador de la tabla CPA_CENTRAL</param>
        /// <param name="ejecInicio">Identificador de la tabla CPA_CENTRAL</param>
        /// <param name="ejecFin">Identificador de la tabla CPA_CENTRAL</param>
        /// <param name="progInicio">Identificador de la tabla CPA_CENTRAL</param>
        /// <param name="progFin">Identificador de la tabla CPA_CENTRAL</param>
        /// <returns></returns>
        public JsonResult RegistraEquiposPMPO(int revision, string ajuste, int id, int transferencia, int barraPMPO, List<MePtomedicionDTO> centralesPMPO, string ejecInicio, string ejecFin, string progInicio, string progFin)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                servicio.RegistraCentralPMPO(revision, ajuste, id, transferencia, barraPMPO, centralesPMPO, ejecInicio, ejecFin, progInicio, progFin, User.Identity.Name);
                model.sMensaje = "Se realizó el registro de parámetros de la central de generación";
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Informacion para la grilla principal
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="central"></param>
        /// <param name="empresa"></param>
        /// <param name="barraTrans"></param>
        /// <returns></returns>
        public JsonResult ListaGrillaPrincipal(int revision, int central, int empresa, int barraTrans)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                model.GrillaPrincipal = servicio.ListaCentralesEmpresasParticipantes(revision, central, empresa, barraTrans);
                model.sMensaje = "Todo correcto";
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Vista parcial de Maxima demanda y Precio potencia
        /// </summary>
        /// <param name="revision"></param>
        /// <returns></returns>
        public PartialViewResult Demanda(int revision)
        {
            return PartialView();
        }

        /// <summary>
        /// Lista parametros registrados para las revisiones
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="estado"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        public JsonResult ListaParametrosRegistrados(int revision, string[] estado, int anio)
        {
            CPPAModel model = new CPPAModel();
            string result = string.Join(", ", estado.Select(d => $"'{d}'"));
            model.GrillaPrincipalParametros = servicio.ListaParametrosRegistrados(revision, result, anio);
            return Json(model);
        }


        /// <summary>
        /// Registra los parametros
        /// </summary>
        /// <param name="revision">Identificador de la tabla CPA_REVISION</param>
        /// <param name="anio">Identificador de la tabla CPA_EMPRESA</param>
        /// <param name="mes"></param>
        /// <param name="registro"></param>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="cambio"></param>
        /// <param name="precio"></param>
        /// <returns></returns>
        public JsonResult RegistrarParametros(int revision, int anio, int mes, string registro, string fecha, string hora, decimal cambio, decimal precio)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                object resMessage = servicio.RegistrarParametros(revision, anio, mes, registro, fecha, hora, cambio, precio, User.Identity.Name);
                model.sMensaje = (string)resMessage.GetType().GetProperty("dataMsg").GetValue(resMessage);
                model.sTipo = (string)resMessage.GetType().GetProperty("typeMsg").GetValue(resMessage);
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Actualiza los parametros
        /// </summary>
        /// <param name="codigo">Identificador de la tabla CPA_PARAMETRO</param>
        /// <param name="registro"></param>
        /// <param name="cambio"></param>
        /// <returns></returns>
        public JsonResult EditarParametros(int codigo, int anio, int mes, string registro, string fecha, string hora, decimal cambio, decimal precio)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                object resMessage = servicio.EditarParametros(codigo, anio, mes, registro, fecha, hora, cambio, precio, User.Identity.Name);
                model.sMensaje = (string)resMessage.GetType().GetProperty("dataMsg").GetValue(resMessage);
                model.sTipo = (string)resMessage.GetType().GetProperty("typeMsg").GetValue(resMessage);
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Anula un registro de los parametros
        /// </summary>
        /// <param name="codigo">Identificador de la tabla CPA_PARAMETRO</param>
        /// <returns></returns>
        public JsonResult AnularParametros(int codigo)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                servicio.AnularParametros(codigo, User.Identity.Name);
                model.sMensaje = "Todo correcto";
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Historico de un registro
        /// </summary>
        /// <param name="codigo">Identificador de la tabla CPA_PARAMETRO</param>
        /// <returns></returns>
        public JsonResult HistoricoParametros(int codigo)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                model.GrillaHistoricoParametros = servicio.ListaParametrosHistoricos(codigo);
                model.sMensaje = "Todo correcto";
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Copiar los datos MD de LVTP
        /// </summary>
        /// <param name="revision">Identificador de la revision</param>
        /// <param name="anio">anio</param>
        /// <returns></returns>
        public JsonResult CopiarParametros(int revision, int anio)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                model.sMensaje = servicio.CopiarMDLVTP(revision, anio, User.Identity.Name);
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Exportar la grilla de empresas a un documento excel.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="revision"></param>
        /// <param name="ajuste"></param>
        /// <param name="anio"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public JsonResult ExportarEmpresas(string[][] form, string revision, string ajuste, string anio, string tipo)
        {
            if (revision == "Normal")
            {
                revision = "";
            }
            else
            {
                revision = " - " + revision;
            }
            CPPAFormatoExcel data = new CPPAFormatoExcel()
            {

                Contenido = form,
                Cabecera = new string[] {
                    "EMPRESA", "ESTADO",
                    "USUARIO CREACION", "FECHA CREACION", "USUARIO MODIFICACION", "FECHA MODIFICACION"
                },
                AnchoColumnas = new int[] {
                    50,50,50,50,50,50
                },
                Titulo = "EMPRESAS " + tipo,
                Subtitulo1 = "Presupuesto " + anio + " - " + ajuste + revision,//revision + " - " + ajuste + " - " + anio,
                //Subtitulo2 = "MAS DATOS"
            };
            string pathFile = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteCPPA].ToString();
            string filename = "Presupuesto " + anio + " - " + ajuste + revision + " " +tipo;
            string reporte = this.servicio.ExportarReporteSimple(data, pathFile, filename);

            return Json(reporte);
        }

        /// <summary>
        /// Exportar la grilla de empresas a un documento excel.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="revision"></param>
        /// <param name="ajuste"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        public JsonResult ExportarParametros(string[][] form, string revision, string ajuste, string anio)
        {
            if (revision == "Normal")
            {
                revision = "";
            }
            else {
                revision = " - " + revision;
            }
            CPPAFormatoExcel data = new CPPAFormatoExcel()
            {

                Contenido = form,
                Cabecera = new string[] {
                    "ESTADO","AÑO - MES EJERCICIO ANTERIOR", "TIPO REGISTRO MD",
                    "FECHA MD", "HORA MD", "TIPO DE CAMBIO S/ - U$$", "PRECIO POTENCIA S/ kW-MES", "USUARIO CREACIÓN", "FECHA CREACIÓN", "USUARIO ACTUALIZACIÓN", "FECHA ACTUALIZACIÓN"
                },
                AnchoColumnas = new int[] {
                    50,50,50,50,50,50,50,50,50,50,50
                },
                Titulo = "Parámetros de Ajuste",
                Subtitulo1 = "Presupuesto " + anio + " - " + ajuste + revision,
                //Subtitulo2 = "MAS DATOS"
            };
            string pathFile = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteCPPA].ToString();
            string filename = "Presupuesto " + anio + " - " + ajuste + revision + " ParametrosAjuste";
            string reporte = this.servicio.ExportarReporteSimple(data, pathFile, filename);

            return Json(reporte);
        }

        /// <summary>
        /// Exportar la grilla de centrales a un documento excel.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="revision"></param>
        /// <param name="ajuste"></param>
        /// <param name="anio"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public JsonResult ExportarCentrales(string[][] form, string revision, string ajuste, string anio, string empresa)
        {
            CPPAFormatoExcel data = new CPPAFormatoExcel()
            {

                Contenido = form,
                Cabecera = new string[] {
                    "CENTRAL", "ESTADO",
                    "USUARIO CREACION", "FECHA CREACION", "USUARIO MODIFICACION", "FECHA MODIFICACION"
                },
                AnchoColumnas = new int[] {
                    50,50,50,50,50,50
                },
                Titulo = "CENTRALES DE GENERACION" + " - " + empresa,
                Subtitulo1 = revision + " - " + ajuste + " - " + anio,
                //Subtitulo2 = "MAS DATOS"
            };
            string pathFile = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteCPPA].ToString();
            string filename = "Reporte Centrales CPPA";
            string reporte = this.servicio.ExportarReporteSimple(data, pathFile, filename);

            return Json(reporte);
        }

        public JsonResult ExportarCentralesPMPO(string[][] form, string revision, string ajuste, string anio)
        {
            if (revision == "Normal")
            {
                revision = "";
            }
            else
            {
                revision = " - " + revision;
            }
            List<PrnFormatoExcel> datos = new List<PrnFormatoExcel>();

            //Creating excel book
            PrnFormatoExcel book = new PrnFormatoExcel()
            {
                Titulo = "CENTRALES DE GENERACIÓN",
                Subtitulo1 = "Presupuesto " + anio + " - " + ajuste + revision,
                AnchoColumnas = new int[] { 50, 50, 20, 20, 20, 20, 50, 50, 30, 30, 30 },
                NombreLibro = "Centrales"
            };

            book.NestedHeader1 = new List<PrnExcelHeader>
            {
                new PrnExcelHeader() { Etiqueta = "", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "GENERACIÓN EJECUTADA", Columnas = 2 },
                new PrnExcelHeader() { Etiqueta = "GENERACIÓN PROGRAMADA", Columnas = 2 },
                new PrnExcelHeader() { Etiqueta = "", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "", Columnas = 1 }
            };

            book.NestedHeader2 = new List<PrnExcelHeader>
            {
                new PrnExcelHeader() { Etiqueta = "CENTRAL", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "EMPRESA", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "INICIO", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "FIN", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "INICIO", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "FIN", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "BARRA TRANSFERENCIA", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "BARRA PMPO", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "USUARIO ACTUALIZACIÓN", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "FECHA ACTUALIZACIÓN", Columnas = 1 },
                new PrnExcelHeader() { Etiqueta = "CENTRALES PMPO", Columnas = 1 }
            };

            book.Contenido = form;
            datos.Add(book);

            string pathFile = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteCPPA].ToString();
            string filename = "Presupuesto " + anio + " - " + ajuste + revision + " Centrales";
            string reporte = this.servicio.ExportarReporteConLibros(datos, pathFile, filename);

            return Json(reporte);
        }

        // <summary>
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string app = Constantes.AppExcel;

            StringBuilder rutaNombreArchivo = new StringBuilder();
            rutaNombreArchivo.Append(ConfigurationManager.AppSettings[ConstantesCPPA.ReporteCPPA].ToString());
            rutaNombreArchivo.Append(file);

            StringBuilder rutaNombreArchivoDescarga = new StringBuilder();
            rutaNombreArchivoDescarga.Append(file);

            byte[] bFile = System.IO.File.ReadAllBytes(rutaNombreArchivo.ToString());
            System.IO.File.Delete(rutaNombreArchivo.ToString());

            return File(bFile, app, rutaNombreArchivoDescarga.ToString());
        }

        // <summary>
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        public virtual ActionResult AbrirArchivoParametros(int formato, string file)
        {
            string app = Constantes.AppExcel;

            StringBuilder rutaNombreArchivo = new StringBuilder();
            rutaNombreArchivo.Append(ConfigurationManager.AppSettings[ConstantesCPPA.ReporteCPPA].ToString());
            rutaNombreArchivo.Append(file);

            StringBuilder rutaNombreArchivoDescarga = new StringBuilder();
            rutaNombreArchivoDescarga.Append(file);

            byte[] bFile = System.IO.File.ReadAllBytes(rutaNombreArchivo.ToString());
            System.IO.File.Delete(rutaNombreArchivo.ToString());

            return File(bFile, app, rutaNombreArchivoDescarga.ToString());
        }

    }
}