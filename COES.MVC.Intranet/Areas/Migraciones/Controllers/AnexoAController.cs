using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Hidrologia.Helper;
using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.MVC.Intranet.Areas.StockCombustibles.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using COES.Servicios.Aplicacion.Pruebaunidad;
using COES.Servicios.Aplicacion.Scada.Helper;
using COES.Servicios.Aplicacion.StockCombustibles;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Mvc;
namespace COES.MVC.Intranet.Areas.Migraciones.Controllers
{
    public class AnexoAController : BaseController
    {
        IEODAppServicio servIEOD = new IEODAppServicio();
        PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();
        HorasOperacionAppServicio servHO = new HorasOperacionAppServicio();
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        EventoAppServicio servicioEvento = new EventoAppServicio();
        RestriccionesOperativasAppServicio servicioRestriciones = new RestriccionesOperativasAppServicio();
        HidrologiaAppServicio logic = new HidrologiaAppServicio();
        StockCombustiblesAppServicio servicioConsumo = new StockCombustiblesAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(AnexoAController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        private static List<EstadoModel> ListaEstadoSistemaA = new List<EstadoModel>();

        #region declaracion de variables
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

        /// <summary>
        /// Inicializar listas utilizadas en los demas metodos
        /// </summary>
        public AnexoAController()
        {
            ListaEstadoSistemaA = new List<EstadoModel>();
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "0", EstadoDescripcion = "NO" });
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "1", EstadoDescripcion = "SÍ" });
        }

        /// <summary>
        /// Almacena los fechas del reporte
        /// </summary>
        public List<DateTime> ListaFechas
        {
            get
            {
                return (Session[ConstantesPR5ReportesServicio.ListaFechas] != null) ?
                    (List<DateTime>)Session[ConstantesPR5ReportesServicio.ListaFechas] : new List<DateTime>();
            }
            set { Session[ConstantesPR5ReportesServicio.ListaFechas] = value; }
        }

        /// <summary>
        /// Almacena filtro de fecha para seteo de variable de busqueda Amexo A PR5
        /// </summary>
        public DateTime FechaFilter1
        {
            get
            {
                return (Session[ConstantesPR5ReportesServicio.FechaFilter1] != null) ?
                    (DateTime)Session[ConstantesPR5ReportesServicio.FechaFilter1] : DateTime.Now.AddDays(-1);
            }
            set { Session[ConstantesPR5ReportesServicio.FechaFilter1] = value; }
        }

        /// <summary>
        /// Almacena filtro de fecha para seteo de variable de busqueda Amexo A PR5
        /// </summary>
        public DateTime FechaFilter2
        {
            get
            {
                return (Session[ConstantesPR5ReportesServicio.FechaFilter2] != null) ?
                    (DateTime)Session[ConstantesPR5ReportesServicio.FechaFilter2] : new DateTime();
            }
            set { Session[ConstantesPR5ReportesServicio.FechaFilter2] = value; }
        }

        /// <summary>
        /// Almacena filtro de fecha para seteo de variable de busqueda Amexo A PR5
        /// </summary>
        public string VersionAnexoA
        {
            get
            {
                return (Session[ConstantesPR5ReportesServicio.VersionAnexoA] != null) ?
                    Session[ConstantesPR5ReportesServicio.VersionAnexoA].ToString() : "";
            }
            set { Session[ConstantesPR5ReportesServicio.VersionAnexoA] = value; }
        }

        /// <summary>
        /// Almacena filtro de fecha para seteo de variable de busqueda Amexo A PR5
        /// </summary>
        public string Tiprepcodi
        {
            get
            {
                return (Session[ConstantesPR5ReportesServicio.Tiprepcodi] != null) ?
                    Session[ConstantesPR5ReportesServicio.Tiprepcodi].ToString() : "";
            }
            set { Session[ConstantesPR5ReportesServicio.Tiprepcodi] = value; }
        }

        /// <summary>
        /// Almacena filtro de fecha para seteo de variable de busqueda Amexo A PR5
        /// </summary>
        public string IndexReporte
        {
            get
            {
                return (Session[ConstantesPR5ReportesServicio.IndexReporte] != null) ?
                    Session[ConstantesPR5ReportesServicio.IndexReporte].ToString() : "";
            }
            set { Session[ConstantesPR5ReportesServicio.IndexReporte] = value; }
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

        #endregion

        #region UTIL

        /// <summary>
        /// Genera Lista Tipo Integrante para Filtros de Reporte
        /// </summary>
        /// <returns></returns>
        public List<TipoInformacion2> ObtenerListaTipoAgente()
        {
            List<TipoInformacion2> lista = new List<TipoInformacion2>();
            var elemento = new TipoInformacion2() { IdTipoInfo = 'S', NombreTipoInfo = "INTEGRANTE" };
            lista.Add(elemento);
            elemento = new TipoInformacion2() { IdTipoInfo = 'N', NombreTipoInfo = "NO INTEGRANTE" };
            lista.Add(elemento);
            return lista;
        }
        /// <summary>
        /// Genera Lista Central Integrante para Filtros de Reporte
        /// </summary>
        /// <returns></returns>
        public List<TipoInformacion> ObtenerCentralIntegrante()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 1, NombreTipoInfo = "COES" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 2, NombreTipoInfo = "NO COES" };
            lista.Add(elemento);
            return lista;
        }

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
            var lista = this.logic.ListMeTipopuntomedicions(ConstantesHidrologia.IdOrigenHidro.ToString());
            model.ListaTipoPtoMedicion = lista.Where(x => tipoInfocodis.Contains(x.Tipoinfocodi)).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Carga lista de modos de operacion y grupos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoCentral"></param>
        /// <returns></returns>
        public PartialViewResult CargarModosOpeGrupos(string idEmpresa, string idTipoCentral)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            List<IndCuadro3DTO> ListaModosOperacion = new List<IndCuadro3DTO>();
            List<IndCuadro3DTO> ListaGrupos = new List<IndCuadro3DTO>();

            if (string.IsNullOrEmpty(idEmpresa)) idEmpresa = "0";
            int[] empresas = new int[idEmpresa.Length];
            empresas = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            if (string.IsNullOrEmpty(idTipoCentral)) idTipoCentral = "0";
            int[] tipocentrales = new int[idTipoCentral.Length];
            tipocentrales = idTipoCentral.Split(',').Select(x => int.Parse(x)).ToArray();

            if (tipocentrales.Contains(3) || tipocentrales.Contains(5))
            {
                //Modos de operacion
                ListaModosOperacion = servHO.ListarModoOperacionXCentralYEmpresa(-2, Int32.Parse(ConstantesHorasOperacion.ParamEmpresaTodos)).Where(y => empresas.Contains((int)y.Emprcodi) && y.Catecodi == 2)
                                .Select(y => new IndCuadro3DTO()
                                {
                                    IdModoOpeOGrupo = y.Grupocodi,
                                    Tipo = "MO",
                                    Valor = y.Gruponomb,
                                    Equicodi = y.Equicodi
                                }
                                ).ToList();
            }

            if (tipocentrales.Contains(2) || tipocentrales.Contains(4) || tipocentrales.Contains(37) || tipocentrales.Contains(39))
            {
                //Grupos
                ListaGrupos = servIEOD.ListarCentralesXEmpresaGener2(idEmpresa, idTipoCentral).Where(x => (x.Famcodi != 5 && x.Famcodi != 3))
                                        .Select(y => new IndCuadro3DTO()
                                        {
                                            IdModoOpeOGrupo = Convert.ToInt32(y.Equicodi),
                                            Tipo = "GR",
                                            Valor = y.Equinomb,
                                            Equicodi = y.Equicodi
                                        }
                                    ).Where(x => x.IdModoOpeOGrupo > 0).ToList();

                ListaModosOperacion.AddRange(ListaGrupos);
            }

            model.ListModosOpeGrupos = ListaModosOperacion.GroupBy(x => new { x.IdModoOpeOGrupo, x.Valor }).Select(x => new IndCuadro3DTO() { IdModoOpeOGrupo = x.Key.IdModoOpeOGrupo, Valor = x.Key.Valor }).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de tipo de combustible por codigo de central
        /// </summary>
        /// <param name="idTipoCentral"></param>
        /// <returns></returns>
        public PartialViewResult CargarTipoCombustible(string idTipoCentral)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();

            if (string.IsNullOrEmpty(idTipoCentral)) idTipoCentral = "0";
            int[] tipoCentral = new int[idTipoCentral.Length];
            tipoCentral = idTipoCentral.Split(',').Select(x => int.Parse(x)).ToArray();

            if (!string.IsNullOrEmpty(idTipoCentral))
            {
                //lista = servicio.ListaComboxReportePotenciaGeneradaxTipoRecurso().Where(x => tipoCentral.Contains(x.Famcodi)).ToList();
            }
            else
            {
                //lista = servicio.ListaComboxReportePotenciaGeneradaxTipoRecurso();
            }

            model.ListaCombustibles = lista.GroupBy(x => x.Fenergnomb).Select(y => y.First()).ToList();

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
        /// Carga la lista de tipos de grupos
        /// </summary>
        /// <returns></returns>
        public PartialViewResult CargarClasificacion()
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.ListaClasificacion = servicio.ListarPrTipoGrupo();

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de opciones de sistema aislado
        /// </summary>
        /// <returns></returns>
        public PartialViewResult CargarSistemaAislado()
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.listaEstadoSistemaA = ListaEstadoSistemaA;

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de tipo de recurso por tipo de central
        /// </summary>
        /// <param name="idtipocentral"></param>
        /// <returns></returns>
        public PartialViewResult CargarTipoRecursoXTipo(string idtipocentral)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> Lista = new List<MeMedicion48DTO>();
            //int [] Result = new int [idtipocentral.Length];
            //Result = idtipocentral.Split(',').Select(x => int.Parse(x)).ToArray();

            if (!string.IsNullOrEmpty(idtipocentral))
            {
                //Lista = servicio.ListaComboxReportePotenciaGeneradaxTipoRecurso().Where(x => idtipocentral.Contains(Convert.ToString(x.Famcodi))).ToList();
            }
            else
            {
                //Lista = servicio.ListaComboxReportePotenciaGeneradaxTipoRecurso();
            }


            model.ListaPotenciaxTipoRecurso = Lista.GroupBy(x => x.Fenergnomb).Select(y => y.First()).ToList();
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
            model.ListaTipoCentrales = lista.GroupBy(x => x.Famnomb).Select(y => y.First()).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de area operativas 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult CargarAreaOperativa()
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.ListaAreaOperativa = this.servicio.GetListaAreaAndSubareaOperativa();

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
        /// Carga la lista de tipo de combustible por central
        /// </summary>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public PartialViewResult CargarTipoCombustibleXCentral(string idCentral)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            List<SiFuenteenergiaDTO> lista = new List<SiFuenteenergiaDTO>();

            List<SiFuenteenergiaDTO> entitys = new List<SiFuenteenergiaDTO>();
            idCentral = string.IsNullOrEmpty(idCentral) ? ConstantesAppServicio.ParametroDefecto : idCentral;
            if (idCentral != ConstantesAppServicio.ParametroDefecto)
            {
                entitys = servicio.ListTipoCombustibleXEquipo(idCentral).ToList();
            }
            else
            {
                entitys = servicio.ListTipoCombustibleXEquipo("-1").ToList();
            }

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

            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            if (idEmpresa != "-1")
            {
                if (idEmpresa.Equals(string.Empty)) { idEmpresa = ConstantesAppServicio.ParametroDefecto; }
                int[] empresas = idEmpresa.Split(',').Select(int.Parse).ToArray();
                entitys = servicio.ListarAreaXEmpresas(ConstantesAppServicio.ParametroDefecto).ToList();
                entitys = entitys.Where(x => empresas.Contains(x.Emprcodi)).ToList();
            }

            model.ListaUbicacion = entitys;

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
        /// Carga la lista de subestaciones por empresa
        /// </summary>
        /// <returns></returns>
        public PartialViewResult CargarSubEstacionFlujoPotencia(string idEmpresa)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            model.ListaSubEstacion = this.servicio.ListaSubEstacionXFormato(ConstantesPR5ReportesServicio.IdFormatoTensionBarra, idEmpresa, DateTime.Now);

            return PartialView(model);
        }

        /// <summary>
        /// Carga la lista de GPS por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarGPS(string idEmpresa)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            idEmpresa = "-1";
            List<MeGpsDTO> lista = this.servicio.ListarGpsByEmpresa(idEmpresa);
            model.ListaGps = lista;

            return PartialView(model);
        }

        /// <summary>
        /// Serializando any lista
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        private byte[] SerializandoReporteAnexoA(object lista)
        {
            //Serializando
            byte[] listByte = null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, lista);
                listByte = ms.ToArray();
            }

            return listByte;
        }

        /// <summary>
        /// Deserializando any objeto byte
        /// </summary>
        /// <param name="listByte"></param>
        /// <returns></returns>
        private object DeserializandoReporteAnexoA(byte[] listByte)
        {
            //Deserializando
            BinaryFormatter bf = new BinaryFormatter();
            object listArray = null;
            using (MemoryStream ms = new MemoryStream(listByte))
            {
                listArray = (object)bf.Deserialize(ms);
            }

            return listArray;
        }
        #endregion

        #region "Menu"
        //
        // GET: /IEOD/AnexoA/MenuAnexoA/
        public ActionResult MenuAnexoA()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            BusquedaIEODModel model = new BusquedaIEODModel();
            DateTime finicio = this.FechaFilter1;

            this.Tiprepcodi = ConstantesPR5ReportesServicio.ReptipcodiAnexoAidcos + string.Empty;
            model.FechaInicio = finicio.ToString(Constantes.FormatoFecha);
            model.FechaFin = finicio.AddDays(1).ToString(Constantes.FormatoFecha);
            model.NroReporte = this.Tiprepcodi;

            return View(model);
        }

        /// <summary>
        /// Cargar menu de opciones
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CargarMenu(int id)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            List<SiMenureporteDTO> ListCat = new List<SiMenureporteDTO>();
            List<SiMenureporteDTO> ListMenu = new List<SiMenureporteDTO>();
            ListCat = servicio.GetListaMenuPR5().ToList().Where(z => z.Reptiprepcodi == ConstantesPR5ReportesServicio.ReptipcodiAnexoAidcos && z.Repcatecodi == -1).OrderBy(c => c.Reporden).ToList();
            if (id.Equals(-1)) { id = int.Parse(this.Tiprepcodi); }
            ListMenu = servicio.GetListaMenuPR5().Where(z => z.Reptiprepcodi == id && z.Repstado == 1).OrderBy(c => c.Repcodi).ToList();
            model.Menu = this.servicio.ListaMenu(ListCat, ListMenu);
            return Json(model);
        }

        /// <summary>
        /// Determinar si la sesion es válida, si se selecciono fecha para reporte de Anexo A
        /// </summary>
        /// <returns></returns>
        public bool EsOpcionValida()
        {
            return base.IsValidSesion
                && this.Tiprepcodi != null && this.Tiprepcodi.Trim() != string.Empty
                && Session[ConstantesPR5ReportesServicio.FechaFilter1] != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RedireccionarOpcionValida()
        {
            if (!base.IsValidSesion)
            {
                return base.RedirectToLogin();
            }
            else
            {
                return RedirectToAction("MenuAnexoA", "Migraciones/AnexoA", new { area = string.Empty });
            }
        }

        /// <summary>
        /// Establecer valor a filtro de fecha
        /// </summary>
        /// <param name="fec"></param>
        /// <returns></returns>
        public JsonResult SetearFechaFilterA(string fec1, string fec2, string versi)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            DateTime dtfecha1 = DateTime.MinValue, dtfecha2 = DateTime.MinValue;
            try
            {
                dtfecha1 = DateTime.ParseExact(fec1, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dtfecha2 = DateTime.ParseExact(fec2, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                this.FechaFilter1 = dtfecha1;
                this.FechaFilter2 = dtfecha2;
                this.VersionAnexoA = versi;

                model.Total = 1;
                model.Resultado = this.IndexReporte == string.Empty ? "MenuAnexoA" : this.IndexReporte;
            }
            catch
            {
                model.Total = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// Exportacion del Anexo A a archivo Excel
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult GenerarReporteAnexoAxls(string fecha, string url)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            DateTime dtfecha = DateTime.MinValue;
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                dtfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                string nameFile = ConstantesMigraciones.RptAnexoIdcos + "_" + dtfecha.ToString("yyyyMMdd") + ConstantesPR5ReportesServicio.ExtensionExcel;
                servicio.GenerarArchivoExcelMigracionesAnexoA(this.VersionAnexoA, dtfecha, ruta + nameFile);

                model.Resultado = nameFile;
                model.Total = 1;
            }
            catch (Exception ex)
            {
                model.Total = -1;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Exportacion de reporte especifico del Anexo A a archivo Excel
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult GenerarReporteAnexoA(int reporcodi, string fec1, string fec2, string url, string param1)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            DateTime dtfecha1 = DateTime.MinValue, dtfecha2 = DateTime.MinValue;
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                dtfecha1 = DateTime.ParseExact(fec1, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (fec2 != null)
                {
                    dtfecha2 = DateTime.ParseExact(fec2, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                else { dtfecha2 = dtfecha1; }
                string nameFile = ConstantesMigraciones.RptAnexoIdcos + "_" + this.IndexReporte + ConstantesPR5ReportesServicio.ExtensionExcel;
                this.servicio.GenerarArchivoExcelAnexoAByItem("", dtfecha1, dtfecha2, ruta + nameFile, reporcodi, param1);

                model.Resultado = nameFile;
                model.Total = 1;
            }
            catch (Exception ex)
            {
                model.Total = -1;
                model.Mensaje = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerarIEOD()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();
            DateTime finicio = DateTime.Now;

            model.Fecha = finicio.AddDays(-1).ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult CargarGenerarIEOD(string fecha)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            DateTime dFecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            List<SiVersionieodDTO> Lista = this.servicio.ListaVersionByFecha(dFecha, ConstantesAppServicio.MprojcodiIDCOS);
            model.Resultado = this.servicio.GenerarIEODHtml(Lista.OrderBy(x => x.Verscodi).ToList());
            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult SaveGenerarIEOD(string fecha)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            DateTime dFecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            dFecha = new DateTime(dFecha.Year, dFecha.Month, dFecha.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            var regVersion = new SiVersionieodDTO()
            {
                Versfechaversion = DateTime.Now,
                Versfechaperiodo = dFecha.Date,
                Versfeccreacion = DateTime.Now,
                Versusucreacion = User.Identity.Name,
                Mprojcodi = ConstantesAppServicio.MprojcodiIDCOS
            };

            model.Total = this.servicio.SaveGenerarIEOD(regVersion);

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public PartialViewResult ListaVersionesIEOD(string fecha)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            DateTime dFecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            List<SiVersionieodDTO> Lista = this.servicio.ListaVersionByFecha(dFecha, ConstantesAppServicio.MprojcodiIDCOS);

            model.Resultado = "cboVersiones";
            return PartialView(model);
        }

        /// <summary>
        /// Descargar archivo del Anexo A
        /// </summary>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public virtual ActionResult ExportarReporteXls(string nameFile)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nameFile;
            return File(fullPath, ConstantesPR5ReportesServicio.AppExcel, nameFile);
        }
        #endregion

        /// <summary>
        /// 3.13.2.1.	Reporte de Eventos: fallas, interrupciones, restricciones y otros de carácter operativo.
        /// </summary>
        /// <returns></returns>
        #region ReporteEventos
        //
        // GET: /IEOD/AnexoA/IndexReporteEventos
        public ActionResult IndexReporteEventos()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();
            DateTime finicio = DateTime.Now;
            string strEmpresas = string.Empty;
            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter2.ToString(Constantes.FormatoFecha);
            var empresas = servicio.ListarEmpresaTodo().Where(x => x.Emprsein == "S").ToList();
            model.ListaEmpresas = empresas;
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = this.IndexReporte;
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
            List<EventoDTO> lista = new List<EventoDTO>();
            List<EventoDTO> listaVersion = new List<EventoDTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            idEmpresa = string.IsNullOrEmpty(idEmpresa) ? ConstantesAppServicio.ParametroDefecto : idEmpresa;
            idUbicacion = string.IsNullOrEmpty(idUbicacion) ? ConstantesAppServicio.ParametroDefecto : idUbicacion;

            int[] ubicaciones = new int[idUbicacion.Length];
            ubicaciones = idUbicacion.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] empresas = new int[idUbicacion.Length];
            empresas = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 1);

                listaVersion = (List<EventoDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas.Contains((int)x.EMPRCODI) && ubicaciones.Contains((int)x.AREACODI)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 1);
                if (listaBytesNext != null)
                {
                    lista = (List<EventoDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteEventosDataReporte(fechaInicial, fechaFinal.AddDays(1));
                lista = lista.Where(x => empresas.Contains((int)x.EMPRCODI) && ubicaciones.Contains((int)x.AREACODI)).ToList();
            }

            model.Resultado = this.servicio.ReporteEventosHtml(lista, fechaInicial, fechaInicial, listaVersion);

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
        public ActionResult IndexReporteRestriccionesOperativas()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter2.ToString(Constantes.FormatoFecha);

            model.ListaEmpresas = servicio.ObtenerEmpresasGeneradoras();

            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();

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
        public JsonResult CargarListaRestricciones(string idEmpresa, string idUbicacion, string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveManttoDTO> lista = new List<EveManttoDTO>();
            List<EveManttoDTO> listaVersion = new List<EveManttoDTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = fechaFinal.AddDays(1);
            idEmpresa = string.IsNullOrEmpty(idEmpresa) ? ConstantesAppServicio.ParametroDefecto : idEmpresa;
            idUbicacion = string.IsNullOrEmpty(idUbicacion) ? ConstantesAppServicio.ParametroDefecto : idUbicacion;

            int[] ubicaciones = new int[idUbicacion.Length];
            ubicaciones = idUbicacion.Split(',').Select(int.Parse).ToArray();

            if (string.IsNullOrEmpty(idEmpresa)) idEmpresa = "0";
            int[] empresas = new int[idEmpresa.Length];
            empresas = idEmpresa.Split(',').Select(int.Parse).ToArray();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 2);

                listaVersion = (List<EveManttoDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas.Contains(x.Emprcodi) && ubicaciones.Contains(x.Areacodi)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 2);
                if (listaBytesNext != null)
                {
                    lista = (List<EveManttoDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteRestriccionesOperativasDataReporte(fechaInicial, fechaFinal);
                lista = lista.Where(x => empresas.Contains(x.Emprcodi) && ubicaciones.Contains(x.Areacodi)).ToList();
            }

            model.Resultado = this.servicio.ReporteRestriccionesOperativasHtml(lista, fechaInicial, fechaInicial, listaVersion);

            return Json(model);
        }

        /// <summary>
        /// Permite pintar la vista del paginado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string idEmpresa, string idUbicacion, string idEquipo, string fechaInicio, string fechaFin)
        {
            Paginacion model = new Paginacion();
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int pageSize = Constantes.PageSize;
            int nroRegistros = this.servicioRestriciones.ContarEveIeodCuadroxEmpresaxEquipos(fechaInicial, fechaFinal, ConstantesPR5ReportesServicio.SubcausacodiRestric, idEmpresa, idEquipo, idUbicacion);

            int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
            model.NroPaginas = nroPaginas;
            model.NroMostrar = Constantes.NroPageShow;
            model.IndicadorPagina = false;
            if (nroPaginas > 1)
            {
                model.IndicadorPagina = true;
            }
            return base.Paginado(model);
        }
        #endregion

        /// <summary>
        /// 3.13.2.2.1	Reporte de las principales restricciones operativas y mantenimiento de las Unidades de Generación y de los equipos del Sistema de Transmisión (solo ejecutadas).
        /// </summary>
        /// <returns></returns>
        #region ReporteRestriccionesOperativasEjec
        //
        // GET: /IEOD/AnexoA/IndexReporteRestriccionesOperativasEjec
        public ActionResult IndexReporteRestriccionesOperativasEjec()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            DateTime finicio = DateTime.Now;

            string strEmpresas = string.Empty;

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter2.ToString(Constantes.FormatoFecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresaRestriccionesOperativas);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();

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
        public JsonResult CargarListaRestriccionesEjec(string idEmpresa, string idUbicacion, string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveManttoDTO> lista = new List<EveManttoDTO>();
            List<EveManttoDTO> listaVersion = new List<EveManttoDTO>();
            string resultado = "";

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            string tiposMantenimiento = "1", tiposEmpresa = "-1", tiposEquipo = "-1", tiposMantto = "-1", indispo = "-1", interrupcion = "-1";

            if (idUbicacion.Equals(string.Empty)) { idUbicacion = "0"; }
            int[] ubicaciones = new int[idUbicacion.Length];
            ubicaciones = idUbicacion.Split(',').Select(x => int.Parse(x)).ToArray();

            if (idEmpresa.Equals(string.Empty)) { idEmpresa = "0"; }
            int[] empresas = new int[idEmpresa.Length];
            empresas = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 3);

                listaVersion = (List<EveManttoDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas.Contains(x.Emprcodi) && ubicaciones.Contains(x.Areacodi)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 2);
                if (listaBytesNext != null)
                {
                    lista = (List<EveManttoDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicioEvento.GenerarReportesGrafico(tiposMantenimiento, fechaInicial, fechaFinal, indispo, tiposEmpresa, ConstantesAppServicio.ParametroDefecto, tiposEquipo, interrupcion, tiposMantto);
            }

            if (lista.Count > 0)
            {
                lista = lista.Where(x => empresas.Contains(x.Emprcodi) && ubicaciones.Contains(x.Areacodi)).ToList();
            }

            resultado = this.servicio.ReporteRestriccionesOperativasHtml(lista, fechaInicial, fechaInicial, listaVersion);

            model.Resultado = resultado;

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
        public ActionResult IndexDespachoRegistrado()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter2.ToString(Constantes.FormatoFecha);

            var empresas = this.servIEOD.ListarEmpresasxTipoEquipos(ConstantesHorasOperacion.CodFamilias);
            model.ListaEmpresas = empresas;
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();

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
        public JsonResult CargarListaDespachoRegistrado(string idEmpresa, string idCentral, int idPotencia, string fechaIni, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MePtomedicionDTO> listaPto = new List<MePtomedicionDTO>();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();
            string resultado = string.Empty;

            DateTime fechaInicial = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int[] empresas = new int[idEmpresa.Length];
            empresas = idEmpresa.Split(',').Select(int.Parse).ToArray();

            int[] centrales = new int[idCentral.Length];
            centrales = idCentral.Split(',').Select(int.Parse).ToArray();

            int tipoinf = (idPotencia == 1) ? ConstantesPR5ReportesServicio.TipoinfoMW : ConstantesPR5ReportesServicio.TipoinfoMVAR;

            if (this.VersionAnexoA != "")
            {
                decimal rep_ = decimal.Parse((idPotencia == 1) ? "4.1" : "4.2");
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, rep_);

                listaVersion = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaPto = this.servicio.ListarPtoMedicionFromM48(listaVersion);
                listaPto = listaPto.Where(x => empresas.Contains(x.Emprcodi.Value) && centrales.Contains(x.Equipadre)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), rep_);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteDespachoRegistradoDataReporte(tipoinf, fechaInicial, fechaFinal);
                listaPto = this.servicio.ListarPtoMedicionFromM48(lista);

                var listaCentralesData = lista.Select(x => x.Equipadre).ToList();
                var listaNoEncontrada = listaCentralesData.Where(x => !centrales.Contains(x)).Select(x => x).ToList();
                listaPto = listaPto.Where(x => empresas.Contains(x.Emprcodi.Value) && centrales.Contains(x.Equipadre)).ToList();
            }

            resultado = this.servicio.ReporteDespachoRegistradoHtml(lista, listaPto, idPotencia, fechaInicial, fechaFinal, listaVersion);
            model.Resultado = resultado;
            model.Total = lista.Count;

            return Json(model);
        }
        #endregion

        /// <summary>
        /// 3.13.2.5.	Reporte de la demanda por áreas (MW).
        /// </summary>
        /// <returns></returns>
        #region ReporteDemandaPorArea
        //
        // GET: /IEOD/AnexoA/IndexReporteDemandaPorArea
        public ActionResult IndexReporteDemandaPorArea()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = this.IndexReporte;

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
        public JsonResult CargarListaDemandaPorArea(string idEmpresa, string idArea, string idEquipo, string fechaInicio, string fechaFin)
        {
            List<PublicacionIEODModel> Listamodel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int[] areas = new int[idArea.Length];
            areas = idArea.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 5);

                listaVersion = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);

                var listaTemp = new List<MeMedicion48DTO>();
                listaTemp.Add(listaVersion[0]);
                listaTemp.Add(listaVersion[1]);

                var listaVersion_ = listaVersion.Where(x => areas.Contains(x.Ptomedicodi)).ToList();

                listaVersion = new List<MeMedicion48DTO>();
                listaVersion.AddRange(listaTemp);
                listaVersion.AddRange(listaVersion_);

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 5);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteDemandaPorAreaYSubareaDataReporte(ConstantesPR5ReportesServicio.LectDespachoEjecutado, ConstantesPR5ReportesServicio.TipoinfoMW, fechaInicial, fechaFinal);
            }

            if (lista.Count > 0)
            {
                var listaTemp = new List<MeMedicion48DTO>();
                listaTemp.Add(lista[0]);
                listaTemp.Add(lista[1]);

                var lista_ = lista.Where(x => areas.Contains(x.Ptomedicodi)).ToList();

                lista = new List<MeMedicion48DTO>();
                lista.AddRange(listaTemp);
                lista.AddRange(lista_);
            }

            model.Resultado = this.servicio.ReporteDemandaPorAreaYSubareaHtml(lista, fechaInicial, listaVersion, idArea);
            Listamodel.Add(model);
            Listamodel.Add(this.GraficoMantenimientoDemandaPorArea(lista.Where(x => x.Reporcodi == ConstantesPR5ReportesServicio.ReporcodiDemandaAreas).ToList(), listaVersion.Where(x => x.Reporcodi == ConstantesPR5ReportesServicio.ReporcodiDemandaAreas).ToList()));

            return Json(Listamodel);
        }

        /// <summary>
        /// Crear objeto grafico de Demanda x Area
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoMantenimientoDemandaPorArea(List<MeMedicion48DTO> lista, List<MeMedicion48DTO> dataVersion)
        {
            if (dataVersion.Count > 0)
            {
                var temp = lista;
                var tempVersi = dataVersion;

                lista = tempVersi;
                dataVersion = temp;
            }

            MeMedicion48DTO areaNorte = lista.Find(x => x.Ptomedinomb == ConstanteFormulaScada.AreaOperativaAbrevNorte);
            MeMedicion48DTO areaCentro = lista.Find(x => x.Ptomedinomb == ConstanteFormulaScada.AreaOperativaAbrevCentro);
            MeMedicion48DTO areaSur = lista.Find(x => x.Ptomedinomb == ConstanteFormulaScada.AreaOperativaAbrevSur);

            var DatosGraficoGen = servicio.GetByIdReporte((int)lista.FirstOrDefault().Reporcodi);
            var DatosGraficoRep = servicio.GetByCriteriaMeReporteGrafico((int)lista.FirstOrDefault().Reporcodi);

            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.TitleText = DatosGraficoGen.Repornombre;

            //Eje X
            model.Grafico.XAxisCategories = new List<string>();
            DateTime horas = new DateTime(2013, 9, 15, 0, 0, 0);
            for (int h = 1; h <= 48; h++)
            {
                model.Grafico.XAxisCategories.Add(horas.ToString(Constantes.FormatoHoraMinuto));
                horas = horas.AddMinutes(30);
            }

            int columnSerie = 48;
            int cc = 0;
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesData = new decimal?[DatosGraficoRep.Count][];
            foreach (var dat in DatosGraficoRep)
            {
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series[cc].Name = dat.Repgrname;
                model.Grafico.Series[cc].Type = dat.Repgrtype;
                model.Grafico.Series[cc].Color = dat.Repgrcolor;
                model.Grafico.Series[cc].YAxis = (int)dat.Repgryaxis;
                model.Grafico.Series[cc].YAxisTitle = (dat.Repgryaxis != 0 ? "MW ÁREA CENTRO" : "MW ÁREA NORTE Y SUR");

                var d = lista.Find(c => c.Ptomedicodi == dat.Ptomedicodi);
                model.Grafico.SeriesData[cc] = new decimal?[columnSerie];
                for (int h = 1; h <= columnSerie; h++)
                {
                    model.Grafico.SeriesData[cc][h - 1] = (d != null) ? (decimal?)d.GetType().GetProperty("H" + h).GetValue(d, null) : 0;
                }

                cc++;
            }

            return model;
        }
        #endregion

        /// <summary>
        /// 3.13.2.6.	Reporte de Demanda de Grandes Usuarios (MW).
        /// </summary>
        /// <returns></returns>
        #region ReporteDemandaGrandesUsuarios
        //
        // GET: /IEOD/AnexoA/IndexReporteDemandaGrandesUsuarios
        public ActionResult IndexReporteDemandaGrandesUsuarios()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = this.IndexReporte;
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
        public JsonResult CargarListaDemandaGrandesUsuarios(string idEmpresa, string idUbicacion, string idEquipo, string fechaInicio, string fechaFin)
        {
            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            var listaReporte = this.servicio.GetListaReporteUL();
            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 6);

                listaVersion = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 6);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteDemandaGrandesUsuariosDataReporte(fechaInicial, fechaFinal);
            }

            //Reporte
            string resultado = this.servicio.ReporteDemandaGrandesUsuariosHtml(lista, fechaInicial, fechaFinal, idUbicacion, listaReporte, listaVersion);
            model.Resultado = resultado;
            listaModel.Add(model);

            //Grafico 1 - Area norte
            model = new PublicacionIEODModel();
            model = GraficoMantenimientoDemandaGrandesUsuarios(lista, listaVersion, idUbicacion, listaReporte, 1);
            listaModel.Add(model);

            //Grafico 2 - Area centro
            model = new PublicacionIEODModel();
            model = GraficoMantenimientoDemandaGrandesUsuarios(lista, listaVersion, idUbicacion, listaReporte, 2);
            listaModel.Add(model);

            //Grafico 3 - Area centro
            model = new PublicacionIEODModel();
            model = GraficoMantenimientoDemandaGrandesUsuarios(lista, listaVersion, idUbicacion, listaReporte, 3);
            listaModel.Add(model);

            //Grafico 4 - Area centro
            model = new PublicacionIEODModel();
            model = GraficoMantenimientoDemandaGrandesUsuarios(lista, listaVersion, idUbicacion, listaReporte, 4);
            listaModel.Add(model);

            return Json(listaModel);
        }

        /// <summary>
        /// Crear objeto grafico de Demanda de Grandes usuarios
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="idUbicacion"></param>
        /// <param name="listaAreaConst"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoMantenimientoDemandaGrandesUsuarios(List<MeMedicion48DTO> lista, List<MeMedicion48DTO> dataVersion, string idUbicacion, List<MeReporteDTO> listaReporteConst, int tipoGrafico)
        {
            if (dataVersion.Count > 0)
            {
                var temp = lista;
                var tempVersi = dataVersion;

                lista = tempVersi;
                dataVersion = temp;
            }

            lista = lista.OrderBy(x => x.Orden).ToList();
            string titulo = string.Empty;
            switch (tipoGrafico)
            {
                case 1: //area norte
                    titulo = @"GRANDES USUARIOS AREA NORTE";
                    lista = lista.Where(x => x.Reporcodi == ConstantesPR5ReportesServicio.IdReporteDemandaGrandesUsuariosNorte).ToList();
                    break;
                case 2: //area centro 1
                    titulo = @"GRANDES USUARIOS AREA CENTRO (1)";
                    lista = lista.Where(x => x.Reporcodi == ConstantesPR5ReportesServicio.IdReporteDemandaGrandesUsuariosCentro).ToList();
                    if (lista.Count > 0)
                    {
                        int total = lista.Count;
                        lista = lista.GetRange(0, total / 2);
                    }
                    break;
                case 3:
                    titulo = @"GRANDES USUARIOS AREA CENTRO (2)";
                    lista = lista.Where(x => x.Reporcodi == ConstantesPR5ReportesServicio.IdReporteDemandaGrandesUsuariosCentro).ToList();
                    if (lista.Count > 0)
                    {
                        int total = lista.Count;
                        lista = lista.GetRange(total / 2 + 1, total / 2 - 1);
                    }
                    break;
                case 4:
                    titulo = @"GRANDES USUARIOS AREA SUR";
                    lista = lista.Where(x => x.Reporcodi == ConstantesPR5ReportesServicio.IdReporteDemandaGrandesUsuariosSur).ToList();
                    break;
            }

            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.TitleText = titulo;
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            model.Grafico.SerieDataS = new DatosSerie[lista.Count][];

            //Eje X
            model.Grafico.XAxisCategories = new List<string>();
            DateTime horas = new DateTime(2013, 9, 15, 0, 0, 0);
            for (int h = 1; h <= 48; h++)
            {
                model.Grafico.XAxisCategories.Add(horas.ToString(Constantes.FormatoHoraMinuto));
                horas = horas.AddMinutes(30);
            }

            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesData = new decimal?[lista.Count][];
            for (int i = 0; i < lista.Count; i++)
            {
                var pto = lista[i];
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series[i].Name = pto.Ptomedidesc;
                model.Grafico.Series[i].Type = "line";
                //model.Grafico.Series[i].Color = "#FF0000";
                model.Grafico.Series[i].YAxis = 0;

                model.Grafico.SeriesData[i] = new decimal?[48];
                for (int h = 1; h <= 48; h++)
                {
                    model.Grafico.SeriesData[i][h - 1] = (decimal?)pto.GetType().GetProperty("H" + h).GetValue(pto, null);
                }
            }

            return model;
        }
        #endregion

        /// <summary>
        /// 3.13.2.7.	Recursos energéticos y diagrama de duración de demanda del SEIN.
        /// </summary>
        /// <returns></returns>
        #region ReporteRecursosEnergeticosDemandaSEIN
        //
        // GET: /IEOD/AnexoA/IndexReporteRecursosEnergeticosDemandaSEIN
        public ActionResult IndexReporteRecursosEnergeticosDemandaSEIN()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);

            var empresas = this.servIEOD.ListarEmpresasxTipoEquipos(ConstantesHorasOperacion.CodFamilias);
            model.ListaEmpresas = empresas;

            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);

            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = this.IndexReporte;
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
        public JsonResult CargarListaRecursosEnergeticosSEIN(string idempresa, string idtipocentral, string idtiporecurso, string fcdesde, string fchasta, int soloRecursos)
        {
            DateTime fechaInicio = DateTime.ParseExact(fcdesde, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fchasta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model;

            List<MeMedicion48DTO> Lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> lista2 = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> lista3 = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion1 = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion2 = new List<MeMedicion48DTO>();
            string resultado = string.Empty;
            string resultado2 = string.Empty;

            int[] tipocentral = new int[idtipocentral.Length];
            tipocentral = idtipocentral.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] empresas = new int[idempresa.Length];
            empresas = idempresa.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] tiporecurso = new int[idtiporecurso.Length];
            tiporecurso = idtiporecurso.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                decimal rep_ = decimal.Parse((soloRecursos == 1) ? "7.1" : "7.2");

                var listaBytes1 = servicio.GetByVersionDetIEOD(this.VersionAnexoA, rep_ + 0.01m);
                listaVersion1 = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes1.Versdatos);
                var listaBytes2 = servicio.GetByVersionDetIEOD(this.VersionAnexoA, rep_ + 0.02m);
                listaVersion2 = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes2.Versdatos);

                var listaBytesNext1 = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), rep_ + 0.01m);
                if (listaBytesNext1 != null)
                {
                    Lista = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext1.Versdatos);
                }
                lista2 = listaVersion2;
                var listaBytesNext2 = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), rep_ + 0.02m);
                if (listaBytesNext2 != null)
                {
                    lista2 = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext2.Versdatos);
                }
            }
            else
            {
                if (soloRecursos == 1) //Solo Recursos Energéticos RER
                {
                    //Lista = this.servicio.ListaGeneracionElectricaxCentralesRER(fechaInicio, fechaFinal);
                    lista2 = this.servicio.ListarGeneracionElectricaRERXTipoGeneracion(Lista);
                }
                else
                {
                    servicio.ReportePotenciaXTipoRecursoDataReporte(idempresa, idtipocentral, idtiporecurso, fechaInicio, fechaFinal, true, out Lista, out lista3);
                    lista2 = servicio.ListaReportePotenciaXTipoHidro(idempresa, idtipocentral, idtiporecurso, fechaInicio, fechaFinal);
                }
            }

            if (Lista.Count > 0)
            {
                //Lista = Lista.Where(x => empresas.Contains(x.Emprcodi) && tipocentral.Contains(x.Famcodi) && tiporecurso.Contains(x.Fenergcodi)).ToList();
            }

            if (soloRecursos == 1) //Solo Recursos Energéticos RER
            {
                //Reporte
                model = new PublicacionIEODModel();
                model.Resultado = this.servicio.ListaReporteGeneracionElectricaCentralesRERHtml(Lista, listaVersion1);
                listaModel.Add(model);

                //Diagrama de Carga por tipo de Recurso
                model = this.GraficoGeneracionElectricaRERXCentral(Lista);
                listaModel.Add(model);

                //GENERACIÓN ELÉCTRICA RER POR TIPO DE GENERACIÓN EN EL SEIN
                model = this.GraficoGeneracionElectricaRERXTipoGeneracion(lista2);
                listaModel.Add(model);
            }
            else
            {
                //Reporte
                model = new PublicacionIEODModel();
                model.Resultado = this.servicio.ListaReportePotenciaXTipoRecursoHtml(Lista, listaVersion1);
                model.Resultado2 = this.servicio.ListaReportePotenciaXTipoHidroHtml(lista2);
                listaModel.Add(model);

                //Diagrama de Carga por tipo de Recurso
                model = this.GraficoRecursosEnergeticosDiagramaCarga(Lista);
                listaModel.Add(model);

                //Participacion por tipo de Recurso
                model = this.GraficoRecursosEnergeticosParticipacionRecurso(Lista);
                listaModel.Add(model);
            }

            return Json(listaModel);
        }

        /// <summary>
        /// Grafico de Potencia x Tipo de Recurso
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private PublicacionIEODModel GraficoRecursosEnergeticosDiagramaCarga(List<MeMedicion48DTO> listaReporteInput)
        {
            List<MeMedicion48DTO> listaReporte = listaReporteInput.OrderByDescending(x => x.Orden).ToList();
            decimal? valor;
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            PublicacionIEODModel model = new PublicacionIEODModel();
            //GraficoWeb grafico = new GraficoWeb();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            model.Grafico.SerieDataS = new DatosSerie[listaReporte.Count][];

            DateTime horas = new DateTime(2013, 9, 15, 0, 0, 0);
            for (int i = 0; i < 48; i++)
            {
                horas = horas.AddMinutes(30);
                model.Grafico.SeriesName.Add(string.Format("{0:H:mm}", horas));
            }

            for (int i = 0; i < listaReporte.Count; i++)
            {
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = listaReporte[i].Ctgdetnomb;
                regSerie.Type = "area";
                regSerie.Color = listaReporte[i].Fenercolor;
                List<DatosSerie> listadata = new List<DatosSerie>();
                for (int j = 1; j <= 48; j++)
                {
                    valor = (decimal?)listaReporte[i].GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(listaReporte[i], null);
                    listadata.Add(new DatosSerie() { Y = valor });
                }
                regSerie.Data = listadata;
                model.Grafico.Series.Add(regSerie);
            }

            model.Grafico.TitleText = " DIAGRAMA DE CARGA POR TIPO DE RECURSO";
            if (listaReporte.Count > 0)
            {
                model.Grafico.YaxixTitle = "MW";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
            }
            return model;
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
                var porcentaje = (total > 0 ? reg.Acumulado / total * 100 : 0);
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
            model.Grafico.Subtitle = "TOTAL RER = " + Decimal.Round(total, 3);
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
        public ActionResult IndexReporteProduccionEnergiaDiaria()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            DateTime finicio = this.FechaFilter1;//DateTime.Now;

            model.FechaInicio = finicio.ToString(Constantes.FormatoFecha);
            model.FechaFin = finicio.ToString(Constantes.FormatoFecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = this.IndexReporte;
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
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> listaData = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int[] empresas = new int[idEmpresa.Length];
            empresas = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] centrales = new int[idCentral.Length];
            centrales = idCentral.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] tipgeneracion = new int[idGeneracion.Length];
            tipgeneracion = idGeneracion.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 8);

                listaVersion = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas.Contains(x.Emprcodi) && centrales.Contains(x.Equipadre) && tipgeneracion.Contains(x.Tgenercodi)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 8);
                if (listaBytesNext != null)
                {
                    listaData = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                listaData = this.servicio.ReporteProduccionEnergiaDiariaDataReporte(idEmpresa, idCentral, fechaInicial, fechaFinal);
                if (listaData.Count == 0) { listaData = this.servicio.ReporteProduccionEnergiaDiariaDataReporte(idEmpresa, idCentral, fechaInicial, fechaFinal); }
            }

            if (listaData.Count > 0 && this.VersionAnexoA != "")
            {
                listaData = listaData.Where(x => empresas.Contains(x.Emprcodi) && centrales.Contains(x.Equipadre) && tipgeneracion.Contains(x.Tgenercodi)).ToList();
            }

            var listaI = listaData.Where(x => x.Emprcoes == ConstantesPR5ReportesServicio.EmpresacoesSi && tipgeneracion.Contains(x.Tgenercodi)).ToList();
            var listaNI = listaData.Where(x => x.Emprcoes == ConstantesPR5ReportesServicio.EmpresacoesNo && tipgeneracion.Contains(x.Tgenercodi)).ToList();

            string resultado = this.servicio.ReporteProduccionEnergiaDiariaHtml(ConstantesPR5ReportesServicio.EmpresacoesSi, listaI, fechaInicial, fechaFinal, listaVersion.Where(x => x.Emprcoes == ConstantesPR5ReportesServicio.EmpresacoesSi).ToList());
            string resultado2 = this.servicio.ReporteProduccionEnergiaDiariaHtml(ConstantesPR5ReportesServicio.EmpresacoesNo, listaNI, fechaInicial, fechaFinal, listaVersion.Where(x => x.Emprcoes == ConstantesPR5ReportesServicio.EmpresacoesNo).ToList());
            model.Resultado = resultado;
            model.Resultado2 = resultado2;

            return Json(model);
        }

        /// <summary>
        /// Json para gráfico de Producción de Energía diaria
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="idGeneracion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarGraficoProduccionEnergiaDiaria(string idEmpresa, string idCentral, string idGeneracion, string fechaInicio, string fechaFin)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();

            int[] empresas = new int[idEmpresa.Length];
            empresas = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] centrales = new int[idCentral.Length];
            centrales = idCentral.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] tipgeneracion = new int[idGeneracion.Length];
            tipgeneracion = idGeneracion.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 8);

                listaVersion = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas.Contains(x.Emprcodi) && centrales.Contains(x.Equicodi) && tipgeneracion.Contains(x.Tgenercodi)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 8);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteProduccionEnergiaDiariaDataReporte(idEmpresa, idCentral, fechaInicial, fechaFinal)
                    .Where(x => x.Emprcoes == ConstantesPR5ReportesServicio.EmpresacoesSi && idGeneracion.Contains(x.Tgenercodi.ToString())).ToList();

                if (lista.Count == 0)
                {
                    lista = this.servicio.ReporteProduccionEnergiaDiariaDataReporte(idEmpresa, idCentral, fechaInicial, fechaFinal)
                        .Where(x => x.Emprcoes == ConstantesPR5ReportesServicio.EmpresacoesSi && idGeneracion.Contains(x.Tgenercodi.ToString())).ToList();
                }
            }

            if (lista.Count > 0 && this.VersionAnexoA != "")
            {
                lista = lista.Where(x => empresas.Contains(x.Emprcodi) && centrales.Contains(x.Equicodi) && tipgeneracion.Contains(x.Tgenercodi)).ToList();
            }

            lista = lista.Where(x => x.Emprcoes == ConstantesPR5ReportesServicio.EmpresacoesSi && tipgeneracion.Contains(x.Tgenercodi)).ToList();

            if (listaVersion.Count > 0)
            {
                var temp = lista;
                var tempVersi = listaVersion;

                lista = tempVersi;
                listaVersion = temp;
            }

            var totalesxempresa = lista.GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(x => new MeMedicion48DTO
            {
                Meditotal = x.Sum(p => p.Meditotal),
                Emprnomb = x.Key.Emprnomb,
                Emprcodi = x.Key.Emprcodi

            }).OrderByDescending(x => x.Meditotal).ToList();

            PublicacionIEODModel model = GraficoProduccionEnergiaDiaria(totalesxempresa);

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Grafico ProduccionEnergia Diaria
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoProduccionEnergiaDiaria(List<MeMedicion48DTO> lista)
        {
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
                regSerie.YAxisTitle = "Programado y Ejecutado";
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
        public ActionResult IndexReporteGeneracionDelSEIN()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = this.IndexReporte;

            return View(model);
        }

        /// <summary>
        /// Interfaz web de RESUMEN DE GENERACIÓN POR ÁREAS DEL SEIN
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaGeneracionDelSEIN(string fechaInicio, string fechaFin)
        {
            List<PublicacionIEODModel> Listamodel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 9);

                listaVersion = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 9);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteMaxGeneracionInstSEINDataReporte(fechaInicial, fechaFinal);
            }
            string resultado = this.servicio.ReporteMaxGeneracionInstSEINHtml(lista, fechaInicial, fechaFinal, listaVersion);

            model.Resultado = resultado;
            Listamodel.Add(model);

            Listamodel.Add(this.GraficoGeneracionSEIN(lista, 1));
            Listamodel.Add(this.GraficoGeneracionSEIN(lista, 2));
            Listamodel.Add(this.GraficoGeneracionSEIN(lista, 3));

            return Json(Listamodel);
        }

        /// <summary>
        /// Gráfico RESUMEN DE GENERACIÓN POR ÁREAS DEL SEIN
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoReporte"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarGraficoGeneracionSein(string fechaInicio, string fechaFin, int tipoReporte)
        {
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();

            List<MeMedicion48DTO> lista = this.servicio.ReporteMaxGeneracionInstSEINDataReporte(fechaInicial, fechaFinal);
            model = this.GraficoGeneracionSEIN(lista, tipoReporte);

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
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
                model.Grafico.XAxisCategories.Add(horas.ToString(Constantes.FormatoHoraMinuto));
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
                    model.Grafico.SeriesData[i][h - 1] = (decimal?)pto.GetType().GetProperty("H" + h).GetValue(pto, null);
                }
            }

            return model;
        }
        #endregion

        /// <summary>
        /// 6. Restricción de suministros: magnitud y causas.
        /// </summary>
        /// <returns></returns>
        #region ReporteRestriccionSuministro

        public ActionResult IndexReporteRestriccionSuministros()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();
            string strEmpresas = string.Empty;
            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter2.ToString(Constantes.FormatoFecha);
            var empresas = servicio.ListarEmpresaTodo().Where(x => x.Emprsein == "S").ToList();
            model.ListaEmpresas = empresas;
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = this.IndexReporte;
            return View(model);

        }

        public JsonResult CargarListaRestriccionesSuministros(string idEmpresa, string idUbicacion)
        {

            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EventoDTO> lista = new List<EventoDTO>();
            List<EventoDTO> listaVersion = new List<EventoDTO>();

            DateTime fechaInicial = FechaFilter1;
            DateTime fechaFinal = FechaFilter2;

            idEmpresa = string.IsNullOrEmpty(idEmpresa) ? ConstantesAppServicio.ParametroDefecto : idEmpresa;
            idUbicacion = string.IsNullOrEmpty(idUbicacion) ? ConstantesAppServicio.ParametroDefecto : idUbicacion;

            var empresas = idEmpresa.Split(',').Select(int.Parse).ToArray();
            var ubicaciones = idUbicacion.Split(',').Select(int.Parse).ToArray();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 1);

                listaVersion = (List<EventoDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas.Contains((int)x.EMPRCODI) && ubicaciones.Contains((int)x.AREACODI)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 1);
                if (listaBytesNext != null)
                {
                    lista = (List<EventoDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteEventosDataReporte(fechaInicial, fechaFinal, esSuministro: "SI");
                lista = lista.Where(x => empresas.Contains((int)x.EMPRCODI) && ubicaciones.Contains((int)x.AREACODI) && x.Interrmanualr == "S" && x.Interrracmf == "S").ToList();
            }

            model.Resultado = this.servicio.ReporteEventosHtml(lista, fechaInicial, fechaInicial, listaVersion);

            return Json(model);


        }
        #endregion

        /// <summary>
        /// 3.13.2.10.	Horas de orden de arranque y parada, así como las horas de ingreso y salida de las Unidades de Generación del SEIN.
        /// </summary>
        /// <returns></returns>
        #region ReporteHorasOrdenAPISGeneracionSEIN
        //
        // GET: /IEOD/AnexoA/IndexReporteHorasOrdenAPIS
        public ActionResult IndexReporteHorasOrdenAPIS()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);

            model.FechaFin = FechaFilter2.ToString(Constantes.FormatoFecha);
            model.Anho = DateTime.Now.Year.ToString();

            model.ListaTipoOperacion = this.servicio.ListarTipoOperacionHO();
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();

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
        public JsonResult CargarHorasOrdenAPISGeneracionSEIN(string idEmpresa, string fechaInicio, string fechaFin, string modoOpe, string idTCentral, string idTCombustible
            , string idSistemaAv, string idOtraClasificacion)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveHoraoperacionDTO> lista = new List<EveHoraoperacionDTO>();
            List<EveHoraoperacionDTO> listaVersion = new List<EveHoraoperacionDTO>();
            string resultado = " ";

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = fechaFinal.AddDays(1);

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 10);

                listaVersion = (List<EveHoraoperacionDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = this.servicio.ReporteHorasOrdenAPISDataReporte(listaVersion, idEmpresa, modoOpe, ConstantesHorasOperacion.ParamTipoOperacionTodos, idTCentral, idTCombustible, idSistemaAv, idOtraClasificacion);

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 10);
                if (listaBytesNext != null)
                {
                    lista = (List<EveHoraoperacionDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.GetListaHorasOrdenAPIS(fechaInicial, fechaFinal);
                lista = this.servicio.ReporteHorasOrdenAPISDataReporte(lista, idEmpresa, modoOpe, ConstantesHorasOperacion.ParamTipoOperacionTodos, idTCentral, idTCombustible, idSistemaAv, idOtraClasificacion);
            }

            resultado = this.servicio.ReporteHorasOrdenAPISHtml(lista, fechaInicial, fechaFinal, listaVersion);

            model.Resultado = resultado;

            return Json(model);
        }

        #endregion

        /// <sumary>
        /// 3.13.2.14.	Volúmenes horarios y caudales horarios de descarga de los embalses asociados a las Centrales Hidroeléctricas.
        /// </sumary>
        /// <returns></returns>
        #region HorariosCaudalVolumenCentralHidroelectrica
        //
        // GET: /IEOD/AnexoA/IndexReporteHorariosCaudalVolumenCentralHidroelectrica
        public ActionResult IndexReporteHorariosCaudalVolumenCentralHidroelectrica()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();
            DateTime finicio = DateTime.Now;
            string strEmpresas = string.Empty;
            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);
            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;
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
            List<MeReporptomedDTO> listaCabecera = new List<MeReporptomedDTO>();
            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> listaVersion = new List<MeMedicion24DTO>();

            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            string resultado = "";
            fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int[] empresas_ = new int[idEmpresa.Length];
            empresas_ = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 14.1m);
                listaVersion = (List<MeMedicion24DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas_.Contains(x.Emprcodi)).ToList();

                var listaBytes2 = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 14.2m);
                listaCabecera = (List<MeReporptomedDTO>)DeserializandoReporteAnexoA(listaBytes2.Versdatos);
                listaCabecera = listaCabecera.Where(x => empresas_.Contains(x.Emprcodi)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 14.1m);
                if (listaBytesNext != null)
                {
                    lista24 = (List<MeMedicion24DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                this.servicio.ReporteCaudalesCentralHidroelectricaDataReporte(ConstantesPR5ReportesServicio.IdReporteVolumenHorario, fechaInicial, fechaFinal, out listaCabecera, out lista24);
            }

            if (lista24.Count > 0)
            {
                listaCabecera = listaCabecera.Where(x => empresas_.Contains(x.Emprcodi)).ToList();
            }

            resultado = this.servicio.ReporteCaudalesCentralHidroelectricaHtml(lista24, listaCabecera, fechaInicial, fechaInicial, listaVersion);
            model.Resultado = resultado;


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
            string resultado = "";

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int[] empresas_ = new int[idEmpresa.Length];
            empresas_ = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 14.3m);

                listaVersion = (List<MeMedicionxintervaloDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas_.Contains(x.Emprcodi)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 14.3m);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicionxintervaloDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.logic.ListaMedIntervaloDescargaVertPag(ConstantesPR5ReportesServicio.IdFormatoDescarga, idEmpresa, fechaInicial, fechaFinal, 1, 1000);
            }

            if (lista.Count > 0)
            {
                lista = lista.Where(x => empresas_.Contains(x.Emprcodi)).ToList();
            }

            resultado = this.servicio.ReporteVertimientosPeriodoVolumenHtml(lista, fechaInicial, fechaInicial, listaVersion);

            model.Resultado = resultado;

            return model;
        }

        #endregion

        ///<sumary>
        ///3.13.2.15.	Vertimientos en los embalses y/o presas en período y volumen.
        /// </sumary>
        /// <returns></returns>
        #region VertimientosPeriodoVolumen
        //
        // GET: /IEOD/AnexoA/IndexReporteVertimientosPeriodoVolumen
        public ActionResult IndexReporteVertimientosPeriodoVolumen()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();
            DateTime finicio = DateTime.Now;
            string strEmpresas = string.Empty;
            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);
            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;
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
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            List<MeMedicionxintervaloDTO> listaVersion = new List<MeMedicionxintervaloDTO>();
            string resultado = "";

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            if (string.IsNullOrEmpty(idEmpresa)) idEmpresa = "0";
            int[] empresas_ = new int[idEmpresa.Length];
            empresas_ = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();



            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 15);

                listaVersion = (List<MeMedicionxintervaloDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas_.Contains(x.Emprcodi)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 15);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicionxintervaloDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.logic.ListaMedIntervaloDescargaVertPag(ConstantesPR5ReportesServicio.IdFormatoVertimiento, idEmpresa, fechaInicial, fechaFinal, 1, 1000);
            }

            if (lista.Count > 0)
            {
                lista = lista.Where(x => empresas_.Contains(x.Emprcodi)).ToList();
            }

            resultado = this.servicio.ReporteVertimientosPeriodoVolumenHtml(lista, fechaInicial, fechaInicial, listaVersion);

            model.Resultado = resultado;

            return Json(model);
        }

        #endregion

        ///<sumary>
        ///3.13.2.16.	Volúmenes o cantidad de combustible almacenado a las 24:00 h de las Centrales Térmicas.
        /// </sumary>
        /// <returns></returns>
        #region DisponibilidadGas
        //
        // GET: /IEOD/AnexoA/IndexDisponibilidadGas 
        public ActionResult IndexReporteDisponibilidadGas()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            var model = new BusquedaIEODModel();
            var servicioStockComb = new StockCombustiblesAppServicio();

            model.FechaInicio = FechaFilter1.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter2.ToString(Constantes.FormatoFecha);

            var empresas = this.servIEOD.ListarEmpresasxTipoEquipos(ConstantesHorasOperacion.CodFamilias);
            model.ListaEmpresas = empresas;


            //model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);

            model.ListaTipoAgente = this.ObtenerListaTipoAgente();
            model.ListaCentralIntegrante = this.ObtenerCentralIntegrante();
            model.ListaYacimientos = servicioStockComb.ListEqCategoriaDetalleByCategoria(ConstantesIntranet.YacimientoGasCodi);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = this.IndexReporte;
            return View(model);
        }


        public PartialViewResult CargarAgentes(string idTipoAgente)
        {
            var model = new BusquedaIEODModel();
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            if (idTipoAgente != "-1")
            {
                entitys = servFormato.GetListaEmpresaFormato(ConstantesStockCombustibles.IdFormatoConsumo);
                List<string> tipoAgente = new List<string>();
                tipoAgente = idTipoAgente.Split(',').ToList();

                entitys = entitys.Where(x => tipoAgente.Contains(x.Emprcoes)).ToList();
            }
            else
            {
                //entitys = servicio.ListEqEquipoEmpresaGEN("-1").ToList();
            }
            model.ListaEmpresas = entitys;
            return PartialView(model);
        }

        // <summary>
        /// Devuelve lista parcial para el Listado de Disponibilidad de Gas
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaDisponibilidadGas(string idsTipoAgente, string idsCentralInt, string idsYacimientos, string idsAgente)
        {
            var servicioStockComb = new StockCombustiblesAppServicio();

            var model = new BusquedaIEODModel();
            var listaReporte = new List<MeMedicionxintervaloDTO>();
            string strCentralInt = servicioStockComb.GeneraCodCentralIntegrante(idsCentralInt);
            DateTime fechaInicial = FechaFilter1;
            DateTime fechaFinal = FechaFilter1;


            listaReporte = servicioStockComb.ListaMedxIntervDisponibilidad(ConstantesStockCombustibles.LectCodiDisponibilidad, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, strCentralInt, ConstantesIntranet.YacimientoGasCodi, idsYacimientos);
            this.ListaFechas = listaReporte.Select(x => x.Medintfechaini).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaInicial = ListaFechas[0];
                fechaFinal = ListaFechas[ListaFechas.Count - 1];
            }

            string resultado = servicioStockComb.GeneraViewReporteDisponibilidadGas(listaReporte, ListaFechas);
            model.Resultado = resultado;
            return PartialView(model);
        }


        #endregion

        #region METODOS DE REPORTE QUEMA DE GAS
        /// <summary>
        /// Permite listar el historico de quema de Gas gas natural de las centrales termoeléctricas
        /// </summary>
        /// <returns></returns> 
        public ActionResult IndexReporteQuemaGasNoEmpleado()
        {
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();

            var model = new BusquedaIEODModel()
            {
                FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha),
                FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha),
                ListaTipoAgente = this.ObtenerListaTipoAgente(),
                ListaCentralIntegrante = this.ObtenerCentralIntegrante(),
                NroReporte = IndexReporte
            };
            return View(model);
        }
        /// <summary>
        /// Devuelve lista parcial para el Listado de Quema de Gas
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaQuemaGas(string idsTipoAgente, string idsCentralInt, string idsAgente)
        {
            var servicioStockComb = new StockCombustiblesAppServicio();
            var model = new BusquedaIEODModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            string strCentralInt = servicioStockComb.GeneraCodCentralIntegrante(idsCentralInt);
            DateTime fechaInicial = FechaFilter1;
            DateTime fechaFinal = FechaFilter1;

            listaReporte = servicioStockComb.ListaMedxIntervQuema(ConstantesStockCombustibles.LectCodiQuemaGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal.AddDays(1), strCentralInt);
            this.ListaFechas = listaReporte.Select(x => x.Medintfechaini).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaInicial = ListaFechas[0];
                fechaFinal = ListaFechas[ListaFechas.Count - 1];
            }

            string resultado = servicioStockComb.GeneraViewReporteQuemaGas(listaReporte, ListaFechas);
            model.Resultado = resultado;
            return PartialView(model);
        }

        #endregion

        ///<sumary>
        ///3.13.2.17.	Volúmenes o cantidad diaria de combustible consumido (asociado a la generación) por cada Unidad de Generación termoeléctrica.
        /// </sumary>
        /// <returns></returns>
        #region CombustibleConsumidoUnidadTermoelectrica
        //
        // GET: /IEOD/AnexoA/IndexReporteCombustibleConsumidoUnidadTermoelectrica
        public ActionResult IndexReporteCombustibleConsumidoUnidadTermoelectrica()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();
            string strEmpresas = string.Empty;
            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);
            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);

            var centrales = servicio.ListEqEquipoEmpresaGEN("-1").ToList();
            model.ListaCentrales = centrales;
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;
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
            //string strCentralInt = this.servicioConsumo.GeneraCodCentralIntegrante("1");
            string idsEstado = "1,2,3";
            // fte energia codi = tipoCombustible 
            string resultado = " ";

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int[] empresas_ = new int[idEmpresa.Length];
            empresas_ = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] centrales = new int[idCentral.Length];
            centrales = idCentral.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] tipocombus = new int[tipoCombustible.Length];
            tipocombus = tipoCombustible.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 17);

                listaVersion = (List<MeMedicionxintervaloDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => tipocombus.Contains(x.Fenergcodi) && empresas_.Contains(x.Emprcodi) && centrales.Contains(x.Equipadre)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 17);
                if (listaBytesNext != null)
                {
                    listaReporteConsumo = (List<MeMedicionxintervaloDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                listaReporteConsumo = this.servicioConsumo.ListaMedxIntervConsumo(ConstantesStockCombustibles.LectCodiConsumo, ConstantesStockCombustibles.Origlectcodi, idEmpresa, fechaInicial, fechaFinal, idsEstado, null, "-1")
                    .Where(x => tipocombus.Contains(x.Fenergcodi) && empresas_.Contains(x.Emprcodi) && centrales.Contains(x.Equipadre)).ToList();
            }

            if (listaReporteConsumo.Count > 0)
            {
                listaReporteConsumo = listaReporteConsumo.Where(x => tipocombus.Contains(x.Fenergcodi) && empresas_.Contains(x.Emprcodi) && centrales.Contains(x.Equipadre)).ToList();
            }

            resultado = this.servicio.ReporteCombustibleConsumidoUnidadTermoelectricaHtml(listaReporteConsumo, fechaInicial, fechaFinal, listaVersion);

            model.Resultado = resultado;

            return Json(model);
        }
        #endregion

        ///<sumary>
        ///3.13.2.18.	Volúmenes diarios de gas natural consumido (asociado a la generación) y presión horaria del gas natural al ingreso (en el lado de alta presión) de cada Unidad de Generación termoeléctrica a gas natural.
        /// </sumary>
        /// <returns></returns>
        #region ConsumoYPresionDiarioUnidadTermoelectrica
        //
        // GET: /IEOD/AnexoA/IndexReporteConsumoYPresionDiarioUnidadTermoelectrica
        public ActionResult IndexReporteConsumoYPresionDiarioUnidadTermoelectrica()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);
            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);
            var centrales = servicio.ListEqEquipoEmpresaGEN("-1").ToList();
            model.ListaCentrales = centrales;
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;

            return View(model);
        }

        /// <summary>
        /// Metodo de carga disenio de reporte de vista CargarListaConsumoYPresionDiarioUnidadTermoelectrica
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaConsumoDiarioUnidadTermoelectrica(string idEmpresa, string idCentral, string idRecurso, string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            List<MeMedicionxintervaloDTO> listaVersion = new List<MeMedicionxintervaloDTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int[] empresas_ = new int[idEmpresa.Length];
            empresas_ = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] centrales = new int[idCentral.Length];
            centrales = idCentral.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                decimal rep_ = decimal.Parse("18.1");

                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, rep_);

                listaVersion = (List<MeMedicionxintervaloDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas_.Contains(x.Emprcodi) && centrales.Contains(x.Equipadre)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), rep_);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicionxintervaloDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteConsumoDiarioUnidadTermoelectricaDataReporte(fechaInicial, fechaFinal, idEmpresa, idCentral, "-1");
                lista = lista.Where(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas).ToList();
            }

            if (lista.Count > 0)
            {
                lista = lista.Where(x => empresas_.Contains(x.Emprcodi) && centrales.Contains(x.Equipadre)).ToList();
            }

            model.Resultado = this.servicio.ReporteConsumoDiarioUnidadTermoelectricaHtml(lista, listaVersion);

            return Json(model);
        }

        /// <summary>
        /// Metodo de carga disenio de reporte de vista CargarListaConsumoYPresionDiarioUnidadTermoelectrica
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaPresionDiarioUnidadTermoelectrica(string idEmpresa, string idCentral, string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> listaVersion = new List<MeMedicion24DTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int[] empresas_ = new int[idEmpresa.Length];
            empresas_ = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] centrales = new int[idCentral.Length];
            centrales = idCentral.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                decimal rep_ = decimal.Parse("18.2");

                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, rep_);

                listaVersion = (List<MeMedicion24DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas_.Contains(x.Emprcodi) && centrales.Contains(x.Equipadre)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), rep_);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicion24DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReportePresionDiarioUnidadTermoelectricaDataReporte(fechaInicial, fechaFinal, idEmpresa, idCentral);
            }

            if (lista.Count > 0)
            {
                lista = lista.Where(x => empresas_.Contains(x.Emprcodi) && centrales.Contains(x.Equipadre)).ToList();
            }

            model.Resultado = this.servicio.ReportePresionDiarioUnidadTermoelectricaHtml(lista, fechaInicial, listaVersion);

            return Json(model);
        }
        #endregion

        ///<sumary>
        ///3.13.2.19.	Reporte cada 30 minutos de la fuente de energía primaria de las unidades RER solar, geotérmica y biomasa. En caso de las Centrales Eólicas, la velocidad del viento registrada cada 30 minutos.
        /// </sumary>
        /// <returns></returns>
        #region RegistroEnergia30Unidades
        //
        // GET: /IEOD/AnexoA/IndexReporteRegistoEnergia30Unidades
        public ActionResult IndexReporteRegistroEnergiaPrimaria30Unidades()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();
            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;

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
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();

            int[] empresas_ = new int[idEmpresa.Length];
            empresas_ = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            List<MePtomedicionDTO> listaPto = this.servicio.ListarPtoEnergiaPrimaria(fechaInicial, fechaFinal);
            listaPto = listaPto.Where(x => empresas_.Contains(x.Emprcodi.Value)).ToList();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 19);
                listaVersion = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 19);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteRegistroEnergia30UnidadesDataReporte(fechaInicial, fechaFinal);
            }

            model.Resultado = this.servicio.ReporteRegistroEnergia30UnidadesHtml(lista, listaPto, fechaInicial, fechaInicial, listaVersion);

            return Json(model);
        }
        #endregion

        ///<sumary>
        ///3.13.2.20.	En caso sea una Central de Cogeneración Calificada, deberá remitir información sobre la producción del Calor Útil de sus Unidades de Generación o el Calor Útil recibido del proceso industrial asociado, en MW
        /// </sumary>
        /// <returns></returns>
        #region CalorUtilGeneracionProceso
        //
        // GET: /IEOD/AnexoA/IndexReporteCalorUtilGeneracionProceso
        public ActionResult IndexReporteCalorUtilGeneracionProceso()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);

            var empresas = this.servicio.ListarEmpresaCoGeneracion(FechaFilter1);
            model.ListaEmpresas = empresas;
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;

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
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int[] empresas_ = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            List<MePtomedicionDTO> listaPto = this.servicio.ListarPtoCalorUtil(fechaInicial, fechaFinal);
            listaPto = listaPto.Where(x => empresas_.Contains(x.Emprcodi.Value)).ToList();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 20);

                listaVersion = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas_.Contains(x.Emprcodi)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 20);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteCalorUtilGeneracionProcesoDataReporte(fechaInicial, fechaFinal);
            }

            model.Resultado = this.servicio.ReporteCalorUtilGeneracionProcesoHtml(lista, listaPto, fechaInicial, fechaInicial, listaVersion);

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
        #region ReportePALineasTransmicion
        //
        // GET: /IEOD/AnexoA/IndexReportePALineasTransmision
        public ActionResult IndexReportePALineasTransmision()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);

            var empresas = servicio.ListaEmpresasByFormato(ConstantesPR5ReportesServicio.IdFormatoFlujoPotencia);
            model.ListaEmpresas = empresas;
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;

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
        public JsonResult CargarListaPALineasTransmision(string fechaIni, string fechaFin, string idEmpresa, string idSubEstacion)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();
            string resultado = string.Empty;

            DateTime fechaInicial = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int[] empresas_ = new int[idEmpresa.Length];
            empresas_ = idEmpresa.Split(',').Select(x => int.Parse(x)).ToArray();

            int[] areas = new int[idSubEstacion.Length];
            areas = idSubEstacion.Split(',').Select(x => int.Parse(x)).ToArray();

            if (idEmpresa.Equals(string.Empty)) { idEmpresa = "0"; }
            if (idSubEstacion.Equals(string.Empty)) { idSubEstacion = "0"; }

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 21);

                listaVersion = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas_.Contains(x.Emprcodi)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 21);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteFlujoPotenciaActivaTransmisionSEINDataReporte(fechaInicial, fechaFinal, ConstantesPR5ReportesServicio.LectcodiFlujoPotencia, ConstantesPR5ReportesServicio.TipoinfoMW, idEmpresa, idSubEstacion);
                lista = lista.Where(x => empresas_.Contains(x.Emprcodi)).ToList();
            }

            resultado = this.servicio.ReporteFlujoPotenciaActivaTransmisionSEINHtml(lista, idEmpresa, idSubEstacion, listaVersion);

            model.Total = lista.Count;
            model.Resultado = resultado;

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
        public ActionResult IndexReporteLineasDesconectadasPorTension()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter2.ToString(Constantes.FormatoFecha);

            var empresas = servicio.GetListaCriteria("1,2,3");
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de lineas Desconectadas Por Tension
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaDesconectadasPorTension(string fechaInicio, string fechaFin, string empresa, string area)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveIeodcuadroDTO> lista = new List<EveIeodcuadroDTO>();
            List<EveIeodcuadroDTO> listaVersion = new List<EveIeodcuadroDTO>();
            string resultado = string.Empty;

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            string listaClase = "1";
            string listaSubcausa = ConstantesPR5ReportesServicio.IdSubCausaPorTension;//"203";

            int[] empresas_ = new int[empresa.Length];
            empresas_ = empresa.Split(',').Select(x => int.Parse(x)).ToArray();

            string[] areas = new string[area.Length];
            areas = area.Split(',').Select(x => x).ToArray();

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 25);

                listaVersion = (List<EveIeodcuadroDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => empresas_.Contains((int)x.Emprcodi)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 25);
                if (listaBytesNext != null)
                {
                    lista = (List<EveIeodcuadroDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = servicio.ReporteSistemasAisladosTemporalesDataReporte(fechaInicial, fechaFinal, listaClase, listaSubcausa);
                lista = lista.Where(x => empresas_.Contains((int)x.Emprcodi)).ToList();
            }

            resultado = this.servicio.ReporteLineasDesconectadasPorTensionHtml(lista, listaVersion);

            model.Resultado = resultado;

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
        public ActionResult IndexReporteSistemasAisladosTemporales()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter2.ToString(Constantes.FormatoFecha);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();

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
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveIeodcuadroDTO> lista = new List<EveIeodcuadroDTO>();
            List<EveIeodcuadroDTO> listaVersion = new List<EveIeodcuadroDTO>();
            string resultado = string.Empty;

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            string listaClase = "1,2,3,4,5,6";
            string listaSubcausa = ConstantesPR5ReportesServicio.IdSubCausaSistemasAislados;//"206";

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 26);

                listaVersion = (List<EveIeodcuadroDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 26);
                if (listaBytesNext != null)
                {
                    lista = (List<EveIeodcuadroDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = servicio.ReporteSistemasAisladosTemporalesDataReporte(fechaInicial, fechaFinal, listaClase, listaSubcausa);
            }

            resultado = this.servicio.ReporteSistemasAisladosTemporalesHtml(lista, listaVersion);

            model.Resultado = resultado;

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
        public ActionResult IndexReporteVariacionesSostenidasSubitas()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;

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
        public JsonResult CargarListaVariacionesSostenidasSubitas(string empresas, string gps, string fechaInicio)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<FIndicadorDTO> lista = new List<FIndicadorDTO>();
            List<FIndicadorDTO> listaVersion = new List<FIndicadorDTO>();

            empresas = "-1"; //modificar
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            string url = Url.Content("~/");

            if (string.IsNullOrEmpty(gps)) gps = "0";
            int[] _gps = new int[gps.Length];
            _gps = gps.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                decimal rep_ = decimal.Parse("27.1");
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, rep_);

                listaVersion = (List<FIndicadorDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => _gps.Contains(x.Gps)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), rep_);
                if (listaBytesNext != null)
                {
                    lista = (List<FIndicadorDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.ReporteVariacionesFrecuenciaSEINDataReporte(empresas, gps, fechaInicial, fechaInicial);
            }

            if (lista.Count > 0 && this.VersionAnexoA != "")
            {
                lista = lista.Where(x => _gps.Contains(x.Gps)).ToList();
            }

            model.Resultado = this.servicio.ReporteVariacionesFrecuenciaSEINHtml(lista, url, listaVersion);
            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="gps"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoVariacionesSostenidasSubitas(string empresas, string gps, string fechaInicio)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<FLecturaDTO> lista = new List<FLecturaDTO>();
            List<FLecturaDTO> listaVersion = new List<FLecturaDTO>();
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            empresas = "-1"; //modificar

            int[] _gps = new int[gps.Length];
            _gps = gps.Split(',').Select(x => int.Parse(x)).ToArray();

            if (this.VersionAnexoA != "")
            {
                decimal rep_ = decimal.Parse("27.2");
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, rep_);

                listaVersion = (List<FLecturaDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                listaVersion = listaVersion.Where(x => _gps.Contains(x.Gpscodi)).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), rep_);
                if (listaBytesNext != null)
                {
                    lista = (List<FLecturaDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = this.servicio.GraficoVariacionesFrecuenciaSEINDataReporte(empresas, gps, fechaInicial, fechaInicial);
            }

            if (lista.Count > 0 && this.VersionAnexoA != "")
            {
                lista = lista.Where(x => _gps.Contains(x.Gpscodi)).ToList();
            }

            if (lista.Count > 0)
            {
                model = GraficoVariacionesSostenidasSubitas(lista, fechaInicial, listaVersion);
                model.Resultado = this.servicio.GraficoVariacionesFrecuenciaHtml(lista, fechaInicial, listaVersion);
            }
            model.NRegistros = lista.Count();

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoVariacionesSostenidasSubitas(List<FLecturaDTO> listaReporte, DateTime fechaIni, List<FLecturaDTO> dataVersion)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            string titulo1 = string.Empty, name = string.Empty, name1 = string.Empty;
            model.Grafico = grafico;

            if (dataVersion.Count > 0)
            {
                var temp = listaReporte;
                var tempVersi = dataVersion;

                listaReporte = tempVersi;
                dataVersion = temp;
            }

            #region grafico 01
            string frecuencias_ = "60.6,60.5,60.4,60.3,60.2,60.1,60.0,59.9,59.8,59.7,59.6,59.5,59.4";
            var paramFrec = frecuencias_.Split(',').OrderBy(x => x).Select(decimal.Parse).ToList();
            List<decimal> valoresMin = new List<decimal>();
            List<decimal> valoresMed = new List<decimal>();
            List<decimal> valoresMax = new List<decimal>();

            DateTime fechaMin1 = new DateTime(fechaIni.Year, fechaIni.Month, fechaIni.Day, 8, 0, 0);
            DateTime fechaMin2 = new DateTime(fechaIni.Year, fechaIni.Month, fechaIni.Day, 23, 0, 0);
            DateTime fechaMed1 = new DateTime(fechaIni.Year, fechaIni.Month, fechaIni.Day, 18, 0, 0);

            var listaMinFrec = listaReporte.Where(x => (x.Fechahora <= fechaMin1 || x.Fechahora > fechaMin2)).ToList();
            var listaMedFrec = listaReporte.Where(x => (x.Fechahora > fechaMin1 && x.Fechahora < fechaMed1)).ToList();
            var listaMaxFrec = listaReporte.Where(x => (x.Fechahora >= fechaMed1 && x.Fechahora <= fechaMin2)).ToList();

            var minTotal = listaMinFrec.Count * 60;
            var medTotal = (listaMedFrec.Count * 60) + 59;
            var maxTotal = (listaMaxFrec.Count * 60) - 59;

            decimal ttMin = listaMinFrec.Count * 60;
            decimal ttMed = (listaMedFrec.Count * 60) + 59;
            decimal ttMax = (listaMaxFrec.Count * 60) - 59;
            int cont_ = 0;
            foreach (var val in paramFrec)
            {
                cont_ = 0;
                foreach (var min in listaMinFrec)
                {
                    for (var j = 0; j <= 59; j++)
                    {
                        var valorH = (decimal?)min.GetType().GetProperty("H" + j.ToString()).GetValue(min, null);
                        if (valorH != null)
                        {
                            if (val == decimal.Parse(string.Format("{0:0.0}", valorH))) { cont_++; }
                        }
                    }
                }
                valoresMin.Add(decimal.Round((cont_ / ttMin) * 100, 1));

                cont_ = 0;
                foreach (var min in listaMedFrec)
                {
                    for (var j = 0; j <= 59; j++)
                    {
                        var valorH = (decimal?)min.GetType().GetProperty("H" + j.ToString()).GetValue(min, null);
                        if (valorH != null)
                        {
                            if (val == decimal.Parse(string.Format("{0:0.0}", valorH))) { cont_++; }
                        }
                    }
                }
                valoresMed.Add(decimal.Round((cont_ / ttMed) * 100, 1));

                cont_ = 0;
                foreach (var min in listaMaxFrec)
                {
                    for (var j = 0; j <= 59; j++)
                    {
                        var valorH = (decimal?)min.GetType().GetProperty("H" + j.ToString()).GetValue(min, null);
                        if (valorH != null)
                        {
                            if (val == decimal.Parse(string.Format("{0:0.0}", valorH))) { cont_++; }
                        }
                    }
                }
                valoresMax.Add(decimal.Round((cont_ / ttMax) * 100, 1));
            }

            List<SerieDuracionCarga> series = new List<SerieDuracionCarga>();

            series.Add(AddSerieVariacionesSostenidasSubitas(valoresMax, "green", "MAX demanda"));
            series.Add(AddSerieVariacionesSostenidasSubitas(valoresMed, "red", "MED demanda"));
            series.Add(AddSerieVariacionesSostenidasSubitas(valoresMin, "blue", "MIN demanda"));

            model.ListaGrafico = series;
            titulo1 = "DISTRIBUCIÓN DE LA FRECUENCIA INSTANTÁNEA";
            model.Grafico.TitleText = titulo1;

            model.Grafico.XAxisCategories = paramFrec.Select(x => x.ToString()).Distinct().ToList();
            #endregion

            return model;
        }

        /// <summary>
        /// Agregar series de grafico 4 de maxima demanda
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="color"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private SerieDuracionCarga AddSerieVariacionesSostenidasSubitas(List<decimal> lista, string color, string name)
        {
            SerieDuracionCarga serie = new SerieDuracionCarga();

            serie.SerieColor = color;
            serie.SerieName = name;
            serie.ListaValores = lista;

            return serie;
        }
        #endregion

        /// <summary>
        /// 3.13.2.30.	Desviaciones de la demanda respecto a su pronóstico
        /// </summary>
        /// <returns></returns>
        #region ReporteDesviacionesDemandaPronostico
        //
        // GET: /IEOD/AnexoA/IndexReporteDesviacionesDemandaPronostico
        public ActionResult IndexReporteDesviacionesDemandaPronostico()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Desviaciones Demanda Pronostico
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        public JsonResult CargarListaDesviacionesDemandaPronostico(string fechaInicio, string fechaFin, int nroPagina)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();


            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 30);

                listaVersion = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 30);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                this.servicio.ReporteDesviacionDemandaPronosticoDataReporte(fechaInicial, out lista);
            }
            model.Resultado = this.servicio.ReporteDesviacionDemandaPronosticoHtml(lista, fechaInicial, listaVersion);
            model.Total = lista.Count;
            return Json(model);
        }

        /// <summary>
        /// Cargar Grafico Desviaciones Pronostico
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarGraficoDesviacionesPronostico(string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            this.servicio.ReporteDesviacionDemandaPronosticoDataReporte(fechaInicial, out lista);
            model = GraficoDemandaPronostico(lista, fechaInicial, fechaFinal);

            var jsonResult = Json(model);
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

            model.Grafico.SerieDataS = new DatosSerie[3][];
            model.Grafico.Series.Add(new RegistroSerie());
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

            model.Grafico.SerieDataS[0] = new DatosSerie[48];
            model.Grafico.SerieDataS[1] = new DatosSerie[48];
            //model.Grafico.SerieDataS[2] = new DatosSerie[48];
            model.Grafico.TitleText = @"Desviaciones de la Demanada Resoecto a su Pronostico";
            model.Grafico.YAxixTitle.Add("MW");


            if (lista.Count > 0)
            {
                model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia:Horas";

                // titulo el reporte               

                model.SheetName = "GRAFICO";
                model.Grafico.YaxixTitle = "(MWh)";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
                // Obtener Lista de intervalos categoria del grafico   
                model.Grafico.SeriesYAxis.Add(0);
                int indiceA = 0, indiceB = 0;
                for (var i = 0; i < lista.Count(); i++)
                {
                    DateTime fecha = (DateTime)lista[i].Medifecha;
                    var registro = lista[i];
                    for (var j = 1; j <= 48; j++)
                    {
                        decimal? valor = 0;
                        valor = (decimal?)registro.GetType().GetProperty("H" + (j).ToString()).GetValue(registro, null);
                        if (valor == null)
                            valor = 0;
                        if (i == 0)
                            model.Grafico.XAxisCategories.Add(registro.Medifecha.AddMinutes(j * 30).ToString(Constantes.FormatoFechaHora));
                        var serie = new DatosSerie();
                        serie.X = fecha.AddMinutes(j * 30);
                        serie.Y = valor;
                        switch (registro.Lectcodi)
                        {

                            case ConstantesPR5ReportesServicio.LectDespachoEjecutado:
                            case ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto:
                                model.Grafico.SerieDataS[0][indiceA * 48 + (j - 1)] = serie;
                                break;
                            case ConstantesPR5ReportesServicio.LectCodiProgDiaria:
                                model.Grafico.SerieDataS[1][indiceB * 48 + (j - 1)] = serie;
                                break;


                        }
                    }
                    switch (registro.Lectcodi)
                    {
                        case ConstantesPR5ReportesServicio.LectDespachoEjecutado:
                        case ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto:
                            indiceA++;
                            break;
                        case ConstantesPR5ReportesServicio.LectCodiProgDiaria:
                            indiceB++;
                            break;
                        case ConstantesPR5ReportesServicio.LectCodiProgSemanal:
                            //indiceC++;
                            break;
                    }

                }
                //modelGraficoDiario = model;
            }// end del if 
            return model;
        }



        /// <summary>
        /// Paginado de Reporte de Desviacion de la demanda
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoDesvDemanda(string fechaInicio, string fechaFin)
        {
            Paginacion model = new Paginacion();

            DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            int nroRegistros = (Int32)(fecFin - fecInicio).TotalDays;
            if (nroRegistros > 0)
            {
                model.NroPaginas = nroRegistros;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }
        #endregion

        /// <summary>
        /// 3.13.2.31.	Desviaciones de la producción de las Unidades de Generación
        /// </summary>
        /// <returns></returns>
        #region ReporteDesviacionesProduccionUG
        //
        // GET: /IEOD/AnexoA/IndexReporteDesviacionesProduccionUG
        public ActionResult IndexReporteDesviacionesProduccionUG()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();
            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);

            var empresas = servicio.GetListaCriteria(ConstantesPR5ReportesServicio.TipoEmpresa);
            model.ListaEmpresas = servicio.ConvertSi_empresaBySiempresa(empresas);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;

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
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int[] empresas_ = new int[empresa.Length];
            empresas_ = empresa.Split(',').Select(x => int.Parse(x)).ToArray();
            this.servicio.ReporteDesviacionesProduccionUGDataVersionada(empresa, fechaInicial, fechaFinal, this.VersionAnexoA, out listaDespacho, out listaVersion);

            model.Resultado = this.servicio.ReporteDesviacionesProduccionUGHtml(listaDespacho, fechaInicial, listaVersion);
            return Json(model);
        }
        #endregion

        /// <summary>
        /// 3.13.2.32.	Costos Marginales de Corto Plazo cada 30 minutos en las Barras del SEIN.
        /// </summary>
        /// <returns></returns>
        #region ReporteCostoMarginalesCortoPlazo
        //
        // GET: /IEOD/AnexoA/IndexReporteCostoMarginalesCortoPlazo
        public ActionResult IndexReporteCostoMarginalesCortoPlazo()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();
            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;
            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Costo Marginales CP
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public PartialViewResult CargarListaCostoMarginalesCP(string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<CmCostomarginalDTO> lista = new List<CmCostomarginalDTO>();
            List<CmCostomarginalDTO> listaVersion = new List<CmCostomarginalDTO>();
            int lectcodiCM = ConstantesPR5ReportesServicio.LectcodiCM;

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 32);

                listaVersion = (List<CmCostomarginalDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 32);
                if (listaBytesNext != null)
                {
                    lista = (List<CmCostomarginalDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = servicio.GetCostosMarginalesPorRangoFechaSntRosa(fechaInicial, fechaFinal, lectcodiCM);
            }

            //model.Resultado = this.servicio.ListaReporteCostoMarginalesCP(lista, fechaInicio, fechaFin);
            model.ListaCM = listaVersion;
            model.ListaCM = lista;
            model.CabeceraCM = lista.GroupBy(x => new { x.Cnfbarcodi, x.Cnfbarnombre }).Select(x => new CmCostomarginalDTO { Cnfbarcodi = x.Key.Cnfbarcodi, Cnfbarnombre = x.Key.Cnfbarnombre }).OrderBy(x => x.Cnfbarcodi).ToList();
            model.Fecha = fechaInicial;

            return PartialView(model);
        }
        #endregion

        /// <summary>
        /// 3.13.2.33.	Costo total de operación ejecutada.
        /// </summary>
        /// <returns></returns>
        #region ReporteCostoTotalOperacionEjecutada
        //
        // GET: /IEOD/AnexoA/IndexReporteCostoTotalOperacionEjecutada
        public ActionResult IndexReporteCostoTotalOperacionEjecutada()//por ver
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;

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
            DateTime fechaInicial = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel();

            List<MeMedicion1DTO> lista = this.servicio.ReporteCostoTotalOperacionEjecutadaDataReporte(fechaInicial);
            model = this.GraficoCostoTotalOperacionEjecutada(lista);

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Grafico de Generacion de Maxima demanda del SEIN
        /// </summary>
        /// <param name="listaData"></param>
        /// <param name="tipoReporte"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoCostoTotalOperacionEjecutada(List<MeMedicion1DTO> lista)
        {
            List<MeMedicion1DTO> listaEjec = lista.Where(x => x.Lectcodi == ConstantesPR5ReportesServicio.LectCostoOperacionEjec).ToList();
            List<MeMedicion1DTO> listaProg = lista.Where(x => x.Lectcodi == ConstantesPR5ReportesServicio.LectCostoOperacionProg).ToList();
            List<DateTime> listaFecha = lista.Select(x => x.Medifecha.Date).Distinct().OrderBy(x => x.Date).ToList();
            int totalDia = listaFecha.Count();

            //
            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.TitleText = @"COSTO TOTAL DE LA OPERACIÓN POR DÍA";
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            model.Grafico.YaxixTitle = "Miles de S/.";
            model.Grafico.SerieDataS = new DatosSerie[2][];

            //Eje X
            model.Grafico.XAxisTitle = "DÍA";
            model.Grafico.XAxisCategories = new List<string>();
            for (int h = 0; h < totalDia; h++)
            {
                model.Grafico.XAxisCategories.Add(EPDate.f_NombreDiaSemanaCorto(listaFecha[h].DayOfWeek) + " " + listaFecha[h].Day);
            }

            //DATA SERIES
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesData = new decimal?[2][];

            //
            model.Grafico.Series.Add(new RegistroSerie());
            model.Grafico.Series[0].Name = "COSTO TOTAL EJEC";
            model.Grafico.Series[0].Type = "column";
            model.Grafico.Series[0].YAxis = 0;
            model.Grafico.Series[0].Color = "#87CCE9";
            //model.Grafico.SeriesData[0] = new decimal?[totalDia];
            model.Grafico.SerieDataS[0] = new DatosSerie[totalDia];
            MeMedicion1DTO ejec = null;
            for (int h = 0; h < totalDia; h++)
            {
                ejec = listaEjec.Find(x => x.Medifecha == listaFecha[h]);
                //model.Grafico.SeriesData[0][h] = ejec.H1 != null ? ejec.H1 / 1000 : ejec.H1;
                model.Grafico.SerieDataS[0][h] = new DatosSerie();
                model.Grafico.SerieDataS[0][h].Y = ejec.H1 != null ? ejec.H1 / 1000 : ejec.H1;
                model.Grafico.SerieDataS[0][h].Name = "%";
            }

            //
            model.Grafico.Series.Add(new RegistroSerie());
            model.Grafico.Series[1].Name = "COSTO TOTAL PROG";
            model.Grafico.Series[1].Type = "column";
            model.Grafico.Series[1].YAxis = 0;
            model.Grafico.Series[1].Color = "#FFA602";
            //model.Grafico.SeriesData[1] = new decimal?[totalDia];
            model.Grafico.SerieDataS[1] = new DatosSerie[totalDia];
            MeMedicion1DTO prog = null;
            for (int h = 0; h < totalDia; h++)
            {
                prog = listaProg.Find(x => x.Medifecha == listaFecha[h]);
                //model.Grafico.SeriesData[1][h] = prog.H1 != null ? prog.H1 / 1000 : prog.H1;
                model.Grafico.SerieDataS[1][h] = new DatosSerie();
                model.Grafico.SerieDataS[1][h].Y = prog.H1 != null ? prog.H1 / 1000 : prog.H1;
                model.Grafico.SerieDataS[1][h].Name = "";
            }

            return model;
        }
        #endregion

        /// <summary>
        /// 3.13.2.35.	Registro de las congestiones del Sistema de Transmisión.
        /// </summary>
        /// <returns></returns>
        #region ReporteRegistroCongestionesST
        //
        // GET: /IEOD/AnexoA/IndexReporteRegistroCongestionesST
        public ActionResult IndexReporteRegistroCongestionesST()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;

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
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<EveIeodcuadroDTO> lista = new List<EveIeodcuadroDTO>();
            List<EveIeodcuadroDTO> listaVersion = new List<EveIeodcuadroDTO>();
            string resultado = " ";

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            string listaClase = "1";
            string listaSubcausa = ConstantesPR5ReportesServicio.IdSubCausaCongestionesST;

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 35);

                listaVersion = (List<EveIeodcuadroDTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 35);
                if (listaBytesNext != null)
                {
                    lista = (List<EveIeodcuadroDTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = servicio.ReporteSistemasAisladosTemporalesDataReporte(fechaInicial, fechaFinal, listaClase, listaSubcausa);
            }

            resultado = this.servicio.ReporteSobrecargaEquipoHtml(lista, listaVersion);

            model.Resultado = resultado;

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
        public ActionResult IndexReporteAsignacionRRPFyRRSF()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);
            decimal? valor = this.servicio.GetMagnitudRPF(FechaFilter1);
            model.MagnitudRPF = valor != null ? String.Format("{0:0.00}", valor) + "%" : string.Empty;
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;

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
            PublicacionIEODModel model = new PublicacionIEODModel();
            string resultado = string.Empty;
            string[][] lista = new string[1][];
            string[][] listaVersion = new string[1][];

            DateTime fecha = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            resultado = this.servicio.ReporteAsignacionRRPFyRRSHtml(fecha, lista, listaVersion);
            model.Resultado = resultado;

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
        public ActionResult IndexReporteRegistroFlujosEnlacesInternacionales()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.ToString(Constantes.FormatoFecha);

            var empresas = servicio.GetListaEmpresaFormato(ConstantesPR5ReportesServicio.IdFormatoFlujoTrans).Where(x => x.Emprcodi == ConstantesPR5ReportesServicio.EmprcodiByZorritos).ToList();
            model.ListaEmpresas = empresas;
            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;

            return View(model);
        }

        /// <summary>
        /// Cargar subestaciones de flujo de enlaces internacionales
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarSubEstacionF(int idEmpresa)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Resultado = this.servicio.ListadoSubEstacionF(idEmpresa);

            return Json(model);
        }

        /// <summary>
        /// Cargar reporte de flujo de enlaces internacionales
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idSubEstacion"></param>
        /// <returns></returns>
        public JsonResult CargarListaRegistroFlujosEI(string fechaInicio, string fechaFin, int idEmpresa, string idSubEstacion)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();
            string resultado = "";

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            model.Formato = servFormato.GetByIdMeFormato(ConstantesPR5ReportesServicio.IdFormatoFlujoTrans);

            if (model.Formato != null)
            {
                var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
                var ListaPtos = servFormato.GetListaPtos(DateTime.Now.Date, 0, idEmpresa, ConstantesPR5ReportesServicio.IdFormatoFlujoTrans, cabecera.Cabquery);

                //Filtrar ListaHoja puntos con variable idSubEstacion
                var entitys = ListaPtos.Where(x => x.Ptomedicodi == ConstantesPR5ReportesServicio.PtomedicodiZorritos && x.Famcodi == int.Parse(ConstantesPR5ReportesServicio.FamcodiLineaTrans)).ToList();
                model.ListaHojaPto = entitys;
            }

            if (this.VersionAnexoA != "")
            {
                var listaBytes = servicio.GetByVersionDetIEOD(this.VersionAnexoA, 37);

                listaVersion = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytes.Versdatos);
                //listaVersion = listaVersion.Where(x => x.Emprcodi == idEmpresa).ToList();

                var listaBytesNext = servicio.GetByVersionDetIEOD((int.Parse(this.VersionAnexoA) + 1).ToString(), 37);
                if (listaBytesNext != null)
                {
                    lista = (List<MeMedicion48DTO>)DeserializandoReporteAnexoA(listaBytesNext.Versdatos);
                }
            }
            else
            {
                lista = servicio.ListaFlujosPotencia(ConstantesPR5ReportesServicio.IdFormatoFlujoTrans, idEmpresa.ToString(), fechaInicial, fechaFinal);
            }

            if (lista.Count > 0 && this.VersionAnexoA != "")
            {
                lista = lista.Where(x => x.Emprcodi == idEmpresa).ToList();
            }

            //lista = ListaM48.Where(x => x.Tipoinfocodi == ConstantesPR5ReportesServicio.TipoinfoMW).ToList();

            resultado = this.servicio.ReporteRegistroFlujosEIHtml(model.ListaHojaPto, lista, fechaInicial, listaVersion);

            model = new PublicacionIEODModel();
            model.Resultado = resultado;

            return Json(model);
        }

        /// <summary>
        /// Paginado de reporte de flujo de enlaces internacionales
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idSubEstacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoFlujoPot(string fechaInicio, string fechaFin, int idEmpresa, string idSubEstacion)
        {
            Paginacion model = new Paginacion();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicio))
            {
                fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(fechaFin))
            {
                fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            var ListaM48 = servicio.ListaFlujosPotencia(ConstantesPR5ReportesServicio.IdFormatoFlujoTrans, idEmpresa.ToString(), fecInicio, fecFin).Where(x => x.Tipoinfocodi == ConstantesPR5ReportesServicio.TipoinfoMW).ToList();
            this.ListaFechas = ListaM48.OrderBy(x => x.Medifecha).Select(x => x.Medifecha).Distinct().ToList();

            int nroRegistros = this.ListaFechas.Count();
            if (nroRegistros > 0)
            {
                model.NroPaginas = nroRegistros;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }
        #endregion

        /// <summary>
        /// 12. Pruebas de unidades de generación por requerimientos propios y por pruebas aleatorias de disponibilidad.
        /// </summary>
        /// <returns></returns>
        #region ReporteRequerimientosPropios
        //
        // GET: /IEOD/AnexoA/IndexRequerimientosPropios
        public ActionResult IndexRequerimientosPropios()
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();
            BusquedaIEODModel model = new BusquedaIEODModel();

            model.FechaInicio = FechaFilter1.ToString(Constantes.FormatoFecha);
            model.FechaFin = FechaFilter1.AddDays(1).ToString(Constantes.FormatoFecha);

            this.IndexReporte = this.ControllerContext.RouteData.Values["action"].ToString();
            model.NroReporte = IndexReporte;

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaRequerimientosPropios(string fechaInicio, string fechaFin)
        {
            List<PublicacionIEODModel> ListaModel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model = new PublicacionIEODModel();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            var ListaReqPropios = (new MigracionesAppServicio()).GetListaReqPropios(fechaInicial, fechaFinal);//09
            model.Resultado = (new MigracionesAppServicio()).ListaReqPropiosHtml(ListaReqPropios);
            ListaModel.Add(model);

            model = new PublicacionIEODModel();
            var ListaLogSorteo = (new MigracionesAppServicio()).GetListaLogSorteo(fechaInicial);//09
            if (ListaLogSorteo.Count > 0)
            {
                var ListaPruebasAleatorias = (new MigracionesAppServicio()).GetListaPruebasAleatorias(fechaInicial);//09
                if (ListaPruebasAleatorias.Count > 0)
                {
                    model.Resultado = (new MigracionesAppServicio()).GenerarHtmlCompensacionPruebasAleatorias(ListaPruebasAleatorias, 1);
                    ListaModel.Add(model);

                    model = new PublicacionIEODModel();
                    int TotalSorteo = (new MigracionesAppServicio()).GetTotalConteoTipo(fechaInicial, "XEQ");//09
                    if (TotalSorteo > 0)
                    {
                        model.Resultado = "<p>Resultado del sorteo: Balota Negra - Día de Prueba...</p>";
                        ListaModel.Add(model);

                        model = new PublicacionIEODModel();
                        ListaPruebasAleatorias = ListaPruebasAleatorias.Where(x => x.Ecodigo == "S").ToList();
                        if (ListaPruebasAleatorias.Count > 0)
                        {
                            model.Resultado = (new MigracionesAppServicio()).GenerarHtmlCompensacionPruebasAleatorias(ListaPruebasAleatorias, 2);
                            ListaModel.Add(model);
                        }
                        else { model.Resultado = (new MigracionesAppServicio()).CabeceraPruebasAleatorias(1).ToString() + "<tbody><td colspan='3'>No hay equipos disponibles para prueba...</td></tbody></table>"; ListaModel.Add(model); }

                    }
                    else { model.Resultado = "<p>Resultado del sorteo: Balota Blanca - Día de no Prueba...</p>"; ListaModel.Add(model); }
                }
                else { model.Resultado = (new MigracionesAppServicio()).CabeceraPruebasAleatorias(1).ToString() + "<tbody><td colspan='3'>Sin informacion...</td></tbody></table>"; ListaModel.Add(model); }
            }
            else { model.Resultado = (new MigracionesAppServicio()).CabeceraPruebasAleatorias(1).ToString() + "<tbody><td colspan='3'>Hoy no se realizo sorteo...</td></tbody></table>"; ListaModel.Add(model); }

            return Json(ListaModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargaResumenPruebasAleatorias(int equicodi, string fechaInicio, string fechaFin)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            var pruebasUnidad = (new MigracionesAppServicio()).GetListaPruebasAleatorias(fechaInicial).Where(x => x.Ecodigo == "S").ToList();//09
            var pruebasUnidadDet = (new PruebaunidadAppServicio()).GetByCriteriaEvePruebaunidads(fechaInicial, fechaFinal);
            var objPtomedicion = (new MigracionesAppServicio()).GetMePtomedicionXEq(equicodi.ToString(), ConstantesAppServicio.OriglectcodiMedidoresGene.ToString());

            if (objPtomedicion.Count > 0)
            {
                var medidores = (new PruebaunidadAppServicio()).GetByCriteriaMedicion96(ConstantesAppServicio.TipoinfocodiMW, objPtomedicion[0].Ptomedicodi, ConstantesAppServicio.LectcodiMedidoresGene, fechaInicial, fechaInicial);
                model.Resultado = (new MigracionesAppServicio()).GenerarHtmlResumenPruebasAleatorias(pruebasUnidad, pruebasUnidadDet, medidores);
            }

            return Json(model);
        }
        #endregion
    }
}