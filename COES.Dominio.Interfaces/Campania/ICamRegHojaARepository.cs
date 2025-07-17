using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Campania;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRegHojaARepository
    {

        List<RegHojaADTO> GetRegHojaAProyCodi( int proyCodi);
        
        bool SaveRegHojaA(RegHojaADTO regHojaADTO);

        bool DeleteRegHojaAById(int id, string usuario);

        int GetLastRegHojaAId();

        RegHojaADTO GetRegHojaAById(int id);

        bool UpdateRegHojaA(RegHojaADTO regHojaADTO);
        List<RegHojaADTO> GetRegHojaAByFilter(string plancodi, string empresa, string estado);



    }
}
