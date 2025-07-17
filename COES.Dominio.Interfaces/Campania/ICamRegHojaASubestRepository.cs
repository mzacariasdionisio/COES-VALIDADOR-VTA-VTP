using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Campania;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRegHojaASubestRepository
    {

        List<RegHojaASubestDTO> GetRegHojaASubestProyCodi( int proyCodi);
        
        bool SaveRegHojaASubest(RegHojaASubestDTO regHojaASubestDTO);

        bool DeleteRegHojaASubestById(int id, string usuario);

        int GetLastRegHojaASubestId();

        RegHojaASubestDTO GetRegHojaASubestById(int id);

        bool UpdateRegHojaASubest(RegHojaASubestDTO regHojaASubestDTO);


    }
}
