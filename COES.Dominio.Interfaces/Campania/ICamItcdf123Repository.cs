using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdf123Repository
    {
        List<Itcdf123DTO> GetItcdf123Codi(int proyCodi);
        bool SaveItcdf123(Itcdf123DTO itcdf123DTO);
        bool DeleteItcdf123ById(int id, string usuario);
        int GetLastItcdf123Id();
        List<Itcdf123DTO> GetItcdf123ById(int id);
        bool UpdateItcdf123(Itcdf123DTO itcdf123DTO);
        List<Itcdf123DTO> GetItcdf123ByFilter(string plancodi, string empresa, string estado);
    }
}
