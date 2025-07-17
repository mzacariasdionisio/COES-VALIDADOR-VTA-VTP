using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdf108Repository
    {
        List<Itcdf108DTO> GetItcdf108Codi(int proyCodi);
        bool SaveItcdf108(Itcdf108DTO itcdf108DTO);
        bool DeleteItcdf108ById(int id, string usuario);
        int GetLastItcdf108Id();
        List<Itcdf108DTO> GetItcdf108ById(int id);
        bool UpdateItcdf108(Itcdf108DTO itcdf108DTO);
        List<Itcdf108DTO> GetItcdf108ByFilter(string plancodi, string empresa, string estado);
    }
}
