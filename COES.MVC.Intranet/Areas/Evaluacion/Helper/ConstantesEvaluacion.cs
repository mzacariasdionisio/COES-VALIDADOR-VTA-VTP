
namespace COES.MVC.Intranet.Areas.Evaluacion.Helper
{
    public class ConstantesEvaluacion
    {
        public const string TipoTermico = "T";
        public const int CategoriaCentralTermica = 4;
        public const int CategoriaGrupoTermico = 3;
        public const int CategoriaModoTermico = 2;



        // Formatos de Archivos
        public const string AppExcel = "application/vnd.ms-excel";

        public const string NombrePlantillaExcelPropiedades = "Plantilla_Propiedad.xlsx";       
        public const string NombrePlantillaExcelParametros = "Plantilla_Reporte_Parametros.xlsx";


        public const string NombrePlantillaExcelReactor = "Plantilla_Reactor.xlsx";
        public const string NombrePlantillaExcelLinea = "Plantilla_Lineas.xlsx";
        public const string NombrePlantillaExcelCeldaAcoplamiento = "Plantilla_Celdas_Acoplamiento.xlsx";
        public const string NombrePlantillaExcelTransformadores = "Plantilla_Transformadores.xlsx";
        public const string NombrePlantillaWord = "Plantilla_Reporte.docx";
        public const string RutaReportes = "Areas/Proteccion/Reporte/";

        //constantes para fileServer
        public const string FolderRaizProteccion = "Proteccion/";
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
        public const string FolderMemoriaCalculo = FolderGestProtec +"/"+ "MemoriaCalculo";
        public const string FolderRele = FolderMemoriaCalculo + "/"+"Rele";
        public const string FolderSubestacion = FolderMemoriaCalculo + "/" + "Subestacion";
        public const string Activo = "1";
        public const string Inactivo = "0";
        public const string CheckActivo = "S";
        public const string CheckInactivo = "N";

        public const int EstadoLinea = 6;

        //Exportaciones
        public const string NombrePlantillaExcelExportacionLinea = "Plantilla_ExportacionLineas.xlsx";
        public const string NombrePlantillaExcelExportacionReactores = "Plantilla_ExportacionReactores.xlsx";
        public const string NombrePlantillaExcelExportacionCeldasAcoplamiento = "Plantilla_ExportacionCeldasAcoplamiento.xlsx";
        public const string NombrePlantillaExcelExportacionTransformadores = "Plantilla_ExportacionTransformadores.xlsx";

        //Nombres de Variables calculadas
        public const string NombreCapacidadMva = "CAPACIDAD_MVA";
        public const string NombreCapacTransCond1A = "CAPAC_TRANS_CORR_1_A";
        public const string NombreCapacTransCond2A = "CAPAC_TRANS_CORR_2_A";
        public const string NombreCapacidadTransmisionA = "CAPACIDAD_TRANSMISION_A";
        public const string NombreCapacidadTransmisionMVA = "CAPACIDAD_TRANSMISION_MVA";
        public const string NombreCapacidadTransmisionMvar = "CAPACIDAD_TRANSMISION_MVAR";
        public const string NombreFactorLimitanteCalc = "FACTOR_LIMITANTE_CALC";
        public const string NombreFactorLimitanteFinal = "FACTOR_LIMITANTE_FINAL";

        public const string NOMBRE_D1_CAPACIDAD_MVA = "D1_CAPACIDAD_MVA";
        public const string NOMBRE_D1_CAPACIDAD_A = "D1_CAPACIDAD_A";
        public const string NOMBRE_D1_CAPACIDAD_TRANSMISION_A = "D1_CAPACIDAD_TRANSMISION_A";
        public const string NOMBRE_D1_CAPACIDAD_TRANSMISION_MVA = "D1_CAPACIDAD_TRANSMISION_MVA";
        public const string NOMBRE_D1_FACTOR_LIMITANTE_CALC = "D1_FACTOR_LIMITANTE_CALC";
        public const string NOMBRE_D1_FACTOR_LIMITANTE_FINAL = "D1_FACTOR_LIMITANTE_FINAL";

        public const string NOMBRE_D2_CAPACIDAD_MVA = "D2_CAPACIDAD_MVA";
        public const string NOMBRE_D2_CAPACIDAD_A = "D2_CAPACIDAD_A";
        public const string NOMBRE_D2_CAPACIDAD_TRANSMISION_A = "D2_CAPACIDAD_TRANSMISION_A";
        public const string NOMBRE_D2_CAPACIDAD_TRANSMISION_MVA = "D2_CAPACIDAD_TRANSMISION_MVA";
        public const string NOMBRE_D2_FACTOR_LIMITANTE_CALC = "D2_FACTOR_LIMITANTE_CALC";
        public const string NOMBRE_D2_FACTOR_LIMITANTE_FINAL = "D2_FACTOR_LIMITANTE_FINAL";

