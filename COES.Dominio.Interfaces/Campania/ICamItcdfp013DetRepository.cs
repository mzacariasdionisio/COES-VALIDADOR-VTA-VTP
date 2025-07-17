using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdfp013DetRepository
    {
        List<Itcdfp013DetDTO> GetItcdfp013DetCodi(int proyCodi);
        bool SaveItcdfp013Det(Itcdfp013DetDTO Itcdfp013DetDTO);
        bool DeleteItcdfp013DetById(int id, string usuario);
        int GetLastItcdfp013DetId();
        Itcdfp013DetDTO GetItcdfp013DetById(int id);
        bool UpdateItcdfp013Det(Itcdfp013DetDTO Itcdfp013DetDTO);
    }
}
