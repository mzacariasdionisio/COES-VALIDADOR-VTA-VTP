using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Siosein2
{
    public class ConstantesSiosein2
    {
        public const int GWh = 58;
        public const string NoEnvio = "NO ENVIÓ";
        #region Nunmerales
        public const int ConceptoHidraulica = 1;
        public const int ConceptoTermica = 2;
        public const int ConceptoCogeneracion = 3;
        public const int ConceptoRer = 4;
        public const int ConceptoImportacion = 5;
        public const int ClasicodiDesvioMensual = 9;
        public const int ClasicodiDesvioSemanal = 5;
        public const int ClasicodiDesvioDiario = 4;
        public const int ClasicodiDesvioAnual = 11;
        public const int ClasicodiReal = 3;

        public const int ClasicodiPrevistoMensual = 8;
        public const int ClasicodiPrevistoSemanal = 2;
        public const int ClasicodiPrevistoDiario = 1;
        public const int ClasicodiPrevistoAnual = 10;

        #endregion

        #region Mape

        public const int EstadoGenerado = 1;
        public const int EstadoValidado = 2;
        public const int EstadoBaja = 3;

        public const int IdReporteDemandaGrandesUsuariosNorte = 24;
        public const int IdReporteDemandaGrandesUsuariosSur = 25;
        public const int IdReporteDemandaGrandesUsuariosCentro = 26;
        public const int IdReporteDemandaGrandesUsuariosElectroandes = 27;

        public const int PtomedicodiGTotal = 1222;
        public const int PtomedicodiCoesTotal = 46715;
        public const string PtoDemandaSeinyCOES = "1222,46715";
        public const int OrigenLecturaMedicionesDespachoMediahora = 2;
        public const int OrigenLecturaDemandaeEnBarraPR03 = 6;

        public const int LecturaProgramacionDiaria = 4;
        public const int LecturaEjecutado = 6;
        public const int LecturaEjecutadoSemanal = 65;
        public const int LecturaEjecutadoHistorico = 75;
        public const int LecturaEjecutadoMensualMP = 76;
        public const int LecturaDemandaConsumidaDiaria = 45;
        public const int LecturaDemandaPronosticadaDiaria = 46;

        public const int DesviacionEstandar = 1;
        public const int Mediana = 2;
        public const int Media = 3;
        public const int Varianza = 4;

        public const int Diario = 1;
        public const int Mensual = 2;
        public const int Anual = 3;

        public const int DemandaSein = 1;
        public const int DemandaCoes = 2;
        public const int DemandaArimaSaNp = 3;
        public const int DemandaArimaCaNp = 4;
        public const int MapeSinCorregir = 5;
        public const int MapeCorregido = 6;
        public const int MapeCorregidoMax = 7;
        public const int MapeCorregidoMin = 8;
        public const int MapeSinCorregirMax = 9;
        public const int MapeSinCorregirMin = 10;

        public const double TransgresionDiario = 1.8;
        public const double TransgresionMensual = 1.7;
        public const double TransgresionAnual = 1.5;
        public const double LimiteTransgresionMape = 20;

        public const string Cumplio = "CUMPLIO";
        public const string Incumple = "INCUMPLE";
        public const string FormatoPorcentajeExcel = "#,##0.00%";
        public const string FormatoDecimalExcel = "# ##0.00";
        public enum TipoCalculo
        {
            TotalReal = 1,
            TotalCorregido = 2,
            Centro = 3,
            Norte = 4,
            Sur = 5,
            Electroande = 6,
            Desvio = 7,
            DemandaReal = 8,
            DemandaProgramada = 9,
            DemandaAjustada = 10,
            DemandaRealCoes = 11
        }

        public static Dictionary<int, TipoCalculo> TipoCalculosPorReportcodi = new Dictionary<int, TipoCalculo>
        {
            {24, TipoCalculo.Norte},
            {25, TipoCalculo.Sur},
            {26, TipoCalculo.Centro},
            {27, TipoCalculo.Electroande}
        };

        public const string NombreExcelEvolucionMapeMensual = "EvoluciónMapeMensual";
        public const string NombreExcelEvolucionMapeSemanalDiario = "EvoluciónMapeSemanalDiario";
        public const string NombreExcelMapeVersion = "MapeVersion";
        public const string NombrePalntillaExcelMape = "PlantillaMape";

        #endregion

        #region Menu

        public const int ReporteEjecutivo = 10;

        #endregion

        #region REPORTE EJECUTIVO

        public const string PantillaReporteEjecutado = "PLT_INFORME_EJE.xlsx";
        public const string PantillaReporteFise = "PLT_INFORME_FISE.xlsx";
        public const string TipoGeneracionRerDefault = "-1";

        public const int FamcodiGeneradorHidroelectrico = 2;
        public const int FamcodiGeneradorTermoelectrico = 3;
        public const int FamcodiCentralHidroelectrico = 4;
        public const int FamcodiCentralTermoelectrico = 5;
        public const int FamcodiGeneradorSolar = 36;
        public const int FamcodiCentralSolar = 37;
        public const int FamcodiGeneradorEolico = 38;
        public const int FamcodiCentralEolico = 39;

        public const int TgenercodiHidroelectrica = 1;
        public const int TgenercodiTermoelectrica = 2;

        public const string NombreMaximaDemanda = "MÁXIMA POTENCIA DEL DIA";

        //constantes solomanete para el calculo
        public const int Total = -1;
        public const int TotalAnterior = -2;
        public const int MaximoCoindidente = -3;
        public const int MaximoCoindidenteAnt = -4;
        public const int MaximoCoindidenteAnual = -5;
        
        public const string ColorFilaTablaRepEje = "#D9E1F2";
        public const string ColorBordeTablaRepEje = "#2F75B5";
        public const string ColorTotTablaRepEje = "#8EA9DB";

        /// <summary>
        /// Tipo de calculo produccion energia electrica (Columnas del excel)
        /// </summary>
        public enum TipoMedicion
        {
            Medicion24 = 1,
            Medicion48 = 2,
            Medicion96 = 3,
        }

        /// <summary>
        /// 
        /// </summary> 
        public enum TipoHora
        {
            HxInicio,
            HxFin
        }

        /// <summary>
        /// Zonas de barras del SEIN
        /// </summary>
        public enum BarraZona
        {
            Norte = 1,
            Centro = 2,
            Sur = 3
        }

        public enum TablaOrigenPunto
        {
            Medicion1 = 1,
            Medicion24 = 24
        }


        public const int FilaCuadroHidroelectrica = 1;
        public const int FilaCuadroTermoelectrica = 2;
        public const int FilaCuadroRenovable = 3;
        public const int FilaCuadroTotalGeneracion = 4;
        public const int FilaCuadroTotalSein = 5;
        public const int FilaCuadroExportacion = 6;
        public const int FilaCuadroImportacion = 7;
        public const int FilaCuadroTotalTIE = 8;
        public const int FilaCuadroTotalGeneracionNoRER = 9;
        public const int FilaCuadroTotalGeneracionSiRER = 10;

        public const int FenergcodiAGUA = 1;
        public const int FenergcodiGAS = 2;
        public const int FenergcodiDIESELB5 = 3;
        public const int FenergcodiRESIDUAL = 4;
        public const int FenergcodiCARBON = 5;
        public const int FenergcodiBAGAZO = 6;
        public const int FenergcodiBIOGAS = 7;
        public const int FenergcodiSOLAR = 8;
        public const int FenergcodiEOLICA = 9;
        public const int FenergcodiRESIDUALR500 = 10;
        public const int FenergcodiRESIDUALR60 = 11;

        //Constante solamente usado en la programación
        public const int FenergcodiRER = -2;
        public const int FenergcodiRERConvencional = 100;
        public const int FenergcodiRERNoConvencional = 101;
        public const int FenergcodiNoAplicaRER = 102;
        public const int FenergcodiGasCamisea = 103;
        public const int FenergcodiGasMalacas = 104;
        public const int FenergcodiGasAguaytia = 105;
        public const int FenergcodiGasLaIsla = 106;
        public const int FenergcodiAguaRER = 107;
        public const int FenergcodiPasada = 108;
        public const int FenergcodiRegulacion = 109;
        public const int FenergcodiRelevanteResidualYDiesel = 110;
        public const int FenergcodiRelevanteBiogasBagazo = 111;
        public const int FenergcodiGasNoCamisea = 112;
        public const int FenergcodiRelevanteResidual = 113;
        public const int FenergcodiTermoelectrico = 114;

        public const int CtgcodiTipoTecnologiaMaximaPotencia = 4;
        public const int CtgcodiYacimientoGas = 1;
        public const int CtgcodiTipoFuenteEnergiaGas = 3;
        public const int CongestionEquiposTransmision = 30;

        public static int RenertipRER = 12;

        public static DateTime FechaInicioEmpresaIntegrante = new DateTime(2008, 07, 31);

        public static int MocmtipocombLiquido = 1;
        public static int MocmtipocombGasNatural = 2;
        public static int RevisionMensual = 1;

        public const int PtomedicodiEmbalseJunin = 40726;
        public const int MrepcodiDemandaEnergiaZonaNorte = 229;
        public const int MrepcodiDemandaEnergiaZonaCentro = 230;
        public const int MrepcodiDemandaEnergiaZonaSur = 231;
        #endregion

        #region BOLETIN


        public const int TmopercodiFusion = 4;
        public const int TmopercodiCambioRazonSocial = 5;

        #endregion

        #region NUMERALES

        public const int NumccodiCostoMensualTotalArea = 33;
        public const int NumccodiEnergiaMensualArea = 34;

        public const int NumecodiEmbalses = 3;
        public const int NumecodiCaudales = 4;

        public const int SconcodiEmbalseJunin = 18;

        public const int ClasicodiNORTE = 12;
        public const int ClasicodiCENTRO = 13;
        public const int ClasicodiSUR = 14;

        public const int TptomedicodiGeneracionMwh = 69;
        public const int TptomedicodiCostoSDDP = 79;
        public const int TptomedicodiCostoCmgUSDxMWh = 80;
        public const int TptomedicodiVolumenFinal = 74;
        public const int TptomedicodiCaudales = 72;
        public const int PtomedicodiSANTAROSA220 = 25047;

        public const int PtomedicodiCmgStaRosaNCP = 1226;

        public const int GrupocodiCTParamonga = 71;
        public const int GrupocodiModoParamonga = 309;
        public const int GrupocodiDespachoParamonga = 417;

        public const int GrupocodiSanJacinto = 568;
        public const int GrupocodiSanJacinto2 = 569;
        public const int GrupocodiMaple = 432;
        public const int GrupocodiCBrava = 572;
        public const int GrupocodiOquendo = 64;

        public enum TipoDemanda
        {
            DiariaEjecutadaAnterior = 45,
            DiariaProgAnterior = 46,
            SemanalProgAnterior = 47,

            DiariaEjecutada = 103, 
            DiariaProg = 110, 
            SemanalProg = 102 
        }

        public enum CausaHoperacion : int
        {
            RequerimientoPropio = 1,
            Seguridad = 2,
            Tension = 3,
            EvitarArranqueParada = 4,
            Otros = 10
        }

        public static Dictionary<CausaHoperacion, string> CausaHoperacionDescripcion = new Dictionary<CausaHoperacion, string>
        {
            { CausaHoperacion.RequerimientoPropio , "Requierimiento Propio" },
            { CausaHoperacion.Seguridad , "Seguridad" },
            { CausaHoperacion.Tension , "Tensión" },
            { CausaHoperacion.EvitarArranqueParada , "Evitar Arranque/Parada" },
            { CausaHoperacion.Otros , "Otros" },
        };

        public enum ZonaPtomedicodi : int
        {
            Norte = ConstantesSiosein2.PtomedicodiGenerZonaNorte,
            Centro = ConstantesSiosein2.PtomedicodiGenerZonaCentro,
            Sur = ConstantesSiosein2.PtomedicodiGenerZonaSur,
        }

        public static Dictionary<ZonaPtomedicodi, string> ZonaDescripcion = new Dictionary<ZonaPtomedicodi, string>
        {
            { ZonaPtomedicodi.Norte , "Norte" },
            { ZonaPtomedicodi.Centro , "Centro" },
            { ZonaPtomedicodi.Sur , "Sur" }
        };

        public static Dictionary<ZonaPtomedicodi, int> ZonaOrden = new Dictionary<ZonaPtomedicodi, int>
        {
            { ZonaPtomedicodi.Norte , 1 },
            { ZonaPtomedicodi.Centro , 2},
            { ZonaPtomedicodi.Sur , 3 }
        };

        public static Dictionary<int, string> HorizonteProgramacion = new Dictionary<int, string>
        {
            { ConstantesAppServicio.EvenclasecodiProgAnual , "Previsto Anual" },
            { ConstantesAppServicio.EvenclasecodiProgMensual , "Previsto Mensual"},
            { ConstantesAppServicio.EvenclasecodiProgSemanal , "Previsto Semanal"},
            { ConstantesAppServicio.EvenclasecodiProgDiario , "Previsto Diario"},
            { ConstantesAppServicio.EvenclasecodiEjecutado , "Ejecutado"},
        };

        public enum UsoHorario
        {
            Base = 1,
            Media = 2,
            Punta = 3,
            Error = -1
        }

        public enum TipoEmpresa
        {
            Transmision = 1,
            Distribucion = 2,
            Generacion = 3,
            UsuariosLibres = 4,
        }
        public const int TipoemprcodiUsuariosLibres = 4;
        public const int TipoemprcodiGeneracion = 1;
        public const int TipoemprcodiTransmision = 2;
        public const int TipoemprcodiDistribucion = 3;

        public const string TipoUrsConReserva = "C";
        public const string TipoUrsSinReserva = "S";

        public const int PtomedicodiGenerZonaCentro = 46710;
        public const int PtomedicodiGenerZonaNorte = 46711;
        public const int PtomedicodiGenerZonaSur = 46712;

        public const int PtomedicodiComplejoArcata = 47549;

        public const int PtomedicodiQN2002_3_4_5 = 46705;
        public const int PtomedicodiQN811_12_14_15 = 46706;
        public const int PtomedicodiQN806_7_10 = 46707;
        public const int PtomedicodiQN805_9 = 46708;
        public const int PtomedicodiQN901_2_3_4_5_11 = 46709;

        public const int PtomedicodiQN_SH1 = 47519;

        public static Dictionary<int, List<decimal?>> TacometroNumeralTickPositions = new Dictionary<int, List<decimal?>>
        {
            { 1, new List<decimal?>() { 0, 3, 5, 7, 10 } },
            { 2, new List<decimal?>() { 0, 5, 10, 20, 25 } },
            { 3, new List<decimal?>() { 0, 5, 15, 25, 50 } },
            { 4, new List<decimal?>() { 0, 10, 15, 25, 40 } },
            { 7, new List<decimal?>() { 0, 25, 30, 35, 40 } },
            { 8, new List<decimal?>() { 0, 10, 15, 25, 30 } },
            { 9, new List<decimal?>() { 0, 1, 2, 3, 4 } },
        };

        public static Dictionary<int, decimal?> TacometroNumeralTickPositionsExtend = new Dictionary<int, decimal?>
        {
            { 1, 20 },
            { 2, 40 },
            { 3, 70 },
            { 4, 60 },
            { 7, 60 },
            { 8, 50 },
            { 9, 10 },
        };

        public static List<string> ListaColoresTacometro = new List<string>() { "#00B050", "#FFFF00", "#FF0000", "#C00000", "#630F01" };

        #endregion

        #region SIOSEIN
        public const int FunptocodiDiaAnterior = 1;
        #endregion
        public const int VtranversionMensual = 1;

        public const int TiporelcodiEmbalseCH = 27;
        public const int ToptipoProgramadoDiario = 1;
        public const int ToptipoProgramadoSemanal = 2;
        public const int SubrestriccionVertimientoEmbalse = 67;
    }




    #region Numerales Datos Base
    public class DataNumeral_5_10
    {
        public string Motivo { get; set; }
        public decimal? Valor { get; set; }
        public string Mes { get; set; }
    }

    public class DataNumeral_5_1_Programado
    {
        public DateTime Medifecha { get; set; }
        public decimal? Cmgpunta { get; set; }
        public decimal? Cmgmedia { get; set; }
        public decimal? Cmgbase { get; set; }
    }

    public class DataNumeral_5_2_Real
    {
        public DateTime Desvfecha { get; set; }
        public decimal? Cmgrpunta { get; set; }
        public decimal? Cmgrmedia { get; set; }
        public decimal? Cmgrbase { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
    }

    #endregion

    public class DataCabeceraREProdEmpGen
    {
        public DateTime DiaMaximaDemanda = DateTime.MinValue;
        public DateTime DiaMaximaDemandaAnhoAnt = DateTime.MinValue;
        public DateTime DiaMaximaDemandaAnual = DateTime.MinValue;
    }
    public class DataCuerpoREProdEmpGen
    {
        public string NombEmpresa = "";
        public decimal? ValorHidroGWh = 0.00m;
        public decimal? ValorTermoGWh = 0.00m;
        public decimal? ValorRerGWh = 0.00m;
        public decimal? ValorTotalGWh = 0.00m;
        public decimal? ValorTotalAnhoAntGwh = 0.00m;
        public decimal? ValorPorcentualProducEnergia = 0.00m;
        public decimal? ValorMaxPotenciaCoinci = 0.00m;
        public decimal? ValorMaxPotenciaCoinciAnhoAnt = 0.00m;
        public decimal? ValorMaxPotenciaCoinciAnual = 0.00m;
        public decimal? ValorPorcentualMaxPotenciaCoinci = 0.00m;
    }
    public class DataReporteGeneracionEmpresas
    {
        //TOTAL
        public decimal? ValorTHidroGWh = 0.00m;
        public decimal? ValorTTermoGWh = 0.00m;
        public decimal? ValorTRerGWh = 0.00m;
        public decimal? ValorTTotalGWh = 0.00m;
        public decimal? ValorTTotalAnhoAntGwh = 0.00m;
        public decimal? ValorTPorcentualProducEnergia = 0.00m;
        public decimal? ValorTMaxPotenciaCoinci = 0.00m;
        public decimal? ValorTMaxPotenciaCoinciAnhoAnt = 0.00m;
        public decimal? ValorTMaxPotenciaCoinciAnual = 0.00m;
        public decimal? ValorTPorcentualMaxPotenciaCoinci = 0.00m;

        //IMPORTACION
        public decimal? ValorInterconexionImpo = 0.00m;
        public decimal? ValorInterconexionImpoAnhoAnt = 0.00m;
        public decimal? ValorPorcentualImpo = 0.00m;
        public decimal? ValorMaxpCoincImpo = 0.00m;
        public decimal? ValorMaxpCoincImpoAnhoAnt = 0.00m;
        public decimal? ValorMaxpCoincImpoAnual = 0.00m;

        //EXPORTACION
        public decimal? ValorInterconexionExpo = 0.00m;
        public decimal? ValorInterconexionExpoAnhoAnt = 0.00m;
        public decimal? ValorPorcentualExpo = 0.00m;
        public decimal? ValorMaxpCoincExp = 0.00m;
        public decimal? ValorMaxpCoincExpAnhoAnt = 0.00m;
        public decimal? ValorMaxpCoincExpoAnual = 0.00m;

        //TOTALES INTERCAMBIOS INTERNACIONALES
        public decimal? Cal1 = 0.00m;
        public decimal? Cal2 = 0.00m;
        public decimal? Cal3 = 0.00m;
        public decimal? Cal4 = 0.00m;
        public decimal? Cal5 = 0.00m;

        //TOTAL DEMANDA SEIN
        public decimal? ValorProducEnergiaTActual = 0.00m;
        public decimal? ValorProducEnergiaTAnterior = 0.00m;
        public decimal? ValorMagnitudPorcentual = 0.00m;
        public decimal? ValorMaxPotCoincidente = 0.00m;
        public decimal? ValorMaxPotCoincidenteAnterior = 0.00m;
        public decimal? ValorMagnitudCoincidente = 0.00m;


    }
    public class Tabla
    {


        public string columna1 { get; set; }
        public decimal? columna2 { get; set; }
        public decimal? columna3 { get; set; }
        public decimal? columna4 { get; set; }
        public decimal? columna5 { get; set; }
        public decimal? columna6 { get; set; }
        public decimal? columna7 { get; set; }
        public decimal? columna8 { get; set; }
        public decimal? columna9 { get; set; }
        public decimal? columna10 { get; set; }
    }

    [Serializable]
    public class DemandaEnergiaZona
    {
        public string Barra { get; set; }
        public decimal? ValDemandaAct { get; set; }
        public decimal? ValDemandaAnt { get; set; }
        public decimal? ValVariacionPorcen { get; set; }
        public decimal? ValDemandaAcumulAct { get; set; }
        public decimal? ValDemandaAcumulAnt { get; set; }
        public decimal? ValParticipacion { get; set; }
        public decimal? ValVariacionAcum { get; set; }
    }
}

