using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_CUADRO
    /// </summary>
    public interface IIndCuadroRepository
    {
        int Save(IndCuadroDTO entity);
        void Update(IndCuadroDTO entity);
        void Delete(int icuacodi);
        IndCuadroDTO GetById(int icuacodi);
        List<IndCuadroDTO> List();
        List<IndCuadroDTO> GetByCriteria();
    }
}
