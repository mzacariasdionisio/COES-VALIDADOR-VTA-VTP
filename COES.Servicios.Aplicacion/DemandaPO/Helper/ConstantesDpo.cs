using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.DPODemanda.Helper
{
    /// <summary>
    /// Archivo de constantes para el proyecto de Demandas
    /// </summary>
    public class ConstantesDpo
    {
        public const string RutaReporte = "ReportePronostico";
        public const string NomTablaDpoDemandaSco = "DPO_DEMANDASCO";
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoMesAnio = "MM yyyy";
        public const string FormatoAnioMes = "yyyyMM";
        public const string FormatoHoraMinuto = "HH:mm";
        public const int Itv1min = 1440;
        public const int Itv15min = 96;
        public const int Itv30min = 48;

        public const int DporawfuenteIeod = 2;

        public const int OriglectcodiTnaSco = 36;
        public const int OriglectcodiTnaIeod = 37;

        //Tipo de fuente para la lectura de archivos RAW (PRN_ESTIMADORRAW_TEMP)
        public const int EtmrawfntIeod = 1;//Manual
        public const int EtmrawfntSco = 2;//Auto

        // Tipo de proceso para la lectura de archivos RAW (DPO_ESTIMADORRAW)
        public const int CargaAutoRawXMinuto = 1;// Auto X 1 minuto
        public const int CargaAutoRawX30Minutos = 2;// Auto X 30 minutos
        public const int CargaManualRaw = 3;// Manual

        public static string tablaEstimadorRaw = "DPO_ESTIMADORRAW_";
        public static string tablaCargaDemandaDpoTmp = "DPO_ESTIMADORRAW_TMP";
        public static string tablaCargaDemandaDpoCmTmp = "DPO_ESTIMADORRAW_CMTMP";
        public static string tablaCargaDemandaDpoMan = "DPO_ESTIMADORRAW_MANUAL";

        public const string FormatoFechaArchivoRaw = "yyyyMMdd";
        public const string FormatoFechaMedicionRaw = "dd/MM/yyyy HH:mm";

        public const string RutaDemandaRaw = "PathProcesoDemandaRaw";
        public const string PathProcesoDemandaRespaldoRaw = "PathProcesoDemandaRespaldoRaw";
        public const string RutaDemandaRawCostoMarginal = "PathProcesoDemandaRawCostoMarginal";

        //constantes para fileServer
        public const string Reportes = "Reportes\\";
        public const string RutaReportes = "Areas/DemandaPO/Reporte/";
        public const string NombreReporteExcelRaws = "ArchivosRaw.xlsx";

        // Formatos de Archivos
        public const string HojaReporteExcel = "REPORTE";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string AppWord = "application/vnd.ms-word";
        public const string AppPdf = "application/pdf";
        public const string AppCSV = "application/CSV";
        public const string AppTxt = "application/txt";

        //Estilo de los mensajes
        public const string MsgInfo = "info";
        public const string MsgSuccess = "success";
        public const string MsgWarning = "warning";
        public const string MsgError = "error";

        // Para el consumo de funcionalidades del Modulo de Corto Plazo
        public const string CaracterCero = "0";
        public const string PathCostosMarginales = "PathCostosMarginales";
        public const string RutaExportacionCostoMarginal = "Areas/CortoPlazo/Reporte/";

        public const string FormatoFechaMedicionRaw30 = "dd/MM/yyyy 00:00";

        #region DemandaDPO - iteracion2
        public const string prefijoFormulasDPO = "DPO_";
        public const string areaUsuariaDPO = "PO";
        public const string datosCorregidos = "DC";
        public const string ExtensionXlsx = ".xlsx";
        public const string ExtensionTxt = ".txt";
        public static string tablaDpoDatos = "DPO_DATOS96";
        public const string OrigenSirpit = "I";
        public const string OrigenSicli = "G";
        public const string TextoSirpit = "Informacion SIRPIT";
        public const string TextoSicli = "Informacion SICLI";
        public const int fuenteSIRPIT = 1;
        public const int fuenteSICLI = 2;
        public const int fuenteTNA = 3;
        public const int fuenteFormulaSPL = 4;
        public const int IdFormatoReprograma = 49;

        public const int DpotdtBarra = 1;
        public const int DpotdtPunto = 2;
        public const int DpotdtArea = 3;

        public const int DpotmeDemUlibre = 1;

        //Variables para grabar la data corregida en la tabla dpo_medicion96
        public const int DpotmeSirpitActiva = 2;
        public const int DpotmeSirpitReactiva = 3;
        public const int DpotmeSicliActiva = 4;
        public const int DpotmeSicliReactiva = 5;
        public const int DpotmeTnaActiva = 6;
        public const int DpotmeTnaReactiva = 7;
        public const int DpotmeDemVegetativa = 8;
        public const int DpotmeAjusteArea = 9;
        public const int DpotmeAjusteBarra = 10;
        public const int DpotmeAjusteSein = 11;

        public const string DpotmeDemandaTotalPorBarra = "1,8,9,10,11";
        public const string DpotmeDemandaTotalPorBarraNoAjuste = "1,8,9,11";
        public const string DpotmeDemandaTotalArea = "1,8,10,11";
        public const string DpotmeDemandaTotalSein = "1,8,9,10";

        public const int ReprogramaModVegetativa = 1;
        public const int ReprogramaModUsurioLibre = 2;
        public const int ReprogramaModTotalBarra = 3;
        public const int ReprogramaModAjusteBarra = 4;
        public const int ReprogramaModAjusteArea = 5;
        public const int ReprogramaModAjusteSein = 6;
        public const int EmpresaTipoUL = 4;
        //Assetec 20240814
        public const int EmpresaTipoGen = 3;
        public const string OrigenActiva = "1,3,8,17";
        public const string OrigenReactiva = "2,4,916,18,19";
        public const string MedidorDemandaPO = "MedidorDemandaPO";
        public const int OrigenLecturaDemanda = 19;
        public const string PrefijoTNA = "TPTNA";
        public const string PrefijoSIRPIT = "TPSIRPIT";
        public const string PrefijoSICLI = "TPSICLI";

        public const int DpocngcodiGeneral = -1;
        //Opciones anexo
        public const int LSExtremadamenteElevada = 3;
        public const string MedgrpPronosticoTotal = "1,2,9,10";
        public const int MedgrpDemVegetativa = 4;
        public const string MedgrpDemVegTotal = "2,4,9,10";//A.Barra, Veg, A.Agru, A.Area
        public const int MedgrpDemIndustrial = 5;
        public const int MedgrpDemIndTotal = 5;

        public const int AreacodiSein = 1;
        #endregion

        #region PR5
        public const string TextoPR5 = "PR5";
        public const int OriglectcodiPR5 = 21;
        public const string OrigenPR5 = "E";
        public const int TipoNodefinido = -1;
        public const int TipoAreaOperativa = 1;
        public const int TipoSubAreaOperativa = 2;

        public const string AreaOperativaAbrevNorte = "NORTE";
        public const string AreaOperativaAbrevCentro = "CENTRO";
        public const string AreaOperativaAbrevSur = "SUR";
        public const string AreaOperativaAbrevNorteMedio = "NORTE_MEDIO";
        public const string AreaOperativaAbrevLima = "LIMA";

        public const string AreaOperativaAbrevCentroNorte = "CENNOR";
        public const string AreaOperativaAbrevCentroSur = "CENSUR";
        public const string AreaOperativaAbrevElectroandes = "ELA";
        public const string AreaOperativaAbrevSurMedio = "SUR_MEDIO";
        public const string AreaOperativaAbrevSurEste = "SUR_ESTE";
        public const string AreaOperativaAbrevSurOeste = "SUR_OESTE";
        public const string AreaOperativaAbrevArequipa = "AREQUIPA";

        public const string AreaOperativaNombreNorte = "NORTE";
        public const string AreaOperativaNombreCentro = "CENTRO";
        public const string AreaOperativaNombreSur = "SUR";
        public const string AreaOperativaNombreNorteMedio = "NORTE MEDIO";
        public const string AreaOperativaNombreLima = "LIMA";
        public const string AreaOperativaNombreCentroNorte = "CENTRO - NORTE";
        public const string AreaOperativaNombreCentroSur = "CENTRO - SUR";
        public const string AreaOperativaNombreElectroandes = "ELA";
        public const string AreaOperativaNombreSurMedio = "SUR MEDIO";
        public const string AreaOperativaNombreSurEste = "SUR ESTE";
        public const string AreaOperativaNombreSurOeste = "SUR OESTE";
        public const string AreaOperativaNombreArequipa = "AREQUIPA";

        public const string FormulaAreaOperativaNorte = "PRUEBA DTI final";
        public const string FormulaAreaOperativaCentro = "PRUEBA DTI final";
        public const string FormulaAreaOperativaSur = "PRUEBA DTI final";
        public const string FormulaAreaOperativaNorteMedio = "PRUEBA DTI final";
        public const string FormulaAreaOperativaLima = "PRUEBA DTI final";
        public const string FormulaAreaOperativaElectroandes = "PRUEBA DTI final";
        public const string FormulaAreaOperativaSurMedio = "PRUEBA DTI final";
        public const string FormulaAreaOperativaSurEste = "PRUEBA DTI final";
        public const string FormulaAreaOperativaSurOeste = "PRUEBA DTI final";
        public const string FormulaAreaOperativaArequipa = "PRUEBA DTI final";
        #endregion

        #region Anexo D        
        public const string DataMaestra = "DM";
        public const string DataProcesar = "PR";

        public const int ReconstruccionR1 = 1;
        public const int ReconstruccionR2 = 2;
        public const int FiltradoF1 = 3;
        public const int FiltradoF2 = 4;
        public const int AnalisisA1 = 5;
        public const int AnalisisA2 = 6;

        public const string NombreReporteConfiguracion = "ArchivosReporteConfiguracion.xlsx";
        #endregion

        public const int FormatcodiDemPrevRDO = 141;//140
        public const int LectcodiDemPrevRDO = 245;
        public const int LectcodiDespacho = 6;
        public const int TipoinfocodiDespacho = 1;
        public const int TipoinfocodiSein = 20;
        public const int PtomedicodiSein = 1222;

        public const string MedCalcFuenteReprograma = "REPROGRAMA";

        public const int SrestcodiGeneracionTotal = 90;
        public const int SrestcodiPeridasTransmision = 59;

        public const string ReporteDemanda = "ReporteMedidorDemanda";

        public const string FormOrigenDespacho = "B";
        public const string FormOrigenTnaSCO = "X";
        public const string FormOrigenTnaIEOD = "Y";

        #region DemVeg.20250114
        public const string RutaReporteProDemVeg = "RutaPronosticoDemVeg";
        #endregion
    }
}
