using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdfp013Repository
    {

        List<Itcdfp013DTO> GetItcdfp013Codi(int proyCodi);
        bool SaveItcdfp013(Itcdfp013DTO Itcdfp013DTO);
        bool DeleteItcdfp013ById(int id, string usuario);
        int GetLastItcdfp013Id();
        List<Itcdfp013DTO> GetItcdfp013ById(int id);
        bool UpdateItcdfp013(Itcdfp013DTO Itcdfp013DTO);
        List<Itcdfp013DTO> GetItcdfp013ByFilter(string plancodi, string empresa, string estado);
    }
}
