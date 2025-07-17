using System;
using System.Collections.Generic;
using System.Configuration;

namespace COES.Servicios.Aplicacion.Eventos.Helper
{
    /// <summary>
    /// Constantes para el modulo de eventos
    /// </summary>
    public class ConstantesEvento
    {
        public const string Turno1 = "1";
        public const string Turno2 = "2";
        public const string Turno3 = "3";
        public const string FiltroFechaEvento = " and ( evenini >= TO_DATE('{0}','YYYY-MM-DD HH24:MI:SS') and evenini < TO_DATE('{1}','YYYY-MM-DD HH24:MI:SS')) ";
        public const string FormatoFecha = "yyyy-MM-dd";
        public const string FormatoFechaExtendido = "yyyy-MM-dd HH:mm:ss";
        public const string SI = "S";
        public const string NO = "N";
        public const string EmailList = "ListaCorreoEvento";
        public const string NombreCorreo = "NombreCorreoEvento";
        public const string AsuntoEmailEvento = "Se ha producido el Evento: {0} a las {1}";
        public const string RutaEnlaceExtranet = "EventoExtranetURL";
        public const string InformePreliminar = "P";
        public const string InformeFinal = "F";
        public const string InformeComplementario = "C";
        public const string InformeArchivos = "A";
        public const string InformePreliminarInicial = "I";
        public const string Linea = "L";
        public const string Unidad = "U";
        public const string Transformador = "T";
        public const string Celda = "C";
        public const string InformeFileName = "INFORME_ANEXO_{0}.{1}";
        public const string TextoHasta = "Hasta: ";
        public const string TextoDespues = "Después de: ";
        public const string TextoEnPlazo = "Dentro del plazo";
        public const string TextoFueraPlazo = "Fuera del plazo";
        public const string TextoCargo = "Cargó";
        public const string TextoInformeFinalizado = "Finalizado";
        public const string TextoInformePendiente = "Pendiente";
        public const string TextoInformeElaboracion = "En elaboración";
        public const string TextoInformeRevisado = "Revisado";
        public const string EstadoPediente = "P";
        public const string EstadoFinalizado = "F";
        public const string EstadoRevisado = "R";
        public const string EstadoAprobado = "A";
        public const string EstadoRechazado = "D";
        public const string TipoRegistroDesconexion = "D";
        public const string TipoRegistroMalaCalidad = "C";
        public const string TipoRegistroInterrupcion = "I";
        public const string AreaSCO = "Centro Control COES";
        public const string InformeConsolidadoSEV = "Consolidado SEV";
        public const string EnPlazo = "P";
        public const string FueraPlazo = "F";
        public const int TipoEventoEvento = 4;
        public const int TipoEventoFalla = 5;
        public const string TextoLogNuevo = "Creación del evento";
        public const string TextoLogActualizacion = "Actualización del evento";
        public const string TextoLogLlenadoInterrupcion = "Registro de interrupción";
        public const string TextoLogEliminacionInterrupcion = "Eliminación de interrupción";
        public const string TextoLogEdicionInterrupcion = "Modificación de interrupción";
        public const string TextoLogConvertirAFinal = "Conversión a final";
        public const string TextoLogConvertirBitacora = "Conversión a bitácora";
        public const string TablaEvento = "EVE_EVENTO";
        public const string TablaInformeFalla = "EVE_INFORMEFALLA";
        public const string TablaInformeFallaN2 = "EVE_INFORMEFALLA_N2";
        public const string TablaEmails = "EVE_MAILS";
        public const string TablaIEODCuadro = "EVE_IEODCUADRO";
        public const string TablaHoraOperacion = "EVE_HORAOPERACION";
        public const string CarpetaInformeFallaN1 = "InformedePerturbaciones";
        public const string CarpetaInformeFallaN2 = "InformedePerturbacionesN2";
        public const string CarpetaInformeMinisterio = "InformeMinisterio";
        public const string RutaBitacora = "Uploads\\Bitacora\\";

