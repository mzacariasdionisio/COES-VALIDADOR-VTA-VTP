using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EN_ENSAYOMODO
    /// </summary>
    public interface IEnEnsayomodoRepository
    {
        int Save(EnEnsayomodoDTO entity);
        void Update(EnEnsayomodoDTO entity);
        void Delete(int enmodocodi);
        EnEnsayomodoDTO GetById(int enmodocodi);
        List<EnEnsayomodoDTO> List();
        List<EnEnsayomodoDTO> GetByCriteria();
        List<EnEnsayomodoDTO> ListarEnsayosModo(int idEnsayo);
    }
}
