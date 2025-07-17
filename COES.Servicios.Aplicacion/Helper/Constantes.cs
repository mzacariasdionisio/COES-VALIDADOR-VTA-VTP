using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Helper
{
    /// <summary>
    /// Constantes generales
    /// </summary>
    public class ConstantesAppServicio
    {
        public const string LogError = "ERROR.APLICACION";
        public const string NoExisteResultado = "NOEXISTE";
        public const string SI = "S";
        public const string NO = "N";
        public const string SIDesc = "SÍ";
        public const string NODesc = "NO";
        public const string Activo = "A";
        public const string Baja = "B";
        public const string Proyecto = "P";
        public const string Anulado = "X";
        public const string Eliminado = "E";
        public const string FueraCOES = "F";
        public const string ActivoDesc = "Activo";
        public const string BajaDesc = "Baja";
        public const string ProyectoDesc = "Proyecto";
        public const string AnuladoDesc = "Anulado";
        public const string EliminadoDesc = "Eliminado";
        public const string FueraCOESDesc = "Fuera de COES";
        public const string NoAgente = "NA";
        public const char CaracterComa = ',';
        public const string ParametroDefecto = "-1";
        public const string ParametroNoExiste = "-1000";
        public const string CaracterH = "H";
        public const string CaracterT = "T";
        public const string CaracterTENTDE = "TranEntrDetah";
        public const string CaracterCosMar = "CosMar";
        public const char CaracterNumeral = '#';
        public const char CaracterPorcentaje = '%';
        public const char CaracterArroba = '@';
        public const char CaracterGuion = '-';
        public const char CaracterDosPuntos = ':';
        public const string CaracterEnter = "\n";
        public const string ParametroOK = "1";
        public const string ParametroNulo = "-2";
        public const string FormatoFechaWS = "dd-MM-yyyy";
        public const string FormatoHora = "HH:mm";
        public const string FormatoFechaFull = "dd/MM/yyyy HH:mm";
        public const string FormatoFechaFull2 = "dd/MM/yyyy HH:mm:ss";
        public const string FormatoFechaHoraAMPM = "dd/MM/yyyy hh:mm tt";
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoFechaYYYYMMDD = "yyyy-MM-dd";
        public const string FormatoFechaDMY = "ddMMyy";
        public const string FormatoFechaYMD = "yyyy-MM-dd";
        public const string FormatoFechaYMD2 = "yyyyMMdd";
        public const string FormatoFechaYMDHora = "yyyyMMddHHmmss";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        public const string FormatoFechaEjecutivo = "dd.MM.yy";
        public const string FormatoFechaEjecutivo2 = "dd.MM.yyyy";
        public const string FormatoHHmmss = "HH:mm:ss";
        public const string FormatoFechaFullSeg = "dd/MM/yyyy HH:mm:ss";
        public const string FormatoFechaFullSegAMPM = "dd/MM/yyyy hh:mm:ss t";
        public const string FormatoDiaMes = "dd/MMM";
        public const string FormatoDiaMes2 = "ddMM";
        public const string FormatoOnlyHora = "HH:mm";
        public const string FormatoMes = "MM yyyy";
        public const string FormatoDia = "dd";
        public const string FormatoMesAnio = "MM-yyyy";
        public const string FormatoAnioMes = "yyyyMM";
        public const string FormatoAnio = "yyyy";
        public const string FormatoMesanio = "MM/yyyy";
        public const string FormatoAnioMesDia = "yyyyMMdd";
        public const string NameSistema = "NTCSE";
        public const int IdOrigenLectura = 16;
        public const int IdModulo = 3;
        public const int LectNroME24 = 24;
        public const int LectNroME1 = 1;
        public const int IdOrigenLectura2 = 18;
        public const int EstacionHidrologica = 43;
        public readonly static string EnlaceLogoCoes = ConfigurationManager.AppSettings["LogoCoes"].ToString();
        public readonly static string LogoCoesEmail = ConfigurationManager.AppSettings["LogoEmail"].ToString();
        public readonly static string LogoCoesEmailIntervenciones = ConfigurationManager.AppSettings["LogoEmailIntervenciones"].ToString();
        public const string NombreLogo = "LogoCOES";
        public const int ErrorPotMax = -996;
        public const int ErrorPotMin = -997;
        public const int ErrorPorcRPF = -998;
        public const int ErrorVarTer = -999;
        public const int ErrorURSNoConf = -995;
        public const string IndicadorNotificacionPR03 = "IndicadorNotificacionPR03";

        public const string VtranflagEntrega = "E";
        public const string VtranflagRetiro = "R";
        public const string IniDiaFecha = "01/";
        public const int FormatcodiSiosein = 83;
        public const int CtgcodiTipotecnologia = 4;

        public const string LectcodiProgSemanal = "3";
        public const string LectcodiProgDiario = "4";
        public const string LectcodiReprogDiario = "5";
        public const string LectcodiEjecutadoHisto = "6";
        public const string LectcodiEjecutado = "93";
        public const string LectcodiAjusteDiario = "7";
        public const int LectcodiProgDiariomcp = 107;
        public const int LectcodiExtranetEjec = 12;
        public const int LectcodiReservaAsigProg = 100;
        public const int LectcodiDespachoEjecDiaSinrsv = 101;

        public const int EvenclasecodiEjecutado = 1;
        public const int EvenclasecodiProgDiario = 2;
        public const int EvenclasecodiProgSemanal = 3;
        public const int EvenclasecodiProgMensual = 4;
        public const int EvenclasecodiProgAnual = 5;
        public const int EvenclasecodiAjusteDiario = 6;

        public const string FamcodiLinea = "8";
        public const string FamcodiSvc = "14";
        public const string Famcoditrafo2d = "9";
        public const string Famcoditrafo3d = "10";
        public const string FamcodiDemanda = "45,6,32";
        public const string FamcodiServiciosAux = "40";
        public const int FamcodiBess = 49;

        public const string ExtensionExcel = ".xlsx";
        public const string ExtensionWord = ".docx";
        public const string ExtensionDle = ".dle";
        public const string ExtensionCsv = ".csv";
        public const string PathArchivoExcel = "Temporales/";
        public const string PathScadaArchivoExcel = "Temporales\\";

        public const int TipoinfocodiMW = 1;
        public const int TipoinfocodiMVAR = 2;
        public const int TipoinfocodiKv = 5;
        public const int TipoinfocodiHZ = 6;
        public const int TipoinfocodiSoles = 7;
        public const int TipoinfocodiRsvFria = 100;
        public const int TipoinfocodiRsvRotante = 101;
        public const int TipoinfocodiMwXMantto = 102;
        public const int TipoinfocodiEfiGas = 109;
        public const int TipoinfocodiEfiCarbon = 110;
        public const int PtomedicodiCostoOpeDia = 1300;
        public const int TipogrupoNoIntegrante = 10;
        public const int TipogrupoRER = 2;
        public const int TipogrupoCOES = 1;
        public const string SubcausacodiCongestion = "201";
        public const string SubcausacodiOperacionCalderos = "202";
        public const string SubcausacodiRegulacionTension = "203";
        public const string SubcausacodiRestriccionesOpe = "205";
        public const string SubcausacodiSistemasAislados = "206";
        public const string SubcausacodiVenteoGas = "210";
        public const string SubcausacodiInterconexInterna = "219";
        public const string SubcausacodiDisponibilidadGas = "222";
        public const string SubcausacodiObservacionesIDCOS = "304";

        public const string CausaevencodiProgramado = "1";
        public const string FamnombHidro = "HIDROELÉCTRICO";
        public const string FamnombTermo = "TERMOELÉCTRICO";
        public const string FamnombSolar = "SOLAR";
        public const string FamnombEolic = "EOLICO";

        public const int OriglectcodiDespachomediahora = 2;
        public const int OriglectcodiFlujos = 17;

        public const int LectcodiDemandaxarea = 27;

        public const int PtomedicodiCmgCP = 1201;
        public const int PropcodiPotEfect = 46;
        public const int PropcodiPotMaximaGenHidro = 298;
        public const string PropcodiListaPotEfect = "46,164";

        public const int OriglectcodiMedidoresGene = 1;
        public const int LectcodiMedidoresGene = 1;

        public const int YacimientoGasCodi = 1;
        public const int OriglectcodiDemandaBarra = 6;
        public const int TipoinfocodiFlujo = 8;
        public const int TipoinfocodiM3s = 11;
        public const int Tipoinfocodim3 = 12;
        public const int TipoinfocodiGalones = 15;
        public const int Tipoinfocodim = 19;
        public const int TipoinfocodiDemanda = 20;
        public const int TipoinfocodiDolares = 21;
        public const int TipoinfocodiKg = 51;
        public const int PtomedicodiNivelDemanda = -1203;
        public const int PtomedicodiEscenario = 1203;
        public const int PtomedicodiDemandaNCP = 1221;
        public const int PtomedicodiOferta = 1222;
        public const int PtomedicodiPerdidasNcp = 1223;
        public const int PtomedicodiDemandaCentro = 1211;
        public const int PtomedicodiDemandaCoes = -1221;
        public const int PtomedicodiTotalCoGener = -1222;
        public const int PtomedicodiTotalRenovab = -1223;
        public const int PtomedicodiGeneracionCoes = -1224;
        public const int TipoinfocodiSegundos = 22;
        public const int PtomedicodiRsvRotante = 1195;
        public const int PtomedicodiRsvFria = 1216;
        public const int PtomedicodiRsvFriaTermica = 1227;
        public const int PtomedicodiRsvFriaHidraulica = 1228;
        public const int PtomedicodiRsvEficiente = 1205;
        public const int PtomedicodiRsvEficienteGas = 1207;
        public const int PtomedicodiRsvEficienteCarbon = 1206;

        public const int GrupoReservaFria = 1;
        public const int AnexoAPtomedicodiRsvFriaTermica = 7000;
        public const int AnexoAPtomedicodiRsvFriaRapida = 7001;
        public const int AnexoAPtomedicodiRsvFriaxMto = 7002;

        //Puntos demanda Ncp
        public const int PtomedicodiDemandaSEIN = 1210;
        public const int PtomedicodiDemandaSur = 1213;
        public const int PtomedicodiDemandaNorte = 1212;
        public const int PtomedicodiDemandaELA = 1214;
        public const int PtomedicodiDemandaECUADOR = 1229;
        public const int PtomedicodiCMGSein = 1217;

        public const string AppExcel = "application/vnd.ms-excel";
        public const int ConcepcodiActivo = 20;
        public const int ConsiscodiActivoEmpresa = 1;
        public const int ConsiscodiIntegranteEmpresa = 2;

        public const int FiltroTension138kv = 56;
        public const int FiltroTension220kv = 55;
        public const int FiltroTension500kv = 1146;

        public const int TipoExcel = 1;
        public const int TipoWord = 2;

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual_usuario_Informes SGI.rar";
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        //constantes para fileServer
        public const string FolderRaizInformesSGIModuloManual = "Informes SGI\\";

        //Manual de usuario SGI
        public const string ArchivoManualUsuarioIntranetSGI = "Manual_Usuario_SGI_v1.2.pdf";

        //constantes para fileServer SGI v1
        public const string FolderRaizv1ModuloManual = "Migración SGOCOES Desktop a Intranet\\";


        //Manual de usuario v1
        public const string ArchivoManualUsuarioIntranetv1 = "Manual_Usuario_SCO_v1.1.pdf";

        #region siosein2
        public const int LectcodiMedidoresGeneracion = 1;
        public const int LectcodiProgramacionMedianoPlazo = 109;
        public const int TptomedicodiGeneracionMwh = 69;
        public const int TipoinfocodiMWh = 3;
        public const int TipoinfocodiMWDemanda = 20;
        public const int TipoinfocodiDolar = 21;
        public const int TipoinfocodiPorcentual = 39;
        public const int TipoinfocodiCtvDolarKWh = 63;
        public const int TipoinfocodiMm3 = 13;
        public const int TipoinfocodiHm3 = 14;
        public const int TipoinfocodiGWh = 58;
        public const int FamcodiCentralHidro = 4;
        public const int FamcodiGeneradorHidro = 2;
        public const int PropcodiPotEfectivaUnidad = 164;
        public const int PropcodiRendiGenerador = 308;
        public const int PropcodiRendiCentral = 932;
        public const int ClasifReal = 3;
        public const int FamcodiGeneradorTermo = 3;
        public const int FamcodiGeneradorSolar = 36;
        public const int FamcodiGeneradorEolico = 38;
        public const int FamcodiCentralTermo = 5;
        public const int FamcodiCentralSolar = 37;
        public const int FamcodiCentralEolico = 39;
        public const int LectcodiProgDiarioMe1 = 22;
        public const int PtomedicodiCostoOperacionNCP = 1301;
        public const int LectcodiProgSemanalMe1 = 23;
        public const string PtomedicodiCmgStaRosaNCP = "1226";
        public const int PtomedicodiCmgXMwhSantaRosa = 1197;

        #endregion

        #region Servicio DigSilent
        public readonly static string FileSystemMigraciones = ConfigurationManager.AppSettings["FileSystemMigraciones"].ToString();
        public const string TipoFuente = "1,2";
        public const string TipoRadio = "0,1,2,3,4,5";
        #endregion

        public const string FormatoFechaMail = "dd.MM.yyyy";
        public readonly static int PlantcodiNotificacionCitacion = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreocitacion"]);
        public readonly static int PlantcodiNotificacionInformeCtaf = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoinformectaf"]);

        public enum TipoDatoColumna
        {
            Caracter = 1,
            Numerico = 2,
            FechaHora = 3
        }

        #region Siosein

        public const string PaisIntercambio = "ECU";
        public const string CodigoInterconexion = "0006";


        public const int PtomedicodiSANTAROSA220 = 25047;
        public const int PtomedicodiTRUJILLO220 = 25052;
        public const int PtomedicodiSOCABAYA220 = 25086;

        public enum BloqueNumeroCmg
        {
            PuntaMaxima = 1,
            MediaMaxima = 2,
            Punta = 3,
            Media = 4,
            Base = 5
        }

        public static Dictionary<BloqueNumeroCmg, string> BloqueNumeroCmgDesc = new Dictionary<BloqueNumeroCmg, string>
        {
            { BloqueNumeroCmg.PuntaMaxima , "Punta Máxima" },
            { BloqueNumeroCmg.MediaMaxima , "Media Máxima" },
            { BloqueNumeroCmg.Punta , "Punta" },
            { BloqueNumeroCmg.Media , "Media" },
            { BloqueNumeroCmg.Base , "Base" },
        };

        public static Dictionary<BloqueNumeroCmg, string> BloqueNumeroCmgTipo = new Dictionary<BloqueNumeroCmg, string>
        {
            { BloqueNumeroCmg.PuntaMaxima , "line" },
            { BloqueNumeroCmg.MediaMaxima , "line" },
            { BloqueNumeroCmg.Punta , "column" },
            { BloqueNumeroCmg.Media , "column" },
            { BloqueNumeroCmg.Base , "column" },
        };

        public const int TptomedicodiCaudalTurbinado = 70;
        public const int TptomedicodiCaudalVertido = 71;
        public const int TptomedicodiCaudales = 72;
        public const int TptomedicodiEvaporacion = 73;
        public const int TptomedicodiVolumenFinal = 74;

        public const int CodigoRegsultadoVolumenInicial = 1;
        public const int CodigoRegsultadoCaudalTurbinado = 3;
        public const int CodigoRegsultadoCaudalVertido = 7;
        public const int CodigoRegsultadoCaudales = 2;
        public const int CodigoRegsultadoVolumenFinal = 5;
        public const int CodigoRegsultadoEvaporacion = 4;

        public const int PmftabcodiEmbalses = 14;
        public const int ReporcodiCaudalesEjecDiario = 63;
        public const int ReporcodiVolumenEmbalses = 64;
        public const int ReporcodiVolumenLagos = 65;

        public const int BarrcodiSantaRosa220 = 14;

        public const int Periodo1 = 1;
        public const int Periodo24 = 24;
        public const int Periodo48 = 48;
        public const int Periodo96 = 96;
        #endregion

        #region Equipos sin datos de ficha técnica

        public const int PropiedadAuditoriaEquinomb = 1;
        public const int PropiedadAuditoriaEquiabrev = 2;
        public const int PropiedadAuditoriaEquiabrev2 = 3;
        public const int PropiedadAuditoriaEmpresa = 1831;
        public const int PropiedadAuditoriaFamcodi = 4;
        public const int PropiedadAuditoriaAreacodi = 10;
        public const int PropiedadAuditoriaEcodigo = 11;
        public const int PropiedadAuditoriaEquipadre = 21;
        public const int PropiedadAuditoriaEquitension = 22;
        public const int PropiedadAuditoriaEquimaniobra = 23;
        public const int PropiedadAuditoriaOsinergcodi = 24;
        public const int PropiedadAuditoriaEstado = 1832;
        public const int PropiedadAuditoriaOperadoremprcodi = 25;
        public const int PropiedadAuditoriaOsinergcodigen = 26;
        public const int PropiedadAuditoriaGrupocodi = 27;

        public const int PropiedadOperacionComercial = 1963;

        public static List<string> ListValPropDecimal = new List<string>() { "NA", "0" };
        public static List<string> ListValPropNumeric = new List<string>() { "NA", "0" };
        public static List<string> ListValPropNumber = new List<string>() { "NA", "0" };
        public static List<string> ListValPropFile = new List<string>() { "NULL" };
        public static List<string> ListValPropN = new List<string>() { "NA", "NULL" };
        public static List<string> ListValPropString = new List<string>() { "NA", "NULL" };

        public const string TipoDecimal = "DECIMAL";
        public const string TipoNumerico = "NUMERIC";
        public const string TipoEntero = "NUMBER";
        public const string TipoArchivo = "FILE";
        public const string TipoN = "N";
        public const string TipoString = "STRING";

        public const bool ValidarDato = true;
        #endregion

        #region IND
        public const int Famcodigaseoducto = 34;

        public enum Estado
        {
            Activo = 1,
            Inactivo = 0
        }

        public const string Historico = "H";

        #endregion

        #region Mejoras CTAF
        public readonly static string FileSystemSco = ConfigurationManager.AppSettings["FileSystemSco"].ToString();
        #endregion

        public const int CodigoOrigenLecturaML = 32;
        public const decimal dPorcentajePotenciaMaxima = 0.1M;
        public const int LectcodiTransferencia = 93;
        public const int PlantillacorreoCna = 165;
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
        //public const int PermisoSEV = 20;
        public const int GenerarArchivo = 21;
        public const int AccionRiSGI = 22;
        public const int AccionRiDJR = 23;
        public const int AccionRiDE = 24;
        public const int PermisoSEVAseg = 25;
        public const int Confidencial = 26;
        public readonly static int PermisoSEV = Convert.ToInt32(ConfigurationManager.AppSettings["PermisoSEV"].ToString());

    }

    /// <summary>
    /// Tipos de lo log de los aplicativos
    /// </summary>
    public struct TipoLog
    {
        public const string Error = "E";
        public const string Mensaje = "M";
        public const string Alerta = "A";
    }


    /// <summary>
    /// Constantes utilizadas Para Auditoria
    /// </summary>
    public class ConstantesAuditoria
    {
        public const string Modificar = "MODIFICADO";
        public const string Eliminar = "ELIMINADO";
        public const string Nuevo = "CREADO";
        public const string Calcular = "CALCULADO";
        public const string Accion = "Accion";
        public const string UsuarioUpdate = "UsuarioUpdate";
        public const string FechaUpdate = "FechaUpdate";
    }

    /// <summary>
    /// Codigos de los tipos de plantilla
    /// </summary>
    public class TipoPlantillaCorreo
    {
        public readonly static string MailFrom = ConfigurationManager.AppSettings["MailFrom"];

        public const int NotificacionEnvioExtranet = 1;
        public const int NotificacionVencimientoEnvio = 2;
        public const int NotificacionAdministradorModulo = 3;
    }
}