        //Relacion de Area usuario y subcausas
        public const string AreacoesParaVisualizacion = "1,3,4,5,7,8";
        public const int RelacionActivo = 1;
        public const int RelacionInactivo = 0;

        #region Mejoras CTAF
        public readonly static int PlazoMinIPI = Convert.ToInt32(ConfigurationManager.AppSettings["PlazoMinIPI"].ToString());
        public readonly static int PlazoMinIF = Convert.ToInt32(ConfigurationManager.AppSettings["PlazoMinIF"].ToString());
        public readonly static string FileSystemExtranet = ConfigurationManager.AppSettings["FileSystemExtranet"].ToString();
        public readonly static string FileSystemSCO = ConfigurationManager.AppSettings["FileSystemSco"].ToString();
        public readonly static int FormatoSco = Convert.ToInt32(ConfigurationManager.AppSettings["FormatoSco"].ToString());
        public readonly static string FileSystemSev = ConfigurationManager.AppSettings["FileSystemSev"].ToString();
        public readonly static string FechaFinSem1 = ConfigurationManager.AppSettings["FechaFinSem1"].ToString();
        public readonly static string FechaInicioSem2 = ConfigurationManager.AppSettings["FechaInicioSem2"].ToString();
        public readonly static string FechaFinSem2 = ConfigurationManager.AppSettings["FechaFinSem2"].ToString();
        #endregion
        public readonly static string SubCarpetaCtaf = "CTAF\\Informe CTAF\\";
        public readonly static string SubCarpetaCtafPublica = "CTAF\\Información a Publicar\\";
        public readonly static string SubCarpetaInformeTecnico = "CTAF\\Informe Tecnico\\";
        public readonly static int MaxCaractAF = Convert.ToInt32(ConfigurationManager.AppSettings["MaxCaractAF"].ToString());
        public const string FormatoFecha2 = "dd/MM/yyyy";

        #region Migraciones 2024
        public const int EvenclasecodiEjecutados = 1;
        public const int EvenclasecodiProgramadoDiario = 2;
        public const int EvenclasecodiProgramadoSemanal = 3;
        public const int EvenclasecodiProgramadoMensual = 4;
        public const int EvenclasecodiProgramadoAnual = 5;
        public const int EvenclasecodiProgramadoAjusteDiario = 6;
        public const int EvenclasecodiEjecutadoDiario = 7;
        public const int EvenclasecodiEjecutadoMensual = 8;
        public const int EvenclasecodiProgramadoAnualMensual = 9;
        public const int EvenclasecodiProgramadoAnualMensualSemanal = 10;
        public const int EvenclasecodiProgramadoMensualSemanalDiario = 11;
        public const int EvenclasecodiProgramadoSemanalDiario = 12;
        #endregion

        public static List<int> GetIdLinea()
        {
            return new List<int> { 8 };
        }

        public static List<int> GetIdTransformador()
        {
            return new List<int> { 9, 10 };
        }

        public static List<int> GetIdInterruptor()
        {
            return new List<int> { 16 };
        }

        public static List<int> GetIdCelda()
        {
            return new List<int> { 6 };
        }

        public static List<int> GetIdUnidades()
        {
            return new List<int> { 2, 3, 36, 38 };
        }
    }

    /// <summary>
    /// Clase para manejar las horas de la exportación
    /// </summary>
    public class HoraExcel
    {
        public int Hora { get; set; }
        public int Minuto { get; set; }
        public decimal Valor { get; set; }
        public int IdEquipo { get; set; }
        public int IdGrupo { get; set; }
        public decimal? Automatico { get; set; }
        public int? IdEquipoPadre { get; set; }
        public int? IdGrupoDet { get; set; }
        public int restoMin { get; set; }
        public int valida { get; set; }

