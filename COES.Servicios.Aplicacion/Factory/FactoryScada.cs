using COES.Dominio.Interfaces.Scada;
using COES.Dominio.Interfaces.Sp7;
using COES.Infraestructura.Datos.Repositorio.Scada;
using COES.Infraestructura.Datos.Respositorio.Scada;
using COES.Infraestructura.Datos.Respositorio.Sp7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Factory
{
    public class FactoryScada
    {
        public static string StrConexionHIS = "ContextoHIS";
        public static string StrConexion = "ContextoSIC";
        public static string StrScada = "ContextoSCADA";
        public static string StrConexionSp7 = "ContextoSP7";

        #region Tablas en BD HIS

        public static IDatosSp7Repository GetDatosSp7Repository()
        {
            return new DatosSp7Repository(StrConexionHIS);
        }

        #endregion

        #region Tablas en BD SCADA

        public static ITrIndempresatSp7Repository GetTrIndempresatSp7Repository()
        {
            return new TrIndempresatSp7Repository(StrScada);
        }

        public static ITrEstadcanalSp7Repository GetTrEstadcanalSp7Repository()
        {
            return new TrEstadcanalSp7Repository(StrScada);
        }

        #endregion

        #region Tablas en BD SICOES

        public static IMeScadaFiltroSp7Repository GetMeScadaFiltroSp7Repository()
        {
            return new MeScadaFiltroSp7Repository(StrConexion);
        }

        public static IMeScadaPtofiltroSp7Repository GetMeScadaPtofiltroSp7Repository()
        {
            return new MeScadaPtofiltroSp7Repository(StrConexion);
        }

        public static IMeScadaSp7Repository GetMeScadaSp7Repository()
        {
            return new MeScadaSp7Repository(StrConexion);
        }

        public static IFLecturaSp7Repository GetFLecturaSp7Repository()
        {
            return new FLecturaSp7Repository(StrConexion);
        }

        public static IScEmpresaRepository GetScEmpresaRepository()
        {
            return new ScEmpresaRepository(StrConexion);
        }

        public static ITrCanalSp7Repository GetTrCanalSp7Repository()
        {
            return new TrCanalSp7Repository(StrConexion);
        }

        public static ITrEmpresaSp7Repository GetTrEmpresaSp7Repository()
        {
            return new TrEmpresaSp7Repository(StrConexion);
        }

        public static ITrLogdmpSp7Repository GetTrLogdmpSp7Repository()
        {
            return new TrLogdmpSp7Repository(StrConexion);
        }

        public static ITrObservacionCorreoRepository GetTrObservacionCorreoRepository()
        {
            return new TrObservacionCorreoRepository(StrConexion);
        }

        public static ITrObservacionEstadoRepository GetTrObservacionEstadoRepository()
        {
            return new TrObservacionEstadoRepository(StrConexion);
        }

        public static ITrObservacionItemEstadoRepository GetTrObservacionItemEstadoRepository()
        {
            return new TrObservacionItemEstadoRepository(StrConexion);
        }

        public static ITrObservacionItemRepository GetTrObservacionItemRepository()
        {
            return new TrObservacionItemRepository(StrConexion);
        }

        public static ITrObservacionRepository GetTrObservacionRepository()
        {
            return new TrObservacionRepository(StrConexion);
        }

        public static ITrZonaSp7Repository GetTrZonaSp7Repository()
        {
            return new TrZonaSp7Repository(StrConexion);
        }

        #endregion

        #region Tablas en BD SP7

        public static ITrCanalcambioSp7Repository GetTrCanalcambioSp7Repository()
        {
            return new TrCanalcambioSp7Repository(StrConexionSp7);
        }

        public static ITrCanalSp7Repository GetTrCanalSp7BdTrealRepository()
        {
            return new TrCanalSp7Repository(StrConexionSp7);
        }

        public static ITrCargaarchxmlSp7Repository GetTrCargaarchxmlSp7Repository()
        {
            return new TrCargaarchxmlSp7Repository(StrConexionSp7);
        }

        public static ITrCircularSp7RepositorySp7 GetTrCalidadSp7Repository()
        {
            return new TrCircularSp7RepositorySp7(StrConexionSp7);
        }

        public static ITrCircularSp7RepositorySp7 GetTrCircularSp7Repository()
        {
            return new TrCircularSp7RepositorySp7(StrConexionSp7);
        }

        public static ITrEmpresaSp7Repository GetTrEmpresaSp7BdTrealRepository()
        {
            return new TrEmpresaSp7Repository(StrConexionSp7);
        }

        public static ITrEstadcanalrSp7Repository GetTrEstadcanalrSp7Repository()
        {
            return new TrEstadcanalrSp7Repository(StrConexionSp7);
        }

        public static ITrMuestrarisSp7Repository GetTrMuestrarisSp7Repository()
        {
            return new TrMuestrarisSp7Repository(StrConexionSp7);
        }

        public static ITrObservacionRepository GetTrObservacionSp7Repository()
        {
            return new TrObservacionRepository(StrConexionSp7);
        }

        public static ITrReporteversionSp7Repository GetTrReporteversionSp7Repository()
        {
            return new TrReporteversionSp7Repository(StrConexionSp7);
        }

        public static ITrVersionSp7Repository GetTrVersionSp7Repository()
        {
            return new TrVersionSp7Repository(StrConexionSp7);
        }

        public static ITrZonaSp7Repository GetTrZonaSp7BdTrealRepository()
        {
            return new TrZonaSp7Repository(StrConexionSp7);
        }

        #endregion

    }

}
