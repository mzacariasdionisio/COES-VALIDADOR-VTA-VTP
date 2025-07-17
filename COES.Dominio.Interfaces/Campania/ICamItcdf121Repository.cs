using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdf121Repository
    {

        List<Itcdf121DTO> GetItcdf121Codi(int proyCodi);
        bool SaveItcdf121(Itcdf121DTO itcdf121DTO);
        bool DeleteItcdf121ById(int id, string usuario);
        int GetLastItcdf121Id();
        List<Itcdf121DTO> GetItcdf121ById(int id);
        bool UpdateItcdf121(Itcdf121DTO itcdf121DTO);
        List<Itcdf121DTO> GetItcdf121ByFilter(string plancodi, string empresa, string estado);
    }
}
