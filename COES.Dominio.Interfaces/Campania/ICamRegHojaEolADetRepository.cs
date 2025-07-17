using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRegHojaEolADetRepository
    {

        List<RegHojaEolADetDTO> GetRegHojaEolADetCodi(int id);

        bool SaveRegHojaEolADet(RegHojaEolADetDTO regHojaEolADetDTO);

        bool DeleteRegHojaEolADetById(int id, string usuario);

        int GetLastRegHojaEolADetId();

        RegHojaEolADetDTO GetRegHojaEolADetById(int id);
    }
}
