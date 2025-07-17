/*****************************************************************************************
* Fecha de Creación: 29-05-2014
* Creado por: COES SINAC
* Descripción: Clase que contiene las constantes de capa de persitencia
*****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace COES.Base.Core
{
    public class ConstantesBase
    {
        public const string MedioActualizArchivo = "F";
        #region Constantes DAOHelperBase

        public const string Aplicacion = "RutaSqlAplicacion";
        public const string ExtensionSqlXml = "Sql.xml";
        public const string Insertar = "USP_SAVE_";
        public const string Actualizar = "USP_UPDATE_";
        public const string Eliminar = "USP_DELETE_";
        public const string Listar = "USP_LIST_";
        public const string ObtenerPorId = "USP_GETBYID_";
        public const string ObtenerPorCriterio = "USP_GETBYCRITERIA_";
        public const string KeyInsertar = "Save";
        public const string KeyActualizar = "Update";
        public const string KeyEliminar = "Delete";
        public const string KeyAuditoria = "UpdateAuditoria";
        public const string KeyListar = "List";
        public const string KeyObtenerPorId = "GetById";
        public const string KeyAnio = "ListAnio";
        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        public const string KeyObtenerPorCodigo = "GetByCodigo";
        public const string KeyObtenerPorCriterio = "GetByCriteria";
        public const string KeyTotalRecords = "TotalRecords";
        public const string KeyGetMaxId = "GetMaxId";
        public const string MainNodeSql = "Sqls";
        public const string SubNodeSql = "Sql";
        public const string KeyNode = "key";
        public const string QueryNode = "query";
        public const string SI = "S";
        public const string NO = "N";
        public const string FormatoFecha = "yyyy-MM-dd";
        public const string FormatoFechaExtendido = "yyyy-MM-dd HH:mm:ss";
        public const string FormatoFechaExtendidoConMilisegundo = "yyyy-MM-dd HH:mm:ss.fff";
        public const string FormatoFechaEntero = "dd-MM-yyyy HH:mm:ss";
        public const string FormatoFechaHora = "yyyy-MM-dd HH:mm";
        public const string FormatFechaFull = "dd/MM/yyyy HH:mm";
        public const string FormatoFechaMes = "yyyy-MM";
        public const string FormatoMesAnio = "MM-yyyy";
        public const string FormatoFechaAn = "yyyy";
        public const string FormatoFechaSoloMes = "MM";
        public const string FormatoAnioMes = "yyyyMM";
        public const string FormatoFechaPE = "dd/MM/yyyy";
        public const string FormatoFechaSoloDia = "dd";
        #region Mejoras RDO
        public const string KeyInsertarHorario = "SaveHorario";
        public const string KeyObtenerRdoPorCriterio = "ObtenerEnvioCumplimiento";
        public const string KeyInsertarEjecutados = "SaveEjecutados";
        public const string KeyObtenerPorCriterioCaudalVolumen = "GetByCriteriaCaudalVolumen";
        public const string KeyInsertarIntranet = "SaveIntranet";
        public const string KeyInsertarIntervaloRDO = "SaveRDO";
        public const string KeyObtenerPorCriterioxHorario = "GetByCriteriaxHorario";
        #endregion

        #endregion

        #region Constantes Archivos

        public const string TipoFile = "F";
        public const string TipoFolder = "D";
        public const string FolderMain = "Inicio";
        public const char CaracterSlash = '/';
        public const string CaracterComa = ",";

        #endregion

        public const string FormatoFechaBase = "dd/MM/yyyy";
        public const string FormatoFechaFullBase = "dd/MM/yyyy HH:mm:ss";

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        /// <summary>
        /// Clave de la consulta de lista de periodos activos.
        /// </summary>
        public static string SqlListaPeriodoActivo = "ListaPeriodoActivo";

        //SP7
        public const int EmprcodiSistema = 0;

        #region siosein2
        public const string FormatoNombreMesAnio = "MMMM yyyy";
        public const string FormatoFechaMesAbrevAnio = "MMM-yyyy";
        public const string FormatoHoraMinuto = "HH:mm";
        #endregion

        #region Yupana

        public const string ConsideraEscenario = "1";
        public const string NoConsideraEscenario = "0";
        public const short Generales = -1;
        public const int ColumnaGmasSize = 20;
        public const int NodoTopologico = 7;
        public const int Linea = 8;
        public const int Trafo2D = 9;
        public const int Trafo3D = 10;

        public const short ModoT = 3;
        public const short PlantaH = 4;
        public const short PlantaT = 5;
        public const short UnidadH = 6;
        public const short UnidadT = 2;
        public const short Urs = 11;
        public const short Rsf = 12;
        public const short Combustible = 13;
        public const short RestricGener = 14;
        public const short GenerMeta = 15;
        public const short SumFlujo = 16;
        public const short DispComb = 17;
        public const short Embalse = 19;
        public const short EstacionHid = 42;
        public const short GrupoPrioridad = 22;
        public const short Prioridad = 23;
        public const short CicloCombinado = 24;
        public const short PlantaNoConvenO = 25;
        public const short RegionSeguridad = 26;
        public const short Caldero = 27;

        public const int GrupoHidro = 2;
        public const int CentralHidro = 4;
        public const int OrigLectDemanda = 18;
        public const int OriglectRer = 15;
        public const int LectHidroProgDiarioCp = 67;
        public const int LectHidroProgSemanalCp = 68;
        public const int TipoInfoDolares = 21;
        public const int TipoInfoMW = 1;
        public const int TipoInfoMWFlujos = 8;
        public const int TipoInfoMWDemanda = 20;
        public const short ReduccionOrden = 5;
        public const short TensionOrden = 1;

        public const short TerLinOrigen = 1;
        public const short TerLinDestino = 2;
        public const short TerNodoT = 3;

        public const short LectHidroCpDiarioFinal = 225;
        public const short LectHidroCpReprogramaDiario = 226;
        public const short LectHidroCpSemanalFinal = 227;

        // Cruce con Despacho #
        public const int IdOrigLectDespacho = 2;
        public const int IdOrigLectFlujos = 17;
        public const int IdOrigLectDemandaPronos = 18;
        public const int IdlectDespachoDiario = 4;//233
        public const int IdlectDespachoSem = 3;//234
        public const int IdlectDespachoReprog = 5;//235
        public const int IdReporteDespacho = 10;

        public const int IdLectProgDiarioCosto = 22;
        public const int IdLectReProgCosto = 21;
        public const int IdLectProgSemanalCosto = 23;

        public const int IdPtoFenixTv = 250;
        public const int IdPtoFenixTg1 = 252;
        public const int IdPtoFenixTg2 = 249;
        public const int IdFenixCCTg1 = 348;
        public const int IdFenixCCTg2 = 345;
        public const int IdFenixCCTg12 = 349;

        public const int IdPtoKallpaTv = 230;
        public const int IdPtoKallpaTg1 = 197;
        public const int IdPtoKallpaTg2 = 203;
        public const int IdPtoKallpaTg3 = 204;
        public const int IdKallpaCCTg1 = 320;
        public const int IdKallpaCCTg2 = 321;
        public const int IdKallpaCCTg3 = 322;
        public const int IdKallpaCCTg12 = 323;
        public const int IdKallpaCCTg23 = 324;
        public const int IdKallpaCCTg31 = 325;
        public const int IdKallpaCCTg123 = 326;

        public const int IdPtoVentanillaTv = 193;
        public const int IdPtoVentanillaTg3 = 113;
        public const int IdPtoVentanillaTg4 = 114;
        public const int IdVentanillaCCTg3 = 286;
        public const int IdVentanillaCCTg3FD = 289;
        public const int IdVentanillaCCTg4 = 287;
        public const int IdVentanillaCCTg4FD = 290;
        public const int IdVentanillaCCTg34 = 288;
        public const int IdVentanillaCCTg34FD = 291;

        public const int IdPtoChilca1Tv = 236;
        public const int IdPtoChilca1Tg1 = 194;
        public const int IdPtoChilca1Tg2 = 196;
        public const int IdPtoChilca1Tg3 = 207;
        public const int IdChilca1CCTg1 = 327;
        public const int IdChilca1CCTg2 = 328;
        public const int IdChilca1CCTg3 = 329;
        public const int IdChilca1CCTg12 = 330;
        public const int IdChilca1CCTg23 = 331;
        public const int IdChilca1CCTg31 = 332;
        public const int IdChilca1CCTg123 = 333;

        public const int IdPtoChilca2Tv = 285;
        public const int IdPtoChilca2Tg4 = 795;
        public const int IdChilca2CCTg4 = 919;

        public const int IdOllerosCC = 318;
        public const int IdPtoOllerosTv = 2159;
        public const int IdPtoOllerosTg1 = 248;

        public const int IdLasFloresCC = 3375;
        public const int IdPtoLasFloresTv = 56677;
        public const int IdPtoLasFloresTg1 = 209;

        public const int TipoCambio = 1;

        public const string ReporteExceldespacho = "Despacho";
        public const string ReporteExcelRSF = "ReporteRSF";
        public const string ReporteExcelSalida = "Salida_";
        public const string ExtensionXls = ".xlsx";

        //Cruce Ptomedicion Total Sein con Subrestricciones
        public const int PtoGeneracionSEIN = 1222;
        public const int PtoDemandaSEIN = 1221;
        public const int PtoPerdidasSEIN = 1223;
        public const int PtoDeficitSEIN = 1224;

        //Restricciones
        public const short ResResopPt = 11;
        //Reporte de salida
        public const short ResVariablesSalida = 26;
        public const short ResCostoMargSalida = 27;
        public const int RestricCostosOperacion = 28;
        public const int RestricTotalSein = 33;
        public const int RestricProvBase = 34;

        //Subrestricciones
        public const short SRES_MANTENIMIENTO_PH = 1;
        public const short SRES_APORTES_PH = 2;
        public const short SRES_RESERVPRIM_PH = 3;
        public const short SRES_NMAX_PH = 4;

        public const short SRES_MANTENIMIENTO_EMB = 5;
        public const short SRES_APORTES_EMB = 6;
        public const short SresVolMinEmb = 7;
        public const short SresVolMaxEmb = 8;
        public const short SresDefMinEmb = 10;
        public const short SresDefMaxEmb = 11;
        public const short SresCauRiegoEmb = 12;
        public const short SRES_VOLMETA_EMB = 13;
        public const short SRES_MANTO_PT = 14;
        public const short SRES_RESOP_PT = 15;

        public const short SRES_CONDINI_PT = 19;
        public const short SresCondiniCaldero = 123;
        public const short SRES_NMAX_PT = 20;
        public const short SRES_RPRIM_PT = 21;
        public const short SRES_UNFOR_PT = 22;
        public const short SresEfecTempPt = 23;
        public const short SRES_TMINCE_PT = 24;
        public const short SRES_DEMBARRA_NT = 25;
        public const short SRES_GENER_RER = 26;
        public const short SRES_GENER_OTROS = 27;
        public const short SRES_MANTENIMIENTO_LT = 28;
        public const short SRES_SISTRESVSECUP = 29;
        public const short SRES_SISTRESVSECDN = 30;
        public const short SresSisRestricGener = 31;
        public const short SresGenMeta = 32;
        public const short SresSumFlujoLimInf = 33;
        public const short SresSumFlujoLimSup = 34;
        public const short SresDispComb = 35;
        public const short SresRprimPrer = 36;
        public const short SresSistResvSecPlantTramo1Up = 37;
        public const short SresSistResvSecPlantTramo1Dn = 38;
        public const short SresSistResvSecPlantMinUp = 39;
        public const short SresSistResvSecPlantMinDn = 40;
        public const short SresSistResvSecUrsPrecioTramo1Up = 41;
        public const short SresSistResvSecUrsPrecioTramo1Dn = 42;
        public const short SresSistResvSecUrsMinUp = 43;
        public const short SresSistResvSecUrsMinDn = 44;
        public const short SresSistResvSecUrsTramo1Up = 45;
        public const short SresSistResvSecUrsTramo1Dn = 46;

        public const short SresSisRSFUp = 47;
        public const short SresSisRSFDn = 48;
        public const short SresPHRestricOp = 49;
        public const short SresSistResvSecPlantTramo2Up = 50;
        public const short SresSistResvSecPlantTramo2Dn = 51;
        public const short SresSistResvSecUrsPrecioTramo2Up = 52;
        public const short SresSistResvSecUrsPrecioTramo2Dn = 53;
        public const short SresSistResvSecUrsTramo2Up = 54;
        public const short SresSistResvSecUrsTramo2Dn = 55;
        public const short SresPotenciaRacionamiento = 56;
        public const short SresPerdidasLineas = 59;
        public const short SresPotTermica = 62;
        public const short SresPotHidro = 63;
        public const short SresPTCCOMB = 86;
        public const short SresPTCVNC = 87;
        public const short SresFlujos = 60;
        public const short SresReservaUrs = 69;
        public const short SresCostoMarginalBarra = 73;
        public const short Sres_nmaxp_ph = 88;
        public const short Sres_nmaxp_pt = 89;

        public const short SresGeneracionSEIN = 90;
        public const short SresDemandaSEIN = 91;
        public const short SresPerdidasSEIN = 92;
        public const short SresDeficitSEIN = 93;
        public const short SresCostoTotal = 96;
        public const short SresProvisionBase = 95;
        public const short SresProvisionBaseUp = 95;
        public const short SresProvisionBaseDn = 104;
        public const short SresSistRegSeguridad = 105;
        public const short SresCostoRacionamiento = 81;
        public const short SresCostoOperacion = 94;
        public const short SresTipoReserva = 118;
        public const short SresIndRsfPhUp = 114;
        public const short SresIndRsfPhDown = 115;
        public const short SresIndRsfPtUp = 116;
        public const short SresIndRsfPtDown = 117;
        public const short SresPotUncr = 124;

        public const short SresVolEmb = 64;

        /// GAMS
        public const string ArchivoGdxFinal = "SINAC_2.gdx";
        public const string ArchivoGdxParcial = "bchout_i.gdx";
        public const string IdArchGdxFinal = "1";
        public const string IdArchGdxParcial = "0";
        public const string IdFuenteSicoes = "2";
        public const string LevelGams = "1";
        public const string MarginalGams = "0";
        public const short NroDecimalGams = 7;
        public const string CadenaFinalGams = "_gams_net_gjo0.gms Stop";
        public const string Phantom = "";
        public const string CaracterInf = "inf";
        public const string CaractNulo = "";
        public const string NombArchivoGSM = "_gams_net_gjo0.gms";
        public const string NombArchivoPeriodo = "Periodos.dat";
        public const string NombArchivoPlantaTGams = "Parametros_de_Termicas.dat";
        public const string NombArchivoCondIniciales = "Condiciones_Iniciales.dat";
        public const string NombArchivoNodoTGams = "Tensiones_de_Barra.dat";
        public const string NombArchivoDemanda = "Demanda.dat";
        public const string NombArchivoForzadaUt = "Unidades_Forzadas.dat";
        public const string NombArchivoForzadaUt2 = "Unidades_Forzadas2.dat";
        public const string NombArchivoTemper = "Restriccion_de_Temperatura.dat";
        public const string NombArchivoCaudales = "Caudales.dat";
        public const string NombArchivoCaudalesCc = "CaudalesCc.dat";
        public const string NombArchivoCaudalesSc = "CaudalesSc.dat";
        public const string NombArchivoCaudalesCcSc = "CaudalesCcSc.dat";
        public const string NombArchivoMantenimiento = "Mantenimientos.dat";
        public const string NombArchivoReserva = "RPF_Sistema.dat";
        public const string NombArchivoPlantaHGams = "Parametros_de_Hidro.dat";
        public const string NombArchivoEmbalseGams = "Parametros_de_Embalses.dat";
        public const string NombArchivoLineaGams = "Parametros_de_Lineas.dat";
        public const string NombArchivoArranquesGams = "Numero_de_Arranques.dat";
        public const string NombArchivoPlantaNoConvOGams = "Generacion_No_Convencional.dat";
        public const string NombArchivoPlantaNoConvOGams2 = "Generacion_No_Convencional2.dat";
        public const string NombArchivoResTerUpGams = "ResTerUp.dat";
        public const string NombArchivoResTerDnGams = "ResTerDn.dat";
        public const string NombArchivoResHidUpGams = "ResHidUp.dat";
        public const string NombArchivoResHidDnGams = "ResHidDn.dat";
        public const string NombArchivoRSFGams = "RSF_Total_del_Sistema.dat";
        public const string NombArchivoresURSGams = "RSF_Potencia_Ofertada.dat";
        public const string NombArchivoresResrangoGams = "RSF_Bandas_Calificadas.dat";
        public const string NombArchivoresCOSTURSGams = "RSF_Precio_de_Ofertas.dat";
        public const string NombArchivoresProvisionBaseGams = "RSF_Provision_Base.dat";
        public const string NombArchivomantoLineasGams = "manLineas.dat";
        public const string NombArchivoVolMetaGams = "Volumenes_Meta.dat";
        public const string NombArchivoVolumenGams = "Restricciones_de_Embalse.dat";
        public const string NombArchivoResGener = "Restriccion_de_Generacion.dat";
        public const string NombArchivoSumaFlujos = "Restriccion_de_Suma_de_Flujos.dat";
        public const string NombArchivoGenerMeta = "Generacion_Meta.dat";
        public const string NombArchivoDispComb = "Disponibilidad_de_Combustible.dat";
        public const string NombArchivoFCostoF = "Funcion_de_Costo_Futuro.dat";
        public const string NombArchivoConstantes = "Constantes.dat";
        public const string NombArchivoTolerancia = "Tolerancia.dat";
        public const string NombArchivoOptSolver = "OptSolver.dat";
        public const string NombArchivoEcuacion = "ecuacion.dat";
        public const string NombArchivoParadasGams = "Numero_de_Paradas.dat";
        public const string NombreArchivoRegionSeguridad = "Coordenadas_de_Seguridad.dat";
        public const string NombreArchivoTiempoViaje = "Tiempos_de_Viaje.dat";
        public const string NombreArchivoRsfTipo = "RSF_Tipo.dat";
        public const string NombArchivoConfiguraciones = "Configuraciones.dat";
        public const string NombArchivoNodoTopDescrip = "NodoTopologico.txt";
        public const string NombArchivoEmbalseDescrip = "Embalse.txt";
        public const string NombArchivoPlantaRerDescrip = "PlantaRer.txt";
        public const string NombArchivoPlantaOtrosDescrip = "PlantaOtros.txt";
        public const string NombArchivoPlantaTermicaDescrip = "PlantaTermica.txt";
        public const string NombArchivoPlantaHidroDescrip = "PlantaHidro.txt";
        public const string NombArchivoLineaDescrip = "Linea.txt";
        public const string NombArchivoPlantaUrsDescrip = "PlantaUrs.txt";
        public const string NombArchivoLogGams = "mylog.txt";
        public const string NombArchivoLogGamsIteraciones = "log.txt";
        public const string NombArchivoLogGamsMetodo = "Metodo_de_Ejecucion.dat";
        public const string NombArchivoFolderTrabajoGams = "foldertrabajo.dat";
        public const string FolderResultado = "RESULTADOS";
        public const string FolderOtros = "Otros";
        //INC
        public const string NombArchivoSets = "Sets.inc";
        public const string NombArchivoCentrales = "Centrales.inc";
        public const string NombArchivoAdmitancia = "Admitancia.inc";
        public const string NombArchivoAnexo = "Anexos.inc";
        public const string NombArchivoEncriptado = "COES_MODEL.g00";
        public const string NombArchivoInputData = "Input_Data.gms";
        //Hidrologia
        public const int Turbinamiento = 1;
        public const int Vertimiento = 2;
        //Terminal
        public const short Terghidro = 5;
        public const short Tergtermico = 4;
        public const short Terplantarer = 15;
        // Propiedad General
        public const int TipoCambioDolar = 98;
        public const string SufijoNulo = "(*)";
        //Propiedad recursos
        public const short PerdidasTransOrd = 7;
        public const short PerdidasMaxOrd = 4;
        public const int MinimoPt = 61;
        public const int MaximoPt = 64;
        public const int MinimoPh = 84;
        public const int MaximoPh = 85;

        // Restricciones de Generacion
        public const int MenorRestric = 0;
        public const int IgualRestric = 1;
        public const int MayorRestric = 2;
        //Ciclo Combinado
        public const int ModoOperInf = 3;
        public const int ModoOperSup = 4;
        public const int Transicion = 3;
        //Parametros
        public const int PotenciaBase = 8;
        public const int CostoRacionamiento = 2;
        public const int PenalidadesExceso = 3;
        public const int CostoDeficitResrSecUp = 13;
        public const int CostoDeficitResrSecDn = 14;
        public const int CostoExcesoResrSecUp = 15;
        public const int CostoExcesoResrSecDn = 16;
        public const int CostoDeficitRegSeg = 17;
        public const int CostoExcesoRegSeg = 18;
        public const int ConstCaudalVol = 19;
        public const int ConsideraTViaje = 20;
        public const int TolBenders = 21;
        public const int TolCplex = 22;
        public const int TolIPOPTsp = 23;
        public const int TolSNOPTsp = 24;
        public const int TolIPOPTfo = 25;
        public const int TolSNOPTfo = 26;
        public const int MaxItPrRel = 27;
        public const int MaxItBend = 28;
        public const int NivelCorte = 29;
        public const int ToleranciaRelativa = 7;
        public const int OptSolver = 10;
        public const int NumIteraciones = 6;
        public const int UmbMinRPF = 10;
        public const int NumProcesadores = 9;
        public const int FuenteGams = 4;
        public const int IdPorcRPF = 282;
        public const int IdMagnitudRSF = 24;
        public const int IdMagnitudRSFDn = 645;
        public const int IdTipoProceso = 12;
        public const int IdRed = 11;
        public const string Parallelmode = "parallelmode";

        // Urs
        public const short TiempoMinimo = 89;
        public const short OrdenTiempoMinimo = 1;
        public const short OrdenProvisionBaseUp = 1;
        public const short OrdenPotTramo1Up = 2;
        public const short OrdenPotTramo1Dn = 3;
        public const short OrdenPrecioTramo1Up = 4;
        public const short OrdenProvisionBaseDn = 6;
        public const short OrdenPrecioTramo1Dn = 5;
        public const decimal PotMinUrs = 6;

        // Propiedad Embalse
        public const int OrdenVolMin = 6;
        public const int OrdenVolMax = 7;
        public const int OrdenFuncComb = 12;

        //Codigo Modo Operacion
        public const short CostoCombustible = 58;
        public const short CostoVar = 49;
        public const short CostoCombustibleOrden = 8;
        public const short CostoVarOrden = 7;
        public const short IdGenMIn = 13;
        public const short IdFuncComb = 63;
        public const short TipoPlantaOrden = 11;
        public const short CombustibleOrden = 8;
        public const short TMinOperOrden = 13;
        public const short TMinTSOrden = 14;
        //Yupana Iteracion 3
        public const short FuenteOrden = 17;
        public const string NombArchivoPerdidasModeloUninodal = "Perdidas_Uninodal.dat";
        public const string NombreArchivoUnidadesIndisponibles = "RSF_Unidades_Indisponibles.dat";
        public const short SresPerdModUninodal = 122;
        public const string SqlSoloPropManual = " inner join cp_propiedad prop on prop.propcodi = a.propcodi and (prop.propcodisicoes = 0 or ( prop.propcodisicoes >= 0 and r.recurlogico =1)) ";
        public const int ConsideraEficiencia = 30;
        public const short SresPTCCOMBEFIC = 120;
        public const short SresPTCVNCEFIC = 121;
        //Fin Yupana Iteracion 3

        // Yupana 2022
        public const string NombArchivoTiempoTransicion = "Tiempos_de_Transicion.dat";
        public const string NombArchivoTiempoCaldero = "Tiempos_de_Caldero.dat";

        //Fin Yupana 2022


        //Escenario
        public const short EscDiario = 1;
        public const short EscSemanal = 2;
        public const short EscReprogramado = 3;


        #endregion

        #region Incumplimiento
        public const string KeyObtenerPorIncumplimiento = "GetByIncumplimiento";
        #endregion

        #region Mejoras EO-EPO
        public const string KeyListVigenciaAnioIngreso = "ListVigenciaAnioIngreso";
        public const string KeyObtenerPorCriterioEstadosVigencia = "GetByCriteriaEstadosVigencia";
        public const string KeyListVigencia36Meses = "ListVigencia36Meses";
        public const string KeyListVigencia48Meses = "ListVigencia48Meses";
        public const string KeyListVigencia12Meses = "ListVigencia12Meses";
        public const string KeyNroRegistros = "NroRegistros";
        #endregion
        #region Mejoras EO-EPO-II
        public const string KeyListExcesoAbsObs = "ListExcesoAbsObs";
        #endregion
        #region Mejoras CTAF
        public const string KeyUpdateEventoCtaf = "UpdateEvenctaf";
        public const string KeyinsertarEventoEvento = "InsertarEventoEvento";
        public const string KeyListInformesSco = "SqlListInformesSco";
        public const string KeyActualizarInformePortalWeb = "SqlActualizarInformePortalWeb";
        public const string KeyObtenerInformeSco = "SqlObtenerInformeSco";
        public const string KeyObtenerPorEmprcodi = "GetByIdxEmprcodi";
        #endregion
        #region Mejoras RDO-II
        public const string KeyObtenerPorCriterioxUltimoEjecutado = "GetByCriteriaMeEnviosUltimoEjecutado";
        #endregion
    }
}
