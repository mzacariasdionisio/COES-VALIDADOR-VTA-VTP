using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdf110DetRepository
    {

        List<Itcdf110DetDTO> GetItcdf110DetCodi(int proyCodi);
        bool SaveItcdf110Det(Itcdf110DetDTO itcdf110DetDTO);
        bool DeleteItcdf110DetById(int id, string usuario);
        int GetLastItcdf110DetId();
        Itcdf110DetDTO GetItcdf110DetById(int id);
        bool UpdateItcdf110Det(Itcdf110DetDTO itcdf110DetDTO);
    }
}
