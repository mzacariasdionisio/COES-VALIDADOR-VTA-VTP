using System.Configuration;

namespace COES.Servicios.Aplicacion.PotenciaFirme
{
    public static class ConstantesPotenciaFirme
    {
        //periodos        
        public const string Abierto = "A";
        public const string Cerrado = "C";
        public const string Preliminar = "P";

        //potencia firme
        public const int ParametroTraerBD = -3;
        public const int ParametroDefecto = -2;
        public const string Aprobado = "A";
        public const string Generado = "G";
        public const int GrupoIncremental = 1;

        public const int FactorIndispFortuita = 0;
        public const int FactorPresencia = 1;

        public const int EsVersionGenerado = 0;
        public const int EsVersionValidado = 1;

        public const string PrefijoPftotpf = "Pftotpf";
        public const string PrefijoPftotpe = "Pftotpe";
        public const string PrefijoPftotpfpor = "Pftotpfpor";

        //Cuadro 
        public const int CuadroIndFort = 1;
        public const int CuadroIndProg = 2;
        public const int CuadroIndProgHidro = 3;
        public const int CuadroFp = 4;
        public const int CuadroCog = 5;
        public const int CuadroRerNC = 6;
        public const int CuadroPFirme = 7;
        public const int CuadroPFirmeEmp = 8;
        public const int CuadroRerHistorico = 9;
        public const int CuadroTodo = 10;

        //Detalle
        public const int TipoDetAnual = 1;
        public const int TipoDetMensual = 2;

        //Tipo Recurso
        public const int RecursoPGarantizada = 1;
        public const int RecursoPAdicional = 2;
        public const int RecursoContratosCV = 3;
        public const int RecursoFactorIndispFortuita = 5;
        public const int RecursoFactorPresencia = 6;

        //exportación
        public const string FormatoPotenciaGarantizada = "PlantillaPF_PotenciaGarantizada.xlsx";
        public const string FormatoPotenciaAdicional = "PlantillaPF_PotenciaAdicional.xlsx";
        public const string FormatoFactorPresencia = "PlantillaPF_FactorPresencia.xlsx";
        public const string FormatoIndisponibilidadFortuita = "PlantillaPF_FactorIndisponibilidadFortuita.xlsx";
        public const string FormatoContratoCV = "PlantillaPF_ContratoCV.xlsx";

        //Calor Útil
        public const int IdFormatoCalorUtil = 123;

        public const string MesIniHistoricoRER = "11 2016";

        //03 2021: Fin histórico inicial (puesta en marcha Potencia Firme)
        //05 2021: Fin histórico RE-19007 (Actualización manual de Cálculo de Potencia Firme de las Centrales RER Huambos y Dunas)
        //02 2023: Fin histórico REQ 2023-001712 (Actualización manual del valor de medidores de Centrales C.E. MARCONA, C.E. CUPISNIQUE, C.E. TALARA, C.E. TRES HERMANAS)
        public readonly static string MesFinHistoricoRER = ConfigurationManager.AppSettings["PotenciaFirmeFechaFinHistoricoRER"].ToString();
    }
}
