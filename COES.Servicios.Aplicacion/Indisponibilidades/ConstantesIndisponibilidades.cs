using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.IEOD;
using System;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.Indisponibilidades
{
    public class ConstantesIndisponibilidades
    {
        public const int IdFormato = 85;
        public const int LectcodiIndisponibilidad = 205;
        public const int TipoinfocodiIndisp = 57;
        public const int TptomediCCDF_T = 61;
        public const int TptomediCCDF_D = 62;
        public const int TptomediSUGAD = 63;
        public const int TipoSucad = 100005;
        public const int OrigenLecturaCapacidad = 28;


        public const int RptCuadro1AnchoEmpresa = 52;
        public const int RptCuadro1AnchoCentral = 35;
        public const int RptCuadro1AnchoUnidad = 32;

        public const int PropcodiPEfectiva = 46;
        public const int PropcodiRendimiento = 319;
        public const int ConcepcodiDividirUnidadModoEsp = 613;
        public const int ConcepcodiRendimiento = 190;
        public const int ConcepcodiRendimientoSI = 539;
        public static readonly List<int> ListaConcepcodiRend = new List<int>() { 190, 539 };
        public const int ConcepcodiValorRsfDefecto = 642;

        public const int ConcepcodiTminop = 139;
        public const int ConcepcodiPotMinima = 16;
        public const int ConcepcodiPotEfectiva = 14;
        public const int ConcepcodiConsumoPotEfectiva = 175;
        public const int ConcepcodiPotParcial1 = 176;
        public const int ConcepcodiConsumoPotParcial1 = 177;
        public const int ConcepcodiPotParcial2 = 178;
        public const int ConcepcodiConsumoPotParcial2 = 179;
        public const int ConcepcodiPotParcial3 = 180;
        public const int ConcepcodiConsumoPotParcial3 = 181;
        public const int ConcepcodiPotParcial4 = 182;
        public const int ConcepcodiConsumoPotParcial4 = 183;
        public const int ConcepcodiPotParcial5 = 285;
        public const int ConcepcodiConsumoPotParcial5 = 286;
        public const int ConcepcodiConsumoPotParcial5_SI = 10286;
        public const string ConcepcodisCurvaCombModo = "14,175,176,177,178,179,180,181,182,183,190,16,516,517,518,520,519,539";
        public const string ConcepcodisCurvaCombModoSinSI = "14,175,176,177,178,179,180,181,182,183,190,16";
        public const string ConcepcodisCurvaCombModoSI = "516,517,518,520,519,14,176,178,180,182,539,16";
        public const int FactorConversionM3h = 1000;
        public const decimal FactorConversionPCToM3h = 35.31466672m;
        //public const string EmpresasFactorK1 = "12097,2,11323,67,11486,11772,11063,10587,8,11563,11528"; //En el GeneraciónPiura,Electroperu,Planta de reserva fría de generación de eten,Minera cerro verde,Samayi,Agroaurora,Petramas,SDF Energía,Shougesa,Empresa concesionaria de energía limpia,Infraestructura y energías del Perú
        public const int ConcepcodiTminarranque = 136;

        public const int CatecodiModoOperacion = 2;
        public const int CatecodiCentralTermica = 4;
        public const int CatecodiGrupoHidraulico = 5;
        public const int CatecodiCentralHidraulico = 6;
        public const int CatecodiIncremental = 14;

        public const int ConcepcodiPotAdicional = 610;
        public const int ConcepcodiCodigoModoOp = 612;
        public const int ConcepcodiCodigoModoTienePAdic = 614;
        public const int ConcepcodiPotEfectivaRestanteAdic = 615;
        public const int ConcepcodiPotFirme = 616;
        public const int ConcepcodiMinSinCalorUtil = 644;

        public const int ConcepcodiNumGenRf = 618;
        public const int ConcepcodiCombAlt = 617;
        public const int ConcepcodiCombArSinAltResidual = 241;
        public const int ConcepcodiCombArSinAltResidualSI = 361;
        public const int ConcepcodiCombArSinAltCarbon = 292; //288
        public const int ConcepcodiCombArSinAltCarbonSI = 368; //288

        public const int IdSubCausaPotenciaEnergia = 101;
        public const int SubcausacodiRestric = 205;

        public const int EvenclasecodiEjec = 1;
        public const int EvenclasecodiProg = 2;

        public const int TgenercodiHidroelectrica = 1;
        public const int TgenercodiTermoelectrica = 2;

        public const int EventoGeneradoFicticio = 1;
        public const int TipoUnificacionCompleta = 1;
        public const int TipoUnificacionExcluyente = 2;

        public const int TipoCombustiblePrincipal = 1;
        public const int TipoCombustibleSecundario = 2;
        public static readonly List<int> ListaFenergcodiCumplimiento = new List<int>() { ConstantesPR5ReportesServicio.FenergcodiCarbon, ConstantesPR5ReportesServicio.FenergcodiDiesel, ConstantesPR5ReportesServicio.FenergcodiGas
                                                        ,ConstantesPR5ReportesServicio.FenergcodiR500, ConstantesPR5ReportesServicio.FenergcodiR6};
        public static readonly List<int> ListaFenergcodiCuadro2 = new List<int>() { ConstantesPR5ReportesServicio.FenergcodiDiesel, ConstantesPR5ReportesServicio.FenergcodiGas
                                                        ,ConstantesPR5ReportesServicio.FenergcodiR500, ConstantesPR5ReportesServicio.FenergcodiR6};

        public const int ConcepcodiRpf = 282;

        //Crud Mantenimiento
        public const int FuenteIndMantto = 1;
        public const string FuenteIndManttoDesc = "Aplicativo";
        public const int FuenteEveMantto = 2;
        public const string FuenteEveManttoDesc = "Mantenimiento";
        public const string TipoAccionIUM = "IUM"; //IND_MANTTO que es un Update de EVE_MANTTO
        public const string TipoAccionIXM = "IXM"; //IND_MANTTO que es una Eliminacion de EVE_MANTTO
        public const string TipoAccionIN = "IN"; //IND_MANTTO es un nuevo mantenimiento
        public const int FuenteEvento = 3;
        public const string FuenteEventoDesc = "Evento";
        public const int FuenteRestric = 4;
        public const string FuenteRestricDesc = "Restricción Operativa";

        public const string FS = "F";
        public const string ES = "E";
        public const int TipoevencodiPruebas = 6;

        public const string InterrupcionSi = "S";
        public const string InterrupcionNO = "N";

        public const int TipoAccionNuevo = 1;
        public const int TipoAccionVer = 2;
        public const int TipoAccionEditar = 3;

        public const int FlagCalculoSi = 1;
        public const int FlagCalculoNo = 0;
        public const int FlagCalculoNoCumpleCondicionPR25 = -1;
        public const int SubcausacodiMinimacarga = 102;
        public const int SubcausacodiRsf = 320;

        public const int AppPR25 = 1;
        public const int AppSiosein2 = 2;
        public const int AppPF = 11;
        public const int AppPFR = 12;

        public const int ReporteSiosein2 = 56;
        public const int ReportePR25Cuadro1 = 1;
        public const int ReportePR25Cuadro2 = 2;
        public const int ReportePR25Cuadro2LimComb = 200;
        public const int ReportePR25Cuadro3FactorK = 3;
        public const int ReportePR25Cuadro4 = 4;
        public const int ReportePR25Cuadro5 = 5;
        public const int ReportePR25Cuadro7 = 7;

        public const int ReportePR25 = 1001;
        public const int ReportePR25PlantillaHidrico = 1002;
        public const int ReportePR25PlantillaTermico = 1003;

        public const int ReportePR25FactorFortTermico = 8;
        public const int ReportePR25FactorProgTermico = 9;
        public const int ReportePR25FactorProgHidro = 10;
        public const int ReportePR25FactorPresencia = 11;

        public const int ReportePR25HistoricoTermo = 12;
        public const int ReportePR25HistoricoHidro = 13;

        public const int ReportePR25DisponibilidadCalorUtil = 14;

        public const string EstadoPeriodoAbierto = "A";
        public const string EstadoPeriodoPublicar = "P";
        public const string EstadoPeriodoCerrado = "C";

        public const string TipoRecalculoPrimeraQuincena = "PQ";
        public const string TipoRecalculoPreliminarMensual = "PM";
        public const string TipoRecalculoMensual = "M";
        public const string TipoRecalculoRevision = "R";

        public const string HorizonteMensual = "M";
        public const string HorizonteVariableAnual = "VA";
        public const string HorizonteVariableMensual = "VM";

        public const string MenuCuadro = "1";
        public const string MenuFactor = "2";
        public const string MenuCuadro7 = "3";
        public const int MenuOpcionCodeIndisp = 805;
        public const int EsVersionAnteriorValidado = 2;
        public const int EsVersionValidado = 1;
        public const int EsVersionGenerado = 0;
        public const string TipoCambioEdicion = "U";
        public const string TipoCambioEliminacion = "X";
        public const string TipoCambioNuevo = "N";

        public const string TiempoHoraPunta = "HP";
        public const string TiempoTodoDia = "TD";

        public const string MedicionOrigen96 = "M96";
        public const string MedicionOrigen48 = "M48";

        public const string TipoReporteAplicativo = "A";
        public const string TipoReporteHistorico = "H";

        public const int TipoUnificarXUnidad = 1;
        public const int TipoUnificarXGenerador = 2;

        public const string INDFortuitaTotal = "FT";
        public const string INDProgramadaTotal = "PT";
        public const string INDFortuitaParcial = "FP";
        public const string INDProgramadaParcial = "PP";

        public const string Activo = "A";
        public const string Inactivo = "I";

        public const int InsumoEveMantto = 10;
        public const int InsumoIndMantto = 20;
        public const int InsumoEveEvento = 30;

        public const int Regla7DiasINDFortuita = 1;
        public const int Regla7DiasINDProgramada = 2;

        public const string FormatoCeldaEvento = "EVENTO";
        public const string FormatoCeldaRestric = "RESTRIC";
        public const string FormatoCeldaAlerta = "ALERTA";
        public const string FormatoCeldaLimComb = "LIMCOMB";

        public const int NumeroDecimalesMaxApp = 15;
        public const int NumeroDecimalesMaxPorcApp = 17;
        public const string StrSeparador = "##";
        public const int CorrelativoInicial = 1000000;
        public const string AnexoB = "B";
        public const string AnexoC = "C";
        public const string ConcepcodiAnexoB = "619,620,621,622";
        public const int ConcepcodiFIFmensualTermo = 619;
        public const int ConcepcodiFIPmensualTermo = 620;
        public const int ConcepcodiFIPmensualHidro = 621;
        public const int ConcepcodiFIPanualHidroYTermo = 622;
        public const string ConcepcodiAnexoC = "623,624,625,626,627,628,629,630,631,632,633,634,635,636,637,638,639,640";
        public const string FlagCR = "S";
        public const string FlagCRDesc = "CR";
        public const int NumMesesValidacionOC = 90;
        public const string MesIniHistoricoTermo = "01 2011";
        public const string MesIniHistoricoHidro = "06 2011";
        public const string MesIniAplicativo = "01 2021";
        public const int SiparvcodiHidro = 74;
        public const int SiparvcodiTermo = 75;

        //05 2021: Fin histórico inicial (puesta en marcha Ficha 8)
        //08 2021: Fin histórico RE-19007 (Actualización manual de Factor de Indisponibilidad Programado Hidráulico de CH La Virgen)
        public const string MesFinHistorico = "08 2021";

        #region IND.PR25.2022
        public const string StkdettipoOriginal = "O";
        public const string StkdettipoModificado = "M";
        public const string HststktipaccionModificado = "Modificado";
        
        public const string ValorPorDefaultStock = "No informó.";
        //public const string ValorPorDefaultCDU = null;
        //public const string ValorPorDefaultCRD = null;
        //public const string ValorPorDefaultCCD = null;
        //public const string ValorPorDefaultSUGAD = null;
        public const decimal ValorPorDefaultFGTE = 1;

        public const int TipoTecnologiaFRCNA = 0;
        public const int TipoTecnologiaFRCCC = 1;
        public const int TipoTecnologiaFRCCS = 2;
        public const int TipoTecnologiaFRCMR = 3;

        public const int FamcodiCentralTermoElectica = 5;

        public const string FormatoFechaDDMMYYYY = "dd/MM/yyyy";
        public const string FormatoFechaYYYYMMDD = "yyyy-MM-dd";

        public const string FormatoFecha = "dd/MM/yyyy";
        public const string ReporteCuadroA1A2 = "ReporteCuadroA1A2";
        public const string ReporteCumplimiento = "ReporteCumplimiento";
        public const string TituloCuadroA1 = "INFORMACIÓN PARA EL CÁLCULO DEL FACTOR DE " +
                                                "INCENTIVO A LA DISPONIBILIDAD DE LAS UNIDADES " +
                                                "DE GENERACIÓN A GAS NATURAL";
        public const string TituloCuadroA2 = "INFORMACIÓN COMPLEMENTARIA SOBRE LA CAPACIDAD " +
                                                "DE TRANSPORTE DE GAS NATURAL ADQUIRIDA/VENDIDA " +
                                                "EN EL MERCADO SECUNDARIO DE GAS NATURAL";
        public const string LibroCuadroA1 = "Cuadro A1";
        public const string LibroCuadroA2 = "Cuadro A2";
        public const string EtiquetaCDU1 = "EMPRESA DE GENERACION: ";
        public const string EtiquetaCDU2 = "CAPACIDAD DE TRANSPORTE DE GAS NATURAL DISPONIBLE " +
                                            "DIARIA DEL TRAMO DE DUCTO PROPIO O DE TERCERO - CDU";
        public const string EtiquetaCDU3 = "Capacidad CDU (MMPCD)";
        public const string EtiquetaCDU4 = "Fecha de inicio de vigencia";
        public const string EtiquetaCDU5 = "Fecha de finalizacion de vigencia";
        public const string DescripcionCDU1 = "Capacidad disponible total";
        public const string DescripcionCDU2 = "Capacidad asignada a la central o unidad de generacion ";
        public const string EtiquetaCRD1 = "EMPRESA DE GENERACION: ";
        public const string EtiquetaCRD2 = "PERIODOS DE VIGENCIA";
        public const string EtiquetaCRD3 = "CAPACIDAD RESERVADA DIARIA - CRD";
        public const string EtiquetaCRD4 = "Capacidad CRD (MMPCD)";
        public const string DescripcionCRD1 = "Capacidad asignada a la central ";
        public const string CrdContratada = "1)Contratada a firme con el concesionario de " +
                                                "transporte de gas natural";
        public const string CrdAdquirida = "2)Adquirida mediante transferencias organizadas " +
                                            "en el Mercado Secundario de Gas Natural segun el iniciso ii) del numeral 4.1";
        public const string CrdVendida = "3)Vendida mediante transferencias organizadas en " +
                                            "el Mercado Secundario de Gas Natural segun el inciso ii) del numeral 4.1";
        public const string CrdTotal = "Capacidad contratada total disponible (1 + 2 - 3)";

        public const string EtiquetaCCD1 = "EMPRESA DE GENERACION: ";
        public const string EtiquetaCCD2 = "CAPACIDAD CONTRATADA DIARIA FIRME CON EL CONCESIONARIO " +
                                                "DE DISTRIBUCION DE GAS NATURAL - CCD";
        public const string EtiquetaCCD3 = "Capacidad CCD (MMPCD)";
        public const string EtiquetaCCD4 = "Fecha de inicio de vigencia";
        public const string EtiquetaCCD5 = "Fecha de finalizacion de vigencia";
        public const string DescripcionCCD1 = "Capacidad disponible total";
        public const string DescripcionCCD2 = "Capacidad asignada a la central ";

        public const string EtiquetaA2CTG1 = "EMPRESA DE GENERACION: ";
        public const string EtiquetaA2CTG2 = "PERIODO DE VIGENCIA : ENTRE EL ";
        public const string EtiquetaA2CTG3 = "CAPACIDAD RESERVADA DIARIA ADQUIRIDA/VENDIDA MEDIANTE " +
                                              "TRANSFERENCIAS EN EL MERCADO SECUNDARIO DE GAS NATURAL";
        public const string EtiquetaA2CTG4 = "EMPRESA CON QUIEN SE TRANSO";
        public const string EtiquetaA2CTG5 = "PUNTO DE SUMINISTRO";
        public const string EtiquetaA2CTG6 = "CANTIDAD ADQUIRIDA/VENDIDA (MMPCD)";
        public const string EtiquetaA2CTG7 = "PRECIO DE TRANSFERENCIA DEL ACUERDO (US$/Mm3)";
        public const string EtiquetaA2CTG8 = "ADQUIRIDA/VENTA";
        public const string EtiquetaA2CTG9 = "FECHA";
        public const string DescripcionA2CTG1 = "Adquisicion del dia ";
        public const string UsuarioNotificacion = "";

        public const int TipoCDU = 1;
        public const string NombreCDU = "CDU";
        public const int TipoCRD = 2;
        public const string NombreCRD = "CRD";
        public const int TipoCCD = 3;
        public const string NombreCCD = "CCD";
        public const int TipoSUGAD = 4;
        public const int TipoSUCAD = 5;
        public const int TipoFGTC1 = 6;
        public const int TipoFGTC2 = 7;
        public const int TipoFGTC3 = 8;
        public const int TipoFGTCMIN = 9;
        public const int TipoSUGAD_CMTR = 10;
        public const int TipoSUCAD_CN2GX24 = 11;
        public const int TipoFGTC = 12;
        public const int TipoFGTE = 13;
        public const int TipoFG = 14;

        public const string TipCombGasNatural = "Gas Natural";

        public const int TipoCombustibleGasNatural = 1;
        public const int TipoCombustibleDiferenteGasNatural = 2;

        public const int TipoinfocodiGasNatural = 46;

        public const int SCSantaRosaEmprcodi = 12096;
        public const int SCSantaRosaEquicodicentral = 289;
        public const int SCSantaRosaTipoinfocodi = 43;
        public const string SCSantaRosaEquicodiunidad = "33, 34, 35";    

        public const decimal FcPcm3 = 35.31466672m;
        public const decimal FcGal = 0.264172m;

        public const int IndependenciaGrupocodi = 312;
        public const int IndependenciaEquicodicentral = 12781;

        public const int ConcepcodiPe = 14;
        public const int ConcepcodiCombPe = 175;
        public const string ConcepcodiMo = "516, 175, 14";
        public const int Concepcodi_516 = 516;
        public const int Concepcodi_175 = 175;
        public const int Concepcodi_14 = 14;

        //Inicio Nota:
        //Esto es una réplica de los valores del key "ModosOperacionEspeciales" del Web.Config del proyecto COES.MVC.Publico
        public const string ModosOperacionEspeciales = "VENTCC3GAS,VENTCC3GASFD,VENTCC4GAS,VENTCC4GASFD,VENTCC34GAS,VENTCC34GASFD,CHILTV2R500,CHILTV3R500,LFLORESTG1GAS,ILO1TV2R500,ILO1TV2R500,ILO1TV3R500,ILO1TV4R500,ILO2_CARB,PARAMONGA";
        //Fin Nota

        public const string TextoCDU = "CDU";
        public const string TextoCRD = "CRD";
        public const string TextoCCD = "CCD";
        public const string TextoSUGAD = "SUGAD";
        public const string TextoSUCAD = "SUCAD";
        public const string TextoFGTE = "FGTE";

        public const string TextoFGTC1Bi = "FGTC1 Bi";
        public const string TextoFGTC2Bi = "FGTC2 Bi";
        public const string TextoFGTC3Bi = "FGTC3 Bi";
        public const string TextoFGTCiMIN = "FGTCi(min)";
        public const string TextoSUGADBi_CMTRB = "SUGAD Bi/CMTR B";
        public const string TextoSUCADCi_CN2GX24 = "SUCAD Ci/CN2Cx24";
        public const string TextoFGTCBi = "FGTC Bi";
        public const string TextoFGTEB = "FGTE B";
        public const string TextoFGi = "FGi";

        public const string TextoGasNatural = "Gas Natural";
        public const string TextoDiferenteGasNatural = "Diferente a Gas Natural";

        public const string TextoCalculos = "Cálculos :";
        public const string TextoK = "K";

        //Tipos para CCD y CDU
        public const int CapacidadAsignada = 1;
        public const int CapacidadTotal = 2;

        //Tipos para CRD
        public const int Contratada = 1;
        public const int Adquirida = 2;
        public const int Vendida = 3;
        public const int Total = 4;
        public const int Central = 5;

        //Para el cuadro A2
        public const int A2Compra = 1;
        public const int A2Venta = 2;
        public const string A2AdquiridaBase = "adquirida";
        public const string A2VentaBase = "venta";
        public const string A2AdquiridaGenerador = "adquirida-g";
        public const string A2VentaGenerador = "venta-g";
        //Zero
        public const string Zero = "0";

        public const int NumeroDecimales = 13;
        public const decimal DiferenciaPermitida = 0.0000000001M;
        #endregion
    }

    public class FuenteDatosMantto
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
    }

    public class CombustibleContrato : ICloneable
    {
        public DateTime? CbctrtFechaIniPer { get; set; }
        public decimal? CbctrtTransGas { get; set; }
        public decimal? CbctrtCapaDistGas { get; set; }
        public decimal? CbctrtStockUtil { get; set; }
        public decimal? CbctrtStockProg { get; set; }
        public DateTime? CbctrtFechaDia { get; set; }

        public int? Gaseoductoequicodi { get; set; }
        public string Gaseoducto { get; set; }
        public int Tipoinfocodi { get; set; }
        public int Tptomedicodi { get; set; }
        public string Tptomedinomb { get; set; }
        public string Tipoinfodesc { get; set; }
        public decimal? Valor { get; set; }
        public bool TieneDeclaracionAgente { get; set; }

        //Assetec [IND.PR252022]
        public int Ptomedicodi { get; set; }
        //Assetec [IND.PR252022]

        #region Campos para consulta
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Gruponomb { get; set; }
        public string Grupoabrev { get; set; }
        public int Fenergcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equipadre { get; set; }
        public int Equicodi { get; set; }
        public int Grupocodi { get; set; }
        public decimal? PotEfectiva { get; set; }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class IndCompensacionPruebaAleatoriaDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Empresa { get; set; }
        public string Central { get; set; }
        public string Unidad { get; set; }
        public string ModoOperacion { get; set; }
        public int HrasIndParcial { get; set; }
        public string PrimerArranque { get; set; }
        public string Exitosa { get; set; }
        public string Compensar { get; set; }
        public int Grupocodi { get; set; }
        public int? Equicodi { get; set; }
    }

    public class ReporteInsumoPR25
    {
        public IndReporteDTO Reporte { get; set; } = new IndReporteDTO();
        public List<IndReporteTotalDTO> ListaRptTot { get; set; } = new List<IndReporteTotalDTO>();
        public List<IndReporteDetDTO> ListaRptDet { get; set; } = new List<IndReporteDetDTO>();
        public List<IndInsumoLogDTO> ListaLogInsumo { get; set; } = new List<IndInsumoLogDTO>();
        public List<IndRelacionRptDTO> ListaRelRpt { get; set; } = new List<IndRelacionRptDTO>();
        public List<PfDispcalorutilDTO> ListaCalorUtil { get; set; } = new List<PfDispcalorutilDTO>();
    }

    public class CargaHistorica
    {
        public int Emprcodi { get; set; }
        public int Equipadre { get; set; }
        public int? Equicodi { get; set; }
        public int? Grupocodi { get; set; }
        public decimal? Tothorasp { get; set; }
        public decimal? Tothorasf { get; set; }
        public int Periodo { get; set; }
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
    }

    public class ParametroAnexoB
    {
        public decimal FIFmensualTermo { get; set; }
        public decimal FIPmensualTermo { get; set; }
        public decimal FIPmensualHidro { get; set; }
        public decimal FIPanualHidroYTermo { get; set; }
    }

    public class FactorINDTeorica
    {
        public int Fenergcodi { get; set; }
        public string TipoCentral { get; set; }
        public string TipoCombustible { get; set; }
        public decimal? FactorTeoricoProg { get; set; }
        public decimal? FactorTeoricoFort { get; set; }
        public bool TieneCicloComb { get; set; }
    }

    public class RegistroCambioPR25 
    {
        public bool TieneCambio { get; set; }
        public int TipoComparacion { get; set; }
        public string Campo { get; set; }

        public IndReporteTotalDTO RegTot1 { get; set; }
        public IndReporteTotalDTO RegTot2 { get; set; }

        public IndReporteDetDTO RegDet1 { get; set; }
        public IndReporteDetDTO RegDet2 { get; set; }

        public IndRelacionRptDTO RegRel1 { get; set; }
        public IndRelacionRptDTO RegRel2 { get; set; }

        public PfReporteTotalDTO RegPfTot1 { get; set; }
        public PfReporteTotalDTO RegPfTot2 { get; set; }
    }

    public class ReiniciarRegla7D 
    {
        public int Equicodi { get; set; }
        public int Equipadre { get; set; }
        public DateTime FechaReinicio { get; set; }
    }

    public class OmitirExcesoPr
    {
        public int Equicodi { get; set; }
        //esta fecha es las 00:00 del mantto programado seleccionado
        public DateTime FechaOmision { get; set; }
    }

    public class ConsumoHorarioCombustible
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int? Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public int? Equicodi { get; set; }
        public string Equinomb { get; set; }
        public int Equipadre { get; set; }
        public int Fenergcodi { get; set; }
        public int Grupoincremental { get; set; }

        public decimal? PotEfectiva { get; set; }
        public decimal? ConsumoPotEfectiva { get; set; }
        public decimal? PotParcial1 { get; set; }
        public decimal? ConsumoPotParcial1 { get; set; }
        public decimal? PotParcial2 { get; set; }
        public decimal? ConsumoPotParcial2 { get; set; }
        public decimal? PotParcial3 { get; set; }
        public decimal? ConsumoPotParcial3 { get; set; }
        public decimal? PotParcial4 { get; set; }
        public decimal? ConsumoPotParcial4 { get; set; }
        public decimal? PotParcial5 { get; set; }
        public decimal? ConsumoPotParcial5 { get; set; }
        public decimal? Rendimiento { get; set; }
        public decimal? PotMinima { get; set; }

        public decimal PendienteM01 { get; set; }
        public decimal CoeficienteIndependiente { get; set; }
        public decimal CoeficienteCorrelacion { get; set; }
        public List<decimal> ListaX { get; set; } //potencia
        public List<decimal> ListaY { get; set; } //consumo

        public string FechaDescPotEfectiva { get; set; }
        public string FechaDescConsumoPotEfectiva { get; set; }
        public string FechaDescPotParcial1 { get; set; }
        public string FechaDescConsumoPotParcial1 { get; set; }
        public string FechaDescPotParcial2 { get; set; }
        public string FechaDescConsumoPotParcial2 { get; set; }
        public string FechaDescPotParcial3 { get; set; }
        public string FechaDescConsumoPotParcial3 { get; set; }
        public string FechaDescPotParcial4 { get; set; }
        public string FechaDescConsumoPotParcial4 { get; set; }
        public string FechaDescPotParcial5 { get; set; }
        public string FechaDescConsumoPotParcial5 { get; set; }
        public string FechaDescRendimiento { get; set; }
        public string FechaDescPotMinima { get; set; }

        public int Tipoinfocodi { get; set; }
        public string UnidadMedida { get; set; }
        public DateTime FechaDia { get; set; }
        public bool ExisteCombRestante { get; set; }
        public decimal? CombRestante { get; set; } //consumo disponible
        public decimal? CDisp { get; set; } //consumo disponible
        public decimal? CPe { get; set; } //Consumo combustible a Potencia Efectiva
        public decimal? Pe { get; set; }
        public decimal? Pasg { get; set; }
        //Pcp: Potencia Activa con el combustible principal.
        public decimal? Pcp { get; set; }
        //Pca:Potencia Activa con el combustible alternativo.
        public decimal? Pca { get; set; }

        public bool ChEsPrincipal { get; set; }
        public int? ChGrupocodi { get; set; }
        public int? ChEquicodi { get; set; }

        public bool EsUnidadEspecial { get; set; }
    }

    public class CalculoEgenerable
    {
        public DateTime FechaDia { get; set; }
        public EqEquipoDTO Unidad { get; set; }

        public decimal Egenerable { get; set; }

        public List<DetalleEgenerable> ListaDetalle { get; set; } = new List<DetalleEgenerable>();
    }

    public class DetalleEgenerable
    {
        public int TminMinimaCarga { get; set; }
        public int TminPlenaCarga { get; set; }

        public decimal EnergiaGenerable { get; set; }
        public decimal CombustibleTotal { get; set; }
        public decimal CombustibleRestante { get; set; }
        public decimal CombustibleMinimaCarga { get; set; }
        public decimal CombustiblePlenaCarga { get; set; }

        public int Fenergcodi { get; set; }
    }
}
