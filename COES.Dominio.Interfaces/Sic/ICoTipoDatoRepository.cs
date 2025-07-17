using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_TIPO_DATO
    /// </summary>
    public interface ICoTipoDatoRepository
    {
        int Save(CoTipoDatoDTO entity);
        void Update(CoTipoDatoDTO entity);
        void Delete(int cotidacodi);
        CoTipoDatoDTO GetById(int cotidacodi);
        List<CoTipoDatoDTO> List();
        List<CoTipoDatoDTO> GetByCriteria(string tipoDato);
    }
}
