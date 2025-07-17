using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_EVENTO
    /// </summary>
    public interface IEveEventoRepository
    {
        int Save(EveEventoDTO entity);
        void Update(EveEventoDTO entity);
        void Delete(int evencodi);
        void Delete_UpdateAuditoria(int evencodi, string username);
        EveEventoDTO GetById(int evencodi);
        List<EveEventoDTO> ListEventos(int idPeriodo);
        List<EveEventoDTO> List();
        List<EveEventoDTO> GetByCriteria();
        List<EveEventoDTO> ConsultaEventoExtranet(DateTime fechaInicio, DateTime fechaFin,
            int? idTipoEvento, int nroPage, int pageSize);
        int ObtenerNroRegistrosConsultaExtranet(DateTime fechaInicio, DateTime fechaFin,
            int? idTipoEvento);
        EveEventoDTO GetDetalleEvento(int idEvento);
        void CambiarVersion(int idEvento, string version, string username);
        List<EveEventoDTO> ListarResumenEventosWeb(DateTime fecha);
        void ActualizarEventoAseguramiento(int idEvento);

        #region PR5
        List<EveEventoDTO> ObtenerEventoEquipo(string idsEquipo, DateTime fechaInicio, DateTime fechaFin, int evenclasecodi);
        List<EveEventoDTO> ListarReporteEventoIOED(string idEmpresa, string idCentral, string idUbicacion, string tipoevencodi, DateTime fechaIni, DateTime fechaFin);
        List<EveEventoDTO> GetEventosCausaSubCausa(string tipoequipo, string causaeven, DateTime fechaInicio, DateTime fechaFin);
        #endregion

        #region SIOSEIN
        List<EveEventoDTO> ObtenerEventosConInterrupciones(DateTime fechaInicio, DateTime fechaFin);
        List<EveEventoDTO> GetListaHechosRelevantes(DateTime fechaInicio, DateTime fechaFin);
        #endregion

        #region MigracionSGOCOES-GrupoB
        List<EveEventoDTO> ListaEventosImportantes(DateTime fecIni);
        #endregion

        #region Mejoras CTAF
        List<EveEventoDTO> ListadoEventoSco(DateTime fechaInicio, DateTime fechaFin);
        void UpdateEventoCtaf(int evencodi, string estado);
        void insertarEventoEvento(int evencodi, int evencodi_as);
        List<EveEventoDTO> ListadoEventosAsoCtaf(int evencodi);
        int ObtieneCantFileEnviadosSco(int evencodi);
        #endregion
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int insertarEventoEventoR(int evencodi, int evencodi_as, IDbConnection conn, DbTransaction tran);
        int UpdateEventoCtafR(int evencodi, string estado, IDbConnection conn, DbTransaction tran);
    }
}
