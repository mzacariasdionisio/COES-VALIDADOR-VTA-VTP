using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.PronosticoDemanda.Helper
{
    public class ConstantesProdem
    {
        //Constantes varias
        public const string RegSi = "S";
        public const string RegNo = "N";
        public const string RegActivo = "A";
        public const string RegInactivo = "I";
        public const int RegTodos = 0;
        public const string RegStrTodos = "0";
        public const int SiProdem = 1;
        public const int NoProdem = 0;
        public const int LimiteBulkInsert = 50000;
        public const string UsuariosValidos = "'allosa','walmeyda','luis.reategui','assetec'";
        //----------------------------------------------------------------------

        //Formatos y Rutas
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoHoraMinuto = "HH:mm";
        public const string ReporteDirectorio = "ReporteTransferencia";
        public const string ReportePronostico = "ReportePronostico";
        public const string ReportePronosticoBarra = "ReportePronosticoBarra";
        public const string ReportePerfilPatron = "ReportePerfilPatron";
        //Prodem3
        public const string ReporteEstimador = "ReporteEstimador";
        public const string ReporteConsultaAporte = "ReporteConsultaAporte";
        public const string ReporteAsociacionBarra = "ReporteAsociacionBarra";
        public const string ReporteDesviacion = "ReporteDesviacion";
        public const string PrefijoEnvio = "_E";
        public const string PrefijoRecepcion = "_R";
        public const string RutaArchivosSCO = "PathReprocesoCostosMarginalesTNA";
        public const string FormatoFechaArchivoRaw = "yyyyMMdd";
        public const string FormatoFechaMedicionRaw = "dd/MM/yyyy HH:mm";        
        //----------------------------------------------------------------------

        //Identificador de los dias de la semana
        public const int DiaAsIntDomingo = 0;
        public const int DiaAsIntLunes = 1;
        public const int DiaAsIntMartes = 2;
        public const int DiaAsIntMiercoles = 3;
        public const int DiaAsIntJueves = 4;
        public const int DiaAsIntViernes = 5;
        public const int DiaAsIntSabado = 6;       
        //----------------------------------------------------------------------

        //Estilo de los mensajes
        public const string MsgInfo = "info";
        public const string MsgSuccess = "success";
        public const string MsgWarning = "warning";
        public const string MsgError = "error";
        //----------------------------------------------------------------------

        //Identificadores de los módulos (M=Modulo, SM=Submodulo)
        public const int SMParametosByPunto = 1;
        public const int SMParametosByAgrupacion = 2;
        public const int SMParametosByAreas = 3;
        public const int SMParametosByBarras = 4;

        //20191205
        public const int SMDepuracionByPunto = 1;
        public const int SMDepuracionByAgrupacion = 2;

        //----------------------------------------------------------------------

        //Intervalos por tipo de patrón
        public const int Itv15min = 96;
        public const int Itv30min = 48;
        public const int Itv60min = 24;
        //----------------------------------------------------------------------

        //Número de registros por página
        public const int LimiteRegxPag = 20;
        //----------------------------------------------------------------------

        //Constantes defecto de configuración
        public const string DefectoDate = "31/12/9999";
        public const int DefectoByPunto = 22198;
        public const int DefectoByAgrupacion = 21355;
        public const int DefectoByArea = 3004;
        public const int DefectoByBarra = -1;
        //----------------------------------------------------------------------

        //Tipo de registro de los parámetros de configuración
        public const int TipoRegParametrosNormal = 1;
        public const int TipoRegParametrosDefecto = 2;
        //----------------------------------------------------------------------

        //Identificador para Listar los motivos de Pronostico
        public const int VarExoPronostico = 401;
        //----------------------------------------------------------------------

        //Identificador del proceso que solicita el perfil patrón
        public const int ProcPatronDemandaEjecutada = 1;
        public const int ProcPatronDemandaPrevista = 2;
        public const int ProcPatronDemandaPrevSemanal = 3;
        public const int ProcPatronDemandaEjecutadaAgrupada = 4;
        public const int ProcPatronDemandaPrevistaAgrupada = 5;
        public const int ProcPatronDemandaArea = 6;
        public const int ProcPatronDemandaVegetativa = 7;
        public const int ProcPatronPerfilExtranet = 8;
        public const int ProcPatronDemandaPorBarra = 9;
        //----------------------------------------------------------------------

        //Identificador ORIGLECTCODI de la tabla ME_ORIGENLECTURA
        public const int OriglectcodiPR03 = 6;//DEMANDA EN BARRA
        public const int OriglectcodiAgrupacion = 33;//AGRUPACIONES DE DEMANDA
        public const int OriglectcodiUnidadesTNA = 36;//Temporal
        public const int OriglectcodiTnaSco = 36;
        public const int OriglectcodiTnaIeod = 37;
        public const int OriglectcodiTnaDpo = 38;
        public const int Activa = 1;
        public const int Reactiva = 2;
        //----------------------------------------------------------------------

        //Identificador PRNM48TIPO de la tabla PRN_MEDICION48 (Nueva versión a partir de 200)
        public const int PrntDemandaEjecutadaAjusteAuto = 201;
        public const int PrntDemandaEjecutadaAjusteManual = 202;
        public const int PrntDemandaPrevistaAjusteManual = 203;
        public const int PrntDemandaAreaAjuste = 204;
        public const int PrntDemandaVegetativaAjuste = 205;
        public const int PrntProdemAreaAjuste = 206;

        public const int PrntPatronDefLunes = 207;
        public const int PrntPatronDefMaMiJV = 208;
        public const int PrntPatronDefSabado = 209;
        public const int PrntPatronDefDomingo = 210;

        public const int PrntParametroBarra = 212;

        //--Mantenimiento y Falla
        //------------------------EJECUTADO
        public const int PrntMantNorte = 213;//MANTENIMIENTO DEL NORTE EJECUTADO
        public const int PrntMantSur = 214;//MANTENIMIENTO DEL SUR EJECUTADO
        public const int PrntMantSierraCentro = 215;//MANTENIMIENTO DE SIERRA/CENTRO EJECUTADO
        public const int PrntMantCentro = 216;//MANTENIMIENTO DEL CENTRO EJECUTADO
        public const int PrntFallaNorte = 217;//FALLA DEL NORTE EJECUTADO
        public const int PrntFallaSur = 218;//FALLA DEL SUR EJECUTADO
        public const int PrntFallaSierraCentro = 219;//FALLA DE SIERRA/CENTRO EJECUTADO
        public const int PrntFallaCentro = 220;//FALLA DEL CENTRO EJECUTADO

        //------------------------PREVISTO            
        public const int PrntMantNortePrevisto = 221; //MANTENIMIENTO DEL NORTE PREVISTO
        public const int PrntMantSurPrevisto = 222; //MANTENIMIENTO DEL SUR PREVISTO
        public const int PrntMantSierraCentroPrevisto = 223; //MANTENIMIENTO DEL SIERROCENTRO PREVISTO
        public const int PrntMantCentroPrevisto = 224; //MANTENIMIENTO DEL CENTRO PREVISTO
        public const int PrntFallaNortePrevisto = 225; //FALLA NORTE PREVISTO
        public const int PrntFallaSurPrevisto = 226; //FALLA SUR PREVISTO
        public const int PrntFallaSierracentroPrevisto = 227; //FALLA SIERRACENTRO PREVISTO
        public const int PrntFallaCentroPrevisto = 228; //FALLA CENTRO PREVISTO

        //--Solo identificadores (no son para consultas SQL)
        public const int PrntFlujoLinea = 229;
        public const int PrntDemandaULibre = 230;

        //Prnm48tipo utilizados en el cálculo del pronóstico de la demanda (Inputs del proceso)
        public const int PrntProdemArea = 231;
        public const int PrntProdemVegetativa = 234;
        public const int PrntProdemIndustrial = 235;
        //----------------------------------------------------------------------

        public const int PrntDemandaBarraAjuste = 236;
        public const int PrntDemandaPrevistaAjusteAuto = 237;
        public const int PrntDemandaSemanalAjusteAuto = 238;
        public const int PrntDemandaSemanalAjusteManual = 239;
        //----------------------------------------------------------------------

        //Identificador PRNMGRTIPO  de la tabla PRN_MEDICIONGRP
        public const int PrnmgrtProdemBarraSinPadre = 0;
        public const int PrnmgrtProdemBarra = 1;
        public const int PrnmgrtProdemBarraAjuste = 2;
        public const int PrnmgrtProdemAgrupacionAjuste = 9;
        public const int PrnmgrtProdemAreaAjuste = 10;
        //0805 Agregado para registrarlos servicios auxiliares en la tabla prn_mediciongrp
        public const int PrnmgrtServicioAuxiliar = 3;

        //Agregado 20200730 - para la demanda vegetativa e industrial por barra
        public const int PrnmgrtDemVegetativa = 4;
        public const int PrnmgrtDemIndustrial = 5;

        //Agregado 20220201 - para la demanda vegetavia e industrial por barra desde la demanda por áreas
        public const int PrnmgrtDemVegDesdeArea = 6;
        public const int PrnmgrtDemIndDesdeArea = 7;
        public const int PrnmgrtProdemDesdeArea = 8;

        //----------------------------------------------------------------------

        //Identificador LECTCODI  de la tabla ME_LECTURA
        public const int LectcodiDemPrevDiario = 110;
        public const int LectcodiDemEjecDiario = 103;
        public const int LectcodiDemPrevSemanal = 102;
        //----------------------------------------------------------------------

        //Identificador TIPOINFOCODI de la tabla SI_TIPOINFORMACION
        public const int TipoinfocodiMWDemanda = 1;
        public const int TipoinfocodiCentigrados = 50;
        public const int TipoinfocodiPorcentaje = 39;
        //----------------------------------------------------------------------

        //Identificador TIPOEMPRCODI de la tabla SI_TIPOEMPRESA
        public const int TipoemprcodiDistribuidores = 2;//Distribuidores
        public const int TipoemprcodiUsuLibres = 4;//Usuarios Libres
        //----------------------------------------------------------------------

        //Identificador PTOMEDICODI de la tabla ME_PTOMEDICION
        public const int PtomedicodiASein = 1210;//assetec 45133; coes 1210---- ASSETEC 20181122
        public const int PtomedicodiASur = 3006;
        public const int PtomedicodiANorte = 3004;
        public const int PtomedicodiACentro = 3005;
        public const int PtomedicodiASierraCentro = 3009;
        //----------------------------------------------------------------------

        //Identificador AREACODI de la tabla EQ_AREA
        public const int AreacodiANorte = 2;
        public const int AreacodiASur = 5;
        public const int AreacodiACentro = 3;
        public const int AreacodiASierraCentro = 6;
        public const int AreacodiASein = -1;
        //----------------------------------------------------------------------

        //Identificador "NOMBRE" de las áreas operativas
        public const string AOperativaSur = "SUR";
        public const string AOperativaNorte = "NORTE";
        public const string AOperativaCentro = "CENTRO";
        public const string AOperativaSCentro = "ELECTROANDES";
        public const string AOperativaSierraCentro = "SIERRACENTRO";
        //----------------------------------------------------------------------

        //Niveles de referencia para la estructura de áreas
        public const int NvlSubestacion = 0;
        public const int NvlAreaOperativa = 1;
        //----------------------------------------------------------------------

        //Identificador VAREXOCODI de la tabla PRN_VARIBLEEXOGENA
        public const int VarexocodiTemperatura = 1;
        public const int VarexocodiSentermica = 2;
        public const int VarexocodiNubosidad = 3;
        public const int VarexocodiHumedad = 4;

        //Tipo de procedimiento para calcular el perfil patrón
        public const string PatronByMediana = "M";
        public const string PatronByPromedio = "P";
        public const string PatronByRegresionLineal = "R";
        //----------------------------------------------------------------------

        //Criterios para clasificar la demanda reportada (PRN_CLASIFICACION)
        public const int PrnclsclasiMuyalta = 1;
        public const int PrnclsclasiAlta = 2;
        public const int PrnclsclasiMedia = 3;
        public const int PrnclsclasiBaja = 4;
        public const int PrnclsclasiDepurado = 5;
        //----------------------------------------------------------------------

        //Criterios para catalogar la demanda reportada
        public const int PrnPerfilNormal = 1;
        public const int PrnPerfilBajaCarga = 2;
        public const int PrnPerfilSBPuntual = 3;
        public const int PrnPerfilCongelado = 4;
        //----------------------------------------------------------------------

        //Parametros Barras Grupo
        public const int Prcatecodi = 10;
        //----------------------------------------------------------------------

        //Constantes del módulo de despacho ejecutado
        public const int PrnmtipoDesEjecEquipo = 20;//DESPACHO EJECUTADO DE UNA CENTRAL
        public const int PrnmtipoDesEjecNorte = 21;//DESPACHO EJECUTADO DEL NORTE
        public const int PrnmtipoDesEjecSur = 22;//DESPACHO EJECUTADO DEL SUR
        public const int PrnmtipoDesEjecSierraCentro = 23;//DESPACHO EJECUTADO DE SIERRA/CENTRO
        public const int PrnmtipoDesEjecCentro = 24;//DESPACHO EJECUTADO DEL CENTRO
        public const int PrnmtipoDesEjecArea = 99;//DESPACHO EJECUTADO DEL AREA
        //----------------------------------------------------------------------

        //Extranet - Causa de justificación en EVE_CAUSAEVENTO
        public static int IdCausaJustificacion = 401;
        public static int IdSubCausaJustificacion = 40100;
        public static int IdJustificacionMantenimiento = 40101;
        public static int IdJustificacionFalla = 40102;
        //----------------------------------------------------------------------

        //Extranet - Códigos antiguos para de las demandas
        public const int AntiguoLectcodiEjecutado = 45;
        public const int AntiguoLectcodiPrevisto = 46;
        public const int AntiguoLectcodiSemanal = 47;
        public const int AntiguoTipoinfocodi = 20;
        //----------------------------------------------------------------------

        //Extranet - Formatos de envio
        public const int FormatcodiDemandaDiaria = 95; //ASSETEC 20181122 - 92; antes del pase a test 
        public const int FormatcodiDemandaSemanalDistrib = 71;
        public const int FormatcodiDemandaMensualDistrib = 96; //ASSETEC 20181122 - 4; antes del pase a test
        //----------------------------------------------------------------------

        //Extranet Identificador HOJACODI correspondiente a un tipo de lectura
        public const int HojacodiDemEjecDiario = 30; //ASSETEC 20181122 - 28; antes del pase a test 
        public const int HojacodiDemPrevDiario = 31; //ASSETEC 20181122 - 29; antes del pase a test
        public const int HojacodiDemPrevSemanal = 26;
        public const int HojacodiDemEjecMensual = 24;
        //----------------------------------------------------------------------

        //RelacionBarras
        public const int BarraNoDefinido = -1;

        //ReduccionRed
        public const string ReduccionRedAgrupacion = "R";
        public const string ReduccionRedDefecto = "D";

        //Tipo de unidad para los archivos RAW (PRN_ESTIMADORRAW)
        public const int EtmrawtpLinea = 1;
        public const int EtmrawtpBarra = 2;
        public const int EtmrawtpShunt = 3;
        public const int EtmrawtpGenerador = 4;
        public const int EtmrawtpCarga = 5;
        public const int EtmrawtpTrafo = 6;

        //Tipo de unidad(String) para los archivos RAW (PRN_ESTIMADORRAW)
        public const string EtmrawtpStrLinea = "Linea";
        public const string EtmrawtpStrBarra = "Barra";
        public const string EtmrawtpStrShunt = "Shunt";
        public const string EtmrawtpStrGenerador = "Generador";
        public const string EtmrawtpStrCarga = "Carga";
        public const string EtmrawtpStrTrafo = "Transformador";

        //Tipo de fuente para la lectura de archivos RAW (PRN_ESTIMADORRAW)
        public const int EtmrawfntIeod = 1;//Manual
        public const int EtmrawfntSco = 2;//Auto

        //Tipo de variable para las unidades de los archivos RAW
        public const int GeneradorPotActivaMW = 1;
        public const int GeneradorPotReactivaMVAR = 2;

        public const int LineaPotActivaMW = 3;
        public const int LineaPotReactivaMVAR = 4;
        public const int LineaPotAparenteMVA = 5;
        public const int LineaPerdidasMW = 6;
        public const int LineaCargaMaxima = 7;
        
        public const int TransPotActivaMW = 8;
        public const int TransPotReactivaMVAR = 9;
        public const int TransPotAparenteMVA = 10;
        public const int TransCargaMaxima = 11;
        public const int TransPerdidasMW = 12;

        public const int BarraTensionKv = 13;
        public const int BarraAngulo = 14;
        public const int BarraDemActivaMW = 15;
        public const int BarraDemReactivaMVAR = 16;

        public const int CargaPotActivaMW = 17;
        public const int CargaPotReactivaMVAR = 18;

        public const int ShuntPotReactivaMVAR = 19;

        //Estado
        public const int GeneradorEstado = 20;
        public const int LineaEstado = 21;
        public const int TransformadorEstado = 22;
        public const int BarraEstado = 23;
        public const int CargaEstado = 24;
        public const int ShuntEstado = 25;

        //Bloques de información para los archivos RAW
        public const string corteBloqueBarras = " 0 / END OF BUS DATA, BEGIN LOAD DATA";
        public const string corteBloqueCargas = " 0 / END OF LOAD DATA, BEGIN FIXED BUS SHUNT DATA";
        public const string corteBloqueShunts = " 0 / END OF FIXED BUS SHUNT DATA, BEGIN GENERATOR DATA";
        public const string corteBloqueGeneradores = " 0 / END OF GENERATOR DATA, BEGIN NONTRANSFORMER BRANCH DATA";
        public const string corteBloqueLineas = " 0 / END OF NONTRANSFORMER BRANCH DATA, BEGIN TRANSFORMER DATA";
        public const string corteBloqueTransformadores = " 0 / END OF TRANSFORMER DATA, BEGIN AREA INTERCHANGE DATA";

        //Constantes para el módulo de Formulas-Configuración(COES)
        public const string OrigenTnaSco = "X";
        public const string OrigenTnaIeod = "Y";
        public const string OrigenServiciosAuxiliares = "Z";
        public const string OrigenTnaDpo = "F";
        public const string TextoTnaSco = "Estimador TNA(SCO)";
        public const string TextoTnaIeod = "Estimador TNA(IEOD)";
        public const string TextoServiciosAuxiliares = "Servicios Auxiliares";
        public const string TextoTnaDpo = "Informacion TNA(DPO)";
        public const string prefijoFormulasTNA = "PRN_";
        public const string ConsPotActivaTna = "1,3,8,15,17,19";

        //Constantes para la lectura de archivos RAW
        public static int indexEqDesconectado = 3;
        public static string valorEqDesconectado = "4";

        //Constantes para las fuentes de información del Pronóstico de la demanda por Barras
        public static string FuenteTna = "tna";
        public static string FuentePr03 = "pr03";

        //Identificador VERGRPCODI de la tabla PRN_VERSIONGRP
        public static int VergrpcodiBase = 1;//Versión base o inicial de los registros

        //Nombre de tablas temporales para la carga de archivos raw
        public static string tablaCargaSco= "PRN_ESTIMADORRAW_AUTOCARGA";
        public static string tablaCargaIeod = "PRN_ESTIMADORRAW_CARGA";

        public static string tablaCargaDemandaDpoSco = "DPO_ESTIMADORRAW_TMP";
    }
}