        public List<HoraExcel> ListaHoras()
        {
            List<HoraExcel> list = new List<HoraExcel>();
            list.Add(new HoraExcel { Hora = 0, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 1, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 1, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 2, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 2, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 3, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 3, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 4, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 4, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 5, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 5, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 6, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 6, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 7, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 7, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 8, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 8, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 9, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 9, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 10, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 10, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 11, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 11, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 12, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 12, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 13, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 13, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 14, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 14, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 15, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 15, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 16, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 16, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 17, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 17, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 18, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 18, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 19, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 19, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 20, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 20, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 21, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 21, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 22, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 22, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 23, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 23, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 0, Minuto = 0 });

            return list;

        }
    }

    public class ConstantesOperacionesVarias
    {
        public const int EvenClase = 4;
        public const int EvenSubcausa = 200;
        public const int RestriccionesOperativas = 205;
        public const int EvenclasecodiPdiario = 2;
        public const int Subcausacodicongestion = 201;
        public const int SubcausacodiPotenciaFija = 337;
        public const int SubcausacodiPotenciaMax = 338;
        public const int SubcausacodiPotenciaMin = 339;
        public const int SubcausacodiPlenacarga = 340;


        public const int FamcodiGenHidro = 2;
        public const int FamcodiGenTermico = 3;
        public const int FamcodiCentHidro = 4;
        public const int FamcodiCentTermico = 5;
        public const int FamcodiGenSolar = 36;
        public const int FamcodiCentSolar = 37;
        public const int FamcodiGenEolico = 38;
        public const int FamcodiCentEolico = 39;
        public const int FamcodiNoDefinido = -1;
        public const int EquicodiNoDefinido = -1;
        public const string EstadoActivo = "ACTIVO";
        public const string Horizonte = "Horizonte";
        public const string TipoOperacion = "TipoOperacion";
        public const string ParametroTodos = "0";
        public const string EventoEjecutado = "1";
    }

    public class ConstantesEnviarCorreos
    {
        public const int ModuloEnviarCorreos = 17;
        public const int ResultadoPruebaAleatoria = 1;
        public const int EvenSubcausa = 202;
        public const int EvenSubcausaNuevoRegistro = 322; //valor por defecto al crear un nuevo registro: Re-programa (opción más usada)
        public const string RutaCorreo = "Uploads\\";
    }

    public class ConstantesPruebasAleatorias
    {
        int ModuloCorreo = 17;
        int PlantillaCorreoResultado = 5;
        int CodigoPlantillaCorreoResultado = 1;
    }

    #region "SGOCOES Func A"
    public class ConstantesAnalisisFallas
    {
        public const string FolderUploadRutaCompleta = @"\\coes.org.pe\archivosapp\web\";

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual_usuario_CTAF_Mejoras.rar";
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        //constantes para fileServer
        public const string FolderRaizCTAFModuloManual = "CTAF\\";
    }
    #endregion

    #region Aplicativo Extranet CTAF

    public class ConstantesExtranetCTAF
    {
        public const int ModcodiCTAF = 32; //Solicitudes

        public const int FdatcodiCTAFExtranet = 12;
        public const int FdatcodiCTAFSolicitudes = 16;

        public const string FormatoInterrupciones = "PlantillaCTAF_FormatoInterrupciones.xlsx";
        public const string FormatoInterrupciónPorActivaciónERACMF = "PlantillaCTAF_FormatoInterrupciónPorActivaciónERACMF.xlsx";
        public const string FormatoReducciónDeSuministros = "PlantillaCTAF_FormatoReducciónDeSuministros.xlsx";

        public const string RutaReportes = "Areas/Eventos/Reporte/";
        public const string PlantillaReporteInterrupciones = "PlantillaCTAF_ReporteInterrupciones.xlsx";
        public const string PlantillaReporteInterrupcionPorActivacionERACMF = "PlantillaCTAF_ReporteInterrupciónPorActivaciónERACMF.xlsx";
        public const string PlantillaReporteReduccionDeSuministros = "PlantillaCTAF_ReporteReducciónDeSuministros.xlsx";

