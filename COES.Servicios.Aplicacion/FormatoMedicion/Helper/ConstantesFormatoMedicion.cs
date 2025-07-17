using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.PMPO.Helper;
using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using static COES.Dominio.DTO.Sic.EqEquipoDTO;

namespace COES.Servicios.Aplicacion.FormatoMedicion
{
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
        public const int IdFormatoDespachoIDCC = 62;
        public const int IdFormatoGeneracionDespachoDiario = 125;
        public const int IdFormatoCaudalRDO = 123;

        public static List<int> ListadoFormatcodiExtranetTrCumpl = new List<int>() { 6, 5, 32, 7, 43 , 62 };

        #endregion

        public const int AplicativoCompHidraulico = 5;
        public const string AplicativoCompHidraulicoListaFormato = "127,128";

        public const int AplicativoPMPO = 6;
        public const string AplicativoPMPOListaFormato = "73,74,75,76,77,78,130,131,132,133,134,135";
    }

    public class CeldaMerge
    {
        public int col { get; set; }
        public int row { get; set; }
        public int colspan { get; set; }
        public int rowspan { get; set; }
        public int rowAbsoluto { get; set; }
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
        public string[] FuenteDato { get; set; } //PrimasRER.2023
    }

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

    }
    
    public class CeldaCambios
    {
        public int Col { get; set; }
        public int Row { get; set; }
    }

    public class ListaSelect
    {
        public int id { get; set; } //se requiere campo en minuscula
        public string text { get; set; } //se requiere campo en minuscula
        public string codigo { get; set; }
        public decimal? monto { get; set; }
    }

    /// <summary>
    /// Parametros para el formato
    /// </summary>
    public class ParamFormato
    {
        public const int TotalMinDia = 24 * 60;
        public const int RowDatos = 18;
        public const int RowTitulo = 9;
        public const int RowCodigo = 1;
        public const int RowArea = 6;
        public const int ColDatos = 2;
        public const int ColFecha = 1;
        public const int ColEmpresa = 150;
        public const int ColFormato = 151;

        public const int ColEmpresaNuevo = 1;
        public const int ColFormatoNuevo = 1;
        public const int RowDatosSH = 12;
    }

    /// <summary>
    /// Parametros para el formato
    /// </summary>
    public class ParamFormatoSH
    {
        public const int RowTitulo = 4;
        public const int RowCuenca = 6;
        public const int RowEmpresa = 7;
        public const int RowEmbalseRio = 8;
        public const int RowPuntoMedicion = 9;
        public const int RowAnio = 13;
        /*public const int TotalMinDia = 24 * 60;
        public const int RowDatos = 18;
        public const int RowCodigo = 1;
        public const int RowArea = 6;
        public const int ColDatos = 2;
        public const int ColFecha = 1;
        public const int ColEmpresa = 150;
        public const int ColFormato = 151;

        public const int ColEmpresaNuevo = 1;
        public const int ColFormatoNuevo = 1;*/
    }

    /// <summary>
    /// Constantes para el manejo de handson
    /// </summary>
    public class HandsonConstantes
    {
        public const int ColWidth = 145;
        public const int ColPorHoja = 7;
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
    /// Query string
    /// </summary>
    public class QueryParametros
    {
        public const char SeparadorFila = '#';
        public const char SeparadorCol = ',';
    }

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

    public class ParametrosEnvio
    {
        /// <summary>
        /// estado 1 cuando se crea la cabecera, luego se cambia a 3 cuando se guarda en las tablas de MeMedicionX
        /// </summary>
        public const byte EnvioEnviado = 1;
        public const byte EnvioProcesado = 2;
        public const byte EnvioAprobado = 3;
        public const byte EnvioRechazado = 4;
        public const byte EnvioFueraPlazo = 5;       
    }

    public class TipoInformacion
    {
        public int IdTipoInfo { get; set; }
        public string NombreTipoInfo { get; set; }
        public string Valor2 { get; set; }
    }

    public class TipoInformacion2
    {
        public char IdTipoInfo { get; set; }
        public string NombreTipoInfo { get; set; }
    }

    public class ManttoBloque
    {
        public int Equicodi { get; set; }
        public int Bloque { get; set; }
        public List<int> ListaManto { get; set; }
    }

    #region SIOSEIN
    public class ListaSelect24
    {
        public int id { get; set; } //se requiere campo en minuscula
        public string codigo { get; set; } //se requiere campo en minuscula
        public string text { get; set; } //se requiere campo en minuscula
        public decimal? monto1 { get; set; }
        public decimal? monto2 { get; set; }
        public decimal? monto3 { get; set; }
        public decimal? monto4 { get; set; }
    }
    #endregion


    #region formato Models

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
        public string FechaHoy { get; set; }
        public int NroSemana { get; set; }
        public string StrFormatCodi { get; set; }
        public string StrFormatPeriodo { get; set; }
        public string StrFormatDescrip { get; set; }
        public MeFormatoDTO Formato { get; set; }
        public List<MeHojaptomedDTO> ListaHojaPto { get; set; }
        public List<MeEnvioDTO> ListaEnvios { get; set; }
        public List<SHCaudalDTO> ListaCaudal { get; set; }
        public List<CeldaCambios> ListaCambios { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EquipoDTO> ListaCuenca { get; set; }
        public List<EqEquipoDTO> ListadoCuenca { get; set; }
        public List<TipoSerie> ListaTipoSerie { get; set; }
        public List<TipoPuntoMedicion> ListaTipoPuntoMedicion{ get; set; }
        public List<MeLecturaDTO> ListaLectura { get; set; }
        public List<MeFormatoDTO> ListaFormato { get; set; }
        public List<int> ListaOpcionRibbon { get; set; }
        public List<FwAreaDTO> ListaAreas { get; set; }
        public List<string> ListaSemanas { get; set; }
        public List<GenericoDTO> ListaSemanas2 { get; set; }
        public List<ListaSelect> ListaPtoMedicion { get; set; }
        public int FilasCabecera { get; set; }
        public List<int> ListaFilasOcultas { get; set; }
        public List<int> ListaColCheck { get; set; }
        public List<int> ListAnio { get; set; }
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
        public List<ManttoBloque> ListaBloqueMantos { get; set; }
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
        public List<ReporteHoraoperacionDTO> ListaHOP { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string DetalleMensaje { get; set; }

        public int ViewHojaPto2 { get; set; }
        //agregado de la extranet
        public int IdFormatoNuevo { get; set; }
        public List<ItemPanelIEOD> ListaPanelIEOD { get; set; }
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
        #region Mejoras IEOD
        public string Emprabrev { get; set; }
        public string NombreArchivoExcel { get; set; }
        #endregion


        public Boolean TieneDataScada { get; set; }

        //PMPO
        public int TipoinfocodiEnvio { get; set; }
        public string TipoinfoabrevEnvio { get; set; }
        public string PeriodoHoja { get; set; }
        public List<MeMedicionxintervaloDTO> ListaMxInt { get; set; }
        public List<MeMedicionxintervaloDTO> ListaMxIntAnioSig { get; set; }
        public List<PmpoBloqueHorario> ListaBloque { get; set; } = new List<PmpoBloqueHorario>();
        public List<PmoFeriadoDTO> ListaFeriado { get; set; } = new List<PmoFeriadoDTO>();
        public int NumMsjPendiente { get; set; }
        //Assetec - Pronóstico de la demanda Etapa 3
        public List<PrnMediciongrpDTO> ListaVersion { get; set; }

        public int FiltroBloqueHorario { get; set; }
        #region Mejoras RDO-II
        public int IdHorario { get; set; }
        #endregion
        public List<PrnVersiongrpDTO> ListaVersionByFecha { get; set; }
        
        //campos para reporte masivo de formatos
        public List<MePtomedicionDTO> ListaPtos { get; set; }

        public List<MePtomedicionDTO> ListaPtosMME { get; set; }

        public int TipoSerie { get; set; }

        public string FechaIni { get; set; }
        public string FechaFin { get; set; }

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
        //public List<Boolean> ListaColumReadOnly { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Boolean ReadOnly { get; set; }
        public string[][] ListaSourceDropDown { get; set; }
        public short[][] MatrizTipoEstado { get; set; }
        public int[][] MatrizDigitoDecimal { get; set; }
        public string[][] MatrizCeldaExcel { get; set; }

        public string[][] ListaExcelComment { get; set; }

        public int TieneData { get; set; }
        //agregado de la extranet
        /// <summary>
        /// almacena variable bool de readOnly de celda
        /// </summary>
        public bool[][] MatrizEstado { get; set; }
        public List<string> ListaDropDown { get; set; }
        //
        public int HMaximoData48Enviado { get; set; }
        public int HMaximoDataScadaDisponible { get; set; }

        public string Resultado { get; set; }
        public object[] Columnas { get; set; }
        public string[] Headers { get; set; }
        public List<CeldaCambios> ListaCambios { get; set; }
        public NestedHeaders NestedHeader { get; internal set; }

        public List<int> FilasActDefecto { get; set; }

        #region Mejoras RDO
        public string[][] ListaExcelDataEjecutados { get; set; }
        public string[][] ListaLimitesMinYupana { get; set; }
        public string[][] ListaLimitesMaxYupana { get; set; }
        public string[][] ListaExcelDataEjecutadosRER { get; set; }
        #endregion
        
        public int ColCabecera { get; set; }

        #region IND.PR25.2022
        public int MaxCols { get; set; }
        public int MaxRows { get; set; }
        #endregion
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
        public int Width { get; set; }
        public string Class { get; set; }
        public string Title { get; set; }
    }

    public class FormatoResultado
    {
        public int Resultado { get; set; }
        public int IdEnvio { get; set; }
        public string Mensaje { get; set; }
        public string DetalleMensaje { get; set; }
        public string Archivo { get; set; }

        //agregado de la extranet
        public string Detalle { get; set; }
        public string sAplicacion { get; set; }
        public int IdHojaPadre { get; set; }
    }

    //Solo usa la extranet para rpfcontrollers
    /// <summary>
    /// Estructura para manajear los datos medio horarios
    /// </summary>
    public class ComparativoItemModelrpf
    {
        public string Hora { get; set; }
        public decimal ValorRPF { get; set; }
        public decimal ValorDespacho { get; set; }
        public decimal Desviacion { get; set; }
    }

    //clase para leer la data del excel de importación de los formatos
    public class ValidacionFormato
    {
        public int TotalEmpresa { get; set; }
        public int Formatcodi { get; set; }
        public int PosIniHistorico { get; set; }
        public int PosFinHistorico { get; set; }
        public int PosIniPronostico { get; set; }
        public int PosFinPronostico { get; set; }
        public string Fechaperiodo { get; set; }
        public string[][] ListaData { get; set; }
        public List<string> ListaPuntos { get; set; }
    }

    /// <summary>
    /// Clase log de errores
    /// </summary>
    public class LogErrorFormato
    {
        public int Formato { get; set; }
        public string Fila { get; set; }
        public string Horas { get; set; }
        public string Bloque { get; set; }
        public string Sem { get; set; }
        public string Al { get; set; }
        public string Del { get; set; }
        public string Anio { get; set; }
        public string Observacion { get; set; }
        public string Punto { get; set; }
        public string ValorCelda { get; set; }
        public List<string[][]> ListaData { get; set; }
        public List<string> ListaPuntos { get; set; }
    }

    #endregion
    //
    #region Constantes Hidrología Carga Datos helper

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

        public const int AnioInicioSH = 1965;

        // Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual_usuario_Series_Hidrológicas.rar";
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        // Constantes para fileServer
        public const string FolderRaizHidrologiaModuloManual = "Series hidrológicas\\";
    }

    public class NombreArchivoHidrologiaCD
    {
        public const string PlantillaHidrologiaCD = "PlantillaHidrologia.xlsx";
        public const string PlantillaVolumenInicialCD = "PlantillaVolumenInicial.xlsx";
        public const string PlantillaSeriesHidrologicasCD = "PlantillaFormatoSH.xlsx";
        public const string NombreArchivoSeriesHidrologicasCD = "FormatoSH";
    }

    #endregion

    #region Constantes Mediciones Carga Datos helper

    public class ConstantesMedicionesCD
    {
        public const int FormatoDiarioCodi = 228;
        public const int FormatoReprogDiarioCodi = 229;
        public const int FormatoSemanalCodi = 230;
        public const string FolderUpload = "Areas\\Mediciones\\Uploads\\";
        public const string FolderReporte = "Areas\\Mediciones\\Reportes\\";
    }

    public class NombreArchivoMedicionesCD
    {
        public const string PlantillaMedicionesCD = "PlantillaMediciones.xlsx";
    }
    #endregion

    #region parámetros formato - Mediciones Carga Datos
    public class ParamMedicionesExcell
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
    #endregion
    //

    #region Constantes DemandaCP helper

    public class ConstantesDemandaCP
    {
        public const int FormatoDiarioCodi = 47;
        public const int FormatoSemanalCodi = 48;
        public const int EstacionHidrologica = 43;
        public const string FolderUpload = "Areas\\DemandaCP\\Uploads\\";
        public const string FolderReporte = "Areas\\DemandaCP\\Reportes\\";
    }

    /// <summary>
    /// Datos de sesion
    /// </summary>
    public class DatosSesionDemandaCP
    {
        public const string SesionFormato = "SesionFormato";
        public const string SesionIdEnvio = "SesionIdEnvio";
        public const string SesionFileName = "SesionFileName";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";
    }

    /// <summary>
    /// Mensajes de pantalla
    /// </summary>
    public class MensajesDemandaCP
    {
        public const string MensajeEnvioExito = "El envío se realizó correctamente";
    }

    /// <summary>
    /// Nombre de archivos
    /// </summary>
    public class NombreArchivoDemandaCP
    {
        public const string ExtensionFileUploadHidrologia = "xlsx";
        public const string FormatoProgDiario = "FormatoProgDiario.xlsx";
        public const string PlantillaDemandaCP = "PlantillaDemandaCP.xlsx";
        public const string PlantillaReporteHistorico = "PlantillaReporteHistorico.xlsx";
        public const string PlantillaReporteEnvio = "PlantillaReporteEnvio.xlsx";
        public const string ReporteEnvio = "ReporteEnvio.xlsx";
        public const string ReporteHistorico = "ReporteHistorico.xlsx";
        public const string NombreReporteHistorico = "Historico.xlsx";
        public const string NombreReporteEnvio = "Envio.xlsx";
    }

    #endregion

    #region Demanda Barras helper

    #endregion

    #region helper constantes

    /// <summary>
    /// Constantes utilizadas en la aplicacion
    /// </summary>
    public class ConstantesFormat
    {
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoHora = "HH:mm:ss";
        public const string FormatoHoraMinuto = "HH:mm";
        public const string FormatoMesAnio = "MM yyyy";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        public const string FormatoFechaFull = "dd/MM/yyyy HH:mm:ss";
        public const string FormatoFechaFullMs = "dd/MM/yyyy HH:mm:ss.fff";
        public const string FormatoFechaAnioMesDia = "yyyyMMdd";
        public const string FormatoOnlyHora = "HH:mm";
        public const string HoraInicio = "00:00:00";
        public const string FormatoNumero = "#,###.00";
        public const string FormatoDecimal = "###.00";
        public const int PageSize = 20;
        public const int PageSizeEvento = 50;
        public const int PageSizeMedidores = 200;
        public const int NroPageShow = 10;
        public const string HojaReporteExcel = "REPORTE";
        public const string HojaFormatoExcel = "FORMATO";
        public const string NombreReportePerturbacionWord = "Perturbacion.docx";
        public const string NombreReportePerturbacionPdf = "Perturbacion.pdf";
        public const string NombreReportePerfilScadaExcel = "Perfiles.xlsx";
        public const string NombrePlantillaImportacionPerfilExcel = "PlantillaImportacion.xlsx";
        public const string NombreFormatoImportacionPerfilExcel = "Importacion.xlsx";
        public const string NombreReporteRSF = "rsf.xlsx";
        public const string NombreReporteRSF30 = "rsf30.xlsx";
        public const string NombreReporteRSFGeneral = "rsf_reporte.xlsx";
        public const string PlantillaReportePerfilScadaExcel = "Plantilla.xlsx";
        public const string NombreCSVServicioRPF = "CumplimientoRPF.csv";
        public const string NombreLogoCoes = "coes.png";
        public const string PlantillaExcel = "Plantilla.xlsx";
        public const string PlantillaExcelRSF30 = "PlantillaRsf30.xlsx";
        public const string PlantillaExcelReporteRSF = "PlantillaReporteRSF.xlsx";
        public const string PlantillaCumplimiento = "PlantillaRPF.xlsx";
        public const string PlantillaCumplimientoFalla = "PlantillaRPFFalla.xlsx";
        public const string PlantillaPotencia = "PlantillaPotenciaRPF.xlsx";
        public const string NombreReporteCargaRPF = "Cumplimiento.xlsx";
        public const string NombreReporteCumplimientoRPF = "Evaluacion.xlsx";
        public const string NombreReporteCumplimientoRPFFalla = "EvaluacionFalla.xlsx";
        public const string ArchivoPotencia = "Potencia.xlsx";
        public const string ArchivoCmVsTarifa = "CmVsTarifa.xlsx";
        public const string ArchivoIncrementoReduccion = "IncrementoReduccion.xlsx";
        public const string ArchivoImportacionPerfiles = "ImportacionPerfil.xlsx";
        public const string ArchivoDesviacion = "Desviacion.xlsx";
        public const string NombreReporteRPFWord = "CumplimientoRPF.docx";
        public const string NombreReporteRPFFallaWord = "CumplimientoFallaRPF.docx";
        public const string NombreChartRPF = "ChartRPF.jpg";
        public const string NombreChartFallaRPF = "ChartFallaRPF.jpg";
        public const string NombreReporteRPFPotencia = "PotenciaMaximaRPF.xlsx";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string AppWord = "application/vnd.ms-word";
        public const string AppPdf = "application/pdf";
        public const string AppCSV = "application/CSV";
        public const string AppXML = "application/XML";
        public const string PaginaLogin = "home/login";
        public const string PaginaAccesoDenegado = "home/autorizacion";
        public const string DefaultControler = "Home";
        public const string LoginAction = "Login";
        public const string DefaultAction = "default";
        public const string RutaCarga = "Uploads/";
        public const int IdAplicacion = 1;
        public const string SI = "S";
        public const string NO = "N";
        public const string TextoSI = "Si";
        public const string TextoNO = "No";
        public const string TextoNoRango = "No rango";
        public const string TextoFPIncorrecto = "P o F en 0";
        public const string TextoNoEnvio = "No envío";
        public const string TextoNoSospechoso = "No sospechoso";
        public const string TextoRSFA = "RSF - A";
        public const string FormatoWord = "WORD";
        public const string FormatoPDF = "PDF";
        public const string AppZip = "application/zip";
        public const string FileZip = "Comprimido.zip";
        public const string AppGen = "application/dat";
        public const char CaracterComa = ',';
        public const char CaracterAnd = '&';
        public const char CaracterArroba = '@';
        public const string Coma = ",";
        public const string AperturaSerie = "[";
        public const string CierreSerie = "]";
        public const string EspacioBlanco = " ";
        public const char CaracterGuion = '-';
        public const string CaracterPorcentaje = "%";
        public const string CaracterCero = "0";
        public const char CaracterDosPuntos = ':';
        public const string CaracterH = "H";
        public const string CaracterComillaSimple = "'";
        public const char CaracterRaya = '_';
        public const char CaracterPunto = '.';
        public const char CaracterIgual = '=';
        public const string LogError = "INTRANET.COES";
        public const string NodoPrincipal = "Principal <span>/</span>";
        public const string SeparacionMapa = "<span>/</span>";
        public const string SufijoImagenUser = "@coes.org.pe.jpg";
        public const string UsuarioAnonimo = "usuario.anonimo";
        public const string ClaveAnonimo = "sgoCOES2014";
        public const string TextoCentral = "CENTRAL";
        public const string TextoMW = "MW";
        public const string ParametroDefecto = "-1";
        public const string Opero = "OPERÓ";
        public const string NoOpero = "NO OPERÓ";
        public const string Indeterminado = "INDETERMINADO";
        public const string ExtensionGif = "gif";
        public const string ExtensionJpg = "jpg";
        public const string ExtensionPng = "png";
        public const string ColorPlomo = "#CCCCCC";
        public const string NombreReporteMantenimiento = "RptMantenimiento.xlsx";
        public const string NombreReporteMantenimiento01 = "RptMantenimiento_01.xlsx";
        public const string NombreReporteMantenimiento02 = "RptMantenimiento_02.xlsx";
        public const string NombreReporteMantenimiento03 = "RptMantenimiento_03.xlsx";
        public const string NombreReporteMantenimiento04 = "RptMantenimiento_04.xlsx";
        public const string NombreReporteMantenimiento05 = "RptMantenimiento_05.xlsx";
        public const string NombreReporteMantenimiento06 = "RptMantenimiento_06.xlsx";
        public const string PlantillaExcelMantenimiento = "PlantillaMantenimiento.xlsx";
        public const int IdModulo = 3;
        public const int AccionEditar = 0;
        public const int AccionNuevo = 1;
        public const string EstadoActivo = "A";
        public const string EstadoEliminado = "E";
        public const string EstadoBaja = "B";
        public const string PlantillaCostosVariablesRepCv = "PlantillaCostosVariablesRepcv.xlsx";
        public const string PlantillaReporteCostosVariablesRepCv = "PlantillaReporteCostosVariables.xlsx";
        public const string PlantillaListadoEquipos = "PlantillaListaEquipos.xlsx";
        public const int PageSizeDemandaMaxima = 30;
        public const string RutaCargaIntranet = "IntranetTest/";
        public const string RutaCargaIntercambio = "IntercambioOsinergmin/";
        public const string InfografiaPortal = "INFOGRAFIA_{0}.";
        public const string UrlPortal = "http://www.coes.org.pe/appintranet/";
        public const string MensajeSesionExpirado = "La sesión del usuario ha expirado";
        public const string MensajePermisoNoValido = "Usted no tiene permisos para realizar esta acción.";
        public const string FormatoF = "dd_MM_yyyy";
        public const int EstacionHidrologica = 43;
    }

    #endregion

    #region Mejoras RDO
    public class ConstantesGeneracionDespachoRDO
    {
        public const string PlantillaGeneracionDespachoRDO = "PlantillaGeneracionDespacho.xlsx";
        public const string FolderUpload = "Areas\\GeneracionDespacho\\Uploads\\";
        public const string FolderReporte = "Areas\\GeneracionDespacho\\Reportes\\";
        public const int CodigoCabeceraDespacho_Intranet = 42;
    }
    #endregion
}
