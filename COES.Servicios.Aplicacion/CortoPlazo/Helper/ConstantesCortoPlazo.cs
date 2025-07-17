using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.IEOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CortoPlazo.Helper
{
    /// <summary>
    /// Constantes utilizadas
    /// </summary>
    public class ConstantesCortoPlazo
    {
        public const string PathCostosMarginales = "PathCostosMarginales";
        public const string PathTempCostosMarginales = "PathTempCostosMarginales";
        public const string PathExportacionPSSE = "PathExportacionPSSE";
        public const string PathExportacionTNA = "PathExportacionTNA";
        public const string PathExportacionRespaldoTNA = "PathExportacionRespaldoTNA";
        public const string PathModificacionNCP = "PathModificacionNCP";
        public const string PathModificacionAlternaNCP = "PathModificacionAlternaNCP";
        public const string PathGeneracionEMS = "PathGeneracionEMS";
        public const string ExtensionRaw = ".raw";
        public const string ExtensionGen = ".gen";
        public const string ExtensionDat = ".DAT";
        public const string ExtensionCsv = ".csv";
        public const string ExtensionTxt = ".txt";
        #region Reprogramas
        public const string ExtensionZip = ".zip";
        #endregion
        public const string IniEPPSCODE = "0 / END OF BUS DATA, BEGIN LOAD DATA";
        public const string FinDatosBarraTNA = " 0 / END OF BUS DATA, BEGIN LOAD DATA";
        public const string IniEPPS = "0 / END OF FIXED SHUNT DATA, BEGIN GENERATOR DATA";
        public const string FinEPPS = "0 / END OF GENERATOR DATA, BEGIN BRANCH DATA";
        public const string IniDatosCongestion = "0 / END OF GENERATOR DATA, BEGIN BRANCH DATA";
        public const string FinDatosCongestion = "0 / END OF BRANCH DATA, BEGIN TRANSFORMER DATA";
        public const string FinDatosTransformador = "0 / END OF TRANSFORMER DATA, BEGIN AREA DATA";

        public const string IniDatosLineasTNA = " 0 / END OF GENERATOR DATA, BEGIN NONTRANSFORMER BRANCH DATA";
        public const string FinDatosLineasTNA = " 0 / END OF NONTRANSFORMER BRANCH DATA, BEGIN TRANSFORMER DATA";
        public const string FinDatosTransformadorTNA = " 0 / END OF TRANSFORMER DATA, BEGIN AREA INTERCHANGE DATA";
        public const string IniDatosGeneradorTNA = " 0 / END OF FIXED BUS SHUNT DATA, BEGIN GENERATOR DATA";
        public const string FinDatosGeneradorTNA = " 0 / END OF GENERATOR DATA, BEGIN NONTRANSFORMER BRANCH DATA";

        public const char CaracterSeparacionCSV = ',';
        public const char CaracterSeparadorSlash = '/';
        public const string FormatoNumero = "N";
        public const string TextoDia = "Día ";
        public const string TextoInicioCH = "Estg";
        public const string TipoHidraulica = "H";
        public const string TipoTermica = "T";
        public const string TipoSolar = "S";
        public const string TipoEolico = "E";
        public const string FuenteGeneracion = "G";
        public const string FileVolumenProgramado = "volfincp.csv";
        public const string FileDatoHidraulico = "chidroPE.dat";
        public const string FileValorAgua = "oppchgcp.csv";
        public const string FileCongestionLineas = "cmgcircp.csv";
        public const string FileCongestionConjunta = "cmgsumcp.csv";
        public const string FileModosOperacion = "gertercp.csv";
        public const string FileCMBarra = "cmgbuscp.csv";
        public const string FileEquivBarraCentral = "dbus.dat";
        public const int IdConjuntoLinea = -1;
        public const int IdLineaTransmision = 8;
        public const int IdTrafo2D = 9;
        public const int IdTrafo3D = 10;
        public const string TxtConjuntoLinea = "Grupo Línea : ";
        public const string TxtLineaTransmision = "Línea       : ";
        public const string TxtTrafo2D = "Trafo2D     : ";
        public const string TxtTrafo3D = "Trafo3D     : ";
        public const string CaracterCero = "0";
        public const string CaracterUno = "1";
        public const int IdPropPotMaxH = 298;
        public const int IdPropPotEfeH = 164; 
        public const int IdPropPotMinH = 299;
        public const int IdPropRendimiento = 308;
        public const int IdPropVelCargaH = 1835; //166;
        public const int IdPropVelDescargaH = 1836; //300;
        public const int IdPropCapacRegulacion = 1830;
        public const string ModoOperacionHO = "MO_HorasOperacion";
        public const string ModoOperacionDefecto = "MO_Defecto";
        public const string ModoOperacionNoExiste = "MO_NoExiste";
        public const int RestriccionPotFija = 1;
        public const int RestriccionPotMaxima = 2;
        public const int RestriccionPotMinima = 3;
        public const int RestriccionPlenaCarga = 4;
        public const string CapacidadRegulacionDiaria = "DIARIA";
        public const string NoTieneCapacidadRegulacion = "NO TIENE";
        public const int IdPropMinutosCargaDescarga = 5;
        public const string SaltoLinea = "\r\n";
        public const string FileLogCp = "LogProceso.txt";
        public const string PropVelocidadDescarga = "V_descargaCM"; //"V_descarga";
        public const string PropVelocidadCarga = "VtomacargaCM"; //"Vtomacarga";
        public const string PropCostoVariable = "CVNC";
        public const string PropPotenciaMaxima = "Pmax";
        public const string PropPotenciaEfectiva = "Pe";
        public const string PropPotenciaMinima = "Pmin";
        public const string PropFactorConversion = "PCI_SI";
        public const string PropIndicadorCurva = "CMGNTC";
        public const string PropIndCentral = "CM_INDCEN";
        public const string PropTipoCambio = "TCambio";
        public const string PropValorAgua = "VALORAGUA";
        public const string PropCurvaAjustadaSPR = "CoordConsumComb";
        public const string PropCostoCombustible = "CComb";
        public const int ConcepCodi_PCIPVariosSI = 698; // Codigo de Formula PCI_Costo_Combustibles_Varios
        public const string UnidadCostoCombustible = "CCombGAS_SI";
        public const string UnidadCostoCombustibleOsig = "CCombGas_Osiner";
        public const double FactorMW = 1000;
        public const string PropCi = "Ci";
        public const string PropPmax = "Pmax";
        public const string CcombXArr = "Ccomb_x_arr";
        public const string CcombXPar = "Ccomb_x_par";
        public const string TCambio = "TCambio";
        public const int PX1 = 14;
        public const int PY1 = 175;
        public const int PX2 = 176;
        public const int PY2 = 177;
        public const int PX3 = 178;
        public const int PY3 = 179;
        public const int PX4 = 180;
        public const int PY4 = 181;
        public const int PX5 = 182;
        public const int PY5 = 183;
        public const string HeaderConsumo = "BARRA           TENSION      ID     TIPO         PGEN          PMAX          PMIN           OPE      COSTO1         NCV         COD       PMAX1           CI1         PMAX2           CI2         PMAX3           CI3         PMAX4           CI4         PMAX5           CI5";
        public const string HeaderConsumoTna = "BARRA           TENSION      ID    						     TIPO      PGEN         PMAX         PMIN            OPE      COSTO1          NCV          COD       PMAX1         CI1       PMAX2         CI2       PMAX3         CI3       PMAX4         CI4       PMAX5         CI5";
        public const string CaracterTab = "    ";
        public const string CaracterComma = "    ,";
        public const string InicioDatosCongestion = "0  DATOS DE CONGESTION";
        public const string InicioDatosCongestionGrupo = "0  DATOS DE CONGESTION CONJUNTA";
        public const string InicioDatosCongestionGrupoFlujoMinimo = "0  DATOS DE CONGESTION CONJUNTA FLUJO MINIMO";
        public const string InicioGeneracionForzada = "0  GENERACION FORZADA";
        public const string FormatoDecimal = "#,###.000";
        public const string CaracterIgual = "= ";
        public const string CaracterIgualTna = "= ,";
        public const string FinArchivo = "0  FIN";
        public const string CaracterSeperacionDoble = " ";
        public const string CampoActivo = "1";
        public const string IniDemandaBarra = "0 / END OF BUS DATA, BEGIN LOAD DATA";
        public const string IniDemandaBarraTNA = " 0 / END OF BUS DATA, BEGIN LOAD DATA";
        public const string FinDemandaBarra = "0 / END OF LOAD DATA, BEGIN FIXED SHUNT DATA";
        public const string FinDemandaBarraTNA = " 0 / END OF LOAD DATA, BEGIN FIXED BUS SHUNT DATA";

        public const string FinFactDevice = "0 / END OF OWNER DATA, BEGIN FACTS DEVICE DATA";
        public const string FinShuntDinamico = "0 / END OF SWITCHED SHUNT DATA, BEGIN GNE DATA";
        public const string InicioShuntDinamico = "0 / END OF FACTS DEVICE DATA, BEGIN SWITCHED SHUNT DATA";
        public const string EstadoActivo = "ACTIVO";
        public const string InicioResultadoGams = "\"Descomposicion Costos Marginales\"";
        public const string FolderCorrida = "Corrida_";
        public const string ArchivoPSSE = "PSSE_";
        public const string ArchivoPSSEGen = "PSSE_OBTENERGENERACION.raw";           
        public const string ArchivoProprocesador = "PREPROCESADOR_";
        public const string ArchivoLOG = "LOGPROCESO_";
        public const string ArchivoEntradaGams = "ENTRADAGAMS_";
        public const string ArchivoEntradaGamsAC = "AC_ENTRADA_GAMS_";
        public const string ArchivoResultadoAC = "AC_RESULTADO_CMG_";
        public const string ArchivoFlujoAC = "AC_FLUJO_";
        public const string ArchivoAddAC = "AC_ADD_";
        public const string ArchivoResultadoGams = "RESULTADOGAMS_";        
        public const string ArchivoResultadoGams2 = "RESULTADO_GAMS_ANALISIS_";
        public const string ArchivoResultadoAlternativoGams = "RESULTADO_GAMS_ALTERNATIVO_";
        public const string ArchivoResultadoAlternativoGamsAnalisis = "RESULTADO_GAMS_ALTERNATIVO_ANALISIS_";
        public const int OperacionCorrecta = 1;
        public const int ErrorGamsNoEjecuto = 2;
        public const int ErrorNoExistePSSE = 3;
        public const int ErrorEnOperacion = 4;
        public const int ErrorNoExisteArchivosNCP = 5;
        public const int ErrorInconsistencias = 6;
        public const int ErrorModosOperacion = 7;
        public const int ErrorInconsistenciaModoOperacionOperacionEMS = 8;
        public const int ErrorOperacionEMS = 9;
        public const int ErrorInconsistenciaModoOperacion = 10;
        public const int ErrorModosOperacionOperacionEMS = 11;
        public const int ErrorInconsistenciaOperacionEMS = 12;
        public const int ErrorNoExisteTopologiaMD = 13;

        public const string AsuntoCorreo = "Ejecución proceso CMgN - Fecha: {0}, Hora: {1}";
        public const string AsuntoReproceso = "Reproceso ejecución proceso CMgN - Fecha: {0}, Hora: {1}";
        public const string AsuntoReprocesoMasivo = "Resultado reproceso masivo CMg: Desde {0} al {1}";
        public const string AsuntoReprocesoMasivoV2 = "Resultado reproceso masivo CMg: Desde {0} al {1} - V2.0";
        public const string AsuntoReprocesoMasivoModificado = "Resultado reproceso masivo CMg";
        public const string AsuntoReprocesoMasivoModificadoV2 = "Resultado reproceso masivo CMg - V2.0";
        public const string AsuntoReprocesoMasivoModificadoTIE = "Resultado reproceso masivo CMg por TIE";
        public const string AsuntoReprocesoMasivoModificadoTIEV2 = "Resultado reproceso masivo CMg por TIE - V2.0";
        public const string AsuntoReprocesoMasivoModificadoVA = "Resultado reproceso masivo CMg por modificación VA";
        public const string AsuntoReprocesoMasivoModificadoVA2 = "Resultado reproceso masivo CMg por modificación VA - V2.0";
        public const string AsuntoValidacionCorreo = "Validación ejecución proceso CMgN - Fecha: {0}, Hora: {1}";
        public const string EmailNotificacionCMgN = "EmailNotificacionCMgN";
        public const char CaracterComa = ',';
        public const int IdCalificacionPorDefecto = 102;
        public const string MensajeModoOperacion = "<strong> Unidades sin Modo de Operación </strong><br /><br />";
        public const string MensajeOperacionEMS = "<strong> Unidades con Modo de Operación y con estado no operativo en el EMS</strong><br /><br />";
        public const string FormatoHoraMinuto = "HH:mm";
        //- Codigos de parametros
        public const int IdParametroRpf = 1;
        public const int IdParametroVolumen = 7;
        public const int IdParametroPendiente = 8;
        public const int IdParametroMinutos = 6;
        public const int IdParametroVariacionPotencia = 9;
        public const int CodigoBarraRepresentativo = 291;
        public const int IdParametroM = 19;
        public const string UltimoPeriodo = "23:59";
        public const string PrimerPeriodo = "00:00";

        #region FIT - VALORIZACION

        public const string InicioResultadoGamsResumen = "\"Resumen\"";

        //- Modificación MOVISOFT 02022021
        public const string InicioResultadoGamsCompensacionTermica = "\"Compensaciones Termicas\"";
        public const string InicioResultadoGamsGeneracionTermica = "\"Generacion Termica\"";
        public const string InicioResultadoGamsGeneracionHidraulica = "\"Generacion Hidraulica\"";
        //- Fin Modificación MOVISOFT 02022021

        #endregion

        #region Mejoras CMgN

        public const string InicioResultadoGamsCongestion = "\"Congestion\"";
        public const string InicioResultadoGamsCongestionConjunto = "\"Ruta en congestion\"";
        public const string InicioResultadoGamsRegionSeguridadArriba = "\"Congestion Region de Seguridad (arriba)\"";
        public const string InicioResultadoGamsRegionSeguridadAbajo = "\"Congestion Region de Seguridad (abajo)\"";

        #endregion

        #region MDCOES

        public const int CatcodiEmbalse = 19;
        public const int PropcodiVolumenMinimo = 28;
        public const int PropcodiVolumenMaximo = 29;
        public const int SrestcodiVolumenesEmbalses = 64;
        public const int SrestcodiCmgPorBarra = 73;
        public const int SrestcodiCmgLineas = 72;
        public const int SrestcodiCmgSumaFlujSup = 96;
        public const int SrestcodiCmgSumaFlujInf = 97;

        public const string TopologiaDiario = "1";
        public const string TopologiaSemanal = "2";
        public const int TopologiaReprograma = 3;
        #endregion

        public const string TipoMDCOES = "Y";
        public const string TipoNCP = "N";
        public const string EstimadorTNA = "T";
        public const string EstimadorPSS = "P";


        #region Regiones_seguridad

        public const int TipoGeneradorHidraulico = 2;
        public const int TipoGeneradorTermico = 3;

        #endregion
        #region IMME
        public const string MetodoCargaCmgProgramadoYupana = "CargarCostosMarginalesProgramadosYupana";
        #endregion

        #region Mejoras CMgN

        public const int Topcodi = 0;
        public const int Catcodi = 7;
        public const int IdConfiguracionUmbral = 1;
        public const string PropiedadCostoMarginal = "73";
        public const string ReporteComparativoDemanda = "ComparativoDemandaEMSvsDemandaEjecutada_{0}.xlsx";
        public const string ReporteComparativoDemandaMasivo = "DiferenciasDemandaEMSvsEjecutada_{0}.xlsx";
        public const string ReporteComparativoCostoMarginal = "ComparativoCostoMarginalvsCostoIncremental_{0}.xlsx";
        public const string ReporteComparativoCongestiones = "ComparativoCongestiones_{0}.xlsx";
        public const string ReporteComparativoCostoIncremental = "ComparativoCostoIncremenal_{0}.xlsx";
        public const string ReporteValidacionCM = "ValidacionCM_{0}.xlsx";
        public const string FormatoCargaFPM = "FactorPerdidaMarginal_{0}.xlsx";
        public const string FormatoUploadFMP = "UploadFPM.xlsx";
        public const string FormatoBarraDesenergizada = "BarrasDesenergizadas.xlsx";



        public static readonly List<int> ListaCalificacionAlertaHO = new List<int>() { ConstantesSubcausaEvento.SubcausaAMinimaCarga, ConstantesSubcausaEvento.SubcausaPorPotenciaEnergia
                                                                                    , ConstantesSubcausaEvento.SubcausaPorPruebas , ConstantesSubcausaEvento.SubcausaPorPruebasAleatoriasPR25
                                                                                    , ConstantesSubcausaEvento.SubcausaPorRsf, ConstantesSubcausaEvento.SubcausaPorRestricOpTemporal };

        public static readonly List<int> ListaCalificacionAlertaUrs = new List<int>() {ConstantesSubcausaEvento.SubcausaPorPotenciaEnergia, ConstantesSubcausaEvento.SubcausaPorRsf};

        public const int TipoRsvSecUp = 1;
        public const int TipoRsvSecDown = 2;

        public const int TipoCompHOvsDesp = 1;
        public const int TipoCompEMSvsDesp = 2;

        public const int AlertaEmsDesconectadoSinDesp = 1;
        public const int AlertaEmsDesconectadoConDesp = 2;
        public const int AlertaEmsConectadoSinDesp = 3;

        public const int TapUmbralesComparativos = 1;
        public const int TapAnguloOptimo = 2;

        public const int TipoTransaccionInternacional = 219;
        public const string BarraTalara = "TALARA220";
        public const string NombreBarraTalara = ",    0.00,    0.00,    0.00,    0.00,    0.00,    1,0 / TALARA   220      LTALA220";
        public const string DemandaTalaraTxt = ",'1 ',1,  1,  1,";
        public const string FileRawTIE = "tie_reproceso.raw";
        public const string FileForzarVA = "forzarva_reproceso.raw";

        #endregion

        #region CMgCP_PR07
        public const string FechaVigenciaPR07 = "FechaVigenciaPR07";
        public const int VersionCMOriginal = 1;
        public const int VersionCMPR07 = 2;
        public const string EjecucionVersionActualCM = "EjecucionVersionActualCM";
        public const string EjecucionVersionPR07CM = "EjecucionVersionPR07CM";
        public const string UsuarioAutomatico = "automatico";
        public const string OrdenEjecucionModelo = "EjecutaVersionPR07Primero";
        #endregion

        #region Ticket 2022-004345
        public const string ErrorGenericoCM = "Se ha producido un error en el cáculo, por favor revise los archivos de entrada.";
        #endregion

        public const string CongestionRegionSeguridad = "R";
        public const string TxtFuenteRegionSeguridad = "Región de Seguridad";

    }

    /// <summary>
    /// Obtener los datos para generar reporte web / excel de Comparativo HO vs Despacho 
    /// </summary>
    public class ReporteComparativoHOvsDespacho 
    {
        public DateTime Fecha { get; set; }

        public int Equipadre { get; set; }
        public int Grupocodi { get; set; }
        public bool EsUnidadEspecial { get; set; }
        public List<PrGrupoDTO> ListaGrupoDespacho { get; set; }

        public GraficoWeb Grafico { get; set; }
        public string ReporteHtml { get; set; }

        public DatoComparativoHOvsDespacho DatoXGrupo { get; set; }
        public DatoComparativoHOvsDespacho DatoXModo { get; set; }
        public List<DatoComparativoHOvsDespacho> ListaDatoXEq { get; set; }

        public bool TieneAlertaCalif { get; set; }

        public ReporteComparativoHOvsDespacho()
        {
            ListaGrupoDespacho = new List<PrGrupoDTO>();
            Grafico = new GraficoWeb();

            DatoXGrupo = new DatoComparativoHOvsDespacho();
            DatoXModo = new DatoComparativoHOvsDespacho();
            ListaDatoXEq = new List<DatoComparativoHOvsDespacho>();
        }
    }

    /// <summary>
    /// Datos del modo / equipo de unidad especial para Comparativo HO vs Despacho 
    /// </summary>
    public class DatoComparativoHOvsDespacho
    {
        //grupocodi del modo
        public int Grupocodi { get; set; }
        public PrGrupoDTO Modo { get; set; }
        public PrGrupoDTO GrupoDesp { get; set; }
        //equicodi de la unidad especial
        public int Equicodi { get; set; }
        public EqEquipoDTO EquipoEsp { get; set; }
        //si el equipo tiene configuración EMS
        public bool TieneEqConfEms { get; set; }

        public decimal Rpf { get; set; }
        public decimal Pmin { get; set; }
        public decimal Pe { get; set; }

        public string[] ListaHora { get; set; }
        public int[] ListaCalifHo { get; set; }
        public string[] ListaSubcausadesc { get; set; }

        public decimal?[] ListaMWDespacho { get; set; }
        public decimal?[] ListaMWHo { get; set; }
        public decimal?[] ListaMWEms { get; set; }
        public int?[] ListaFlagEms { get; set; }
        public decimal?[] ListaMWRUp { get; set; }
        public decimal?[] ListaMWRDown { get; set; }
        public decimal?[] ListaDiferencia { get; set; }
        public decimal?[] ListaDesviacion { get; set; }

        public bool[] ListaAlerta { get; set; }
        public int[] ListaAlertaEms { get; set; }

        public string[] ListaDescripcionHo { get; set; }
        public string[] ListaDescripcionEMS { get; set; }
        public List<string> ListaMensaje { get; set; }

        public DatoComparativoHOvsDespacho()
        {
            Modo = new PrGrupoDTO();
            GrupoDesp = new PrGrupoDTO();
            EquipoEsp = new EqEquipoDTO();

            ListaHora = new string[48];
            ListaCalifHo = new int[48];
            ListaSubcausadesc = new string[48];

            ListaMWDespacho = new decimal?[48];
            ListaMWHo = new decimal?[48];
            ListaMWEms = new decimal?[48];
            ListaFlagEms = new int?[48];
            ListaMWRUp = new decimal?[48];
            ListaMWRDown = new decimal?[48];
            ListaDiferencia = new decimal?[48];
            ListaDesviacion = new decimal?[48];

            ListaAlerta = new bool[48];
            ListaAlertaEms = new int[48];

            ListaDescripcionHo = new string[48];
            ListaDescripcionEMS = new string[48];
            ListaMensaje = new List<string>();
        }
    }

    public class ReporteComparativoHOvsRsvaSec
    {
        public string Titulo { get; set; }
        public DateTime Fecha { get; set; }
        public List<DatoComparativoHOvsRsvaSec> ListaDato { get; set; } = new List<DatoComparativoHOvsRsvaSec>();
        public string ReporteHtml { get; set; }
    }

    public class DatoComparativoHOvsRsvaSec
    {
        public string Urs { get; set; }
        public string Central { get; set; }
        public string Gruponomb { get; set; }
        public string Agc { get; set; }

        public string Periodo { get; set; }
        public string HoraIni { get; set; }
        public string HoraFin { get; set; }

        public string Mensaje { get; set; }

    }

    public class DatoComparativoHOvsRasig
    {
        public DateTime Fecha { get; set; }
        public int GrupocodiUrs { get; set; }
        public int Hopcodi { get; set; }
        public int HIni { get; set; }
        public int HFin { get; set; }
        public bool TieneAlerta { get; set; }

        //grupocodi del modo
        public int Grupocodi { get; set; }
        public PrGrupoDTO Modo { get; set; }
        public PrGrupoDTO Urs { get; set; }
        public EveHoraoperacionDTO Ho { get; set; }

        public string[] ListaHora { get; set; }
        public decimal?[] ListaMWRUp { get; set; }
        public decimal?[] ListaMWRDown { get; set; }

        public bool[] ListaAlerta { get; set; }
        public string[] ListaDescripcionHo { get; set; }
        public string[] ListaAlertadesc { get; set; }

        public int[] ListaCalifHo { get; set; }
        public string[] ListaSubcausadesc { get; set; }

        public List<string> ListaMensaje { get; set; }

        public DatoComparativoHOvsRasig()
        {
            Modo = new PrGrupoDTO();
            Urs = new PrGrupoDTO();
            Ho = new EveHoraoperacionDTO();

            ListaHora = new string[48];
            ListaMWRUp = new decimal?[48];
            ListaMWRDown = new decimal?[48];

            ListaAlerta = new bool[48];
            ListaDescripcionHo = new string[48];
            ListaAlertadesc = new string[48];

            ListaCalifHo = new int[48];
            ListaSubcausadesc = new string[48];

            ListaMensaje = new List<string>();
        }
    }

    public class ValidacionProcesosCMg
    {
        public int Resultado { get; set; }
        public List<DetalleValidacionCM> ListaDetalle { get; set; }
       
    }


    public class DetalleValidacionCM
    {
        public string Hora { get; set; }
        public string FechaEjecucion { get; set; }
        public string Estimador { get; set; }
        public string Programa { get; set; }
        public string MensajeArhivo { get; set; }
        public string MensajeTopologia { get; set; }
        public string MensajePeriodo { get; set; }
        public List<string> Archivos { get; set; }
    }
}
