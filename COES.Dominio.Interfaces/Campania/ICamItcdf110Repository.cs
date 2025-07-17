using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdf110Repository
    {

        List<Itcdf110DTO> GetItcdf110Codi(int proyCodi);
        bool SaveItcdf110(Itcdf110DTO itcdf110DTO);
        bool DeleteItcdf110ById(int id, string usuario);
        int GetLastItcdf110Id();
        List<Itcdf110DTO> GetItcdf110ById(int id);
        bool UpdateItcdf110(Itcdf110DTO itcdf110DTO);
        List<Itcdf110DTO> GetItcdf110ByFilter(string plancodi, string empresa, string estado);

    }
}
