using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdfp011DetRepository
    {
        List<Itcdfp011DetDTO> GetItcdfp011DetCodi(int proyCodi);
        bool SaveItcdfp011Det(List<Itcdfp011DetDTO> Itcdfp011DetDTOs, int itcdfp011Codi, string usuCreacion);
        bool DeleteItcdfp011DetById(int id, string usuario);
        int GetLastItcdfp011DetId();
        List<Itcdfp011DetDTO> GetItcdfp011DetById(int id);
        bool UpdateItcdfp011Det(Itcdfp011DetDTO Itcdfp011DetDTO);
        List<Itcdfp011DetDTO> GetItcdfp011DetByIdPag(int id, int offset, int pageSize);
        bool GetCloneItcdfp011DetById(int id, int newId);
    }
}