        public const string PlantillaReporteWordInterrupcionPorActivacionERACMF = "PlantillaCTAF_ReporteInterrupciónPorActivaciónERACMF.docx";
        public const string PlantillaReporteWordInterrupciones = "PlantillaCTAF_ReporteInterrupciones.docx";
        public const string PlantillaReporteWordReduccionDeSuministros = "PlantillaCTAF_ReporteReducciónDeSuministros.docx";

        public const string PlantillaReporteSolicitud = "PlantillaCTAF_ReporteProcesoAsignaciónResponsabilidad.xlsx";

        public enum Fuentedato
        {
            InterrupcionActivacionERACMF = 13,
            Interrupcion = 14,
            ReduccionSuministros = 15
        }

        //configuracion de parámetros
        public const int IdParametroConfiguracionPlazo = 18;
        public const string ValorParametroEnPlazo = "EN_PLAZO";
        public const string ValorParametroFinPlazo = "FIN_PLAZO";
        public const int NumeroMaxSegundosFechaIniExtranet = 180; //a la fecha de interrupcion se le suma n segundos, el agente no puede cargar más de ese rango

        public const int IdProcesoConfiguracionHoraNotificacion = 23;

        //Notificacion de carga de informacion
        public const int PlantcodiNotificacionCarga = 102;

        //Notificacion de Solicitudes
        public const int PlantcodiNotificacionEstadoSolicitudes = 103;
        public const int PlantcodiNotificacionSolicitudesPendientes = 104;

        //Notificacion envío de información VTP
        public const int PlantcodiNotificacionEnvioExtranet = 166; 

        //Estado Solicitud
        public const string Pendiente = "P";

        //Notificacion Diaria de Solicitudes Pendientes
        public const string PrcsmetodoAlertaDiariaSolicitudesPendientes = "EnviarCorreoAutomaticoDiarioSolicitudes";

        public readonly static bool FlagEnviarNotificacionCargaEvento = ConfigurationManager.AppSettings["FlagNotificarCargaInformacionDeAgente"].ToString() == "S";
        public readonly static bool FlagEnviarNotificacionEstadoSolicitudes = ConfigurationManager.AppSettings["FlagNotificarEstadosSolicitudes"].ToString() == "S";
        public readonly static bool FlagEnviarNotificacionSolicitudesPendientes = ConfigurationManager.AppSettings["FlagNotificarSolicitudesPendientes"].ToString() == "S";

        //Flags que se utilizan para utilizar el aplicativo en modo de pruebas
        public readonly static string ListaEmailAdminEventosTo = ConfigurationManager.AppSettings["ListaEmailAdminEventosTo"].ToString();
        public readonly static string ListaEmailAdminEventosCC = ConfigurationManager.AppSettings["ListaEmailAdminEventosCC"].ToString();

        //Flags que se utilizan para utilizar el aplicativo en modo de pruebas estado solicitud
        public readonly static string ListaEmailAdminSolicitudTo = ConfigurationManager.AppSettings["ListaEmailAdminSolicitudesTo"].ToString();
        public readonly static string ListaEmailAdminSolicitudCC = ConfigurationManager.AppSettings["ListaEmailAdminSolicitudesCC"].ToString();

        //public const para  en control de archivos  Archivos (upload y download)";
        public const string MensajesFile = "Mensajes\\";
        public const string EventosFile = "Eventos\\AplicativoExtranetCTAF\\";
        //public const string Eventos = "Eventos\\";
        public const string SNombreCarpetaMensaje = "Mensajes";
        public const string SNombreCarpetaEventos = "Eventos";
        public const string SNombreCarpetaTemporal = "Temporal";
        public const string SNombreCarpetaTemporal2 = "Temporal2";
        public const string SNombreCarpetaInforFinal = "InfFinal";
        public const string SNombreCarpetaOtrosArchivos = "OtrosArchivos";
        public const string SNombreCarpetaObservArchivos = "ObservArchivos";

