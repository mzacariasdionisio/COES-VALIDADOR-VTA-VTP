using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace COES.MVC.Extranet.Areas.RechazoCarga.Helper
{
    public class ConstantesRechazoCarga
    {
        public const int DiasEspera = 30;
        public const int HorasEspera = 60;
        public const int EstadoCuadroEjecutado = 2;

        public const int HorizonteReprograma = 4;

        public const string RutaCarga = "Uploads/";

        public const int OrigenExtranet = 2;

        public const int EstadoVigente = 1;
    }

    /// <summary>
    /// Datos de sesion
    /// </summary>
    public class DatosSesionRechazoCarga
    {
        public const string RutaCompletaArchivo = "RutaCompletaArchivo";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";
        public const string SesionIdEnvio = "SesionIdEnvio";

        public const string SesionNombreArchivoDetalle = "SesionNombreArchivoDetalle";
        public const string RutaCompletaArchivoDetalle = "RutaCompletaArchivoDetalle";
    }
   
    /// <summary>
    /// Nombre de archivos
    /// </summary>
    public class NombreArchivoRechazoCarga
    {
        public const string ExtensionFileUploadRechazoCarga = "xlsx";
        public const string ProgramaRechazoCarga = "ProgramaRechazoCarga.xlsx";

        public const string ParamEsquema = "ParamEsquema.xlsx";
        public const string CuadroEjecucionUsuario = "CuadroEjecucionUsuario.xlsx";
        public const string CuadroEjecucionDetUsuario = "CuadroEjecucionDetUsuario.xlsx";
    }

}