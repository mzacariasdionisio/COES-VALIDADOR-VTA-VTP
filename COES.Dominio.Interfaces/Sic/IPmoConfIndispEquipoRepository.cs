using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPmoConfIndispEquipoRepository
    {
        int Save(PmoConfIndispEquipoDTO entity);
        int Update(PmoConfIndispEquipoDTO entity);
        PmoConfIndispEquipoDTO GetById(int pmcindcodi);
        void EliminarCorrelacion(PmoConfIndispEquipoDTO entity);

        List<PmoConfIndispEquipoDTO> List(int emprcodi, int tsddpcodi, int famcodi);
    }
}
