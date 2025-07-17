namespace COES.Servicios.Aplicacion.PrimasRER.Helper
{
    /// <summary>
    /// Constantes de PrimasRER
    /// </summary>
    public class ConstantesPrimasRER
    {
        public const int anioBase = 2021;
        public static readonly string[] numeroVersiones = { "0", "1", "2", "3", "4", "5" };
        public static readonly int[] mesesAnioTarifario = { 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3, 4 };
        public static readonly int[] mesesActualesAnioTarifario = { 5, 6, 7, 8, 9, 10, 11, 12 };
        public static readonly int[] mesesSiguientesAnioTarifario = { 1, 2, 3, 4 };
        public static readonly string[] mesesDesc = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
        public static readonly string[] mesesAnioTarifarioDesc = { "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre", "Enero", "Febrero", "Marzo", "Abril" };
        public static readonly int[] numeroTrimestres = { 1, 2, 3, 4 };

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual_usuario_Prima RER.rar";
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        //constantes para fileServer
        public const string FolderRaizPrimaRERModuloManual = "Prima RER\\";

        // Insumos
        public static readonly string[] insumosDesc = { "Inyección Neta 15 min.", "Costo Marginal 15 min.", "Ingreso por Potencia", "Ingreso por Cargo Prima RER", "Energía Dejada de Inyectar 15 min.", "Saldos VTA 15 min.", "Saldo VTP", "Inyección Neta Mensual 15 min." };
        public static readonly string[] versionesDesc = { "Anual", "1er Ajuste Trimestral", "2do Ajuste Trimestral", "3er Ajuste Trimestral", "4to Ajuste Trimestral", "Liquidación" };
        public static readonly string[] cabeceraInsumosDesc = {"Inyección Neta (MWh)", "Costo Marginal (S/./MWh)", "Ingreso por Potencia RER (S/.)", "Ingreso por Cargo Prima RER (S/.)", "Energia Dejada de Inyectar (MWh)"};

        public const int numero0 = 0;
        public const int numero1 = 1;
        public const int numero2 = 2;
        public const int numero3 = 3;
        public const int numero4 = 4;
        public const int numero5 = 5;
        public const int numero6 = 6;
        public const int numero15 = 15;
        public const int numero24 = 24;
        public const int numero31 = 31;
        public const int numero60 = 60;
        public const int numero96 = 96;
        public const int numero4000 = 4000;
        public const int numeroMenosUno = -1;
        public const int numeroMenosTres = -3;
        public const int numeroMenosQuince = -15;
        public const string menosUno = "-1";

        public const string horizonteMensual = "M";

        //Para AnioVersion
        public const string estadoAnioVersionAbierto = "1";
        public const string estadoAnioVersionCerrado = "0";

        //Para Centrales RER
        public const string estadoActivo = "A";

        //Para solicitudes EDI
        public const string estadoAbierto = "A";
        public const string estadoCerrado = "C";
        public const string estadoGenerado = "G";
        public const string estadoValidado = "V";
        public const string estadoFueraPlazo = "FP";
        public const string estadoEnPlazo = "EP";

        public const string eliminadoNo = "NO";
        public const string eliminadoSi = "SI";

        public const string tipoMensual = "M";
        public const string tipoRevision = "R";

        public const string tipoIntervalo0 = "0";
        public const string tipoIntervalo1 = "1";
        public const string tipoIntervalo2 = "2";

        public const int tipoReporte = 0;
        public const int tipoReporteAprobados = 1;
        public const int tipoReporteNoAprobados = 2;
        public const int tipoReporteFuerzaMayor = 3;

        public const int tipoReportePrimaRERIngresoPorPotencia = 1;
        public const int tipoReportePrimaRERIngresoPorEnergia = 2;
        public const int tipoReportePrimaREREnergiaNeta = 3;
        public const int tipoReportePrimaRERCostoMarginal = 4;
        public const int tipoReportePrimaRERFactorAjuste = 5;
        public const int tipoReportePrimaRERIngresoPorPrimaRER = 6;
        public const int tipoReportePrimaRERSaldosVTEAResumen = 7;
        public const int tipoReportePrimaRERSaldosVTEA1Trimestre = 8;
        public const int tipoReportePrimaRERSaldosVTEA2Trimestre = 9;
        public const int tipoReportePrimaRERSaldosVTEA3Trimestre = 10;
        public const int tipoReportePrimaRERSaldosVTEA4Trimestre = 11;
        public const int tipoReportePrimaRERSaldosVTP = 12;
        public const int tipoReportePrimaRERTarifaAdjudicada = 13;
        public const int tipoReportePrimaRERSaldoMensualPorCompensar = 14;

        public const string tipoResultadoInyeccionNeta = "1";
        public const string tipoResultadoCostoMarginal = "2";
        public const string tipoResultadoIngresosPotencia = "3";
        public const string tipoResultadoIngresosCargoPrimaRER = "4";
        public const string tipoResultadoEnergiaDejadaInyectar = "5";
        public const string tipoResultadoSaldosVTEA = "6";
        public const string tipoResultadoSaldosVTP = "7";
        public const string tipoResultadoInyeccionNetaMensual = "8";

        public const string descReporte = "";
        public const string descReporteAprobados = "Aprobados";
        public const string descReporteNoAprobados = "No Aprobados";
        public const string descReporteFuerzaMayor = "Fuerza Mayor";

        public const string operacionCrear = "Crear";
        public const string operacionActualizar = "Actualizar";

        public const int FenergcodiAgua = 1;
        public const int FenergcodiEolica = 9;
        public const int FenergcodiSolar = 8;
        public const int TgenercodiTermo = 2;
        public const int FenergcodiBiogas = 7;
        public const int FenergcodiGas = 2;
        public const string SI = "S";

        public const string descEstadoAbierto = "Abierto";
        public const string descEstadoCerrado = "Cerrado";
        public const string descEstadoGenerado = "Generado";
        public const string descEstadoValidado = "Validado";
        public const string descEstadoFueraPlazo = "Fuera de Plazo";
        public const string descEstadoEnPlazo = "En Plazo";
        public const string descTipoMensual = "Mensual";
        public const string descTipoRevision = "Revisión";

        public const string IdEjecutadosEJ = "1";
        public const string IdPlazoEntregaEDI = "IdPlazoEntregaEDI";
        public const string EnergiaUnidadDelimitador = ",";

        public const string AppExcel = "application/vnd.ms-excel";

        public const string origenDatosValorTipico = "VT";
        public const string origenDatosArchivoExcel = "AE";

        public const string flagDentroRango = "D";
        public const string flagFueraRango = "F";

        public const int IdCausasDeFuerzaMayorCalificadasPorOsinergmin = 6;

        public const string resultadoEstadoAprobada = "Aprobada";
        public const string resultadoEstadoNoAprobada = "No aprobada";
        public const string resultadoEstadoSolicitudDeFuerzaMayor = "Solicitud de Fuerza Mayor";

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
        public const string FormatoFechaHoraFull2 = "yyyyMMddHHmmssffff";

        //RUTAS
        public const string ReporteDirectorio = "ReporteTransferencia";
        public const string RutaArchivoEnergiaUnidad = "RutaArchivoEnergiaUnidad";
        public const string RutaArchivoSustento = "RutaArchivoSustento";
        public const string ReportePrimaRER = "ReportePrimaRER";

        //CONFIGURACION ARCHIVO
        public const string MaxSizeSustento = "MaxSizeSustentoMB";


        // PRIMAS RER 2DA ITERACCIÓN
        public const string RutaArchivosSddp = "RutaArchivosSddp";
        public static string tablaRerLeccsvTemp = "RER_LECCSV_TEMP";
        public static string tablaGerCsvDet = "RER_GERCSV_DET";

        public static string tablaRerInsumoCmTemp = "RER_INSUMO_CM_TEMP";
        public static string tablaRerInsumoDiaTemp = "RER_INSUMO_DIA_TEMP";
    }
}
