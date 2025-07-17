using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.Interfaces.ReportesFrecuencia;
using COES.Infraestructura.Datos.Repositorio.ReportesFrecuencia;

namespace COES.Servicios.Aplicacion.Factory
{
    public class FactoryReportesFrecuencia
    {
        public static string StrConexion = "ContextoSIC";
        public static string StrConexionDTR = "ContextoDTR";

        //VALORIZACIÓN DE TRANSFERENCIA DE ENERGÍA ACTIVA

        /*public static IAreaRepository GetAreaRepository()
        {
            return new AreaRespository(StrConexion);
        }*/

        
        /// <summary>
        /// Periodo de declaracion que permitira administa los codigos VTA/VTP por periodo
        /// </summary>
        public static IEquipoGPSRepository GetEquipoGPSRepository()
        {
            return new EquipoGPSRepository(StrConexion);
        }
        
        public static IReporteFrecuenciaRepository GetReporteFrecuenciaRepository()
        {
            return new ReporteFrecuenciaRepository(StrConexion);
        }

        public static IReporteFrecuenciaAuditRepository ReporteFrecuenciaAuditRepository()
        {
            return new ReporteFrecuenciaAuditRepository(StrConexion);
        }

        public static IReporteSegundosFaltantesRepository GetReporteSegundosFaltantesRepository()
        {
            return new ReporteSegundosFaltantesRepository(StrConexion);
        }

        public static IEtapaERARepository GetEtapaERARepository()
        {
            return new EtapaERARepository(StrConexion);
        }

        public static ICargaVirtualRepository GetCargaVirtualRepository()
        {
            return new CargaVirtualRepository(StrConexion);
        }

        public static ICopiarInformacionRepository GetCopiarInformacionRepository()
        {
            return new CopiarInformacionRepository(StrConexion);
        }

        public static IExtraerFrecuenciaRepository GetExtraerFrecuenciaRepository()
        {
            return new ExtraerFrecuenciaRepository(StrConexion);
        }

        public static IInformacionFrecuenciaRepository GetInformacionFrecuenciaRepository()
        {
            return new InformacionFrecuenciaRepository(StrConexion);
        }


    }
}
