using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.Eventos.Helper
{
    /// <summary>
    /// Constantes usadas en el módulo
    /// </summary>
    public class ConstantesEventos
    {
        public const string ExportacionRSF = "ReporteRSF.xlsx";
        public const string ExportacionRSF30 = "ReporteRSF30.xlsx";
        public const string ExportacionRSFReporte = "ReservaAsignada.xlsx";
        public const string ArchivoRA = "ra.txt";
        public const string SesionVersion = "SesionVersion";
        public const string SesionTurno = "SesionTurno";
        public const string SesionFamilia = "SesionFamilia";
        public const string SesionTipoEmpresa = "SesionTipoEmpresa";
        public const string SesionEmpresa = "SesionEmpresa";
        public const string SesionFechaInicio = "SesionFechaInicio";
        public const string SesionFechaFin = "SesionFechaFin";
        public const string SesionInterrupcion = "SesionInterrupcion";
        public const string SesionTipoEvento = "SesionTipoEvento";
        public const string RutaReporte = "Areas/Eventos/Reporte/";
        public const string ArchivoAgcXML = "RegRgUp.xml";
        public const string ArchivoComprmidoXml = "RsfComprimido.zip";
        public const string ArchivoComprmidoAgcXml = "RsfComprimido.zip";
        public const string ArchivoUnitMaxGeneration = "UnitMaxGenerationAGC.xml";
        public const string ArchivoAGCUnitMaxGeneration = "UnitMaxGeneration.xml";

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranetSGI = "Manual_Usuario_SGI_v1.2.pdf";
        public const string ModuloManualUsuarioSGI = "Manuales de Usuario\\";

        //constantes para fileServer
        public const string FolderRaizSGIModuloManual = "Migración SGOCOES Desktop a Intranet\\";

        //inicio MODULO PRUEBA UNIDAD
        //Constantes Escenarios de Prueba aleatoria
        public const string Escenario1 = "No hay problemas en la unidad";
        public const string Escenario2 = "Falla entre arranque y sincronización";
        public const string Escenario3 = "Falla en primera rampa de toma de carga";
        public const string Escenario4 = "Falla a plena carga";

        //grupo funcional
        public const int CatecodiGrupoFuncional = 2;

        //conceptos de prueba de unidad
        public const int ConcepcodiPotenciaEfectiva = 14;
        public const int ConcepcodiTiempoEntreArranques = 136;
        public const int ConcepcodiTiempoArranqueSinc = 121;
        public const int ConcepcodiTiempoSincPotEf = 126;
        public const int ConcepcodiRpf = 282;
        public const int ConcepcodiTiempoPruebaAleat = 283;
        public const int ConcepcodiRatioPcalPefect = 284;

        //origlectcodi de despacho
        public const int OriglectcodiDespacho = 1;

        //lectcodi de pruebas aleatorias
        public const int LectcodiPruebaAleat = 106;

        //tipo informacion MW
        public const int TipoInfoMw = 1;

        //Mensajes
        public const string MensajeGeneralEscenario = "Ha ocurrido un error.";
        public const string MensajeHorasEscHoHsHpe = "Revisar horas ingresadas (HO), (HS), (HPE). ";
        public const string MensajeHorasEscHoHfallaHo2Hs2Hpe2 = "Revisar horas ingresadas (HO), (HFalla), (HO2), (HS2), (HPE2). ";
        public const string MensajeHorasEscHoHsHfallaHo2Hs2Hpe2 = "Revisar horas ingresadas (HO), (HS), (HFalla), (HO2), (HS2), (HPE2). ";
        public const string MensajeHorasEscHoHsHpeHfallaHo2Hs2Hpe2 = "Revisar horas ingresadas (HO), (HS), (HPE), (HFalla), (HO2), (HS2), (HPE2). ";
        public const string MensajeDiaPruebaNoCoincide = "Día de prueba no coincide con fechas ingresadas.";
        public const string MensajeInconsistFechaHora = "Inconsistencia de fecha-hora.";

        //fin MODULO PRUEBA UNIDAD

        //inicio MODULO PRUEBA UNIDAD
        public const string FechaConsultaInicioPrUnd = "FechaConsultaInicioPrUnd";
        public const string FechaConsultaFinPrUnd = "FechaConsultaFinPrUnd";
        //fin MODULO PRUEBA UNIDAD
    }

    /// <summary>
    /// Constantes para envio de correos
    /// </summary>
    public class ConstantesEnviarCorreo
    {
        public const int PlantcodiPdm = 21;
        public const int PlantcodiPdo = 22;
        public const int PlantcodiPdmPdo = 23;
        public const int PlantcodiPdmSegundoFormato = 24;
        public const int PlantcodiPdoSegundoFormato = 25;
        public const int PlantcodiPdmPdoSegundoFormato = 26;
        public const int PlantcodiRdo = 27;


        public const int PlantcodiPruebasAleatorias = 20;
        public const int PlantcodiCostoVariable = 28;
        public const int PlantcodiIdcos = 29;
        public const int PlantcodiInfoNecesariaPdo = 30;
        public const int PlantcodiRacionamientoCarga = 31;
        public const int PlantcodiIndispSistemaTelef = 32;
        public const int PlantcodiDisponibSistemaTelef = 33;
        public const int PlantcodiRechazoManualDeCarga = 34;
        public const int PlantcodiProgramaSemanalPrelim = 35;
        public const int PlantcodiProgramaSemanalFinalManto = 36;
        public const int PlantcodiProgramaSemanalFinalOperacion = 37;
        public const int PlantcodiProgramaSemanalFinalOperacionManto = 38;
        public const int PlantcodiInfoPsoPsm = 39;
        public const int PlantcodiTermElabProgVerFinPsmPso = 40;
        public const int PlantcodiTermElabProgPr43 = 41;
        public const int PlantcodiTermElabProgPdm = 42;
        public const int PlantcodiTermElabProgPdo = 43;
        public const int PlantcodiEntregaPdoPdm = 44;
        public const int Plantcodireporteemergencia = 59;

        #region Mejoras CMgN
        public const int PlantcodiCMGsHOparaIEOD = 116;
        public const int PlantcodiReporteCMg= 117;
        public const int PlantcodiReporteHO = 110;
        public const int PlantcodiReportePremCMg = 111;
        public const int PlantcodiReportePremHO= 112;
        public const int PlantcodiReporteFinCMg = 113;
        public const int PlantcodiReporteFinHO = 114;
        public const int PlantcodiUpdateCMGsHOparaIEOD = 115;
        #endregion

        //Plantillas de Informe de Fallas N1
        public const int PlantcodiInformePrelimInicialEnvioN1 = 45;
        public const int PlantcodiInformePrelimInicialReenvioN1 = 46;
        public const int PlantcodiInformePrelimEnvioN1 = 47;
        public const int PlantcodiInformePrelimReenvioN1 = 48;
        public const int PlantcodiInformeFinalEnvioN1 = 49;
        public const int PlantcodiInformeFinalReenvioN1 = 50;

        //Plantillas de Informe de Fallas N2
        public const int PlantcodiInformePrelimInicialSinInformeN2 = 51;
        public const int PlantcodiInformePrelimInicialN2 = 52;
        public const int PlantcodiInformeFinalSinInformeEnvioN2 = 53;
        public const int PlantcodiInformeFinalSinInformeReenvioN2 = 54;

        public const int PlantcodiInfFallaFinalNoEmitidoN2 = 55;
        public const int PlantcodiInfFallasFinalSPrelimInicialN2 = 56;
        public const int PlantcodiInfFallasFinalSinPrelimInicNiFinalEmp = 57;


        //Subcausa relacionado a Envío de correo
        public const int SubcausacodiProgramaDiario = 321;
        public const int SubcausacodiReprograma = 322;
        public const int SubcausacodiCostoVariable = 323;
        public const int SubcausacodiIdcos = 324;
        public const int SubcausacodiManto7Dias = 325;
        public const int SubcausacodiAnalisisAtr = 326;
        public const int SubcausacodiInfoNecesariaPdo = 327;
        public const int SubcausacodiRacionamientoCarga = 328;
        public const int SubcausacodiIndispSistemaTelef = 329;
        public const int SubcausacodiSistemaComunicPrincipal = 330;
        public const int SubcausacodiRechazoManualCarga = 331;
        public const int SubcausacodiProgramaSemanalPreliminar = 332;
        public const int SubcausacodiProgramaSemanalFinal = 333;
        public const int SubcausacodiInfoPsoPsm = 334;
        public const int SubcausacodiTerminoElabPrograma = 335;
        public const int SubcausacodiEntregaPdpoPdm = 336;
        public const int SubcausacodiReporteemergencia = 351;

        #region Mejoras CMgN
        public const int SubcausacodiCMgHOparaIEDO = 403;
        public const int SubcausacodiReporteCMg = 404;
        public const int SubcausacodiReporteHO = 405;
        public const int SubcausacodiReportePremCMg = 406;
        public const int SubcausacodiReportePremHO = 407;
        public const int SubcausacodiReporteFinCMg = 408;
        public const int SubcausacodiReporteFinHO = 409;
        public const int SubcausacodiUpdateCMgHOparaIEDO = 410;
        #endregion

        //Carpetas de informes SCO
        public const string CarpetaInformeFallaN1 = "InformedePerturbaciones";
        public const string CarpetaInformeFallaN2 = "InformedePerturbacionesN2";
        public const string CarpetaInformeMinisterio = "InformeMinisterio";
    }
}