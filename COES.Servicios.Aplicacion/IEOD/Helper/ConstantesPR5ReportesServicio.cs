using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.IEOD
{
    public class ConstantesPR5ReportesServicio
    {        
        public const string FolderRaizPR5 = "Intranet/Aplicativo_InformesSGI/";
        public const string FolderAnexoA = "AnexoA";
        public const string FolderEjecutivoSemanal = "EjecutivoSemanal";
        public const string FolderInformeSemanal = "InformeSemanal";

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual_usuario_Informes SGI.rar";
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        //constantes para fileServer
        public const string FolderRaizPR5ModuloManual = "Informes SGI\\";

        public const string FolderInformeMensual = "InformeMensual";
        public const string FolderInformeEjecutivoMensual = "InformeEjecutivoMensual";
        public const string FolderInformeAnual = "InformeAnual";

        public const string SubfolderPlantilla = "Plantilla";
        public const string SubfolderReporte = "Reporte";
        public const string SubfolderReporteExcel = "Reporte_Excel";
        public const string SubfolderReporteWord = "Reporte_Word";

        public const string RutaReportes = "Areas/IEOD/Reporte/";

        public const int SubCausaEquipoOperacionIng = 348;
        public const int SubCausaEquipoOperacionRet = 349;
        public const int SubCausaEquipRepotenciados = 350;
        public const string SubCausaEquiposIntegradosPrimeraconex = "345,349";
        public const int SubCausaEquipReubicados = 347;

        public const string TipoFteEnergiaTodos = "1,2,3,4";
        public const int TgenercodiHidro = 1;
        public const int TgenercodiTermo = 2;
        public const int TgenercodiSolar = 3;
        public const int TgenercodiEolica = 4;

        public const int TgenercodiRER = 100; //ESPECIAL
        public const int TgenercodiTotal = 101; //ESPECIAL
        public const int TipoSemanaRelProdSinTTIE = 1;
        public const int TipoSemanaRelProdConTTIE = 2;
        public const int TipoSemanaRelExp = 3;
        public const int TipoSemanaRelImp = 4;

        public const string TgenerHidro = "HIDROELÉCTRICO";
        public const string TgenerTermo = "TERMOELÉCTRICO";
        public const string TgenerSolar = "SOLAR";
        public const string TgenerEolica = "EÓLICO";

        public const int FenergcodiAgua = 1;
        public const int FenergcodiGas = 2;
        public const int FenergcodiDiesel = 3;
        public const int FenergcodiResidual = 4;
        public const int FenergcodiCarbon = 5;
        public const int FenergcodiBagazo = 6;
        public const int FenergcodiBiogas = 7;
        public const int FenergcodiSolar = 8;
        public const int FenergcodiEolica = 9;
        public const int FenergcodiR500 = 10;
        public const int FenergcodiR6 = 11;
        public const int FenergcodiNoAplica = 12;
        public const int FenergcodiRef = 13;
        public const int FenergcodiNaf = 14;
        public const int FenergcodiNR = 15;
        public const int FenergcodiFLE = 16;
        public const int FenergcodiFG = 17;
        public const int FenergcodiNFG = 18;
        public const int FenergcodiTodos = -1;
        public const int FenergcodiNoDefinido = 0;

        public const int CategoriaRecursoAgua = 2;
        public const int CategoriaRecursoGas = 3;
        public const int CategoriaTecnolog = 4;

        public const int SubCategoriaRecursoAguaPasada = 6;
        public const int SubCategoriaRecursoAguaRegulacion = 5;

        public const int SubCategoriaRecursoGasNatural = 8;
        public const int SubCategoriaRecursoGasMalacas = 7;
        public const int SubCategoriaRecursoGasAguaytia = 9;
        public const int SubCategoriaRecursoGasLaIsla = 10;

        public const int SubCategoriaRecursoResidualR500 = 1000; //Ficticio 
        public const int SubCategoriaRecursoResidualR6 = 1001; //Ficticio 
        public const string GrupocombR500 = "R500";
        public const string GrupocombR6 = "R6";

        public const int CtgcodiFteGas = 15;

        public const int LectcodiFlujoPotencia = 116;
        public const int LectcodiTensionKv = 117;
        public const int IdDemandaMW = 20;
        public const int LectDemandaConsumidaDiaria = 45;
        public const int FamcodiNoExcluir = -5;
        public const string TipoGenRER = "S";
        public const string TipoGenerTodos = "-1";

        public const string PtomedicodiDemandaEcuador = "1229";

        public const int TipoinfoMW = 1;
        public const int TipoinfoMVAR = 2;
        public const int TipoinfoKv = 5;
        public const int TipoinfoSoles = 7;
        public const int PtomedicodiCostoOpr = 1300;
        public const int PtomedicodiCostoOperacionNCP = 1301;
        public const int TipoinfoMWDemanda = 20;
        public const int LectDespachoEjecutadoHisto = 6; //93; //101;
        public const int LectDespachoEjecutado = 93; //93; //101;
        public const int LectDespachoReprogramado = 5;
        public const int LectDespachoProgramadoDiario = 4;
        public const int LectDespachoProgramadoSemanal = 3;
        public const int LectMedidorGeneracion = 1;
        public const int OriglectDespacho = 2;

        public const int TipoData48PR5GruposDespacho = 1;
        public const int TipoData48PR5UnidadesGeneracion = 2;

        public const string EmpresacoesSi = "S";
        public const string EmpresacoesNo = "N";
        public const int TipoEmpresaUsuarioLibre = 4;

        public const string AreaNorte = "NORTE";
        public const string AreaCentro = "CENTRO";
        public const string AreaSur = "SUR";

        public const int ReporcodiMaxGeneNorHisto = 14;
        public const int ReporcodiMaxGeneCenHisto = 15;
        public const int ReporcodiMaxGeneSurHisto = 16;
        public const int ReporcodiDemandaAreas = 17;
        public const int ReporcodiDemandaSubareas = 18;
        public const int ReporcodiFlujoLineaAnexoA = 73;
        public const int ReporcodiTensionBarraAnexoA = 76;
        public const int ReporcodiFlujoLineaTIEAnexoA = 77;

        public const int IdReporteCaudalesHidro = 21;
        public const int IdReporteVolumenHorario = 22;
        public const int IdReporteVolumenUtil = 15;
        public const int IdReporteCaudales = 16;

        public const int IdReporteFlujo = 73;
        public const int IdReporteTension = 76;
        public const int IdReporteFlujoIDCOS = 72;
        public const int IdReporteInterconexionCentroNorte = 74;
        public const int IdReporteInterconexionCentroSur = 75;
        public const int IdReporteInterconexionSurOesteSurEste = 78;
        public const int IdReporteInfSemInterconexionCentroNorte = 83;
        public const int IdReporteInfSemInterconexionCentroSur = 84;
        public const int IdReporteEjecSemInterconexionCentroNorte = 85;
        public const int IdReporteEjecSemInterconexionCentroSur = 86;

        public const int IdReporteCaudalNaturalInformeSemanal = 100;
        public const int IdReporteCaudalNaturalEjecutivoSemanal = 101;
        public const int IdReporteCaudalNaturalInformeMensual = 144;
        public const int IdReporteCaudalNaturalEjecutivoMensual = 146;
        public const int IdReporteCaudalNaturalInformeAnual = 148;
        public const int IdReporteCaudalDescargadoInformeSemanal = 102;
        public const int IdReporteCaudalDescargadoEjecutivoSemanal = 103;
        public const int IdReporteCaudalDescargadoInformeMensual = 145;
        public const int IdReporteCaudalDescargadoEjecutivoMensual = 147;
        public const int IdReporteCaudalDescargadoInformeAnual = 149;

        public const int IdReporteVolumenUtilInformeSemanal = 128;
        public const int IdReporteVolumenUtilEjecutivoSemanal = 129;
        public const int IdReporteVolumenUtilInformeMensual = 141;
        public const int IdReporteVolumenUtilEjecutivoMensual = 142;
        public const int IdReporteVolumenUtilInformeAnual = 143;

        public const string ColorReporteDemandaGrandesUsuariosNorte = "#2980B9";
        public const string ColorReporteDemandaGrandesUsuariosCentro = "#5B919B";
        public const string ColorReporteDemandaGrandesUsuariosSur = "#1F497D";

        public const string ColorFilaTablaRepEje = "#D9E1F2";
        public const string ColorBordeTablaRepEje = "#2F75B5";
        public const string ColorTotTablaRepEje = "#8EA9DB";

        public const string strCtgCodis = "3,4";
        public const int FteenergiaGas = 3;
        public const int TipoTecnologia = 4;

        public const int PropPotGarantizadaHidro = 1483;
        public const int PropRendimientoHidro = 932;

        public const int PropPotEfecHidro = 46;
        public const int PropPotEfecHidroGen = 164;
        public const int PropPotEfecTermoGenSem = 188;
        public const int PropPotEfectTermo = 53;
        public const int PropPotEfecEolica = 1602;
        public const int PropPotEfectSolar = 1710;

        public const int PropPotMinHidroGen = 299;

        public const int PropPotInstaladaHidro = 42;
        public const int PropPotInstaladaGenHidro = 1530;
        public const int PropPotInstaladaTermo = 49;
        public const int PropPotInstaladaGenTermo = 1563;
        public const int PropPotInstaladaEolica = 1602;
        public const int PropPotInstaladaSolar = 1710;

        public const int PropPotNominalHidro = 44;
        public const int PropPotNominalGenHidro = 297;
        public const int PropPotNominalTermo = 51;
        public const int PropPotNominalGenTermo = 189;

        public const int PropGrupoOperacionCoes = 258;

        public const int FamcodiCtralHidro = 4;
        public const int FamcodiCtralTermo = 5;
        //public const int FamcodiCtralSolar = 57;
        //public const int FamcodiCtralEolica = 59;
        public const int FamcodiCtralSolar = 37;
        public const int FamcodiCtralEolica = 39;

        public const int FamcodiGeneradorHidro = 2;
        public const int FamcodiGeneradorTemo = 3;
        public const int FamcodiGeneradorSolar = 36;
        public const int FamcodiGeneradorEolic = 38;

        public const int PropPortenciaEfectiva = 1;
        public const int PropPortenciaInstalada = 2;
        public const int PropCapacidadEmbalseLaguna = 1844;

        public const int PtomediLagunasEnel = 44105;
        public const int PtomediLagunasJunin = 44121;
        public const int PtomediLagunasEgasa = 44105;

        public const int TipoEventCodiHoraOpe = 8;

        //Anexo A
        public const string Directorio = "Areas\\IEOD\\Reporte\\";
        public const string RptExcel = "RptAnexoA";
        public const string RptExcelAnexoA = "AnexoA";
        public const string RptExcelAnexoA1 = "Anexo1_Resumen";
        public const string RptExcelAnexoA2 = "Anexo2_Hidrología";
        public const string RptExcelAnexoA3 = "Anexo3_RPFyRSF";
        public const string RptExcelAnexoA4 = "Anexo4_Hop";
        public const string RptExcelAnexoA5 = "Anexo5_Manttoejec";
        public const string RptExcelAnexoA6 = "Anexo6_CMgCP";
        public const string RptExcelEjecSem = "Ejecutivo_Semanal_COES_";
        public const string RptExcelSemanal = "Reporte_Semanal_COES_";
        public const string ExtensionExcel = ".xlsx";
        public const string ColorInfSGI = "#4472C4";

        #region EXCEL_INFORME_EJECUTIVO_SEMANAL

        public const string PathArchivoExcel = "Temporales/";
        public const string PantillaExcelInformeSemanal = "PLT_INF_SEMANAL.xlsx";
        public const string PantillaExcelEjecutivoSemanal = "PLT_EJEC_SEMANAL.xlsx";
        public const string PantillaExcelInformeMensual = "PLT_INF_MENSUAL.xlsx";
        public const string PantillaExcelInformeAnual = "PLT_INF_ANUAL.xlsx";
        public const string PantillaExcelEjecutivoMensual = "PLT_INFORME_EJE.xlsx";

        public const string ColorFondoCabInformeEjecutivoSem = "#538DD5"; //#538DD5 
        public const string TipoLetraCuerpo = "Arial";
        public const int TamLetraCuerpo = 10;
        public const int TamLetraCuerpo2 = 8;
        public const int TamLetraCuerpo3 = 6;
        public const string TipoLetraCabecera = "Calibri Light";
        public const int TamLetraCabecera = 11;

        public const string FormatoNumero1Digito = "#,##0.0";
        public const string FormatoNumero2Digito = "#,##0.00";
        public const string FormatoNumero3Digito = "#,##0.000";
        public const string FormatoNumero1DigitoPorcentaje = "#,##0.0%";
        public const string FormatoNumero2DigitoPorcentaje = "#,##0.00%";
        public const string FormatoNumero3DigitoPorcentaje = "#,##0.000%";

        public const string TipoVistaIndividual = "Individual";
        public const string TipoVistaGrupal = "Grupal";

        #endregion

        public const int FilaIniHeaderDefault = 1;
        public const int ColIniFooterDefault = 2;
        public const int ColIniFooter1 = 1;
        public const int FilaIniTituloDefault = 5;
        public const int ColIniTituloDefault = 2;
        public const int ColIniTitulo1 = 1;

        //Menu Reporte PR5
        public const int ReptipcodiInformeSemanal = 3;
        public const int ReptipcodiEjecutivoSemanal = 2;
        public const int ReptipcodiInformeMensual = 6;
        public const int ReptipcodiEjecutivoMensual = 10;
        public const int ReptipcodiInformeAnual = 7;
        public const int ReptipcodiAnexoAExcel = 4;
        public const int ReptipcodiAnexoAWord = 11;
        public const int ReptipcodiAnexoAidcos = 9;

        public const int MprojcodiIEOD = 1;
        public const int MprojcodiIDCOS = 2;
        public const int MprojcodiSIOSEIN2 = 4;

        public const int IdTipoInfocodiEnergiaActiva = 3;
        public const int IdExportacionL2280MWh = 41238;
        public const int IdImportacionL2280MWh = 41239;

        public const int PtomedicodiInternacionales = 3;

        public const string FamcodiSubEst = "1";
        public const string FamcodiBarra = "7";
        public const string FamcodiLineaTrans = "8";
        public const string FamcodiTransform2d = "9";
        public const string FamcodiTransform3d = "10";
        public const string FamcodiTransformC = "26";
        public const string FamcodiTransformT = "27";
        public const string FamcodiTransformZ = "29";
        public const int IdFormatoDescarga = 42;
        public const int IdFormatoVertimiento = 41;

        public const string CausaExt = "2";
        public const string CausaFna = "3";
        public const string CausaFhu = "4";
        public const string CausaFec = "5";
        public const string CausaFep = "6";
        public const string CausaOtr = "8";
        public const string CausaFni = "-1";

        public const int CateevencodiManttoEjecutado = 1;
        public const int CateevencodiEventoOcurrido = 2;
        public const string EvenclaseEjecutado = "1";
        public const string EvenclaseProgDiario = "2";
        public const int TipoevencodiEvento = 4;
        public const int TipoevencodiFalla = 5;
        public const int TipoevencodiPrueba = 6;
        public const int TipoGrafGeneracionSEINxArea = 1;
        public const int TipoGrafGeneracionSEINxTipoGen = 2;
        public const int TipoGrafGeneracionSEINxRER = 3;

        public const int BarrzareaNorte = 1;
        public const int BarrzareaCentro = 2;
        public const int BarrzareaSur = 3;
        public const int BarraCodiStaRosa220 = 14;
        public static int ConcepCodiTCambio = 1;

        //costo toTal de la operacion
        public const int LectCostoOperacionEjec = 6;
        public const int LectCostoOperacionProg = 4;
        public const int LectCodiDemandaNCP = 1221;
        public const int LectCodiProgSemanal = 3;
        public const int LectCodiProgDiaria = 4;
        public const int NumeroFilas = 1000;

        public const int SubcausacodiRestric = 205;

        public const decimal FrecuenciaMaxSostenida = 60.36m;
        public const decimal FrecuenciaMinSostenida = 59.64m;
        public const decimal FrecuenciaMaxSubita = 61m;
        public const decimal FrecuenciaMinSubita = 59m;

        public const decimal LimiteTransporteTIE = 70.0m;

        public const string IdsSistemaAv = "0,1";
        public const string IdsOtraClasificacion = "-1,0,1,2,3,10";
        public const string MantenimientoEjecutado = "1";
        public const int TipoReporteCaudales = 1;
        public const string IdTipoPtoMedhidro = "11,40";
        public const int IdOrigenLectura = 16;
        public const string IdTipoPtoMedhidro2 = "11,14,40";
        public const int LecturaHidro = 75;
        public const int FormatcodiFuenEnerPri = 68;
        public const int FormatcodiCalorUtil = 65;
        public const string IdSubCausaSobrecarga = "343";
        public const string OrigLecturaPR05IEOD = "21";
        public const string LectDespachoProgReprog = "4,5";
        public const int OrigLecturaDespacho30min = 2;

        //Reserva Fria
        public const string ListadoEstado = "'A'";
        public const int ConcepcodiPotenciaEfectiva = 14;
        public const int TipoReservaFriaTotal = 1;
        public const int TipoReservaFriaRapida = 2;
        public const int TipoReservaFriaMinima = 3;
        public const int TipoReservaFriaIndisponibilidad = 4;
        public const string TipoReservaFriaTotalColor = "#7dcee8";
        public const string TipoReservaFriaRapidaColor = "#0000FF";
        public const string TipoReservaFriaMinimaColor = "#FF0000";
        public const string TipoReservaFriaIndisponibilidadColor = "#0A7705";
        public const int TiempoSincRapidoMin = 6 * 60;
        public const decimal FactorKCalculoUnidadTV = 0.55m;

        public const int CatecodiCentralHidraulica = 6;
        public const int CatecodiGrupoHidraulico = 5;

        public const string IdSubCausaCaldero = "202";
        public const string IdSubCausaCongestionesST = "201";
        public const string IdSubCausaPorTension = "203";
        public const string IdSubCausaSistemasAislados = "206";
        public const string IdSubCausaPorPruebaNoTermo = "208";
        public const int LectcodiCM = 6;

        public const int LectCodiTensionBarra = 97;
        public const string TipoEmpresa = "3";
        public const string TipoEmpresaRestriccionesOperativas = "1,2,3,4";
        public const int TipoInfoCodiTensionBarra = 5;
        public const int CausaevencodiOperacion = 100;

        public const int MensajeError = 1;
        public const int MensajeAlerta = 2;

        //Anexo A        
        public const int TIPO_ANEXO_A = 1;

        //Informe Semanal
        public const int TIPO_SEMANAL = 2;

        //Ejecutivo Semanal
        public const int TIPO_EJECUTIVO_SEM = 3;

        //Ejecutivo Semanal
        public const string IndexReporteInformeMensual = "IndexReporteInformeMensual";

        //Informe Mensua
        public const int EvolMensualVolUtilEmbLag = 317;
        public const int EvolMensualCaudales = 319;
        //Informe Anual
        public const int EvolAnualVolUtilEmbLag = 335;
        public const int EvolAnualCaudales = 336;

        public const string FamcodiTipoCentrales = "4,5,37,39";
        public const int TipoEmprcodiGeneracion = 3;
        public const string AppExcel = "application/vnd.ms-excel";
        public const string RptExcelDesviacionPronostico = "RptExcelDesviacionPronostico.xls";
        public const string PlantillaDesviacionPronostico = "PlantillaDesviacionPronostico.xls";

        public const int PtomediRioChancay = 44142;
        public const int PtomediRioSanta = 44143;
        public const int PtomediRioPativilca = 44144;

        public const int PtomediRioRimac = 0;
        public const int PtomediRioEulalia = 0;

        public const int PtomediRioMantaro = 0;
        public const int PtomediRioTulumayo = 0;
        public const int PtomediRioTarma = 0;

        public const int PtomediCuencaCharcani4 = 0;
        public const int PtomedicuencaAricota = 0;
        public const int PtomediCuencaGaban = 0;
        public const int PtomediCuencaVilcanota = 0;

        //Maxima Demanda
        public const int TipoMaximaDemandaMedidores = 1;
        public const int TipoMaximaDemandaDespacho = 2;
        public const string PtomedicodiFlujoTrans2280 = "42725";
        public const int PtomedicodiZorritos = 42725;

        public const int LectcodiEjecutadoPorAreaClienteDesktop = 27;
        public const int PtomedicodiEcuador = 3027;
        public const int PtomedicodiElectroandes = 3009;
        public const int PtomedicodiAreaNorte = 3004;
        public const int PtomedicodiAreaCentro = 3005;
        public const int PtomedicodiAreaSur = 3006;
        public const int EmprcodiByZorritos = 12;//Codigo empresa para visualizar linea transmision ZORRITOS

        //Area operativa
        public const int PtomedicodiAreaOperativaNorte = 44973;
        public const int PtomedicodiAreaOperativaCentro = 44974;
        public const int PtomedicodiAreaOperativaSur = 44975;

        //////////////////////////////////////////////////////////////////
        /// Dashboard
        //////////////////////////////////////////////////////////////////

        public const int TipoDashProdYMaxDem = 1;
        public const string TipoDashDescProdYMaxDem = "Produccion Energía y Máxima Demanda";

        public const int TipoDashDemAreaOp = 2;
        public const string TipoDashDescDemAreaOp = "Demanda por Área Operativa";

        public const int TipoDashConsGranUsuario = 3;
        public const string TipoDashDescConsGranUsuario = "Consumo Grandes Usuarios";

        public const int TipoDashCostOp = 4;
        public const string TipoDashDescCostOp = "Costos Operación";

        public const int TipoDashCostMarg = 5;
        public const string TipoDashDescCostMarg = "Costos Marginales";

        public const int TipoDashRREE = 6;
        public const string TipoDashDescRREE = "Participación Recursos Energéticos";

        public const int TipoDashRER = 7;
        public const string TipoDashDescRER = "Participación RER";

        public const int TipoDashEnergPrim = 10;
        public const string TipoDashDescEnergPrim = "Energía Primaria";

        public const int TipoDashComb = 8;
        public const string TipoDashDescComb = "Combustibles Utilizados";

        public const int TipoDashCostVar = 9;
        public const string TipoDashDescCostVar = "Costos Variables";

        //Conversion
        public const int FactorGW = 1000;//MW a GW

        //Balance eléctrico
        public const int TipoDashBEDia = 1;
        public const int TipoDashBEMes = 2;
        public const int TipoDashBEMesOld = 3;
        public const int TipoDashBEMesVar = 4;
        public const int TipoDashBEAnio = 5;
        public const int TipoDashBEAnioOld = 6;
        public const int TipoDashBEAnioVar = 7;
        public const int TipoDashBEAnioMovil = 8;
        public const int TipoDashBEAnioMovilOld = 9;
        public const int TipoDashBEAnioMovilVar = 10;

        public const int TipoDashBEHistorica = 11;
        public const string DescAcumuladoAnioMovil = "Acumulado Año movil";

        //Demanda máxima
        public const int TipoDashMDPotenciaInstantanea = 1;
        public const int TipoDashMDDemandaHoraria = 2;
        public const int TipoDashMDDemandaDiaria = 4;

        //Demanda por Área operativa
        public const int PtomedicodiSein = 88888;
        public const string PtomedibarranombSein = "SEIN";

        //Grandes UL
        public const int TipoReporteConsolidado = 1;
        public const int TipoReporteDetallado = 2;

        public const int TipoRangoMayor100 = 1;
        public const int TipoRangoEntre30y100 = 2;
        public const int TipoRangoEntre20y30 = 3;
        public const int TipoRangoMenor20 = 4;

        //Recursos Energeticos
        public const int TipoRepRecursoEnergCtg = 1;
        public const int TipoRepFteEnergia = 2;
        public const int TipoRepFteEnergiaRER = 3;

        public const int EstcomcodiLiquido = 1;
        public const string EstcomnombLiquido = "LÍQUIDO";
        public const string EstcomcolorLiquido = "#0000ff";

        public const int EstcomcodiSolido = 2;
        public const string EstcomnombSolido = "SÓLIDO";
        public const string EstcomcolorSolido = "#D0D0D0";

        public const int EstcomcodiGaseoso = 3;
        public const string EstcomnombGaseoso = "GASEOSO";
        public const string EstcomcolorGaseoso = "#C9DEEC";

        public const int EstcomcodiRadiacion = 4;
        public const string EstcomnombRadiacion = "RADIACIÓN";
        public const string EstcomcolorRadiacion = "#F39C12";

        //Energia Primaria 
        public const int OriglectcodiFuenteEnergiaPrimaria = 21;
        public const int LectcodiFuenteEnergiaPrimaria = 99;

        public const int TptomedicodiVelocidadViento = 51;
        public const int TptomedicodiRadiacionSolar = 53;
        public const int TptomedicodiVolumenBagazo = 54;
        public const int TptomedicodiVolumenBiogas = 57;

        public const string EnergprimnombRadiacionSolar = "Radiación Solar";
        public const string EnergprimnombVelocidadViento = "Velocidad Viento";
        public const string EnergprimnombBiomasa = "Biomasa";
        public const string EnergprimnombBiogas = "Biogas";

        public const string EnergprimcolorRadiacionSolar = "#FFC300";
        public const string EnergprimcolorVelocidadViento = "#FF5733";
        public const string EnergprimcolorBiomasa = "#C70039";
        public const string EnergprimcolorBiogas = "#900C3E";

        //////////////////////////////////////////////////////////////////

        #region siosein2
        public const int LectDemandaPronosticadaDiaria = 46;
        public const int LectDemandaPronosticadaSemanal = 47;
        public const int ReporcodiMaxGeneNorHistorico = 31;
        public const int ReporcodiMaxGeneCenHistorico = 32;
        public const int ReporcodiMaxGeneSurHistorico = 33;
        public const int ReporcodiFlujoEnlaceCenNorSIOSEIN2 = 28;
        public const int ReporcodiFlujoEnlaceCensurSIOSEIN2 = 29;
        /// 
        public const int CaudalRioSantaChanchayPativilca = 1;
        public const int CaudalRioRimacSantaEulalia = 2;
        public const int CaudalRioMantaroTulumayoTarma = 3;
        public const int CaudalChiliAricotaVilcanotaSanGarban = 4;
        #endregion

        #region SIOSEIN
        public enum TipoOperacion
        {
            Ingreso = 1,
            Retiro = 2
        }

        public static Dictionary<int, int> PropPotenciaInstaldaxFamilia = new Dictionary<int, int>()
        {
            { FamcodiCtralTermo, PropPotInstaladaTermo},
            { FamcodiCtralHidro, PropPotInstaladaHidro},
            { FamcodiCtralSolar, PropPotInstaladaSolar},
            { FamcodiCtralEolica, PropPotInstaladaEolica},
            { FamcodiGeneradorHidro, PropPotInstaladaGenHidro},
            { FamcodiGeneradorTemo, PropPotInstaladaGenTermo},
            { FamcodiGeneradorSolar, PropPotInstaladaSolar},
            { FamcodiGeneradorEolic, PropPotInstaladaEolica},
        };

        #endregion

        public const string FileUnidadesConRestriccionRFria = "UnidadesConRestriccionRFria.xlsx";

        #region Archivo PDF

        public const string PathArchivosInformeSemanalPDF = "Intranet/Aplicativo_InformesSGI/InformeSemanal/PDF/";
        public const string ArchivoInformeSemanalPDF = "InformeSemanal_{0}.pdf";

        public const string PathArchivosInformeEjecutivoSemanalPDF = "Intranet/Aplicativo_InformesSGI/EjecutivoSemanal/PDF/";
        public const string ArchivoInformeEjecutivoSemanalPDF = "InformeEjecutivoSemanal_{0}.pdf";

        public const string PathArchivosInformeMensualPDF = "Intranet/Aplicativo_InformesSGI/InformeMensual/PDF/";
        public const string ArchivoInformeMensualPDF = "Inf_{0}_{1}_v{2}_SGI.pdf";

        public const string PathArchivosInformeAnualPDF = "Intranet/Aplicativo_InformesSGI/InformeAnual/PDF/";
        public const string ArchivoInformeAnualPDF = "Inf_Anual {0}_v{1}_SGI.pdf";

        public const string PathArchivosInformeEjecutivoMensualPDF = "Intranet/Aplicativo_InformesSGI/InformeEjecutivoMensual/PDF/";
        public const string ArchivoInformeEjecutivoMensualPDF = "Informe_Ejecutivo_Mensual_{0}_{1}_{2}.pdf";//Informe_Ejecutivo_Mensual_{MM}_{YYYY}_{VERSION}.pdf        

        #endregion


    }

    public class NotasPieExcelEjecutivoSemanal
    {
        #region NombreCuadrosGraficos

        #region Reporte 1.1
        public const string I_Cuadro1_Reporte_1p1 = "Cuadro  N°1: ";
        public const string I_Grafico1_Reporte_1p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_1p1 = "Cuadro  N°1: ";
        public const string G_Grafico1_Reporte_1p1 = "Gráfico N°1: ";
        #endregion

        #region Reporte 1.2
        public const string I_Cuadro1_Reporte_1p2 = "Cuadro  N°1: ";
        public const string I_Grafico1_Reporte_1p2 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_1p2 = "Cuadro  N° 2: ";
        #endregion

        #region Reporte 2.1

        public const string I_Cuadro1_Reporte_2p1 = "Cuadro  N° 1: ";
        public const string I_Grafico1_Reporte_2p1 = "Gráfico N° 1: ";
        public const string I_Grafico2_Reporte_2p1 = "Gráfico N° 2: ";


        public const string G_Cuadro1_Reporte_2p1 = "Cuadro  N° 3: ";
        public const string G_Grafico1_Reporte_2p1 = "Gráfico N° 2: ";
        public const string G_Grafico2_Reporte_2p1 = "Gráfico N° 3: ";

        #endregion

        #region Reporte 2.2
        public const string I_Cuadro1_Reporte_2p2 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_2p2 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_2p2 = "Gráfico N°2: ";
        public const string I_Grafico3_Reporte_2p2 = "Gráfico N°3: ";

        public const string G_Cuadro1_Reporte_2p2 = "Cuadro N°4: ";
        public const string G_Grafico1_Reporte_2p2 = "Gráfico N°4: ";
        public const string G_Grafico2_Reporte_2p2 = "Gráfico N°5: ";
        public const string G_Grafico3_Reporte_2p2 = "Gráfico N°6: ";

        #endregion

        #region Reporte 2.3
        public const string I_Cuadro1_Reporte_2p3 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_2p3 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_2p3 = "Gráfico N°2: ";
        public const string I_Grafico3_Reporte_2p3 = "Gráfico N°3: ";

        public const string G_Cuadro1_Reporte_2p3 = "Cuadro N°5: ";
        public const string G_Grafico1_Reporte_2p3 = "Gráfico N°7: ";
        public const string G_Grafico2_Reporte_2p3 = "Gráfico N°8: ";
        public const string G_Grafico3_Reporte_2p3 = "Gráfico N°9: ";

        #endregion

        #region Reporte 2.4

        public const string I_Cuadro1_Reporte_2p4 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_2p4 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_2p4 = "Gráfico N°2: ";


        public const string G_Cuadro1_Reporte_2p4 = "Cuadro N°6: ";
        public const string G_Grafico1_Reporte_2p4 = "Gráfico N°10: ";
        public const string G_Grafico2_Reporte_2p4 = "Gráfico N°11: ";

        #endregion

        #region Reporte 2.5

        public const string I_Cuadro1_Reporte_2p5 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_2p5 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_2p5 = "Cuadro N°7: ";
        public const string G_Grafico1_Reporte_2p5 = "Gráfico N°12: ";

        #endregion

        #region Reporte 3.1

        public const string I_Cuadro1_Reporte_3p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_3p1 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_3p1 = "Gráfico N°2: ";
        public const string I_Grafico3_Reporte_3p1 = "Gráfico N°3: ";
        public const string I_Grafico4_Reporte_3p1 = "Gráfico N°4: ";

        public const string G_Cuadro1_Reporte_3p1 = "Cuadro N°8: ";
        public const string G_Grafico1_Reporte_3p1 = "Gráfico N°13: ";
        public const string G_Grafico2_Reporte_3p1 = "Gráfico N°14: ";
        public const string G_Grafico3_Reporte_3p1 = "Gráfico N°15: ";
        public const string G_Grafico4_Reporte_3p1 = "Gráfico N°16: ";

        #endregion

        #region Reporte 3.2

        public const string I_Cuadro1_Reporte_3p2 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_3p2 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_3p2 = "Cuadro N°9: ";
        public const string G_Grafico1_Reporte_3p2 = "Gráfico N°17: ";

        #endregion

        #region Reporte 3.3

        public const string I_Cuadro1_Reporte_3p3 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_3p3 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_3p3 = "Gráfico N°2: ";

        public const string G_Cuadro1_Reporte_3p3 = "Cuadro N°10: ";
        public const string G_Grafico1_Reporte_3p3 = "Gráfico N°18: ";
        public const string G_Grafico2_Reporte_3p3 = "Gráfico N°19: ";

        #endregion



        #region Reporte 4.1
        public const string I_Cuadro1_Reporte_4p1 = "Cuadro N°1: ";

        public const string G_Cuadro1_Reporte_4p1 = "Cuadro N°11: ";

        #endregion


        #region Reporte 4.2
        public const string I_Grafico1_Reporte_4p2 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_4p2 = "Gráfico N°2: ";
        public const string I_Grafico3_Reporte_4p2 = "Gráfico N°3: ";

        public const string G_Grafico1_Reporte_4p2 = "Gráfico N°20: ";
        public const string G_Grafico2_Reporte_4p2 = "Gráfico N°21: ";
        public const string G_Grafico3_Reporte_4p2 = "Gráfico N°22: ";
        #endregion

        #region Reporte 4.3
        public const string I_Cuadro1_Reporte_4p3 = "Cuadro N°1: ";

        public const string G_Cuadro1_Reporte_4p3 = "Cuadro N°12: ";

        #endregion

        #region Reporte 4.4
        public const string I_Grafico1_Reporte_4p4 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_4p4 = "Gráfico N°2: ";
        public const string I_Grafico3_Reporte_4p4 = "Gráfico N°3: ";
        public const string I_Grafico4_Reporte_4p4 = "Gráfico N°4: ";


        public const string G_Grafico1_Reporte_4p4 = "Gráfico N°23: ";
        public const string G_Grafico2_Reporte_4p4 = "Gráfico N°24: ";
        public const string G_Grafico3_Reporte_4p4 = "Gráfico N°25: ";
        public const string G_Grafico4_Reporte_4p4 = "Gráfico N°26: ";

        #endregion

        #region Reporte 5.1
        public const string I_Cuadro1_Reporte_5p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_5p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_5p1 = "Cuadro N°13: ";
        public const string G_Grafico1_Reporte_5p1 = "Gráfico N°27: ";
        #endregion

        #region Reporte 6.1
        public const string I_Cuadro1_Reporte_6p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_6p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_6p1 = "Cuadro N°14: ";
        public const string G_Grafico1_Reporte_6p1 = "Gráfico N°28: ";
        #endregion

        #region Reporte 7.1
        public const string I_Cuadro1_Reporte_7p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_7p1 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_7p1 = "Gráfico N°2: ";

        public const string G_Cuadro1_Reporte_7p1 = "Cuadro N°15: ";
        public const string G_Grafico1_Reporte_7p1 = "Gráfico N°29: ";
        public const string G_Grafico2_Reporte_7p1 = "Gráfico N°30: ";
        #endregion

        #region Reporte 8.1
        public const string I_Cuadro1_Reporte_8p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_8p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_8p1 = "Cuadro N°16: ";
        public const string G_Grafico1_Reporte_8p1 = "Gráfico N°31: ";
        #endregion



        #region Reporte 9.1
        public const string I_Cuadro1_Reporte_9p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_9p1 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_9p1 = "Gráfico N°2: ";
        public const string I_Grafico3_Reporte_9p1 = "Gráfico N°3: ";

        public const string G_Cuadro1_Reporte_9p1 = "Cuadro N°17: ";
        public const string G_Grafico1_Reporte_9p1 = "Gráfico N°32: ";
        public const string G_Grafico2_Reporte_9p1 = "Gráfico N°33: ";
        public const string G_Grafico3_Reporte_9p1 = "Gráfico N°34: ";
        #endregion


        #region Reporte 10.1
        public const string I_Cuadro1_Reporte_10p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_10p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_10p1 = "Cuadro N°18: ";
        public const string G_Grafico1_Reporte_10p1 = "Gráfico N°35: ";

        #endregion

        #region Reporte 11.1
        public const string I_Cuadro1_Reporte_11p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_11p1 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_11p1 = "Gráfico N°2: ";
        public const string I_Grafico3_Reporte_11p1 = "Gráfico N°3: ";

        public const string G_Cuadro1_Reporte_11p1 = "Cuadro N°19: ";
        public const string G_Grafico1_Reporte_11p1 = "Gráfico N°36: ";
        public const string G_Grafico2_Reporte_11p1 = "Gráfico N°37: ";
        public const string G_Grafico3_Reporte_11p1 = "Gráfico N°38: ";
        #endregion



        #endregion
    }

    public class NotasPieExcelInformeSemanal
    {
        #region NombreCuadrosGraficos

        #region Reporte 1.1
        public const string I_Cuadro1_Reporte_1p1 = "Cuadro  N°1: ";
        public const string I_Grafico1_Reporte_1p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_1p1 = "Cuadro  N°1: ";
        public const string G_Grafico1_Reporte_1p1 = "Gráfico N°1: ";
        #endregion

        #region Reporte 1.2
        public const string I_Cuadro1_Reporte_1p2 = "Cuadro  N°1: ";
        public const string I_Grafico1_Reporte_1p2 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_1p2 = "Cuadro  N° 2: ";
        #endregion

        #region Reporte 2.1

        public const string I_Cuadro1_Reporte_2p1 = "Cuadro  N° 1: ";
        public const string I_Grafico1_Reporte_2p1 = "Gráfico N° 1: ";
        public const string I_Grafico2_Reporte_2p1 = "Gráfico N° 2: ";


        public const string G_Cuadro1_Reporte_2p1 = "Cuadro  N° 3: ";
        public const string G_Grafico1_Reporte_2p1 = "Gráfico N° 2: ";
        public const string G_Grafico2_Reporte_2p1 = "Gráfico N° 3: ";

        #endregion

        #region Reporte 2.2
        public const string I_Cuadro1_Reporte_2p2 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_2p2 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_2p2 = "Gráfico N°2: ";
        public const string I_Grafico3_Reporte_2p2 = "Gráfico N°3: ";
        public const string I_Grafico4_Reporte_2p2 = "Gráfico N°4: ";

        public const string G_Cuadro1_Reporte_2p2 = "Cuadro N°4: ";
        public const string G_Grafico1_Reporte_2p2 = "Gráfico N°4: ";
        public const string G_Grafico2_Reporte_2p2 = "Gráfico N°5: ";
        public const string G_Grafico3_Reporte_2p2 = "Gráfico N°6: ";
        public const string G_Grafico4_Reporte_2p2 = "Gráfico N°7: ";

        #endregion

        #region Reporte 2.3
        public const string I_Cuadro1_Reporte_2p3 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_2p3 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_2p3 = "Gráfico N°2: ";
        public const string I_Grafico3_Reporte_2p3 = "Gráfico N°3: ";

        public const string G_Cuadro1_Reporte_2p3 = "Cuadro N°5: ";
        public const string G_Grafico1_Reporte_2p3 = "Gráfico N°8: ";
        public const string G_Grafico2_Reporte_2p3 = "Gráfico N°9: ";
        public const string G_Grafico3_Reporte_2p3 = "Gráfico N°10: ";

        #endregion

        #region Reporte 2.4

        public const string I_Cuadro1_Reporte_2p4 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_2p4 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_2p4 = "Gráfico N°2: ";


        public const string G_Cuadro1_Reporte_2p4 = "Cuadro N°6: ";
        public const string G_Grafico1_Reporte_2p4 = "Gráfico N°11: ";
        public const string G_Grafico2_Reporte_2p4 = "Gráfico N°12: ";

        #endregion

        #region Reporte 2.5

        public const string I_Cuadro1_Reporte_2p5 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_2p5 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_2p5 = "Cuadro N°7: ";
        public const string G_Grafico1_Reporte_2p5 = "Gráfico N°13: ";

        #endregion

        #region Reporte 3.1

        public const string I_Cuadro1_Reporte_3p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_3p1 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_3p1 = "Gráfico N°2: ";
        public const string I_Grafico3_Reporte_3p1 = "Gráfico N°3: ";
        public const string I_Grafico4_Reporte_3p1 = "Gráfico N°4: ";

        public const string G_Cuadro1_Reporte_3p1 = "Cuadro N°8: ";
        public const string G_Grafico1_Reporte_3p1 = "Gráfico N°14: ";
        public const string G_Grafico2_Reporte_3p1 = "Gráfico N°15: ";
        public const string G_Grafico3_Reporte_3p1 = "Gráfico N°16: ";
        public const string G_Grafico4_Reporte_3p1 = "Gráfico N°17: ";

        #endregion

        #region Reporte 3.2

        public const string I_Cuadro1_Reporte_3p2 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_3p2 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_3p2 = "Cuadro N°9: ";
        public const string G_Grafico1_Reporte_3p2 = "Gráfico N°18: ";

        #endregion

        #region Reporte 3.3

        public const string I_Cuadro1_Reporte_3p3 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_3p3 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_3p3 = "Gráfico N°2: ";

        public const string G_Cuadro1_Reporte_3p3 = "Cuadro N°10: ";
        public const string G_Grafico1_Reporte_3p3 = "Gráfico N°19: ";
        public const string G_Grafico2_Reporte_3p3 = "Gráfico N°20: ";

        #endregion

        #region Reporte 4.1
        public const string I_Cuadro1_Reporte_4p1 = "Cuadro  N° 1: ";
        public const string I_Grafico1_Reporte_4p1 = "Gráfico N° 1: ";
        public const string I_Grafico2_Reporte_4p1 = "Gráfico N° 2: ";

        public const string G_Cuadro1_Reporte_4p1 = "Cuadro  N° 11: ";
        public const string G_Grafico1_Reporte_4p1 = "Gráfico N° 21: ";
        public const string G_Grafico2_Reporte_4p1 = "Gráfico N° 22: ";

        #endregion

        #region Reporte 4.2
        public const string I_Grafico1_Reporte_4p2 = "Gráfico N° 1: ";
        public const string I_Grafico2_Reporte_4p2 = "Gráfico N° 2: ";
        public const string I_Grafico3_Reporte_4p2 = "Gráfico N° 3: ";
        public const string I_Grafico4_Reporte_4p2 = "Gráfico N° 4: ";

        public const string G_Grafico1_Reporte_4p2 = "Gráfico N° 23: ";
        public const string G_Grafico2_Reporte_4p2 = "Gráfico N° 24: ";
        public const string G_Grafico3_Reporte_4p2 = "Gráfico N° 25: ";
        public const string G_Grafico4_Reporte_4p2 = "Gráfico N° 26: ";

        #endregion

        #region Reporte 4.3
        public const string I_Cuadro1_Reporte_4p3 = "Cuadro N° 1: ";
        public const string I_Grafico1_Reporte_4p3 = "Gráfico N° 1: ";
        public const string I_Grafico2_Reporte_4p3 = "Gráfico N° 2: ";
        public const string I_Grafico3_Reporte_4p3 = "Gráfico N° 3: ";

        public const string G_Cuadro1_Reporte_4p3 = "Cuadro N° 12: ";
        public const string G_Grafico1_Reporte_4p3 = "Gráfico N° 27: ";
        public const string G_Grafico2_Reporte_4p3 = "Gráfico N° 28: ";
        public const string G_Grafico3_Reporte_4p3 = "Gráfico N° 29: ";

        #endregion

        #region Reporte 5.1
        public const string I_Cuadro1_Reporte_5p1 = "Cuadro N°1: ";
        public const string I_Cuadro1_Reporte_5p2 = "Cuadro N°2: ";

        public const string G_Cuadro1_Reporte_5p1 = "Cuadro N°13: ";

        #endregion

        #region Reporte 5.2
        public const string I_Grafico1_Reporte_5p2 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_5p2 = "Gráfico N°2: ";
        public const string I_Grafico3_Reporte_5p2 = "Gráfico N°3: ";
        public const string I_Grafico4_Reporte_5p2 = "Gráfico N°4: ";
        public const string I_Grafico5_Reporte_5p2 = "Gráfico N°5: ";
        public const string I_Grafico6_Reporte_5p2 = "Gráfico N°6: ";
        public const string I_Grafico7_Reporte_5p2 = "Gráfico N°7: ";
        public const string I_Grafico8_Reporte_5p2 = "Gráfico N°8: ";
        public const string I_Grafico9_Reporte_5p2 = "Gráfico N°9: ";
        public const string I_Grafico10_Reporte_5p2 = "Gráfico N°10: ";
        public const string I_Grafico11_Reporte_5p2 = "Gráfico N°11: ";
        public const string I_Grafico12_Reporte_5p2 = "Gráfico N°12: ";
        public const string I_Grafico13_Reporte_5p2 = "Gráfico N°13: ";

        public const string G_Grafico1_Reporte_5p2 = "Gráfico N°30: ";
        public const string G_Grafico2_Reporte_5p2 = "Gráfico N°31: ";
        public const string G_Grafico3_Reporte_5p2 = "Gráfico N°32: ";
        public const string G_Grafico4_Reporte_5p2 = "Gráfico N°33: ";
        public const string G_Grafico5_Reporte_5p2 = "Gráfico N°34: ";
        public const string G_Grafico6_Reporte_5p2 = "Gráfico N°35: ";
        public const string G_Grafico7_Reporte_5p2 = "Gráfico N°36: ";
        public const string G_Grafico8_Reporte_5p2 = "Gráfico N°37: ";
        public const string G_Grafico9_Reporte_5p2 = "Gráfico N°38: ";
        public const string G_Grafico10_Reporte_5p2 = "Gráfico N°39: ";
        public const string G_Grafico11_Reporte_5p2 = "Gráfico N°40: ";
        public const string G_Grafico12_Reporte_5p2 = "Gráfico N°41: ";
        public const string G_Grafico13_Reporte_5p2 = "Gráfico N°42: ";
        #endregion

        #region Reporte 5.3
        public const string I_Cuadro1_Reporte_5p3 = "Cuadro N°1: ";

        public const string G_Cuadro1_Reporte_5p3 = "Cuadro N°14: ";

        #endregion

        #region Reporte 5.4
        public const string I_Grafico1_Reporte_5p4 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_5p4 = "Gráfico N°2: ";
        public const string I_Grafico3_Reporte_5p4 = "Gráfico N°3: ";
        public const string I_Grafico4_Reporte_5p4 = "Gráfico N°4: ";
        public const string I_Grafico5_Reporte_5p4 = "Gráfico N°5: ";
        public const string I_Grafico6_Reporte_5p4 = "Gráfico N°6: ";
        public const string I_Grafico7_Reporte_5p4 = "Gráfico N°7: ";
        public const string I_Grafico8_Reporte_5p4 = "Gráfico N°8: ";


        public const string G_Grafico1_Reporte_5p4 = "Gráfico N°43: ";
        public const string G_Grafico2_Reporte_5p4 = "Gráfico N°44: ";
        public const string G_Grafico3_Reporte_5p4 = "Gráfico N°45: ";
        public const string G_Grafico4_Reporte_5p4 = "Gráfico N°46: ";
        public const string G_Grafico5_Reporte_5p4 = "Gráfico N°47: ";
        public const string G_Grafico6_Reporte_5p4 = "Gráfico N°48: ";
        public const string G_Grafico7_Reporte_5p4 = "Gráfico N°49: ";
        public const string G_Grafico8_Reporte_5p4 = "Gráfico N°50: ";

        #endregion

        #region Reporte 6.1
        public const string I_Cuadro1_Reporte_6p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_6p1 = "Gráfico N° 1: ";
        public const string I_Grafico2_Reporte_6p1 = "Gráfico N° 2: ";
        public const string I_Grafico3_Reporte_6p1 = "Gráfico N° 3: ";

        public const string G_Cuadro1_Reporte_6p1 = "Cuadro N°15: ";
        public const string G_Grafico1_Reporte_6p1 = "Gráfico N° 51: ";
        public const string G_Grafico2_Reporte_6p1 = "Gráfico N° 52: ";
        public const string G_Grafico3_Reporte_6p1 = "Gráfico N° 53: ";
        #endregion

        #region Reporte 7.1
        public const string I_Cuadro1_Reporte_7p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_7p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_7p1 = "Cuadro N°16: ";
        public const string G_Grafico1_Reporte_7p1 = "Gráfico N°54: ";
        #endregion

        #region Reporte 8.1
        public const string I_Cuadro1_Reporte_8p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_8p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_8p1 = "Cuadro N°17: ";
        public const string G_Grafico1_Reporte_8p1 = "Gráfico N°55: ";
        #endregion

        #region Reporte 8.2
        public const string I_Cuadro1_Reporte_8p2 = "Cuadro N°1: ";

        public const string G_Cuadro1_Reporte_8p2 = "Cuadro N°18: ";
        #endregion

        #region Reporte 9.1
        public const string I_Cuadro1_Reporte_9p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_9p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_9p1 = "Cuadro N°19: ";
        public const string G_Grafico1_Reporte_9p1 = "Gráfico N°56: ";
        #endregion

        #region Reporte 9.2                                        
        public const string I_Cuadro1_Reporte_9p2 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_9p2 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_9p2 = "Cuadro N°20: ";
        public const string G_Grafico1_Reporte_9p2 = "Gráfico N°57: ";
        #endregion

        #region Reporte 9.3                                        
        public const string I_Cuadro1_Reporte_9p3 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_9p3 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_9p3 = "Cuadro N°21: ";
        public const string G_Grafico1_Reporte_9p3 = "Gráfico N°58: ";
        #endregion

        #region Reporte 10.1
        public const string I_Cuadro1_Reporte_10p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_10p1 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_10p1 = "Gráfico N°2: ";

        public const string G_Cuadro1_Reporte_10p1 = "Cuadro N°22: ";
        public const string G_Grafico1_Reporte_10p1 = "Gráfico N°59: ";
        public const string G_Grafico2_Reporte_10p1 = "Gráfico N°60: ";

        #endregion

        #region Reporte 11.1
        public const string I_Cuadro1_Reporte_11p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_11p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_11p1 = "Cuadro N°23: ";
        public const string G_Grafico1_Reporte_11p1 = "Gráfico N°61: ";
        #endregion

        #region Reporte 13.1
        public const string I_Cuadro1_Reporte_13p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_13p1 = "Gráfico N° 1: ";

        public const string G_Cuadro1_Reporte_13p1 = "Cuadro N°24: ";
        public const string G_Grafico1_Reporte_13p1 = "Gráfico N° 62: ";
        #endregion

        #region Reporte 14.1
        public const string I_Cuadro1_Reporte_14p1 = "Cuadro N°1:  ";
        public const string I_Grafico1_Reporte_14p1 = "Gráfico N°1:  ";
        public const string I_Grafico2_Reporte_14p1 = "Gráfico N°2:  ";
        public const string I_Grafico3_Reporte_14p1 = "Gráfico N°3:  ";

        public const string G_Cuadro1_Reporte_14p1 = "Cuadro N°25: ";
        public const string G_Grafico1_Reporte_14p1 = "Gráfico N°63: ";
        public const string G_Grafico2_Reporte_14p1 = "Gráfico N°64: ";
        public const string G_Grafico3_Reporte_14p1 = "Gráfico N°65: ";
        #endregion

        #endregion
    }

    public class NotasPieWebInformeSemanal
    {
        #region NombreCuadrosGraficos

        #region Reporte 1.1  INGRESO EN OPERACIÓN COMERCIAL AL SEIN
        public const string Cuadro1_Reporte_1p1 = "Relación de ingresos a operación comercial en el periodo al {0}.";
        public const string Grafico1_Reporte_1p1 = "Ingreso de Potencia Efectiva por tipo de Recurso Energético y Tecnología del 01 de enero al {0} (MW).";

        #endregion

        #region Reporte 1.2  RETIRO DE OPERACIÓN COMERCIAL DEL SEIN
        public const string Cuadro1_Reporte_1p2 = "Relación de retiros de operación comercial en el periodo del 01 de enero al {0}.";
        public const string Grafico1_Reporte_1p2 = "Salida de Potencia Efectiva por tipo de Recurso Energético y Tecnología del 01 de enero al {0} (MW).";

        #endregion

        #region Reporte 2.1   PRODUCCIÓN POR TIPO DE GENERACIÓN

        public const string Cuadro1_Reporte_2p1 = "Producción de energía eléctrica (GWh) por tipo de generación en el SEIN.";
        public const string Grafico1_Reporte_2p1 = "Comparación de la producción de energía eléctrica acumulada por tipo de generación periodo 01 de enero al {0} de {1}.";
        public const string Grafico2_Reporte_2p1 = "Evolución semanal de la producción de energía y comparación de la variación semanal para los años {0}, {1}, {2}.";

        #endregion

        #region Reporte 2.2  PRODUCCIÓN POR TIPO DE RECURSO ENERGÉTICO (GWh)
        public const string Cuadro1_Reporte_2p2 = "Producción de energía eléctrica (GWh) por tipo de recurso energético en el SEIN.";
        public const string Cuadro1_Reporte_2p2_1 = "(*) La Generación Acumulada Anual en GWh comprende del día 01 de enero al {0} de {1}, exceptuando la generación de energía eléctrica del día 29 de febrero.";
        public const string Grafico1_Reporte_2p2 = "Comparación de la producción de energía eléctrica acumulada por tipo de recurso energético, periodo 01 de enero al {0} de {1}";
        public const string Grafico2_Reporte_2p2 = "Comparación de la participación semanal por tipo de recurso energético.";
        public const string Grafico3_Reporte_2p2 = "Participación de la producción de energía eléctrica por tipo recurso energético durante la semana operativa {0}";
        public const string Grafico4_Reporte_2p2 = "Participación de la producción de energía eléctrica por tipo recurso energético acumulada al {0} de {1}";

        #endregion

        #region Reporte 2.3  PRODUCCIÓN POR RECURSOS ENERGÉTICOS RENOVABLES (GWh)
        public const string Cuadro1_Reporte_2p3 = "Producción de energía eléctrica (GWh) con recursos energético renovables en el SEIN.";
        public const string Cuadro1_Reporte_2p3_1 = "(*) Se denomina RER a los Recursos Energéticos Renovables (biomasa, eólica, solar, geotérmica, mareomotriz), e hidroléctricas cuya capacidad instalada no sobrepase los 20 MW, según D.L. N° 1002";
        public const string Grafico1_Reporte_2p3 = "Comparación de la producción de energía eléctrica acumulada con recursos energéticos renovables, periodo 01 de enero al {0} de {1} de los años {2}, {3} y {4}";
        public const string Grafico2_Reporte_2p3 = "Participación de las RER en la Matriz de Generación del SEIN - Periodo 01 de enero al {0} de {1} de de {2}";
        public const string Grafico3_Reporte_2p3 = "Participación de las RER en la Matriz de Generación del SEIN - semana operativa {0}";

        #endregion

        #region Reporte 2.4  FACTOR DE PLANTA DE LAS CENTRALES RER DEL SEIN

        public const string Cuadro1_Reporte_2p4 = "Producción de energía eléctrica (GWh) y factor de planta de las centrales con recursos energético renovables en el SEIN.";
        public const string Grafico1_Reporte_2p4 = "Producción de energía eléctrica (GWh) y factor de planta de las centrales con recursos energético renovables por tipo de generación en el SEIN - semana operativa {0}-{1}";
        public const string Grafico2_Reporte_2p4 = "Factor de planta de las centrales con recursos energético renovables en el SEIN, en el periodo acumulado 01 de enero al {0} de {1} de los años {2} y {3}.";

        #endregion

        #region Reporte 2.5  PARTICIPACIÓN DE LA PRODUCCIÓN (GWh) POR EMPRESAS INTEGRANTES

        public const string Cuadro1_Reporte_2p5 = "Participación de las empresas generadoras del COES en la producción de energía eléctrica (GWh) en la semana operativa {0} de los años {1} y {2}.";
        public const string Grafico1_Reporte_2p5 = "Comparación de producción energética (GWh) de las empresas generadoras del COES en la semana operativa {0} de los años {1} y {2}.";

        #endregion


        #region Reporte 3.1  MÁXIMA DEMANDA COINCIDENTE DE POTENCIA POR TIPO DE GENERACIÓN (MW)

        public const string Cuadro1_Reporte_3p1 = "Máxima demanda coincidente de potencia (MW) por tipo de generación en el SEIN.";
        public const string Grafico1_Reporte_3p1 = "Comparación de la máxima demanda coincidente de potencia (MW) por tipo de generación en el SEIN, en la semana operativa {0} de los años {1}, {2} y {3} ";
        public const string Grafico2_Reporte_3p1 = "Diagrama de carga del despacho en el día de máxima demanda por recurso energético duranta la semana operativa {0}";
        public const string Grafico3_Reporte_3p1 = "Evolución semanal de la máxima demanda y comparación de la variación semanal para los años {0},{1},{2}";
        public const string Grafico4_Reporte_3p1 = "Evolución mensual de la máxima demanda semanal para los años {0},{1},{2},{3}";

        public const string Grafico4_Reporte_3p1_2 = "Evolución mensual de la máxima demanda mensual y comparación de la variación anual para los años {0},{1},{2},{3}";
        public const string Grafico4_Reporte_3p1_Nota = "Las máximas demandas calculadas provienen de los medidores de generación con valores cada 15 minutos.";

        #endregion


        #region Reporte 3.2  PARTICIPACIÓN DE LAS EMPRESAS INTEGRANTES EN LA MÁXIMA DEMANDA COINCIDENTE (MW)

        public const string Cuadro1_Reporte_3p2 = "Participación de las empresas generadoras del COES en la máxima demanda coincidente (MW) durante la semana operativa {0} de los años {1} y {2}.";
        public const string Grafico1_Reporte_3p2 = "Comparación de la máxima demanda coincidente  (MW) de las empresas generadoras del COES durante la semana operativa {0} de los años {1} y {2}";

        #endregion

        #region Reporte 3.3  EVOLUCIÓN DE LA DEMANDA POR ÁREAS OPERATIVAS DEL SEIN (GWh)

        public const string Cuadro1_Reporte_3p3 = "Evolución de la Demanda por áreas operativas del SEIN.";
        public const string Cuadro1_Reporte_3p3_Nota1 = "Los valores acumulados son los datos que corresponden desde el día 01 hasta el día que finaliza la semana de análisis";
        public const string Cuadro1_Reporte_3p3_Nota2 = "Los valores de demanda por áreas no incluye la energía exportada a Ecuador";
        public const string Grafico1_Reporte_3p3 = "Comparación de la máxima demanda coincidente de potencia (MW) por área operativa en el SEIN, en la semana operativa {0} de los años {1}, {2} y {3}.";
        public const string Grafico2_Reporte_3p3 = "Evolución de la variación semanal acumulada por área operativas de los años {0}, {1}.";
        public const string Grafico2_Reporte_3p3_Ejec = "Evolución de la variación semanal acumulada por área operativas de los años {0},{1},{2}.";

        #endregion

        #region Reporte 4.1  DEMANDA COINCIDENTE DE LOS PRINCIPALES GRANDES USUARIOS EN EL DÍA DE MÁXIMA DEMANDA SEMANAL (MW)
        public const string Cuadro1_Reporte_4p1 = "Participación de la Demanda de los Grandes Usuarios en los periodos Horas Fuera de Punta y Horas Punta en el día de la máxima demanda de la semana operativa N°{0}-{1}.";
        public const string Grafico1_Reporte_4p1 = "Comparación de la máxima demanda coincidente de potencia (MW) por área operativa en el SEIN, en la semana operativa N°{0} de los años {1}, {2} y {3}.";
        public const string Grafico2_Reporte_4p1 = "Diagrama de carga de los Grandes Usuarios por áreas operativas en el día de máxima demanda de la semana operativa N°";

        #endregion

        #region Reporte 4.2  DIAGRAMA DE CARGA POR RANGOS DE POTENCIA EN GRANDES USUARIOS (MW)
        public const string Grafico1_Reporte_4p2 = "Diagrama de carga de los Grandes Usuarios con potencias {0} en el día de máxima demanda de la semana operativa N°{1}";
        public const string Grafico2_Reporte_4p2 = "Diagrama de carga de los Grandes Usuarios con potencias {0} en el día de máxima demanda de la semana operativa N°{1}";
        public const string Grafico3_Reporte_4p2 = "Diagrama de carga de los Grandes Usuarios con potencias {0} en el día de máxima demanda de la semana operativa N°{1}";
        public const string Grafico4_Reporte_4p2 = "Diagrama de carga de los Grandes Usuarios con potencias {0} en el día de máxima demanda de la semana operativa N°{1}";

        #endregion

        #region Reporte 4.3   DEMANDA DE ENERGÍA ELÉCTRICA POR ÁREA OPERATIVA DE LOS PRINCIPALES GRANDES USUARIOS (GWh)
        public const string Cuadro1_Reporte_4p3 = "Demanda de energía eléctrica de los Grandes Usuarios por áreas operativas del SEIN.";
        public const string Grafico1_Reporte_4p3 = "Evolución de la Demanda de energía de los Grandes Usuarios en el SEIN por áreas operativas y variación semanal en el SEIN.";
        public const string Grafico2_Reporte_4p3 = "Comparación de la participación de la demanda de energía de los Grandes Usuarios por áreas operativas de la semana operativa {0} para los años {1} y {2}.";
        public const string Grafico3_Reporte_4p3 = "Evolución de la Demanda de energía de los Grandes Usuarios en el SEIN por áreas operativas y variación semanal para el año {0}.";

        #endregion

        #region Reporte 5.1  VOLÚMEN UTIL DE LOS EMBALSES Y LAGUNAS (Millones de m3)
        public const string Cuadro1_Reporte_5p1 = "Volúmen útil de los principales embalses y lagunas del SEIN al término del periodo de análisis de los años {0} y {1}.";

        #endregion

        #region Reporte 5.2  EVOLUCIÓN DE VOLUMENES DE LOS EMBALSES Y LAGUNAS
        public const string GraficoX_Reporte_5p2 = "Evolución semanal del volumen {0} durante los años {1} - {2}.";

        #endregion

        #region Reporte 5.3  PROMEDIO MENSUAL DE LOS CAUDALES (m3/s)
        public const string Cuadro1_Reporte_5p3 = "Promedio de caudales de la semana {0} en los años {1} y {2}.";

        #endregion

        #region Reporte 5.4  EVOLUCIÓN DE LOS CAUDALES
        public const string GraficoX_Reporte_5p4 = "Evolución del promedio semanal de {0} en los años {1} - {2}.";

        #endregion

        #region Reporte 6.1  CONSUMO DE COMBUSTIBLE Por tipo de combustible
        public const string Cuadro1_Reporte_6p1 = "Evolución diaria de los consumos de combustibles líquidos, sólidos y gasosos en el SEIN durante la semana operativa {0} - {1}.";
        public const string Grafico1_Reporte_6p1 = "Comparación diaria del consumo del gas natural y biogás durante la semana operativa {0} - {1}.";
        public const string Grafico2_Reporte_6p1 = "Comparación diaria del consumo de diesel 2, residual 6 y residual 500 durante la semana operativa {0} - {1}.";
        public const string Grafico3_Reporte_6p1 = "Consumo de bagazo durante la semana operativa {0} - {1}.";

        #endregion

        #region Reporte 7.1  Evolución  de los Costos de Operación acumulado semanal (Millones de S/.)
        public const string Cuadro1_Reporte_7p1 = "Comparación de los costos de operación acumulado semanal del SEIN de los años {0}, {1} y {2}.";
        public const string Grafico1_Reporte_7p1 = "Evolución de los costos de operación ejecutado acumulado semanal del SEIN de los años {0}, {1} y {2}.";

        #endregion

        #region Reporte 8.1  Evolución  de los Costos Marginales Nodales Promedio semanal (US$/MWh)
        public const string Cuadro1_Reporte_8p1 = "Comparación de los costos Marginales Promedio Ponderado del SEIN  (Barra de Referencia Santa Rosa) de los años  {0}, {1} y {2}.";
        public const string Grafico1_Reporte_8p1 = "Evolución de los costos Marginales Promedio semanal del SEIN  (Barra de Referencia Santa Rosa) de los años  {0}, {1} y {2}.";

        #endregion

        #region Reporte 8.2  Evolución  de los Costos Marginales Nodales Promedio semanal por área operativa (US$/MWh)
        public const string Cuadro1_Reporte_8p2 = "Comparación de los costos Marginales Promedio semanal por área operativa del SEIN del año {0}.";
        #endregion

        #region Reporte 9.1  PREFIL DE TENSIÓN EN BARRAS DE LA RED DE 500kV
        public const string Cuadro1_Reporte_9p1 = "Perfil de tensión en barras de la red de 500kV durante la semana operativa N° {0} - {1}.";
        public const string Grafico1_Reporte_9p1 = "Perfil de tensión en barras de la red de 500kV de la semana operativa {0} - {1}.";

        #endregion

        #region Reporte 9.2  PREFIL DE TENSIÓN EN BARRAS DE LA RED DE 220kV
        public const string Cuadro1_Reporte_9p2 = "Perfil de tensión en barras de la red de 220kV durante la semana operativa N° {0} - {1}.";
        public const string Grafico1_Reporte_9p2 = "Perfil de tensión en barras de la red de 220kV de la semana operativa {0} - {1}.";

        #endregion

        #region Reporte 9.3  PREFIL DE TENSIÓN EN BARRAS DE LA RED DE 138kV
        public const string Cuadro1_Reporte_9p3 = "Perfil de tensión en barras de la red de 138V durante la semana operativa N° {0} - {1}.";
        public const string Grafico1_Reporte_9p3 = "Perfil de tensión en barras de la red de 138kV de la semana operativa {0} - {1}.";

        #endregion

        #region Reporte 10.1  FLUJOS MÁXIMO DE INTERCONEXIONES EN LOS ENLACES CENTRO NORTE Y CENTRO SUR 
        public const string Cuadro1_Reporte_10p1 = "Flujos de Interconexión CENTRO - {1} de la semana operativa {0}.";
        public const string Grafico1_Reporte_10p1 = "Evolución de los flujos máximos de potencia en el Enlace de Interconexión CENTRO - {1} de la semana operativa {0}.";

        #endregion

        #region Reporte 11.1  HORAS DE CONGESTION POR ÁREA OPERATIVA
        public const string Cuadro1_Reporte_11p1 = "Horas de operación de los principales equipos de congestion en la semana operativa {0} {1}, {2}, {3}.";
        public const string Cuadro1_Reporte_11p1_ = "Horas de operación de los principales equipos de congestion en la semana operativa {0}, {1}, {2}.";
        public const string Grafico1_Reporte_11p1 = "Comparación de las horas de operación de los principales equipos de congestion de los años {0}, {1} y {2}.";

        #endregion

        #region Reporte 13.1  Intercambios de Electricidad de energía y potencia
        public const string Cuadro1_Reporte_13p1 = "Intercambio de energía y potencia durante la semana operativa {0} - {1}.";
        public const string Grafico1_Reporte_13p1 = "Comparación diaria los intercambios de energía y potencia durante la semana operativa ";

        #endregion

        #region Reporte 14.1  FALLAS POR TIPO DE EQUIPO Y CAUSA SEGÚN CLASIFICACION CIER
        public const string Cuadro1_Reporte_14p1 = "Número de fallas y energía interrumpida (MWh) por tipo de equipo y Causa según clasificacion CIER durante la semana operativa {0}.";
        public const string Grafico1_Reporte_14p1 = "Porcentaje de participación por tipo de causa en el número de fallas.";
        public const string Grafico2_Reporte_14p1 = "Comparación en el número de fallas por tipo de equipo.";
        public const string Grafico3_Reporte_14p1 = "Comparación de la energía interrumpida aproximada por tipo de equipo durante la semana operativa {0}.";

        #endregion

        #endregion
    }

    public class NotasPieExcelInformeMensual
    {
        #region NombreCuadrosGraficos

        #region Reporte 1.1
        public const string I_Cuadro1_Reporte_1p1 = "Cuadro  N°1: ";
        public const string I_Grafico1_Reporte_1p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_1p1 = "Cuadro  N°1: ";
        public const string G_Grafico1_Reporte_1p1 = "Gráfico N°1: ";
        #endregion

        #region Reporte 1.2
        public const string I_Cuadro1_Reporte_1p2 = "Cuadro  N°1: ";
        public const string I_Grafico1_Reporte_1p2 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_1p2 = "Cuadro  N°2: ";
        #endregion

        #region Reporte 1.3
        public const string I_Cuadro1_Reporte_1p3 = "Cuadro  N°1: ";
        public const string I_Grafico1_Reporte_1p3 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_1p3 = "Cuadro  N°3: ";
        public const string G_Grafico1_Reporte_1p3 = "Gráfico N°2: ";
        #endregion

        #region Reporte 2.1

        public const string I_Cuadro1_Reporte_2p1 = "Cuadro  N° 1: ";
        public const string I_Grafico1_Reporte_2p1 = "Gráfico N° 1: ";


        public const string G_Cuadro1_Reporte_2p1 = "Cuadro  N° 4: ";
        public const string G_Grafico1_Reporte_2p1 = "Gráfico N° 3: ";

        #endregion

        #region Reporte 2.2
        public const string I_Cuadro1_Reporte_2p2 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_2p2 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_2p2 = "Cuadro N°5: ";
        public const string G_Grafico1_Reporte_2p2 = "Gráfico N°4: ";

        #endregion

        #region Reporte 2.3
        public const string I_Cuadro1_Reporte_2p3 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_2p3 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_2p3 = "Gráfico N°2: ";

        public const string G_Cuadro1_Reporte_2p3 = "Cuadro N°6: ";
        public const string G_Grafico1_Reporte_2p3 = "Gráfico N°5: ";
        public const string G_Grafico2_Reporte_2p3 = "Gráfico N°6: ";

        #endregion

        #region Reporte 2.4

        public const string I_Cuadro1_Reporte_2p4 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_2p4 = "Gráfico N°1: ";
        public const string I_Grafico2_Reporte_2p4 = "Gráfico N°2: ";


        public const string G_Cuadro1_Reporte_2p4 = "Cuadro N°7: ";
        public const string G_Grafico1_Reporte_2p4 = "Gráfico N°7: ";
        public const string G_Grafico2_Reporte_2p4 = "Gráfico N°8: ";

        #endregion

        #region Reporte 2.5

        public const string I_Cuadro1_Reporte_2p5 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_2p5 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_2p5 = "Cuadro N°8: ";
        public const string G_Grafico1_Reporte_2p5 = "Gráfico N°9: ";

        #endregion

        #region Reporte 3.1

        public const string I_Cuadro1_Reporte_3p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_3p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_3p1 = "Cuadro N°9: ";
        public const string G_Grafico1_Reporte_3p1 = "Gráfico N°10: ";

        #endregion

        #region Reporte 3.2

        public const string I_Cuadro1_Reporte_3p2 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_3p2 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_3p2 = "Cuadro N°10: ";
        public const string G_Grafico1_Reporte_3p2 = "Gráfico N°11: ";

        #endregion



        #region Reporte 4.1
        public const string I_Cuadro1_Reporte_4p1 = "Cuadro  N° 1: ";

        public const string G_Cuadro1_Reporte_4p1 = "Cuadro  N° 11: ";

        #endregion

        #region Reporte 4.2
        public const string I_Grafico1_Reporte_4p2 = "Gráfico N° 1: ";
        public const string I_Grafico2_Reporte_4p2 = "Gráfico N° 2: ";
        public const string I_Grafico3_Reporte_4p2 = "Gráfico N° 3: ";

        public const string G_Grafico1_Reporte_4p2 = "Gráfico N° 12: ";
        public const string G_Grafico2_Reporte_4p2 = "Gráfico N° 13: ";
        public const string G_Grafico3_Reporte_4p2 = "Gráfico N° 14: ";

        #endregion

        #region Reporte 4.3
        public const string I_Cuadro1_Reporte_4p3 = "Cuadro N° 1: ";

        public const string G_Cuadro1_Reporte_4p3 = "Cuadro N° 12: ";

        #endregion

        #region Reporte 4.4
        public const string I_Grafico1_Reporte_4p4 = "Gráfico N° 1: ";
        public const string I_Grafico2_Reporte_4p4 = "Gráfico N° 2: ";
        public const string I_Grafico3_Reporte_4p4 = "Gráfico N° 3: ";
        public const string I_Grafico4_Reporte_4p4 = "Gráfico N° 4: ";

        public const string G_Grafico1_Reporte_4p4 = "Gráfico N° 15: ";
        public const string G_Grafico2_Reporte_4p4 = "Gráfico N° 16: ";
        public const string G_Grafico3_Reporte_4p4 = "Gráfico N° 17: ";
        public const string G_Grafico4_Reporte_4p4 = "Gráfico N° 18: ";

        #endregion

        #region Reporte 5.1
        public const string I_Cuadro1_Reporte_5p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_5p1 = "Gráfico N°1: ";
        public const string I_Cuadro2_Reporte_5p1 = "Cuadro N°2: ";
        public const string I_Grafico2_Reporte_5p1 = "Gráfico N°2: ";
        public const string I_Cuadro3_Reporte_5p1 = "Cuadro N°3: ";
        public const string I_Grafico3_Reporte_5p1 = "Gráfico N°3: ";

        public const string G_Cuadro1_Reporte_5p1 = "Cuadro N°13: ";
        public const string G_Grafico1_Reporte_5p1 = "Gráfico N°19: ";
        public const string G_Cuadro2_Reporte_5p1 = "Cuadro N°14: ";
        public const string G_Grafico2_Reporte_5p1 = "Gráfico N°20: ";
        public const string G_Cuadro3_Reporte_5p1 = "Cuadro N°15: ";
        public const string G_Grafico3_Reporte_5p1 = "Gráfico N°21: ";

        #endregion



        #region Reporte 6.1
        public const string I_Cuadro1_Reporte_6p1 = "Cuadro N°1: ";
        public const string I_Grafico1_Reporte_6p1 = "Gráfico N°1: ";

        public const string G_Cuadro1_Reporte_6p1 = "Cuadro N°16: ";
        public const string G_Grafico1_Reporte_6p1 = "Gráfico N°22: ";
        #endregion



        #region Reporte 7.1
        public const string I_Cuadro1_Reporte_7p1 = "Cuadro N°1:  ";
        public const string I_Grafico1_Reporte_7p1 = "Gráfico N°1:  ";
        public const string I_Grafico2_Reporte_7p1 = "Gráfico N°2:  ";
        public const string I_Grafico3_Reporte_7p1 = "Gráfico N°3:  ";

        public const string G_Cuadro1_Reporte_7p1 = "Cuadro N°17: ";
        public const string G_Grafico1_Reporte_7p1 = "Gráfico N°23: ";
        public const string G_Grafico2_Reporte_7p1 = "Gráfico N°24: ";
        public const string G_Grafico3_Reporte_7p1 = "Gráfico N°25: ";
        #endregion

        #endregion
    }

    public class NotasPieWebInformeMensual
    {
        #region NombreCuadrosGraficos

        #region Reporte 1.1  INGRESO EN OPERACIÓN COMERCIAL AL SEIN
        public const string Cuadro1_Reporte_1p1 = "Relación de ingresos a operación comercial en el periodo Enero - {0} del año {1}.";
        public const string Grafico1_Reporte_1p1 = "Ingreso de Potencia Instalada por tipo de Recurso Energético y Tecnología de Enero a {0} {1} (MW).";

        #endregion

        #region Reporte 1.2  RETIRO DE OPERACIÓN COMERCIAL DEL SEIN
        public const string Cuadro1_Reporte_1p2 = "Relación de retiros de operación comercial en el periodo Enero - {0} del año {1}.";
        public const string Grafico1_Reporte_1p2 = "Salida de Potencia Efectiva por tipo de Recurso Energético y Tecnología de Enero a {0} {1} (MW).";

        #endregion

        #region Reporte 1.3  POTENCIA INSTALADA EN EL SEIN
        public const string Cuadro1_Reporte_1p3 = "Comparación de la potencia instalada en el SEIN {0} {1} y {0} {2}";
        public const string Grafico1_Reporte_1p3 = "Comparación de la potencia instalada en el SEIN {0} {1} y {0} {2}";

        #endregion

        #region Reporte 2.1   PRODUCCIÓN POR TIPO DE GENERACIÓN

        public const string Cuadro1_Reporte_2p1 = "Producción de energía eléctrica (GWh) por tipo de generación en el SEIN";
        public const string Grafico1_Reporte_2p1 = "Comparación de la producción de energía eléctrica acumulada por tipo de generación periodo enero - {0}";

        #endregion

        #region Reporte 2.2  PRODUCCIÓN POR TIPO DE RECURSO ENERGÉTICO (GWh)
        public const string Cuadro1_Reporte_2p2 = "Producción de energía eléctrica (GWh) por tipo de recurso energético en el SEIN.";
        public const string Grafico1_Reporte_2p2 = "Comparación de la producción de energía eléctrica acumulada por tipo de recurso energético, periodo enero - {0}.";

        #endregion

        #region Reporte 2.3  PRODUCCIÓN POR RECURSOS ENERGÉTICOS RENOVABLES (GWh)
        public const string Cuadro1_Reporte_2p3 = "Producción de energía eléctrica (GWh) con recursos energético renovables en el SEIN.";
        public const string Grafico1_Reporte_2p3 = "Comparación de la producción de energía eléctrica acumulada con recursos energéticos renovables, periodo enero - {0} de los años {1}, {2} y {3}";
        public const string Grafico2_Reporte_2p3 = "Participación de las RER en la Matriz de Generación del SEIN - Periodo enero a {0} {1}";

        #endregion

        #region Reporte 2.4  FACTOR DE PLANTA DE LAS CENTRALES RER DEL SEIN

        public const string Cuadro1_Reporte_2p4 = "Producción de energía eléctrica (GWh) y factor de planta de las centrales con recursos energético renovables en el SEIN (Valores considerados a partir del inicio de su operación comercial)";
        public const string Grafico1_Reporte_2p4 = "Producción de energía eléctrica (GWh) y factor de planta de las centrales con recursos energético renovables por tipo de generación en el SEIN - {0} {1}";
        public const string Grafico2_Reporte_2p4 = "factor de planta de las centrales con recursos energético renovables en el SEIN, en el periodo acumulado enero - {0} de los años {1} y {2}.";

        #endregion

        #region Reporte 2.5  PARTICIPACIÓN DE LA PRODUCCIÓN (GWh) POR EMPRESAS INTEGRANTES

        public const string Cuadro1_Reporte_2p5 = "Participación de las empresas generadoras del COES en la producción de energía eléctrica (GWh) en el mes de {0} de los años {1} y {2}.";
        public const string Grafico1_Reporte_2p5 = "Comparación de producción energética (GWh) de las empresas generadoras del COES en el mes de {0} de los años {1} y {2}.";

        #endregion


        #region Reporte 3.1  MÁXIMA DEMANDA COINCIDENTE DE POTENCIA POR TIPO DE GENERACIÓN (MW)

        public const string Cuadro1_Reporte_3p1 = "Máxima demanda coincidente de potencia (MW) por tipo de generación en el SEIN.";
        public const string Grafico1_Reporte_3p1 = "Comparación de la máxima demanda coincidente de potencia (MW) por tipo de generación en el SEIN, en el periodo enero - {0} de los años  {1}, {2} y {3} ";


        #endregion


        #region Reporte 3.2  PARTICIPACIÓN DE LAS EMPRESAS INTEGRANTES EN LA MÁXIMA DEMANDA COINCIDENTE (MW)

        public const string Cuadro1_Reporte_3p2 = "Participación de las empresas generadoras del COES en la máxima demanda coincidente (MW) en el mes de {0} de los años {1} y {2}.";
        public const string Grafico1_Reporte_3p2 = " Comparación de la máxima demanda coincidente  (MW) de las empresas generadoras del COES en el mes de {0} de los años {1} y {2}";

        #endregion       

        #region Reporte 4.1  VOLÚMEN UTIL DE LOS EMBALSES Y LAGUNAS (Millones de m3)
        public const string Cuadro1_Reporte_4p1 = "Volúmen útil de los principales embalses y lagunas del SEIN al término del periodo mensual ( {0} ) de los años {1} y {2}.";

        #endregion

        #region Reporte 4.2  EVOLUCIÓN DE VOLUMENES DE LOS EMBALSES Y LAGUNAS
        public const string GraficoX_Reporte_4p2 = "Evolución semanal del volumen {0} durante los años {1} - {2}.";

        #endregion

        #region Reporte 4.3  PROMEDIO MENSUAL DE LOS CAUDALES (m3/s)
        public const string Cuadro1_Reporte_4p3 = "Promedio de caudales de los meses de {0} {1} y {2}.";

        #endregion

        #region Reporte 4.4  EVOLUCIÓN DE LOS CAUDALES
        public const string GraficoX_Reporte_4p4 = "Evolución del promedio semanal de {0} en los años {1} - {2}.";

        #endregion

        #region Reporte 5.1  Evolución  de los Costos Marginales Nodales Promedio semanal (US$/MWh)
        public const string CuadroX_Reporte_5p1 = "Valor de los costos marginales medios registrados en las principales barras del área {0} durante el mes de {1}.";
        public const string GraficoX_Reporte_5p1 = "Costos marginales medios registrados en las principales barras del área {0} durante el mes de {1}.";

        #endregion

        #region Reporte 6.1  HORAS DE CONGESTION POR ÁREA OPERATIVA
        public const string Cuadro1_Reporte_6p1 = "Horas de operación de los principales equipos de congestion en {0} de los años {1}, {2}, {3}.";
        public const string Grafico1_Reporte_6p1 = "Comparación de las horas de operación de los principales equipos de congestion en {0} de los años {1}, {2} y {3}.";

        #endregion        

        #region Reporte 7.1  FALLAS POR TIPO DE EQUIPO Y CAUSA SEGÚN CLASIFICACION CIER
        public const string Cuadro1_Reporte_7p1 = "Número de fallas y energía interrumpida (MWh) por tipo de equipo y Causa según clasificacion CIER en el mes de {0}.";
        public const string Grafico1_Reporte_7p1 = "Porcentaje de participación por tipo de causa en el número de fallas.";
        public const string Grafico2_Reporte_7p1 = "Comparación en el número de fallas por tipo de equipo.";
        public const string Grafico3_Reporte_7p1 = "Comparación de la energía interrumpida aproximada por tipo de equipo en el mes de {0}.";

        #endregion

        #endregion
    }

    /// <summary>
    /// Correlativos de la tabla SI_MENUREPORTE para el Anexo A
    /// </summary>
    public class ConstantesAnexoAPR5
    {        
        public const int IndexReporteEventos = 76;
        public const int IndexReporteRestriccionesOperativas = 77;
        public const int IndexReporteIngresoOperacionCISEIN = 78;

        public const int IndexDespachoRegistrado = 4;
        public const int IndexReporteDemandaPorArea = 5;
        public const int IndexReporteDemandaGrandesUsuarios = 6;
        public const int IndexReporteRecursosEnergeticosDemandaSEIN = 7;
        public const int IndexReporteProduccionEnergiaDiaria = 184;
        public const int IndexReporteGeneracionDelSEIN = 8;

        public const int IndexReporteHorasOrdenAPIS = 10;
        public const int IndexReporteHoraInicioFinIndisponibilidad = 11;
        public const int IndexReporteReservaFriaSistema = 12;
        public const int IndexReporteCaudalesCentralHidroelectrica = 13;
        public const int IndexReporteHorariosCaudalVolumenCentralHidroelectrica = 14;
        public const int IndexReporteVertimientosPeriodoVolumen = 18;
        public const int IndexReporteCantidadCombustibleCentralTermica = 171;
        public const int IndexReporteCombustibleConsumidoUnidadTermoelectrica = 175;
        public const int IndexReporteConsumoYPresionDiarioUnidadTermoelectrica = 176;
        public const int IndexReporteRegistroEnergiaPrimaria30Unidades = 177;
        public const int IndexReporteCalorUtilGeneracionProceso = 179;

        public const int IndexReportePALineasTransmision = 15;
        public const int IndexReporteTensionBarrasSEIN = 20;
        public const int IndexReporteSobrecargaEquipos = 21;
        public const int IndexReporteLineasDesconectadasPorTension = 172;
        public const int IndexReporteSistemasAisladosTemporales = 173;

        public const int IndexReporteVariacionesSostenidasSubitas = 16;
        public const int IndexReporteSistemasAisladosTemporalesYVariacionesSostenidasSubitas = 180;
        public const int IndexReporteInterrupSumERACyRMCRacionamiento = 340;

        public const int IndexReporteDesviacionesDemandaPronostico = 17;
        public const int IndexReporteDesviacionesProduccionUG = 174;

        public const int IndexReporteCostoMarginalesCortoPlazo = 3;
        public const int IndexReporteCostoTotalOperacionEjecutada = 178;
        public const int IndexReporteCalificacionOperacionUnidades = 181;
        public const int IndexReporteRegistroCongestionesST = 182;
        public const int IndexReporteAsignacionRRPFyRRSF = 183;

        public const int IndexReporteRegistroFlujosEnlacesInternacionales = 2;

        public const int IndexObservacion = 294;
        public const int IndexRecomendacionConclusion = 295;
    }

    /// <summary>
    /// Correlativos de la tabla SI_MENUREPORTE para el Anexo A
    /// </summary>
    public class ConstantesVersionAnexoAPR5
    {
        public const decimal IndexReporteEventos = 1.1m;
        public const decimal IndexReporteEventosInterrup = 1.2m;
        public const decimal IndexReporteEventosEquipo = 1.3m;
        public const decimal IndexReporteRestriccionesOperativasRestricOp = 2.1m;
        public const decimal IndexReporteRestriccionesOperativasMantto = 2.2m;
        public const decimal IndexReporteIngresoOperacionCISEIN = 3;

        public const decimal IndexDespachoRegistradoMW = 4.1m;
        public const decimal IndexDespachoRegistradoMvar = 4.2m;
        public const decimal IndexReporteDemandaPorArea = 5;
        public const decimal IndexReporteDemandaGrandesUsuarios = 6;
        public const decimal IndexReporteRecursosEnergeticosDemandaSEINRer1 = 7.11m;
        public const decimal IndexReporteRecursosEnergeticosDemandaSEINRer2 = 7.12m;
        public const decimal IndexReporteRecursosEnergeticosDemandaSEIN1 = 7.21m;
        public const decimal IndexReporteRecursosEnergeticosDemandaSEIN2 = 7.22m;
        public const decimal IndexReporteProduccionEnergiaDiaria = 8;
        public const decimal IndexReporteGeneracionDelSEIN = 9;

        public const decimal IndexReporteHorasOrdenAPIS = 10;
        public const decimal IndexReporteHoraInicioFinIndisponibilidad = 11;
        public const decimal IndexReporteReservaFriaSistema = 12;
        public const decimal IndexReporteCaudalesCentralHidroelectrica = 13.1m;
        public const decimal IndexReporteCaudalesCentralHidroelectricaPto = 13.2m;
        public const decimal IndexReporteHorariosCaudalVolumenCentralHidroelectrica = 14.1m;
        public const decimal IndexReporteHorariosCaudalVolumenCentralHidroelectricaPto = 14.2m;
        public const decimal IndexReporteHorariosCaudalVolumenCentralHidroelectricaDescarga = 14.3m;
        public const decimal IndexReporteVertimientosPeriodoVolumen = 15;
        public const decimal IndexReporteCantidadCombustibleCentralTermica = 16;
        public const decimal IndexReporteCombustibleConsumidoUnidadTermoelectrica = 17;
        public const decimal IndexReporteConsumoYPresionDiarioUnidadTermoelectricaConsumo = 18.1m;
        public const decimal IndexReporteConsumoYPresionDiarioUnidadTermoelectricaPresion = 18.2m;
        public const decimal IndexReporteConsumoYPresionDiarioUnidadTermoelectricaTemperatura = 18.3m;
        public const decimal IndexReporteRegistroEnergiaPrimaria30Unidades = 19.1m;
        public const decimal IndexReporteRegistroEnergiaPrimaria30UnidadesPto = 19.2m;
        public const decimal IndexReporteCalorUtilGeneracionProceso = 20.1m;
        public const decimal IndexReporteCalorUtilGeneracionProcesoPto = 20.2m;

        public const decimal IndexReportePALineasTransmision = 21;
        public const decimal IndexReporteTensionBarrasSEIN = 23.1m;
        public const decimal IndexReporteTensionBarrasSEINPto = 23.2m;
        public const decimal IndexReporteSobrecargaEquipos = 24;
        public const decimal IndexReporteLineasDesconectadasPorTension = 25;
        public const decimal IndexReporteSistemasAisladosTemporales = 26;

        public const decimal IndexReporteVariacionesSostenidasSubitasGPS = 27.1m;
        public const decimal IndexReporteVariacionesSostenidasSubitasGrafico = 27.2m;

        public const decimal IndexReporteDesviacionesDemandaPronostico = 30;
        public const decimal IndexReporteDesviacionesProduccionUG = 31;

        public const decimal IndexReporteCostoMarginalesCortoPlazo = 32;
        public const decimal IndexReporteCostoTotalOperacionEjecutada = 33;
        public const decimal IndexReporteCalificacionOperacionUnidades = 34;
        public const decimal IndexReporteRegistroCongestionesST = 35;
        public const decimal IndexReporteAsignacionRRPFyRRSFMatriz = 36;
        public const decimal IndexReporteAsignacionRRPFyRRSFValor = 36;

        public const decimal IndexReporteRegistroFlujosEnlacesInternacionales = 37.1m;
        public const decimal IndexReporteRegistroFlujosEnlacesInternacionalesPto = 37.2m;
    }


    /// <summary>
    /// Correlativos de la tabla SI_MENUREPORTE para el Anexo A
    /// </summary>
    public class ConstantesAnexoAPR9
    {
        /*public const int IndexReporteEventos = 76;
        public const int IndexReporteRestriccionesOperativas = 77;
        public const int IndexReporteIngresoOperacionCISEIN = 78;

        public const int IndexDespachoRegistrado = 4;
        public const int IndexReporteDemandaPorArea = 5;
        public const int IndexReporteDemandaGrandesUsuarios = 6;
        public const int IndexReporteRecursosEnergeticosDemandaSEIN = 7;
        public const int IndexReporteProduccionEnergiaDiaria = 184;
        public const int IndexReporteGeneracionDelSEIN = 8;

        public const int IndexReporteHorasOrdenAPIS = 10;
        public const int IndexReporteHoraInicioFinIndisponibilidad = 11;
        public const int IndexReporteReservaFriaSistema = 12;
        public const int IndexReporteCaudalesCentralHidroelectrica = 13;
        public const int IndexReporteHorariosCaudalVolumenCentralHidroelectrica = 14;
        public const int IndexReporteVertimientosPeriodoVolumen = 18;
        public const int IndexReporteCantidadCombustibleCentralTermica = 171;
        public const int IndexReporteCombustibleConsumidoUnidadTermoelectrica = 175;
        public const int IndexReporteConsumoYPresionDiarioUnidadTermoelectrica = 176;
        public const int IndexReporteRegistroEnergiaPrimaria30Unidades = 177;
        public const int IndexReporteCalorUtilGeneracionProceso = 179;

        public const int IndexReportePALineasTransmision = 15;
        public const int IndexReporteTensionBarrasSEIN = 20;
        public const int IndexReporteSobrecargaEquipos = 21;
        public const int IndexReporteLineasDesconectadasPorTension = 172;
        public const int IndexReporteSistemasAisladosTemporales = 173;

        public const int IndexReporteVariacionesSostenidasSubitas = 16;

        public const int IndexReporteDesviacionesDemandaPronostico = 17;
        public const int IndexReporteDesviacionesProduccionUG = 174;

        public const int IndexReporteCostoMarginalesCortoPlazo = 3;
        public const int IndexReporteCostoTotalOperacionEjecutada = 178;
        public const int IndexReporteCalificacionOperacionUnidades = 181;
        public const int IndexReporteRegistroCongestionesST = 182;
        public const int IndexReporteAsignacionRRPFyRRSF = 183;

        public const int IndexReporteRegistroFlujosEnlacesInternacionales = 2;*/

        //Anexo A - Migraciones GrupoB
        public const int IndexReporteRestriccionesOperativasEjec = 195;
        public const int IndexRequerimientosPropios = 204;
        public const int IndexReporteQuemaGasNoEmpleado = 203;
        public const int IndexReporteDisponibilidadGas = 202;
        public const int IndexReporteRestriccionSuministros = 198;
    }

    /// <summary>
    /// Correlativos de la tabla SI_MENUREPORTE para el Ejecutivo Semanal
    /// </summary>
    public class ConstantesEjecutivoSemanalPR5
    {
        public const int IndexResumenRelevante = 108;

        public const int CatecodiOfertaGeneracion = 81;
        public const int IndexIngresoOpComercSEIN = 22;
        public const int IndexRetiroOpComercSEIN = 23;

        public const int CatecodiProduccionEnergia = 82;
        public const int IndexProdTipoGen = 24;
        public const int IndexProdTipoRecurso = 25;
        public const int IndexProdRER = 26;
        public const int IndexFactorPlantaRER = 27;
        public const int IndexParticipacionEmpresas = 28;

        public const int CatecodiMDCoincidente = 83;
        public const int IndexMaximaDemandaTipoGeneracionEjecut = 29;
        public const int IndexMaximaDemandaXEmpresaEjecut = 30;
        public const int IndexDemandaXAreaOpeEjecut = 31;

        public const int CatecodiHidrologia = 84;
        public const int IndexVolUtilEmbLag = 32;
        public const int IndexEvolucionVolEmbLag = 33;
        public const int IndexPromCaudales = 34;
        public const int IndexEvolucionCaudalesEjecut = 35;

        public const int CatecodiCostoOperacion = 85;
        public const int IndexEvolCostosOperacionEjecutados = 36;

        public const int CatecodiCostoMarginal = 86;
        public const int IndexEvolCostosMarginalesProm = 37;

        public const int CatecodiFlujoInterconexion = 87;
        public const int IndexFlujoMaximoInterconexionesEjecut = 38;

        public const int CatecodiHoraCongestion = 88;
        public const int IndexHorasCongestionAreaOpeEjecut = 39;

        public const int CatecodiCombustibles = 89;
        public const int IndexConsumoCombustibleEjecut = 40;

        public const int CatecodiIntercambioInternacionales = 90;
        public const int IndexIntercambioInternacionalesEjecut = 41;

        public const int CatecodiEventoFallas = 91;
        public const int IndexEventoFallaSuministroEnergEjecut = 42;

        public const int CatecodiAnexos = 92;
        public const int IndexEventoFallaSuministroEnergAnexoEjecut = 43;

        public const int IndexEventoDetalleEvento = 342;
    }

    /// <summary>
    /// Correlativos de la tabla SI_MENUREPORTE para el Informe Semanal
    /// </summary>
    public class ConstantesInformeSemanalPR5
    {
        public const int IndexResumenRelevante = 108;
        public const int IndexResumenProduccion = 343;

        public const int CatecodiOfertaGeneracion = 276;
        public const int IndexSemIngresoOpComercSEIN = 44;
        public const int IndexSemRetiroOpComercSEIN = 45;

        public const int CatecodiProduccionEnergia = 288;
        public const int IndexSemProdTipoGen = 46;
        public const int IndexSemProdTipoRecurso = 47;
        public const int IndexSemProdRER = 48;
        public const int IndexSemFactorPlantaRER = 49;
        public const int IndexSemParticipacionEmpresas = 50;

        public const int CatecodiMDCoincidente = 278;
        public const int IndexMaximaDemandaTipoGeneracionSemanal = 51;
        public const int IndexMaximaDemandaXEmpresaSemanal = 52;
        public const int IndexDemandaXAreaOpeSemanal = 53;

        public const int CatecodiDemandaGU = 289;
        public const int IndexDemandaGUMaximaDemandaSemanal = 54;
        public const int IndexDiagramaCargaGURangoPotencia = 55;
        public const int IndexDemandaGUXAreaOperativa = 56;

        public const int CatecodiHidrologia = 279;
        public const int IndexSemVolUtilEmbLag = 57;
        public const int IndexEvolucionVolEmbLagSem = 58; 
        public const int IndexSemPromCaudales = 59;
        public const int IndexEvolucionCaudalesSem = 60;

        public const int CatecodiCombustibles = 284;
        public const int IndexConsumoCombustibleSemanal = 61;

        public const int CatecodiCostoOperacion = 280;
        public const int IndexSemEvolCostosOperacionEjecutados = 62;

        public const int CatecodiCostoMarginal = 290;
        public const int IndexSemEvolCostosMarginalesProm = 63;
        public const int IndexSemEvolCostosMarginalesPorArea = 64;

        public const int CatecodiTensionBarra = 291;
        public const int IndexTensionBarras500Semanal = 65;
        public const int IndexTensionBarras220Semanal = 66;
        public const int IndexTensionBarras138Semanal = 67;

        public const int CatecodiFlujoInterconexion = 282;
        public const int IndexFlujoMaximoInterconexiones = 68;

        public const int CatecodiHoraCongestion = 283;
        public const int IndexHorasCongestionAreaOpeSemanal = 69;

        public const int CatecodiManttoEjec = 292;
        public const int IndexMantenimientoEjecutadosSemanal = 70;

        public const int CatecodiIntercambioInternacionales = 285;
        public const int IndexIntercambioInternacionalesSemanal = 71;

        public const int CatecodiEventoFallas = 286;
        public const int IndexEventoFallaSuministroEnerg = 72;

        public const int CatecodiAnexos = 287;
        public const int IndexEventoFallaSuministroEnergAnexoSemanal = 75;

        public const int IndexDetalleEventos = 341;
        public const int PerteneceNumeral = 1;
    }

    /// <summary>
    /// Tabla maestra de SI_VERSION_CONCEPTO
    /// </summary>
    public class ConstantesConceptoVersion
    {
        /// <summary>
        /// FENERGCODI - CODIGO DE FUENTE DE ENERGIA
        /// </summary>
        /// 
        public const int ConceptoCodigoFenergcodi = 1;
        public const int ConceptoCodigoBarrcodiAreaNorte = 2;
        public const int ConceptoCodigoBarrcodiAreaCentro = 3;
        public const int ConceptoCodigoBarrcodiAreaSur = 4;
        public const int ConceptoAnioActualVolUtil = 5;
        public const int ConceptoAnioAnteriorVolUtil = 6;
        public const int ConceptoCodiEmbalseyLaguna = 7;
        public const int ConceptoCodigoUltimas3SemanasMaxDemanda = 8;
        public const int ConceptoCodigoAnioAnteriorMaxDemanda = 9;
        public const int ConceptoCodigoMaxDemandaPeriodo = 10;
        public const int ConceptoCodiCaudalNatural = 11;
        public const int ConceptoCodiCaudalDescargado = 12;
        public const int ConceptoCodigoMaxTensionBarraRed = 13;
        public const int ConceptoCodigoMedTensionBarraRed = 14;
        public const int ConceptoCodigoMinTensionBarraRed = 15;
        public const int ConceptoCodigoBarrzareaNorte = 16;
        public const int ConceptoCodigoBarrzareaCentro = 17;
        public const int ConceptoCodigoBarrzareaSur = 18;        
        public const int ConceptoCodiHrsCongestionAreasOperativas = 19;
        public const int ConceptoCodigoCmgNorte = 20;
        public const int ConceptoCodigoCmgCentro = 21;
        public const int ConceptoCodigoCmgSur = 22;
        public const int ConceptoCodigoEcmpPonderaSEIN = 23;
        public const int ConceptoCodiCaudalNaturalGrafico = 24;
        public const int ConceptoCodiIntercambiosInternacionales = 25;
        public const int ConceptoCodigoFMInterconexionesNorte = 26;
        public const int ConceptoCodigoFMInterconexionesSur = 27;        
        public const int ConceptoCodiIntercambiosInternacAnt = 28;
        public const int ConceptoCodiIntercambiosInternacActual = 29;
        public const int ConceptoCodiIntercambiosInternacVariacion = 30;
        public const int ConceptoCodiFallasPorEquiposYCausa = 31;
        public const int ConceptoCodigoEvolucionDemandaAreasOperativas = 32;
        public const int ConceptoCodiMaxDemandaAnioActual = 33;
        public const int ConceptoCodiMaxDemandaAnioAnterior = 34;
        public const int ConceptoCodiMaxDemandaVariacion = 35;
        public const int ConceptoCodigoFactorPlantaCentralesRER = 36;
        public const int ConceptoCodiParticipacionProdAnioActual = 37;
        public const int ConceptoCodiParticipacionProdAnioAnterior = 38;
        public const int ConceptoCodiParticipacionProdVariacion = 39;

        public const int ConceptoCodiDemandaGUHPF = 40;
        public const int ConceptoCodiDemandaGUHP = 41;
        public const int ConceptoCodiGraficoGU = 42;
        public const int ConceptoCodigoProduccionRER = 43;
        public const int ConceptoCodiDemandaAreaOpeGU = 44;
        public const int ConceptoCodiDemandaEnergiaElectricaNorte = 45;
        public const int ConceptoCodiDemandaEnergiaElectricaCentro = 46;
        public const int ConceptoCodiDemandaEnergiaElectricaSur = 47;
        public const int ConceptoCodiTotalDemandaEnergiaGranUsuario = 48;
        public const int ConceptoCodiGraficoGrandesUsuariosLibres = 49;       
        public const int ConceptoCodiGraficoCargaRangosGUMayor100 = 50;
        public const int ConceptoCodiGraficoCargaRangosGUMe30_100 = 51;
        public const int ConceptoCodiGraficoCargaRangosGUMe20_30 = 52;
        public const int ConceptoCodiGraficoCargaRangosGUMenor20 = 53;
        public const int ConceptoCodiIngresoOperaciónComercial = 54;
        public const int ConceptoCodiRetiroOperaciónComercial = 55;  
        public const int ConceptoCodigoProduccionRecurso = 56;
        public const int ConceptoCodiEquiposSinPotenciaInstalada = 57;
        public const int ConceptoCodiPotenciaInstaladaActual = 58;
        public const int ConceptoCodiPotenciaInstaladaAnterior = 59;
        public const int ConceptoCodiVariacionPotenciaInstalada = 60;


       public const int ConceptoCodiProduccionElectrcidadHidro = 61;
       public const int ConceptoCodiProduccionElectrcidadTermo= 62;
       public const int ConceptoCodiProduccionElectrcidadRER = 63;
       public const int ConceptoCodiProduccionElectrcidadTotalPeriodo= 64;
       public const int ConceptoCodiProduccionElectrcidadAcumulado = 65;




        public const int ConceptoCodiProdTipoGeneracionPeriodoActual = 66;
        public const int ConceptoCodiProdTipoGeneracionPeriodoAnterior = 67;
        public const int ConceptoCodiProdTipoGeneracionPeriodoPrecAnterior = 68;
        public const int ConceptoCodiProdTipoGeneracionPeriodoAnteriorPrecAnterior = 69;
        public const int ConceptoCodiProdTipoGeneracionVariacionPeriodoAnterior = 70;
        public const int ConceptoCodiGeneracionAcumuladaPeriodoActual = 71;
        public const int ConceptoCodiGeneracionAcumuladaPeriodoAnterior = 72;
        public const int ConceptoCodiGeneracionAcumuladaPeriodoPrecAnterior = 73;
        public const int ConceptoCodiGeneracionAcumuladaVariacionActualVSAnterior= 74;
        public const int ConceptoCodiGeneracionAcumuladaVariacionAnteriorVSPrecedeAnterior = 75;
        public const int ConceptoCodiGraficoEvolucionSemanalEnergia = 76;
        public const int ConceptoCodiGraficoProdSemVariacionAnual = 77;

        public const int ConceptoCodiMaxDemandaPeriodoPrecAnterior = 78;
        public const int ConceptoCodiMaxDemandaPeriodoAnterior = 79;
        public const int ConceptoCodiMaxDemandaPeriodoActual = 80;
        public const int ConceptoCodiMaxDemandaPeriodoAnioAnterior = 81;
        
        public const int ConceptoCodiMaxDemandaPrecAnioAnterior= 82;
        public const int ConceptoCodiMaxDemandaVariacionAnioAnterior = 83;
        public const int ConceptoCodiMaxDemandaVariacionActualvsAnterior= 84;
        public const int ConceptoCodiMaxDemandaVariacionAnteriorvsPrecedeAnterior= 85;
        public const int ConceptoCodiGraficoComparacionMaximaDemanda = 86;
        public const int ConceptoCodiGraficoMaximaDemandaVariacionSem = 87;
        public const int ConceptoCodiGraficoCargaDespacho = 88;
        public const int ConceptoCodiGraficoMdsExpEcuador = 89;
        public const int ConceptoCodiResumenRelevante = 90;

        public const int ConceptoCodiMaxPotenciaMesActual = 91;
        public const int ConceptoCodiMaxPotenciaMesAnioAnt = 92;
        public const int ConceptoCodiMaxPotenciaAnioActual = 93;
        public const int ConceptoCodiMaxPotenciaAnioAnt = 94;
        public const int ConceptoCodiMaxPotenciaVarAnioActualVsAnterior = 95;
        public const int ConceptoCodiEventosDetalle = 96;

        public const int ConceptoCodiEnergiaProgramada = 97;
        public const int ConceptoCodiDesvEjecVsProg = 98;
        public const int ConceptoCodiGraficoEvolucionFuenteEnergia = 99;

        public const int ConceptoCodiGraficoEvolucionFuenteEnergiaEjecutivo = 138;
        public const int ConceptoCodiGraficoParticipacionRecursosEnergeticosEjec = 139;
        public const int ConceptoCodiPartRERAcumuladoActual = 140;
        public const int ConceptoCodiPartRERSemanaActual = 141;
        public const int ConceptoCodiTotalDemandaSEIN = 142;
        public const int ConceptoCodiTotalEventosFallas = 143;
        public const int ConceptoCodiTotalEnergInterrAprox = 144;
        public const int ConceptoCodiPartUtilRecEnerg = 145;
        public const int ConceptoCodiPartEmpProdTotal = 146;
        public const int ConceptoCodiPartEmpProdTotalMenores = 147;
        public const int ConceptoCodiGraficoComparacionCobertMax = 148;
        public const int ConceptoCodiGraficoCoberturaMAxPotCoincidente = 149;

        /// <summary>
        /// FECHA
        /// </summary>
        public const int ConceptoFecha = 100;
        /// <summary>
        /// Valor total
        /// </summary>
        public const int ConceptoTotal = 101;
        public const int ConceptoAcumuladoAnioActual = 102;
        public const int ConceptoAcumuladoAnioAnterior = 103;
        public const int ConceptoAcumuladoAnioPrecedeAnterior = 104;
        public const int ConceptoVariacionAcumAnioActualVsAnterior = 105;
        public const int ConceptoVariacionAcumAnioAnteriorVsPrecedeAnterior = 106;

        public const int ConceptoVolumenUtilSemanaInicio = 107;
        public const int ConceptoPorcentajeLenadoInicioSemana = 108;
        public const int ConceptoVolumenUtilSemanaFin = 109;
        public const int ConceptoPorcentajeLenadoFinSemana = 110;
        public const int ConceptoCapacidadLagunaEmbalse = 111;
        public const int ConceptoValorSemanaMaximaDemanda = 112;
        public const int ConceptoVariacionPorcentajeMaximaDemanda = 113;
        public const int ConceptoPorcentajeVariacion = 114;
        public const int ConceptoValorTensionBarraRed = 115;
        public const int ConceptoValorCostosMarginalesBarras = 116;       
        public const int ConceptoValorCostoMarMaxMes = 117;
        public const int ConceptoValorCostoMarPromMes = 118;
        public const int ConceptoValorCostoMarMinMes = 119;
        public const int ConceptoValorEcmpPonderaSEIN = 120;
        public const int ConceptoEnergiaExportada = 121;
        public const int ConceptoMaxdemExportada = 122;
        public const int ConceptoenergiaImportada = 123;
        public const int ConceptoMaxdemImportada = 124;
        public const int ConceptoValorFMInterconexiones = 125;
        public const int ConceptoValorEvolucionDemandaAreasOperativas = 126;
        public const int ConceptoValorFactorPlantaCentralesRER = 127;
        public const int ConceptoValorProduccionRER = 128;
        public const int ConceptoValorDemandaAcumAnioActual = 129;
        public const int ConceptoValorDemandaAcumAnioAnterior = 130;
        public const int ConceptoValorDemandaAcumVariacionAnioActualVsAnioAnt = 131;
        public const int ConceptoValorProduccionRecurso = 132;
        public const int ConceptoValorTotalProduccionElectricidad = 133;
        public const int ConceptoValorTotalIntercambiosInternacionales = 134;

        public const int ConceptoValorTotalGeneraciónHidro = 135;
        public const int ConceptoValorTotalGeneraciónTermo = 136;
        public const int ConceptoValorTotalGeneraciónRer = 137;

        public const int ConceptoCodiGrafPieMaxPot = 150;
        public const int ConceptoCodiGrafBarraMaxPot = 151;
        public const int ConceptoCodiGrafPieGenEnerg = 152;
        public const int ConceptoCodiGrafBarraGenEnerg = 153;
        public const int ConceptoCodiEvolIntegrantesCOES = 154;
        public const int ConceptoCodiIOEmprIntegrantesCOES = 155;
    }

    public class PR5ConstanteFecha 
    {
        //Variables de año actual
        public const int ValorAnioAct_SemAct = 1;
        public const int ValorAnioAct_SemAct2 = 12;
        public const int ValorAnioAct_Sem1Ant = 2;
        public const int ValorAnioAct_Sem2Ant = 3;
        public const int ValorAnioAct_SemAct_Var = 4;
        public const int ValorAnioAct_SemAct_Desv = 13;
        public const int ValorAnioAct_SemAct_Resta = 18;

        public const int ValorAnioAct_FechaInicial = 14;
        public const int ValorAnioAct_FechaFinal = 15;
        public const int ValorAnioAct_FechaInicial_Var = 16;
        public const int ValorAnioAct_FechaFinal_Var = 17;

        public const int ValorAnioAct_Acum = 5;
        public const int ValorAnioAct_Acum_Var = 6;

        public const int ValorAnioAct_MesAct = 7;
        public const int ValorAnioAct_MesAct_Var = 8;

        public const int ValorAnioAct_Total = 9;
        public const int ValorAnioAct_Total_Var = 10;

        //Variables de 1 año anterior
        public const int ValorAnio1Ant_SemAct = 21;

        public const int ValorAnio1Ant_FechaInicial = 26;
        public const int ValorAnio1Ant_FechaFinal = 27;
        public const int ValorAnio1Ant_FechaInicial_Var = 28;
        public const int ValorAnio1Ant_FechaFinal_Var = 29;

        public const int ValorAnio1Ant_Acum = 22;
        public const int ValorAnio1Ant_Acum_Var = 23;

        public const int ValorAnio1Ant_Total = 24;
        public const int ValorAnio1Ant_Total_Var = 25;

        //Variables de 2 años anteriores
        public const int ValorAnio2Ant_SemAct = 41;
        public const int ValorAnio2Ant_Acum = 42;

        public const int ValorAnio2Ant_Total = 43;
        public const int ValorAnio2Ant_Total_Var = 44;

        //Variables de 3 años anteriores
        public const int ValorAnio3Ant_SemAct = 61;
        public const int ValorAnio3Ant_Acum = 62;

        public const int ValorAnio3Ant_Total = 63;
        public const int ValorAnio3Ant_Total_Var = 64;

        //
        public const int ValorAA_SemAct = 100;
        public const int ValorSem_SemAct = 101;
    }

    public class CeldaCambiosPR5
    {
        public int Col { get; set; }
        public int Row { get; set; }
    }

    /// <summary>
    /// Objeto para almacenar las series de duración de carga
    /// </summary>
    public class SerieDuracionCarga
    {
        public string SerieName { get; set; }
        public List<decimal> ListaValores { get; set; }
        public List<decimal?> ListaVal { get; set; }
        public List<DateTime> ListaValores2 { get; set; }
        public string SerieColor { get; set; }

        public string SerieType { get; set; }

        public int SerieYaxis { get; set; }
        public decimal Valor { get; set; }
    }

    public class TipoDashboardIEOD
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
    }

    /// <summary>
    /// Clase Estado de combustible - Dashboard
    /// </summary>
    public class EstadoCombustibleIEOD
    {
        public int Estcomcodi { get; set; }
        public string Estcomnomb { get; set; }
        public string Estcomcolor { get; set; }
        public int Orden { get; set; }
    }

    /// <summary>
    /// Clase de Energia Primaria - Dashboard
    /// </summary>
    public class TipoEnergiaPrimariaIEOD
    {
        public string Energprimnomb { get; set; }
        public string Energprimcolor { get; set; }
        public int Tptomedicodi { get; set; }
        public int Orden { get; set; }
        public int TipoTotal { get; set; }
        public int ValidarCero { get; set; }
    }

}
