using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ABI_MEDIDORES_RESUMEN
    /// </summary>
    public interface IAbiMedidoresResumenRepository
    {
        int GetMaxId();
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(AbiMedidoresResumenDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(AbiMedidoresResumenDTO entity);
        void DeleteByRango(DateTime fechaIni, DateTime fechaFin, IDbConnection conn, DbTransaction tran);
        AbiMedidoresResumenDTO GetById(int mregencodi);
        List<AbiMedidoresResumenDTO> List();
        List<AbiMedidoresResumenDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin);
    }
}
