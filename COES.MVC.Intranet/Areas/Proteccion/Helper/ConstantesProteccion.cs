
namespace COES.MVC.Intranet.Areas.Proteccion.Helper
{
    public class ConstantesProteccion
    {
        public const string TipoTermico = "T";
        public const int CategoriaCentralTermica = 4;
        public const int CategoriaGrupoTermico = 3;
        public const int CategoriaModoTermico = 2;



        // Formatos de Archivos
        public const string AppExcel = "application/vnd.ms-excel";

        public const string NombrePlantillaExcelPropiedades = "Plantilla_Propiedad.xlsx";
        public const string NombrePlantillaExcelParametros = "Plantilla_Reporte_Parametros.xlsx";


        public const string NombrePlantillaExcelReleUsoGeneral = "Plantilla_Rele_UsoGeneral.xlsx";
        public const string NombrePlantillaExcelReleMandoSincronizado = "Plantilla_Rele_MandoSincronizado.xlsx";
        public const string NombrePlantillaExcelRelePmu = "Plantilla_Rele_PMU.xlsx";
        public const string NombrePlantillaExcelReleTorsional = "Plantilla_Rele_Torsional.xlsx";

        public const string RutaReportes = "Areas/Proteccion/Reporte/";

        //constantes para fileServer
        public const string Plantilla = "Plantilla\\";

        public const int CodigoReleTipoUsoGeneral = 101;
        public const int CodigoReleTipoUsoMandoSincronizado = 102;
        public const int CodigoReleTipoUsoPmu = 104;
        public const int CodigoReleTipoUsoTorsional = 103;

        public const string Nuevo = "NUEVO";
        public const string Editar = "EDITAR";
        public const string Consulta = "CONSULTAR";
        public const string FolderGestProtec = "GestProtec";
        public const string FolderReporte = FolderGestProtec + "/" + "Reporte";
        public const string FolderTemporal = FolderGestProtec + "/" + "Temporal";
        public const string FolderArchivoZIP = FolderGestProtec + "/" + "ArchivoZIP";
        public const string FolderMemoriaCalculo = FolderGestProtec + "/" + "MemoriaCalculo";
        public const string FolderRele = FolderMemoriaCalculo + "/" + "Rele";
        public const string FolderSubestacion = FolderMemoriaCalculo + "/" + "Subestacion";
        public const string FolderManual = FolderGestProtec + "/" + "Manual";
        public const string ArchivoManualEquipoProteccion = "Manual_Usuario_Intranet_Protecciones.pdf";
        public const string Activo = "1";
        public const string Inactivo = "0";
        public const string CheckActivo = "S";
        public const string CheckInactivo = "N";



        public const int CatalogoEstadoEquipoProteccion = 1;
        public const int CatalogoSistemaReleEquipoProteccion = 2;
        public const int CatalogoMarcaProteccion = 3;
        public const int CatalogoTipoUsoEquipoProteccion = 4;
        public const int CatalogoMandoSincronizadoEquipoProteccion = 5;
        public const int CatalogoReleTorcionalImplEquipoProteccion = 0;
        public const int EstadoLinea = 6;


        public const string EP_Equicodi = "EP_Equicodi";
        public const string EP_Nivel = "EP_Nivel";
        public const string EP_Celda = "EP_Celda";
        public const string EP_Rele = "EP_Rele";
        public const string EP_IdArea = "EP_IdArea";
        public const string EP_NombSubestacion = "EP_NombSubestacion";
        public const string EP_TituloRele = "EP_TituloRele";

    }

    /// <summary>
    /// Datos de sesion
    /// </summary>
    public class DatosSesionProteccion
    {
        public const string RutaCompletaArchivo = "RutaCompletaArchivo";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";
        public const string SesionIdEnvio = "SesionIdEnvio";
    }


    



    public class NombreArchivoTipoUso
    {
        public const string ExtensionFileUploadTipoUso = "xlsx";       

    }
}