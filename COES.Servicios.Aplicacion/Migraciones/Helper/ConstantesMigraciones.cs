using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Migraciones.Helper
{
    public class ConstantesMigraciones
    {
        public const int AccionVer = 1;
        public const int AccionEditar = 2;
        public const int AccionNuevo = 3;
        public const int AccionExportar = 199945;
        public const int AccionGrabarBD = 199946;

        public const string TiporelcodiDigsilent = "12,14,15,16";

        public const string PropcodiDigsilentLinea = "1068";
        public const string PropcodiDigsilentTrafo2d = "1076";
        public const string PropcodiDigsilentTrafo3d = "1077";
        public const string PropcodiDigsilentSvc = "1078";
        public const string PropcodiDigsilentDemanda = "1839,1840,1842,1843,1841";
        public const string FileDigsilente = "DigSilent";
        public const string RptRols = "RptRol";
        public const string RptRDemandaBarras = "RptDemandadBarras";

        public const int TipoInformeMantenimientoProg = 1;
        public const int TipoInformeEstadoOperativo = 2;
        public const int TipoInformeEstadoOperativo90 = 3;
        public const string RptInfoP25 = "RptInfoP25";
        public const string RptEstOpe = "RptEstOpe";
        public const string RptRestricOp = "RptRestricionesOperativas";
        public const int TieneCheckProc25 = 1;

        public const string RolTurnoNoExistente = "---";
        public const int AreacodiDTI = 1;
        public const int AreacodiSGI = 17;
        public const int AreacodiSPR = 7;
        public const int AreacodiSubdirCoord = 3;
        public const int ActcodiA1 = 158;
        public const int ActcodiA2 = 159;
        public const int ActcodiA3 = 160;
        public const int ActcodiC1 = 164;
        public const int ActcodiC2 = 165;
        public const int ActcodiC3 = 166;
        public const int ActcodiE1 = 161;
        public const int ActcodiE2 = 162;
        public const int ActcodiE3 = 163;

        public const string ReporteWordIDCOS = "ReporteIDCOS";
        public const string CatecodiAll = "-2";
        public const string CatecodiParametro = "2,3,4,5,6,9,10,11,12,13";
        public const string CatecodiParametroFiltro = "2,9,6,4,3,5,15,16,17,18";
        public const string CatecodiParametroVisualizacion = "1,2,11,9";
        public const string CatecodiControlCambio = "4,6";
        public const string Caudcp = "caudcp.dat";
        public const int Activo = 1;
        public const int Inactivo = 0;
        public const int GrupodatActivo = 0;
        public const int GrupodatInactivo = 1;
        public const int GrupoPropiedadEquipoTiene = 1;
        public const int GrupoPropiedadEquipoNoTiene = 0;
        public const string AreacoesParaVisualizacion = "1,3,4,5,7,8";
        public const string RptDemArea = "RptDemArea";
        public const string RptProdcco = "RptProdcco";
        public const string RptMedidores = "RptMedidores";
        public const string RptControlCambios = "Reporte_Control_Cambios";
        public const string RptControlCambiosNombreHoja = "Reporte";
        public const string RptControlCambiosTituloHoja = "Reporte de Control de Cambios";

        public const int TipogrupoTodos = 0;
        public const int TipogrupoIntegrantes = 1;
        public const int TipogrupoRER = 2;
        public const int TipogrupoCogeneracion = 3;
        public const int TipogrupoNoIntegrantes = 10;
        public const int TipogrupoReservaFriaFicticio = 195338;

        public const int ReporteCdispatchFl = 66;
        public const int ReporteCdispatchCmg = 67;

        public const string CabeceraCDisptachWeb = "NivelDemanda,Escenario,ReservaFriaTermica,ReservaRotante,ReservaEficiente,ReservaEficienteGas,ReservaEficienteCarbon";

        public const string Totalcogeneracion = "999,Total CoGeneracion,-1222,20,F0,GENERACION,0";
        public const string Totalrenov = "999,Total Renovables,-1223,20,F0,GENERACION,0";
        public const string Generacioncoes = "999,Total Generación COES,-1224,20,F0,GENERACION,0";

        public const string Demandancp = "9991,Demanda NCP,1221,20,F0,SALIDAS NCP,0";
        public const string Gtotal = "9991,Oferta,1222,20,F1,SALIDAS NCP,0";
        public const string Perdidasncp = "9991,Perdidas NCP,1223,20,F1,SALIDAS NCP,0";
        public const string Deficitncp = "9991,Deficit NCP,1224,20,F3,SALIDAS NCP,0";

        public const string Demandacentroncp = "0,Demanda Centro NCP,1225,20,F1,,";

        public const string Flcotasoca = "9992,Flujo Cotaruse-Socabaya 220 kV,1310,8,F0,FLUJO EN LINEAS,L-2053,L-2054";
        public const string Flchimbtruj = "9992,Flujo Chimbote-Trujillo 220 kV,1318,8,F0,FLUJO EN LINEAS,L-2232,L-2233";
        public const string Flsepanuchimb = "9992,Flujo Paramonga-Chimbote 220 kV,1311,8,F0,FLUJO EN LINEAS,L-2215,L-2216";
        public const string Flpomacochasjuan = "9992,Flujo Pomacocha-San Juan 220 kV,1316,8,F0,FLUJO EN LINEAS,L-2205,L-2206";
        public const string Fltrujguadalupe = "9992,Flujo Trujillo - Guadalupe 220 kV,1320,8,F0,FLUJO EN LINEAS,L-2234,L-2235";
        public const string Flarmicota = "9992,Flujo Campo Armiño - Cotaruse 220 kV,1323,8,F0,FLUJO EN LINEAS,L-2051,L-2052";
        public const string Floconsjose = "9992,Flujo Ocoña - San Jose 500 kV,1324,8,F0,FLUJO EN LINEAS,L-5036";
        public const string Flsjosemonta = "9992,Flujo San Jose - Montalvo 500 kV,1325,8,F0,FLUJO EN LINEAS,L-5037";
        public const string Fltinnvasoca = "9992,Flujo Tintaya Nueva - Socabaya 220 kV,1326,8,F0,FLUJO EN LINEAS,L-2022,L-2023";
        public const string Flsocamoque = "9992,Flujo Socabaya - Moquegua 220 kV,1327,8,F0,FLUJO EN LINEAS,L-2025,L-2026";
        public const string Flcarabchimb = "9992,Flujo Carabayllo - Chimbote 500 kV,1328,8,F0,FLUJO EN LINEAS,L-5006";
        public const string Flguadareque = "9992,Flujo Guadalupe - Reque 220 kV,1329,8,F0,FLUJO EN LINEAS,L-2236,L-2237";
        public const string Flninapiura = "9992,Flujo La Niña - Piura 220 kV,1330,8,F0,FLUJO EN LINEAS,L-2241";
        public const string Flcarabchilca = "9992,Flujo Carabayllo - Chilca 500 kV,1331,8,F0,FLUJO EN LINEAS,L-5001";
        public const string Flsjuanindus = "9992,Flujo San Juan - Industriales 220 kV,1332,8,F0,FLUJO EN LINEAS,L-2018";
        public const string Flchilcaplani = "9992,Flujo Chilca CTM - La Planicie 220 kV,1333,8,F0,FLUJO EN LINEAS,L-2103,L-2104";
        public const string Flplanicarab = "9992,Flujo La Planicie - Carabayllo 220 kV,1334,8,F0,FLUJO EN LINEAS,L-2105,L-2106";
        public const string Flventachav = "9992,Flujo Ventanilla - Chavarria 220 kV,1335,8,F0,FLUJO EN LINEAS,L-2244,L-2245,L-2246";
        public const string Flventazapa = "9992,Flujo Ventanilla - Zapallal 220 kV,1336,8,F0,FLUJO EN LINEAS,L-2242,L-2243";
        public const string Flhuanzcarab = "9992,Flujo Huanza - Carabayllo 220 kV,1337,8,F0,FLUJO EN LINEAS,L-2110";

        public const string Cmgsein_ncp = "9993,Cmg SEIN NCP(US$/MWh),1217,21,F2,COSTOS MARGINALES,0";
        public const string Cmgstarosancp = "9993,Cmg Santa Rosa NCP(US$/MWh),1226,21,F2,COSTOS MARGINALES,SANTAROSA220";
        public const string Cmgtrujillo_ncp = "9993,Cmg Trujillo NCP(US$/MWh),1219,21,F2,COSTOS MARGINALES,TRUJILLO220";
        public const string Cmgsocabaya_ncp = "9993,Cmg Socabaya NCP(US$/MWh),1220,21,F2,COSTOS MARGINALES,SOCABAYA220";
        public const string Niveld = "9993,Nivel de Demanda,-1203,10,F0,COSTOS MARGINALES,0";// 0 decimales

        public const string Escenario = "0,Escenario,1203,10,F0,,"; // 0 decimales
        public const string Cmgxmwh_sr = "0,CmgxMWh SR,1197,7,F1,,";
        public const string Cmgxmwh_srideal = "0,CmgxMWh Ideal SR,1201,7,F1,,";
        public const string Unidadmarginal = "0,Unidad Marginal,1200,10,F0,,"; //codigo gr marginal
        public const string Unidadmarginalideal = "0,Unidad Marginal Ideal,1202,10,F0,,"; //codigo gr marginal ideal

        public const string Reservarotante = "0,Reserva Rotante,-1195,20,F1,,";
        public const string Reservasec = "0,Reserva Primaria Estacional,1215,20,F1,,";
        public const string Reservafria = "0,Reserva Fria,1216,20,F1,,"; //esto no se guarda porque no se calcula la hidrica
        public const string Reservafriatermica = "9994,Reserva Fría Térmica,1227,20,F0,RESERVA,0";
        public const string Reservafriahidraulica = "0,ReservaFriaHidraulica,1228,20,F1,,"; //esto no se calcula
        public const string ReservaEficiente = "9994,Reserva Eficiente,1205,20,F0,RESERVA,0";
        public const string ReservaEficienteGas = "9994,Reserva Eficiente Gas,1207,20,F0,RESERVA,0";
        public const string ReservaEficienteCarbon = "9994,Reserva Eficiente Carbón,1206,20,F0,RESERVA,0";

        public const string Ntablachaca = "9995,Nivel Tablachaca,1218,19,F1,R,0";
        public const string Demandasein = "0,Demanda,1210,20,F1,,";

        public const string Demandacentro = "9996,Demanda Centro,1211,20,F1,DEMANDA POR AREAS,0";
        public const string Demandanorte = "9996,Demanda Norte,1212,20,F1,DEMANDA POR AREAS,0";
        public const string Demandasur = "9996,Demanda Sur,1213,20,F1,DEMANDA POR AREAS,0";
        public const string Demandaela = "9996,Demanda Electroandes,1214,20,F1,DEMANDA POR AREAS,0";
        public const string Demandaecuador = "9996,Demanda Ecuador,1229,20,F1,DEMANDA POR AREAS,0";
        public const string Demandacoes = "9996,Demanda COES,-1221,20,F0,DEMANDA POR AREAS,0";

        //Inicio cambio 02-06-2021 - Movisoft
        public const string NotaCostoOperacionDolares = "El costo total operativo calculado corresponde al precio obtenido por el aplicativo YUPANA.";
        //Fin cambio 02-06-2021 - Movisoft

        public const string NotaReservaEficiente = "Las columnas “Reserva Eficiente Gas” y “Reserva Eficiente Carbon” resultan de la suma de unidades no despachadas a gas y carbón, respectivamente, a las cuales se les suma la Reserva Rotante de gas y carbón.  La columna “Reserva Eficiente” es la suma de ambas reservas. Estos cálculos son aproximados debido a que no consideran limitaciones de ningún tipo.";

        public const string ColorCdispatchRFria = "#BCCCEA";
        public const string ColorCdispatchTminarr = "#FF7F50";
        public const string ColorCdispatchMantto = "#9370DB";
        public const string ColorCdispatchGrupoOpera = "#10EA53";
        public const string ColorCdispatchRotante = "#FFEAA5";

        //Manual de usuario SGI
        public const string ArchivoManualUsuarioIntranetSGI = "Manual_Usuario_SGI_v1.2.pdf";
        public const string ModuloManualUsuarioSGI = "Manuales de Usuario\\";

        //constantes para fileServer SGI
        public const string FolderRaizSGIModuloManual = "Migración SGOCOES Desktop a Intranet\\";

        //Para reporte de Reserva Fría de AnexoA
        public const string AnexoAReservafriatermica = "9997,ReservaFriaTermica,7000,20,F0,RESERVA,0";
        public const string AnexoAReservafriatermicaRap = "9997,ReservaFria Rapida Termica,7001,20,F0,RESERVA,0";
        public const string AnexoAReservafriatermicaXMantto = "9997,ReservaFria Termica xMantenimiento,7002,20,F0,RESERVA,0";

        public const int ConcepcodiOpComercial = 611;
        public const int PropcodiPotInstNomEolico = 1602;
        public const int PropcodiPotInstNomSolar = 1710;
        public const int Concepcodi85 = 85;
        public const int Concepcodi14 = 14;
        public const int Concepcodi16 = 16;
        public const int Concepcodi136 = 136;
        public const int Concepcodi139 = 139;
        public const string RptDespachoMD = "RptDespachoMD";
        public const string RptAnexoIdcos = "RptAnexoIdcos";
        public const string RptCmgCortoPlazo = "RptCmgCortoPlazo";
        public const string RptCDispatch = "RptCDispatch";
        public const string Cirflwcp = "cirflwcp.csv";
        public const string Gerhidcp = "gerhidcp.csv";
        public const string Gergndcp = "gergndcp.csv";
        public const string Gertercp = "gertercp.csv";
        public const string Defcitcp = "defcitcp.csv";
        public const string Cmgbuscp = "cmgbuscp.csv";
        public const string Cmgdemcp = "cmgdemcp.csv";
        public const string Cpdexbus30 = "cpdexbus30.dat";
        public const string Resaghcp = "resaghcp.csv";
        public const string Resagtcp = "resagtcp.csv";
        public const int SiparcodiDigsilentDemanda = 16;

        public const string SubcausaReqSistema = "POR REQUERIMIENTO DEL SISTEMA";
        public const string SubcausaReqPropio = "POR REQUERIMIENTO PROPIO";
        public const string SubcausaPruebaAleat = "POR PRUEBAS ALEATORIAS PROC.25";
        public const string SubcausaTension = "POR TENSION SS";
        public const string SubcausaMantto = "POR MANTENIMIENTO";
        public const string SubcausaNoDefinido = "NO DEFINIDO";

        public const string FamnombHidro = "hidráulicos";
        public const string FamnombTermo = "térmicos";
        public const string FamnombSolar = "solares";
        public const string FamnombEolic = "eólicos";

        public const string FlCentroSur = "9992,Flujo Enlace Centro - Sur,1250,8,F0,FLUJO EN LINEAS,L-5033,L-5033_CS,L-5034,L-5034 CS,L-2051,L-2052";//nueva linea
        public const string FlChilcaCarapongo = "9992,Flujo Chilca - Carapongo 500 kV,1251,8,F0,FLUJO EN LINEAS,L-5001";//nueva linea
        public const string FlCarapongoCarabayllo = "9992,Flujo Carapongo - Carabayllo 500 kV,1252,8,F0,FLUJO EN LINEAS,L-5003";//nueva linea
        public const string FlChimboteTrujillo = "9992,Flujo Chimbote - Trujillo 500 kV,1253,8,F0,FLUJO EN LINEAS,L-5008";//nueva linea
        public const string FlTrujilloNinia = "9992,Flujo Trujillo - La Niña 500 kV,1254,8,F0,FLUJO EN LINEAS,L-5010";//nueva linea
        public const string FlColcabambaPoroma = "9992,Flujo Colcabamba - Poroma 500 kV,1255,8,F0,FLUJO EN LINEAS,L-5031,L-5031_CS";//nueva linea
        public const string FlChilcaPoroma = "9992,Flujo Chilca - Poroma 500 kV,1256,8,F0,FLUJO EN LINEAS,L-5032,L-5032_CS";//nueva linea
        public const string FlPoromaYarabamba = "9992,Flujo Poroma - Yarabamba 500 kV,1257,8,F0,FLUJO EN LINEAS,L-5033,L-5033_CS";//nueva linea
        public const string FlPoromaOconia = "9992,Flujo Poroma - Ocoña 500 kV,1258,8,F0,FLUJO EN LINEAS,L-5034,L-5034_CS";//nueva linea
        public const string FlYarabambaMontalvo = "9992,Flujo Yarabamba - Montalvo 500 kV,1259,8,F0,FLUJO EN LINEAS,L-5035";//nueva linea
        public const string FlHuancavelicaIndependencia = "9992,Flujo Huancavelica - Independencia 220 kV,1260,8,F0,FLUJO EN LINEAS,L-2231,L-2132";//nueva linea
        public const string FlChilcaSanJuan = "9992,Flujo Chilca - San Juan 220 kV,1261,8,F0,FLUJO EN LINEAS,L-2093,L-2094,L-2095";//nueva linea

        public const string Defbuscp = "defbuscp.csv";
        public const string Ncpcope = "ncpcope.csv";
        public const string Cpde30PE = "cpde30PE.dat";
        public const string Gerunicp = "gerunicp.csv";

        public const string FormatoFecha = "dd/MM/yyyy";

        #region Migraciones 2024
        public const int ReporteHistoricoSemanalHidrologia = 11;
        #endregion

        public enum Catecodi
        {
            ModoOperacionTermico = 2,
            GrupoTermico = 3,
            CentralTermico = 4,
            CentralHidro = 6,
            ModoOperacionHidro = 9,
            GrupoHidraulico = 5,
            CentralHidraulico = 6,
            GrupoSolar = 15,
            CentralSolar = 16,
            GrupoEolico = 17,
            CentralEolica = 18
        }
        public const string ConcepcodisCodigoOsinergmin = "641,650,651,652,653,654";

        //Demanda por area
        public const int IdReporteDemandaAreaPrincipal = 71;

        #region AddIn
        public const string Seleccionado = "1";
        public const string NoSeleccionado = "0";

        public const string HojaActiva = "Activa";
        public const string HojaReactiva = "Reactiva";
        public const string HojaFlujos = "Flujos";
        #endregion

        #region Notificaciones

        public const int PlantcodiNotificacionCargaDespacho = 80;
        public const int PlantcodiNotificacionRecalculoCostoOp = 81;

        public const string VariableNombreUsuario = "{NOMBRE_USUARIO}";
        public const string VariableFechaSistema = "{FECHA_SISTEMA}";
        public const string VariableFechaProceso = "{FECHA_PROCESO}";

        #endregion

        public const int TipoCalculoMwMantto = 1;
        public const int TipoCalculoReservaFria = 2;

        #region YUPANA RSF 2024

        public const int RsftipoSubir = 2;
        public const int RsftipoBajar = 3;
        public const int RpfTipo = 1;

        public const int ReservaPrimaria = 1;
        public const int ReservaSecundaria = 2;

        public const string EstadoVigenteDesc = "Vigente";
        public const string EstadoNoVigenteDesc = "No Vigente";

        #endregion

    }

    /// <summary>
    /// Clase para obtener los datos del usuario que vienen del servicio
    /// </summary>
    public class UsuarioParametro
    {
        public int UserCode { get; set; }
        public string UsernName { get; set; }
        public string UserEmail { get; set; }
        public string UserLogin { get; set; }
        public int AreaCode { get; set; }
        public string AreaName { get; set; }
        public string AreaAbrev { get; set; }
    }
    public class MigracionesResult
    {
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public int nRegistros { get; set; }
        public string Comentario { get; set; }
        public string Resultado2 { get; set; }
        public string Resultado3 { get; set; }
        public string Detalle { get; set; }
    }

    public class AddInColModo
    {
        public string AbrevModo { get; set; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        public string Asterisco { get; set; }
        public string Formula { get; set; }
    }

    public class ReporteProduccion
    {
        public List<RegistroRepProduccion> ListadoRegistros { get; set; }
        public decimal? TotalSein { get; set; }
        public decimal? TotalMD { get; set; }
        public string StrTotalSein { get; set; }
        public string StrTotalMD { get; set; }

    }
    public class RegistroRepProduccion
    {
        public int NumReg { get; set; }
        public int Emprcodi { get; set; }
        public int? Ptomedicodi { get; set; }
        public int IdUnidad { get; set; }
        public DateTime? FechaMaxDemanda { get; set; }
        public string StrFechaMaxDemanda { get; set; }
        public string Emprnomb { get; set; }
        public string UnidadNomb { get; set; }
        public decimal EHidro { get; set; }
        public decimal ETermo { get; set; }
        public decimal ETotal { get; set; }
        public decimal? MaxDemanda { get; set; }
        public string StrEHidro { get; set; }
        public string StrETermo { get; set; }
        public string StrETotal { get; set; }
        public string StrMaxDemanda { get; set; }

    }

    public class RegistroRepHidrologia
    {
        public decimal? H1 { get; set; }
        public string MedifechaPto { get; set; }
        public string OrigenPtomedidesc { get; set; }
        public string Tipoinfodesc { get; set; }
        public string Lastuser { get; set; }
        public string LastdateDesc { get; set; }


    }

    #region RSF YUPANA 2024

    public class Datos48Reserva
    {
        public PrReservaDTO DatosSubir { get; set; }
        public PrReservaDTO DatosBajar { get; set; }
    }

    #endregion

}
