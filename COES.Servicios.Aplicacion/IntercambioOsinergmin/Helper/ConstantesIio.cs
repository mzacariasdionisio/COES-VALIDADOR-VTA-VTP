using COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper;
namespace COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper
{
    /// <summary>
    /// TODO: Agregar los codigos de tabla
    /// </summary>
    public class ConstantesIio
    {
        public const string EmpresaDisplayName = "Empresa";
        public const string UsuarioLibreDisplayName = "Usuario Libre";
        public const string SuministroDisplayName = "Suministro";
        public const string CentralDisplayName = "Central de Generación";
        public const string GrupoGeneracionDisplayName = "Grupo de Generación";
        public const string ModoOperacionDisplayName = "Modo de Operación";
        public const string CuencaDisplayName = "Cuenca";
        public const string EmbalseDisplayName = "Embalse";
        public const string LagoDisplayName = "Lago";
        public const string BarraDisplayName = "Barra";
        public const string ConAsignacionPendienteDisplayName = "Resultado";

        public const string FamNombCentral = "CENTRAL";
        public const string FamNombGenerador = "GENERADOR";
        public const string FamNombEmbalse = "EMBALSE";
        public const string FamNombLago = "LAGO";
        public const string FamNombBarra = "BARRA";
        public const string FamNombCuenca = "CUENCA";
        public const string FamNombModoOperacion = "MODO";

        public const string EmpresaTableName = "ADM_EMPRESA";
        public const string CentralTableName = "";
        public const string GrupoGeneracionTableName = "";
        public const string ModoOperacionTableName = "";
        public const string CuencaTableName = "";
        public const string EmbalseTableName = "";
        public const string LagoTableName = "";
        public const string BarraTableName = "";
        public const string ConAsignacionPendienteTableName = "";

        public const string MaestrosDisplayName = "Maestros";
        public const string SincronizacionDisplayName = "Sincronizacion";

        public const EntidadSincroniza EntidadAsignacionesPendientes = EntidadSincroniza.Ninguno;
        public const EntidadSincroniza EntidadMaestroSeleccionadaPorDefecto = EntidadSincroniza.Empresa;

        public const string ReporteSincronizacionFileName = "ReporteSincronizacion.xls";

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Nombre del reporte a generar de los datos sincronizados en la importación de los
        /// datos del Sicli, a saber: tabla 04 y tabla 05.
        /// </summary>
        public const string ReporteDatosImportacionFileName = "ReporteDatosImportados_{0}.xls";

        //- alpha.HDT - 29/06/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Nombre del archivo de reporte para generar los datos de los maestros del Osinergmin.
        /// </summary>
        public const string ReporteDatosOsinergminFileName = "ReporteDatosOsinergmin_{0}.xls";


    }
}