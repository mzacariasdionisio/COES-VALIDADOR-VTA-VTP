using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamDetSolHojaCRepository
    {

        List<DetSolHojaCDTO> GetDetSolHojaCCodi(int id);

        bool SaveDetSolHojaC(DetSolHojaCDTO detSolHojaCDTO);

        bool DeleteDetSolHojaCById(int id, string usuario);

        int GetLastDetSolHojaCId();

        DetSolHojaCDTO GetDetSolHojaCById(int id);

        bool UpdateDetSolHojaC(DetSolHojaCDTO detSolHojaCDTO);
    }
}