        //Nombre del  modulo que se maneja
        public const string SModuloEventos = "Eventos";

        //Formato de reportes
        public const int TipoReporteTotal = 1;
        public const int MaxNumDigitos = 6;

        public const bool TieneFormatoNumeroEspecialMW = true;
        public const bool EsNumeroTruncadoMW = true;
        public const bool EsNumeroRedondeadoMW = false;
        public const int DigitosParteDecimalMW = 3;

        public const bool TieneFormatoNumeroEspecialDuracion = false;
        public const bool EsNumeroTruncadoDuracion = false;
        public const bool EsNumeroRedondeadoDuracion = true;
        public const int DigitosParteDecimalDuracion = 2;

        public const string f = "f";
        public const string Df = "Df";

        public const decimal FactorAnchoColumExcel = 5.384M;
        public const decimal FactorAltoFilaExcel = 29M;

        public const decimal FactorAnchoColumWord = 0.02651m; // 0.0368m;
        public const decimal FactorAltoFilaWord = 0.02651m;

        public const int EracmfRpt01TotalDatos = 1;
        public const int EracmfRpt02Resumen = 2;
        public const int EracmfRpt03MalasActuaciones = 3;
        public const int EracmfRpt04Menores3Min = 4;
        public const int EracmfRpt05Reportaron0 = 5;
        public const int EracmfRpt06NoReportaron = 6;
        public const int EracmfRpt07Demoras = 7;
        public const int EracmfRpt08Decision = 8;
        public const int EracmfRpt09Resarcimiento = 9;
        public const int InterrupcionRpt09Resarcimiento = 9;

        public const int InterrupRpt02DemoraRestablecimiento = 13;

        public const int ReduccionRpt01 = 15;

        public const string RptTipoFuente = "Calibri";
        public const string RptColor = "#bfbfbf";
        public const int RptTamanio = 8;
    }

    public class ConstantesEracmf
    {
        public const int FilaInicialExcelEracmf = 0;

        public const string Empresa = "EMPRESA";
        public const string Zona = "ZONA";
        public const string CodRele = "COD. RELE";
        public const string Marca = "MARCA";
        public const string Modelo = "MODELO";
        public const string Serie = "NRO SERIE";
        public const string Subestacion = "SUBESTACION";
        public const string NivelTension = "NIVEL TENSION KV(KV)";
        public const string CircAlimentador = "CIRCUITO ALIMENTADOR";
        public const string CodInterruptor = "CODIGO INTERRUPTOR";
        public const string NEtapa = "NUM. ETAPA";
        public const string ArranqueU = "ARRANQUE (Hz) R. UMBRAL";
        public const string TiempoU = "TIEMPO (seg) R. UMBRAL";
        public const string ArranqueD = "ARRANQUE (Hz) R. DERIVADA";
        public const string Dfdt = "DF/DT (Hz/s) R. DERIVADA";
        public const string TiempoD = "TIEMPO (seg) R. DERIVADA";
        public const string MaxRegistrada = "MAXIMA (MW) D. REGISTRADA";
        public const string MedRegistrada = "MEDIA (MW) D. REGISTRADA";
        public const string MinRegistrada = "MINIMA (MW) D. REGISTRADA";
        public const string Referencia = "D. REFERENCIA (MW)";
        public const string Suministrador = "SUMINISTRADOR";
        public const string Observaciones = "OBSERVACIONES";
        public const string FecImplem = "FECHA DE IMPLEMENTACION";
        public const string FecIngreso = "FECHA DE INGRESO";
        public const string FecRetiro = "FECHA DE RETIRO";
        public const string TipoRegistro = "TIPO REGISTRO";

