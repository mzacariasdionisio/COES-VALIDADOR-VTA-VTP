using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamFormatoD1BDETRepository
    {
        List<FormatoD1BDetDTO> GetFormatoD1BDETCodi(int proyCodi);
        bool SaveFormatoD1BDET(FormatoD1BDetDTO formatoD1BDETDTO);
        bool DeleteFormatoD1BDETById(int id, string usuario);
        int GetLastFormatoD1BDETId();
        FormatoD1BDetDTO GetFormatoD1BDETById(int id);
        //bool UpdateFormatoD1BDET(FormatoD1BDetDTO formatoD1BDETDTO);

    }
}
