using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_RELACION_INSUMOS
    /// </summary>
    public interface IPfRelacionInsumosRepository
    {
        int Save(PfRelacionInsumosDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PfRelacionInsumosDTO entity);
        void Delete(int pfrinscodi);
        PfRelacionInsumosDTO GetById(int pfrinscodi);
        List<PfRelacionInsumosDTO> List();
        List<PfRelacionInsumosDTO> GetByCriteria(int pfrptcodi);
    }
}
