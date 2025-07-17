using System;

namespace COES.Servicios.Aplicacion.CostoOportunidad
{
    
    public class ConstantesCostoOportunidad
    {
        
        public const int LectcodiReservaProgramada = 100;//PDO - resaghcp y resagtcp
        public const int LectcodiDespachoConReserva = 107;//Con - gerhidcp y gertercp
        public const int LectcodiDespachoSinReserva = 101;//Sin - gerhidcp y gertercp
        public const int PorcentajeRpf = 236;
        public const int PorcentajeRpfHidro = 246;
        public const int PotenciaMinima = 16;
        public const int PotenciaEfectiva = 14;
        public const int ConcepcodiRpf = 282;

        #region RSF_PR22
        public const string TipoOperacionURS = "1";
        public const string TipoReporteExtranet = "2";
        public const string TipoEquipoRPF = "3";
        public const int TipoUnidad = 1;
        public const int TipoCentral = 2;
        public const string TipoDatoSenial = "1";
        public static readonly string[] TipoDatosDesc = { "AGCLocRm/Status", "AGCStat/Status", "AGCMSRU/MvMoment", "AGCMRSL/MvMoment", "SetPnt_P/MvMoment", "P1/MvMoment" };
        public static readonly string[] TipoDatosDescFile = { "AGCLocRm-Status", "AGCStat-Status", "AGCMSRU-MvMoment", "AGCMRSL-MvMoment", "SetPnt_P-MvMoment", "P1-MvMoment" };
        public static readonly int[] TipoDatosIds = { 1, 2, 3, 4, 5, 6 };
        public static readonly string[] TipoDatoFP = { "FactorPresencia" };
        public static readonly int[] TipoDatoFPId = { 7 };
        public const int AccionCrear = 1;
        public const int AccionEditar = 2;
        public const int ImportarSeniales_Mensual_DiasFaltantes = 0;
        public const int ImportarSeniales_Mensual_Todo = 1;
        public const int ImportarSeniales_Diario = 2;
        public const int Med60VieneDeBD = 1;
        public const int Med60Calculado = 2;
        public const int ProcesoNormal = 0;
        public const int ProcesoConFactoresPresencia = 1;
        public const string ProcesoDiario = "D";
        public const string ProcesoMensual = "M";
        public const string EstadoExitoso = "E";
        public const string EstadoFallido = "F";
        public const string TieneEquipoParaRPF = "1";
        public const int IdTipoInformacionFP = 14;
        public const int IdTipoInformacionSenialSP7 = 15;
        public const string ProdiatipoDiario = "D";
        public const string ProdiatipoMensual = "M";
        public const string UsuarioProcesoAutomatico = "SISTEMA";
        public const string ProcesoAutomaticoFactorUtilizacion = "CalculoFactoresUtilizacion";
        public const string TipoReproceso = "R";
        public const string TipoManual = "M";
        public const string TipoAutomatico = "A";
        public const int ErrorValoresIniciales = 1;
        public const int ErrorExistenciaSeniales = 2;
        public const int IdRegistroBandera = 1;
        public const string EstadoHayEjecucionEnCurso = "1";
        public const string EstadoNoHayEjecucionEnCurso = "0";
        #endregion

    }

    #region Clases_RSF_PR22
    public class DiferenciaValor
    {        
        public DateTime Fecha { get; set; }
        public string FechaDesc { get; set; }
        public decimal? PeriodoProg { get; set; }
        public decimal? VariableDelta { get; set; }

    }

    public class TablasTRScada
    {
        public DateTime FechaDia { get; set; }
        public string NombreTabla { get; set; }
        public string Existe { get; set; }

    }

    public class TablaMedicion60
    {
        public int MesTabla { get; set; }
        public int AnioTabla { get; set; }
        public string NombreTabla { get; set; }
        public int TipoTabla { get; set; }

    }

    public class Medicion60XSegundo
    {
        public DateTime FechaDia { get; set; }
        public int Hora { get; set; }
        public int Minuto { get; set; }
        public int Segundo { get; set; }
        public int Canalcodi { get; set; }
        public decimal Valor { get; set; }
    }

    public class ErrorScada
    {
        public int Canalcodi { get; set; }
        public decimal? Valor { get; set; }
        public DateTime FechaHora { get; set; }
        public string Mensaje { get; set; }

    }

    public class FactorUtilizacionExcel
    {
        public DateTime FechaDia { get; set; }
        public decimal? ValorAlpha { get; set; }
        public decimal? ValorBeta { get; set; }
    }

    public class FactorUtilizacionErrorExcel
    {
        public int NumeroFilaExcel { get; set; }
        
        public string CampoExcel { get; set; }
        public string ValorCeldaExcel { get; set; }
        public string MensajeValidacion { get; set; }
    }

    #endregion
}
