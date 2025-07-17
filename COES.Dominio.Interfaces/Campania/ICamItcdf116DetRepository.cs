using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdf116DetRepository { 

        List<Itcdf116DetDTO> GetItcdf116DetCodi(int proyCodi);
        bool SaveItcdf116Det(Itcdf116DetDTO itcdf116DetDTO);
        bool DeleteItcdf116DetById(int id, string usuario);
        int GetLastItcdf116DetId();
        Itcdf116DetDTO GetItcdf116DetById(int id);
        bool UpdateItcdf116Det(Itcdf116DetDTO itcdf116DetDTO);
    
    }
}
