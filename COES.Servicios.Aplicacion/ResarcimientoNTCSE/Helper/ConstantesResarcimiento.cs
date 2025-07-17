using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.ResarcimientoNTCSE.Helper
{
    public class ConstantesResarcimiento
    {
        public const string Periodoestadohabilitado = "0";
        public const string Periodoestadobloqueado = "1";
        //Constantes SIS_AUDITORIA_REGISTROS
        public const string AudiNombreSistema = "NTCSE";
        public const string AudiNombreTabla = "SIS_AUDITORIA_REGISTROS";
        //Constantes RNT_CONFIGURACION
        public const string TensionConfatributo = "NIVELTENSION";
        public const string Configuracionatributo = "CONFIGURACION";
        public const string TensionConfparametro = "TENSIONNOMB";
        //public const string Atributo = "CONFIGURACION";       
        public const string NombrePlantillaExcelRRC = "PlantillaRRC.xlsx";
        public const string NombreReporteExcelRRC = "ReporteRRC.xlsx";
        public const string NombrePlantillaExcelRPE = "PlantillaRPE.xlsx";
        public const string NombreReporteExcelRPE = "ReporteRPE.xlsx";
        public const string NombrePlantillaExcelRPEAuditoria = "PlantillaRPEAuditoria.xlsx";
        public const string NombreReporteExcelRPEAuditoria = "ReporteRPEAuditoria.xlsx";
        public const string NombrePlantillaExcelRRCAuditoria = "PlantillaRRCAuditoria.xlsx";
        public const string NombreReporteExcelRRCAuditoria = "ReporteRRCAuditoria.xlsx";
        public const string UrlGenerarReporte = "Reporte Generado: URL:";
        public const string RepositorioResarcimientos = "RepositorioResarcimientos";
        public const string Duplicada = "Información se encuentra registrada. Ingrese otro valor.";
        public const string ErrorDeSistema = "Error en el Sistema...";
        public const string Registrar = "Se Registro Correctamente";
        public const string Modificar = "Registro Modificado";
        public const string Eliminar = "Registro Eliminado";
        public const string Seleccione = "(SELECCIONE)";
        public const string Duplicada_Eliminado = "Período ha sido eliminado y no se puede volver a agregar. Contactar con TI.";
        public const string Parametro = "CICLO";
        public const string Parametro2 = "TENSIONNOMB";
        public const string Atributo = "NIVELTENSION";
        public const string Atributo2 = "CONFIGURACION";
        public const string PlantillaRC = "PlantillaRechazoCarga.xlsm";
    }

    public class HelperResarcimientos
    {
        public static string ListarPeriodoDebugLog(RntPeriodoDTO dto)
        {
            string result = "";
            result += "Se modifico correctamente {" + dto.PeriodoCodi + "}";
            result += "Se modifico correctamente {" + dto.PeriodoCodi + "}";
            result += "Se modifico correctamente {" + dto.PeriodoCodi + "}";
            return result;
        }
    }
}
