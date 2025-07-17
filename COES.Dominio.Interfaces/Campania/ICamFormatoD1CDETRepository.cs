using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamFormatoD1CDETRepository
    {
        List<FormatoD1CDetDTO> GetFormatoD1CDETCodi(int proyCodi);
        bool SaveFormatoD1CDET(FormatoD1CDetDTO formatoD1CDETDTO);
        bool DeleteFormatoD1CDETById(int id, string usuario);
        int GetLastFormatoD1CDETId();
        FormatoD1CDetDTO GetFormatoD1CDETById(int id);
        //bool UpdateFormatoD1CDET(FormatoD1CDetDTO formatoD1CDETDTO);
    }
}
