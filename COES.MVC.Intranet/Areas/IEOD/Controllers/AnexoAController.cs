using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Hidrologia.Helper;
using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using COES.Servicios.Aplicacion.ReportesMedicion;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IEOD.Controllers
{
    public class AnexoAController : BaseController
    {
        IEODAppServicio servIEOD = new IEODAppServicio();
        PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();
        HidrologiaAppServicio servHidro = new HidrologiaAppServicio();
        HorasOperacionAppServicio servHO = new HorasOperacionAppServicio();

        #region Declaracion de variables de Sesión

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        #endregion

        #region Útil

        /// <summary>
        /// Determinar si la sesion es válida, si se selecciono fecha para reporte de Anexo A
        /// </summary>
        /// <returns></returns>
        public bool EsOpcionValida()
        {
            return base.IsValidSesionView();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RedireccionarOpcionValida()
        {
            if (!base.IsValidSesionView())
            {
                return base.RedirectToLogin();
            }
            else
            {
                return RedirectToAction("MenuAnexoA", "IEOD/AnexoA", new { area = string.Empty });
            }
        }

        /// <summary>
        /// Obtener numero formateado de 3 digitos
        /// </summary>
        /// <returns></returns>
        public static NumberFormatInfo GenerarNumberFormatInfo()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        /// <summary>
        /// Setear valores para cada index
        /// </summary>
        /// <param name="model"></param>
        /// <param name="repcodi"></param>
        private BusquedaIEODModel GetModelGenericoIndex(int repcodi, string fechaConsulta)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            //Fechas
            model.FechaInicio = fechaConsulta.Replace("-", "/");
            model.FechaFin = fechaConsulta.Replace("-", "/");
            DateTime fechaInicial = DateTime.ParseExact(model.FechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            model.FechaPeriodo = fechaInicial;

            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaInicial);

            //numeral seleccionado
            model.Tiporeporte = ConstantesPR5ReportesServicio.TIPO_ANEXO_A;
            model.Idnumeral = repcodi;
            model.Reporcodi = repcodi;

            SiMenureporteDTO objItem = servicio.GetByIdMenuReporte(repcodi);
            model.TituloWeb = objItem.Mreptituloweb
                + "<span class='filtro_version_desc'></span>"
                + "<br/>"
                + "<span class='filtro_fecha_desc'>" + model.FiltroFechaDesc + "</span>";
            model.TieneFiltroDobleFecha = UtilAnexoAPR5.TieneFiltroDobleFechaAnexoA(repcodi);

            model.Url = Url.Content("~/");

            return model;
        }

        /// <summary>
        /// Descripción de los filtros fecha
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        private string GetDescripcionFiltroFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            return UtilAnexoAPR5.GetDescripcionFiltroFechaAnexoA(fechaInicial, fechaFinal);
        }

        #endregion

        #region Filtros Web

        /// <summary>
        /// Lista los tipos de informacion de acuerdo a la unidad seleccionada
        /// </summary>
        /// </summary>
        /// <param name="sUnidad"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarPuntosMedicion()
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            int[] tipoInfocodis = { 11, 14, 40 };
            var lista = this.servHidro.ListMeTipopuntomedicions(ConstantesHidrologia.IdOrigenHidro.ToString());
            model.ListaTipoPtoMedicion = lista.Where(x => tipoInfocodis.Contains(x.Tipoinfocodi)).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Carga lista de modos de operacion y grupos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoCentral"></param>
        /// <returns></returns>
        public PartialViewResult CargarModosOpeGrupos(string idEmpresa, string idTipoCentral, string idTCombustible)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            model.ListModosOpeGrupos = servicio.ListarModoOpe(idTipoCentral, idEmpresa, idTCombustible);

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de modos de operacion por empresa y tipo de central
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoCentral"></param>
        /// <returns></returns>
        public PartialViewResult CargarModos(string idEmpresa, string idTipoCentral)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            List<PrGrupoDTO> listaModo = new List<PrGrupoDTO>();

            int[] tipoCentral = new int[idTipoCentral.Length];
            tipoCentral = idTipoCentral.Split(',').Select(x => int.Parse(x)).ToArray();

            if (tipoCentral.Length == 1)
            {
                if (tipoCentral[0] == 5)//termo
                {
                    listaModo = servicio.ListarModoOperacionXFamiliaAndEmpresa("5", idEmpresa);
                }
            }

            model.ListaModo = listaModo;

            return PartialView(model);

        }

        /// <summary>
        /// Carga la lista de causa de evento por tipo de central
        /// </summary>
        /// <param name="idTipoCentral"></param>
        /// <returns></returns>
        public PartialViewResult CargarCausa(string idTipoCentral)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();

            model.ListaCausa = new List<EveCausaeventoDTO>();

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de unidades por empresa y tipo de central
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoCentral"></param>
        /// <returns></returns>
        public PartialViewResult CargarUnidad(string idEmpresa, string idTipoCentral)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            List<EqEquipoDTO> lista = new List<EqEquipoDTO>();

            int[] tipoCentral = new int[idTipoCentral.Length];
            tipoCentral = idTipoCentral.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] empresa = new int[idEmpresa.Length];
            empresa = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            if (tipoCentral.Length == 1)
            {
                if (tipoCentral[0] == 5)//termo
                {
                    lista = servicio.ListarEquipoxFamiliasxEmpresas(new int[] { 3 }, empresa);
                }
            }

            model.ListaUnidades = lista;

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de opciones de sistema aislado
        /// </summary>
        /// <returns></returns>
        public PartialViewResult CargarSistemaAislado()
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            List<EstadoModel> ListaEstadoSistemaA = new List<EstadoModel>();
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "0", EstadoDescripcion = "NO" });
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "1", EstadoDescripcion = "SÍ" });
            model.listaEstadoSistemaA = ListaEstadoSistemaA;

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de tipo de central por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarTipoCentral(string idEmpresa)
        {

            BusquedaIEODModel model = new BusquedaIEODModel();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();

            int[] result = new int[idEmpresa.Length];

            result = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            if (!string.IsNullOrEmpty(idEmpresa))
            {
                //lista = servicio.ListaComboxReportePotenciaGeneradaxTipoRecurso().Where(x => result.Contains(x.Emprcodi)).ToList();
            }
            else
            {
                //lista = servicio.ListaComboxReportePotenciaGeneradaxTipoRecurso();
            }

            int[] centrals = { 4, 5, 37, 39 };
            lista = lista.Where(x => centrals.Contains(x.Famcodi)).ToList();
            model.ListaTipoCentrales = lista.GroupBy(x => x.Famnomb).Select(y => y.First()).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de centrales por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarEmpresaxTipoCentral(string idTipoCentral)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            if (idTipoCentral != "-1")
            {
                entitys = this.servIEOD.ListarEmpresasxTipoEquipos(idTipoCentral);
            }
            else
            {
                entitys = this.servIEOD.ListarEmpresasxTipoEquipos(ConstantesPR5ReportesServicio.FamcodiTipoCentrales);
            }

            model.ListaEmpresas = entitys.OrderBy(x => x.Emprnomb).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de centrales por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarCentralxEmpresa(string idEmpresa)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            if (idEmpresa != "-1")
            {
                entitys = servicio.ListarCentralesXEmpresaXFamiliaGEN2(idEmpresa, ConstantesPR5ReportesServicio.FamcodiTipoCentrales).ToList();
            }
            else
            {
                entitys = servicio.ListarCentralesXEmpresaXFamiliaGEN2("-1", ConstantesPR5ReportesServicio.FamcodiTipoCentrales).ToList();
            }

            model.ListaCentrales = entitys.OrderBy(x => x.Equinomb).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Carga Lista de tipo generacion por central
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public PartialViewResult CargarTipoGeneracionxCentral(string equicodi)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            List<SiTipogeneracionDTO> entitys = new List<SiTipogeneracionDTO>();
            entitys = servicio.TipoGeneracionxCentral(equicodi).ToList();
            model.ListTipogeneracion = entitys.Where(x => x.Tgenercodi != -1).OrderBy(x => x.Tgenernomb).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de tipo de combustible por grupo o modo de operación
        /// </summary>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public PartialViewResult CargarTipoCombustibleXModo(string idEmpresa, string idModo)
        {
            idModo = string.IsNullOrEmpty(idModo) ? ConstantesAppServicio.ParametroDefecto : idModo;

            BusquedaIEODModel model = new BusquedaIEODModel();

            List<SiFuenteenergiaDTO> entitys = servicio.ListTipoCombustibleXModo(idEmpresa, idModo).ToList();

            model.ListaTipoCombustibles = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de tipo de combustible por central
        /// </summary>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public PartialViewResult CargarTipoCombustibleXCentral(string idCentral)
        {
            idCentral = string.IsNullOrEmpty(idCentral) ? ConstantesAppServicio.ParametroDefecto : idCentral;

            BusquedaIEODModel model = new BusquedaIEODModel();

            List<SiFuenteenergiaDTO> entitys = servicio.ListTipoCombustibleXEquipo(idCentral);

            model.ListaTipoCombustibles = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de tipo de combustible por central
        /// </summary>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public PartialViewResult CargarTipoCombustibleXTipoCentral(string idTipoCentral, string idEmpresa)
        {
            idTipoCentral = string.IsNullOrEmpty(idTipoCentral) ? ConstantesAppServicio.ParametroDefecto : idTipoCentral;

            BusquedaIEODModel model = new BusquedaIEODModel();

            List<SiFuenteenergiaDTO> entitys = servicio.ListTipoCombustibleXTipoCentral(idTipoCentral, idEmpresa);

            model.ListaTipoCombustibles = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de ubicaciones por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarUbicacion(string idEmpresa)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            if (string.IsNullOrEmpty(idEmpresa)) idEmpresa = ConstantesAppServicio.ParametroDefecto;

            List<EqAreaDTO> entitys = servicio.ListarAreaXEmpresas(ConstantesAppServicio.ParametroDefecto).ToList();
            if (idEmpresa != "-1")
            {
                int[] empresas = idEmpresa.Split(',').Select(int.Parse).ToArray();
                entitys = entitys.Where(x => empresas.Contains(x.Emprcodi)).ToList();
            }

            model.ListaUbicacion = entitys.GroupBy(x => x.Areacodi).Select(x => x.First()).OrderBy(x => x.Areanomb).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de equipos por empresa y ubicacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idUbicacion"></param>
        /// <returns></returns>
        public PartialViewResult CargarEquipos(string idEmpresa, string idUbicacion)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            List<EqEquipoDTO> entitys = this.servicio.ListarEquipos(idEmpresa, idUbicacion).ToList();
            model.ListaEquipo = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de equipos por empresa y central
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idUbicacion"></param>
        /// <returns></returns>
        public PartialViewResult CargarEquiposXCentral(string idEmpresa, string centrales)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            if (string.IsNullOrEmpty(centrales)) centrales = ConstantesAppServicio.ParametroDefecto;

            int[] equipadres = new int[centrales.Length];
            equipadres = centrales.Split(',').Select(x => int.Parse(x)).ToArray();

            List<EqEquipoDTO> entitys = this.servIEOD.ListarCentralesXEmpresaGener(idEmpresa, ConstantesHorasOperacion.CodFamiliasGeneradores);
            entitys = entitys.Where(x => centrales == ConstantesAppServicio.ParametroDefecto || equipadres.Contains(x.Equipadre.GetValueOrDefault(-2))).ToList();

            model.ListaEquipo = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de GPS por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarGPS()
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            List<MeGpsDTO> lista = this.servicio.ListarGpsxFiltro(ConstantesAppServicio.ParametroDefecto, true);
            model.ListaGps = lista;

            return PartialView(model);
        }

        #endregion

        #region Versiones Excel y Word

        [HttpPost]
        public JsonResult ListadoVersion(string fechaPeriodo)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime dFechaPeriodo = DateTime.ParseExact(fechaPeriodo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaVersion = servicio.ListaVersionByFechaAnexoAExcel(dFechaPeriodo);
                model.ListaVersion2 = servicio.ListaVersionByFechaAnexoAWord(dFechaPeriodo);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Guardar versión
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="motivo"></param>
        /// <returns></returns>
        public async Task<JsonResult> GuardarNuevaVersion(string fechaPeriodo, string motivo, int tmrepcodi, int gpscodi = 1, bool incluirLeyendaEcuador = false)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            int verscodi = 0;
            try
            {
                this.ValidarSesionJsonResult();

                //Validación
                DateTime dFechaPeriodo = DateTime.ParseExact(fechaPeriodo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                motivo = (motivo ?? "").Trim();
                if (motivo == null || motivo.Trim() == string.Empty)
                {
                    throw new Exception("Debe ingresar motivo.");
                }

                //Guardar registro
                SiVersionDTO objVersion = new SiVersionDTO()
                {
                    Versfechaperiodo = dFechaPeriodo,
                    Mprojcodi = ConstantesPR5ReportesServicio.MprojcodiIEOD,
                    Tmrepcodi = tmrepcodi,
                    Versmotivo = motivo,
                    Versfechaversion = DateTime.Now,
                    Versusucreacion = base.UserName,
                    Versfeccreacion = DateTime.Now
                };
                verscodi = servicio.SaveSiVersion(objVersion);
                
                if (ConstantesPR5ReportesServicio.ReptipcodiAnexoAExcel == tmrepcodi)
                {
                    objVersion = servicio.GetByIdSiVersion(verscodi);

                    //Guardar totalizado por grupo despacho y fuente de energía. Esto se utilizará luego en el Log del informe / ejecutivo semanal
                    servicio.CopiarProdGenTotalizadaAnexoA(objVersion.Versfechaperiodo);

                    //Procesar
                    servicio.GenerarIndicadores(dFechaPeriodo);

                    //Generar archivo excel en directorio local
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.Directorio;
                    string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                    servicio.GenerarArchivoExcelAnexoACompleto("", objVersion.Versfechaperiodo, ruta, objVersion.Verscorrelativo, out string nameFile, pathLogo);

                    //Mover a FileServer
                    servicio.MoverArchivoExcelAnexoAToFileServer(objVersion.Versfechaperiodo, ruta, nameFile);
                }

                if (ConstantesPR5ReportesServicio.ReptipcodiAnexoAWord == tmrepcodi)
                {
                    objVersion = servicio.GetByIdSiVersion(verscodi);
                    
                    //Guardar totalizado por grupo despacho y fuente de energía para calcular la maxima demanda ejecutada y programada
                    servicio.CopiarProdGenTotalizadaSemanal(objVersion.Versfechaperiodo.AddDays(-7), objVersion.Versfechaperiodo);
                    
                    //Procesar
                    servicio.GenerarIndicadores(dFechaPeriodo);

                    //Generar archivo word en directorio local
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.Directorio;
                    string nameFile = PR5ReportesAppServicio.GetNombreArchivoAnexoAWord(objVersion.Versfechaperiodo, objVersion.Verscorrelativo);

                    int resultadoWord = await servicio.GenerarArchivoWordAnexoACompleto(objVersion.Versfechaperiodo, gpscodi, incluirLeyendaEcuador, ruta, nameFile);

                    //Mover a FileServer
                    servicio.MoverArchivoWordAnexoAToFileServer(objVersion.Versfechaperiodo, ruta, nameFile);
                }

                model.Resultado = verscodi.ToString();
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

        public virtual ActionResult DescargarArchivoExcelXVersion(int verscodi)
        {
            base.ValidarSesionUsuario();

            string directorioDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.Directorio;

            servicio.CopiarArchivoExcelAnexoAFsALocal(verscodi, directorioDestino, out string fileName);

            //eliminar archivo temporal
            string fullPath = directorioDestino + fileName;
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public virtual ActionResult DescargarArchivoWordXVersion(int verscodi)
        {
            base.ValidarSesionUsuario();

            string directorioDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.Directorio;

            servicio.CopiarArchivoWordAnexoAFsALocal(verscodi, directorioDestino, out string fileName);

            //eliminar archivo temporal
            string fullPath = directorioDestino + fileName;
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public JsonResult VistaPreviaArchivoExcel(int verscodi)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            try
            {
                base.ValidarSesionUsuario();

                //Copiar archivo a reportes
                string subcarpetaDestino = ConstantesPR5ReportesServicio.Directorio;
                string directorioDestino = AppDomain.CurrentDomain.BaseDirectory + subcarpetaDestino;

                servicio.GenerarArchivoExcelHojaVisible(verscodi, directorioDestino, out string fileName);

                string url = subcarpetaDestino + fileName;

                model.Resultado = url;
                model.Detalle = fileName;
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
        public JsonResult VistaPreviaArchivoWord(int verscodi)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            try
            {
                base.ValidarSesionUsuario();

                //Copiar archivo a reportes
                string subcarpetaDestino = ConstantesPR5ReportesServicio.Directorio;
                string directorioDestino = AppDomain.CurrentDomain.BaseDirectory + subcarpetaDestino;

                servicio.CopiarArchivoWordAnexoAFsALocal(verscodi, directorioDestino, out string fileName);

                string url = subcarpetaDestino + fileName;

                model.Resultado = url;
                model.Detalle = fileName;
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

        #endregion

        #region Menu Anexo A

        //
        // GET: /IEOD/AnexoA/MenuAnexoA/
        public ActionResult MenuAnexoA(string fecha)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            BusquedaIEODModel model = new BusquedaIEODModel();

            DateTime finicio = DateTime.Today.AddDays(-1);
            if (!string.IsNullOrEmpty(fecha)) finicio = DateTime.ParseExact(fecha.Replace("-", "/"), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            model.FechaInicio = finicio.ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = finicio.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Cargar menu de opciones
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CargarMenu()
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            List<SiMenureporteDTO> listaItems = servicio.GetListaAdmReporte(ConstantesPR5ReportesServicio.ReptipcodiAnexoAExcel);
            model.Menu = UtilAnexoAPR5.ListaMenuHtml(listaItems);

            return Json(model);
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesAppServicio.ModuloManualUsuario;
            string nombreArchivo = ConstantesAppServicio.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesAppServicio.FolderRaizInformesSGIModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        public JsonResult ListarHojaExcel()
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            try
            {
                base.ValidarSesionJsonResult();

                List<SiMenureporteHojaDTO> listaHoja = servicio.GetByCriteriaSiMenureporteHojas(ConstantesPR5ReportesServicio.ReptipcodiAnexoAExcel);
                model.ListaHojaExcel = listaHoja;
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

        public JsonResult GuardarVisibleHojaExcel(string mrephcodisVisible)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            try
            {
                base.ValidarSesionJsonResult();

                List<int> listaMrephcodisVisible = new List<int>();
                if (!string.IsNullOrEmpty(mrephcodisVisible))
                    listaMrephcodisVisible = mrephcodisVisible.Split(',').Select(x => int.Parse(x)).ToList();
                servicio.GuardarListaHoja(ConstantesPR5ReportesServicio.ReptipcodiAnexoAExcel, listaMrephcodisVisible);

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
        public PartialViewResult ConfigurarGPS()
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            model.ListaGPS = this.servicio.ListarGpsxFiltro(ConstantesAppServicio.ParametroDefecto);

            //Solo los activos
            model.ListaGPS = model.ListaGPS.Where(x => x.Gpsestado == "A").ToList();

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ListarGpsFrecuencia()
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeGpsDTO> ListaGPS = new List<MeGpsDTO>();
            //model.ListaGPS = this.servicio.ListarGpsxFiltro(ConstantesAppServicio.ParametroDefecto);
            var Lista = this.servicio.ListarGpsxFiltro(ConstantesAppServicio.ParametroDefecto);

            foreach (var item in Lista)
            {
                if (item.Gpsindieod == "S")
                {
                    MeGpsDTO entity = new MeGpsDTO();
                    entity = item;
                    ListaGPS.Add(entity);
                }
            }
            model.ListaGPS = ListaGPS;
            return Json(model);
        }


        [HttpPost]
        public JsonResult GrabarConfiguracionGPS(string id)
        {
            return Json(this.servicio.ActualizarListadoGPS(id));
        }

        #endregion

        #region Exportación Excel

        /// <summary>
        /// Descargar archivo del Anexo A
        /// </summary>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public virtual ActionResult ExportarReporteXls(string nameFile)
        {
            string subcarpetaDestino = ConstantesPR5ReportesServicio.Directorio;
            string directorioDestino = AppDomain.CurrentDomain.BaseDirectory + subcarpetaDestino;
            string fullPath = directorioDestino + nameFile;

            //eliminar archivo temporal
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nameFile);
        }

        /// <summary>
        /// Exportacion del Anexo A a archivo Excel
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult GenerarReporteAnexoAByNumero(int numReporte, string fecha)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime dtfecha = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.Directorio;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = string.Empty;

                switch (numReporte)
                {
                    case 2:
                        this.servicio.GenerarArchivoExcelAnexoA2Hidrologia("", dtfecha, ruta, out nameFile, pathLogo);
                        break;
                    case 3:
                        this.servicio.GenerarArchivoExcelAnexoA3RPFyRSF("", dtfecha, ruta, out nameFile, pathLogo);
                        break;
                    case 4:
                        this.servicio.GenerarArchivoExcelAnexoA4Hop("", dtfecha, ruta, out nameFile, pathLogo);
                        break;
                    case 5:
                        this.servicio.GenerarArchivoExcelAnexoA5Manttoeje("", dtfecha, ruta, out nameFile, pathLogo);
                        break;
                    case 6:
                        this.servicio.GenerarArchivoExcelAnexoA6CMgCP("", dtfecha, ruta, out nameFile, pathLogo);
                        break;
                }

                model.Resultado = nameFile;
                model.Total = 1;
            }
            catch (Exception ex)
            {
                model.Total = -1;
                model.Resultado2 = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Exportacion de un item en especifico del Anexo A a archivo Excel
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult GenerarReporteAnexoAByItem(int reporcodi, string fec1, string fec2, string param1, string param2, string param3, string param4, string idEmpresa, string idCentral)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.Directorio;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                DateTime dtfecha1 = DateTime.ParseExact(fec1, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime dtfecha2 = fec2 != null ? DateTime.ParseExact(fec2, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : dtfecha1;

                SiMenureporteDTO objItem = servicio.GetByIdMenuReporte(reporcodi);

                string nameFile = ConstantesPR5ReportesServicio.RptExcel + "_" + objItem.Repabrev.Replace("Index", "") + ConstantesPR5ReportesServicio.ExtensionExcel;
                this.servicio.GenerarArchivoExcelAnexoAByItem("", dtfecha1, dtfecha2, ruta + nameFile, reporcodi, param1, param2, param3, param4, idEmpresa, idCentral, pathLogo);

                model.Resultado = nameFile;
                model.Total = 1;
            }
            catch (Exception ex)
            {
                model.Total = -1;
                model.Resultado2 = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.1.	Reporte de Eventos: fallas, interrupciones, restricciones y otros de carácter operativo.
        /// </summary>
        /// <returns></returns>
        #region ReporteEventos
        //
        // GET: /IEOD/AnexoA/IndexReporteEventos
        public ActionResult IndexReporteEventos(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteEventos, fecha);

            var empresas = servicio.ListarEmpresaTodo().Where(x => x.Emprsein == "S").ToList();
            model.ListaEmpresas = empresas;

            return View(model);
        }

        /// <summary>
        /// Listar reporte de eventos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idUbicacion"></param>
        /// <param name="idEquipo"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        public JsonResult CargarListaEventos(string idEmpresa, string idUbicacion, string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            servicio.ReporteEventosDataVersionada(idEmpresa, idUbicacion, fechaInicial, fechaFinal, "", out List<EventoDTO> lista, out List<EventoDTO> listaVersion, out List<EveInterrupcionDTO> listaInterrup, out List<EveInterrupcionDTO> listaInterrupVersion, out List<EqEquipoDTO> listaEq, out List<EqEquipoDTO> listaEqVersion);

            model.Resultado = UtilAnexoAPR5.ReporteEventosHtml(fechaInicial, fechaInicial, lista, listaVersion, listaInterrup, listaInterrupVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.2.	Reporte de las principales restricciones operativas y mantenimiento de las Unidades de Generación y de los equipos del Sistema de Transmisión.
        /// </summary>
        /// <returns></returns>
        #region ReporteRestriccionesOperativas
        //
        // GET: /IEOD/AnexoA/IndexReporteRestriccionesOperativas
        public ActionResult IndexReporteRestriccionesOperativas(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteRestriccionesOperativas, fecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresaRestriccionesOperativas);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            return View(model);
        }

        /// <summary>
        /// Listar reporte de restricciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idUbicacion"></param>
        /// <param name="idEquipo"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nPagina"></param>
        /// <returns></returns>
        public JsonResult CargarListaRestriccionesYMantto(string idEmpresa, string idUbicacion, string fechaInicio, string fechaFin)
        {
            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model = new PublicacionIEODModel();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            this.servicio.ReporteRestriccionesOperativasDataVersionada(idEmpresa, idUbicacion, fechaInicial, fechaFinal, "", out List<EveIeodcuadroDTO> listaRestricOp, out List<EveIeodcuadroDTO> listaRestricOpVersion, out List<EveManttoDTO> listaMantto, out List<EveManttoDTO> listaManttoVersion);

            model.Resultado = UtilAnexoAPR5.ReporteRestriccionesOperativasHtml(fechaInicial, fechaInicial, listaRestricOp, listaRestricOpVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);
            listaModel.Add(model);

            model = new PublicacionIEODModel();
            model.Resultado = UtilAnexoAPR5.ReporteMantenimentosHtml(fechaInicial, fechaInicial, listaMantto, listaManttoVersion);
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        /// <summary>
        /// 3.13.2.3.	Reporte de ingreso a operación comercial de unidades o centrales de generación, así como de la conexión e integración al SEIN de instalaciones de transmisión.
        /// </summary>
        /// <returns></returns>
        #region ReporteIngresoOperacionConexionIntegracionSEIN
        //
        // GET: /IEOD/AnexoA/IndexReporteIngresoOperacionCISEIN
        public ActionResult IndexReporteIngresoOperacionCISEIN(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteIngresoOperacionCISEIN, fecha);

            model.ListaEmpresas = this.servicio.ListarEmpresaTodo();
            model.ListaTipoEquipo = this.servicio.ListarTiposEquipo().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();

            return View(model);
        }

        /// <summary>
        /// Listar reporte de ingresos de operaciones
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="sTipoEquipo"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaIngresoOperacion(string empresas, string sTipoEquipo, string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveEventoEquipoDTO> lista = new List<EveEventoEquipoDTO>();
            List<EveEventoEquipoDTO> listaVersion = new List<EveEventoEquipoDTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            servicio.ReporteIngresoConexionIntegracionDataVersionada(empresas, sTipoEquipo, fechaInicial, fechaFinal, "", out lista, out listaVersion);

            model.Resultado = UtilAnexoAPR5.ReporteIngresoConexionIntegracionHtml(lista, fechaInicial, fechaFinal);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }
        #endregion

        /// <summary>
        /// 3.13.2.4.	Despacho registrado cada 30 minutos de las Unidades de Generación de los Integrantes del COES, asimismo, se incluye las Unidades de Generación con potencia superior a 5 MW conectadas al SEIN de empresas no Integrantes del COES (MW, MVAr).
        /// </summary>
        /// <returns></returns>
        #region DespachoRegistrado
        //
        // GET: /IEOD/AnexoA/IndexDespachoRegistrado
        public ActionResult IndexDespachoRegistrado(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexDespachoRegistrado, fecha);

            var empresas = this.servIEOD.ListarEmpresasxTipoEquipos2(ConstantesHorasOperacion.CodFamilias, "3,4");
            model.ListaEmpresas = empresas;
            model.ListTipogeneracion = servicio.ListarSiTipogeneracion();

            return View(model);
        }

        /// <summary>
        /// Listar reporte de Despacho
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idUbicacion"></param>
        /// <param name="idEquipo"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        public JsonResult CargarListaDespachoRegistrado(string idEmpresa, string idCentral, int idPotencia, int tipoDato48, 
                                                string fechaIni, string fechaFin, string idtipoGeneracion, int soloRecursosRER)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista, listaVersion;
            List<MePtomedicionDTO> listaPto, listaPtoVersion;

            DateTime fechaInicial = DateTime.ParseExact(fechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            //Validación
            TimeSpan ts = fechaFinal.Subtract(fechaInicial);
            if (ts.TotalDays > 7)
            {
                model.Resultado = "-1";
                model.Total = 0;
                model.Resultado2 = "El reporte web no debe exceder los 7 días, para la exportación en excel sí está permitido.";
                model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);
                return Json(model);
            }

            servicio.ReporteDespachoRegistradoDataVersionada(idEmpresa, idCentral, idPotencia, tipoDato48, fechaInicial, fechaFinal, idtipoGeneracion, soloRecursosRER, "",
                                        out lista, out listaVersion, out listaPto, out listaPtoVersion, out List<string> listaMensaje);

            model.Resultado = UtilAnexoAPR5.ReporteDespachoRegistradoHtml(lista, listaPto, idPotencia, fechaInicial, fechaFinal, listaVersion, tipoDato48, listaMensaje);
            model.Total = lista.Count;
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        /// <summary>
        /// 3.13.2.5.	Reporte de la demanda por áreas (MW).
        /// </summary>
        /// <returns></returns>
        #region ReporteDemandaPorArea
        //
        // GET: /IEOD/AnexoA/IndexReporteDemandaPorArea
        public ActionResult IndexReporteDemandaPorArea(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteDemandaPorArea, fecha);

            model.ListaAreaOperativa = (new FormatoReporteAppServicio()).GetListaAreaAndSubareaOperativa();

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            return View(model);
        }

        /// <summary>
        /// Listar reporte de Demanda x Area
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idArea"></param>
        /// <param name="idEquipo"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaDemandaPorArea(int tipoDato48, string idArea, string fechaInicio, string fechaFin)
        {
            List<PublicacionIEODModel> listamodel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista, listaVersion;

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            servicio.ReporteDemandaPorAreaYSubareaDataVersionada(tipoDato48, idArea, fechaInicial, fechaFinal, "", out lista, out listaVersion
                                                , out List<MeReporptomedDTO> areas, out List<MeReporptomedDTO> subareas, out MeReporteDTO objRpt, out List<MeReporteGraficoDTO> listaConfGraf);

            string url = Url.Content("~/");
            model.Resultado = UtilAnexoAPR5.ReporteDemandaPorAreaYSubareaHtml(url, lista, fechaInicial, listaVersion, idArea, areas, subareas);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);
            listamodel.Add(model);

            model = new PublicacionIEODModel();
            model.Grafico = UtilAnexoAPR5.GetGraficoDemandaPorArea(false, lista, objRpt, listaConfGraf);
            listamodel.Add(model);

            return Json(listamodel);
        }

        #endregion

        /// <summary>
        /// 3.13.2.6.	Reporte de Demanda de Grandes Usuarios (MW).
        /// </summary>
        /// <returns></returns>
        #region ReporteDemandaGrandesUsuarios
        //
        // GET: /IEOD/AnexoA/IndexReporteDemandaGrandesUsuarios
        public ActionResult IndexReporteDemandaGrandesUsuarios(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteDemandaGrandesUsuarios, fecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            return View(model);
        }

        /// <summary>
        /// Lista DemandaGrandesUsuarios HTML
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idUbicacion"></param>
        /// <param name="idEquipo"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaDemandaGrandesUsuarios(string fechaInicio)
        {
            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista, listaVersion;

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = fechaInicial;

            servicio.ReporteDemandaGrandesUsuariosDataVersionada(fechaInicial, fechaFinal, "", out lista, out listaVersion, out List<SiEmpresaDTO> listaEmpresaArea);

            //Reporte
            string resultado = UtilAnexoAPR5.ReporteDemandaGrandesUsuariosHtml(lista, listaVersion, listaEmpresaArea);
            model.Resultado = resultado;
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);
            listaModel.Add(model);

            //Grafico 1 - Area norte
            List<GraficoWeb> listaGrafico = UtilAnexoAPR5.ListarGraficoDemandaGrandesUsuarios(lista, listaEmpresaArea);
            model = new PublicacionIEODModel() { Grafico = listaGrafico[0] };
            listaModel.Add(model);

            //Grafico 2 - Area centro                                               
            model = new PublicacionIEODModel() { Grafico = listaGrafico[1] };
            listaModel.Add(model);

            //Grafico 3 - Area centro                                               
            model = new PublicacionIEODModel() { Grafico = listaGrafico[2] };
            listaModel.Add(model);

            //Grafico 4 - Area centro                                               
            model = new PublicacionIEODModel() { Grafico = listaGrafico[3] };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        /// <summary>
        /// 3.13.2.7.	Recursos energéticos y diagrama de duración de demanda del SEIN.
        /// </summary>
        /// <returns></returns>
        #region ReporteRecursosEnergeticosDemandaSEIN
        //
        // GET: /IEOD/AnexoA/IndexReporteRecursosEnergeticosDemandaSEIN
        public ActionResult IndexReporteRecursosEnergeticosDemandaSEIN(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteRecursosEnergeticosDemandaSEIN, fecha);

            var empresas = this.servIEOD.ListarEmpresasxTipoEquipos(ConstantesHorasOperacion.CodFamilias);
            model.ListaEmpresas = empresas;

            return View(model);
        }

        /// <summary>
        /// Cargar Lista Recursos Energeticos SEIN
        /// </summary>
        /// <param name="idempresa"></param>
        /// <param name="idtipocentral"></param>
        /// <param name="idtiporecurso"></param>
        /// <param name="fcdesde"></param>
        /// <param name="fchasta"></param>
        /// <param name="soloRecursos"></param>
        /// <returns></returns>
        public JsonResult CargarListaRecursosEnergeticosSEIN(int tipoDato48, string idempresa, string idCentral, string idtiporecurso, string fcdesde, string fchasta, int soloRecursos)
        {
            DateTime fechaInicial = DateTime.ParseExact(fcdesde, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fchasta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model;

            List<MeMedicion48DTO> lista, lista2;
            List<MeMedicion48DTO> listaVersion1, listaVersion2;
            string resultado = string.Empty;
            string resultado2 = string.Empty;
            List<string> listaMensaje = new List<string>();

            servicio.ReportePotenciaXTipoRecursoDataVersionada(tipoDato48, idempresa, idCentral, idtiporecurso, soloRecursos, fechaInicial, fechaFinal, ""
                                    , out lista, out lista2, out listaVersion1, out listaVersion2, out List<SiTipogeneracionDTO> listaTipoGen, out List<PrGrupoDTO> listaGrupoData, 
                                    out listaMensaje);

            if (soloRecursos == 1 || soloRecursos == 3) //Solo Recursos Energéticos RER
            {
                //Reporte
                model = new PublicacionIEODModel();
                model.Resultado = UtilAnexoAPR5.ListaReporteGeneracionElectricaCentralesRERHtml(fechaInicial, lista, listaGrupoData, listaVersion1, listaTipoGen);
                model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);
                model.NRegistros = lista.Count;
                listaModel.Add(model);

                //Diagrama de Carga por tipo de Recurso
                model = this.GraficoGeneracionElectricaRERXCentral(lista);
                listaModel.Add(model);

                //GENERACIÓN ELÉCTRICA RER POR TIPO DE GENERACIÓN EN EL SEIN
                model = this.GraficoGeneracionElectricaRERXTipoGeneracion(lista2);
                listaModel.Add(model);
            }
            else
            {
                //Reporte
                model = new PublicacionIEODModel();
                model.Resultado = UtilAnexoAPR5.ListaReportePotenciaXTipoRecursoHtml(lista, listaVersion1);
                model.Resultado2 = UtilAnexoAPR5.ListaReportePotenciaXTipoHidroHtml(lista2);
                model.ListaMensaje = listaMensaje;
                model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);
                listaModel.Add(model);

                //Diagrama de Carga por tipo de Recurso
                model = new PublicacionIEODModel() { Grafico = UtilAnexoAPR5.GetGraficoDiagramaCarga(lista) };
                listaModel.Add(model);

                //Participacion por tipo de Recurso
                model = this.GraficoRecursosEnergeticosParticipacionRecurso(lista);
                listaModel.Add(model);
            }

            var jsonResult = Json(listaModel);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Grafico de Participacion por Tipo de Recurso
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private PublicacionIEODModel GraficoRecursosEnergeticosParticipacionRecurso(List<MeMedicion48DTO> listaReporte)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            model.Grafico.SerieDataS = new DatosSerie[listaReporte.Count][];

            List<RegistroSerie> listaSerie = new List<RegistroSerie>();

            for (int i = 0; i < listaReporte.Count; i++)
            {
                decimal valor = 0;
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = listaReporte[i].Ctgdetnomb;
                regSerie.Type = "area";
                regSerie.Color = listaReporte[i].Fenercolor;
                List<DatosSerie> listadata = new List<DatosSerie>();
                valor += ((decimal)listaReporte[i].Meditotal / 2);

                regSerie.Acumulado = valor;
                regSerie.Data = listadata;

                listaSerie.Add(regSerie);
            }

            decimal total = listaSerie.Sum(x => x.Acumulado.GetValueOrDefault(0));

            //asignar porcentaje
            foreach (var reg in listaSerie)
            {
                if (reg.Acumulado == 0) continue;
                var porcentaje = reg.Acumulado / total * 100;
                reg.Porcentaje = porcentaje;
            }

            model.Grafico.Series.AddRange(listaSerie);


            model.Grafico.TitleText = "PARTICIPACIÓN TIPO DE RECURSO";
            if (listaReporte.Count > 0)
            {
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
            }
            return model;
        }

        /// <summary>
        /// Grafico de Potencia x Tipo de Recurso
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private PublicacionIEODModel GraficoGeneracionElectricaRERXCentral(List<MeMedicion48DTO> listaReporteInput)
        {
            List<MeMedicion48DTO> listaReporte = listaReporteInput.OrderByDescending(x => x.Orden).ToList();
            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            model.Grafico.SerieDataS = new DatosSerie[listaReporte.Count][];

            var listaTipoGen = this.servicio.ListarTipoGeneracionRER().OrderByDescending(x => x.Orden);
            foreach (var reg in listaTipoGen)
            {
                var listaEmpr = listaReporteInput.Where(x => x.Tgenercodi == reg.Tgenercodi).GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(x => new { x.Key.Emprcodi, x.Key.Emprnomb })
                    .OrderByDescending(x => x.Emprnomb).ToList();
                foreach (var item in listaEmpr)
                {
                    var listaDataXEmpr = listaReporteInput.Where(x => x.Tgenercodi == reg.Tgenercodi && x.Emprcodi == item.Emprcodi).OrderByDescending(x => x.Grupocentral).ToList();

                    foreach (var grupo in listaDataXEmpr)
                    {
                        decimal valor = 0;
                        RegistroSerie regSerie = new RegistroSerie();
                        regSerie.Name = grupo.Grupocentral;
                        regSerie.Type = "bar";
                        regSerie.Color = grupo.Tgenercolor;

                        List<DatosSerie> listadata = new List<DatosSerie>();
                        valor += ((decimal)grupo.Meditotal);

                        regSerie.Acumulado = valor;
                        regSerie.Data = listadata;

                        model.Grafico.Series.Add(regSerie);
                    }
                }
            }

            model.Grafico.TitleText = "GENERACIÓN ELÉCTRICA DE LAS CENTRALES RER (MWh)";
            if (listaReporte.Count > 0)
            {
                model.Grafico.YaxixTitle = "MWh";
                model.Grafico.XAxisTitle = "CENTRALES";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
            }
            return model;
        }

        /// <summary>
        /// Grafico Generacion Electrica RER por TipoGeneracion
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private PublicacionIEODModel GraficoGeneracionElectricaRERXTipoGeneracion(List<MeMedicion48DTO> listaReporte)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            model.Grafico.SerieDataS = new DatosSerie[listaReporte.Count][];

            List<RegistroSerie> listaSerie = new List<RegistroSerie>();

            for (int i = 0; i < listaReporte.Count; i++)
            {
                decimal valor = 0;
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = listaReporte[i].Tgenernomb;
                regSerie.Type = "area";
                regSerie.Color = listaReporte[i].Tgenercolor;
                List<DatosSerie> listadata = new List<DatosSerie>();
                valor += ((decimal)listaReporte[i].Meditotal / 2);

                regSerie.Acumulado = valor;
                regSerie.Data = listadata;

                listaSerie.Add(regSerie);
            }

            decimal total = listaSerie.Sum(x => x.Acumulado.GetValueOrDefault(0));

            //asignar porcentaje
            foreach (var reg in listaSerie)
            {
                var porcentaje = total != 0 ? reg.Acumulado / total * 100 : null;
                reg.Porcentaje = porcentaje;
            }

            model.Grafico.Series.AddRange(listaSerie);

            model.Grafico.TitleText = "GENERACIÓN ELÉCTRICA RER POR TIPO DE GENERACIÓN EN EL SEIN";
            model.Grafico.Subtitle = "TOTAL RER = " + Decimal.Round(total, 3) + " MWh";
            if (listaReporte.Count > 0)
            {
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
            }
            return model;
        }

        #endregion

        /// <summary>
        /// 3.13.2.8.	Evolución de la producción de energía diaria.
        /// </summary>
        /// <returns></returns>               
        #region ReporteProduccionEnergiaDiaria
        //
        // GET: /IEOD/AnexoA/IndexReporteProduccionEnergiaDiaria
        public ActionResult IndexReporteProduccionEnergiaDiaria(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteProduccionEnergiaDiaria, fecha);

            model.FechaInicio = (new DateTime(model.FechaPeriodo.Year, model.FechaPeriodo.Month, 1)).ToString(ConstantesAppServicio.FormatoFecha);

            model.ListaEmpresas = servicio.ObtenerEmpresasGeneradoras();

            return View(model);
        }

        /// <summary>
        /// Lista de Evolución de la producción de energía html
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="idGeneracion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaProduccionEnergiaDiaria(string idEmpresa, string idCentral, string idGeneracion, string fechaInicio, string fechaFin)
        {
            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model = new PublicacionIEODModel();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            servicio.ReporteProduccionEnergiaDiariaDataVersionada(idEmpresa, idCentral, idGeneracion, fechaInicial, fechaFinal, "",
                                        out List<MeMedicion48DTO> listaDC, out List<MeMedicion48DTO> listaDNC,
                                        out List<MePtomedicionDTO> listaPto48Coes, out List<MePtomedicionDTO> listaPto48NoCoes, 
                                        out List<MeMedicion48DTO> listaVC, out List<MeMedicion48DTO> listaVNC);

            model.Resultado = UtilAnexoAPR5.ReporteProduccionEnergiaDiariaHtml(ConstantesPR5ReportesServicio.EmpresacoesSi, listaPto48Coes, listaDC, fechaInicial, fechaFinal, listaVC);
            model.Resultado2 = UtilAnexoAPR5.ReporteProduccionEnergiaDiariaHtml(ConstantesPR5ReportesServicio.EmpresacoesNo, listaPto48NoCoes, listaDNC, fechaInicial, fechaFinal, listaVNC);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            PublicacionIEODModel model2 = GraficoProduccionEnergiaDiaria(listaDC);

            listaModel.Add(model);
            listaModel.Add(model2);

            var jsonResult = Json(listaModel);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Grafico ProduccionEnergia Diaria
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoProduccionEnergiaDiaria(List<MeMedicion48DTO> dataCoes)
        {
            var lista = dataCoes.GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(x => new MeMedicion48DTO
            {
                Meditotal = x.Sum(p => p.Meditotal),
                Emprnomb = x.Key.Emprnomb,
                Emprcodi = x.Key.Emprcodi

            }).OrderByDescending(x => x.Meditotal).ToList();

            decimal? valor;
            NumberFormatInfo nfi = GenerarNumberFormatInfo();

            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            model.Grafico.SerieDataS = new DatosSerie[lista.Count][];

            for (int i = 0; i < lista.Count; i++)
            {
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = lista[i].Emprnomb;
                regSerie.Type = "column";
                regSerie.Color = "#3498DB";
                List<DatosSerie> listadata = new List<DatosSerie>();

                valor = (decimal?)lista[i].Meditotal;
                model.Grafico.SeriesName.Add(lista[i].Emprnomb.ToString());
                listadata.Add(new DatosSerie() { Y = valor });

                regSerie.Data = listadata;
                regSerie.YAxisTitle = "Ejecutado";
                model.Grafico.Series.Add(regSerie);
            }

            model.Grafico.TitleText = "GENERACIÓN DE ENERGÍA EJECUTADA POR EMPRESAS INTEGRANTES COES (MWh)";
            if (lista.Count > 0)
            {
                model.Grafico.YaxixTitle = "(%)";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
            }
            return model;
        }

        #endregion

        /// <summary>
        /// 3.13.2.9.	Máxima generación instantánea del SEIN (MW).
        /// </summary>
        /// <returns></returns>
        #region ReporteMaxGeneraciondelSEIN
        //
        // GET: /IEOD/AnexoA/IndexReporteGeneracionDelSEIN
        public ActionResult IndexReporteGeneracionDelSEIN(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteGeneracionDelSEIN, fecha);

            return View(model);
        }

        /// <summary>
        /// Interfaz web de RESUMEN DE GENERACIÓN POR ÁREAS DEL SEIN
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaGeneracionDelSEIN(int tipoDato48, string fechaInicio, string fechaFin)
        {
            List<PublicacionIEODModel> Listamodel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista, listaVersion;

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            servicio.ReporteMaxGeneracionInstSEINDataVersionada(tipoDato48, fechaInicial, fechaFinal, "", out lista, out listaVersion);

            string url = Url.Content("~/");
            string resultado = UtilAnexoAPR5.ReporteMaxGeneracionInstSEINHtml(url, lista, fechaInicial, fechaFinal, listaVersion);

            model.Resultado = resultado;
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);
            Listamodel.Add(model);

            Listamodel.Add(this.GraficoGeneracionSEIN(lista, 1));
            Listamodel.Add(this.GraficoGeneracionSEIN(lista, 2));
            Listamodel.Add(this.GraficoGeneracionSEIN(lista, 3));

            return Json(Listamodel);
        }

        /// <summary>
        /// Grafico de Generacion de Maxima demanda del SEIN
        /// </summary>
        /// <param name="listaData"></param>
        /// <param name="tipoReporte"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoGeneracionSEIN(List<MeMedicion48DTO> listaData, int tipoReporte)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();
            switch (tipoReporte)
            {
                case ConstantesPR5ReportesServicio.TipoGrafGeneracionSEINxArea:
                    model.Grafico.TitleText = @"GENERACIÓN ELÉCTRICA POR ÁREAS OPERATIVAS DEL SEIN";
                    lista = listaData.Where(x => x.Orden >= 13 && x.Orden <= 15).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoGrafGeneracionSEINxTipoGen:
                    model.Grafico.TitleText = @"GENERACIÓN ELÉCTRICA DEL SEIN POR TIPO DE GENERACIÓN";
                    lista = listaData.Where(x => x.Orden >= 17 && x.Orden <= 20).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoGrafGeneracionSEINxRER:
                    model.Grafico.TitleText = @"GENERACIÓN CON RECURSOS ENERGÉTICOS RENOVABLES (RER) DEL SEIN POR TIPO DE GENERACIÓN";
                    lista = listaData.Where(x => x.Orden >= 22 && x.Orden <= 25).ToList();
                    break;
            }
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            model.Grafico.SerieDataS = new DatosSerie[lista.Count][];

            //Eje X
            model.Grafico.XAxisCategories = new List<string>();
            DateTime horas = new DateTime(2013, 9, 15, 0, 0, 0);
            for (int h = 1; h <= 48; h++)
            {
                model.Grafico.XAxisCategories.Add(horas.ToString(ConstantesAppServicio.FormatoOnlyHora));
                horas = horas.AddMinutes(30);
            }

            lista = lista.OrderBy(x => x.Orden).ToList();

            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesData = new decimal?[lista.Count][];
            for (int i = 0; i < lista.Count; i++)
            {
                var pto = lista[i];
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series[i].Name = pto.Descripcion;
                model.Grafico.Series[i].Type = "area";

                switch (tipoReporte)
                {
                    case ConstantesPR5ReportesServicio.TipoGrafGeneracionSEINxArea: model.Grafico.Series[i].Color = (i == 0 ? "#3BBD87" : (i == 1 ? "#4F81BD" : "#F47618")); break;
                    case ConstantesPR5ReportesServicio.TipoGrafGeneracionSEINxTipoGen: model.Grafico.Series[i].Color = (i == 0 ? "#FF0000" : (i == 1 ? "#F9FD0F" : (i == 2 ? "#FF8B00" : "#05BBFA"))); break;
                    case ConstantesPR5ReportesServicio.TipoGrafGeneracionSEINxRER: model.Grafico.Series[i].Color = (i == 0 ? "#FF0000" : (i == 1 ? "#F9FD0F" : (i == 2 ? "#FF8B00" : "#05BBFA"))); break;
                }

                model.Grafico.Series[i].YAxis = 0;

                model.Grafico.SeriesData[i] = new decimal?[48];
                for (int h = 1; h <= 48; h++)
                {
                    model.Grafico.SeriesData[i][h - 1] = (decimal?)pto.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(pto, null);
                }
            }

            return model;
        }
        #endregion

        /// <summary>
        /// 3.13.2.10.	Horas de orden de arranque y parada, así como las horas de ingreso y salida de las Unidades de Generación del SEIN.
        /// </summary>
        /// <returns></returns>
        #region ReporteHorasOrdenAPISGeneracionSEIN
        //
        // GET: /IEOD/AnexoA/IndexReporteHorasOrdenAPIS
        public ActionResult IndexReporteHorasOrdenAPIS(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteHorasOrdenAPIS, fecha);

            model.ListaEmpresas = servicio.ObtenerEmpresasGeneradoras();
            model.Anho = DateTime.Now.Year.ToString();
            model.ListaTipoOperacion = this.servicio.GetSubCausaEventCriteria();
            model.ListaTipoCentral = this.servHO.ListarTipoCentralHOP();

            return View(model);
        }

        /// <summary>
        /// Listar repor de horas de operacion del SEIN
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idTOperacion"></param>
        /// <param name="idTCentral"></param>
        /// <param name="idTCombustible"></param>
        /// <param name="idSistemaA"></param>
        /// <param name="idOtraClasificacion"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        public JsonResult CargarHorasOrdenAPISGeneracionSEIN(string idEmpresa, string fechaInicio, string fechaFin, string modoOpe, string idTCentral, string idTCombustible)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveHoraoperacionDTO> lista, listaVersion;

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            servicio.ReporteHorasOrdenAPISDataVersionada(idEmpresa, modoOpe, idTCentral, idTCombustible, ConstantesHorasOperacion.ParamTipoOperacionTodos, fechaInicial, fechaFinal
                , "", out lista, out listaVersion);

            model.Resultado = UtilAnexoAPR5.ReporteHorasOrdenAPISHtml(lista, fechaInicial, fechaFinal, listaVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.11.	Hora de inicio y fin de las Indisponibilidades de las Unidades de Generación del SEIN y su respectivo motivo.
        /// </summary>
        /// <returns></returns>                
        #region ReporteHoraInicioFinDisponibilidad
        //
        // GET: /IEOD/AnexoA/IndexReporteHoraInicioFinIndisponibilidad
        public ActionResult IndexReporteHoraInicioFinIndisponibilidad(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteHoraInicioFinIndisponibilidad, fecha);

            model.ListaEmpresas = servicio.ObtenerEmpresasGeneradoras();
            model.ListaTipoCentral = servicio.ListarTipoCentralGenerador();
            model.Anho = DateTime.Now.Year.ToString();

            return View(model);
        }

        /// <summary>
        /// Listar reporte de Horas de indisponibilidad
        /// </summary>
        /// <param name="tiposEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposEquipo"></param>
        /// <param name="tiposMantto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarHorasIndiponibilidad(string fechaInicio, string fechaFin, string empresas, string tipoCentral)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            List<EveManttoDTO> lista, listaVersion;
            servicio.ReporteHoraIndisponibilidadesDataVersionada(empresas, tipoCentral, fechaInicial, fechaFinal
                , "", out lista, out listaVersion);

            model.Resultado = UtilAnexoAPR5.ReporteHoraIndisponibilidadesHtml(lista, fechaInicial, fechaFinal, listaVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.12.	Reserva Fría del sistema.
        /// </summary>
        /// <returns></returns>
        #region ReporteReservaFriaSistema

        //
        // GET: /IEOD/AnexoA/IndexReporteReservaFriaSistema
        public ActionResult IndexReporteReservaFriaSistema(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteReservaFriaSistema, fecha);

            model.ListaEmpresas = servicio.ObtenerEmpresasGeneradoras();
            model.Idnumeral = ConstantesAnexoAPR5.IndexReporteReservaFriaSistema;
            model.Tiporeporte = ConstantesPR5ReportesServicio.TIPO_ANEXO_A;

            return View(model);
        }

        /// <summary>
        /// Reporte HTML Reserva Fría
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarReporteReservaFriaSistema(string fechaInicio, string fechaFin, string idEmpresa, string tipoCombustible, string filtroRF)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            try
            {
                DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                List<MePtomedicionDTO> listaPto = new List<MePtomedicionDTO>();
                List<MeMedicion48DTO> totalData = new List<MeMedicion48DTO>(), listaRFDetalle;

                servicio.ReporteReservaFriaDataVersionada(fechaInicial, fechaFinal, idEmpresa, tipoCombustible
                    , "", out listaPto, out totalData, out listaRFDetalle, out CDespachoDiario regCDespachoXDia, out List<GraficoWeb> listaGrafico, out GraficoWeb graficoRFAnexoA, out GraficoWeb graficoRfriaAnexoA);

                model.Resultado = UtilAnexoAPR5.ReporteReservaFriaHtml(fechaInicial, filtroRF, listaPto, totalData);
                model.Graficos = listaGrafico;

                model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public PartialViewResult ListarUnidadesRFria(string fecha)
        {
            UnidadesRFriaModel model = new UnidadesRFriaModel();
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.ListadoUnidades = this.servicio.GetByCriteriaMeRfriaUnidadrestrics(fechaConsulta);
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult EditarUnidadRFria(int id, string fecha)
        {
            UnidadesRFriaModel model = new UnidadesRFriaModel();
            model.ListaEmpresa = (new GrupoDespachoAppServicio()).ObtenerListaEmpresas();
            model.Fecha = fecha;

            if (id == 0)
            {
                model.Entidad = new MeRfriaUnidadrestricDTO();
                model.Entidad.Urfriaactivo = 1;
                model.Emprcodi = -1;
                model.Centralcodi = -1;
                model.Grupocodi = -1;
                model.ListaCentral = new List<PrGrupoDTO>();
                model.ListaGrupo = new List<PrGrupoDTO>();
            }
            else
            {
                model.Entidad = this.servicio.GetByIdMeRfriaUnidadrestric(id);
                model.HoraInicio = ((DateTime)model.Entidad.Urfriafechaini).ToString(Constantes.FormatoHoraMinuto);
                model.HoraFin = ((DateTime)model.Entidad.Urfriafechafin).ToString(Constantes.FormatoHoraMinuto);
                int grupoCodi = (int)model.Entidad.Grupocodi;
                PrGrupoDTO unidad = (new DespachoAppServicio()).GetByIdPrGrupo(grupoCodi);
                PrGrupoDTO central = (new DespachoAppServicio()).GetByIdPrGrupo((int)unidad.Grupopadre);
                int empresa = (int)central.Emprcodi;
                model.ListaCentral = this.servicio.ObtenerCentralesPorEmpresa(empresa);
                model.ListaGrupo = this.servicio.ObtenerUnidadesPorCentral(empresa, central.Grupocodi);
                model.Emprcodi = empresa;
                model.Centralcodi = central.Grupocodi;
                model.Grupocodi = grupoCodi;
            }

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ObtenerCentralesRFria(int idEmpresa)
        {
            return Json(this.servicio.ObtenerCentralesPorEmpresa(idEmpresa));
        }

        [HttpPost]
        public JsonResult ObtenerUnidadesRFria(int idEmpresa, int central)
        {
            return Json(this.servicio.ObtenerUnidadesPorCentral(idEmpresa, central));
        }

        [HttpPost]
        public JsonResult GrabarUnidaRFria(UnidadesRFriaModel model)
        {
            try
            {
                MeRfriaUnidadrestricDTO entity = new MeRfriaUnidadrestricDTO();

                entity.Urfriacodi = model.Codigo;

                string horInil = model.HoraInicio.Split(':')[0].PadLeft(2, '0');
                string horInir = model.HoraInicio.Split(':')[1].PadLeft(2, '0');

                string horFinl = model.HoraFin.Split(':')[0].PadLeft(2, '0');
                string horFinr = model.HoraFin.Split(':')[1].PadLeft(2, '0');

                string horIni = horInil + ":" + horInir;
                string horFin = horFinl + ":" + horFinr;


                entity.Urfriafechaperiodo = DateTime.ParseExact(model.Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Urfriafechaini = DateTime.ParseExact(model.Fecha + " " + horIni, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                entity.Urfriafechafin = DateTime.ParseExact(model.Fecha + " " + horFin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                entity.Grupocodi = model.Grupocodi;
                entity.Urfriaactivo = 1;
                entity.Urfriaobservacion = model.Observacion;
                entity.Urfriausucreacion = base.UserName;
                entity.Urfriausumodificacion = base.UserName;
                entity.Urfriafeccreacion = DateTime.Now;
                entity.Urfriafecmodificacion = DateTime.Now;
                this.servicio.SaveMeRfriaUnidadrestric(entity);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult EliminarRFria(int id)
        {
            try
            {
                this.servicio.DeleteMeRfriaUnidadrestric(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite exportar los ingresos por transmisión a Excel
        /// </summary>      
        [HttpPost]
        public JsonResult ExportarUnidadesRFria(string fecha)
        {
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.Directorio;
                string file = ConstantesPR5ReportesServicio.FileUnidadesConRestriccionRFria;
                int result = this.servicio.ExportarMeRfriaUnidadrestric(fechaConsulta, path, file);
                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarUnidadesRFria()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.Directorio +
                ConstantesPR5ReportesServicio.FileUnidadesConRestriccionRFria;
            return File(fullPath, Constantes.AppExcel, ConstantesPR5ReportesServicio.FileUnidadesConRestriccionRFria);
        }

        #endregion

        /// <summary>
        /// 3.13.2.13.	Caudales en los principales afluentes a las Centrales Hidroeléctricas.
        /// </summary>
        /// <returns></returns>
        #region CaudalesCentralHidroelectrica
        //
        // GET: /IEOD/AnexoA/IndexReporteCaudalesCentralHidroelectrica
        public ActionResult IndexReporteCaudalesCentralHidroelectrica(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteCaudalesCentralHidroelectrica, fecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas).GroupBy(x => x.Emprcodi).Select(y => y.First()).ToList();

            model.ListaCuenca = this.servHidro.ListarEquiposXFamilia(ConstantesHidrologia.IdCuenca);
            int[] tipoHidro = { 4, 19, 23, 42 };
            model.ListaTipoCentral = this.servHidro.ListarFamilia().Where(x => tipoHidro.Contains(x.Famcodi)).ToList();

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Caudales de Centrales Hidroelectricas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        public JsonResult CargarListaCaudalesCentralHidroelectrica(string idEmpresa, string fechaInicio, string fechaFin, int nroPagina, string idsFamilia)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            List<MeReporptomedDTO> listaPto, listaPtoVersion;
            List<MeMedicion24DTO> lista, listaVersion;
            servicio.ReporteCaudalesCentralHidroelectricaDataVersionada(fechaInicial, fechaFinal, idEmpresa, idsFamilia
               , "", out listaPto, out lista, out listaPtoVersion, out listaVersion);

            string resultado = UtilAnexoAPR5.ReporteCaudalesCentralHidroelectricaHtml(lista, listaPto, fechaInicial, fechaInicial, listaVersion);
            model.Resultado = resultado;
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarGraficoCaudalesCentralHidroelectrica(string idEmpresa, string fechaInicio, string fechaFin, int nroPagina, string idsFamilia)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeReporptomedDTO> listaCabecera = new List<MeReporptomedDTO>();

            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;

            int[] familias = new int[idsFamilia.Length];
            familias = idsFamilia.Split(',').Select(x => int.Parse(x)).ToArray();

            fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            List<MeMedicion24DTO> lista24 = this.servHidro.ListaMed24HidrologiaTiempoReal(ConstantesPR5ReportesServicio.IdReporteCaudalesHidro, ConstantesPR5ReportesServicio.IdOrigenLectura, idEmpresa,
               fechaInicial, fechaFinal, ConstantesPR5ReportesServicio.IdTipoPtoMedhidro);

            lista24 = lista24.Where(x => familias.Contains((int)x.Famcodi)).ToList();
            List<DateTime> listaFechas = lista24.Select(x => x.Medifecha).Distinct().ToList();

            if (lista24.Count > 0)
            {
                listaCabecera = this.servicio.ListarEncabezadoMeReporptomeds(ConstantesPR5ReportesServicio.TipoReporteCaudales, idEmpresa, ConstantesPR5ReportesServicio.IdTipoPtoMedhidro)
                    .Where(x => familias.Contains((int)x.Famcodi)).ToList();
            }
            model = GraficoCaudalesCentralHidroelectrica(lista24, listaCabecera, fechaInicial, fechaInicial);
            model.Total = lista24.Count;

            var jsonResult = Json(model);
            return jsonResult;
        }

        /// <summary>
        ///  Genera el model para generar grafico y reportes en excel
        /// </summary>
        /// <param name="idsEmpresas"></param>
        /// <param name="idsCuencas"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idLectura"></param>
        /// <param name="nroPagina"></param>
        /// <param name="anho"></param>
        /// <param name="semanaIni"></param>
        /// <param name="semanaFin"></param>
        /// <param name="anhoInicial"></param>
        /// <param name="anhoFinal"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <param name="unidad"></param>
        /// <returns></returns>
        private PublicacionIEODModel GraficoCaudalesCentralHidroelectrica(List<MeMedicion24DTO> listaM24, List<MeReporptomedDTO> listaCabecera, DateTime fechaIni, DateTime fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            GraficoWeb grafico = new GraficoWeb();
            model.Grafico = grafico;
            model.Grafico.Series = new List<RegistroSerie>();

            if (listaM24.Count > 0)
            {
                var lstFechasAux = listaM24.Select(x => x.Medifecha).Distinct().ToList();// para los rangos de fechas de reporte semanal, mensual , anual           
                model.FechaInicio = fechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);
                //model.RbTipoReporte = rbDetalleRpte;

                model.Grafico.XAxisTitle = "Dia:" + fechaIni.ToString(ConstantesAppServicio.FormatoFecha);

                // obtenemos el nombre del tipo de informacion para el titulo el reporte
                model.Grafico.TitleText = "REPORTE GRÁFICO CAUDALES DE LAS CENTRALES HIDROELECTRICAS";
                model.SheetName = "GRAFICO";


                model.Grafico.YaxixTitle = "(" + listaM24[0].Tipoinfoabrev + ")";
                model.Grafico.XAxisCategories = new List<string>();

                // Obtener Lista de intervalos categoria del grafico
                int totalIntervalos = 48;
                for (var i = 0; i <= totalIntervalos; i++)
                {
                    model.Grafico.XAxisCategories.Add(fechaIni.AddMinutes(i * 30).ToString(ConstantesAppServicio.FormatoOnlyHora));
                }

                var listaGrupoMedicion = listaM24.GroupBy(x => new { x.Ptomedicodi, x.Tipoinfocodi }).Select(group => group.First()).ToList();

                int nSeries = 0;
                foreach (var reg in listaGrupoMedicion)
                {
                    string nombreSerie = reg.Equinomb;
                    var descrip = (reg.Tipoptomedinomb.Length > 15) ? reg.Tipoptomedinomb.Substring(0, 14) : reg.Tipoptomedinomb;
                    nombreSerie += " - " + descrip;

                    model.Grafico.Series.Add(new RegistroSerie());
                    model.Grafico.Series[nSeries].Name = nombreSerie;
                    model.Grafico.Series[nSeries].Data = new List<DatosSerie>();
                    model.Grafico.Series[nSeries].Type = "line";
                    nSeries++;
                }
                // Obtener lista de valores para las series del grafico
                for (var i = 0; i < listaGrupoMedicion.Count(); i++)
                {
                    //// Recorrer por cada pto medicion
                    var pto = listaGrupoMedicion[i].Ptomedicodi;
                    var lista = listaM24.Where(x => x.Ptomedicodi == pto).OrderBy(x => x.Medifecha);
                    foreach (var reg in lista)
                    {
                        for (var j = 1; j <= totalIntervalos / 2; j++)
                        {
                            for (int k = 1; k <= 2; k++)
                            {
                                decimal? valor = null;
                                DateTime fechaSerie = DateTime.MinValue;
                                valor = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(reg, null);
                                fechaSerie = reg.Medifecha.AddMinutes(((j - 1) + (k - 1)) * 30);

                                model.Grafico.Series[i].Data.Add(new DatosSerie()
                                {
                                    Y = valor,
                                    X = fechaSerie
                                });
                            }
                        }
                    }
                }

            }// end del if 
            return model;
        }

        #endregion

        /// <summary>
        /// 3.13.2.14.	Volúmenes horarios y caudales horarios de descarga de los embalses asociados a las Centrales Hidroeléctricas.
        /// </summary>
        /// <returns></returns>
        #region HorariosCaudalVolumenCentralHidroelectrica
        //
        // GET: /IEOD/AnexoA/IndexReporteHorariosCaudalVolumenCentralHidroelectrica
        public ActionResult IndexReporteHorariosCaudalVolumenCentralHidroelectrica(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteHorariosCaudalVolumenCentralHidroelectrica, fecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            return View(model);
        }

        /// <summary>
        /// Metodo de carga disenio de reporte de vista
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarReporteVertimiento(string idEmpresa, string fechaInicio, string fechaFin)
        {
            List<PublicacionIEODModel> lista = new List<PublicacionIEODModel>();
            lista.Add(this.GetModelReporteHorariosCaudalVolumenCentralHidroelectrica(idEmpresa, fechaInicio, fechaFin));
            lista.Add(this.GetModelReporteDescarga(idEmpresa, fechaInicio, fechaFin));

            return Json(lista);
        }

        /// <summary>
        /// Metodo de carga disenio de reporte de vista IndexReporteHorariosCaudalVolumenCentralHidroelectrica
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        private PublicacionIEODModel GetModelReporteHorariosCaudalVolumenCentralHidroelectrica(string idEmpresa, string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeReporptomedDTO> listaPto, listaPtoVersion;
            List<MeMedicion24DTO> lista, listaVersion;

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            servicio.ReporteHorariosVolumenEmbalseCentralHidroelectricaDataVersionada(fechaInicial, fechaFinal, idEmpresa
                , "", out listaPto, out lista, out listaPtoVersion, out listaVersion);

            string resultado = UtilAnexoAPR5.ReporteCaudalesCentralHidroelectricaHtml(lista, listaPto, fechaInicial, fechaInicial, listaVersion);
            model.Resultado = resultado;
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return model;
        }

        /// <summary>
        /// Metodo de carga disenio de reporte de vista
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        private PublicacionIEODModel GetModelReporteDescarga(string idEmpresa, string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            List<MeMedicionxintervaloDTO> listaVersion = new List<MeMedicionxintervaloDTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            servicio.ReporteDescargaLagunaDataVersionada(idEmpresa, fechaInicial, fechaFinal
                , "", out lista, out listaVersion);

            string resultado = UtilAnexoAPR5.ReporteVertimientosPeriodoVolumenHtml(lista, fechaInicial, fechaInicial, listaVersion);
            model.Resultado = resultado;

            return model;
        }

        #endregion

        /// <summary>
        /// 3.13.2.15.	Vertimientos en los embalses y/o presas en período y volumen.
        /// </summary>
        /// <returns></returns>
        #region VertimientosPeriodoVolumen
        //
        // GET: /IEOD/AnexoA/IndexReporteVertimientosPeriodoVolumen
        public ActionResult IndexReporteVertimientosPeriodoVolumen(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteVertimientosPeriodoVolumen, fecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            return View(model);
        }

        /// <summary>
        /// Metodo de carga disenio de reporte de vista
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaVertimientosPeriodoVolumen(string idEmpresa, string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicionxintervaloDTO> lista, listaVersion;

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            servicio.ReporteVertimientosPeriodoVolumenDataVersionada(idEmpresa, fechaInicial, fechaFinal, "", out lista, out listaVersion);

            model.Resultado = UtilAnexoAPR5.ReporteVertimientosPeriodoVolumenHtml(lista, fechaInicial, fechaInicial, listaVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.16.	Volúmenes o cantidad de combustible almacenado a las 24:00 h de las Centrales Térmicas.
        /// </summary>
        /// <returns></returns>
        #region CantidadCombustibleCentralTermica
        //
        // GET: /IEOD/AnexoA/IndexReporteCantidadCombustibleCentralTermica
        public ActionResult IndexReporteCantidadCombustibleCentralTermica(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteCantidadCombustibleCentralTermica, fecha);

            var empresas = this.servIEOD.ListarEmpresasxTipoEquipos(ConstantesHorasOperacion.CodFamilias);
            model.ListaEmpresas = empresas;

            var centrales = servicio.ListEqEquipoEmpresaGEN("-1").ToList();
            model.ListaCentrales = centrales;

            return View(model);
        }

        /// <summary>
        /// Metodo de carga disenio de reporte de vista CargarListaCantidadCombustibleCentralTermica
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaCantidadCombustibleCentralTermica(string idEmpresa, string idCentral, string idTipoComb, string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicionxintervaloDTO> lista, listaVersion;

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            servicio.ReporteCantidadCombustibleCentralTermicaDataVersionada(idEmpresa, idCentral, idTipoComb, fechaInicial, fechaFinal, "", out lista, out listaVersion);

            model.Resultado = UtilAnexoAPR5.ReporteCantidadCombustibleCentralTermicaHtml(lista, fechaInicial, fechaFinal, listaVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.17.	Volúmenes o cantidad diaria de combustible consumido (asociado a la generación) por cada Unidad de Generación termoeléctrica.
        /// </summary>
        /// <returns></returns>
        #region CombustibleConsumidoUnidadTermoelectrica
        //
        // GET: /IEOD/AnexoA/IndexReporteCombustibleConsumidoUnidadTermoelectrica
        public ActionResult IndexReporteCombustibleConsumidoUnidadTermoelectrica(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteCombustibleConsumidoUnidadTermoelectrica, fecha);

            var empresas = this.servIEOD.ListarEmpresasxTipoEquipos(ConstantesHorasOperacion.CodFamilias);
            model.ListaEmpresas = empresas;

            var centrales = servicio.ListEqEquipoEmpresaGEN(ConstantesAppServicio.ParametroDefecto).ToList();
            model.ListaCentrales = centrales;

            return View(model);
        }

        /// <summary>
        /// Metodo de carga disenio de reporte de vista CargarListaCombustibleConsumidoUnidadTermoelectrica
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult ListarCombustiblesConsumidoUnidTermo(string idEmpresa, string idCentral, string fechaInicio, string fechaFin, string tipoCombustible)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicionxintervaloDTO> listaReporteConsumo = new List<MeMedicionxintervaloDTO>();
            List<MeMedicionxintervaloDTO> listaVersion = new List<MeMedicionxintervaloDTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            this.servicio.ReporteCombustibleConsumidoUnidadTermoelectricaDataVersionada(idEmpresa, idCentral, tipoCombustible, fechaInicial, fechaFinal, "", out listaReporteConsumo, out listaVersion);

            model.Resultado = UtilAnexoAPR5.ReporteCombustibleConsumidoUnidadTermoelectricaHtml(listaReporteConsumo, fechaInicial, fechaFinal, listaVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.18.	Volúmenes diarios de gas natural consumido (asociado a la generación) y presión horaria del gas natural al ingreso (en el lado de alta presión) de cada Unidad de Generación termoeléctrica a gas natural.
        /// </summary>
        /// <returns></returns>
        #region ConsumoYPresionDiarioUnidadTermoelectrica
        //
        // GET: /IEOD/AnexoA/IndexReporteConsumoYPresionDiarioUnidadTermoelectrica
        public ActionResult IndexReporteConsumoYPresionDiarioUnidadTermoelectrica(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteConsumoYPresionDiarioUnidadTermoelectrica, fecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            var centrales = servicio.ListEqEquipoEmpresaGEN("-1").ToList();
            model.ListaCentrales = centrales;

            return View(model);
        }

        /// <summary>
        /// Metodo de carga disenio de reporte de vista CargarListaConsumoYPresionDiarioUnidadTermoelectrica
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaReporteGas(string idEmpresa, string idCentral, string fechaInicio, string fechaFin, int idParametro)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            switch (idParametro)
            {
                case 1:
                    List<MeMedicion24DTO> lista1, listaVersion1;
                    servicio.ReportePresionDiarioUnidadTermoelectricaDataVersionada(idEmpresa, idCentral, fechaInicial, fechaFinal, "", out lista1, out listaVersion1);

                    model.Resultado = UtilAnexoAPR5.ReportePresionDiarioUnidadTermoelectricaHtml(lista1, fechaInicial, fechaFinal, listaVersion1, idParametro);
                    break;
                case 2: //temperatura
                    List<MeMedicion24DTO> lista0, listaVersion0;
                    servicio.ReporteTemperaturaUnidadTermoelectricaDataVersionada(idEmpresa, idCentral, fechaInicial, fechaFinal, "", out lista0, out listaVersion0);

                    model.Resultado = UtilAnexoAPR5.ReportePresionDiarioUnidadTermoelectricaHtml(lista0, fechaInicial, fechaFinal, listaVersion0, idParametro);
                    break;
                case 3:
                    List<MeMedicionxintervaloDTO> lista2, listaVersion2;
                    servicio.ReporteConsumoDiarioUnidadTermoelectricaDataVersionada(idEmpresa, idCentral, fechaInicial, fechaFinal
                    , "", out lista2, out listaVersion2);

                    model.Resultado = UtilAnexoAPR5.ReporteConsumoDiarioUnidadTermoelectricaHtml(lista2, listaVersion2);
                    break;
            }

            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.19.	Reporte cada 30 minutos de la fuente de energía primaria de las unidades RER solar, geotérmica y biomasa. En caso de las Centrales Eólicas, la velocidad del viento registrada cada 30 minutos.
        /// </summary>
        /// <returns></returns>
        #region RegistroEnergia30Unidades
        //
        // GET: /IEOD/AnexoA/IndexReporteRegistoEnergia30Unidades
        public ActionResult IndexReporteRegistroEnergiaPrimaria30Unidades(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteRegistroEnergiaPrimaria30Unidades, fecha);

            model.ListaEmpresas = this.servicio.ListarEmpresaEnergiaPrimaria(DateTime.MinValue, DateTime.Now);

            return View(model);
        }

        /// <summary>
        /// Metodo de carga disenio de reporte de vista CargarListaRegistoEnergia30Unidades
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaRegistroEnergia30Unidades(string idEmpresa, string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            List<MeMedicion48DTO> lista, listaVersion;
            List<MePtomedicionDTO> listaPto, listaPtoVersion;

            servicio.ReporteRegistroEnergia30UnidadesDataVersionada(idEmpresa, fechaInicial, fechaFinal, "", out lista, out listaVersion, out listaPto, out listaPtoVersion);

            model.Resultado = UtilAnexoAPR5.ReporteRegistroEnergia30UnidadesHtml(lista, listaPto, fechaInicial, fechaFinal, listaVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.20.	En caso sea una Central de Cogeneración Calificada, deberá remitir información sobre la producción del Calor Útil de sus Unidades de Generación o el Calor Útil recibido del proceso industrial asociado, en MW
        /// </summary>
        /// <returns></returns>
        #region CalorUtilGeneracionProceso
        //
        // GET: /IEOD/AnexoA/IndexReporteCalorUtilGeneracionProceso
        public ActionResult IndexReporteCalorUtilGeneracionProceso(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteCalorUtilGeneracionProceso, fecha);

            var empresas = this.servicio.ListarEmpresaCoGeneracion(model.FechaPeriodo);
            model.ListaEmpresas = empresas;

            return View(model);
        }

        /// <summary>
        /// Metodo de carga disenio de reporte de vista CargarListaCalorUtilGeneracionProceso
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaCalorUtilGeneracionProceso(string fechaInicio, string fechaFin, string idEmpresa)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista, listaVersion;
            List<MePtomedicionDTO> listaPto, listaPtoVersion;

            servicio.ReporteCalorUtilGeneracionProcesoDataVersionada(idEmpresa, fechaInicial, fechaFinal, "", out lista, out listaVersion, out listaPto, out listaPtoVersion);

            model.Resultado = UtilAnexoAPR5.ReporteCalorUtilGeneracionProcesoHtml(lista, listaPto, fechaInicial, fechaFinal, listaVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        /// <summary>
        /// Carga la lista de centrales por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarCentralxEmpresaCoGeneracion(string idEmpresa)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            model.ListaGrupoCentral = this.servicio.ListarGrupocentralXEmpresaCoGeneracion(idEmpresa);

            return PartialView(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.21.	Registro cada 30 minutos del flujo (MW y MVAr) por las líneas de transmisión y transformadores de potencia definidos por el COES.
        /// </summary>
        /// <returns></returns>
        #region ReportePALineasTransmision
        //
        // GET: /IEOD/AnexoA/IndexReportePALineasTransmision
        public ActionResult IndexReportePALineasTransmision(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReportePALineasTransmision, fecha);

            model.ListaEmpresas = servicio.ListarEmpresaFromMeReporte(ConstantesPR5ReportesServicio.ReporcodiFlujoLineaAnexoA, ConstantesAppServicio.TipoinfocodiMW);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Potencia Activa Lineas Transmision
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idSubEstacion"></param>
        /// <returns></returns>
        public JsonResult CargarListaPALineasTransmision(string fechaIni, string fechaFin, int idPotencia, string idEmpresa)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista, listaVersion;

            servicio.ReporteFlujoPotenciaActivaTransmisionSEINDataVersionada(idEmpresa, idPotencia, fechaInicial, fechaFinal, "", out lista, out listaVersion, out List<MePtomedicionDTO> listaPto, out List<MePtomedicionDTO> listaPtoVersion);

            model.Resultado = UtilAnexoAPR5.ReporteFlujoPotenciaActivaTransmisionSEINHtml(fechaInicial, fechaFinal, idPotencia, lista, listaVersion, listaPto, listaPtoVersion);
            model.Total = lista.Count;
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.23.	Registro cada 30 minutos de la tensión de las Barras del SEIN definidas por el COES.
        /// </summary>
        /// <returns></returns>
        #region ReporteTensionBarrasSEIN
        //
        // GET: /IEOD/AnexoA/IndexReporteTensionBarrasSEIN
        public ActionResult IndexReporteTensionBarrasSEIN(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteTensionBarrasSEIN, fecha);

            model.ListaEmpresas = servicio.ListarEmpresaFromMeReporte(ConstantesPR5ReportesServicio.ReporcodiTensionBarraAnexoA, ConstantesAppServicio.TipoinfocodiKv);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Tension Barras SEIN
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public JsonResult CargarListaTensionBarrasSEIN(string fechaIni, string fechaFin, string idEmpresa)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista, listaVersion;
            List<MePtomedicionDTO> listaPto, listaPtoVersion;

            servicio.ReporteTensionBarrasSeinDataVersionada(idEmpresa, fechaInicial, fechaFinal, "", out lista, out listaVersion, out listaPto, out listaPtoVersion);

            model.Resultado = UtilAnexoAPR5.ReporteTensionBarrasSeinHtml(fechaInicial, fechaFinal, lista, listaVersion, listaPto, listaPtoVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.24.	Reporte de sobrecarga de equipos mayores a 100 kV. De presentarse sobrecarga en equipos menores a 100 kV hasta los 60 kV, que ocasione acciones correctivas en la Operación en Tiempo Real, se incluirá dicha sobrecarga en el reporte respectivo.
        /// </summary>
        /// <returns></returns>
        #region ReporteSobrecargaEquipos
        //
        // GET: /IEOD/AnexoA/IndexReporteSobrecargaEquipos
        public ActionResult IndexReporteSobrecargaEquipos(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteSobrecargaEquipos, fecha);

            return View(model);
        }

        /// <summary>
        /// Lista reporte de sobrecarga de equipos
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaSobrecargaEquipo(string fechaInicio, string fechaFin)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveIeodcuadroDTO> lista, listaVersion;

            servicio.ReporteSobrecargaEquipoDataVersionada(fechaInicial, fechaFinal, "", out lista, out listaVersion);

            model.Resultado = UtilAnexoAPR5.ReporteSobrecargaEquipoHtml(lista, listaVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.25.	Reporte de líneas desconectadas por Regulación de Tensión.
        /// </summary>
        /// <returns></returns>
        #region ReporteLineasDesconectadasPorTension
        //
        // GET: /IEOD/AnexoA/IndexReporteLineasDesconectadasPorTension
        public ActionResult IndexReporteLineasDesconectadasPorTension(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteLineasDesconectadasPorTension, fecha);

            var empresas = servicio.GetListaCriteria("1,2,3");
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de lineas Desconectadas Por Tension
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaDesconectadasPorTension(string fechaInicio, string fechaFin, string empresa)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveIeodcuadroDTO> lista, listaVersion;

            servicio.ReporteLineasDesconectadasPorTensionDataVersionada(empresa, string.Empty, fechaInicial, fechaFinal, "", out lista, out listaVersion);

            model.Resultado = UtilAnexoAPR5.ReporteLineasDesconectadasPorTensionHtml(lista, listaVersion);

            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.26.	Reporte de Sistemas Aislados Temporales
        /// </summary>
        /// <returns></returns>
        #region ReporteSistemasAisladosTemporales
        //
        // GET: /IEOD/AnexoA/IndexReporteSistemasAisladosTemporales
        public ActionResult IndexReporteSistemasAisladosTemporales(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteSistemasAisladosTemporales, fecha);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Sistemas aislados temporales
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaSistemasAisladosTemporales(string fechaInicio, string fechaFin)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveIeodcuadroDTO> lista, listaVersion;

            servicio.ReporteSistemasAisladosTemporalesDataVersionada(fechaInicial, fechaFinal, false, "", out lista, out listaVersion);

            string url = Url.Content("~/");
            model.Resultado = UtilAnexoAPR5.ReporteSistemasAisladosTemporalesHtml(false, url, lista, listaVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.27.	Reporte de las variaciones sostenidas y súbitas de frecuencia en el SEIN.
        /// </summary>
        /// <returns></returns>
        #region ReporteVariacionesSostenidasSubitasFrecuencia
        //
        // GET: /IEOD/AnexoA/IndexReporteVariacionesSostenidasSubitas
        public ActionResult IndexReporteVariacionesSostenidasSubitas(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteVariacionesSostenidasSubitas, fecha);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Variaciones Sostenidas Subitas
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="gps"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaVariacionesSostenidasSubitas(string gps, string fechaInicio)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = fechaInicial;

            servicio.GenerarIndicadores(fechaInicial);

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<FIndicadorDTO> lista, listaVersion;

            servicio.ReporteVariacionesFrecuenciaSEINDataVersionada(gps, fechaInicial, fechaFinal, "", out lista, out listaVersion, out List<MeGpsDTO> listaGPS);

            string url = Url.Content("~/");
            model.Resultado = UtilAnexoAPR5.ReporteVariacionesFrecuenciaSEINHtml(lista, url, listaGPS);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="gps"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoVariacionesSostenidasSubitas(int gps, string fechaInicio)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            servicio.GraficoVariacionesFrecuenciaSEINDataVersionada(gps, fechaInicial, "", out List<FLecturaDTO> listaFrecRango,
                                            out List<FLecturaDTO> listaFrecDebajo, out List<FLecturaDTO> listaFrecEncima, out GraficoWeb graficoCampana);

            model.Grafico = graficoCampana;
            model.Resultado = UtilAnexoAPR5.ReporteRangoFrecuenciaHtml(listaFrecRango);
            model.Resultado2 = UtilAnexoAPR5.ReporteUmbralFrecuenciaHtml(true, listaFrecDebajo);
            model.Resultado3 = UtilAnexoAPR5.ReporteUmbralFrecuenciaHtml(false, listaFrecEncima);

            model.NRegistros = 1;

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        /// <summary>
        /// 3.13.2.28.	Reporte de Sistemas Aislados Temporales y sus variaciones sostenidas y súbitas de frecuencia.
        /// </summary>
        /// <returns></returns>
        #region ReporteSistemasAisladosTemporalesYVariacionesSostenidasSubitas
        //
        // GET: /IEOD/AnexoA/IndexReporteSistemasAisladosTemporalesYVariacionesSostenidasSubitas
        public ActionResult IndexReporteSistemasAisladosTemporalesYVariacionesSostenidasSubitas(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteSistemasAisladosTemporalesYVariacionesSostenidasSubitas, fecha);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Sistemas aislados temporales
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaSistemasAisladosTemporalesYVariacionesSostenidasSubitas(string fechaInicio)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();

            servicio.ReporteSistemasAisladosTemporalesYVariacionesSostenidasSubitasDataVersionada(ConstantesAppServicio.ParametroDefecto, fechaInicial, "", out List<InfSGIAisladosTempGPS> lista);

            model.Resultado = UtilAnexoAPR5.ReporteSistemasAisladosTemporalesYVariacionesSostenidasSubitasHtml(lista);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaInicial);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.29.	Reporte de las interrupciones de suministro, Esquema Rechazo Automático de Carga (ERAC) y Rechazo Manual de Carga (RMC), así como Racionamiento
        /// , conforme a lo establecido en el Procedimiento Técnico del COES N° 16 “Racionamiento por déficit de oferta” (PR-16) o el que lo sustituya.
        /// </summary>
        /// <returns></returns>
        #region ReporteInterrupSumERACyRMCRacionamiento
        //
        // GET: /IEOD/AnexoA/IndexReporteInterrupSumERACyRMCRacionamiento
        public ActionResult IndexReporteInterrupSumERACyRMCRacionamiento(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteInterrupSumERACyRMCRacionamiento, fecha);

            return View(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.30.	Desviaciones de la demanda respecto a su pronóstico
        /// </summary>
        /// <returns></returns>
        #region ReporteDesviacionesDemandaPronostico
        //
        // GET: /IEOD/AnexoA/IndexReporteDesviacionesDemandaPronostico
        public ActionResult IndexReporteDesviacionesDemandaPronostico(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteDesviacionesDemandaPronostico, fecha);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Desviaciones Demanda Pronostico
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        public JsonResult CargarListaDesviacionesDemandaPronostico(string fechaInicio, string fechaFin)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista, listaVersion;

            servicio.ReporteDesviacionDemandaPronosticoDataVersionada(fechaInicial, fechaFinal, "", out lista, out listaVersion);

            model.Resultado = UtilAnexoAPR5.ReporteDesviacionDemandaPronosticoHtml(lista, fechaInicial, fechaFinal, listaVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            PublicacionIEODModel model2 = GraficoDemandaPronostico(lista, fechaInicial, fechaFinal);

            listaModel.Add(model);
            listaModel.Add(model2);

            var jsonResult = Json(listaModel);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Crear objeto de Grafico Demanda Pronostico
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoDemandaPronostico(List<MeMedicion48DTO> lista, DateTime fechaIni, DateTime fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            DateTime fechaFinAux = fechaFin;
            model.Grafico = grafico;
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();

            model.Grafico.SerieDataS = new DatosSerie[2][];
            model.Grafico.Series.Add(new RegistroSerie());
            model.Grafico.Series.Add(new RegistroSerie());

            model.Grafico.Series[0].Name = "Ejecutado";
            model.Grafico.Series[0].Type = "line";
            model.Grafico.Series[0].Color = "#3498DB";
            model.Grafico.Series[0].YAxisTitle = "MW";
            model.Grafico.Series[1].Name = "Programación Diaria";
            model.Grafico.Series[1].Type = "line";
            model.Grafico.Series[1].Color = "#DC143C";
            model.Grafico.Series[1].YAxisTitle = "MW";

            model.Grafico.TitleText = @"Desviaciones de la Demanada Respecto a su Pronostico";
            model.Grafico.YAxixTitle.Add("MW");

            if (lista.Count > 0)
            {
                int numDia = Convert.ToInt32((fechaFin.Subtract(fechaIni)).TotalDays) + 1;

                model.Grafico.SerieDataS[0] = new DatosSerie[48 * numDia];
                model.Grafico.SerieDataS[1] = new DatosSerie[48 * numDia];

                model.FechaInicio = fechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia:Horas";

                // titulo el reporte
                model.SheetName = "GRAFICO";
                model.Grafico.YaxixTitle = "(MWh)";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
                model.Grafico.SeriesYAxis.Add(0);

                int indiceDia = 0;
                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    //Buscamos Demanda Ejecutada
                    var objEjecutado = lista.Find(x => x.Medifecha == day && x.Lectcodi != ConstantesPR5ReportesServicio.LectCodiProgDiaria);
                    //Buscamos Demanda Proyectada Diaria
                    var objProgDiario = lista.Find(x => x.Medifecha == day && x.Lectcodi == ConstantesPR5ReportesServicio.LectCodiProgDiaria);

                    for (var j = 1; j <= 48; j++)
                    {
                        decimal? valor1 = null, valor2 = null;
                        if (objEjecutado != null)
                        {
                            valor1 = (decimal?)objEjecutado.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(objEjecutado, null);
                            valor1 = valor1 != null ? Math.Round(valor1.Value, 1) : valor1;
                        }

                        if (objProgDiario != null)
                        {
                            valor2 = (decimal?)objProgDiario.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(objProgDiario, null);
                            valor2 = valor2 != null ? Math.Round(valor2.Value, 1) : valor1;
                        }

                        model.Grafico.XAxisCategories.Add(day.AddMinutes(j * 30).ToString(ConstantesAppServicio.FormatoFechaHora));
                        var serieEjec = new DatosSerie();
                        serieEjec.X = day.AddMinutes(j * 30);
                        serieEjec.Y = valor1;

                        var serieProg = new DatosSerie();
                        serieProg.X = day.AddMinutes(j * 30);
                        serieProg.Y = valor2;

                        model.Grafico.SerieDataS[0][indiceDia * 48 + (j - 1)] = serieEjec;
                        model.Grafico.SerieDataS[1][indiceDia * 48 + (j - 1)] = serieProg;
                    }

                    indiceDia++;
                }
            }

            return model;
        }

        #endregion

        /// <summary>
        /// 3.13.2.31.	Desviaciones de la producción de las Unidades de Generación
        /// </summary>
        /// <returns></returns>
        #region ReporteDesviacionesProduccionUG
        //
        // GET: /IEOD/AnexoA/IndexReporteDesviacionesProduccionUG
        public ActionResult IndexReporteDesviacionesProduccionUG(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteDesviacionesProduccionUG, fecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Desviaciones produccion de las unidades de generacion
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDesviacionesProduccionUG(string fechaInicio, string fechaFin, string empresa)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista, listaVersion;
            List<MePtomedicionDTO> listaPto, listaPtoVersion;

            this.servicio.ReporteDesviacionesProduccionUGDataVersionada(empresa, fechaInicial, fechaFinal, "", out lista, out listaVersion, out listaPto, out listaPtoVersion);

            model.Resultado = UtilAnexoAPR5.ReporteDesviacionesProduccionUGHtml(lista, listaPto, fechaInicial, listaVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        /// <summary>
        /// 3.13.2.32.	Costos Marginales de Corto Plazo cada 30 minutos en las Barras del SEIN.
        /// </summary>
        /// <returns></returns>
        #region ReporteCostoMarginalesCortoPlazo
        //
        // GET: /IEOD/AnexoA/IndexReporteCostoMarginalesCortoPlazo
        public ActionResult IndexReporteCostoMarginalesCortoPlazo(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteCostoMarginalesCortoPlazo, fecha);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Costo Marginales CP
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaCostoMarginalesCP(string fechaInicio)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<SiCostomarginalDTO> lista, listaVersion;

            servicio.ReporteCostoMarginalesCPDataVersionada(fechaInicial, "", out lista, out listaVersion, out List<BarraDTO> listaBarra);

            model.Resultado = UtilAnexoAPR5.ReporteCostoMarginalesCPHtml(fechaInicial, 1, lista, listaBarra);
            model.Resultado2 = UtilAnexoAPR5.ReporteCostoMarginalesCPHtml(fechaInicial, 2, lista, listaBarra);
            model.Resultado3 = UtilAnexoAPR5.ReporteCostoMarginalesCPHtml(fechaInicial, 3, lista, listaBarra);
            model.Fecha = fechaInicial;
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaInicial);

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        /// <summary>
        /// 3.13.2.33.	Costo total de operación ejecutada.
        /// </summary>
        /// <returns></returns>
        #region ReporteCostoTotalOperacionEjecutada
        //
        // GET: /IEOD/AnexoA/IndexReporteCostoTotalOperacionEjecutada
        public ActionResult IndexReporteCostoTotalOperacionEjecutada(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteCostoTotalOperacionEjecutada, fecha);

            return View(model);
        }

        /// <summary>
        /// Gráfico RESUMEN DE GENERACIÓN POR ÁREAS DEL SEIN
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoReporte"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarGraficoCostoTotalOperacionEjecutada(string fecha)
        {
            DateTime fechaInicial = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = fechaInicial;

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion1DTO> lista, listaVersion;

            servicio.ReporteCostoTotalOperacionEjecutadaDataVersionada(fechaInicial, fechaFinal, "", out lista, out listaVersion, out GraficoWeb graficoWeb);

            model.Grafico = graficoWeb;
            model.Mensaje = graficoWeb.Subtitle;
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        /// <summary>
        /// 3.13.2.34. Calificación de la Operacion de las Unidades de Generación
        /// </summary>
        /// <returns></returns>
        #region ReporteCalificacionDelaOperaciónDelasUnidadesDeGeneracion
        public ActionResult IndexReporteCalificacionOperacionUnidades(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteCalificacionOperacionUnidades, fecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            model.ListaTipoOperacion = this.servHO.ListarTipoOperacionHO();
            model.ListaTipoCentral = this.servHO.ListarTipoCentralHOP();

            model.Anho = DateTime.Now.Year.ToString();

            return View(model);
        }

        /// <summary>
        /// CargarCalificacionReporte
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idTOperacion"></param>
        /// <param name="idTCentral"></param>
        /// <param name="idTCombustible"></param>
        /// <param name="nroPagina"></param>
        /// <param name="idSistemaAv"></param>
        /// <param name="idOtraClasificacion"></param>
        /// <returns></returns>
        public JsonResult CargarCalificacionReporte(string idEmpresa, string fechaInicio, string fechaFin, string idTOperacion, string idTCentral, string idTCombustible)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveHoraoperacionDTO> lista, listaVersion;

            servicio.ReporteHorasOrdenAPISDataVersionada(idEmpresa, ConstantesAppServicio.ParametroDefecto, idTCentral, idTCombustible, idTOperacion, fechaInicial, fechaFinal, "", out lista, out listaVersion);

            model.Resultado = UtilAnexoAPR5.ReporteCalificacionHoraOperacionHtml(lista, fechaInicial, fechaFinal, listaVersion);

            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.35.	Registro de las congestiones del Sistema de Transmisión.
        /// </summary>
        /// <returns></returns>
        #region ReporteRegistroCongestionesST
        //
        // GET: /IEOD/AnexoA/IndexReporteRegistroCongestionesST
        public ActionResult IndexReporteRegistroCongestionesST(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteRegistroCongestionesST, fecha);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Registro Congestiones ST
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaRegistroCongestionesST(string fechaInicio, string fechaFin)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveIeodcuadroDTO> lista, listaVersion;

            servicio.ReporteRegistroCongestionesSTDataVersionada(fechaInicial, fechaFinal, "", out lista, out listaVersion);

            model.Resultado = UtilAnexoAPR5.ReporteRegistroCongestionesSTHtml(lista, listaVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.36.	Registro de asignación de la RRPF y RRSF
        /// </summary>
        /// <returns></returns>
        #region ReporteAsignacionRRPFyRRSF
        //
        // GET: /IEOD/AnexoA/IndexReporteAsignacionRRPFyRRSF
        public ActionResult IndexReporteAsignacionRRPFyRRSF(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteAsignacionRRPFyRRSF, fecha);

            decimal? valor = this.servicio.GetMagnitudRPF(model.FechaPeriodo);
            model.MagnitudRPF = valor != null ? String.Format("{0:0.00}", valor * 100) + "%" : string.Empty;

            return View(model);
        }

        /// <summary>
        /// Reporte HTML Registro de asignación de la RRPF y RRSF
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaAsignacionRRPFyRRSF(string fechaInicio)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();
            string[][] lista, listaVersion;

            servicio.ReporteAsignacionRRPFyRRSDataVersionada(fechaInicial, fechaFinal, "", out lista, out listaVersion, out decimal? magnitudRpf, out decimal? magnitudRpfVersion);

            model.Resultado = UtilAnexoAPR5.ReporteAsignacionRRPFyRRSHtml(fechaInicial, lista, listaVersion);
            model.Resultado2 = magnitudRpf != null ? String.Format("{0:0.00}", magnitudRpf) + "%" : string.Empty;
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.37.	Registro de los flujos (MW y MVAr) cada 30 minutos de los enlaces internacionales.
        /// </summary>
        /// <returns></returns>
        #region ReporteRegistroFlujosEnlacesInternacionales
        //
        // GET: /IEOD/AnexoA/IndexReporteRegistroFlujosEnlacesInternacionales
        public ActionResult IndexReporteRegistroFlujosEnlacesInternacionales(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexReporteRegistroFlujosEnlacesInternacionales, fecha);

            model.ListaEmpresas = servicio.ListarEmpresaFromMeReporte(ConstantesPR5ReportesServicio.ReporcodiFlujoLineaTIEAnexoA, ConstantesAppServicio.TipoinfocodiMW);

            return View(model);
        }

        /// <summary>
        /// Cargar reporte de flujo de enlaces internacionales
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idSubEstacion"></param>
        /// <returns></returns>
        public JsonResult CargarListaRegistroFlujosEI(string fechaInicio, string fechaFin)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista, listaVersion;
            List<MePtomedicionDTO> listaHojaPto, listaHojaPtoVersion;

            servicio.ReporteRegistroFlujosEIDataVersionada(fechaInicial, fechaFinal, "", out lista, out listaVersion, out listaHojaPto, out listaHojaPtoVersion);

            model.Resultado = UtilAnexoAPR5.ReporteRegistroFlujosEIHtml(fechaInicial, lista, listaVersion, listaHojaPto, listaHojaPtoVersion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        /// <summary>
        /// 3.13.2.38.	Observaciones
        /// </summary>
        /// <returns></returns>
        #region Observacion
        //
        // GET: /IEOD/AnexoA/IndexObservacion
        public ActionResult IndexObservacion(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexObservacion, fecha);

            return View(model);
        }

        /// <summary>
        /// Guardar la observacion
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="mrepcodi"></param>
        /// <param name="verscodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaNota(string periodo, int mrepcodi)
        {
            DateTime fecha = DateTime.ParseExact(periodo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            this.servicio.ListarNotasObservaciones(fecha, "", mrepcodi, out List<SiNotaDTO> lista, out List<SiNotaDTO> listaVersion);
            lista = lista.Where(x => x.Sinotatipo != 1).ToList();

            return Json(lista);
        }

        /// <summary>
        /// Guardar la informacion de la nota
        /// </summary>
        /// <param name="nota"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarNota(SiNotaDTO nota)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (nota.Sinotacodi == 0)
                {
                    DateTime fecha = DateTime.ParseExact(nota.SinotaperiodoDesc, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                    nota.Sinotaperiodo = fecha;
                    nota.Sinotafeccreacion = DateTime.Now;
                    nota.Sinotaestado = 1;//Activo
                    nota.Sinotausucreacion = base.UserName;
                    nota.Verscodi = null;
                    this.servicio.SaveSiNota(nota);
                }
                else
                {
                    nota = servicio.GetByIdSiNota(nota.Sinotacodi);

                    nota.Sinotafecmodificacion = DateTime.Now;
                    nota.Sinotaestado = 1;//Activo
                    nota.Sinotausumodificacion = base.UserName;
                    nota.Verscodi = null;
                    this.servicio.UpdateSiNota(nota);
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Eliminar nota
        /// </summary>
        /// <param name="Sinotacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteNota(int Sinotacodi)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            try
            {
                base.ValidarSesionJsonResult();

                this.servicio.DeleteSiNota(Sinotacodi);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Actualización del orden
        /// </summary>
        /// <param name="dataCambioJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateListaNota(string periodo, int mrepcodi, int id, int fromPosition, int toPosition, string direction)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            try
            {
                periodo = periodo.Replace("a", "/");
                DateTime fecha = DateTime.ParseExact(periodo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                this.servicio.ListarNotasObservaciones(fecha, "", mrepcodi, out List<SiNotaDTO> lista, out List<SiNotaDTO> listaVersion);
                lista = lista.Where(x => x.Sinotatipo != 1).ToList();

                if (direction == "back")
                {
                    int orden = toPosition;
                    List<SiNotaDTO> ltmp = new List<SiNotaDTO>();
                    ltmp.Add(lista[fromPosition - 1]);
                    ltmp.AddRange(lista.GetRange(toPosition - 1, fromPosition - toPosition));
                    foreach (var reg in ltmp)
                    {
                        reg.Sinotafecmodificacion = DateTime.Today;
                        reg.Sinotausumodificacion = base.UserName;
                        reg.Sinotaorden = orden;
                        this.servicio.UpdateSiNotaOrden(reg); //Actualizar el orden
                        orden++;
                    }
                }
                else
                {
                    int orden = fromPosition;
                    List<SiNotaDTO> ltmp = new List<SiNotaDTO>();
                    ltmp.AddRange(lista.GetRange(fromPosition, toPosition - fromPosition));
                    ltmp.Add(lista[fromPosition - 1]);
                    foreach (var reg in ltmp)
                    {
                        reg.Sinotafecmodificacion = DateTime.Today;
                        reg.Sinotausumodificacion = base.UserName;
                        reg.Sinotaorden = orden;
                        this.servicio.UpdateSiNotaOrden(reg); //Actualizar el orden
                        orden++;
                    }
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }
        #endregion

        /// <summary>
        /// 3.13.2.39.	 Recomendaciones y Conclusiones
        /// </summary>
        /// <returns></returns>
        #region RecomendacionConclusion
        //
        // GET: /IEOD/AnexoA/IndexObservacion
        public ActionResult IndexRecomendacionConclusion(string fecha)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesAnexoAPR5.IndexRecomendacionConclusion, fecha);

            return View(model);
        }

        #endregion
        }
    }