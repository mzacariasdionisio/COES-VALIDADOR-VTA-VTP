using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRegHojaBRepository
    {
        List<RegHojaBDTO> GetRegHojaBProyCodi(int proyCodi);

        bool SaveRegHojaB(RegHojaBDTO regHojaBDTO);

        bool DeleteRegHojaBById(int id, string usuario);

        int GetLastRegHojaBId();

        RegHojaBDTO GetRegHojaBById(int id);

        bool UpdateRegHojaB(RegHojaBDTO regHojaBDTO);
        List<RegHojaBDTO> GetRegHojaBByFilter(string pericodi, string empresa, string estado);


    }
    
}
