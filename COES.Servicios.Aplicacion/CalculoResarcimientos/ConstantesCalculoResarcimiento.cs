using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CalculoResarcimientos
{
    public class ConstantesCalculoResarcimiento
    {
        public const int ParametroDefecto = -1;

        public const string EstadoProceso = "P";
        public const string EstadoCulminado = "C";
        public const string TextoEstadoProceso = "Proceso";
        public const string TextoEstadoCulminado = "Culminado";
        public const string PeriodoSemestral = "Semestral";
        public const string ArchivoPtoEntrega = "PuntoEntrega.xlsx";
        public const string ArchivoImportacionSuministrador = "PuntoEntregaSuministrador.xlsx";
        public const string MonedaSoles = "S";
        public const string IdPeriodoSemestral = "S";
        public const string IdPeriodoTrimestral = "T";
        public const string RutaExportacionInsumos = "Areas/CalculoResarcimiento/Reporte/";
        public const string ArchivoIngresoTransmision = "IngresosTransmisionSemestral.xlsx";
        public const string ArchivoPuntoEntrega = "PuntoEntregaPeriodo.xlsx";
        public const string ArchivoErroresInterrucpcion = "Errores.xlsx";
        public const string ArchivoPuntoEntregaSuministrador = "SuministradoresPorPuntoEntrega.xlsx";
        public const string ArchivoEventosCOES = "EventosCOES.xlsx";
        public const string PlantillaInterrupcion = "PlantillaInterrupcion.xlsm";
        public const string FormatoCargaInterrupcion = "FormatoCargaInterrupcion.xlsm";
        public const string FormatoCargaInterrupcionInsumo = "FormatoCargaInterrupcionInsumo.xlsm";             
        public const string ArchivoImportacionInterrupcion = "ArchivoImportacionInterrupciones.xlsm";
        public const string ArchivoImportacionInterrupcionInsumo = "AcchivoImportacionInterrupcionInsumo.xlsm";
        public const string ArchivoImportacionConsolidado = "ArchivoImportacionInterrupciones.xlsx";        
        public const string PlantillaRechazoCarga = "PlantillaRechazoCarga.xlsm";
        public const string FormatoCargaRechazoCarga = "FormatoCargaRechazoCarga.xlsm";
        public const string ArchivoImportacionRechazoCarga = "ArchivoImportacionRechazoCarga.xlsm";
        public const string PlantillaObservacion = "PlantillaObservacion.xlsm";
        public const string FormatoCargaObservacion = "FormatoCargaObservacion.xlsm";
        public const string ArchivoImportacionObservacion = "ArchivoImportacionObservacion.xlsx";
        public const string PlantillaRespuesta = "PlantillaRespuesta.xlsm";
        public const string FormatoCargaRespuesta = "FormatoCargaRespuesta.xlsm";
        public const string ArchivoImportacionRespuesta = "ArchivoImportacionRespuesta.xlsm";
        public const string ArchivoConsolidadoInterrupcion = "ConsolidadoPuntodeEntrega.xlsx";
        public const string ArchivoConsolidadoRechazoCarga = "ConsolidadoRechazoCarga.xlsx";
        public const string PlantillaConsolidadoInterrupcion = "PlantillaConsolidadoInterrupcion.xlsx";
        public const string PlantilaaConsolidadoRechazoCarga = "PlantillaConsolidadoRechazoCarga.xlsx";
        public const string PlantillaCargaPtoEntregaSuministrador = "PlantillaCargaPtoEntregaSuministrador.xlsx";
        public const string PlantillaMedicionEvento = "PlantillaMedicion.xlsx";
        public const string FormatoCargaMedicionEvento = "FormatoCargaMedicion.xlsx";
        public const string ArchivoImportacionMedicion = "ArchivoImportacionMedicion.xlsx";
        public const string PlantillaCargaInterrupcionInsumo = "PlantillaCargaInterrupcionInsumo.xlsm";
        public const string Todos = "T";
        public const string RutaBaseResarcimientos = "LocalDirectory";
        public const string FolderResarcimientos = "Resarcimientos/";
        public const string PlantillaNotificacionInterrupcion = "PlantillaNotificacionInterrupcion.xlsx";
        public const string PlantillaNotificacionRechazoCarga = "PlantillaNotificacionRechazoCarga.xlsx";
        public const string PlantillaNotificacionRespuesta = "PlantillaNotificacionRespuesta.xlsx";
        public const string PlantillaNotificacionObservacion = "PlantillaNotificacionObservacion.xlsx";
        public const string ArchivoNotificacionInterrupcion = "NotificacionInterrupcionSuministro_{0}.xlsx";
        public const string ArchivoNotificacionRechazoCarga = "NotificacionRechazoCarga_{0}.xlsx";
        public const string ArchivoNotificacionRepuesta = "NotificacionRespuestaObservacion_{0}.xlsx";
        public const string ArchivoNotificacionObservacion = "NotificacionObservacion_{0}.xlsx";


        public const string TipoClienteLibre = "L";
        public const string TipoClienteRegulado = "R";
        public const string TextoClienteLibre = "Libre";
        public const string TextoClienteRegulado = "Regulado";
        public const string TextoSi = "Si";
        public const string TextoNo = "No";
        public const string EnvioTipoInterrupcion = "S";
        public const string EnvioTipoRechazoCarga = "R";
        public const string EnvioTipoObservacion = "O";
        public const string EnvioTipoRespuesta = "T";
        public const string EnvioTipoDeclaracion = "D";
        public const string EnvioIngresoTransmision = "I";
        public const string ArchivoInterrupcion = "InterrupcionSuministro_{0}.{1}";
        public const string TemporalInterrupcion = "TemporalInterrupcion_{0}.{1}";
        public const string ArchivoIngreso = "IngresoTransmision_{0}.{1}";
        public const string TemporalIngreso = "TemporalIngresoTransmision.{0}";
        public const string ArchivoObservacion = "Observacion_{0}.{1}";
        public const string ArchivoRespuesta = "Repuesta_{0}.{1}";
        public const string ValidacionNoActualizacionInterrupcion = "Solo pueden modificar la interrupción cuando la conformidad del responsable sea NO y la conformidad de suministrador sea SI.";
        public const string ValidarActualizacionInterrupcion = "Debe actualizar algún dato de la interrupcíón dado que colocó SI en la conformidad de suministrador.";
        public const string ArchivoSustentoObservacionZip = "SustentoObservacionesComprimido.zip";
        public const string ArchivoEvidenciaInterrupcionesZip = "EvidenciasInterrupcionesComprimido.zip";
        public const string ArchivoSustentoRespuestas = "SustentoRespuestasComprimido.zip";
        public const string FolderSustentoObservacion = "SustentoObservacion";
        public const string FolderEvidenciaInterrupcion = "EvidenciasInterrupcion";
        public const string FolderSustentoRespuesta = "SustentoRespuesta";

        public const int AccionNuevo = 1;
        public const int AccionEditar = 2;

        public const int ReporteExcelConsolidadoInterrupcionesSuministro = 1;
        public const int ReporteExcelCumplimientoEnvíoInterrupciónSuministros = 2;
        public const int ReporteExcelCumplimientoEnvíoObservaciones = 3;
        public const int ReporteExcelCumplimientoEnvíoRespuestasObservaciones = 4;
        public const int ReporteExcelInterrupcionesFuerzaMayor = 5;
        public const int ReporteExcelContrasteInterrupcionesSuministro = 6;
        public const int ReporteExcelInterrupcionesEnControversia = 7;
        public const int ReporteExcelComparativoSemestralTrimestral  = 8;

        public const int ReporteWordInformeFinal = 9;

        public const int ReporteZipInterrupcionesPorAgenteResponsable  = 10;
        public const int ReporteZipInterrupcionesPorSuministrador = 11;

        public const int TipoEmpresasDistribuidoras = 2;
        public const int TipoEmpresasGeneradoras = 3;

        public const int ComparativoSemestralRespectoTrimestral = 1;
        public const int ComparativoTrimestralRespectoSemestral = 2;

        public const int TablasPE = 1;
        public const int TablasRC = 2;

        public const int HabilitacionIngresoDatosPuntoEntrega = 1;
        public const int HabilitacionIngresoDatosRechazoCarga = 2;
        public const string FuenteIngresoIntranet = "I";
        public const string FuenteIngresoExtranet = "E";
        public const int TipoNotificacionEliminacion = 1;
        public const int TipoNotificacionNuevo = 2;
        public const int TipoNotificacionActualizacion = 3;
        public const int TipoNotificacionNuevoActualizacion = 4;
        //public const string RutaBaseArchivoResarcimiento = @"\\coes.org.pe\archivosapp\webapp\";
        public static readonly string RutaBaseArchivoResarcimiento = ConfigurationManager.AppSettings["RutaBaseArchivoResarcimiento"];
         
        //public const string RutaBaseArchivoResarcimiento = @"D:\ArchivosApp\Webapp\";
        public const string RutaResarcimientos = "Resarcimientos/";

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual_usuario_Resarcimientos.rar";
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        //constantes para fileServer
        public const string FolderRaizResarcimientosModuloManual = "Resarcimientos\\";
        public const int CantidadRegistrosFormato = 1200;
        public const string ExtensionZip = "zip";

    }

    public class InterrupcionSuministroPE
    {
        public int SuministradorId { get; set; }
        public int? PuntoEntregaId { get; set; }
        public int? NivelTensionId { get; set; }
        public int? CausaId { get; set; }

        public int? Responsable1Id { get; set; }
        public int? Responsable2Id { get; set; }
        public int? Responsable3Id { get; set; }
        public int? Responsable4Id { get; set; }
        public int? Responsable5Id { get; set; }

        public string Responsable1CompCero { get; set; }
        public string Responsable2CompCero { get; set; }
        public string Responsable3CompCero { get; set; }
        public string Responsable4CompCero { get; set; }
        public string Responsable5CompCero { get; set; }
        public string Responsable1DispFinal { get; set; }
        public string Responsable2DispFinal { get; set; }
        public string Responsable3DispFinal { get; set; }
        public string Responsable4DispFinal { get; set; }
        public string Responsable5DispFinal { get; set; }
        public string ARDF1 { get; set; }
        public string ARDF2 { get; set; }
        public string ARDF3 { get; set; }
        public string ARDF4 { get; set; }
        public string ARDF5 { get; set; }
        

        public int? InterrupcionTipoId { get; set; } //contraste

        public string Suministrador { get; set; }
        public int? Correlativo { get; set; }
        public string TipoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string PuntoEntregaBarraNombre { get; set; }
        public string NumSuministroClienteLibre { get; set; }
        public string NivelTensionNombre { get; set; }
        public int? AplicacionLiteral { get; set; }
        public decimal? EnergiaPeriodo { get; set; }
        public string IncrementoTolerancia { get; set; }
        public string TipoNombre { get; set; }
        public string CausaNombre { get; set; }
        public decimal? Ni { get; set; }
        public decimal? Ki { get; set; }
        public string TiempoEjecutadoIni { get; set; }
        public string TiempoEjecutadoFin { get; set; }
        public string TiempoProgramadoIni { get; set; }
        public string TiempoProgramadoFin { get; set; }
        public string Responsable1Nombre { get; set; }
        public decimal? Responsable1Porcentaje { get; set; }
        public string Responsable2Nombre { get; set; }
        public decimal? Responsable2Porcentaje { get; set; }
        public string Responsable3Nombre { get; set; }
        public decimal? Responsable3Porcentaje { get; set; }
        public string Responsable4Nombre { get; set; }
        public decimal? Responsable4Porcentaje { get; set; }
        public string Responsable5Nombre { get; set; }
        public decimal? Responsable5Porcentaje { get; set; }
        public string CausaResumida { get; set; }
        public decimal? EiE { get; set; }
        public decimal? Resarcimiento { get; set; }
        public decimal? AgenteResp1 { get; set; }
        public decimal? AplicacionAResp1 { get; set; }
        public decimal? AgenteResp2 { get; set; }
        public decimal? AplicacionAResp2 { get; set; }
        public decimal? AgenteResp3 { get; set; }
        public decimal? AplicacionAResp3 { get; set; }
        public decimal? AgenteResp4 { get; set; }
        public decimal? AplicacionAResp4 { get; set; }
        public decimal? AgenteResp5 { get; set; }
        public decimal? AplicacionAResp5 { get; set; }

        public DateTime? FechaEjecIniMinuto { get; set; } //contraste
        public DateTime? FechaProgramadoIniMinuto { get; set; } //contraste
        public DateTime? FechaProgramadoFinMinuto { get; set; } //contraste
        public string FechaProgramadoIniMinutoDesc { get; set; } //contraste
        public string FechaProgramadoFinMinutoDesc { get; set; } //contraste
        public string FechaEjecIniMinutoDesc { get; set; } //contraste        
        public DateTime? FechaEjecFin { get; set; } //contraste

        public int InterrupcionId { get; set; }
        public string Observacion { get; set; }
        public string CamposContraste { get; set; }
    }

    public class InterrupcionSuministroRC
    {
        public int SuministradorId { get; set; }
        public int? BarraId { get; set; }
        public int? EventoId { get; set; }

        public int? Responsable1Id { get; set; }
        public int? Responsable2Id { get; set; }
        public int? Responsable3Id { get; set; }
        public int? Responsable4Id { get; set; }
        public int? Responsable5Id { get; set; }

        public string Suministrador { get; set; }
        public int? Correlativo { get; set; }
        public string TipoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string BarraNombre { get; set; }
        public string AlimentadorNombre{ get; set; }
        public decimal? Enst { get; set; }
        public string EventoNombre { get; set; }
        public string Comentario { get; set; }
        public string TiempoEjecutadoIni { get; set; }
        public string TiempoEjecutadoFin { get; set; }
        public decimal? Pk { get; set; }
        public string Compensable { get; set; }
        public decimal? Ens { get; set; }
        public decimal? Resarcimiento { get; set; }
        public string AgResponsable1Nombre { get; set; }
        public string AgResponsable2Nombre { get; set; }
        public string AgResponsable3Nombre { get; set; }
        public string AgResponsable4Nombre { get; set; }
        public string AgResponsable5Nombre { get; set; }
        public decimal? AgResponsable1Porcentaje { get; set; }        
        public decimal? AgResponsable2Porcentaje { get; set; }        
        public decimal? AgResponsable3Porcentaje { get; set; }       
        public decimal? AgResponsable4Porcentaje { get; set; }        
        public decimal? AgResponsable5Porcentaje { get; set; }
        public decimal? AgResponsable1USD { get; set; }
        public decimal? AgResponsable2USD { get; set; }
        public decimal? AgResponsable3USD { get; set; }
        public decimal? AgResponsable4USD { get; set; }
        public decimal? AgResponsable5USD { get; set; }
        public decimal? AplicacionAResp1 { get; set; }
        public decimal? AplicacionAResp2 { get; set; }
        public decimal? AplicacionAResp3 { get; set; }
        public decimal? AplicacionAResp4 { get; set; }
        public decimal? AplicacionAResp5 { get; set; }
        public string DispFinalAResp1 { get; set; }
        public string DispFinalAResp2 { get; set; }
        public string DispFinalAResp3 { get; set; }
        public string DispFinalAResp4 { get; set; }
        public string DispFinalAResp5 { get; set; }
        public string DFAR1 { get; set; }
        public string DFAR2 { get; set; }
        public string DFAR3 { get; set; }
        public string DFAR4 { get; set; }
        public string DFAR5 { get; set; }
    }

    public class EventoRC
    {
        public int EventoId { get; set; }

        public int? AgResponsable1Id { get; set; }
        public int? AgResponsable2Id { get; set; }
        public int? AgResponsable3Id { get; set; }
        public int? AgResponsable4Id { get; set; }
        public int? AgResponsable5Id { get; set; }

        public string EventoDescripcion { get; set; }
        public string Fecha { get; set; }        
        public string AgResponsable1Nombre { get; set; }
        public string AgResponsable2Nombre { get; set; }
        public string AgResponsable3Nombre { get; set; }
        public string AgResponsable4Nombre { get; set; }
        public string AgResponsable5Nombre { get; set; }
        public decimal? AgResponsable1Porcentaje { get; set; }
        public decimal? AgResponsable2Porcentaje { get; set; }
        public decimal? AgResponsable3Porcentaje { get; set; }
        public decimal? AgResponsable4Porcentaje { get; set; }
        public decimal? AgResponsable5Porcentaje { get; set; }
        public string Comentario { get; set; }

    }

    public class MontoTotalResarcimiento
    {
        public int EmpresaId { get; set; }

        public string EmpresaTipo { get; set; }
        public string EmpresaNombre { get; set; }        
        public decimal? ResarcimientoIS { get; set; }
        public decimal? ResarcimientoRC { get; set; }
        public decimal? ResarcimientoTotal { get; set; }
        public decimal? ResarcimientoTotalDF { get; set; }
        public decimal? ResarcimientoFallasTx { get; set; }

    }

    public class LimiteIngreso
    {
        public int IngresoId { get; set; }
        public int EmpresaId { get; set; }

        public string EmpresaNombre { get; set; }
        public decimal? IngresoTransmisionSoles { get; set; }
        public decimal? IngresoTransmisionDolares { get; set; }
        public decimal? Tope { get; set; }
        public decimal? ResarcimientoTotal { get; set; }
        public decimal? ResarcimientoFallasTx { get; set; }
        public string Limite { get; set; }
        public decimal? Ajuste { get; set; }
    }

    public class CumplimientoEnvioInterrupcion
    {
        public int SuministradorId { get; set; }

        public string SuministradorNombre { get; set; }        
        public string FechaUltimoEnvio { get; set; }
        public int? NumInterrupcionesEnPlazo { get; set; }
        public int? NumInterrupcionesFueraPlazo { get; set; }
        public string Mensaje { get; set; }
        public bool ConMensaje { get; set; }
    }

    public class CumplimientoEnvioObservacion
    {
        public int AgenteResponsableId { get; set; }

        public string AgenteResponsableNombre { get; set; }
        public string FechaUltimoEnvio { get; set; }
        public int? NumInterrupcionesConformidadRespNo { get; set; }
    }

    public class CumplimientoEnvioRespuestaObservacion
    {
        public int SuministradorId { get; set; }

        public string SuministradorNombre { get; set; }
        public string FechaUltimoEnvio { get; set; }
        public int? NumObservacionesTotales { get; set; }
        public int? NumObservacionesSinResponder { get; set; }
    }

    public class InterrupcionSuministroIntranet
    {
        public int SuministradorId { get; set; }
        public int? PuntoEntregaId { get; set; }
        public int? NivelTensionId { get; set; }        
        public int? CausaId { get; set; }
        public int NumeroAgentesResponsables { get; set; }

        public int? Responsable1Id { get; set; }
        public int? Responsable2Id { get; set; }
        public int? Responsable3Id { get; set; }
        public int? Responsable4Id { get; set; }
        public int? Responsable5Id { get; set; }

        public string Suministrador { get; set; }
        public int? Correlativo { get; set; }
        public string TipoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string PuntoEntregaBarraNombre { get; set; }
        public string NumSuministroClienteLibre { get; set; }
        public string NivelTensionNombre { get; set; }
        public int? AplicacionLiteral { get; set; }
        public decimal? EnergiaPeriodo { get; set; }
        public string IncrementoTolerancia { get; set; }
        public string TipoNombre { get; set; }
        public string CausaNombre { get; set; }
        public decimal? Ni { get; set; }
        public decimal? Ki { get; set; }
        public string TiempoEjecutadoIni { get; set; }
        public string TiempoEjecutadoFin { get; set; }
        public string TiempoProgramadoIni { get; set; }
        public string TiempoProgramadoFin { get; set; }
        public string Responsable1Nombre { get; set; }
        public decimal? Responsable1Porcentaje { get; set; }
        public string Responsable2Nombre { get; set; }
        public decimal? Responsable2Porcentaje { get; set; }
        public string Responsable3Nombre { get; set; }
        public decimal? Responsable3Porcentaje { get; set; }
        public string Responsable4Nombre { get; set; }
        public decimal? Responsable4Porcentaje { get; set; }
        public string Responsable5Nombre { get; set; }
        public decimal? Responsable5Porcentaje { get; set; }
        public string CausaResumida { get; set; }
        public decimal? EiE { get; set; }
        public decimal? Resarcimiento { get; set; }

        public bool RegistroEnControversiaSum { get; set; }
        public bool RegistroEnControversiaResp { get; set; }

        public string Resp1ConformidadResponsable { get; set; }
        public string Resp1Observacion { get; set; }
        public string Resp1DetalleObservacion { get; set; }
        public string Resp1Comentario1 { get; set; }
        public string Resp1ConformidadSuministrador { get; set; }
        public string Resp1Comentario2 { get; set; }
        public string Resp2ConformidadResponsable { get; set; }
        public string Resp2Observacion { get; set; }
        public string Resp2DetalleObservacion { get; set; }
        public string Resp2Comentario1 { get; set; }
        public string Resp2ConformidadSuministrador { get; set; }
        public string Resp2Comentario2 { get; set; }
        public string Resp3ConformidadResponsable { get; set; }
        public string Resp3Observacion { get; set; }
        public string Resp3DetalleObservacion { get; set; }
        public string Resp3Comentario1 { get; set; }
        public string Resp3ConformidadSuministrador { get; set; }
        public string Resp3Comentario2 { get; set; }
        public string Resp4ConformidadResponsable { get; set; }
        public string Resp4Observacion { get; set; }
        public string Resp4DetalleObservacion { get; set; }
        public string Resp4Comentario1 { get; set; }
        public string Resp4ConformidadSuministrador { get; set; }
        public string Resp4Comentario2 { get; set; }
        public string Resp5ConformidadResponsable { get; set; }
        public string Resp5Observacion { get; set; }
        public string Resp5DetalleObservacion { get; set; }
        public string Resp5Comentario1 { get; set; }
        public string Resp5ConformidadSuministrador { get; set; }
        public string Resp5Comentario2 { get; set; }
        public string DesicionControversia { get; set; }
        public string ComentarioFinal { get; set; }

    }

    public class DatoPorSuministrador
    {
        public int SuministradorId { get; set; }
        public string SuministradorNombre { get; set; }
        public int? NumResponsablesTablaHorizontalPE { get; set; }
        public int? NumResponsablesTablaHorizontalRC { get; set; }
        public int? NumFilasTablaHorizontalPE { get; set; }
        public int? NumFilasTablaHorizontalRC { get; set; }
        public List<RegistroWord> ListaPEHorizontal { get; set; }
        public List<RegistroWord> ListaPEVertical { get; set; }
        public List<RegistroWord> ListaRCHorizontal { get; set; }
        public List<RegistroWord> ListaRCVertical { get; set; }
        public Totales TotalPE3Menos { get; set; }
        public Totales TotalRC3Menos { get; set; }
    }

    public class Totales
    {
        public decimal? SumTotalAgenteResp1 { get; set; }
        public decimal? SumTotalAplicacionAResp1 { get; set; }
        public decimal? SumTotalAgenteResp2 { get; set; }
        public decimal? SumTotalAplicacionAResp2 { get; set; }
        public decimal? SumTotalAgenteResp3 { get; set; }
        public decimal? SumTotalAplicacionAResp3 { get; set; }
    }

    public class RegistroWord
    {                
        public string NombreCliente { get; set; }
        public string BarraNombre { get; set; }
        public int numResponsables { get; set; }

        public int? Responsable1Id { get; set; }
        public string Responsable1Nombre { get; set; }
        public decimal? SumAgenteResp1 { get; set; }
        public decimal? SumAplicacionAResp1 { get; set; }

        public int? Responsable2Id { get; set; }
        public string Responsable2Nombre { get; set; }
        public decimal? SumAgenteResp2 { get; set; }
        public decimal? SumAplicacionAResp2 { get; set; }

        public int? Responsable3Id { get; set; }
        public string Responsable3Nombre { get; set; }
        public decimal? SumAgenteResp3 { get; set; }
        public decimal? SumAplicacionAResp3 { get; set; }

        public int? Responsable4Id { get; set; }
        public string Responsable4Nombre { get; set; }
        public decimal? SumAgenteResp4 { get; set; }
        public decimal? SumAplicacionAResp4 { get; set; }

        public int? Responsable5Id { get; set; }
        public string Responsable5Nombre { get; set; }
        public decimal? SumAgenteResp5 { get; set; }
        public decimal? SumAplicacionAResp5 { get; set; }
        public List<string> ListaDisposicionFinalR1 { get; set; }
        public List<string> ListaDisposicionFinalR2 { get; set; }
        public List<string> ListaDisposicionFinalR3 { get; set; }
        public List<string> ListaDisposicionFinalR4 { get; set; }
        public List<string> ListaDisposicionFinalR5 { get; set; }
        public decimal? FactorR1 { get; set; }
        public decimal? FactorR2 { get; set; }
        public decimal? FactorR3 { get; set; }
        public decimal? FactorR4 { get; set; }
        public decimal? FactorR5 { get; set; }
       
    }
}