        public const string MensajeValidacionNumero = "No es un valor numérico";

    }

    public class EmpresasTipo
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }

    }

    public class SolicitudesPendientes
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int NumSolicitudes { get; set; }
    }

    public class EmpresaReporte
    {
        public string EmpresaSICCOES { get; set; }
        public string TipoEmpresa { get; set; }
        public string EmpresaERACMF { get; set; }
        public string CodigoOsinergmin { get; set; }
        public string Usuario { get; set; }
        public string Fecha { get; set; }
        public int Emprcodi { get; set; }
        public int Afemprcodi { get; set; }

    }

    /// <summary>
    /// Objeto que almacena los reportes de la información de la Extranet CTAF
    /// </summary>
    public class ReporteInterrupcion
    {
        public int TipoReporte { get; set; }

        public int IdEmpresa { get; set; }
        public string NombEmpresa { get; set; }
        public string Zona { get; set; }
        public string Suministro { get; set; }
        public string Subestacion { get; set; }
        public decimal Potencia { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFinal { get; set; }
        public decimal Duracion { get; set; }
        public decimal DuracionHoras { get; set; }
        public decimal EnergiaNoSuministrada { get; set; }
        public string Observaciones { get; set; }

        public string Funcion { get; set; }
        public string Etapa { get; set; }
        public decimal? Etapaf1 { get; set; }
        public decimal? Etapaf2 { get; set; }
        public decimal? Etapaf3 { get; set; }
        public decimal? Etapaf4 { get; set; }
        public decimal? Etapaf5 { get; set; }
        public decimal? Etapaf6 { get; set; }
        public decimal? Etapaf7 { get; set; }
        public decimal? EtapaDf1 { get; set; }
        public decimal? EtapaDf2 { get; set; }
        public decimal? EtapaDf3 { get; set; }
        public decimal? TotalZona { get; set; }
        public decimal RedSumDE { get; set; }
        public decimal RedSumA { get; set; }
        public decimal Reduccion { get; set; }
        public int NumFila { get; set; }
        public bool TieneDemoraCeldaPintada { get; set; }
        public string ColorDuracion { get; set; }
        public bool ColorEtapaf1 { get; set; }
        public bool ColorEtapaf2 { get; set; }
        public bool ColorEtapaf3 { get; set; }
        public bool ColorEtapaf4 { get; set; }
        public bool ColorEtapaf5 { get; set; }
        public bool ColorEtapaf6 { get; set; }
        public bool ColorEtapaf7 { get; set; }
        public bool ColorEtapaDf1 { get; set; }
        public bool ColorEtapaDf2 { get; set; }
        public bool ColorEtapaDf3 { get; set; }

        public decimal SumaEtapaf1 { get; set; }
        public decimal SumaEtapaf2 { get; set; }
        public decimal SumaEtapaf3 { get; set; }
        public decimal SumaEtapaf4 { get; set; }
        public decimal SumaEtapaf5 { get; set; }
        public decimal SumaEtapaf6 { get; set; }
        public decimal SumaEtapaf7 { get; set; }
        public decimal SumaEtapaDf1 { get; set; }
        public decimal SumaEtapaDf2 { get; set; }
        public decimal SumaEtapaDf3 { get; set; }

        public decimal SumaTotal { get; set; }
        public string Evencodi { get; set; }
        public string Evenini { get; set; }
        public string CodigoNombreEmpresa { get; set; }
        public int Afecodi { get; set; }
        public int EVENCODI { get; set; }
        public DateTime EVENINI { get; set; }
        public int Intsumcodi { get; set; }
    }

    #endregion

    public class ConstantesEnviarCorreoS
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
        public const int PlantcodiReporteCMg = 117;
        public const int PlantcodiReporteHO = 110;
        public const int PlantcodiReportePremCMg = 111;
        public const int PlantcodiReportePremHO = 112;
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
