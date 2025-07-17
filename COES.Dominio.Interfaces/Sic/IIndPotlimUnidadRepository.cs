using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_POTLIM_UNIDAD
    /// </summary>
    public interface IIndPotlimUnidadRepository
    {
        int Save(IndPotlimUnidadDTO entity);
        void Update(IndPotlimUnidadDTO entity);
        void Delete(int equlimcodi);
        IndPotlimUnidadDTO GetById(int equlimcodi);
        List<IndPotlimUnidadDTO> List();
        List<IndPotlimUnidadDTO> GetByCriteria();
        int Save(IndPotlimUnidadDTO entity, IDbConnection connection, IDbTransaction transaction);
    }
}
