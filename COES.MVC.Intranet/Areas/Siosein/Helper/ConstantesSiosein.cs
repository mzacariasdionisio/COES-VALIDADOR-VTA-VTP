using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Siosein.Helper
{
    public class ConstantesSiosein
    {
        public const string FormatFecha = "dd/MM/yyyy";
        public const string TipoEmpresa = "3"; //Generadores
        public const string HidroFamiliaCodi = "4";


        public const string HojaFormatoExcelCN = "Caudal Natural";
        public const string HojaFormatoExcelVol = "Volumenes totales";
        public const string HojaFormatoExcelCD = "Caudal Descargado";
        public const string HojaFormatoExcelRes = "Restriccion por Central";


        public const int TipoEquipoCuenca = 41;
        public const int NColumnaRestricciones = 13;
        public const int NFilasAnuales = 12;
        //public const int NFilasRestricciones = 15;
        public const int NFilasRestricciones = 25;
        public const int NfilasCaudalnaturalData = 12;
        public const int NfilasCaudalDescargadoData = 12;
        public const int NfilasVolumenTotalData = 1;
        public const int LectCodiCaudalNatural = 103;
        public const int LectCodiVolumenTotal = 104;
        public const int LectCodiCaudalDescargado = 105;
        public const int LectCodiRestricciones = 106;
        public const int TipoPtoMedMantenimientoIndis = 56;
        public const int TipoPtoMedCaudalRegante = 57;
        public const int TipoPtoMedCaudalEcologico = 58;
        public const int IdFormatoCaudalNatural = 72;
        public const int IdFormatoVolumenTotal = 73;
        public const int IdFormatoCaudalDescargado = 74;
        public const int IdFormatoRestricCentral = 75;
        public const string strFormatosPortenciaEnergia = "72,73,74,75";
        public const char SeparadorFila = '#';
        public const char SeparadorCol = ',';

        public const int IdTipoembalse = 19;

        public const int HorasRegularDia = 7;
        public const decimal CaudalMaximo = 15.84m;
        public const int DiasTotalesPeriodoEvaluacion = 183;
        public const decimal VolumenUtilReservorioDiario = 0.06m;
        public const decimal FactorPresencia = 1m;

        public const int NroCaudalNatural = 1;
        public const int NroVolumenes = 2;
        public const int NroCaudalDescargado = 3;

        public const string NombreCaudalNatural = "cbCuencaCaudalNatural";
        public const string NombreVolumen = "cbCuencaVolumen";
        public const string NombreCaudalDescargado = "cbCuencaCaudalDescargado";

        public const int IdPropiedadPotEfect = 46;

        public const int FactorPresenciaBase = 1;
        public const int FactorPresenciaMedia = 2;
        public const int FactorPresenciaPunta = 3;

        //SIOSEIN
        public const string ColorEstadoVacio = "red";
        public const string ColorEstadoMedio = "orange";
        public const string ColorEstadoTerminado = "green";

        public const int TptoSalida = 62;
        public const int Tptocaudaldescargado = 10;
        public const string Tiporelcodi = "27,28";
        public const string Tiporelcodicaudaldescargado = "29";


        public const string Famcodi = "4,5,37,39";
        public const string FamcodiTermo = "5";


        //GESTOR SIOSEIN
        public const int ModcodiGestorSiossein = 29;
        public const string usuAdmin = "jperez";

        //
        public const string RemisionEmpresa = "COES";
        public const string RemisionUser = "usuariocoes";
        public const string RemisionHost = "127.0.0.1";

        //Fuente de Datos        
        //Informe Mensual
        public const int TipoInformeMensual = 1;

        //Informe Anual
        public const int TipoInformeAnual = 2;

        public enum BloqueCmg
        {
            PuntaMaxima = 1,
            MediaMaxima = 2,
            Punta = 3,
            Media = 4,
            Base = 5,
        }

        public const string SI = "SI";
        public const string NO = "NO";

        public static Dictionary<int, string> ColorTipoMensaje = new Dictionary<int, string>()
        {
            { 0, "#FFFFFF" },
            { 3, "#1ab394" },
            { 4, "#EF5352" },
            { 5, "#F8AC59" },
            { 6, "#23c6c8" },
        };

        public enum Estado
        {
            Ok = 1,
            Error = 0
        }

        public enum Mensaje
        {
            Recibido = 1,
            Enviado = 2,
            Eliminado = 3
        }

    }

    /// <summary>
    /// Datos de sesion
    /// </summary>
    public class DatosSesionDeclaracion
    {
        public const string SesionFormato = "SesionFormato";
        public const string SesionIdEnvio = "SesionIdEnvio";
        public const string SesionFileName = "SesionFileName";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";
        public const string SPeriodo = "sPeriodo";
    }

    public class NombreArchivoDeclaracion
    {
        public const string ExtensionFileUploadDeclaracion = "xlsx";
    }

    //public class CabeceraCol
    //{
    //    public string TituloCol { get; set; }
    //    public string SubtituloCol { get; set; }
    //    public int Equicodi { get; set; } // para ocultar columnas
    //}
}