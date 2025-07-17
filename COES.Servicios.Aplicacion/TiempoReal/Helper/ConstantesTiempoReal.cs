using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.TiempoReal.Helper
{
    public class ConstantesTiempoReal
    {
        public const string ControlCentralizado = "C";
        public const string ControlProporcional = "P";
        public const string DespachoCComb = "Ccomb";
        public const string DespachoCVariableCombustible = "Cvc";
        public const string DespachoCVariableNoCombustible = "Cvnc";
        public const string TagB1CostoVariable = "Network/.Generation";
        public const string TagB1Mantenimiento = "Network/Companies";
        public const string MantenimientoDisponible = "Available";
        public const string MantenimientoIndisponible = "Unavailable";
        public const int FamiliaHydro = 2;
        public const int FamiliaThermo = 3;
        public const int FamiliaBanco = 11;
        public const int FamiliaReactor = 12;
        public const int FamiliaCS = 13;
        public const int FamiliaSVC = 14;
        public const string MantenimientoEnServicio = "E";
        public const string MantenimientoFueraServicio = "F";
        public const int RestriccionesOperativas = 205;
        public const string ListaFrecuencia = "ListaFrecuencia";
        public const string ListaCircular = "ListaCircular";

        //Estadística
        public const string RutaReportes = "Areas/TiempoReal/Reporte/";
        public const string FolderEstadistica = "Estadistica/";
        public const string AppCSV = "application/CSV";
        public const string SeparadorCampo = ",";
        public const string SeparadorCampoCSV = ";";

        public const int TipoArchivoLineaTrafo = 1;
        public const int TipoArchivoBarra = 2;

        public const int FormatoProcesoArchivoEstadistica = 139;

        #region REPORTE 
        public const string NombreReporteExcelActualizaciones = "ActualizacionesTiempoRealSP7.xlsx";
        public const string NombreReporteExcelCargaArchivoXML = "ListaCargaArchivoXML.xlsx";

        public Dictionary<int, string> TipoCargaArchivo = new Dictionary<int, string>();

        public ConstantesTiempoReal()
        {
            TipoCargaArchivo.Add(1, "Sincronizacion directa con Siemens");
            TipoCargaArchivo.Add(2, "Carga archivo XML ICCP");
            TipoCargaArchivo.Add(3, "Carga archivo XML PERU");
        }
        #endregion
    }

    public class ConstantesRegistroObservacion
    {
        public const string EstadoPendiente = "P";
        public const string TipoCaida = "C";
        public const string TipoObservacion = "O";

        #region FIT - Señales no disponibles SCADA

        public const string ProcesoAutomatico = "A";
        public const string ProcesoManual = "M";

        #endregion
    }

    public class ConstantesTrCanal
    {
        //Canal
        public const string TipoPuntoAnalogico = "A";
    }

    public class FiltroActualizacionSp7Model
    {
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public int TipoReporte { get; set; }
    }

    public class ConstantesFiltro
    {
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public int TipoReporte { get; set; }
    }
}
