using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_RELACION_INDISPONIBILIDAD
    /// </summary>
    public interface IPfrRelacionIndisponibilidadRepository
    {
        //int Save(PfrRelacionIndisponibilidadDTO entity);
        int Save(PfrRelacionIndisponibilidadDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PfrRelacionIndisponibilidadDTO entity);
        void Delete(int pfrrincodi);
        PfrRelacionIndisponibilidadDTO GetById(int pfrrincodi);
        List<PfrRelacionIndisponibilidadDTO> List();
        List<PfrRelacionIndisponibilidadDTO> GetByCriteria(int pfrrptcodi);
    }
}
