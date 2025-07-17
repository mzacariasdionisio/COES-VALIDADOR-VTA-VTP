using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.ServicioRPF.Helper
{
    public class ConstantesDatosAGC
    {
        public const int IdParametroPlazo = 26;
        public const string RutaExportacionExtranet = "Areas/Rpf/Reporte/";
        public const string RutaExportacionIntranet = "Areas/ServicioRPF/Reporte/";
        public const string NombreArchivoAGC = "NombreArchivoAGC";
        public const string HoraCero = "00:00:00";
        public const string HoraCeroAlter = "0:00:00";



        public const string OK = "OK";
        public const string NoExiste = "No existe archivo, por favor cargue.";
        public const string Error = "ERROR";
        public const string FormatoIncorrectoPtoMedicion = "El código de la columna {0} de la primera línea no tiene formato correcto.";
        public const string FormatoCodigosDuplicados = "El código {0} solo debe aparecer dos veces (Para SetPoint y Estado)";
        public const string FormatoIncorrectoTipoInfo = "El código de la columna {0} de la segunda línea no tiene formato correcto.";
        public const string CantidadPtoMedicionVSTipoInfo = "No existe la misma cantidad de códigos de la primera y sexta línea.";
        public const string VerificarCantidadCodigos = "La cantidad de códigos debe ser un número par(SetPoint y Estado)";
        public const string CodigosPuntosIguales = "El código de la columna {0} de la primera fila debe ser igual al de la columna anterior";
        public const string CodigosValorSetPoint = "El código de la columna {0} de la segunda fila debe ser SetPoint";
        public const string CodigosValorEstado = "El código de la columna {0} de la seguna fila debe ser Estado";
        public const string CodigoInvalido = "El código de la columna {0} de la primera fila no le pertenece a la empresa seleccionada.";
        public const string FechaNoValida = "Fecha: Año column 2, Mes columna 3 y Día columna 4.";
        public const string AnioIncorrecto = "El año no tiene el formato correcto.";
        public const string MesIncorrecto = "El mes no tiene el formato correcto.";
        public const string DiaIncorrecto = "El día no tiene el formato correcto.";
        public const string FechasNoCoinciden = "La fecha seleccionada y la fecha en el formato no coinciden.";
        public const string DatosInicio = "Los datos deben iniciar en la fila 8.";
        public const string NoExisteRegistro = "No existe el registro para la hora: {0}";
        public const string FechaNoTieneFormato = "El registro correspondiente a la hora {0} no tiene formato correcto.";
        public const string FechaNoDatosCompleto = "El registro correspondiente a la hora {0} debe tener llenado todas las columnas.";
        public const string ValidacionFormatoNumero = "La celda de fila: {0} y Columna: {1} no tiene formato correcto.";
        public const string ValoresNegativosNoPermitidos = "No se permiten valores negativos.";
        public const string FechaNoPermitidaDiaAnterior = "Solo de permiten cargar datos del día anteior.";
        public const string HoraNoPermitida = "Se ha superado la hora máxima de carga: {0} a.m.";
        public const string SetPoint = "SetPoint";
        public const string Estado = "Estado";
        public const string CaracterPuntos = ":";
        public const char CaracterCero = '0';
        public const char CaracterSlash = '/';
        public const char SeparadorPuntos = ':';
        public const int CodigoBasePoint = 6;
        public const int CodigoSetPoint = 5;
        public const int CodigoEstado = 2;
        public const string FuenteExtranet = "E";
        public const string FuenteIntranet = "I";
        public const string EnviadoEnPlazo = "1";
        public const string NoEnviado = "2";
        public const string EnviadoPorCOES = "3";
        public const string TextoEnPlazo = "Enviado en plazo";
        public const string TextoEnviadoPorCOES = "Cargado por COES";
        public const string TextoNoEnvio = "No envió";
        public const int Resolucion15min = 1;
        public const int Resolucion30min = 2;
        public const string PorUnidad = "1";
        public const string PorCentral = "2";

        public const int CasoUrsNoEspecial_TodasElegidas_OpUnidadRepCentral = 1;
        public const int CasoUrsNoEspecial_TodasElegidas_OpCentralRepUnidad = 2;
        public const int CasoUrsNoEspecial_TodasElegidas_OpUnidadRepUnidad = 3;
        public const int CasoUrsNoEspecial_TodasElegidas_OpCentralRepCentral = 4;

        public const int CasoUrsNoEspecial_NoTodasElegidas_OpUnidadRepUnidad_Seleccionadas = 5;
        public const int CasoUrsNoEspecial_NoTodasElegidas_OpCentralRepUnidad_Seleccionadas = 6;
        public const int CasoUrsNoEspecial_NoTodasElegidas_OpUnidadRepUnidad_NoSeleccionadas = 7;
        public const int CasoUrsNoEspecial_NoTodasElegidas_OpCentralRepUnidad_NoSeleccionadas = 8;

        public const int CasoUrsEspecial_RepUnidad = 9;

    }


    #region Clases_RSF_PR22
    public class ComparativoExtranetConSP7
    {
        public string Hora { get; set; }
        public decimal? ValorExtranet { get; set; }
        public decimal? ValorSP7 { get; set; }
        public decimal? Diferencia { get; set; }
        public decimal? Desviacion { get; set; }

    }

    public class DatoCsv
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
        public string Valor { get; set; }

    }

    public class Urs
    {
        public int Grupocodi { get; set; }
        public int Equicodi { get; set; }
        public string NombreUrs { get; set; }
        public string NombreCentral { get; set; }
        public bool EsEspecial { get; set; }
        public int? PtomedicionCentral { get; set; }
        public string DatoOperacion { get; set; } //1:UNIDAD, 2:CENTRAL
        public string DatoReporte { get; set; } //1:UNIDAD, 2:CENTRAL
        public List<EquipoGen> LstUnidTotales { get; set; }
        public bool TodasUnidadesSeleccionadas { get; set; }
        public List<EquipoGen> LstUnidSeleccionadas { get; set; }
        public List<EquipoGen> LstUnidNoSeleccionadas { get; set; }
    }

    public class EquipoGen
    {
        public int Ptomedicion { get; set; }
        public int Equicodi { get; set; }
        public string NombreUrs { get; set; }
        public bool Bandera { get; set; }
    }

    public class RegKumpliy
    {
        public int OrdenColumna { get; set; }
        public int Grupocodi { get; set; }
        public int? Ptomedicion { get; set; }
        public int Equicodi { get; set; }
        public string NombreUrs { get; set; }
        public string NombreCentral { get; set; }
        public int NumSegundo { get; set; }
        public decimal? Setpoint { get; set; }
        public decimal? Estado { get; set; }
        public decimal? Basepoint { get; set; }        
    }

    public class DatoCE2
    {
        public decimal? DatoSetpoint { get; set; }
        public decimal? DatoEstado { get; set; }
        public decimal? DatoBasepoint { get; set; }
        public int Equicodi { get; set; }
        public string Tipo { get; set; }
    }
    #endregion
}
