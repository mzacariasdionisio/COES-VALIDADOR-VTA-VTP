using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdf104Repository
    {

        List<Itcdf104DTO> GetItcdf104Codi(int proyCodi);
        bool SaveItcdf104(Itcdf104DTO itcdf104DTO);
        bool DeleteItcdf104ById(int id, string usuario);
        int GetLastItcdf104Id();
        List<Itcdf104DTO> GetItcdf104ById(int id);
        bool UpdateItcdf104(Itcdf104DTO itcdf104DTO);
        List<Itcdf104DTO> GetItcdf104ByFilter(string plancodi, string empresa, string estado);

    }
}
