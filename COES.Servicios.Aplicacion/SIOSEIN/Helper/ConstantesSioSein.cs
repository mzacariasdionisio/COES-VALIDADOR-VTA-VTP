using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.SIOSEIN
{
    public class ConstantesSioSein
    {
        public const string TipoEmpresa = "3";
        public const int IdTipoexcepcionninguno = 1;
        public const int IdTipoexcepcionSistAislado = 2;
        public const int NrofilasCabecera = 1;
        public const int RecalculoMensual = 1;

        public const int GeneHidro = 2;
        public const int GeneTermo = 3;
        public const int GeneSolar = 36;
        public const int GeneEolic = 38;
        public const int ReptipcodiMensual = 6;
        public const int ReptipcodiAnual = 7;
        public const int ReptipcodEjecutivoMensual = 10;

        //Tablas Prie
        public const int cosmarversion = 1;
        public const string CaracterCosMar = "CosMar";
        public const int LectCodiPrie01 = 201;
        public const int LectCodiPrie03 = 1;
        public const int LectCodiPrie04 = 1;
        public const int LectCodiPrie26 = 108;
        public const int LectCodiPrie27 = 107;
        public const int LectCodiPrie28 = 109;
        public const int LectCodiPrie29 = 110;
        public const int LectCodiPrie33 = 4;
        public const int idFormato = 83;
        public const int TipoInfoCodi = 1;
        public const int TipoInfoCodiMW = 1;
        public const string TipoInfoCodiPrie04 = "1,2";
        public const int Origlectcodi = 27;
        public const int ProgramacionSemanal = 3;
        public const int ProgramacionDiaria = 4;
        public const string ReporteResumen30 = "RR30";
        public const string ReporteResumen33 = "RR33";
        public const string Reporte33MaxDemanEmpresa = "R33MDE";
        public const string Reporte30MaxDemanEmpresa = "R30MDE";
        public const string Reporte33MaxDemanTecnologia = "R33MDT";
        public const string Reporte30MaxDemanTecnologia = "R30MDT";
        public const string ReporteLiquido23 = "RL23";
        public const string ReporteBagazo23 = "RB23";
        public const string ReporteCarbon23 = "RC23";
        public const string ReporteGas23 = "RG23";
        public const string ContentTXT23 = "CTXT23";
        public const string Liquidos = "1,3,4,10,11";
        public const string Mensual = "1";
        public const string Trimestral = "2";
        public const string Anual = "3";
        //public const string GasNatural = "2";
        //public const string Carbon = 

        public const int nColumnPri01 = 5;
        public const int nColumnPri03 = 32;
        public const int nColumnPri26 = 13;
        public const int nColumnPri27 = 15;
        public const int nColumnPri28 = 14;
        public const int nColumnPri29 = 15;

        public const string EmprHidro = "2,4";
        public const string EmprTermi = "3,5";
        public const string EmprSolar = "36,37";
        public const string EmprEolic = "38,39";
        public const string PathArchivoExcel = "Areas/Siosein/Temporal/";

        public const int Prie01 = 1;
        public const int Prie02 = 2;
        public const int Prie03 = 3;
        public const int Prie04 = 4;
        public const int Prie05 = 5;
        public const int Prie06 = 6;
        public const int Prie07 = 7;
        public const int Prie08 = 8;
        public const int Prie09 = 9;
        public const int Prie10 = 10;
        public const int Prie11 = 11;
        public const int Prie12 = 12;
        public const int Prie13 = 13;
        public const int Prie14 = 14;
        public const int Prie15 = 15;
        public const int Prie16 = 16;
        public const int Prie17 = 17;
        public const int Prie18 = 18;
        public const int Prie19 = 19;
        public const int Prie20 = 20;
        public const int Prie21 = 21;
        public const int Prie22 = 22;
        public const int Prie23 = 23;
        public const int Prie24 = 24;
        public const int Prie25 = 25;
        public const int Prie26 = 26;
        public const int Prie27 = 27;
        public const int Prie28 = 28;
        public const int Prie29 = 29;
        public const int Prie30 = 30;
        public const int Prie31 = 31;
        public const int Prie32 = 32;
        public const int Prie33 = 33;
        public const int Prie34 = 34;
        public const int Prie35 = 35;
        public static char[] SplitPrie = { '@', '/', '|', '\'' }; //SIOSEIN-PRIE-2021
        public const string IniDiaFecha = "01/";

        //Tabla Prie 17
        public const int IdExportacionL2280MWh = 41238;//41103;// 
        public const int IdImportacionL2280MWh = 41239;//41104;//
        public const int IdExportacionL2280MVARr = 41240;//41105;// 
        public const int IdImportacionL2280MVARr = 41241;//41106;//
        public const string Horizonte = "Horizonte";
        public const string TipoOperacion = "TipoOperacion";
        public const int evenClase = 1;
        public const int subCausacodi = 219;
        public const string GraficoEvolucionEnergia = "GEE";
        public const string GraficoContratoIntercambios = "GCI";

        //Tabla Prie 03
        public const int TipoInfocodi03 = 1;
        public const int origlectcodi03 = 1;
        public const string tGenercodi03 = "4,1,3,2";


        //Tabla Prie 04
        public const int concepcodiPE = 14;
        public const int concepcodiCVC = 27;
        public const int concepcodiCVNC = 62;
        public const int concepcodiCV = 35;
        public const int concepcodiCarbon = 221;
        public const int concepcodiDB5 = 207;
        public const int concepcodiR500 = 214;
        public const int concepcodiR6 = 208;
        public const int concepcodiGas = 191;

        public static string ConcepabrevCVC = "Cvc";
        public static string ConcepabrevCVNC = "CVNC";
        public static string ConcepabrevCV = "CVsoles";
        public static string ConcepabrevCarbon = "CTotalCarb";
        public static string ConcepabrevDB5 = "CTotalDB5";
        public static string ConcepabrevR500 = "CTotalR500";
        public static string ConcepabrevR6 = "CTotalR6";
        public static string ConcepabrevGas = "CCombGas_SI";
        public static string ConcepabrevTCambio = "TCambio";
        public static string ConcepabrevCcomb = "CComb";
        public static string ConcepabrevCCombAlt = "CCombAlt";
        public static string ConcepabrevPCI = "PCI_SI";
        public static string ConcepabrevCVNC_US = "CVNC_US";
        public static string ConcepabrevCombArSin = "CombArSin";
        public static string ConcepabrevCombSinPC = "CombSinPC";
        public static string ConcepabrevComPCSinc = "ComPCSinc";
        public static string ConcepabrevComSinPar = "ComSinPar";
        public static string ConcepabrevCombArSinAlt = "CombArSinAlt";
        public static string ConcepabrevCombSinPCAlt = "CombSinPCAlt";
        public static string ConcepabrevComSinParAlt = "ComSinParAlt";
        public static string ConcepabrevComPCSinAlt = "ComPCSinAlt";
        public static string ConcepabrevCMarrPar = "CMarrPar";
        public static string ConcepabrevCcomb_x_arr = "Ccomb_x_arr";
        public static string ConcepabrevCcomb_x_par = "Ccomb_x_par";
        public static string ConcepabrevPe = "Pe";
        public const int FactorConversionPot30minToEnergiaHora = 2;

        public const int TipoUnidadTV = 2;
        public const int TipoUnidadNoTVCComb = 1;

        // Tabla Prie 07
        public static string EntGeneracion = "EG";
        public static string RetContBilateral = "CB";
        public static string RetContLicitacion = "CL";
        public static string RetSinContrato = "CS";


        // Tabla Prie 18
        public const int lectcodi18 = 75;

        // Tabla Prie 26
        public const string idHidroelectrica = "4";
        public const string idTermoelectrica = "5";
        public const string idRer = "37,38,39";
        public const string idSolares = "37";
        public const string idEolica = "39";
        public const string tipoIntegrantes = "1";
        public const string tGRerGenNoInt = "2,3,10";
        public const string FamRerGenNoInt = "4,5,37,39";
        public const int PtoMedNuevo = 0;

        // Tabla Prie 27
        public const int TipoInfocodi27 = 2;
        public const string OsiCodStaRosa = "B0016";
        public const string OsiCodSocab = "B0050";
        public const string OsiCodTrujillo = "B0007";
        // Tabla Prie 28
        public const int TipoInfocodi28 = 3;
        // Tabla Prie 29
        public const int TipoInfocodi29 = 4;

        // Tabla Prie 32
        public const string PtoMedicodi32 = "23515,23306,43157,771,770";
        public const int LectCodiDia = 4;
        public const int LectCodiSem = 3;

        // Tabla prie 34
        public const string idTipoGener34 = "2";
        public const int famcodiHidroElectrica = 4;
        public const int famcodiSolar = 37;
        public const int famcodiEolico = 39;
        public const string famHidroTermo = "4,5,37,39";
        public const string tipoCentralH = "H";
        public const int LectCodiEjec = 6;

        //Costo Operacion
        public const int famcodiTermoElectrica = 5;
        public const int IdGeneradorHidroelectrico = 2;
        public const int IdGeneradorTermoelectrico = 3;
        public const int IdGeneradorSolar = 36;
        public const int IdGeneradorEolico = 38;
        public const int idTipoGeneracion = 2;
        public const int IdFamiliaSSAA = 40;
        public const int IdTipogrupoNoIntegrante = 10;
        public const string FormatFecha = "dd/MM/yyyy";
        public const string FormatAnioMesDia = "yyyyMMdd";
        public const string FormatAnioMes = "yyyyMM";
        public const string FormatFechaHoraMin = "dd/MM/yyyy HH:mm:ss";
        public const string FormatFechaHora = "dd/MM/yyyy HH:mm"; //SIOSEIN-PRIE-2021
        public const string GenHidro = "1";
        public const string GenTermo = "11,6,7,4,3,2,5,10";
        public const string GenSolar = "8";
        public const string GenEolica = "9";

        public const string tipoCentral = "T";

        //Reporte tabla 16
        public static int[] TipoEmpresas = { 3, 1, 2, 4 };
        public static char[] SplitComa = { ',' };
        public const string ReporteTipoEmpresa = "RTE";
        public const string ReporteTipoCausa = "RTC";

        public const int CodFallNoIdentificado = -1;
        public const int CodFallExterna = 2;
        public const int CodFallFenNatural = 3;
        public const int CodFallHumana = 4;
        public const int CodFallEquipo = 5;
        public const int CodFallSisProtec = 6;
        public const int CodFallOtros = 8;

        public const int CodGeneracion = 3;
        public const int CodTransmision = 1;
        public const int CodDistribucion = 2;
        public const int CodUsuarioLibre = 4;
        //TIPOS DE EMPRESA
        public const int CodTipEmprGeneracion = 3;
        public const int CodTipEmprTransmision = 1;
        public const int CodTipEmprDistribucion = 2;
        public const int CodTipEmprUsuarioLibre = 4;

        //TIPOS DE EQUIPO
        //--** LINEA DE TRANSMISION     => 8
        //--** CENTRAL TERMOELECTRICA   => 5
        //--** TRANSFORMADOR3D          => 10
        //--** SUBSTACIÓN               => 1
        //--** BARRA                    => 7
        //--** TRANSFORMADOR2D          => 9
        //--** CENTRAL HIDROELECTRICA   => 4
        //--** INSTALACION DE CLIENTE   => 32
        //--** GENERADOR TERMOELECTRICA => 3
        //--** CELDA                    => 6
        public const int CodTipEquipLineaTransmision = 8;
        public const int CodTipEquipTransformador2D = 9;
        public const int CodTipEquipTransformador3D = 10;
        public const int CodTipEquipGenTermoelectrico = 3;
        public const int CodTipEquipCenTermoelectrico = 5;
        public const int CodTipEquipCenHidroelectrico = 4;
        public const int CodTipEquipBarra = 7;
        public const int CodTipEquipSubestacion = 1;
        public const int CodTipEquipCelda = 6;
        public const int CodTipEquipCliente = 32;

        //REPORTE POR GRUPOS DE TIPO EQUIPO
        public const string LineasDeTransmision = "LINEAS DE TRANSMISIÓN";
        public const string Transformadores = "TRANSFORMADORES";
        public const string Barras = "BARRAS";
        public const string UnidadesDeGeneracion = "UNIDADES DE GENERACIÓN";

        //Reporte tabla 22
        public const int LectcodiEjecutadoHisto = 75;


        //Reporte tabla 25
        public const string IngresoGenerador = "Ingreso";
        public const string RetiroGenerador = "Retiro";

        //Reporte tabla 30
        public const string ReporteGeneracionPorInsumo = "RGPI";
        public const string VerificacionSiEsGas = "Gas";
        public const string VerificacionNoEsGas = "NoGas";
        public const string GasCamisea = "GAS DE CAMISEA";
        public const int CodigoFuenteGas = 15;
        public const string Agua = "AGUA";
        public const string Eolico = "EOLICA";
        public const string Residual = "RESIDUAL";
        public const string Bagazo = "BAGAZO";
        public const string Carbon = "CARBON";
        public const string Diesel2 = "DIESEL B5";
        public const string strGrupoReservaFria = "Reserva Fría";
        public const string strGrupoEmergencia = "Emergencia";
        public const string strGrupoNodoEnergetico = "Nodo Energético del Sur";
        public const string strRer = "RER";


        public const int FuenteEnerAgua = 1;
        public const int FuenteEnerGas = 2;
        public const int FuenteEnerDieselB5 = 3;
        public const int FuenteEnerResidual = 4;
        public const int FuenteEnerCarbon = 5;
        public const int FuenteEnerBagazo = 6;
        public const int FuenteEnerBiogas = 7;
        public const int FuenteEnerSolar = 8;
        public const int FuenteEnerEolica = 9;
        public const int FuenteEnerResidualR500 = 10;
        public const int FuenteEnerResidualR6 = 11;

        //Reporte tabla 05
        public const int EnergiaReactiva = 2;
        public const int EnergiaActiva = 1;

        public const int TgenerTermoelectrica = 2;
        public const int TgenerSolar = 3;
        public const int TgenerEolica = 4;
        public const int TgenerHidroelectrica = 1;
        public const string ReporteResumen05 = "RR05";
        public static int GrupoRenovable = 2;



        public static string GrupoReservaFria = "GRUPORESERVAFRIA";
        public static string GrupoEmergencia = "GRUPOEMERGENCIA";
        public static string GrupoNodoEnergetico = "GRUPONODOENERGETICO";

        public static string CateRenovables = "RENOVABLES";
        public static string CateEmergencia = "EMERGENCIA";
        public static string CateReservaFria = "RESERVA FRÍA";
        public static string CateNodoEnergetico = "NODO ENERGÉTICO DEL SUR";

        //public static string Reporte30MaxDemanEmpresa = "R30MDE";
        public static string Reporte05MaxDemanEmpresa = "R05MDE";
        public static string Reporte05MaxDemanTecnologia = "R05MDT";
        public const string ReporteContenidoTXT = "RCTXT";
        public static string Reporte05IntercambioEnerg = "R05IE";
        public static string NoReR = "N";
        public static string Hidro = "HIDRO";
        public static string HidroRER = "HIDRO (RER)";
        public static string Hidroelectricas20MV = "HIDROELÉCTRICAS < 20 MW";
        public static string Hidrico = "HÍDRICO";
        public static string Hidroelectricas = "HIDROELÉCTRICAS";
        public static string MotoresDiesel = "MOTORES DIESEL";
        public static string Solar = "SOLAR";
        public static string SiReR = "S";
        public static string BagazoReR = "BAGAZO (RER)";
        public static string BiogasReR = "BIOGÁS (RER)";
        public static string CarbonReporte = "CARBÓN";
        public static string EolicoRer = "EÓLICO (RER)";
        public static string Aerogenerador = "AEROGENERADOR";
        public static string Diesel2Reporte = "DIESEL 2";
        public static string MDieselBiogas = "M. DIESEL - BIOGÁS";
        public static int GasLaIsla = 27;//CTGDETCODI DEL GAS LA ISLA
        public static int GasDeAguaytia = 26;//CTGDETCODI DEL GAS DE AGUAYTIA
        public static int GasDeCamisea = 25;//CTGDETCODI DEL GAS DE CAMISEA
        public static int GasDeMalacas = 24;//CTGDETCODI DEL GAS DE MALACAS
        public static int GrupoCoGeneracion = 3;
        public static string Cogeneracion = "COGEN";//Cogeneracion
        public static string Lista = "Lista";
        public static string Grafico = "Grafico";
        public static string ExportacionPerEcu = "EXPORTACIÓN (PER-ECU)";
        public static string ImportacionEcuPer = "IMPORTACIÓN (ECU-PER)";

        public const string LectCodiMedidores = "1";
        public const string LectCodiProgDiario = "4";

        public const int fenercodiAgua = 1;
        public const int fenercodiGas = 2;
        public const int fenercodiDiesel = 3;
        public const int fenercodiResi = 4;
        public const int fenercodiCarb = 5;
        public const int fenercodiBaga = 6;
        public const int fenercodiBiogas = 7;
        public const int fenercodiSolar = 8;
        public const int fenercodiEolic = 9;
        public const int fenercodiResi500 = 10;
        public const int fenercodiResi6 = 11;
        public const int ReporcodiCauEjeDia = 63;
        public const int ReporcodiVolRsvEjeDia = 68;
        public const int ReporcodiVolLag = 65;
        public const int ReporcodiVolEmb = 64;

        public const string Tiprepcodi = "Tiprepcodi";
        public const string FechaFilter1 = "FechaFilter1";
        public const string FechaFilter2 = "FechaFilter2";
        public const string Tpriecodi = "Tpriecodi";

        public const int SubCausaEquipoOperacionIng = 344;
        public static readonly List<int> IdsCateCodis = new List<int> { 15, 16 };
        public const int TipoRecursoGas = 2;
        public const int FteenergiaGas = 14;
        public const int TipoTecnologia = 15;
        public const int FamcodiGeneradorTemo = 3;
        public const int PropPotenciaEfectiva = 1;
        public const int PropPotenciaInstalada = 2;
        public const string PropPortenciaGarantizada = "1483";
        public const int FamcodiCtralHidro = 4;
        public const int FamcodiCtralTermo = 5;
        public const int FamcodiCtralSolar = 57;
        public const int FamcodiCtralEolica = 59;
        public const int PropPotInstaladaHidro = 42;
        public const int PropPotInstaladaTermo = 49;
        public const int PropPotInstaladaEolica = 1602;
        public const int PropPotInstaladaSolar = 1710;
        public const int PropPotEfecHidro = 46;
        public const int PropPotEfectTermo = 53;
        public const int PropPotEfecEolica = 1602;
        public const int PropPotEfectSolar = 1710;
        public const int LectDespachoEjecutado = 6;
        public const int LectDespachoReprogramado = 5;
        public const int LectDespachoProgramadoDiario = 4;
        public const int LectDespachoProgramadoSemanal = 3;
        public const string EmpresacoesSi = "S";
        public const string EmpresacoesNo = "N";
        public const string TipoFteEnergiaTodos = "1,2,3,4";
        public const int TipoFteHidro = 1;
        public const int TipoFteTermo = 2;
        public const int TipoFteSolar = 3;
        public const int TipoFteEolica = 4;
        public const int FenergiaGAS = 2;
        public const int CtgcodiFteGas = 14;
        public const int SemanalProgramado = 0;
        public const int fenercodiResidu = 4;
        public const int fenercodiCarbon = 5;
        public const int BarraCodiStaRosa220 = 14;
        public static int ConcepCodiTCambio = 1;
        public const int IdLecturaFlujoPotencia = 94;
        public const int IdTipoinfoMW = 1;
        public const string FamcodiSubEst = "1";
        public const string FamcodiBarra = "7";
        public const string FamcodiLineaTrans = "8";
        public const string FamcodiTransform2d = "9";
        public const string FamcodiTransform3d = "10";
        public const string FamcodiTransformC = "26";
        public const string FamcodiTransformT = "27";
        public const string FamcodiTransformZ = "29";
        public const string CausaExt = "2";
        public const string CausaFna = "3";
        public const string CausaFhu = "4";
        public const string CausaFec = "5";
        public const string CausaFep = "6";
        public const string CausaOtr = "8";
        public const int IdReporteCaudales = 11;
        public const int IdReporteVolumenUtil = 10;
        public const int PtomediLagunasEnel = 43403;
        public const int PtomediLagunasJunin = 43419;
        public const int PtomediLagunasEgasa = 43403;
        //Remisiones
        public const string Delimitador = "|";
        public const byte EnvioEnviado = 1;

        #region SIOSEIN MODIFICACION
        //REGION INICIO SIOSEIN MODIFICACION
        public const int Exportacion = 2;
        public const int Importacion = 1;
        public const string ECU = "1";
        public const int PotenciaEfecGeneradHidro = 164;
        public const int PotenciaEfecGeneradTermo = 188;

        public const int PotenciaInstalGeneHidro = 1530;
        public const int PotenciaInstalGeneTermo = 1563;

        //REGION FIN SIOSEIN MODIFICACION
        #endregion

        public const string AppExcel = "application/vnd.ms-excel";
        public const string RptExcel = "RptCostoMarginal";
        public const string RptExcelNulos = "CMgSinValor";
        public const string ExtensionExcel = ".xlsx";
        public const int nColumnTabla1 = 5;

        public const string Recasegsumres = "26,27,28,29,30";
        public const string Recaprimarer = "33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,84,85,94,99,100,101,102";
        public const string Recaprimafise = "65,66,67,68,69,70,71,72,73";
        public const string Recaconfiasum = "74,75,76,77,103";
        public const string Recageneradic = "78,79,80,81,82,83";
        public const string Recasegsumrf = "86,87,88,89,90,91,92";
        public const string Recaprimacase = "";
        public const string Recaotroscarg = "31,32";

        #region SIOSEIN2 - NUMERALES
        public const int FdatcodiCMCPmen = 8;
        public const int FdatcodiCMCPdia = 9;
        #endregion

        #region SIOSEIN-PRIE-2021
        public const string RutaReportePRIE = "RutaReportePRIE";

        public const string AlineaColumnaIzquierda = "left";
        public const string AlineaColumnaCentro = "center";
        public const string AlineaColumnaDerecha = "right";
        public const string AlineaColumnaJustificada = "justify";

        public const string Descargas = @"\" + "Downloads" + @"\";

        public const string TipoColumnaString = "string";
        public const string TipoColumnaInteger = "integer";
        public const string TipoColumnaDouble = "double";

        public const string TipoEmpresaPrieVTP = "1,2,3,4";

        public const string SumaColumnas = "columnas";
        public const string SumaFilas = "filas";

        public const int SeccionCabeceraoPie = 1;
        public const int SeccionCuerpo = 0;

        public const int Numero15 = 15;
        public const int Numero30 = 30;

        #endregion
    }

    /// <summary>
    /// Correlativos de la tabla SI_MENUREPORTE para el Informe Mensual
    /// </summary>
    public class ConstantesInformeMensual
    {
        public const int IndexMensualResumenRelevante = 305;

        public const int CatecodiMensualOfertaGeneracion = 297;
        public const int IndexMensualIngresoOpComercSEIN = 306;
        public const int IndexMensualRetiroOpComercSEIN = 307;
        public const int IndexMensualPotenciaInstaladaSEIN = 308;

        public const int CatecodiMensualProduccionEnergia = 298;
        public const int IndexMensualProdTipoGen = 309;
        public const int IndexMensualProdTipoRecurso = 310;
        public const int IndexMensualProdRER = 311;
        public const int IndexMensualFactorPlantaRER = 312;
        public const int IndexMensualParticipacionEmpresas = 313;

        public const int CatecodiMensualMDCoincidente = 299;
        public const int IndexMensualMaximaDemandaTipoGeneracion = 314;
        public const int IndexMensualMaximaDemandaXEmpresa = 315;

        public const int CatecodiMensualHidrologia = 300;
        public const int IndexMensualVolUtilEmbLag = 316;
        public const int IndexMensualEvolucionVolEmbLag = 317;
        public const int IndexMensualPromMensualCaudales = 318;
        public const int IndexMensualEvolucionCaudales = 319;

        public const int CatecodiMensualCostoMarginal = 301;
        public const int IndexMensualCostosMarginalesProm = 320;
        public const int IndexMensualCostosMarginalesPorArea = 321;

        public const int CatecodiMensualHoraCongestion = 302;
        public const int IndexMensualHorasCongestionAreaOpe = 322;

        public const int CatecodiMensualEventoFallas = 303;
        public const int IndexMensualEventoFallaSuministroEnerg = 323;

        public const int CatecodiMensualAnexos = 348;
        public const int IndexMenProduccionElectricidad = 349;
        public const int IndexMenMaximaPotenciaCoincidente = 350;
        public const int IndexMenListadoEventos = 351;

        public const int ReptipcodiMensual = 6;

    }

    /// <summary>
    /// Correlativos de la tabla SI_MENUREPORTE para el Informe Anual
    /// </summary>
    public class ConstantesInformeAnual
    {
        public const int IndexAnualProduccionEnergia = 324;

        public const int CatecodiAnualOfertaGeneracion = 121;
        public const int IndexAnualIngresoOpComercSEIN = 325;
        public const int IndexAnualRetiroOpComercSEIN = 326;
        public const int IndexAnualPotenciaInstSEIN = 345;

        public const int CatecodiAnualProduccionEnergia = 122;
        public const int IndexAnualProdTipoGen = 328;
        public const int IndexAnualVariacionInteranual = 329;
        public const int IndexAnualProdTipoRecurso = 330;
        public const int IndexAnualProdRER = 331;
        public const int IndexAnualFactorPlantaRER = 332;
        public const int IndexAnualProduccionEmpresas = 344;

        public const int CatecodiAnualMDCoincidente = 123;
        public const int IndexAnualMaxDemandaTipoGeneracion = 333;
        public const int IndexAnualMaxDemandaPorEmpresa = 334;

        public const int CatecodiAnualHidrologia = 124;
        public const int IndexAnualEvolVolAlmacenados = 335;
        public const int IndexAnualEvolCaudales = 336;

        public const int CatecodiAnualCostosMarginales = 125;
        public const int IndexAnualEvolCostosMarginales = 337;

        public const int CatecodiAnualHoraCongestion = 126;
        public const int IndexAnualHorasCongestionAreaOpe = 338;

        public const int CatecodiAnualIntercambiosInt = 127;
        public const int IndexAnualInterInternacionales = 339;

        public const int IndexAnualProdElectricidad = 352;
        public const int IndexAnualMaxPotenciaCoincidente = 353;


        public const int ReptipcodiAnual = 7;
        


    }

    /// <summary>
    /// Correlativos de la tabla SI_MENUREPORTE para el Ejecutivo Mensual
    /// </summary>
    public class ConstantesInformeEjecutivoMensual
    {

        public const int CatecodiEjecMensualProduccionPotencia = 218; //1. PRODUCCIÓN Y POTENCIA COINCIDENTE EN BORNES DE GENERACIÓN DEL SEIN
        public const int CatecodiEjecMensualDemandaBarras  = 228;	//2.  DEMANDA DE ENERGÍA EN BARRAS DE TRANSFERENCIAS DEL SEIN
        public const int CatecodiEjecMensualHidrologia  = 232;	//3. HIDROLOGÍA PARA LA OPERACIÓN DEL SEIN
        public const int CatecodiEjecMensualInterconexiones  = 235;	//4.  INTERCONEXIONES
        public const int CatecodiEjecMensualHorasCongestion  = 237;	//5.  HORAS CONGESTION EN LOS PRINCIPALES EQUIPOS DE TRANSMISIÓN
        public const int CatecodiEjecMensualEvolCostosMarginales  = 239;	//6. EVOLUCIÓN DE LOS COSTOS MARGINALES
        public const int CatecodiEjecMensualManttEjecutados  = 243;	//7.  MANTENIMIENTOS EJECUTADOS
        public const int CatecodiEjecMensualTranferEnergPot  = 245;	//8.  TRANSFERENCIA DE ENERGIA Y POTENCIA
        public const int CatecodiEjecMensualCompTransmisoras  = 250;	//9. COMPENSACION A TRANSMISORAS
        public const int CatecodiEjecMensualEventosFallas  = 255;	//10. EVENTOS Y FALLAS QUE OCACIONARON INTERRUPCION O DISMINUCION DE SUMINISTRO ELECTRICO
        public const int CatecodiEjecMensualEventosFallas2  = 257;	//11. EVENTOS Y FALLAS QUE OCACIONARON INTERRUPCION O DISMINUCION DE SUMINISTRO ELECTRICO
        public const int CatecodiEjecMensualEmpresaIntegCoes  = 270;	//12. EMPRESAS INTEGRANTES DEL COES

        public const int IndexProdEmpresaGeneradora =219; //	1.1. Producción por empresa generadora
        public const int IndexTotalCentralesGeneracion = 220; //	1.2. Producción total de centrales de generación eléctrica con exportación a ecuador 
        public const int IndexParticEmpProduccionMes = 221; //	1.3. Participación por empresas en la producción total de energía del mes
        public const int IndexCrecimientoMensualMaxPotencia = 222; //	1.4. Evolución del crecimiento mensual de la máxima potencia coincidente sin exportación a ecuador
        public const int IndexComparacionCoberturaMaxDemanda = 223; //	1.5. Comparación de la cobertura de la máxima demanda por tipo de generación
        public const int IndexDespachoMaxPotenciaCoincidente = 224; //	1.6. Despacho en el día de máxima potencia coincidente
        public const int IndexCobMaxPotCoincidenteTecnologia = 225; //	1.7. Cobertura de la máxima potencia coincidente por tipo de tecnología 
        public const int IndexUtilizacionRecursosEnergeticos = 226; //	1.8. Utilización de los recursos energéticos
        public const int IndexUtilizacionRecEnergeticosProdElec = 227; //	1.9. Participación de la utilización de los recursos energéticos en la producción de energía eléctrica
        public const int IndexDemandaZonaNorte = 229; //	2.1 DEMANDA DE ENERGíA ZONA NORTE
        public const int IndexDemandaZonaCentro = 230; //	2.2 DEMANDA DE ENERGÍA ZONA CENTRO
        public const int IndexDemandaZonaSur = 231; //	2.3 DEMANDA DE ENERGÍA ZONA SUR
        public const int IndexVolumenEmbLag = 233; //	3.1. VOLÚMEN UTIL DE LOS EMBALSES Y LAGUNAS(Millones de m3)                
        public const int IndexEvolucionVolumenes = 346; // 3.2 EVOLUCION DE LOS VOLUMNES
        public const int IndexPromedioCaudales = 234; //	3.3. PROMEDIO MENSUAL DE LOS CAUDALES(m3/s)
        public const int IndexEvolucionCaudales = 347;  // 3.4 EVOLUCION DE LOS CAUDALES
        public const int IndexInterconexiones = 236; //	4.1.   Interconexiones
        public const int IndexCongestionEqTransmision = 238; //	5.1  Horas Congestion principales equipos de Transmision
        public const int IndexEvolucionCMGbarra = 240; //	6.1. EVOLUCIÓN DEL COSTO MARGINAL EN BARRA DE REFERENCIA
        public const int IndexCostosMarginalesModoOpe = 241; //	6.2. COSTOS MARGINALES DE LOS PRINCIPALES MODOS DE OPERACIÓN
        public const int IndexCostosMarginalesBarrasSein = 242; //	6.3.  COSTOS MARGINALES EN LAS PRINCIPALES BARRAS DEL SEIN(US$/MWh)
        public const int IndexMantenimientosEjecutados = 244; //	7.1.  Mantenimientos Ejecutados
        public const int IndexTransferenciaEnergiaActiva = 246; //	8.1.  Transferencias de Energia Activa
        public const int IndexTransferenciaPotencia = 247; //	8.2.  Transferencias de Potencia
        public const int IndexValorizacionTransfPotencia = 248; //	8.3.  Valorizacion de las transferencias de Potencia(Soles)
        public const int IndexPotenciaFirmeEmpresas = 249; //	8.4.  Potencia Firme por Empresas(MW)
        public const int IndexCompensacionPeajeConexTransmision = 251; //	9.1.  Compensacion a transmisoras por peaje de conexion y trasnmision, Sistema principal y Sistema garantizado de Transmision
        public const int IndexPorcentajeCompPeajeConexTransmision = 252; //	9.2.  Porcentaje de compensacion por Peaje de conexion y Transmision
        public const int IndexCompensacionIngresoTarifario = 253; //	9.3.  Compensacion a transmisoras por ingreso tarifario del Sistema principal y Sistema garantizado de transmision
        public const int IndexPorcentajeCompIngresoTarifario = 254; //	9.4.  Porcentaje de compensacion por Ingreso Tarifario
        public const int IndexEventoFallaSuministroElect = 256; //	10.1.  Detalle del Evento
        public const int IndexFallaTipoequipoCausa = 258; //	11.1.  Fallas por tipo de equipo y causa segun clasificacion
        public const int IndexEnergiaInterumpidaFallasZonas = 269; //	11.2.  Energía interrumpida(MWh) por fallas en las diferentes zonas del sistema eléctrico.
        public const int IndexEvolucionIntegrantesCoes = 271; //	12.1. EVOLUCIÓN DE INTEGRANTES DEL COES
        public const int IndexIngresoEmprIntegrAlCoes = 272; //	12.2. INGRESO DE EMPRESAS INTEGRANTES AL COES 
        public const int IndexRetiroEmprIntegrDelCoes = 273; //	12.3. RETIRO DE EMPRESAS INTEGRANTES DEL COES
        public const int IndexCambioDenomFusionEmprIntegrCoes = 274; //12.4. CAMBIO DE DENOMINACIÓN Y FUSIÓN DE EMPRESAS INTEGRANTES DEL COES

        public const int ReptiprepcodiEjecMensual = 10;

    }

    public class CostosMarginalesStaRosa
    {
        public int? Anio { get; set; }
        public int? numMes { get; set; }
        public string nombMes { get; set; }
        public decimal? CostoPromedio { get; set; }
    }

    public class RegistroPotInstalado
    {
        public string MesDescripcion { get; set; }
        public string TipoGen { get; set; }
        public decimal? PotInstalada { get; set; }
    }


    public class ListaExpSiosein
    {
        public int Indice { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
    }

    /// <summary>
    /// Datos de sesion
    /// </summary>
    public class DatosSesionSiosein
    {
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionListaCostoMarginal = "SesionListaCostoMarginal";
        public const string SesionMatriz1 = "SesionMatriz1";
        public const string SesionMatriz2 = "SesionMatriz2";
        public const string SesionMatriz3 = "SesionMatriz3";
    }

    public class ReporteSioseinRECA 
    {
        public string Emprcodosinergminpeaje { get; set; }
        public string Emprnombpeaje { get; set; }
        public decimal RecaudacionTransmision { get; set; }
        public decimal RecaudacionGenerAdicional { get; set; }
        public decimal RecaudacionSegSumNRF { get; set; }
        public decimal RecaudacionSegSumReservaFria { get; set; }
        public decimal RecaudacionPrimaRER { get; set; }
        public decimal RecaudacionPrimaFise { get; set; }
        public decimal RecaudacionPrimaCase { get; set; }
        public decimal RecaudacionConfiabilidadSum { get; set; }
        public decimal RecaudacionOtrosCargos { get; set; }
    }

    public class ReporteCostoOperacion : MeMedicion1DTO
    {
        public decimal? Ejecutado { get; set; }
        public decimal? Programado { get; set; }
        public decimal? Variacion { get; set; }
    }

}
