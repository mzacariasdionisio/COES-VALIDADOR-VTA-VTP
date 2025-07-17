using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Hidrologia
{
    public class ConstHidrologia
    {
        public const char SeparadorFila = '#';
        public const char SeparadorCol = ',';
        public const string FromEmail = "webapp@coes.org.pe";
        public const string FromName = "Declaración Jurada de Hidrología";
        public const string ToMail = "ToMailInterconexion";
        public const string FolderUpload = "Areas\\Hidrologia\\Uploads\\";
        public const string FolderReporte = "Areas\\Hidrologia\\Reportes\\";
        public const string NombreArchivoLogo = "coes.png";
        public const string NombreArchivoEnvio = "ArchivoEnvio.xlsx";
        public const string SesionNombreArchivo = "SesionNombreArchivoVolCMg";
        public const string SesionDatosImportacion = "SesionDatosImportacionArchivoVolCMg";

        public const int IdModulo = 3;
        public const int IdOrigenHidro = 16;
        public const int Caudal = 11;
        public const int EjectutadoTR = 66;
        public const int Ejecutado = 1;
        public const int Programado = 2;
        public const int Volumen = 14;
        public const int Altura = 40;
        public const int IdFormatoDescargaLaguna = 42;
        public const int IdFormatoVertimiento = 41;
        public const int Horas = 1;
        public const int Diario = 1;
        public const int SemanalProg = 2;
        public const int Mensual = 3;
        public const int Semanal = 5;

        public const int IdReporteAppPowelDiario = 7;
        public const int IdReporteAppPowelSem = 8;
        public const string HojaReporteDiarioInfoAppPowel = "Inf. Diaria";
        public const string HojaReporteSemanalInfoAppPowel = "Inf. Semanal";

        public const int FormatcodiHidroCPProgDiario = 109;
        public const int FormatcodiHidroCPReprogDiario = 110;
        public const int FormatcodiHidroCPProgSemanal = 111;

        //Hidrologia CMgCP
        public const string TieneConCheckExtranet = "1";
        public const string TieneSinCheckExtranet = "2";
        public const int TipoHEjecutado = 1;
        public const int TipoHProgramado = 2;

        public const decimal Hm3ToMm3 = 1000.0m; //hectometro a miles de metros cubicos

        public const int TopologiaBase = 0;
        public const int RecurcodiPlantaH = 4;
        public const int RecurcodiEmbalse = 19;

        public const string CalculoVolumenAutomatico = "1";
        public const string CalculoVolumenReproceso = "2";

        public const string ComponenteTipoCaudal = "1";
        public const string ComponenteTipoCentralAguaArriba = "2";
        public const string ComponenteTipoCaudalTurbinadoCentralAguaArriba = "8";
        public const string ComponenteTipoCentralTurbinante = "3";
        public const string ComponenteTipoVolumenEmbalse = "9";

        public const string ConfiguracionTipoCaudalExtranet = "1";
        public const string ConfiguracionTipoCaudalYupana = "2";
        public const string ConfiguracionTipoPotIDCC = "3";
        public const string ConfiguracionTipoPotTNA = "4";
        public const string ConfiguracionTipoPotYupana = "5";

        public const int FuentePotIDCC = 1;
        public const int FuentePotTNA = 2;
        public const int FuentePotYupana = 3;
        public const int FuenteCauExtEjec = 4;
        public const int FuenteCauExtProg = 5;
        public const int FuenteCauYupana= 6;

        public const int CalculoVolMedido = 0;
        public const int CalculoVolCalculado = 7;
        public const int CalculoVolMenorVmin = 8;
        public const int CalculoVolMayorVmax = 9;

        //Reporte Proyección Hidrológica - REQ 2023-000403
        public const int IdFormatoProyHidro = 140;
    }

    public class ConstantesTipoInfo
    {
        public const string TipoInfoCaudal = "Caudales";
        public const string TipoInfoPresas = "Presas";
        public const string TipoInfoVolumen = "Volumen";
    }

    public class ConstantesRpteTR
    {
        public const int CaudalVolumen = 1;
        public const int Caudales = 2;
        public const int Volumenes = 3;
    }

    public class ConstantesRbDetalleRpte
    {
        public const int Horas = 0;
        public const int Diario = 1;
        public const int SemanalProgramado = 2;
        public const int SemanalCronologico = 3;
        public const int Mensual = 4;
        public const int Anual = 5;
    }

    public class ConstantesrbTipoRpteQnVol
    {
        public const int Semanal = 1;
        public const int Mensual = 2;
        public const int Anual = 3;
    }

    public class ConstantesrbTipoRpteTR
    {
        public const int FormatoSCO = 2;
        public const int Volumenes = 3;
        public const int Caudales = 4;
    }

    public class ConstantesRptePronostico
    {
        /// <summary>
        /// Constantes para Reporte de Pronostico
        /// </summary>
        //inicio agregado
        public const int IdReporteCodiPronosticoDiario = 4;
        public const int IdReporteCodiPronosticoSemanal = 6;
        public const int IdReporteCodiHistorico = 5;
        //fin agregado
        public const int TipoReporteDiario = 1;
        public const int TipoReporteSemanal = 2;
        public const int TipoReporteDiarioPronosticoCantDia = 3;
        public const int TipoReporteDiarioHistoricoCantDia = 3;
        public const int TipoReporteSemanalPronosticoCantDia = 10;
        public const int TipoReporteSemanalHistoricoCantDia = 7;
        public const string TipoPeriodoReporteDiario = "Diario";
        public const string TipoPeriodoReporteSemanal = "Semanal";
        public const string TituloReporteDiarioPronostico = "REPORTE PROGRAMA DIARIO (PREVISTO)";
        public const string TituloReporteDiarioHistorico = "REPORTE PROGRAMA DIARIO (HISTÓRICO)";
        public const string TituloReporteSemanalPronostico = "REPORTE PROGRAMA SEMANAL (PREVISTO)";
        public const string TituloReporteSemanalHistorico = "REPORTE PROGRAMA SEMANAL (HISTÓRICO)";
        public const string HojaReporteDiarioPronostico = "Pronóstico Diario";
        public const string HojaReporteDiarioHistorico = "Histórico Diario";
        public const string HojaReporteSemanalPronostico = "Pronóstico Semanal";
        public const string HojaReporteSemanalHistorico = "Histórico Semanal";
        public const string FiltroPronosticoDiarioDesc = "DÍA ACTUAL";
        public const string FiltroHistoricoDiarioDesc = "DÍA ACTUAL";
        public const string FiltroPronosticoSemanalDesc = "SEMANA HIDROLÓGICA PREVISTA";
        public const string FiltroHistoricoSemanalDesc = "SEMANA OPERATIVA";
        public const int TipoReporteSemanalHistoricoCantSemana = -1;
        public const string LeyendaDataFaltante = "Información Faltante / Incompleta";
    }

    public class ConfiguracionDefaultEmbalseHidrologia
    {
        public List<CpRecursoDTO> ListaRecursoEmbalse { get; set; }
        public List<CpRecursoDTO> ListaRecursoPlantaH { get; set; }
        public List<MePtomedicionDTO> ListaPtoVolumen { get; set; }
        public List<MePtomedicionDTO> ListaPtoCaudal { get; set; }
        public List<MePtomedicionDTO> ListaPtoIDCC { get; set; }
        public List<EqEquipoDTO> ListaEqCentral { get; set; }
        public List<EqEquipoDTO> ListaEqTNA { get; set; }
    }

    public class ConfiguracionEmbalseHidrologia
    {
        public CmModeloEmbalseDTO Modelo { get; set; }
        public List<CmModeloComponenteDTO> ListaCaudal { get; set; }
    }

}
