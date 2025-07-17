using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCroFicha1DetRepository
    {
        List<CroFicha1DetDTO> GetCroFicha1DetCodi(int proyCodi);

        bool SaveCroFicha1Det(CroFicha1DetDTO CroFicha1DetDTO);

        bool DeleteCroFicha1DetById(int id, string usuario);

        int GetLastCroFicha1DetId();

        CroFicha1DetDTO GetCroFicha1DetById(int id);

        bool UpdateCroFicha1Det(CroFicha1DetDTO CroFicha1DetDTO);
    }
}
