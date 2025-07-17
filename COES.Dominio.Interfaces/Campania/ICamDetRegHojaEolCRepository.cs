using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamDetRegHojaEolCRepository
    {
        List<DetRegHojaEolCDTO> GetDetRegHojaEolCCodi(int id);

        bool SaveDetRegHojaEolC(DetRegHojaEolCDTO detRegHojaEolCDTO);

        bool DeleteDetRegHojaEolCById(int id, string usuario);

        int GetLastDetRegHojaEolCId();

        DetRegHojaEolCDTO GetDetRegHojaEolCById(int id);

        bool UpdateDetRegHojaEolC(DetRegHojaEolCDTO detRegHojaEolCDTO);
    }
}
