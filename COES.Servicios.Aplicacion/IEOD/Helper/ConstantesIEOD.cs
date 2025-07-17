using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.IEOD
{
    public class ConstantesIEOD
    {
        public const int IdModulo = 3;
        public const int IdOrigenIEOD = 16;
        public const int IdCausaEvento = 203;
        public const string SubcausaCmgEXT = "EXT";
        public const string SubcausaCmgINT = "INT";
        public const string Controladores = "57, 'StockCombustibles/Envio', 'Consumo'-58, 'StockCombustibles/Envio', 'presiongas'-59, 'StockCombustibles/Envio', 'DisponibilidadGas'-60, 'StockCombustibles/Envio', 'quemagas'-61, '#','#'-62, 'ieod/TensionBarra','index'- 63, 'ieod/FlujoTrans','index'- 65, 'ieod/CalorUtil','index'";
        public const string ControllerFuenteDatos = "1, 'ieod/HorasOperacion','index'";
        public const int OrigLectcodiIEODpr16 = 19;
        public const int OrigLectcodiIEODpr5 = 21;
        public const int OrigLectcodiIEOD = 21;
        public const string FamCentral = "2,3,37,39";
        public const string RptExcelEquivalencia = "RptExcelEquivalencia.xlsx";
        public const string RptExcelPtoCalculado = "RptPuntoCalculado.xls";
        public const string HojaExcelPtoCalculado = "PUNTO_CALCULADO";
        public const string TituloExcelPtoCalculado = "Reporte de Puntos calculados";
        public const string NombreLogoCoes = "coes.png";
        public const string AppExcel = "application/vnd.ms-excel";

        public const int FdatcodiPadreHOP = 1;
        public const int FdatcodiHOPTermoelectrica = 4;
        public const int FdatcodiHOPTermoelectricaBiogasBagazo = 11;
        public const int FdatcodiHOPHidroelectrica = 5;
        public const int FdatcodiHOPSolar = 6;
        public const int FdatcodiHOPEolica = 7;

        public const int IdFormatoDemandaDiaria = 95;
        public const int IdFormatoDemandaMensual = 96;
        public const int IdFormatoDemandaSemanal = 71;
        public const int IdFormatoDemandaRDOPrevista = 141;//Assetec - 20221213

        public const int LectCodiDemandaDiariaProgramado = 110;
        public const int LectCodiDemandaDiariaEjecutado = 103;
        public const int LectCodiDemandaPrevistaRDO = 245;//Assetec - 20221213

        //Fuente Energia Primaria
        public const string KeyFechaIniProcesoFEnergSolar = "FechaProcesoIniFEnergPrimRerSolar";

        public const string TituloGraficoEnergiaPrimaria = "Gráfico de Fuente de Energía Primaria de las Unidades RER";
        public const string TituloGraficoCalorUtil = "Gráfico de Información de Calor Útil";
        public const string TituloGraficoDespacho = "Gráfico de Información de Despacho";
        public const string TituloGraficoTensionGener = "Gráfico de Información de Tensión de las Unidades de Generación";
        public const string TituloGraficoGeneracionRER = "Gráfico de Información de Generación RER";
        public const string IdccG = "IDCC-G_";

        //Tipoinfocodi y Hojacodi
        public const int EjecTipoInfoMVAR = 2;
        public const int EjecHojaCodiMVAR = 32;

        public const int ProgTipoInfoMVAR = 2;
        public const int ProgHojaCodiMVAR = 33;

        public const int TipoConfigcodi = 1;
        public const int TipoGrulincodi = 2;
        public const int TipoRegsegcodi = 3;

        public const int DesfaceGrupoLineaFlujoMuinimo = 1000000;//6000000
        public const int ConstanteDesfaseRegionSeguridad = 5000000;
        public const int ConstanteDesfaseGrupoLinea = 1000000;

        public const string RutaArchivos = "Areas/IEOD/Reporte/";
        public const string NombreArchivoDatos = "FormatoCargaDatos.xlsx";
        public const string NombreImportacionArchivo = "ImportacionCargaDatos.xlsx";
        public const string SessionTipoInfoCelda = "SessionTipoInfoCelda";
        public const string InterconexionesEntreSSOOSein = "InterconexionesEntreSSOOSein.xlsx";
        public const string NombreArchivoConsulta = "ConsultaDatos.xlsx";

        //Relación de barras de transferencia y área operativa
        public const string CharN = "N";
        public const string CharC = "C";
        public const string CharS = "S";
        public const string strNorte = "Norte";
        public const string strCentro = "Centro";
        public const string strSur = "Sur";
        public const string strSi = "SI";
        public const string strNo = "NO";
        public const int AccionNuevo = 1;
        public const int AccionEditar = 2;


        //Propiedad de color y termica
        public const int IDPropiedadColor = 2223;
        public const string PropiedadColorDefault = "#C6F5FB";
        public const int IDFamiliaTermica = 5;

        //Manual de usuario SCO
        public const string ArchivoManualUsuarioIntranet = "Manual_Usuario_SCO_v1.1.pdf";
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        //constantes para fileServer SCO
        public const string FolderRaizMigracionSCOModuloManual = "Migración SGOCOES Desktop a Intranet\\";

        //Manual de usuario SGI
        public const string ArchivoManualUsuarioIntranetSGI = "Manual_Usuario_SGI_v1.2.pdf";
        public const string ModuloManualUsuarioSGI = "Manuales de Usuario\\";

        //constantes para fileServer SGI
        public const string FolderRaizSGIModuloManual = "Migración SGOCOES Desktop a Intranet\\";
    }

    public class ConstantesHard
    {
        public const int IdFormatoCalorUtil = 65;
        public const int LectCodiCalorUtil = 96;
        public const int IdFormatoDespacho = 62;
        public const int LectCodiDespacho = 93;
        public const int LectCodiDespachoAntiguo = 6;
        public const int IdFormatoEnergiaPrimaria = 68;
        public const int IdFormatoEnergiaPrimariaSolar = 97;
        public const int IdFormatoEnergiaPrimariaEolicoTermico = 98;
        public const int LectCodiEnergiaPrimaria = 99;
        public const int IdFormatoFlujoTrans = 63;
        public const int LectCodiFlujoTrans = 94;
        public const int IdFormatoPotenciaAutoProd = 67;
        public const int LectCodiPotenciaAutoProd = 98;
        public const int IdFormatoTensionBarra = 66;
        public const int LectCodiTensionBarra = 97;
        public const int IdFormatoTension = 64;
        public const int LectCodiTension = 95;
        public const int IdFormatoRpf = 92;//92 Test y Produccion, 93 movisoft
    }

    public class ConstantesDesconexion
    {
        public const string TipoEmpresa = "3";
        public const int DesconexionEquipo = 341;
        public const int IdFatcodiDesconex = 2;
        public const string ArchEnvioDesconexion = "ARCH_";
        public const string RepositorioDesconexion = "RepositorioRestriccion";
        public const string RepositorioTemporalDescconexion = "RepositorioTemporalDescconexion";
    }

    public class ConstantesHorasOperacion
    {
        public const int IdFormato = 0;
        public const string TipoEmpresa = "3";
        public const int IdEmpresaTodos = 0;
        public const string FormatoFechaHora = "yyyy-MM-dd HH:mm";
        public const string FormatoOnlyHora = "HH:mm";
        public const string FormatoOnlyHoraFull = "HH:mm:ss";
        public const string CodFamilias = "4,5, 37, 39";
        public const string CodFamiliasRSF = "4,5, 37, 39, 50";
        public const string CodFamiliasGeneradores = "2,3,36,38";
        public const string CodFamiliasNoTermica = "4, 37, 39";
        public const string CodFamiliasDesconexion = "8, 9, 10,12";
        public const int IdTipoCentralTodos = -1;
        public const int IdTipoHidraulica = 4;
        public const int IdTipoTermica = 5;
        public const int IdTipoSolar = 37;
        public const int IdTipoEolica = 39;
        public const int IdGeneradorTemoelectrico = 3;
        public const int IdGeneradorHidroelectrico = 2;
        public const int IdGeneradorSolar = 36;
        public const int IdGeneradorEolica = 38;
        public const int TipoEventCodiHoraOpe = 8;
        public const string EtiquetaGrupo = "Grupo";
        public const string EtiquetaModo = "Modo";
        public const string EtiquetaListaModo = "Seleccione Unidades para el Modo de Operación";
        public const string EtiquetaListaGrupo = "Seleccione Unidades para la Central";
        public const int TipoHidrologiaCentral = 1;

        public const int ValidarEMS = 100;
        public const int ValidarScada = 101;
        public const int ValidarIntervenciones = 102;
        public const int ValidarCostoIncremental = 109;
        public const int ValidarHoraOperacion = 103;
        public const int ValidarIDCC = 104;
        public const int ValidarIDCCCalorUtil = 1041;
        public const int ValidarIDCCFenergPrimSolar = 1042;
        public const int ValidarIDCCFenergPrimEolicoTermico = 1043;
        public const int ValidarIDCCTension = 1044;
        public const int ValidarPR21 = 106;
        public const int ValidarMedidores = 105;
        public const int ValidarMedidoresActiva = 1051;
        public const int ValidarMedidoresReactiva = 1052;
        public const int ValidarMedidoresSSAA = 1053;

        public const int IdFuenteDatoEMS = 1;
        public const int IdFuenteDatoScada = 2;
        public const int IdFuenteDatoIDCCDespacho = 3;
        public const int IdFuenteDatoIDCCCalorUtil = 4;
        public const int IdFuenteDatoIDCCFenergPrim = 5;
        public const int IdFuenteDatoIDCCFenergPrimSolar = 51;
        public const int IdFuenteDatoIDCCFenergPrimEolicoTermico = 52;
        public const int IdFuenteDatoIDCCTensionGen = 6;
        public const int IdFuenteDatoPR21 = 7;
        public const int IdFuenteDatoMedidores = 8;
        public const int IdFuenteDatoMedidoresActiva = 81;
        public const int IdFuenteDatoMedidoresReactiva = 82;
        public const int IdFuenteDatoMedidoresSSAA = 83;
        public const string FuenteDatoEMS = "";
        public const string FuenteDatoScada = "";
        public const string FuenteDatoIntervenciones = "";
        public const string FuenteDatoHoraOperacion = "";
        public const string FuenteDatoIDCCDespacho = "Despacho diario ejecutado";
        public const string FuenteDatoIDCCCalorUtil = "Calor Útil";
        public const string FuenteDatoIDCCFenergPrim = "Fuente de Energía Primaria";
        public const string FuenteDatoIDCCTensionGen = "Tensión de las Unidades de Generación";
        public const string FuenteDatoPR21 = "";
        public const string FuenteDatoMedidores = "";

        public const int AlertaEmsNO = 0;
        public const int AlertaEmsSI = 1;
        public const int AlertaScadaNO = 0;
        public const int AlertaScadaSI = 1;
        public const int AlertaIntervencionNO = 0;
        public const int AlertaIntervencionSI = 1;
        public const int AlertaCostoIncrementalNO = 0;
        public const int AlertaCostoIncrementalSI = 1;

        public const int FlagFiltroTR = 1;
        public const int FlagModoOperacionEncendido = 1;
        public const decimal CostoIncrementalNoDefinido = 1000000;

        public const int TipoNuevoDiaNoHayDataAnterior = 0;
        public const int TipoNuevoDiaExitoso = 1;
        public const int TipoNuevoDiaRegistroPrevioExistente = 2;

        public const int TipoFinalizarDiaNoHayData = 0;
        public const int TipoFinalizarDiaExitoso = 1;
        public const int TipoFinalizarDiaNoExisteRegCierre = 2;

        public const string ParamEmpresaTodos = "-2";
        public const string ParamCentralTodos = "-2";
        public const string ParamEmpresaSeleccione = "-3";
        public const string ParamCentralSeleccione = "-3";
        public const string ParamModoSeleccione = "-3";
        public const string ParamTipoOperacionTodos = "-2";

        public const string EstadoActivo = "A";
        public const string HopFalla = "F";

        public const int FamLinea = 8;
        public const int FamTrafo = 9;
        public const int FamTrafo3D = 10;
        public const int HoequitipoCongestion = 1;
        public const int HoequitipoEvento = 2;
        public const int HoequitipoEventoPotenciaFija = 3;
        public const int HoequitipoEventoPotenciaMax = 4;
        public const int HoequitipoEventoPotenciaMin = 5;
        public const int HoequitipoEventoPlenacarga = 6;
        public const string HoequitipoEventoPotenciaFijaDesc = "Potencia Fija";
        public const string HoequitipoEventoPotenciaMaxDesc = "Potencia Máxima";
        public const string HoequitipoEventoPotenciaMinDesc = "Potencia Mínima";
        public const string HoequitipoEventoPlenacargaDesc = "Plena Carga";

        public const string LimTransm = "xLT";
        public const int CheckSistemaAislado = 1;
        public const string FueraServicio = "F/S";

        public const string NombreArchivoHOP = "ReporteHorasOperacion";
        public const string NombreHojaHOP = "Reporte";
        public const string TituloHojaHOP = "Reporte de Horas de Operación";
        public const string NombreHojaHOPOsinergmin = "Reporte";
        public const string TituloHojaHOPOsinergmin = "Reporte de Horas de Operación - OSINERGMIN";

        public const string TipoModoOpCicloCombinado = "CC";
        public const string TipoModoOpCicloSimple = "CS";
        public const string TipoModoOpEspecial = "E";
        public const string FlagModoEspecial = "S";

        public const int TipoListadoTodo = 1;
        public const int TipoListadoSoloTermico = 2;
        public const int TipoTemporal = 1;
        public const int FlagTipoHoModo = 1;
        public const int FlagTipoHoUnidad = 2;
        public const int FlagCalificadoSI = 1;
        public const int FlagCalificadoNO = 0;

        public const int ScadaFormatresolucion = 15;
        public const int ScadaRowPorHora = 4;
        public const int ScadaRowPorDia = 96;
        public const decimal ScadaValorFicticio = -100000;

        public const int DelayNumMinDataEms = 34; //cuando son las 23:34 se toma el ems de las 23:00 porque falta un minuto para obtener la nueva data
        public const int DelayNumMinDataScada = 39;

        public const int CruceHoNoExiste = 1;
        public const int CruceHoSiExiste = 2;
        public const int CruceHoUnidadEspecialSiExiste = 3;
        public const int CruceHoUnidadEspecialSiExisteRepartirHo = 4;
        public const string MensajeCruceHoNoExiste = "";
        public const string MensajeCruceHoSiExiste = "";
        public const string MensajeCruceHoUnidadEspecialSiExiste = "";
        public const string MensajeCruceHoUnidadEspecialSiExisteRepartirHo = "";

        public const int TipoHONormal = 0;
        public const int TipoHOUnidadEspecial = 1;

        public const int PosicionNuevo = -1;

        public const string MensajeNotificacionUnidEspecial = "El usuario Administrador ha registrado/modificado las horas de operación, favor de verificar.";
        public const int FlagUnidadEspecialAdminCreacion = 0;
        public const int FlagUnidadEspecialAdminModificacionPropio = 1;
        public const int FlagUnidadEspecialAdminModificacionFromAgente = 2;

        public const int FlagUnidadEspecialAgenteCreacion = 5;
        public const int FlagUnidadEspecialAgenteModificacionPropio = 6;
        public const int FlagUnidadEspecialAgenteModificacionFromAdmin = 7;

        public const int FlagConfirmarValIntervenciones = 1;
        public const int FlagConfirmarValidacion = 1;

        public const string HoraFinDefecto = "23:58";

        //bitacora
        public const int TipoEventoFalla = 5;
        public const int IdTipoEventoBitacora = 2;

        //Compensacion
        public const string CompOrdArrq = "S";

        //Notificacion de correo
        public const int PlantcodiNotifHoModif = 60;
        public const int PlantcodiNotifIntervencionesFS = 61;

        //Estado de Unidad de una Hora de operación
        public const int HOUnidadActivo = 1;
        public const int HOUnidadInactivo = 0;

        //Opciones
        public const int OpcionSave = 1;
        public const int OpcionUpdate = 2;

        //Centrales de Reserva Fría que deben indicar las unidades que operaron
        public static List<int> ListaEquicodiRsvFriaToRegistrarUnidad = new List<int> () { 16290, 16291 };

        //conceptos de prueba de unidad
        public const int ConcepcodiPotenciaEfectiva = 14;
        public const int ConcepcodiTiempoMinOperacion = 139;
        public const int ConcepcodiPotenciaMinima = 16;
        public const int ConcepcodiTiempoEntreArranques = 136;

        // SI_PARAMETRO_VALOR para configurar Umbral
        public const int IdParametroUmbral= 39;
    }

    public class ConstantesMotivoOperacionForzada
    {
        public const int CodigoNoDefinido = -1;
        public const string DescNoDefinido = "...";
        public const int CodigoReqPropio = 1;
        public const string DescReqPropio = "REQ. PROPIO";
        public const int CodigoSeguridad = 2;
        public const string DescSeguridad = "SEGURIDAD(R. POR PRUEBAS)";
        public const int CodigoTension = 3;
        public const string DescTension = "TENSIÓN";
        public const int CodigoEvitarArranqueParada = 4;
        public const string DescEvitarArranqueParada = "EVITAR ARRANQUE/PARADA";
        public const int CodigoEcuador = 5;
        public const string DescEcuador = "ECUADOR";
        public const int CodigoOtros = 10;
        public const string Descotros = "OTROS";
    }

    public class ConstantesTopologiaElect
    {
        public const string TipoEmpresa = "3";
        //public const string CodFamilias = "2,3,4,5, 37, 39";
        public const int IdTipoHidraulica = 4;
        public const int IdTipoTermica = 5;
        public const int IdTipoSolar = 37;
        public const int IdTipoEolica = 39;
        public const int IdTipoexcepcionninguno = 0;
        public const int IdTipoexcepcionSistAislado = 1;
        public const int IdGeneradorHidroelectrico = 2;
        public const string TipoEmpresas = "1,2,3";// Transmisión, Distribución, Generación
        public const string TipodeEquipos = "2,3,4,5,6,7,8,9,10, 37, 39";
        public const int TiporelcodiTopologia = 26;
    }

    /// <summary>
    /// Clase de los tipos de excepcion de la topologia
    /// </summary>
    public class TipoDatoTopologia
    {
        public int Codi { get; set; }
        public string DetName { get; set; }
    }

    public class ItemPanelIEOD
    {
        public int Formatcodi { get; set; }
        public int Fdatcodi { get; set; }
        public DateTime FechaPlazo { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime FechaPeriodo { get; set; }
        public int? IdEnvio { get; set; }
        public int Cumplimiento { get; set; }
        public string EnvioPlazo { get; set; }
    }


    public class ConstantesRestriccionesOper
    {
        public const string TipoEmpresa = "3";
        public const int SubcausacodiRestric = 205;
        public const int IdFatcodiRestricc = 3;
        public const string ArchEnvioRestricion = "ARCH_";

        public const string RepositorioRestriccion = "RepositorioRestriccion";
        public const string RepositorioTemporal = "RepositorioTemporal";
    }

    //inicio agregado
    public class ConstantesVerificacion
    {
        public const int IdVerifMantenimiento = 1;
        public const int IdVerifRestriccionOperativa = 2;
        public const int IdVerifEvento = 3;
        public const int IdVerifHorasOperacion = 4;
        public const int IdVerifMedidorGeneracion = 5;
        public const int IdVerifCentralSolar = 6;
    }

    public class ConstanteValidacion
    {
        public const int EstadoNoValidado = 0;
        public const int EstadoValidado = 1;
        public const int EstadoTodos = -1;
    }
    //fin agregado

    public class ConstantesEnvio
    {
        public const string FolderReporte = "Areas\\IEOD\\Reporte\\";
        public const string NombreArchivoEnvio = "ArchivoEnvio.xlsx";
        public const string NombreArchivoCumplimiento = "ArchivoCumplimiento.xlsx";

        //Tipo de Envios
        public const string ENVIO_EN_PLAZO = "P";
        public const string ENVIO_FUERA_PLAZO = "F";
        public const string ENVIO_PLAZO_DESHABILITADO = "D";
        public const string ENVIO_NO_INFORMADO = "N";

        //Panel Ieod - Cumplimiento
        public const int CumplEnviadoFueraPlazo = 0;
        public const int CumplEnviadoEnPlazo = 1;
        public const int CumplEnviadoFdat = 2;
        public const int CumplEnviadoIncompleto = 3;
    }

    public class ConstantesAmpliacion
    {
        public const int IdModulo = 3;
        public const int IdOrigenHidro = 16;
    }

    public class ConstantesEventos
    {
        public const string NombreArchivoEventos = "ArchivoEvento.xlsx";
    }

    public class ConstantesSubcausaEvento
    {
        public const int SubcausaNoIdentificado = -1;
        public const int SubcausaPorPotenciaEnergia = 101;
        public const int SubcausaAMinimaCarga = 102;
        public const int SubcausaPorTension = 103;
        public const int SubcausaPorNecesidadRPF = 104;
        public const int SubcausaPorSeguridad = 105;
        public const int SubcausaPorPruebas = 106;
        public const int SubcausaPorPruebasAleatoriasPR25 = 114;
        public const int SubcausaPorPruebaSoliTercero = 115;
        public const int SubcausaPorRsf = 320;

        public const int SubcausaPorCongeneracion = 342;
        public const int SubcausaPorManiobras = 121;
        public const int SubcausaPorReservaEspecial = 315;

        public const int SubcausaPorRestricOpTemporal = 122;

        public const string SubcausaNoIdentificadoColor = "#A9A9A9"; //el mismo color se usa cuando la hop es mayor a la fecha y hora actual
        public const string SubcausaDefaultColor = "#008000";
        public const string SubcausaPorPotenciaEnergiaColor = "#4fc3f7";
        public const string SubcausaAMinimaCargaColor = "#FAEBD7";
        public const string SubcausaPorTensionColor = "#5F9EA0";
        public const string SubcausaPorNecesidadRPFColor = "#FF7F50";
        public const string SubcausaPorRestricOpTemporalColor = "#FFFF00";
        public const string SubcausaPorSeguridadColor = "#7011AD";
        public const string SubcausaPorPruebasColor = "#B4F985";
        public const string SubcausaPorPruebasAleatoriasColor = "#773C00";
    }

    /// <summary>
    /// Relaciona la Fuente de Datos con el Tipo de Equipo, p.e: Horas de Operacion con Central Termicas
    /// </summary>
    public class FuenteDatosXFamilia
    {
        public int Fdatcodi { get; set; }
        public int Famcodi { get; set; }
    }

    /// <summary>
    /// Clase de Motivo Operacion Forzada
    /// </summary>
    public class MotivoOperacionForzada
    {
        public int Motcodi { get; set; }
        public string Motdesc { get; set; }
    }

    /// <summary>
    /// Clase de Tipo de Desglose de la Hora de Operación
    /// </summary>
    public class TipoDesgloseHoraOperacion
    {
        public int Subcausacodi { get; set; }
        public string Subcausadesc { get; set; }
        public int TipoDesglose { get; set; }
    }

    public class ModoOperacionPR5
    {
        public int IdModoOpeOGrupo { get; set; }
        public string Tipo { get; set; }
        public string Valor { get; set; }
        public int TipoDesglose { get; set; }
        public int Equicodi { get; set; }
    }

    public class ReporteExcelCT
    {
        public bool FlagActivo { get; set; }
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Gruponomb { get; set; }
        public double CVariable1 { get; set; }
        public string CVariable1Formateado { get; set; }
        public double CIncremental1 { get; set; }
        public string CIncremental1Formateado { get; set; }
        public double CIncremental2 { get; set; }
        public string CIncremental2Formateado { get; set; }
        public string Tramo1 { get; set; }
        public string Tramo2 { get; set; }
    }
}
