using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamDetBioHojaCRepository
    {
        List<DetBioHojaCDTO> GetDetBioHojaCCodi(int id);

        bool SaveDetBioHojaC(DetBioHojaCDTO detBioHojaCDTO);

        bool DeleteDetBioHojaCById(int id, string usuario);

        int GetLastDetBioHojaCId();

        DetBioHojaCDTO GetDetBioHojaCById(int id);

        bool UpdateDetBioHojaC(DetBioHojaCDTO detBioHojaCDTO);


    }
}
