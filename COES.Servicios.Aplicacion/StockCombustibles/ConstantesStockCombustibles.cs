using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.StockCombustibles
{
    public class ConstantesStockCombustibles
    {
        public static readonly List<int> IdsTptoStock = new List<int> { 25, 34, 36, 38 };
        public static readonly List<int> IdsTptoRecepcion = new List<int> { 26, 35, 37, 39 };
        public static readonly List<int> IdsTptoConsumo = new List<int> { 28, 32, 31, 45, 27, 29, 30, 33 };// { 27,28,29,30, 31,32,33, 45 };
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoDiaMes = "dd/MMM";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        public const string FormatoHoraMinuto = "HH:mm";
        public const int NoValidado = 0;


        public const string StrTptoStock = "25,34,36,38,86"; //"25,34,36,38";//"24,33,35,37,41";
        public const string StrTptoRecepcion = "26,35,37,39,87"; //"26,35,37,39";//"25,34,36,38,42";
        public const string StrTptoConsumo = "28,32,31,45,27,29,30,33,88,85,90"; //"28,32,31,45,27,29,30,33";///"27,28,29,30,31,32,33,45";//"26,27,28,29,30,31,32";

        public const int IdFormatoConsumo = 57;
        public const int IdFormatoPGas = 58;
        public const int IdFormatoDisponibilidadGas = 59;
        public const int IdFormatoQuemaGas = 60;
        public const int LectCodiStock = 77;
        public const int LectCodiConsumo = 78;
        public const int LectCodiPresionGas = 79;
        public const int LectCodiTempAmbiente = 80;
        public const int LectCodiDisponibilidad = 81;
        public const int LectCodiQuemaGas = 82;
        public const int IdTipoInfoPresionDeGas = 52;//49;
        public const int IdTipoInfoTemperaturaAmbiente = 50;
        public const int TipoInfocodiPresion = 52;//49;
        public const int TipoInfocodiTemperatura = 50;

        public const int Origlectcodi = 21;//13;
        public const int CentralTermica = 5;
        public const int TipotomedicodiStock = 16;
        public const int TipotomedicodiConsumo = 17;
        public const int TipotomedicodiPresionGas = 40;//39;
        public const int TipotomedicodiTemperatura = 41;//40;

        public const string StrCtralIntCoes = "-1,0,1,2,3";
        public const string StrCtralIntNoCoes = "10";
        public const string ValueCbCoes = "1";
        public const string ValueCbNoCoes = "2";

        public const string TxtLiquido = "LÍQUIDO";
        public const string TxtSolido = "SÓLIDO";
        public const string TxtGaseoso = "GASEOSO";

        public const string ValueCbEstadoLiquido = "1";
        public const string ValueCbEstadoSolido = "2";
        public const string ValueCbEstadoGaseoso = "3";
        public const string NombreLogoCoes = "coes.png";

        public const int TipoReporteStock = 1;
        public const int TipoReporteAcumulado = 2;
        public const int TipoReporteAcumuladoDet = 3;

        public const int ColCentral = 0;
        public const int ColTipo = 1;
        public const int ColUnidad = 2;
        public const int ColInicial = 3;
        public const int ColRecepcion = 4;
        public const int ColConsumo = 5;
        public const int ColFinal = 6;
        public const int ColDeclarado = 7;

        public const int ColObservacion = 8;
        public const int ColLen = 9;
        public const int BandaTR = 3;

        #region Informes SGI

        public const string DesvariableListaCorreoEmpresa = "Lista de correos por empresa";
        public const string DesvariableFechaIeod = "Fecha del IEOD";
        public const string DesvariableEmresa = "Empresa";
        public const string DesvariableListaFormato = "Lista de formatos pendientes";

        public const string VariableListaCorreoEmpresa = "{Lista_Correos_Empresa}";
        public const string VariableFechaIeod = "{Fecha_IEOD}";
        public const string VariableEmresa = "{Empresa}";
        public const string VariableListaFormato = "{Lista_Formato_Pendientes}";

        public const int IdPlantillaNotificacion = 193;
        public const int IdConfiguracionCorreo = 1;

        #endregion

        public const int IdProcesoNotificacion = 57;
    }
}
