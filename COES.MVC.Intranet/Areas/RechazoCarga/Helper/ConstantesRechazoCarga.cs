using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Helper
{
    public class ConstantesRechazoCarga
    {
        public const int DiasEspera = 30;
        public const int HorasEspera = 60;
        //public const string CuadroProgEjecutado = "E";

        public const int HorizonteReprograma = 4;
        public const int HorizonteDiario = 1;
        public const int HorizonteSemanal = 2;
        public const int HorizonteMensual = 3;

        public const int PageSizeDemandaUsuario = 30;
        public const int NroPageShow = 10;

        public const int CodigoAreaNivel = 4;

        //Constantes Archivo Demanda Usuario
        public const int filaFechaArchivoDemanda = 2;
        public const int filaTipoDemandaArchivoDemanda = 3;
        public const int filaInicioDatos = 5;

        public const int EstadoCuadroProgramado = 1;
        public const int EstadoCuadroEjecutado = 2;
        public const int EstadoCuadroNoEjecutado = 3;
        public const int EstadoCuadroReprogramado = 4;

        //Perfiles
        public const int PerfilSPR = 1;
        public const int PerfilSCO = 2;
        public const int PerfilSEV = 3;
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

        public const string DemandaUsuario = "DemandaUsuario.xlsx";
        public const string DemandaUsuarioErrores = "DemandaUsuarioErrores.xlsx";
    }
    
}