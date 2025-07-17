using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdfp011Repository
    {

        List<Itcdfp011DTO> GetItcdfp011Codi(int proyCodi);
        bool SaveItcdfp011(Itcdfp011DTO Itcdfp011DTO);
        bool DeleteItcdfp011ById(int id, string usuario);
        int GetLastItcdfp011Id();
        Itcdfp011DTO GetItcdfp011ById(int id);
        bool UpdateItcdfp011(Itcdfp011DTO Itcdfp011DTO);

        List<Itcdfp011DTO> GetItcdfp011ByFilter(string plancodi, string empresa, string estado);
    }
}
