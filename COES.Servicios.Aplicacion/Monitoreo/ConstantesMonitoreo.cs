using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Monitoreo
{
    /// <summary>
    /// Clase constantes utilizadas en el Monitoreo MME
    /// </summary>
    public class ConstantesMonitoreo
    {
        public const int TipoFormulaCuota = 2;
        public const int TipoFormulaCuotaErrorBT = 102;
        public const int TipoFormulaHHI = 4;
        public const int TipoFormulaHHIErrorBT = 104;
        public const int TipoFormulaHHICuotaMercado = 21;
        public const int TipoFormulaIOP = 7;
        public const int TipoFormulaRSD = 8;
        public const int TipoFormulaRSDErrorBT = 108;
        public const int TipoFormulaILE = 11;
        public const int TipoFormulaILEErrorBT = 111;
        public const int TipoFormulaIMU = 12;
        public const int TipoFormulaIMUErrorBT = 112;
        public const int TipoFormulaIRT = 18;
        public const int TipoFormulaIRTErrorBT = 118;

        public const int TipoFormulaMWEjec = 1;
        public const int TipoFormulaMWProg = 6;
        public const int TipoFormulaTotalPotencia = 3;
        public const int TipoFormulaMaximaDemanda = 5;
        public const int TipoFormulaCMEjec = 9;
        public const int TipoFormulaCMProg = 19;
        public const int TipoFormulaCV = 10;
        public const int TipoFormulaPotGrupoEjec = 13;
        public const int TipoFormulaPotGrupoProg = 14;
        public const int TipoFormulaCongestion = 16;
        public const int TipoFormulaEnlaceTrans = 17;

        public const int TipoDataGrupocodiModo = 1;
        public const int TipoDataCV = 2;

        public const int TipoFormularioPorAprobar = 1;
        public const int TipoFormularioNoPortalWeb = 0;
        public const int TipoFormularioSiPortalWeb = 0;
        public const int TipoFormularioPublicado = 2;
        public const int TipoFormularioNopublicado = 1;
        public const int TipoFormularioNoVigente = 3;
        public const int TipoCausaCongestion = 201;

        public const int CambioMmmdatomwejec = 1;
        public const int CambioMmmdatomwprog = 2;
        public const int CambioMmmdatocmgejec = 3;
        public const int CambioMmmdatocmgprog = 4;
        public const int CambioCvar = 5;
        public const int CambioEmprcodi = 6;
        public const int CambioMogrupocodi = 7;
        public const int CambioBarrcodi = 8;
        public const int CambioCnfbarcodi = 9;

        public const int CodigoS = 1;
        public const int CodigoHHI = 2;
        public const int CodigoIOP = 3;
        public const int CodigoRSD = 4;
        public const int CodigoILE = 5;
        public const int CodigoIMU = 6;
        public const int CodigoIRT = 7;

        public const string AbrevCuotaMercado = "IMME-1";
        public const string AbrevIndHeHi = "IMME-2";
        public const string AbrevPivotal = "IMME-3";
        public const string AbrevResidual = "IMME-4";
        public const string AbrevLerner = "IMME-5";
        public const string AbrevImu = "IMME-6";
        public const string AbrevRed = "IMME-7";

        public const string RptExcelCuotaMercado = "(IMME-1) CUOTA DE MERCADO (S%)";
        public const string RptExcelIndHeHi = "(IMME-2) ÍNDICE DE HERFINDAHL Y HIRSCHMAN (HHI)";
        public const string RptExcelPivotal = "(IMME-3) ÍNDICE DE OFERTA PIVOTAL (IOP)";
        public const string RptExcelResidual = "(IMME-4) ÍNDICE DE OFERTA RESIDUAL (RSD)";
        public const string RptExcelLerner = "(IMME-5) ÍNDICE DE LERNER (ILE)";
        public const string RptExcelImu = "(IMME-6) ÍNDICE DEL MARGEN PRECIO - COSTO (IMU)";
        public const string RptExcelRed = "(IMME-7) ÍNDICE DE MONITOREO DE LA RED DE TRANSMISIÓN (IRT)";
        public const string RptExcelGeneralIndicadores = "RCD N°209-2017-OS CD Reporte_IMME_";

        public const string Directorio = "Areas\\Monitoreo\\Reporte\\";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string ExtensionExcel = ".xlsx";
        public const string RutaExcelIndicadores = "ReporteMonitoreo";
        public const string PorAprobar = "Por Aprobar";
        public const string Publicado = "Publicado";

        public const int CongesgdespachoEstadoActivo = 1;

        public const int PrGrupoBarraprogEstadoActivo = 1;
        public const string CatecodiRelacionGrupoDespachoBarraProg = "4,6";
        public const string CatecodiGrupoDespacho = "3,5";

        public const int FactorHHI = 10000;
        public const string ColorHHITendenciaCero = "#3CBC8D";
        public const string ColorHHITendenciaUno = "#FF022C";

        public const int ValorIOPEsPivotal = 1;
        public const int ValorIOPNoPivotal = 0;
        public const string ColorIOPEsPivotal = "#00B0F0";
        public const string ColorIOPNoPivotal = "#F79646";
        public const string TituloCuotaMercado = "CUOTA DE MERCADO";
        public const string ColorCmgEmpresaPar = "#9BC2E6";
        public const string ColorCmgEmpresaImpar = "#D9E1F2";
        public const string ColorLogError = "#FFC000";

        public const int PosicionInicial = 0;
        public const int PosicionUnoCuotaMercado = 10;
        public const int PosicionDosCuotaMercado = 20;
        public const int PosicionTresCuotaMercado = 51;
        public const int PosicionCuatroCuotaMercado = 100;

        public const int PosicionUnoHhi = 999;
        public const int PosicionDosHhi = 1800;
        public const int PosicionTresHhi = 1801;
        public const int PosicionCuatroHhi = 2000;
        
        public const string TituloBarraCuota = "PARTICIPACIÓN DIARIA DE LA CUOTA DE MERCADO POR EMPRESA INTEGRANTE DEL COES ";
        public const string TituloHHI = "EVOLUCIÓN DIARIA DEL HHI POR EMPRESA INTEGRANTE DEL COES - ";
        public const string TituloPivotal = "Índice de Oferta Pivotal";
        public const string TituloResidual = "Índice de Oferta Residual";
        public const string TituloLener = "Índice de Lerner";
        public const string TituloPrecioCosto = "Índice del Margen Precio Costo";
        public const string TituloPromedioResidual = "EVOLUCIÓN PROMEDIO DE LA OFERTA RESIDUAL POR EMPRESA GENERADORA INTEGRANTE DEL COES";
        public const string TituloIndiceLener = " ILE (ÍNDICE LERNER)";
        public const string TituloIndicePrecioCosto = " IMU (ÍNDICE COSTO)";
        public const string TituloEvoluacionMediaHHI = "Evolución media horaria del indicador HHI";

        public const string PeriodoDia = "01";
        public const string PeriodoSemana = "02";
        public const string PeriodoMes = "03";

        public const string TituloHHIY = "HHI";

        public const int ReportesIndicadores = 1;
        public const int LogErroresMonitoreo = 2;
        public const int ControlCambiosMonitoreo = 3;
        public const int ReportePorcentajeErrorBandaTolerancia = 4;
        public const int Dashboard = 5;
        public const int EstadoAprobado = 2;
        public const int EstadoNovigente = 3;

    }

    /// <summary>
    /// Clase utilizada para los puntos del Grafico Dashboard
    /// </summary>
    public class PuntoGraficoMedidorGeneracionMoni
    {

        public string Empresa { get; set; }
        public int EmprCodi { get; set; }
        public int BarrCodi { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaString { get; set; }
        public decimal? ValorFuente1 { get; set; }
        public decimal?[] ListaFuente1 { get; set; }
        public decimal? ValorFuente2 { get; set; }
    }

    /// <summary>
    /// Clase para Dashboard 1 - Cuota de mercado
    /// </summary>
    public class GraficoMedidorGeneracionMoni
    {
        public int TipoUsuario { get; set; }
        public string Nombre { get; set; }
        public List<PuntoGraficoMedidorGeneracionMoni> ListaPunto { get; set; }
        public string TituloGrafico { get; set; }
        public string TituloFuente1 { get; set; }
        public string TituloFuente2 { get; set; }
        public string LeyendaFuente1 { get; set; }
        public string LeyendaFuente2 { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string DescPeriodo { get; set; }
        public string DescCentral { get; set; }
        public string ValorFuente1 { get; set; }
        public string ValorFuente2 { get; set; }
        public int TipoGrafico { get; set; }
        public List<string> NombreEmpresa { get; set; }
        public List<int> CodigoEmpresa { get; set; }
        public List<int> CodigoBarra { get; set; }
        public List<string> CategoriaFecha { get; set; }
    }

    public class ReporteControlCambios
    {
        public DateTime FechaEnvioAnt { get; set; }
        public DateTime FechaEnvioActual { get; set; }
        public DateTime Fecha { get; set; }
        public decimal? MWEjecAnt { get; set; }
        public decimal? MWEjecActual { get; set; }
        public decimal? MWProgAnt { get; set; }
        public decimal? MWProgActual { get; set; }
        public decimal? CosmarAnt { get; set; }
        public decimal? CosmarActual { get; set; }
        public decimal? CosmarProgAnt { get; set; }
        public decimal? CosmarProgActual { get; set; }
        public decimal? CvAnt { get; set; }
        public decimal? CvActual { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int Barrcodi { get; set; }
        public string Barrnombre { get; set; }
        public int Grupocodi { get; set; }
        public string Gruponomb{ get; set; }
        public DateTime FechaCreacionAnterior { get; set; }
        public DateTime FechaCreacionActual { get; set; }
        public string UsuarioCreacionAnterior { get; set; }
        public string UsuarioCreacionActual { get; set; }
    }
}
