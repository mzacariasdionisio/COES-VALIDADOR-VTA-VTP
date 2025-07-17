using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ABI_PRODGENERACION
    /// </summary>
    public interface IAbiProdgeneracionRepository
    {
        int GetMaxId();
        int Save(AbiProdgeneracionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(AbiProdgeneracionDTO entity);
        void DeleteByRango(DateTime fechaIni, DateTime fechaFin, IDbConnection conn, DbTransaction tran);
        AbiProdgeneracionDTO GetById(int pgencodi);
        List<AbiProdgeneracionDTO> List(DateTime fechaIni, DateTime fechaFin, string flagIntegrante, string flagRER);
        List<AbiProdgeneracionDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin, string flagIntegrante, string flagRER);
    }
}
