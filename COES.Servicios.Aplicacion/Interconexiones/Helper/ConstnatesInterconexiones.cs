using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Interconexiones.Helper
{
    public class ConstantesInterconexiones
    {
        public const decimal PotIntMK1 = 6.132M;
        public const decimal PotIntMK2 = 6.396M;
        public const int IdPropCapL2249 = 1800;
        public const int IdPropFacCapL2249 = 1801;
        public const int IdL2249 = 1073;
        public const int IdCtTumbes = 1498;
        public const int IdMK1 = 1068;
        public const int IdMK2 = 1069;
        public const int IdLectCPDiaPre = 71;
        public const int IdLectCPSemPre = 72;
        public const int IdLectCPDiaFin = 73;
        public const int IdLectCPSemFin = 74;
        public const int Excedente = 1;

        public const int IdPtoMedicionL2280 = 5020;
        //public const int IdExportacionL2280MWh = 41103;
        //public const int IdImportacionL2280MWh = 41104;
        //public const int IdExportacionL2280MVARr = 41105;
        //public const int IdImportacionL2280MVARr = 41106;

        public const int IdExportacionL2280MWh = 41238;
        public const int IdImportacionL2280MWh = 41239;
        public const int IdExportacionL2280MVARr = 41240;
        public const int IdImportacionL2280MVARr = 41241;

        public const int FuenteTIEFlujoNewAnexoA = 2;
        public const int FuenteTIEFlujoOldDesktop = 3;

        public const int IdReporteInterconexion = 3;
        public const int IdFormatoInterconexion = 50;
        public const int IdTipoPtomedicodiExportacionMwh = 20;
        public const int IdTipoPtomedicodiImportacionMwh = 21;
        public const int IdTipoPtomedicodiExportacionMVarh = 22;
        public const int IdTipoPtomedicodiImportacionMVarh = 23;
        public const int IdTipoPtomedicodiMedidaElectrica = 15;
        public const int IdTipoInfocodiEnergiaActiva = 3;
        public const int IdTipoInfocodiEnergiaReactiva = 4;
        public const int IdTipoInfocodiKV = 5;
        public const int IdTipoInfocodiA = 9;
        public const int IdEmpresaInterconexion = 12;

        public const int IdPtoMedicionPrincipal = 5020;
        public const int IdPtoMedicionSecundario = 43518; //43020

        public const int IdMedidorConsolidado = 0;
        public const int IdMedidorPrincipal = 1;
        public const int IdMedidorSecundario = 2;
        public const string NombMedidorConsolidado = "Consolidado";
        public const string NombMedidorPrincipal = "Principal";
        public const string NombMedidorSecundario = "Secundario";

        public const string NombreArhivoInformeInterconexion = "InformeInterconexion_{0}.docx";
        public const string PathArchivos = "Areas/Interconexiones/Reporte/";
        public const string FolderIntervenciones = "Interconexiones/";
        public const string ExtensionInforme = "docx";

        public const string DirectorioInterconexiones = "Areas\\Interconexiones\\Reporte\\";
        public const string PlantillaReporteHistorico = "PlantillaReporteHistorico.xlsx";
        public const string NombreReporteHistorico = "Historico.xlsx";
        public const string RutaPortalInterconexion = "Operación/Intercambios Internacionales/Lectura de Contadores/";
        public const string PathPortalWeb = "PathPortalWebInterconexion";
    }
}
