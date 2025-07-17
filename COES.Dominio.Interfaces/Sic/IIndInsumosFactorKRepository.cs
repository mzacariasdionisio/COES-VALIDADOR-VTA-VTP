using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_INSUMOS_FACTORK
    /// </summary>
    public interface IIndInsumosFactorKRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        IndInsumosFactorKDTO GetById(int insfckcodi);
        IndInsumosFactorKDTO GetByCriteria(int ipericodi, int emprcodi, int equicodicentral, int equicodiunidad, int grupocodi, int famcodi);
        List<IndInsumosFactorKDTO> GetByPeriodo(int ipericodi);
        void Save(IndInsumosFactorKDTO entity, IDbConnection conn, DbTransaction tran);
        void UpdateFRC(int insfckcodi, decimal insfckfrc, string insfckusumodificacion, DateTime insfckfecmodificacion, IDbConnection conn, DbTransaction tran);
        void UpdateFRCByImport(int insfckcodi, decimal insfckfrc, string insfckusuultimp, DateTime insfckfecultimp, string insfckranfecultimp, IDbConnection conn, DbTransaction tran);
    }
}
