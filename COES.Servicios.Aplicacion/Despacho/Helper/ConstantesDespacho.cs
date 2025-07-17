using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Despacho.Helper
{
    public class ConstantesDespacho
    {
        public const string CTotalDB5 = "CTotalDB5";
        public const string CTotalR500 = "CTotalR500";
        public const string CTotalR6 = "CTotalR6";
        public const string CTotalCarb = "CTotalCarb";
        public const string CCombGasSI = "CCombGas_SI";
        public const string CTotalBag = "CTotalBag";
        public const string CTotalBio = "CTotalBio";

        public const string TipoTermico = "T";
        public const string TipoHidraulico = "H";
        public const int CategoriaCentralTermica = 4;
        public const int CategoriaGrupoTermico = 3;
        public const int CategoriaModoTermico = 2;
        public const int CategoriaCentralHidraulico = 6;
        public const int CategoriaGrupoHidraulico = 5;
        public const int CategoriaModoHidraulico = 9;

        #region BarraModelada
        public const int CategoriaBarraModelada = 10;
        public const int FamiliaBarraEquipo = 7;
        #endregion

        #region CURVA CONSUMO

        public const string Activo = "A";
        public const string FolderReporte = "Areas\\Despacho\\Reporte\\";
        public const string NombreReporteEnvios = "ReporteCostoIncremental.xlsx";
        public const string NombreReporteEnviosTexto = "segmterm.dat";

        public const string CEC = "Cec_SI";
        public const string PE = "Pe";
        public const string Rendimiento = "Rend_SI";
        public const string CostoCombustible = "CComb";
        public const string CVNC = "CVNC";
        public const string CVC = "Cvc";
        public const string TipComb = "TipComb";
        public const int ConcepCodi_PCIPVariosSI = 698; // Codigo de Formula PCI_Costo_Combustibles_Varios

        #endregion

        public const string TipoProgramaDiario = "D";
        public const string TipoProgramaSemanal = "S";
        public const string TipoProgramaTodos = "-1";
        public const string TipoProgramaDiarioDesc = "Diario";
        public const string TipoProgramaSemanalDesc = "Semanal";

        #region FICHA TÉCNICA

        //Acciones
        public const int AccionVer = 1;
        public const int AccionEditar = 2;
        public const int AccionNuevo = 3;

        public const int EstadoActivo = 1;
        public const int EstadoInactivo = 0;
        public const string Si = "S";
        public const string No = "N";

        public const string EstiloBaja = "background-color: #FFDDDD;";

        public const string FormatoFechaReporte = "yyyyMMddHHmmss";
        public const string HojaPlantillaExcel = "PLANTILLA";
        public const string HojaCategoria = "CATEGORÍA DE GRUPO";

        // Formatos de Archivos
        public const string AppExcel = "application/vnd.ms-excel";

        public const string NombrePlantillaExcelConceptos = "Plantilla_Concepto.xlsx";
        public const string RutaReportes = "Areas/Despacho/Reporte/";

        // Constante para el repositorio CSV
        public const string AppCSV = "application/CSV";
        public const string SeparadorCampo = ",";
        public const string SeparadorCampoCSV = ";";

        //Ficha Tecnica - Etapa II
        public const int FichaMaestraPortal = 1;

        #endregion

        #region HTRABAJO

        public const int TipoGeneradorSolar = 36;
        public const int TipoSolar = 37;
        public const int TipoGeneradorEolico = 38;
        public const int TipoEolica = 39;

        public const int TipoHtrabajo = 1;
        public const int TipoScada = 2;
        public const string TipoFuenteHtrabajo = "I";
        public const string TipoFuenteScada = "S";

        //generarcion csv
        public const string HojaActivaExcel = "Activa";
        public const string FormatoFechaHoraCsv = "yyyy-MM-dd_HH-mm";
        public const string FormatoFechaHora = "dd_MM_yyyy_HH:mm";

        //notificación
        public const int PlantcodiNotificacion = 163;

        //proceso automatico
        public const int PrcscodiEjecutarCargaFTPPronRER = 43;
        public const string PrcsmetodoEjecutarCargaFTPPronRER = "EjecutarCargaFTPPronRER";

        public const string KeyHtrabajoRERUrlWS = "HtrabajoRERUrlWS";
        public const string KeyHtrabajoRERUrlClienteCore = "HtrabajoRERUrlClienteCore";

        public const string KeyHtrabajoRERPathArchivosClienteCore = "HtrabajoRERPathArchivosClienteCore";
        public const string KeyHtrabajoRERPathArchivosTemporal = "HtrabajoRERPathArchivosTemporal";
        public const string KeyHtrabajoRERUmbralIni = "HtrabajoRERUmbralIni";
        public const string KeyHtrabajoRERUmbralFin = "HtrabajoRERUmbralFin";

        #endregion

        #region CDISPATCH
        //lectura
        public const int LectcodiReprogDiario = 5;
        public const int LectcodiEjecutadoHisto = 6;

        //siproceso
        public const int PrcscodiEjecutarCargaHtrabajo = 115;
        public const string PrcsmetodoEjecutarCargaHtrabajoSicoes = "EjecutarCargaHtrabajoEnSicoes";

        //notificación
        public const int PlantcodiNotificacionExito = 82;
        public const int PlantcodiNotificacionError = 83;

        //Variables correo
        public const string VariableFechaProceso = "{FECHA_PROCESO}";
        public const string VariableFechaSistema = "{FECHA_SISTEMA}";
        public const string VariableAlertas = "{ALERTAS}";
        public const string VariableErrores = "{ERRORES}";
        #endregion
    }

    public class FilaExcelConcepto
    {
        public int Row { get; set; }
        public int NumItem { get; set; }
        public int Concepcodi { get; set; }
        public string Concepdesc { get; set; }
        public string Concepnombficha { get; set; }
        public string Concepabrev { get; set; }
        public string Concepdefinicion { get; set; }
        public string Concepunid { get; set; }
        public string Conceptipo { get; set; }
        public string StrConceptipolong1 { get; set; }
        public string StrConceptipolong2 { get; set; }
        public string Concepfichatec { get; set; }
        public string StrCatecodi { get; set; }
        public string Catenomb { get; set; }
        public string StrConcepfecmodificacion { get; set; }
        public string Concepusumodificacion { get; set; }
        public decimal? Concepliminf { get; set; }
        public decimal? Conceplimsup { get; set; }
        public string StrConcepliminf { get; set; }
        public string StrConceplimsup { get; set; }

        public int Catecodi { get; set; }
        public int? Conceptipolong1 { get; set; }
        public int? Conceptipolong2 { get; set; }
        public DateTime Concepfecmodificacion { get; set; }
    }
}