        public const string NOMBRE_D3_CAPACIDAD_MVA = "D3_CAPACIDAD_MVA";
        public const string NOMBRE_D3_CAPACIDAD_A = "D3_CAPACIDAD_A";
        public const string NOMBRE_D3_CAPACIDAD_TRANSMISION_A = "D3_CAPACIDAD_TRANSMISION_A";
        public const string NOMBRE_D3_CAPACIDAD_TRANSMISION_MVA = "D3_CAPACIDAD_TRANSMISION_MVA";
        public const string NOMBRE_D3_FACTOR_LIMITANTE_CALC = "D3_FACTOR_LIMITANTE_CALC";
        public const string NOMBRE_D3_FACTOR_LIMITANTE_FINAL = "D3_FACTOR_LIMITANTE_FINAL";

        public const string NOMBRE_D4_CAPACIDAD_MVA = "D4_CAPACIDAD_MVA";
        public const string NOMBRE_D4_CAPACIDAD_A = "D4_CAPACIDAD_A";
        public const string NOMBRE_D4_CAPACIDAD_TRANSMISION_A = "D4_CAPACIDAD_TRANSMISION_A";
        public const string NOMBRE_D4_CAPACIDAD_TRANSMISION_MVA = "D4_CAPACIDAD_TRANSMISION_MVA";
        public const string NOMBRE_D4_FACTOR_LIMITANTE_CALC = "D4_FACTOR_LIMITANTE_CALC";
        public const string NOMBRE_D4_FACTOR_LIMITANTE_FINAL = "D4_FACTOR_LIMITANTE_FINAL";

        //Plantilla Exportacion Reles
        public const string NombrePlantillaExcelReleSincronismo = "Plantilla_ExportacionRelesSincronismo.xlsx";
        public const string NombrePlantillaExcelReleSobreTension = "Plantilla_ExportacionRelesSobretension.xlsx";
        public const string NombrePlantillaExcelRelesMandoSincronizado = "Plantilla_ExportacionRelesMandoSincronizado.xlsx";
        public const string NombrePlantillaExcelRelesPmu = "Plantilla_ExportacionRelesPMU.xlsx";
        public const string NombrePlantillaExcelRelesTorsionales = "Plantilla_ExportacionRelesTorsionales.xlsx";

        public const string L_Equicodi = "L_Equicodi";
        public const string L_Codigo = "L_Codigo";
        public const string L_Empresa = "L_Empresa";
        public const string L_Estado = "L_Estado";
        public const string L_SubEstacion1 = "L_SubEstacion1";
        public const string L_SubEstacion2 = "L_SubEstacion2";
        public const string L_Area = "L_Area";
        public const string L_Tension = "L_Tension";
        public const string L_IncluirCalcular = "L_IncluirCalcular";

        public const string CA_Equicodi = "CA_Equicodi";
        public const string CA_Codigo = "CA_Codigo";
        public const string CA_Ubicacion = "CA_Ubicacion";
        public const string CA_Empresa = "CA_Empresa";
        public const string CA_Area = "CA_Area";
        public const string CA_Tension = "CA_Tension";
        public const string CA_Estado = "CA_Estado";
        public const string CA_IncluirCalcular = "CA_IncluirCalcular";

        public const string R_Equicodi = "R_Equicodi";
        public const string R_Codigo = "R_Codigo";
        public const string R_Ubicacion = "R_Ubicacion";
        public const string R_Empresa = "R_Empresa";
        public const string R_Area = "R_Area";
        public const string R_Estado = "R_Estado";
        public const string R_IncluirCalcular = "R_IncluirCalcular";

        public const string T_Equicodi = "T_Equicodi";
        public const string T_Codigo = "T_Codigo";
        public const string T_Tipo = "T_Tipo";
        public const string T_Ubicacion = "T_Ubicacion";
        public const string T_Empresa = "T_Empresa";
        public const string T_Area = "T_Area";
        public const string T_Tension = "T_Tension";
        public const string T_Estado = "T_Estado";
        public const string T_IncluirCalcular = "T_IncluirCalcular";

    }

    /// <summary>
    /// Datos de sesion
    /// </summary>
    public class DatosSesionEvaluacion
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