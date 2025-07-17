using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Campania;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRegHojaCRepository
    {

        List<RegHojaCDTO> GetRegHojaCProyCodi( int proyCodi);
        
        bool SaveRegHojaC(RegHojaCDTO regHojaCDTO);

        bool DeleteRegHojaCById(int id, string usuario);

        int GetLastRegHojaCId();

        RegHojaCDTO GetRegHojaCById(int id);

        bool UpdateRegHojaC(RegHojaCDTO regHojaCDTO);

        List<DetRegHojaCDTO> GetRegHojaCByFilter(string plancodi, string empresa, string estado);
    }
}
