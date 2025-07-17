using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdf116Repository
    {

        List<Itcdf116DTO> GetItcdf116Codi(int proyCodi);
        bool SaveItcdf116(Itcdf116DTO itcdf116DTO);
        bool DeleteItcdf116ById(int id, string usuario);
        int GetLastItcdf116Id();
        List<Itcdf116DTO> GetItcdf116ById(int id);
        bool UpdateItcdf116(Itcdf116DTO itcdf116DTO);

        List<Itcdf116DTO> GetItcdf116ByFilter(string plancodi, string empresa, string estado);


    }
}
