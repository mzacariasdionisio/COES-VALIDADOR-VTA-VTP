using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace COES.Servicios.Aplicacion.PMPO.Helper
{
    public class ConstantesPMPO
    {
        public const string PasswordTemplateExcel = "@C035-S1N4C-5T5.";
        public const int RowEmpresaExtranet = 2;
        public const int RowMesExtranet = 4;
        public const int RowFormatoExtranet = 5;
        public const int RowUnidadExtranet = 7;

        public const int ColEmpresaExtranet = 3;
        public const int ColMesExtranet = 3;
        public const int ColFormatoExtranet = 3;
        public const int ColUnidadExtranet = 3;

        public const int ColHandsonFinReadonly = 6;
        public const int RowIniData = 6;
        public const int ColIniData = 8;

        public const int ModuloPMPO = 18;
        public const int ModuloHidrologia = 3;
        public const int AgrupcodiPmpo = 10;

        public const int NotificarApertura = 1;
        public const int PlantcodiNotificacionAperturaPmpo = 118;
        public const int PlantcodiNotificacionVencimientoPmpo = 119;
        public const int PlantcodiNotificacionPendienteExtranetHidrologia = 120;
        public const string RemitenteSPR = "Subdirección de Programación";
        public const string AnexoSPR = " – Anexo: 595";

        //web.config
        public const string KeyFlagPmpoEnviarNotificacion = "FlagPmpoEnviarNotificacion";
        public const string KeyFlagPmpoDirectorioDat = "DirectorioDat";

        public readonly static string CarpetaFileServerPmpo = ConfigurationManager.AppSettings["PathAppPMPO"].ToString();
        public const string SubcarpetaRptOsinergmin = "ReportesOsignermin/";
        public const string SubcarpetaEnvio = "Mensajes/";
        public const string SubcarpetaNotificacion = "Notificacion/";

        public const string FolderUpload = "Uploads\\";
        public const string FolderFormato = "Formato\\";
        public const string DirectorioTemporal = "ReportePMPO";
        public const string PlantillaSDDP = "PlantillaResultadosSDDP.xlsx";
        public const string FolderDat = "PmpoDatTmp\\";
        public const string FolderCsv = "PmpoCsvTmp\\";

        public const string FolderReporte = "Areas/PMPO/Reporte/";

        public const int EmprcodiCoes = 1;
        public const int FormatoPMPO = 100;
        public const int FormatoValidacionPMPO = 138;
        public static List<int> ListadoFormatcodiPMPO = new List<int>() { 73, 74, 75, 76, 77, 78 };
        public static List<int> ListadoFormatcodiPMPOIntranet = new List<int>() { 135, 130, 131, 132, 133, 134 };

        public const int HojaOrdenHistorico = 1;
        public const int HojaOrdenPronostico = 2;

        public const int EstadoRealizadoExtranet = 1;
        public const int EstadoPendienteExtranet = 2;

        public const int EstadoDerivadoIntranet = 1;
        public const int EstadoSinDerivarIntranet = 2;

        public const int EstadoCumpEnPlazo = 1;
        public const int EstadoCumpFueraPlazo = 2;

        public const int LectcodiExtranetHist = 104;
        public const int LectcodiExtranetPron = 105;
        public const int LectcodiIntranetHist = 114;
        public const int LectcodiIntranetPron = 115;

        public const int TipoRangoPrevioHist = 0;
        public const int TipoRangoHist = 1;
        public const int TipoRangoPron = 2;
        public const int TipoRangoPronFuturo = 3;

        //parámetros de plazos
        public const int ConfpmcodiPlazoIni = 9;
        public const int ConfpmcodiPlazoFin = 8;
        public const int ConfpmcodiFueraPlazoIni = 10;

        public const string FormatoHidraulicoSemanal = "H";
        public const string FormatoTermicoSemanal = "T";
        public const string FormatoHidraulicoMensual = "HM";
        public const string FormatoTermicoMensual = "TM";

        //parámetros de correo
        public const int ConfpmcodiAsuntoVencimientoPlazo = 7;

        #region PMPO_2022
        public const int OrigenListado = 1; //accion procedente del listado de anios
        public const int OrigenPopup = 2; //accion procedente del popup de versiones de anios

        public const int AccionCrear = 1;
        public const int AccionEditar = 2;
        public const int AccionVerDetalles = 3;
        public const int AccionEliminar = 4;

        public const int EstadoActivo = 1;
        public const int EstadoInactivo = 0;

        public const int EstadoProcesado = 1;
        public const int EstadoSinProcesar = 0;

        //ARCHIVOS .DAT
        public const int ColumnaGmasSize = 6;
        public const int TipoDuvame = 1;
        public const int TipoDuvase = 2;
        public const string NombreRteSemanal = "duvase05.dat";
        public const string NombreRteMensual = "duvame05.dat";

        //CAUDALES
        public const string HidrologiaFinal = "16";
        public const string ProgramaMedianoPlazo = "22";
        public const string Activo = "A"; //para filtro de listado puntos hidro final y sddp

        public const string EstadoActivo_ = "A";
        public const string EstadoBaja_ = "B";
        public const string EstadoEliminado_ = "X";
        public const string ValueSiCoes = "S";
        public const string ValueNoCoes = "N";
        public const int ValorXDefecto = -1;

        public const string NombreArchivoZip = "htopol_w_extnam.zip";
        public const string NombreDat_htopol_w = "htopol_w.dat";
        public const string NombreDat_extnam = "extnam.dat";
        public const int Colum_htopol_w_Orden = 5;
        public const int Colum_htopol_w_Codigo = 10;
        public const int Colum_htopol_w_Nombre = 34;
        public const int Colum_htopol_w_Cero = 2;

        public static List<int> ListadoFormatcodiHidroExtranet = new List<int>() { 34, 33, 36, 35 };

        //SERIES BASE
        public const int EsOficial = 1;
        public const int Noficial = 0;
        public const int EstadoEliminado = 1;

        public const int SerieBaseSemanal = 1;
        public const int SerieBaseMensual = 2;
        public const int SerieHidroSemanal = 3;
        public const int SerieHidroMensual = 4;

        public const int InformacionHistorico = 1;
        public const int InformacionPronostico = 2;

        //SERIE HIDROLOGICA
        public const int OrigenBase = 1;
        public const int OrigenHistorico = 2;
        public const int OrigenPronostico = 3;
        public const int Autocompletar = 4;
        public const int EditadaPorUsuario = 5;
        public const string DatSerieHidroSemanal = "hinflw_w.dat";
        public const string DatSerieHidroMensual = "hinflw.dat";

        //ARCHIVOS EXCEL
        public const string PlantillaSerieBaseSemanal = "Caudal Semanal";
        public const string PlantillaSerieBaseMensual = "Caudal Mensual";
        public const string NombreHojaSerieBaseSemanal = "Arqs";
        public const string NombreHojaSerieBaseMensual = "Base";
        public const string ExtensionFile = "xlsx";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";
        public const int ColFecha = 1;
        public const int RowSddp = 1;

        public const string ValoresNegativos = "Contiene valores negativos.";
        public const string ValoresNoNumericos = "Contiene valores no numéricos";
        public const string ValoresVacios = "Contiene valores Vacíos";

        //CENTRALES SDDP
        public const string SesionIdTopologia = "SesionTopologia";

        public const int TPtoMedVolUtil = 7; //unido a index
        public const int TPtoMedVolTotal = 74;//unido a index

        public const string HorizonteSemanal = "S";
        public const string HorizonteMensual = "M";

        public const int FuenteSemanal = 1;
        public const int FuenteMensual = 2;

        public const int EscenarioBase = 0;
        public const int CategoriaCentralHidroelectrica = 1;
        public const int RelacionTurbinamiento = 1;
        public const int RelacionVertimiento = 2;
        public const int OpcionSi = 1;
        public const int OpcionNo = 0;
        public const int ParaWeb = 1;
        public const int ParaExcel = 2;
        public const int ParaDetalles = 3;
        public const int ECentralPasada = 0;
        public const int EAlmacenamiento = 1;

        public const int TamLetraCabecera = 10;
        public const int TamLetraCuerpo = 9;
        public const string TipoLetraCabecera = "Arial";
        public const string TipoLetraCuerpo = "Arial";
        public const string ColorCabecera = "#2980B9";
        public const string ColorBorde = "#B0AEB0";

        public const int PropFlagCHSiCoes = 1;
        public const int PropFlagCHTipo = 2; //Existente:0, Futura:1
        public const int PropFlagCHConexion = 3;//Paralelo:0, Cascada:1
        public const int PropPotencia = 4;
        public const int PropCoefProduccion = 5;
        public const int PropCaudalMinT = 6;
        public const int PropCaudalMaxT = 7;
        public const int PropFactorIndForzada = 8;
        public const int PropFactorIndHistorica = 9;
        public const int PropCostoOM = 10;
        public const int PropFlagTipoEmbalse = 11; //CentralPasada:0, Almacenamiento:1
        public const int PropFactorEmpuntamiento = 12;
        public const int PropFlagESiCoes = 13;
        public const int PropVolMin = 14;
        public const int PropVolMax = 15;
        public const int PropTipoVertimiento = 16;
        public const int PropFlagAdicionarVolMin = 17;
        public const int PropFlagAjustarVolMin = 18;
        public const int PropFlagAjustarVolIni = 19;
        public const int PropCoefEvap1 = 20;
        public const int PropCoefEvap2 = 21;
        public const int PropCoefEvap3 = 22;
        public const int PropCoefEvap4 = 23;
        public const int PropCoefEvap5 = 24;
        public const int PropCoefEvap6 = 25;
        public const int PropCoefEvap7 = 26;
        public const int PropCoefEvap8 = 27;
        public const int PropCoefEvap9 = 28;
        public const int PropCoefEvap10 = 29;
        public const int PropCoefEvap11 = 30;
        public const int PropCoefEvap12 = 31;
        public const int PropArea1 = 32;
        public const int PropVolumen1 = 33;
        public const int PropArea2 = 34;
        public const int PropVolumen2 = 35;
        public const int PropArea3 = 36;
        public const int PropVolumen3 = 37;
        public const int PropArea4 = 38;
        public const int PropVolumen4 = 39;
        public const int PropArea5 = 40;
        public const int PropVolumen5 = 41;
        public const int PropDefluenciaTotMin = 42;
        public const int PropIndicadorEA = 43;
        public const int PropVolumenTotalInicial = 117;

        //DAT: CHIDROPE MHIDROPE
        public const int PrioridadChidrope = 1;
        public const int PrioridadMhidrope = 2;
        public const string NombreDatSalidaHidroZip = "ArchivosSalidaHidro.zip";
        public const string NombreDat_Chidrope = "chidrope.dat";
        public const string NombreDat_mHidrope = "mhidrope.dat";
        public const int TipoChidrope = 1;
        public const int TipoMhidrope = 2;
        public const int ColumNum = 4;
        public const int ColumNombre = 12;
        public const int ColumPV = 4;
        public const int ColumVAA = 4;
        public const int ColumTAA = 4;

        //TIPO RELACIÓN: VIERTE/TURBINA
        public const int IdRelVierte = 2;
        public const int IdRelTurbina = 1;

        //MODIFICACION CENTRAL SDDP
        public const string ResolucionMoidif = "1";
        public const string ResolucionBase = "0";
        public const int IdTopologiaBase = 0;
        public const int IdTopologiaModificacion = 1;

        //ESCENARIO SDDP
        public const string ResolucionSemanal = "S";
        public const string ResolucionMensual = "M";
        public const int EscenarioOficial = 1;

        public static List<int> ListadoFormatcodiHidroExtranetVolumen = new List<int>() { 18, 21, 24, 27, 28, 51 };
        public static List<int> ListadoTptocodiHidroExtranetVolumen = new List<int>() { 7, 11 };

        #endregion

        public const string SesionEmpresas = "SesionEmpresaPMPO";
        public const string FormatoDecimalNET = "0.00";

        //Tipo de Evento del log, 1: Inicio, 2: fin, 3: Correcto, 4: Error
        public const int TipoEventoLogInicio = 1;
        public const int TipoEventoLogFin = 2;
        public const int TipoEventoLogCorrecto = 3;
        public const int TipoEventoLogError = 4;
        public const int TipoEventoLogAlerta = 5;

        public const string TipoEventoLogInicioDesc = "INICIO";
        public const string TipoEventoLogFinDesc = "FIN";
        public const string TipoEventoLogCorrectoDesc = "CORRECTO";
        public const string TipoEventoLogErrorDesc = "ERROR";

        //Tipos de codigos SDDP
        public const int TsddpCaudalNatural = 1;
        public const int TsddpPlantaTermica = 2;
        public const int TsddpPlantaHidraulica = 3;
        public const int TsddpRER = 4;
    }

    public class PmpoDataXEmpresas
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int Hoja { get; set; }
        public List<int> listaHoja { get; set; }
        public List<string[][]> listaData { get; set; }
    }

    public class PmpoSemana
    {
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime Fecha1Mes { get; set; }
        public int NroSemana { get; set; }
        public int Etapa { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string SemanaDesc { get; set; }
        public int TotalHoras { get; set; }
    }

    public class PmpoBloqueHorario
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime Fecha1Mes { get; set; }
        public int NroSemana { get; set; }
        public int NroBloque { get; set; }
        public decimal Horas { get; set; }
        public string SemanaDesc { get; set; }
        public string BloqueDesc { get; set; }
        public int TipoRango { get; set; }

        public List<MeMedicionxintervaloDTO> ListaDato { get; set; } = new List<MeMedicionxintervaloDTO>();

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    /// Clase para notificación a agentes
    /// </summary>
    public class PmpoNotificacion
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int Formatcodi { get; set; }
        public string Formatnombre { get; set; }
        public DateTime Fecha1Mes { get; set; }
        public string ToEmails { get; set; }
        public List<string> ListaToEmails { get; set; }
        public string MensajeApp { get; set; }

        public bool TieneError { get; set; }
        public string MensajeResultado { get; set; }

        public string AsuntoCorreo { get; set; }

        public string HtmlCuerpo { get; set; }
    }

    public class PmpoFile
    {
        public string TmpFileName { get; set; }
        public string FileName { get; set; }

        public int Tipoinfocodi { get; set; }
        public int Tptomedicodi { get; set; }
        public string Descripcion { get; set; }

        public decimal FilePorcentaje { get; set; }
        public int TamanioKB { get; set; }

        public bool EsArchivoDat { get; set; }
        public bool EsArchivoCsv { get; set; }
        public bool EsArchivoCsvDefault { get; set; }
        public bool EsArchivoCsvVolInicial { get; set; }
        public bool EsArchivoCsvDemanda { get; set; }
    }

    public class PuntosSDDP
    {
        public int Ptomedicodi { get; set; }
        public int PtoXEstacionCodi { get; set; }
        public string NombrePto { get; set; }
        public string Descripcion { get; set; }
        public decimal Factor { get; set; }

        public List<int> ListaFormatcodi { get; set; }
        public string Formatnombre { get; set; }
    }

    public class PmpoHIDetalle : ICloneable
    {
        public int Sddpcodi { get; set; }
        public List<int> ListaEquicodi { get; set; } = new List<int>();
        public string ListaEquicodiStr { get; set; } = "";
        public string ListaEquiabrevStr { get; set; } = "";
        public List<PmoConfIndispEquipoDTO> ListaCorrelaciones { get; set; } = new List<PmoConfIndispEquipoDTO>();

        public DateTime Horaini { get; set; }
        public DateTime Horafin { get; set; }
        public decimal HorasTotal { get; set; }
        public decimal HorasIndisp { get; set; }
        public int? Min { get; set; }
        public string PmCindConJuntoEqp { get; set; }

        public int Anio { get; set; }
        public int NroSemana { get; set; }
        public decimal PmCindPorrcentaje { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class PmpoHITotal : ICloneable
    {
        public int Sddpcodi { get; set; }
        public string Sddpnomb { get; set; }
        public int Sddpnum { get; set; }
        public List<int> ListaEquicodi { get; set; } = new List<int>();
        public string ListaEquicodiStr { get; set; } = "";
        public int Grupocodimodo { get; set; }

        public string PmCindConJuntoEqp { get; set; }

        public int Anio { get; set; }
        public int Mes { get; set; }
        public int NroSemana { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }

        public decimal HoraTotal { get; set; }
        public decimal HoraIndisp { get; set; }
        public decimal PorcentajeDisp { get; set; }
        public List<PmpoHIDetalle> ListaDetXSem { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class PmpoProcesamientoDat
    {
        public int Tsddpcodi { get; set; }
        public string TipoFormato { get; set; }

        public List<PmoSddpCodigoDTO> ListaCodigo { get; set; }
        public List<EveManttoDTO> ListaManttos { get; set; }

        public List<PmpoHITotal> ListaDisp { get; set; }
        public List<PmoDatPmhiTrDTO> ListaResultado { get; set; }
    }

    public class GridExcelModel
    {
        /// <summary>
        /// Model para el manejo de grillas tipo excel
        /// </summary>

        public string[] Headers { get; set; }       //colHeaders : Setea verdadero o falso para activar o desactivar los encabezados de las columnas por defecto ( A, B , C ). También puede definir una matriz [' One' , ' Dos ', ' Tres ' , ...] o una función para definir las cabeceras. Si una función se establece el índice de la columna se pasa como parámetro.
        public int[] Widths { get; set; }           //colWidths : Define el ancho de la columna en píxeles. Acepta número, cadena ( que se convertirá en número) , matriz de números (si desea definir el ancho de columna por separado para cada columna ) o una función (si desea ajustar el ancho de columna dinámicamente en cada render )
        public object[] Columnas { get; set; }      //columns : Define las propiedades de las celdas y y los datos para ciertas columnas . Aviso: El uso de esta opción establece un número fijo de columnas ( Opciones startCols , minCols , maxCols serán ignoradas ) .
        public string[][] Data { get; set; }        //data : Fuente de datos inicial que se une a la red de datos por cuadrícula de referencia (datos de edición altera la fuente de datos . Ver Entendimiento vinculante como referencia

        public int FixedColumnsLeft { get; set; }   //Permite especificar el número de columnas fijas ( congelado ) en el lado izquierdo de la tabla
        public int FixedRowsTop { get; set; }       //Permite especificar el número de filas fijos ( congelado ) en la parte superior de la tabla

        public const string TipoTexto = "text";
        public const string TipoNumerico = "numeric";
        public const string TipoFecha = "date";
        public const string TipoLista = "dropdown";
        public const string TipoAutocompletar = "autocomplete";

        //Lista de Objetos complementarios
        public object[] ListaEmpresas { get; set; }
        public object[] ListaURS { get; set; }
        public object[] ListaBarras { get; set; }
        public object[] ListaCentralGeneracion { get; set; }
        public object[] ListaSistemasTrans { get; set; }
        public object[] ListaUnidadesGeneracion { get; set; }
        public string[] ListaTipoUsuario { get; set; }
        public string[] ListaCalidad { get; set; }
        public object[] Repartos { get; set; }
        public string[] Empresas { get; set; }
        public double[] Porcentajes { get; set; }
        public double[] Totals { get; set; }
        public string[] ListaLicitacion { get; set; }
        public object[] ListaPeajeIngreso { get; set; }
        public object[] ListaSistemaTrans { get; set; }
        public object[] ListaCentralesGen { get; set; }
        public object[] ListaCompensacion { get; set; }
        public object[] ListaSistemasTransmision { get; set; }

        //Otros objetos
        public string sMensaje { get; set; }
        public int CantidadEmpresas { get; set; }
        public int NumeroColumnas { get; set; }
        public bool Grabar { get; set; }
        public int RegError { get; set; }
        public string MensajeError { get; set; }
    }

    public class PmpoReporteCsv
    {
        public List<PmpoSemana> ListaSemana { get; set; }

        public List<MeMedicionxintervaloDTO> ListaGenHidra { get; set; }
        public List<MeMedicionxintervaloDTO> ListaGenTermo { get; set; }
        public List<MeMedicionxintervaloDTO> ListaGenReno { get; set; }
        public List<MeMedicionxintervaloDTO> ListaCostMargi { get; set; }
        public List<MeMedicionxintervaloDTO> ListaCostOpera { get; set; }
        public List<MeMedicionxintervaloDTO> ListaDeficitPotencia { get; set; }
        public List<MeMedicionxintervaloDTO> ListaDeficitEnergia { get; set; }
        public List<MeMedicionxintervaloDTO> ListaConsCombu { get; set; }
        public List<MeMedicionxintervaloDTO> ListaEnergTipoGen { get; set; }
        public List<MeMedicionxintervaloDTO> ListaCaudalVertido { get; set; }
        public List<MeMedicionxintervaloDTO> ListaVoluDesc { get; set; }

        public List<MePtomedicionDTO> ListaPtoCostMargi { get; set; }
        public List<MePtomedicionDTO> ListaPtoVoluDesc { get; set; }
        public List<MePtomedicionDTO> ListaPtoDeficitPotencia { get; set; }
        public List<MePtomedicionDTO> ListaPtoDeficitEnergia { get; set; }

        public List<PmpoBloqueHorario> ListaBloqueCostMargi { get; set; }
        public List<PmpoBloqueHorario> ListaBloqueVoluDesc { get; set; }
        public List<PmpoBloqueHorario> ListaBloqueDeficitPotencia { get; set; }
        public List<PmpoBloqueHorario> ListaBloqueDeficitEnergia { get; set; }
    }

    #region PMPOe3 Central Sddp

    public class FormatoPtoMedicion
    {
        public string Codigo { get; set; }
        public int CodigoFormato { get; set; }
        public int CodigoPtoMedicion { get; set; }
        public string Nombre { get; set; }

    }

    public class EstacionHidroAsociada
    {
        public int CodigoEstacionHA { get; set; }
        public string NombreEstacionHA { get; set; }

    }

    public class CentralHidroelectrica
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal? Factor { get; set; }
    }

    public class Embalse
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal? Factor { get; set; }
        public int TipoVolumen { get; set; }
        public string TipoVolumenDesc { get; set; }
        public int FormatoSemanal { get; set; }
        public string FuenteSemanalDesc { get; set; }
        public int PtoSemanal { get; set; }
        public int FormatoMensual { get; set; }
        public string FuenteMensualDesc { get; set; }
        public int PtoMensual { get; set; }
    }

    public class CoeficienteEvaporacion
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal? Dato { get; set; }
    }
    public class VolumenArea
    {
        public int Codigo { get; set; }
        public int NumPar { get; set; }
        public decimal? Volumen { get; set; }
        public decimal? Area { get; set; }

    }

    public class CentralSddp
    {
        public int CodigoCentral { get; set; }
        public int CodigoTopologia { get; set; }
        public int Orden { get; set; }
        public int? CodigoSddp { get; set; }
        public int? NumSddp { get; set; }
        public string NombreSddp { get; set; }
        public string Descripcion { get; set; }
        public int CodigoEstacionHidro { get; set; }
        public string EstacionHidroDesc { get; set; }
        public int? CodigoTurbinamiento { get; set; } //acepta valores nulos
        public string TurbinamientoDesc { get; set; }
        public int? CodigoVertimiento { get; set; } //acepta valores nulos
        public string VertimientoDesc { get; set; }

        public int CentralHidroSiCoes { get; set; } //No:0, SI:1
        public List<CentralHidroelectrica> ListaCentralesHidro { get; set; }
        public string CentralesHidroDesc { get; set; }
        public int TipoCentralHidro { get; set; } //Existente:0, Futura:1
        public int TipoConexionCentralHidro { get; set; } //Paralelo:0, Cascada:1
        public decimal? Potencia { get; set; }
        public decimal? PotenciaDefectoParalelo { get; set; }
        public decimal? PotenciaDefectoCascada { get; set; }
        public decimal? CoefProduccion { get; set; }
        public decimal? CoefProduccionDefectoParalelo { get; set; }
        public decimal? CoefProduccionDefectoCascada { get; set; }
        public decimal? CaudalMinTurbinable { get; set; }
        public decimal? CaudalMaxTurbinable { get; set; }
        public decimal? CaudalMaxTurbinableDefectoParalelo { get; set; }
        public decimal? CaudalMaxTurbinableDefectoCascada { get; set; }
        public decimal? FactorIndisForzada { get; set; }
        public decimal? FactorIndisHistorica { get; set; }
        public decimal? CostoOM { get; set; }
        public decimal? CostoOMDefectoParalelo { get; set; }
        public decimal? CostoOMDefectoCascada { get; set; }

        public int TipoEmbalse { get; set; } //CentralPasada:0, Almacenamiento:1
        public decimal? FactorEmpuntamiento { get; set; }
        public int EmbalseSiCoes { get; set; } //No:0, SI:1
        public List<Embalse> ListaEmbalses { get; set; }
        public string EmbalsesDesc { get; set; }
        public decimal? VolumenMin { get; set; }
        public decimal? VolumenMax { get; set; }
        public int? TipoVertimiento { get; set; } //No controlable:2, Controlable:1, Parcialmente controlable:3
        public int? AdicionarVolMin { get; set; } //No:0, Si:1
        public int? AjustarVolMin { get; set; } //No:0, Si:1
        public int? AjustarVolIni { get; set; } //No:0, Si:1
        public List<CoeficienteEvaporacion> ListaCoefEvaporacion { get; set; }
        public List<VolumenArea> ListaVolumenArea { get; set; }

        public DateTime FechaModificacion { get; set; }
        public string FechaModificacionDesc { get; set; }
        public string UsuarioModificacion { get; set; }
    }

    public class EscenarioSddp
    {
        public int Codigo { get; set; }
        public int NumVersion { get; set; }
        public string Nombre { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }
        public int Resolucion { get; set; }
        public string ResolucionDesc { get; set; }
        public int Identificador { get; set; }
        public string IdentificadorDesc { get; set; }

        public string FechaModificacionDesc { get; set; }
        public string UsuarioModificacion { get; set; }

    }
    #endregion

    #region Modificaciones De Centrales 

    //public class ModificacionCentralSDDP
    //{
    //    public int CodigoModificacion { get; set; }
    //    public int Orden { get; set; }
    //    public int CodigoSddp { get; set; }
    //    public string NombreSddp { get; set; }
    //    public DateTime FechaFutura { get; set; }
    //    public decimal? TurbinamientoMIn { get; set; }
    //    public decimal? DefluenciaTotMin { get; set; }
    //    public decimal? FactorProduccion { get; set; }
    //    public decimal? VolumenMax { get; set; }
    //    public decimal? Potencia { get; set; }
    //    public int ICP { get; set; }
    //    public int IH { get; set; }
    //    public int IndicadorEA { get; set; }
    //    public DateTime FechaModificacion { get; set; }
    //    public string UsuarioModificacion { get; set; }

    //    public string FechaFuturaDesc { get; set; }
    //    public string FechaModificacionDesc { get; set; }
    //}

    #endregion

    #region Volumen Embalse

    /// <summary>
    /// Clase handson del volumen inicial
    /// </summary>
    public class PmpoVolumenEmbalse
    {
        public string Emprnomb { get; set; }
        public int CodigoSddp { get; set; }
        public int Mrecurcodi { get; set; }
        public string NombreSddp { get; set; }

        public int Equicodi { get; set; }
        public string Embalse { get; set; }

        public int Fila { get; set; }

        public decimal? FactorK { get; set; }
        public decimal? FactorEmb { get; set; }
        public decimal? VolIniXEmb { get; set; }
        public decimal? VolMaxCentral { get; set; }
        public decimal? VolMinCentral { get; set; }
        public decimal? VolIniXCentral { get; set; }
        public decimal? VolIniPuXCentral { get; set; }

        public decimal? VolIniXEmbBD { get; set; }
        public decimal? VolIniXEmbExtranetBD { get; set; }
        public decimal? VolIniPuXCentralBD { get; set; }

        public decimal InputVmin { get; set; }
        public decimal InputVmax { get; set; }
        public decimal InputM { get; set; }
        public decimal InputFlagPuDefault { get; set; }
        public decimal InputFlagVolInicialDefault { get; set; }
    }

    #endregion

}
