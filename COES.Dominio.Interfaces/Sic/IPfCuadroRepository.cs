using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_CUADRO
    /// </summary>
    public interface IPfCuadroRepository
    {
        int Save(PfCuadroDTO entity);
        void Update(PfCuadroDTO entity);
        void Delete(int pfcuacodi);
        PfCuadroDTO GetById(int pfcuacodi);
        List<PfCuadroDTO> List();
        List<PfCuadroDTO> GetByCriteria();
    }
}
