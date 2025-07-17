using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Titularidad.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Titularidad;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Titularidad.Controllers
{
    public class TransferenciaController : BaseController
    {
        TitularidadAppServicio servTitEmp = new TitularidadAppServicio();
        SeguridadServicioClient servSeguridad = new SeguridadServicioClient();
        TransferenciasAppServicio servicioTransferencia = new TransferenciasAppServicio();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(TransferenciaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Almacena Name del archivo Excel cargado en el servidor
        /// </summary>
        public String FileName
        {
            get
            {
                return (Session[CodigosParametro.SesionFileName] != null) ?
                    Session[CodigosParametro.SesionFileName].ToString() : null;
            }
            set { Session[CodigosParametro.SesionFileName] = value; }
        }

        public String NombreFile
        {
            get
            {
                return (Session[CodigosParametro.SesionNombreArchivo] != null) ?
                    Session[CodigosParametro.SesionNombreArchivo].ToString() : null;
            }
            set { Session[CodigosParametro.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        public TransferenciaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion

        #region Página Principal

        /// <summary>
        /// Página Principal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            TransferenciaModel model = new TransferenciaModel();
            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.ListaEmpresaOrigen = servTitEmp.ListAllEmpresas();
            model.ListaEmpresaDestino = servTitEmp.ListAllEmpresas();
            model.IndiceEmpresaOrigen = 0;
            model.IndiceEmpresaDestino = 0;

            return View(model);
        }

        /// <summary>
        /// Listado transferencias según Empresa origen, empresa destino y/o descripción 
        /// </summary>
        /// <param name="empresaOrigen"></param>
        /// <param name="empresaDestino"></param>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoTransferencias(int empresaOrigen, int empresaDestino, string descripcionMigracion, int estadoAnulado)
        {
            if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

            TransferenciaModel model = new TransferenciaModel();
            model.ListadoTransferencias = servTitEmp.ListarTransferenciasXEmpresaOrigenXEmpresaDestino(empresaOrigen, empresaDestino, descripcionMigracion, estadoAnulado);

            return PartialView(model);
        }

        /// <summary>
        /// Exportar el listado de migraciones
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarExcelListadoTransferencias()
        {
            string ruta = string.Empty;
            string[] datos = new string[2];
            try
            {
                servTitEmp.ExportarReporte();

                var jsonResult = Json(datos);
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        /// <summary>
        /// Descarga el reporte excel del servidor
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult DescargarReporte()
        {
            string nombreArchivo = ConstantesTitularidad.NombreReporte;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesTitularidad.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #endregion

        #region Detalle de Transferencia

        /// <summary>
        /// Detalle de transferencia
        /// </summary>
        /// <param name="idTransferencia"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DetalleTransferencia(int idTransferencia)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            var oTransferencia = servTitEmp.GetByIdSiMigracion(idTransferencia);
            var oAnulado = servTitEmp.GetSiMigracionAnuladoByIdDeleted(oTransferencia.Migracodi);

            var lsEquipo = servTitEmp.ListarEquiposByMigracion(idTransferencia);
            var lPto = servTitEmp.ListarPtomedicionByMigracodi(idTransferencia);
            var lGrupo = servTitEmp.ListarGrupoByMigracodi(idTransferencia);
            var listaLog = servTitEmp.ListarLogByMigracion(idTransferencia, oTransferencia.Tmopercodi);
            var listQuerysEjecutDba = servTitEmp.ListarLogDbaByMigracion(idTransferencia, oTransferencia.Tmopercodi);
            var listaDetalleAdicional = servTitEmp.ListarDetalleAdicionalXMigracion(idTransferencia);

            TransferenciaModel modelo = new TransferenciaModel
            {
                Migracion = oTransferencia,
                ListaEquipo = lsEquipo,
                ListaPtoMedicion = lPto,
                ListaGrupo = lGrupo,
                ListaLog = listaLog,
                ListaDetalleAdicional = listaDetalleAdicional,

                ListQuerysEjecutDba = listQuerysEjecutDba,

                TieneFechaCorte = TitularidadAppServicio.TieneFechaCorte(oTransferencia.Tmopercodi),
                EsRegQueAnulaOtroReg = oAnulado != null,
                Migracodi = oAnulado != null ? oAnulado.Migracodi : 0,
                TieneProcesoStrPendiente = oTransferencia.Migrareldeleted > 0 || oAnulado != null ? false : (oTransferencia.Migraflagstr == ConstantesTitularidad.FlagProcesoStrNo)
            };
            modelo.TieneProcesoStrPendiente = TitularidadAppServicio.TieneTransferirEmpresa(oTransferencia.Tmopercodi) ? modelo.TieneProcesoStrPendiente : false;

            //Permisos DBA
            var objUsuario = this.servSeguridad.ObtenerUsuarioPorLogin(User.Identity.Name);
            modelo.TienePermisoLogDBA = objUsuario != null && objUsuario.AreaCode == ConstantesTitularidad.AreacodiDTI;

            modelo.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);

            return View(modelo);
        }

        /// <summary>
        /// Anular transferencia
        /// </summary>
        /// <param name="migracodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AnularTransferencia(int migracodi)
        {
            var model = new TransferenciaModel();

            try
            {
                this.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                this.servTitEmp.AnularTransferencia(migracodi, User.Identity.Name);

                model.Resultado = ConstantesAppServicio.ParametroOK;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Anular transferencia
        /// </summary>
        /// <param name="migracodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarStr(int migracodi)
        {
            var model = new TransferenciaModel();

            try
            {
                this.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                this.servTitEmp.RegistrarTransferenciaStr(migracodi, User.Identity.Name);

                model.Resultado = ConstantesAppServicio.ParametroOK;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion

        #region Registro de Transferencia

        /// <summary>
        /// Registrar nueva transferencia
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Transferencia()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            TransferenciaModel model = new TransferenciaModel();

            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            model.ListaTipoOperacion = servTitEmp.ListSiTipomigraoperacions();
            model.ListaEmpresaAnidada = servTitEmp.ListAllEmpresas();

            return View(model);
        }

        /// <summary>
        /// Obtiene RUC y Razón Social de una empresa seleccionada, en el Registro de Nueva Transferencia
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerEmpresaById(int idEmpresa)
        {
            TransferenciaModel model = new TransferenciaModel();
            try
            {
                this.ValidarSesionJsonResult();

                model.Empresa = servTitEmp.GetSiEmpresaById(idEmpresa);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Listado areas, equipo, grupos y puntos de medición por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarDatosXEmpresa(int idEmpresa)
        {
            var model = new TransferenciaModel();

            try
            {
                this.ValidarSesionJsonResult();

                model.ListaEquipo = servTitEmp.ListarEquiposXEmpresa(idEmpresa);
                model.ListaArea = servTitEmp.ListarAreaXEmpresa(idEmpresa);
                model.ListaFamilia = servTitEmp.ListarFamiliaXEmp(idEmpresa);

                model.Resultado = ConstantesAppServicio.ParametroOK;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        /// <summary>
        /// Validar si ya existe una migracion con los mismos datos
        /// </summary>
        /// <param name="tmopercodi"></param>
        /// <param name="emprcodiOrigen"></param>
        /// <param name="emprcodi"></param>
        /// <param name="strFechaCorte"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarNuevaMigracion(int tmopercodi, int emprcodiOrigen, int emprcodi, string strFechaCorte)
        {
            var model = new TransferenciaModel();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime fechaCorte = DateTime.Now;

                SiMigracionDTO migracion = new SiMigracionDTO()
                {
                    Emprcodi = emprcodi,
                    Tmopercodi = tmopercodi,
                    Migrafeccorte = fechaCorte,
                };
                SiMigraemprOrigenDTO migraOrigen = new SiMigraemprOrigenDTO() { Emprcodi = emprcodiOrigen };

                if (TitularidadAppServicio.TieneFechaCorte(tmopercodi))
                {
                    if (!string.IsNullOrEmpty(strFechaCorte))
                    {
                        fechaCorte = DateTime.ParseExact(strFechaCorte, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        if (fechaCorte.Date > DateTime.Now.Date)
                            throw new Exception("La Fecha de corte no debe ser después a la fecha de hoy");
                    }
                    else
                    {
                        throw new Exception("Debe ingresar una Fecha de corte");
                    }
                }

                // if (!servTitEmp.ValidarExistenciaMigracion(migracion, migraOrigen))
                model.Resultado = ConstantesAppServicio.ParametroOK;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Generar html resumen de la transferencia a procesar
        /// </summary>
        /// <param name="strTransf"></param>
        /// <returns></returns>
        public JsonResult GenerarViewProcesarHtml(string strTransf)
        {
            var model = new TransferenciaModel();
            try
            {
                this.ValidarSesionJsonResult();

                SiMigracionDTO migracion;
                SiMigraemprOrigenDTO migraOrigen;
                List<int> listaEquipo;
                bool regHistoricoTransf;
                this.GetObjetosTransferencia(strTransf, out migracion, out migraOrigen, out listaEquipo, out regHistoricoTransf);

                model.Resultado = servTitEmp.GenerarViewProcesarHtml(migracion, migraOrigen, listaEquipo);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Procesar solo una transferencia de Equipos
        /// </summary>
        /// <param name="empresa_destino"></param>
        /// <param name="descripcion"></param>
        /// <param name="tipo_migracion"></param>
        /// <param name="empresa_origen"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarTransferencia(string strTransf)
        {
            var model = new TransferenciaModel();

            try
            {
                this.ValidarSesionJsonResult();
                //if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                SiMigracionDTO migracion;
                SiMigraemprOrigenDTO migraOrigen;
                List<int> listaEquipo;
                bool regHistoricoTransf;
                //ASSETEC: 20210903
                model.Mensaje = "";
                model.Detalle = "";
                //-----------------
                this.GetObjetosTransferencia(strTransf, out migracion, out migraOrigen, out listaEquipo, out regHistoricoTransf);
                regHistoricoTransf = false;
                model.Migracodi = servTitEmp.RegistrarTransferencia(migracion, migraOrigen, listaEquipo, regHistoricoTransf);
                model.Resultado = ConstantesAppServicio.ParametroOK;

                #region ASSETEC: 202108: Se ejecuta el procedimiento de Proceso para Mercados
                /*if (migracion.MigraflagPM == 1 && (migracion.Tmopercodi == ConstantesTitularidad.TipoMigrCambioRazonSocial || migracion.Tmopercodi == ConstantesTitularidad.TipoMigrFusion))
                {
                    // model.Migracodi -> Para guardar un log de transacciones por cada proceso de migración (Crear tabla)
                    // migraOrigen.Emprcodi -> Empresa de Origen
                    // migracion.Emprcodi -> Empresa de Destino
                    Log.Info("Ejecutando en TIEE - Proceso para Mercados");
                    string sMensaje = "";
                    string sDetalle = "";
                    string sResultado = this.servicioTransferencia.MigrarProcesoMercado(model.Migracodi, migracion.Migradescripcion, migraOrigen.Emprcodi, migracion.Emprcodi, base.UserName, out sMensaje, out sDetalle);
                    model.Mensaje = sMensaje;
                    model.Detalle = sDetalle;
                    Log.Info("Ejecutado Proceso para Mercados | Fecha : " + DateTime.Now + " | Usuario: " + base.UserName);
                }*/
                #endregion

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Obtener objetos de transferencia de un objeto Json
        /// </summary>
        /// <param name="strTransf"></param>
        /// <param name="migracion"></param>
        /// <param name="migraOrigen"></param>
        /// <param name="listaEquipo"></param>
        private void GetObjetosTransferencia(string strTransf, out SiMigracionDTO migracion, out SiMigraemprOrigenDTO migraOrigen, out List<int> listaEquipo, out bool regHistoricoTransf)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            TransferenciaParametroModel objTransf = strTransf != null ? serializer.Deserialize<TransferenciaParametroModel>(strTransf) : new TransferenciaParametroModel();

            DateTime fechaCorte = DateTime.Now;
            if (!string.IsNullOrEmpty(objTransf.StrFechaCorte))
                fechaCorte = DateTime.ParseExact(objTransf.StrFechaCorte, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            migracion = new SiMigracionDTO()
            {
                Emprcodi = objTransf.Emprcodi,
                Tmopercodi = objTransf.Tmopercodi,
                Migrafeccorte = fechaCorte,
                Migradescripcion = (objTransf.Descripcion != null ? objTransf.Descripcion.Trim() : string.Empty),
                Migrausucreacion = User.Identity.Name,
                Migraflagstr = objTransf.RegStrTransf,
                MigraflagPM = objTransf.RegPM 
            };

            migraOrigen = new SiMigraemprOrigenDTO() { Emprcodi = objTransf.EmprcodiOrigen, Migempusucreacion = User.Identity.Name };

            listaEquipo = objTransf.ListaEquicodi;

            regHistoricoTransf = objTransf.RegHistoricoTransf;

            //ENGIE HACIA SOUTHERN PERU CC 20/10/17
            if (objTransf.EmprcodiOrigen == 48 && objTransf.Emprcodi == 29)
            {
                string str1 = "7099,7124,7101,7102,7103,7104,7105,7106,7107,7112,7108,7109,7110,7111,7113,7114,7115,7116,7117,7118,7119,7120,7121,7122,7123,7100";
                listaEquipo = str1.Split(',').Select(int.Parse).ToList();
            }

            //LUZ DEL SUR HACIA INLAND 28/02/18
            if (objTransf.EmprcodiOrigen == 13 && objTransf.Emprcodi == 12634)
            {
                string str1 = "18915, 18916, 18917, 18918, 18924, 18920, 18921, 18922, 18923, 18919 ";
                listaEquipo = str1.Split(',').Select(int.Parse).ToList();
            }


            //ENEL GENERACION PERU S.A.A. HACIA REFINERIA ZINC CAJAMARQUILLA  20/12/16
            if (objTransf.EmprcodiOrigen == 12096 && objTransf.Emprcodi == 60)
            {
                string str1 = "6670,6687,6685";
                listaEquipo = str1.Split(',').Select(int.Parse).ToList();
            }

            //REFINERIA ZINC CAJAMARQUILLA HACIA NEXA RESOURCES CAJAMARQUILLA S.A.  23/05/18
            if (objTransf.EmprcodiOrigen == 60 && objTransf.Emprcodi == 10489
)
            {
                string str1 = "18992, 19013, 18994, 18995, 18996, 18997, 18998, 18999, 19000, 19001, 19002, 19003, 19004, 19005, 19006, 19007, 19008, 19009, 19010, 19011, 19012, 18993";
                listaEquipo = str1.Split(',').Select(int.Parse).ToList();
            }

            //STATKRAFT S.A HACIA INVERSIONES SHAQSHA S.A.C. 25/01/18
            /*if (objTransf.EmprcodiOrigen == 12758 && objTransf.Emprcodi == 12624
)
            {
                string str1 = "19014,19015,19019,19017,19018,19016 ";
                listaEquipo = str1.Split(',').Select(int.Parse).ToList();
            }*/


            // ATN PATRIMONIO EN FIDEICOMISO HACIA  ATN S.A. 01/02/17
            if (objTransf.EmprcodiOrigen == 11114 && objTransf.Emprcodi == 12240)
            {
                string str1 = " 6768, 6769, 6770, 6771, 6772, 6773, 6774, 6775, 6776, 6777, 6778, 6779, 6780, 6781, 6782, 6783, 6784, 6786, 6787, 6788, 6790, 6791, 6792, 6793, 6794, 6795, 6796, 6797, 6799, 6800, 6801, 6802, 6803, 6804, 6805, 6806, 6807, 6808, 6809, 6810, 6811, 6812, 6813, 6814, 6815, 6816, 6817, 6818, 6819, 6820, 6821, 6822, 6825, 6826, 6827, 6765, 6766, 6767, 6828, 6830, 6831, 6832, 6833, 6834, 6835, 6836, 6837, 6838, 6839, 6840, 6841, 6842, 6843, 6844, 6845, 6846, 6847, 6848, 6849, 6850, 6851, 6852, 6853, 6854, 6855";
                listaEquipo = str1.Split(',').Select(int.Parse).ToList();
            }

            // ATN 1 S.A. HACIA MINERA CERRO VERDE 09/03/16
            if (objTransf.EmprcodiOrigen == 11540 && objTransf.Emprcodi == 67)
            {
                string str1 = "4496, 4530, 4498, 4499, 4500, 4501, 4502, 4503, 4504, 4505, 4506, 4507, 4508, 4509, 4510, 4511, 4512, 4513, 4514, 4515, 4516, 4517, 4518, 4519, 4520, 4521, 4522, 4523, 4524, 4525, 4526, 4527, 4528, 4529, 4497";
                listaEquipo = str1.Split(',').Select(int.Parse).ToList();
            }

            // CHINANGO S.A.C. HACIA CONELSUR LT SAC 19/12/16
            if (objTransf.EmprcodiOrigen == 10901 && objTransf.Emprcodi == 12106)
            {
                string str1 = "6662, 6667, 6663, 6665, 6664, 6666, 6661, 6659, 6668, 6658, 6660 ";
                listaEquipo = str1.Split(',').Select(int.Parse).ToList();
            }

            // CHINANGO S.A.C. HACIA ELECTRO CENTRO 23/12/16
            if (objTransf.EmprcodiOrigen == 10901 && objTransf.Emprcodi == 28)
            {
                string str1 = "6751,6753,6752";
                listaEquipo = str1.Split(',').Select(int.Parse).ToList();
            }
            // EMPRESA DE GENERACION HUANZA HACIA ANDEAN POWER S.A.C. 18/01/17
            if (objTransf.EmprcodiOrigen == 206 && objTransf.Emprcodi == 12056)
            {
                string str1 = "6754, 6755 ";
                listaEquipo = str1.Split(',').Select(int.Parse).ToList();
            }
            // ENEL GENERACION PERU S.A.A. HACIA CONELSUR LT SAC 20/12/16
            if (objTransf.EmprcodiOrigen == 12096 && objTransf.Emprcodi == 12106)
            {
                string str1 = "6691, 6750, 6730, 6744, 6672, 6749, 6748, 6678, 6711, 6712, 6713, 6714, 6715, 6717, 6718, 6719, 6720, 6721, 6723, 6724, 6716, 6734, 6683, 6686, 6671, 6669, 6694, 6693, 6695, 6701, 6698, 6728, 6736, 6673, 6740, 6743, 6732, 6676, 6677, 6679, 6708, 6709, 6688, 6706, 6705, 6704, 6722, 6710, 6731, 6739, 6742, 6725, 6735, 6737, 6738, 6741, 6690, 6684, 6697, 6726, 6703, 6696, 6692, 6680, 6682, 6674, 6681, 6689, 6699, 6702, 6727, 6729, 6747, 6675, 6745, 6746, 6707, 6733";
                listaEquipo = str1.Split(',').Select(int.Parse).ToList();
            }
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //PTOMEDICODI: 45900  LASTCODI:22015 ==> PROBLEMA DE DATA CON EMPRESA , REPORTE MUESTRA EMPRESA: EL BROCAL .S.A.
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        }

        #endregion

    }
}