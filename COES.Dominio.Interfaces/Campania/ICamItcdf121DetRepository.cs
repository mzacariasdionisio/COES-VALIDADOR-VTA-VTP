using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdf121DetRepository
    {

        List<Itcdf121DetDTO> GetItcdf121DetCodi(int proyCodi);
        bool SaveItcdf121Det(Itcdf121DetDTO itcdf121DetDTO);
        bool DeleteItcdf121DetById(int id, string usuario);
        int GetLastItcdf121DetId();
        Itcdf121DetDTO GetItcdf121DetById(int id);
        bool UpdateItcdf121Det(Itcdf121DetDTO itcdf121DetDTO);
    }
}
