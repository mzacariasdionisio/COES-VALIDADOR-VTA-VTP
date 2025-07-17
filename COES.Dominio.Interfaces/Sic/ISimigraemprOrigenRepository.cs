using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRAEMPRORIGEN
    /// </summary>
    public interface ISiMigraemprOrigenRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(SiMigraemprOrigenDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiMigraemprOrigenDTO entity);
        void Delete(int migempcodi);
        SiMigraemprOrigenDTO GetById(int migempcodi);
        List<SiMigraemprOrigenDTO> List();
        List<SiMigraemprOrigenDTO> GetByCriteria();
        List<SiMigraemprOrigenDTO> ListadoTransferenciaXEmprOrigenXEmprDestino(int iEmpresaOrigen, int iEmpresaDestino, string SDescripcion);
    }
}
