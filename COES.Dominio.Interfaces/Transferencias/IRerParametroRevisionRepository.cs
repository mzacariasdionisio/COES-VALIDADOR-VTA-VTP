using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IRerParametroRevisionRepository
    {
        int Save(RerParametroRevisionDTO entity);
        void Update(RerParametroRevisionDTO entity);
        void Delete(int rerParametroRevisionId);
        RerParametroRevisionDTO GetById(int rerParametroRevisionId);
        List<RerParametroRevisionDTO> List();
        List<RerParametroRevisionDTO> ListByRerpprcodiByTipo(int Rerpprcodi, string Rerpretipo);
        void DeleteAllByRerpprcodi(int Rerpprcodi);
    }
}

