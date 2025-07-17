using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCroFicha1Repository
    {
        List<CroFicha1DTO> GetCroFicha1ProyCodi(int proyCodi);

        bool SaveCroFicha1(CroFicha1DTO CroFicha1DTO);

        bool DeleteCroFicha1ById(int id, string usuario);

        int GetLastCroFicha1Id();

        CroFicha1DTO GetCroFicha1ById(int id);

        bool UpdateCroFicha1(CroFicha1DTO CroFicha1DTO);

        List<CroFicha1DetDTO> GetCroFicha1ByFilter(string plancodi, string empresa, string estado);
    }
}
