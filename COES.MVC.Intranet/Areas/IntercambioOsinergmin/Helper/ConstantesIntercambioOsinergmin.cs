using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Helper
{
    public class ConstantesIntercambioOsinergmin
    {
        public const string Delimitador = "|";
        public const string ProcedimientoSicli = "SICLI";
        public const int tipoInfoCodi = 1;
        public const string TablaSicli04 = "TMP_CLI_TABLA04";
        public const string TablaSicli05 = "TMP_CLI_TABLA05";
        public const string TablaSicli02 = "TMP_CLI_TABLA02";

    }

    public class ParametrosEnvio
    {
        public const byte EnvioEnviado = 1;
        public const byte EnvioProcesado = 2;
        public const byte EnvioAprobado = 3;
        public const byte EnvioRechazado = 4;
        public const byte EnvioFueraPlazo = 5;
    }

    //- alpha.HDT - 09/07/2017: Cambio para atender el requerimiento. 
    /// <summary>
    /// Enumera los tipos de incidencias en la importación de los datos Sicli.
    /// </summary>
    public enum EnuTipoIncidenciaImportaSicli
    {
        Ninguno = 0,
        SuministroUsuarioLibreNoExiste = 1,
        EmpresaNoExiste = 2,
        /// <summary>
        /// La Barra con el código Osinergmin no existe.
        /// </summary>
        BarraNoExiste = 3
    }

    //mejoras PR16 07/08/2019
    public class DatosSesionIntercambioOsinergmin
    {
        public const string RutaCompletaArchivo = "RutaCompletaArchivo";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        //public const string SesionMatrizExcel = "MatrizExcel";
        //public const string SesionIdEnvio = "SesionIdEnvio";
    }

    /// <summary>
    /// Nombre de archivos
    /// </summary>
    public class NombreArchivoIntercambioOsinergmin
    {
        public const string ExtensionFileUploadRechazoCarga = "xlsx";
        //public const string ProgramaRechazoCarga = "ProgramaRechazoCarga.xlsx";

        //public const string ParamEsquema = "ParamEsquema.xlsx";
        //public const string CuadroEjecucionUsuario = "CuadroEjecucionUsuario.xlsx";

        //public const string DemandaUsuario = "DemandaUsuario.xlsx";
    }


}