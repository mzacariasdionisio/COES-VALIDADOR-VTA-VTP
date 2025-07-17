using System;
using System.Configuration;

namespace COES.Servicios.Aplicacion.Equipamiento.Helper
{
    public class ConstantesEquipamientoAppServicio
    {

        //Acciones
        public const int AccionVer = 1;
        public const int AccionEditar = 2;
        public const int AccionNuevo = 3;

        public const int OpcionReporteFT = 1;
        public const int OpcionTodos = 2;

        public const string Activo = "A";
        public const string EnProyecto = "P";
        public const string Baja = "B";

        public const string Si = "S";
        public const string No = "N";

        public const int ValorSi = 1;
        public const int ValorNo = 0;

        public const string EstiloBaja = "background-color: #FFDDDD;";

        public const string HojaEmpresa = "EMPRESA";
        public const string HojaFamilia = "TIPO DE EQUIPO";
        public const string HojaUbicacion = "UBICACIÓN";
        public const string HojaSubestacion = "SUBESTACIÓN";

        public const string HojaProyecto = "PROYECTO";
        public const string HojaEquipo = "EQUIPO";

        public const string FormatoFechaReporte = "yyyyMMddHHmmss";
        public const string HojaPlantillaExcel = "PLANTILLA";

        // Formatos de Archivos
        public const string AppExcel = "application/vnd.ms-excel";

        public const string NombrePlantillaExcelPropiedades = "Plantilla_Propiedad.xlsx";
        public const string NombrePlantillaExcelNuevosEquipos = "Plantilla_Nuevos_Equipos.xlsx";
        public const string NombrePlantillaExcelParametros = "Plantilla_Reporte_Parametros.xlsx";
        //public const string NombrePlantillaExcelParametrosCV = "Plantilla_Propiedad.xlsx";
        public const string NombrePlantillaExcelRelPryEq = "PLANTILLA_RELACION_PROYECTO_EQUIPO.xlsx";

        public const string RutaReportes = "Areas/Equipamiento/Reporte/";

        // Constante para el repositorio CSV
        public const string AppCSV = "application/CSV";
        public const string SeparadorCampo = ",";
        public const string SeparadorCampoCSV = ";";
    }

    public class FilaExcelPropiedad
    {
        public int Row { get; set; }
        public int NumItem { get; set; }
        public int Propcodi { get; set; }
        public string Propnomb { get; set; }
        public string Propnombficha { get; set; }
        public string Propabrev { get; set; }
        public string Propdefinicion { get; set; }
        public string Propunidad { get; set; }
        public string Proptipo { get; set; }
        public string StrProptipolong1 { get; set; }
        public string StrProptipolong2 { get; set; }
        public string Propfichaoficial { get; set; }
        public string StrFamcodi { get; set; }
        public string NombreFamilia { get; set; }
        public string StrPropfecmodificacion { get; set; }
        public string Propusumodificacion { get; set; }
        public decimal? Propliminf { get; set; }
        public decimal? Proplimsup { get; set; }
        public string StrPropliminf { get; set; }
        public string StrProplimsup { get; set; }

        public int Famcodi { get; set; }
        public int? Proptipolong1 { get; set; }
        public int? Proptipolong2 { get; set; }
        public DateTime Propfecmodificacion { get; set; }
        //public string StrProgr { get; set; }

        //public int Equicodi { get; set; }
        //public int Areacodi { get; set; }
        //public int Emprcodi { get; set; }
        //public int Operadoremprcodi { get; set; }
        //public DateTime FechaIni { get; set; }
        //public DateTime FechaFin { get; set; }
        //public decimal MwIndisp { get; set; }
        //public int Tipoevencodi { get; set; }
        //public int Claprocodi { get; set; }
    }

    public class FilaExcelEquipo
    {
        public int Row { get; set; }
        public int NumItem { get; set; }
        public string StrEmprcodi { get; set; }
        public string StrOperador { get; set; }
        public string StrFamcodi { get; set; }
        public string StrUbicacion { get; set; }
        public string Equinomb { get; set; }
        public string Equiabrev { get; set; }
        public string Equiabrev2 { get; set; }
        public string Equiestado { get; set; }
        public string StrEquitension { get; set; }
        public string StrSubestacion { get; set; }

        public int Emprcodi { get; set; }
        public int Operadoremprcodi { get; set; }
        public int? Famcodi { get; set; }
        public int? Areacodi { get; set; }
        public int Equipadre { get; set; }
        public decimal? Equitension { get; set; }
        public int Equicodi { get; set; }
    }

    public class FilaExcelPropiedadesEquipos
    {
        public int Row { get; set; }
        public int NumItem { get; set; }
        public string Valor { get; set; }
        public string StrFechapropequi { get; set; }
        public string Propequisustento { get; set; }
        public string Propequicomentario { get; set; }
        public string StrPropequifecmodificacion { get; set; }
        public string Propequiusumodificacion { get; set; }
        public string StrPropequicheckcero { get; set; }

        public DateTime? Fechapropequi { get; set; }
        public int? Propequicheckcero { get; set; }
        public int Propcodi { get; set; }
        public int Equicodi { get; set; }
    }


    public class FilaExcelRelEquipoProyecto
    {
        public int Row { get; set; }
        public int NumItem { get; set; }
        public string StrCodigoProyecto { get; set; }
        public string StrEmpresaProyecto { get; set; }
        public string StrCodigoEstudio { get; set; }
        public string StrCodigoEquipo { get; set; }
        public string StrNombreEquipo { get; set; }
        public string StrAbreviaturaEq { get; set; }
        public string StrEstadoEq { get; set; }
        public string StrEmpresaEq { get; set; }
        public string StrTipoEquipo { get; set; }
        public string StrUbicacion { get; set; }

        public int Ftprycodi { get; set; }
        public int Equicodi { get; set; }
    }

    public class EmpresaCoes
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Codigo { get; set; }
    }
}
