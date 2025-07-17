using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_GASEODUCTOXCENTRAL
    /// </summary>
    public interface IIndGaseoductoxcentralRepository
    {
        int Save(IndGaseoductoxcentralDTO entity);
        void Update(IndGaseoductoxcentralDTO entity);
        void Delete(int gasctrcodi);
        IndGaseoductoxcentralDTO GetById(int gasctrcodi);
        List<IndGaseoductoxcentralDTO> List();
        List<IndGaseoductoxcentralDTO> GetByCriteria();
        bool Inactivar(int gasctrcodi);
    }
}
