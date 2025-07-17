using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.RDO.Helper
{
    /// <summary>
    /// Parámetros de formato
    /// </summary>
    public class ParametrosFormato
    {
        public const int PeriodoDiario = 1;
        public const int PeriodoSemanal = 2;
        public const int PeriodoMensual = 3;
        public const int PeriodoAnual = 4;
        public const int PeriodoMensualSemana = 5;

        public const int ResolucionCuartoHora = 15;
        public const int ResolucionMediaHora = 30;
        public const int ResolucionHora = 60;
        public const int ResolucionDia = 60 * 24;
        public const int ResolucionSemana = 60 * 24 * 7;
        public const int ResolucionMes = 60 * 24 * 30;
        public const int FilaExcelData = 13;
        public const int Ejecutado = 1;
        public const int Programado = 2;
        public const int FilaExcelDataCP = 16;
        public const int TiempoReal = 0;

        public const int TipoAgregarTiempoAdicionalNo = 0;
        public const int TipoAgregarTiempoAdicionalAdd1 = 1;
        public const int TipoAgregarTiempoAdicionalSub1 = -1;

        public const int TotalMinDia = 24 * 60;
        public const int RowDatos = 18;
        public const int RowTitulo = 9;
        public const int RowCodigo = 1;
        public const int RowArea = 6;
        public const int ColDatos = 2;
        public const int ColFecha = 1;
        public const int ColEmpresa = 150;
        public const int ColFormato = 151;
    }

    /// <summary>
    /// Parámetros de formato
    /// </summary>
    public class ConstantesRDO
    {
        public const string LogError = "ERROR.APLICACION";
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        public const string FormatoFechaFull = "dd/MM/yyyy HH:mm";
        public const int VerUltimoEnvio = 1;
        public const int NoVerUltimoEnvio = 0;
        public const string HojaFormatoExcel = "FORMATO";
        public const string CaracterEnter = "\n";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string ExtensionFileUpload = "xlsx";
        public const string RutaCargaFile = "RutaCargaFile";
        public const string MensajeEnvioExito = "El envío se realizó correctamente";       
        public const string CaracterH = "H";
    }
    /// <summary>
    /// Parámetros de formato de medición
    /// </summary>
    public class ConstantesFormatoMedicion
    {
        public const int IdPtoMedicionL2280 = 5020;

        //Prueba
        //public const int IdExportacionL2280MWh = 41103;
        //public const int IdImportacionL2280MWh = 41104;
        //public const int IdExportacionL2280MVARr = 41105;
        //public const int IdImportacionL2280MVARr = 41106;

        //Produccion
        public const int IdExportacionL2280MWh = 41238;
        public const int IdImportacionL2280MWh = 41239;
        public const int IdExportacionL2280MVARr = 41240;
        public const int IdImportacionL2280MVARr = 41241;

        public const int IdMWh = 3;
        public const int IdMVARh = 4;
        public const int IdlecturaInterconexion = 1;
        public const int IdCausaJustificacion = 203;
        public const int IdFormatoTablaPrie01 = 81;

        //inicio pruebas unidad
        public const int IdFormatoPruebasAleatorias = 84;
        public const int IdTipoinfocodiMw = 1;
        public const int IdLectcodiPruebasAleatorias = 106;
        //fin pruebas unidad

        //Estructuras para almacenamiento de información para los aplicativos BI de Producción de energía y Máxima Demanda
        public const int IdExportacionMW = 46659;
        public const int IdImportacionMW = 46658;

        #region FIT - VALORIZACION DIARIA
        public const int IdOrigenLecturaTransferencias = 34;
        #endregion

        #region Pronóstico de Demanda

        public const int FormatcodiProgDiario = 47;
        public const int FormatcodiReprogDiario = 49;
        public const int FormatcodiProgSemanal = 48;

        #endregion

        //Generación RER Programada
        public const int IdFormatoGeneracionRERDiario = 121;
        public const int IdFormatoGeneracionRERSemanal = 122;
        public const int LectGeneracionRERDiario = 61;
        public const int LectGeneracionRERSemanal = 62;
        public const string ColorColumnaHora = "#CCFFFF";
        public const string ColorColumnaCentralPar = "#99CCFF";
        public const string ColorColumnaCentralImpar = "#c1c1ff";
        public const string ColorColumnaCentralParWeb = "#54a6f9";
        public const string ColorColumnaCentralImparWeb = "#9393F1";
        public const int TipoPresentacionUnidad = 1;
        public const int TipoPresentacionGrupoDespacho = 2;
        public const int TipoPresentacionCentral = 3;

        public const int AplicativoProgRER = 1;
        public const string AplicativoProgRERListaFormato = "65,121,122";
        public const string AplicativoProgRERListaFormatoAmpl = "121,122";

        public const int AplicativoStock = 2;
        public const string AplicativoStockListaFormato = "57,58,59,60";

        public const int AplicativoPotenciaFirme = 3;
        public const string AplicativoPotenciaFirmeListaFormato = "123";

        public const int AplicativoIndisponibilidades = 4;
        public const string AplicativoIndisponibilidadesListaFormato = "85";

        #region Mejoras RDO
        public const int IdFormatoGeneracionDespachoDiario = 125;
        #endregion
    }

    /// <summary>
    /// Datos de sesion
    /// </summary>
    public class DatosSesionRDO
    {
        public const string SesionFormato = "SesionFormato";
        public const string SesionIdEnvio = "SesionIdEnvio";
        public const string SesionFileName = "SesionFileName";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";
    }

    /// <summary>
    /// Tipos de lectura
    /// </summary>
    public class ParametrosLectura
    {
        public const int PeriodoDiario = 1;
        public const int PeriodoSemanal = 2;
        public const int PeriodoMensual = 3;
        public const int PeriodoAnual = 4;
        public const int PeriodoMensualSemana = 5;
        public const int Ejecutado = 1;
        public const int Programado = 2;
    }
    /// <summary>
    /// Parámetros Generación Despacho
    /// </summary>
    public class ParametrosGeneracionDespachoExcel
    {
        public const int FilaExcelData = 8;
        public const int RowDatos = 8;
        public const int RowFormato = 3;
        public const int RowCodigo = 1;
        public const int RowArea = 6;
        public const int ColDatos = 2;
        public const int ColFecha = 1;
        public const int ColEmpresa = 2;
        public const int ColFormato = 2;
    }
    /// <summary>
    /// Parámetros de Envío
    /// </summary>
    public class ParametrosEnvio
    {
        public const byte EnvioEnviado = 1;
        public const byte EnvioProcesado = 2;
        public const byte EnvioAprobado = 3;
        public const byte EnvioRechazado = 4;
        public const byte EnvioFueraPlazo = 5;
    }

    /// <summary>
    /// Constantes de envío
    /// </summary>
    public class ConstantesEnvioRdo
    {
        //Tipo de Envios
        public const string ENVIO_EN_PLAZO = "P";
        public const string ENVIO_FUERA_PLAZO = "F";
        public const string ENVIO_PLAZO_DESHABILITADO = "";

        //Panel Ieod - Cumplimiento
        public const int CumplEnviadoFueraPlazo = 0;
        public const int CumplEnviadoEnPlazo = 1;
        public const int CumplEnviadoFdat = 2;
        public const int CumplEnviadoIncompleto = 3;
    }

    /// <summary>
    /// Constantes de Generación de Despacho RDO
    /// </summary>
    public class ConstantesGeneracionDespachoRDO
    {
        public const string PlantillaGeneracionDespachoRDO = "PlantillaGeneracionDespacho.xlsx";
        public const string FolderUpload = "Areas\\RDO\\Uploads\\";
        public const string FolderReporte = "Areas\\RDO\\Reportes\\";
        public const int CodigoCabeceraDespacho_Intranet = 42;
        public const int FormatoDiarioCodi = 228;
    }

    public class ConstantesDemandaCP
    {
        public const int FormatoDiarioCodi = 47;
        public const int FormatoSemanalCodi = 48;
        public const int EstacionHidrologica = 43;
        public const string FolderUpload = "Areas\\DemandaCP\\Uploads\\";
        public const string FolderReporte = "Areas\\DemandaCP\\Reportes\\";
    }

    public class ConstantesHidrologiaCD
    {
        //Formato Hidrología para Carga Datos
        public const int FormatoDiarioCodi = 109;
        public const int FormatoReprogramaCodi = 110;
        public const int FormatoSemanalCodi = 111;

        //Formato Hidrología para VOLUMEN INICIAL Carga Datos
        public const int FormatoVolumenDIarioCodi = 115;
        public const int FormatoVolumenReprogramaCodi = 116;
        public const int FormatoVolumenSemanalCodi = 117;

        public const string FolderUpload = "Areas\\Hidrologia\\Uploads\\";
        public const string FolderReporte = "Areas\\Hidrologia\\Reportes\\";
    }

    /// <summary>
    /// Permisos sobre una opcion
    /// </summary>
    public struct Acciones
    {
        public const int Grabar = 1;
        public const int Editar = 2;
        public const int Nuevo = 3;
        public const int Consultar = 4;
        public const int Eliminar = 5;
        public const int Copiar = 6;
        public const int Exportar = 7;
        public const int Anular = 8;
        public const int Imprimir = 9;
        public const int Detalle = 10;
        public const int Rechazar = 11;
        public const int Aprobar = 12;
        public const int AccesoEmpresa = 13;
        public const int Informe = 15;
        public const int Importar = 16;
        public const int Adicional = 17;
        public const int Consolidado = 18;
        public const int PermisoSCO = 19;
        public const int PermisoSEV = 20;
        public const int GenerarArchivo = 21;
        public const int AccionRiSGI = 22;
        public const int AccionRiDJR = 23;
        public const int AccionRiDE = 24;
        public const int PermisoSEVAseg = 25;
    }

    /// <summary>
    /// Query string
    /// </summary>
    public class QueryParametros
    {
        public const char SeparadorFila = '#';
        public const char SeparadorCol = ',';
    }

    /// <summary>
    /// Semanas
    /// </summary>
    public class FirstDayOfWeek
    {
        public const int Sunday = 0;
        public const int Monday = 1;
        public const int Tuesday = 2;
        public const int Wednesday = 3;
        public const int Thursday = 4;
        public const int Friday = 5;
        public const int Saturday = 6;
    }

    /// <summary>
    /// Modulos del sistema
    /// </summary>
    public class Modulos
    {
        public const int AppHidrologia = 3;
        public const int AppMedidoresDistribucion = 4;
        public const int AppDemandaCP = 5;
    }

    /// <summary>
    /// Constantes para el manejo de handson
    /// </summary>
    public class HandsonConstantes
    {
        public const int ColWidth = 145;
        public const int ColPorHoja = 7;
    }

    public class CeldaCambios
    {
        public int Col { get; set; }
        public int Row { get; set; }
    }
    public class CeldaMerge
    {
        public int col { get; set; }
        public int row { get; set; }
        public int colspan { get; set; }
        public int rowspan { get; set; }
        public int rowAbsoluto { get; set; }
    }
    public class TipoInformacion
    {
        public int IdTipoInfo { get; set; }
        public string NombreTipoInfo { get; set; }
        public string Valor2 { get; set; }
    }
    public class CabeceraRow
    {
        public string TituloRow { get; set; }
        public string NombreRow { get; set; }
        public int IsMerge { get; set; }
        public string TituloRowAnt { get; set; }
        public int ColumnIni { get; set; }
        public int Ancho { get; set; }
        public string AlineacionHorizontal { get; set; }
        public string AlineacionVertical { get; set; }
        public int TipoLimite { get; set; }
        public string TipoDato { get; set; }
        public bool EsEditable { get; set; }
        public string NombreClase { get; set; }
    }

    #region Formatos
    public class FormatoModel
    {
        public bool EsEmpresaVigente { get; set; }
        public string TipoPlazo { get; set; }
        public string AreaCoes { get; set; }
        public string Titulo { get; set; }
        public string Empresa { get; set; }
        public int IdEmpresa { get; set; }
        public int IdLectura { get; set; }
        public int IdFormato { get; set; }
        public int IdEnvio { get; set; }
        public int IdModulo { get; set; }
        public int IdArea { get; set; }
        public string FechaEnvio { get; set; }
        public bool OpGrabar { get; set; }
        public bool OpAccesoEmpresa { get; set; }
        public bool OpEditar { get; set; }
        public string Anho { get; set; }
        public string Mes { get; set; }
        public string Semana { get; set; }
        public string Dia { get; set; }
        public string Fecha { get; set; }
        public int NroSemana { get; set; }
        public string StrFormatCodi { get; set; }
        public string StrFormatPeriodo { get; set; }
        public string StrFormatDescrip { get; set; }
        public MeFormatoDTO Formato { get; set; }
        public List<MeHojaptomedDTO> ListaHojaPto { get; set; }
        public List<MeEnvioDTO> ListaEnvios { get; set; }
        public List<CeldaCambios> ListaCambios { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<MeLecturaDTO> ListaLectura { get; set; }
        public List<MeFormatoDTO> ListaFormato { get; set; }
        public List<int> ListaOpcionRibbon { get; set; }
        public List<FwAreaDTO> ListaAreas { get; set; }
        public List<string> ListaSemanas { get; set; }
        public List<GenericoDTO> ListaSemanas2 { get; set; }
        public List<ListaSelect> ListaPtoMedicion { get; set; }
        public int FilasCabecera { get; set; }
        public List<int> ListaFilasOcultas { get; set; }
        public int ColumnasCabecera { get; set; }
        public int ColumnasFijas { get; set; }
        public string ViewHtml { get; set; }
        public Boolean EnPlazo { get; set; }
        public string MensajePlazo { get; set; }
        public string MensajeError { get; set; }
        public HandsonModel Handson { get; set; }
        public int Editable { get; set; }
        public List<TipoInformacion> ListaSemana { get; set; }
        public Boolean ValidacionFormatoCheckblanco { get; set; }
        public List<string> ListaMensajeValidacion { get; set; }
        public List<string> ListaObservaciones { get; set; }
        public List<MeEnvioDTO> ListaEnvios2 { get; set; }
        public MeEnvioDTO EnvioActual { get; set; }
        public int IdEnvioLast { get; set; }
        public string FechaEnvioLast { get; set; }
        public string fechaMaximaDemanda { get; set; }
        public List<MePtosuministradorDTO> ListaPtoSuministrador { get; set; }
        public List<SiEmpresaDTO> ListaEmpresasSuministradoras { get; set; }
        public List<string> ListaAreaOperativa { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<EqAreaDTO> ListaSubestacion { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<SiTipoinformacionDTO> ListaTipoInformacion { get; set; }
        public List<MeTipopuntomedicionDTO> ListaTipopuntotomedicion { get; set; }
        public Boolean EnabledDespacho { get; set; }
        public List<decimal> ListaDespacho { get; set; }
        public List<EveManttoDTO> ListaMantenimiento { get; set; }
        public List<EveEventoDTO> ListaEvento { get; set; }
        public List<EqEquirelDTO> ListaTopologia { get; set; }
        public Boolean IsExcelWeb { get; set; }
        public string FechaNext { get; set; }
        public Boolean ValidaMantenimiento { get; set; }
        public Boolean ValidaRestricOperativa { get; set; }
        public Boolean ValidaEventos { get; set; }
        public Boolean UtilizaScada { get; set; }
        public Boolean ValidaHorasOperacion { get; set; }
        public Boolean ValidaMedidorGeneracion { get; set; }
        public Boolean ValidaCentralSolar { get; set; }
        public int IdAplicativo { get; set; }
        public int IdHoja { get; set; }
        public string NombreHoja { get; set; }
        public Boolean UtilizaMedidorGeneracion { get; set; }
        public SiParametroValorDTO ParamSolar { get; set; }
        public Boolean UtilizaHoja { get; set; }
        public List<FormatoModel> ListaFormatoModel { get; set; }
        public MeHojaDTO Hoja { get; set; }
        public List<MeJustificacionDTO> ListaJustificacion { get; set; }
        public List<string[][]> ListaData { get; set; }
        public List<int> ListaHoja { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public int ViewHojaPto2 { get; set; }
        public int IdFormatoNuevo { get; set; }
        public Boolean ValidaSimilitudDataPeriodoAnt { get; set; }
        public Boolean ValidaTiempoReal { get; set; }
        public int IdHojaPadre { get; set; }
        public Boolean MostrarDataBDSinEnvioPrevio { get; set; }
        public Boolean UtilizaFiltroCentral { get; set; }
        public Boolean UtilizaFlujoTrans { get; set; }
        public List<TipoInformacion> ListaSelectSemanas { get; set; }
        public List<EveSubcausaeventoDTO> ListaCausaJustificacion { get; set; }
        public int Periodo { get; set; }
        public List<MeHojaDTO> ListaMeHoja { get; set; }
        public List<MeHojaDTO> ListaMeHojaPadre { get; set; }
        public string ListaMeHojaJson { get; set; }
        public string TituloGrafico { get; set; }
        public List<MePlazoptoDTO> ListaPtoConfigPlazo { get; set; }
        public string Emprabrev { get; set; }
        public string NombreArchivoExcel { get; set; }    
        public Boolean TieneDataScada { get; set; }

    }

    public class CaudalVolumenModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public int empresa { get; set; }
        public string EmprNomb { get; set; }
        public string ViewHtml { get; set; }
        public List<string> ListaSemanas { get; set; }
        public int NroSemana { get; set; }
        public string NombreMes { get; set; }
        public int Dia { get; set; }
        public string Fecha { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string FechaPlazo { get; set; }
        public string AnhoMes { get; set; }
        public int HoraPlazo { get; set; }
        public string Resultado { get; set; }
        public int IdModulo { get; set; }
        public List<MeHojaptomedDTO> ListaHojaPto { get; set; }
        public MeFormatoDTO Formato { get; set; }
        public List<MeHeadcolumnDTO> ListaHeadColumn { get; set; }
        public List<MePtomedicionDTO> ListaMedicion { get; set; }
        public List<PtoMedida> ListaPtoMedida { get; set; }
        public List<EqEquipoDTO> ListaCuenca { get; set; }
        public List<MeFormatoDTO> ListaFormato { get; set; }
        public List<MeLecturaDTO> ListaLectura { get; set; }
        public List<MeAmpliacionfechaDTO> ListaAmpliacion { get; set; }
        public bool EnPlazo { get; set; }
        public string ValidacionPlazo { get; set; }
        public string StrFormatCodi { get; set; }
        public string StrFormatPeriodo { get; set; }
        public string StrFormatDescrip { get; set; }
        public string cadenaLectCodi { get; set; }
        public string cadenaLectPeriodo { get; set; }
    }
    public class HandsonModel
    {
        public string[][] ListaExcelData { get; set; }
        public string ListaExcelData2 { get; set; }
        public string Esquema { get; set; }
        public string[][] ListaExcelDescripcion { get; set; }
        public string[][] ListaExcelFormatoHtml { get; set; }
        public List<CeldaMerge> ListaMerge { get; set; }
        public List<int> ListaColWidth { get; set; }
        public List<Boolean> ListaFilaReadOnly { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Boolean ReadOnly { get; set; }
        public string[][] ListaSourceDropDown { get; set; }
        public short[][] MatrizTipoEstado { get; set; }
        public string[][] ListaExcelComment { get; set; }
        public int TieneData { get; set; }
        public bool[][] MatrizEstado { get; set; }
        public List<string> ListaDropDown { get; set; }
        public int HMaximoData48Enviado { get; set; }
        public int HMaximoDataScadaDisponible { get; set; }
        public string Resultado { get; set; }
        public object[] Columnas { get; set; }
        public string[] Headers { get; set; }
        public List<CeldaCambios> ListaCambios { get; set; }
        public NestedHeaders NestedHeader { get; internal set; }
        public string[][] ListaExcelDataEjecutados { get; set; }
        public string[][] ListaLimitesMinYupana { get; set; }
        public string[][] ListaLimitesMaxYupana { get; set; }
    }
    public class NestedHeaders
    {
        public NestedHeaders()
        {
            ListCellNestedHeaders = new List<List<CellNestedHeader>>();
        }
        public List<List<CellNestedHeader>> ListCellNestedHeaders { get; set; }
    }
    public class CellNestedHeader
    {
        public string Label { get; set; }
        public int Colspan { get; set; }
    }
    public class ListaSelect
    {
        public int id { get; set; } //se requiere campo en minuscula
        public string text { get; set; } //se requiere campo en minuscula
        public string codigo { get; set; }
        public decimal? monto { get; set; }
    }
    #endregion

    public class FormatoResultado
    {
        public int Resultado { get; set; }
        public int IdEnvio { get; set; }
        public string Mensaje { get; set; }

        //agregado de la extranet
        public string Detalle { get; set; }
        public string sAplicacion { get; set; }
        public int IdHojaPadre { get; set; }
    }

    public class PtoMedida
    {
        public int IdMedida { get; set; }
        public string NombreMedida { get; set; }
    }

    public class BusquedaModel
    {
        public List<TipoInformacion> ListaSemanas { get; set; }
    }
}
