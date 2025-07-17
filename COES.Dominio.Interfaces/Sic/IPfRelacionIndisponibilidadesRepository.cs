using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_RELACION_INDISPONIBILIDADES
    /// </summary>
    public interface IPfRelacionIndisponibilidadesRepository
    {
        int Save(PfRelacionIndisponibilidadesDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PfRelacionIndisponibilidadesDTO entity);
        void Delete(int pfrindcodi);
        PfRelacionIndisponibilidadesDTO GetById(int pfrindcodi);
        List<PfRelacionIndisponibilidadesDTO> List();
        List<PfRelacionIndisponibilidadesDTO> GetByCriteria(int pfrptcodi);
    }
}
