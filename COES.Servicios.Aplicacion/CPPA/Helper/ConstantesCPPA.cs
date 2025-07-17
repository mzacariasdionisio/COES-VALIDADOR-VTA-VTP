using System.Drawing.Printing;
using System.Security.Policy;

namespace COES.Servicios.Aplicacion.CPPA.Helper
{
    /// <summary>
    /// Constantes de PrimasRER
    /// </summary>
    public class ConstantesCPPA
    {
        public static readonly int[] numMeses = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        public static readonly string[] mesesDesc = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre" };
        public static readonly string[] mesesDescCorta = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Set", "Oct", "Nov", "Dic" };
        public static readonly string[] mesesDescNum = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
        public static readonly string[] ajustesRevision = { "A1", "A2", "A3", "A4", "A5" };
        public static readonly string[] estadosRevision = { "A", "C", "X" };
        public static readonly string[] tipoEmpresa = { "Transmisión", "Distribución", "Generación", "Usuario Libre" };

        public const int numero9 = 9;
        public const int numero4 = 4;
        public const int numero15 = 15;
        public const int numero60 = 60;
        public const int numero96 = 96;
        public const int numero1997 = 1997;
        public const int numero1000 = 1000;
        public const int numero2000 = 2000;

        public const int number2025 = 2025;
        public const string A1 = "A1";
        public const string revisionNormal = "Normal";
        public const string revisionRevision = "Revisión";
        public const string revisionRevision1 = "Revisión 1";
        public const string ultimoNO = "N";
        public const string ultimoSI = "S";

        public const string tipoNuevo = "N";
        public const string tipoEditar = "E";
        public const string tipoCopiarParametros = "C";
        public const string tipoAnular = "X";

        public const string tipoInusmoEnergiaActivaEjecutada = "1";
        public const string tipoInusmoEnergiaActivaProgramada = "4";
        public const string tipoInusmoCostoMarginalEjecutada = "2";
        public const string tipoInusmoCostoMarginalProgramada = "3";

        public const int numeroTipoEmpresaTransmision = 1;
        public const int numeroTipoEmpresaDistribucion = 2;
        public const int numeroTipoEmpresaGeneracion = 3;
        public const int numeroTipoEmpresaUsuarioLibre = 4;

        public const string anInternalApplicationErrorHasOccurred = "Ha ocurrido un error interno del aplicativo";

        public const string todos = "TODOS";
        public const string estadoRevisionAbierto = "A";
        public const string estadoRevisionCerrado = "C";
        public const string estadoRevisionAnulado = "X";
        public const string estadoRevisionTodos = "'A','C','X'";
        public const string estadoParametroActivo = "A";
        public const string estadoEmpresaActivo = "A";
        public const string estadoCentralActivo = "A";
        public const string estadoPublicacionSi = "S";
        public const string estadoPublicacionNo = "N";

        public const string descEstadoAbierto = "Abierto";
        public const string descEstadoCerrado = "Cerrado";
        public const string descEstadoAnulado = "Anulado";

        public const string descTipoNuevo = "Nuevo";
        public const string descTipoEditar = "Editar";
        public const string descTipoCopiarParametros = "Copiar parámetros";
        public const string descTipoAnular = "Anular";

        public const string tipoRegistroMDEjecutado = "E";
        public const string tipoRegistroMDProgramado = "P";

        public const string fechasEjecutadas = "FE";
        public const string fechasProgramadas = "FP";

        public const string operacionCrear = "Crear";
        public const string operacionActualizar = "Actualizar";

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual Usuario_CPPA.rar"; 
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        //constantes para fileServer
        public const string FolderRaizCPPAModuloManual = "Cálculo Porcentajes ppto. anual\\";

