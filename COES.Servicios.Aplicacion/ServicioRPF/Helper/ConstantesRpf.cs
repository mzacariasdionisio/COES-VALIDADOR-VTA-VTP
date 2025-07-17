using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.ServicioRPF.Helper
{
    public class ConstantesRpf
    {
        public const string FormatoCSV = ".csv";
        public const string RutaCarga = "Uploads/";
        public const int Potencia = 1;
        public const int Frecuencia = 6;
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoFechaExtendido = "dd/MM/yyyy HH:mm:ss";
        public const char CaracterCero = '0';
        public const string CaracterSlash = "/";
        public const string HoraCero = "00:00:00";
        public const string HoraCeroAlter = "0:00:00";
        public const string CaracterPuntos = ":";
        public const char SeparadorPuntos = ':';
        public const string TableName = "ME_MEDICION";
        public const string StrConexion = "cnnAplicacion";
        public const string StrAzure = "cnnAplicationAzure";
        public const string LogGrabado = "Extranet.Cloud";
        public const string CaracterComa = ",";
        public const string ItemResultado = "<li>{0}</li>";
        public const string AperturaResultado = "<ul>";
        public const string CierreResultado = "</ul>";
        public const string SeparacionItem = " - ";
        public const string RequestUser = "user";
        public const string RequestIntranet = "ind";
        public const string VistaRestringido = "Restringido";
        public const string EquiCodi = "EquiCodi";
        public const string EquiNomb = "EquiNomb";
        public const string PtoMediCodi = "PtoMediCodi";
        public const string PtoMediNomb = "PtoMediNomb";
        public const string AdminUser = "AdminUser";
        public const char SeparadorComa = ',';
        public const string CaracterGuionBajo = "_";
        public const string Parametro = "@";
        public const string SI = "S";
        public const string NO = "N";
        public const string HoraPermitida = "08:00";
        public const string MensajeInicio = "Solo se permite cargar datos del día anterior y solo hasta las {0} a.m.";
        public const string Consistente = "CONSISTENTE";
        public const string Inconsistente = "INCONSISTENTE";
        public const string OPERO = "OPERÓ";
        public const string CERO = "CERO";
        public const string OK = "OK";
        public const int ResolucionCuartoHora = 15;
        public const int ResolucionMediaHora = 30;
        public const int ResolucionHora = 60;
    }

    /// <summary>
    /// Textos de validaciones de archivos
    /// </summary>
    public class ValidacionArchivoRpf
    {
        public const string OK = "OK";
        public const string NoExiste = "No existe archivo, por favor cargue.";
        public const string Error = "ERROR";
        public const string FormatoIncorrectoPtoMedicion = "El código de la columna {0} de la primera línea no tiene formato correcto.";
        public const string FormatoIncorrectoTipoInfo = "El código de la columna {0} de la segunda línea no tiene formato correcto.";
        public const string CantidadPtoMedicionVSTipoInfo = "No existe la misma cantidad de códigos de la primera y segunda línea.";
        public const string VerificarCantidadCodigos = "La cantidad de códigos debe ser un número par(Potencia y Frecuencia)";
        public const string CodigosPuntosIguales = "El código de la columna {0} de la primera fila debe ser igual al de la columna anterior";
        public const string CodigosValorPotencia = "El código de la columna {0} de la segunda fila debe ser 1";
        public const string CodigosValorFrecuencia = "El código de la columna {0} de la seguna fila debe ser 6";
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
    }
}
