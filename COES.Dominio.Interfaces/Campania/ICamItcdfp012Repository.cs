using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcdfp012Repository
    {
        List<Itcdfp012DTO> GetItcdfp012Codi(int proyCodi);
        bool SaveItcdfp012(Itcdfp012DTO Itcdfp012DTO);
        bool DeleteItcdfp012ById(int id, string usuario);
        int GetLastItcdfp012Id();
        List<Itcdfp012DTO> GetItcdfp012ById(int id);
        bool UpdateItcdfp012(Itcdfp012DTO Itcdfp012DTO);
        List<Itcdfp012DTO> GetItcdfp012ByFilter(string plancodi, string empresa, string estado);
    }
}