        // Insumos
        public const int idInsumoMedidoresGeneracion = 1;
        public const int idInsumoCostoMarginalLVTEA = 2;
        public const int idInsumoCostoMarginalPMPO = 3;
        public const int idInsumoGeneraciónProgramadaPMPO = 4;
        public const string insumoMedidoresGeneracion = "1";
        public const string insumoCostoMarginalLVTEA = "2";
        public const string insumoCostoMarginalPMPO = "3";
        public const string insumoGeneraciónProgramadaPMPO = "4";
        public static readonly int[] ordenInsumos = { 1, 2, 3, 4 };
        public static readonly string[] insumosDesc = { "Medidores generación 15 min.", "Costo marginal LVTEA 15 min.", "Costo marginal PMPO 15 min.", "Generación programada PMPO 15 min." };
        public static readonly string[] insumosJuntoDesc = { "MedidoresGeneración15Min.", "CostoMarginalLVTEA15Min.", "CostoMarginalPMPO15Min.", "GeneraciónProgramadaPMPO15Min." };
        public static readonly string[] insumosTituloDesc = { "Registro de medidores de generación (MWh)", "Costos Marginales LVTEA (S/. /MWh)", "Costos Marginales PMPO (S/. /MWh)", "Registro de generación programada PMPO (MWh)" };
        public static readonly string[] insumosNombArchivoDesc = { "RegistroMedidoresGeneracion", "CostosMarginalesLVTEA", "CostosMarginalesPMPO", "RegistroGeneracionProgramada" };

        public const string AppExcel = "application/vnd.ms-excel";
        public const string TextPlain = "text/plain";
        public const string NombreArchivoError = "Error.txt";
        public const string NoSePudoDescargarElArchivo = "No se pudo descargar el archivo.";

        public const string invocadoPorExtranet = "Extranet";
        public const string invocadoPorIntranet = "Intranet";
        #region Excel
        public const string AlineaColumnaIzquierda = "left";
        public const string AlineaColumnaCentro = "center";
        public const string AlineaColumnaDerecha = "right";
        public const string AlineaColumnaJustificada = "justify";

        public const string TipoColumnaString = "string";
        public const string TipoColumnaInteger = "integer";
        public const string TipoColumnaDouble = "double";

        public const int SeccionTitulo = 2;
        public const int SeccionCabeceraoPie = 1;
        public const int SeccionCuerpo = 0;

        public const string ColorFondoAzul = "blue";

        public const string SumaColumnas = "columnas";
        public const string SumaFilas = "filas";
        #endregion

        //FORMATO DE FECHAS
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        public const string FormatoFechaHoraFull = "dd/MM/yyyy HH:mm:ss";

        //RUTAS
        public const string ReporteDirectorio = "ReporteTransferencia";

        //CANTIDAD INTERVALOS
        public const int cantidad96Intervalos = 96;

        // TABLAS CU09
        public const string RutaArchivosSddp = "RutaArchivosSddpCppa";
        public static string tablaLeccsvTemp = "RER_LECCSV_TEMP";
        public static string tablaGerCsvDet = "CPA_GERCSVDET";

        #region CU03
        public const string ReporteCPPA = "ReporteCPPA";
        public const string tipoEmpresaGeneradora = "G";
        public const string tipoEmpresaDistribuidora = "D";
        public const string tipoEmpresaUsuarioLibre = "U";
        public const string tipoEmpresaTransmisora = "T";
        public const string eActivo = "A";
        public const string eAnulado = "X";
        public const string tipoNormal = "N";
        public const string tipoEspecial = "E";
        public const string tipoA1 = "A1";
        #endregion

        //Estilo de los mensajes
        public const string MsgInfo = "info";
        public const string MsgSuccess = "success";
        public const string MsgWarning = "warning";
        public const string MsgError = "error";

        #region CU05
        public const string accionNuevo = "N";
        public const string accionEditar = "E";
        public const string accionAnular = "X";
        #endregion

        // Nuevas constantes
        public const string AppWord = "application/vnd.ms-word";
        public const string AppPdf = "application/pdf";
        public const string AppCSV = "application/CSV";
        public const string AppXML = "application/XML";

        public const string ReporteDemanda = "ReporteDemanda";
        public const string ReporteTransmisores = "ReporteTransmisores";

        // Rutas para los servicios web
        public const string urlApiCppa = "CalculoPorcentajePpto/";//"https://localhost:7001/api/CalculoPorcentajePpto/";
        public const string urlApiSeguridadRefresh = "Usuario/refresh";//"https://localhost:7168/api/Usuario/refresh";
        public const string urlApiSeguridadAuthenticate = "Usuario/Authenticate";//"https://localhost:7168/api/Usuario/Authenticate";

        // Estado de Revisión
        public const string Abierto = "A";
        public const string Cerrado = "C";
        public const string Anulado = "X";
    }
}