using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRACION
    /// </summary>
    public interface ISiMigracionRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        int Save(SiMigracionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiMigracionDTO entity);
        void Delete(int migracodi);
        SiMigracionDTO GetById(int migracodi);
        List<SiMigracionDTO> List();
        List<SiMigracionDTO> GetByCriteria();
        List<SiMigracionDTO> ListarTransferenciasXEmpresaOrigenXEmpresaDestino(int iEmpresaOrigen, int iEmpresaDestino, string sDescripcion);

        List<SiMigracionDTO> ListarHistoricoEstadoEmpresa(string emprcodi, DateTime fechaConsulta);
        void UpdateMigraAnulacion(int migracodi, int migrareldeleted, string user, DateTime fechaActualizacion, IDbConnection conn, DbTransaction tran);
        void UpdateMigraProcesoPendiente(int migracodi, IDbConnection conn, DbTransaction tran);
        #region siosein2
        List<SiMigracionDTO> ListarTransferenciasXTipoMigracion(string tmopercodi, DateTime fechaInicio, DateTime fechaFin);
        #endregion
    }
}
