using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IRerSddpRepository
    {
        int Save(RerSddpDTO entity);
        void Update(RerSddpDTO entity);
        void Delete(int rerSddpId);
        RerSddpDTO GetById(int rerSddpId);
        List<RerSddpDTO> List();
    }
}

