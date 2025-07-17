using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_PLAZOPTO
    /// </summary>
    public interface IMePlazoptoRepository
    {
        int Save(MePlazoptoDTO entity);
        void Update(MePlazoptoDTO entity);
        void Delete(int plzptocodi);
        MePlazoptoDTO GetById(int plzptocodi);
        List<MePlazoptoDTO> List();
        List<MePlazoptoDTO> GetByCriteria();
    }
}
