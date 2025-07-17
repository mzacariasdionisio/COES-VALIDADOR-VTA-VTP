using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamBioHojaCRepository
    {
        List<BioHojaCDTO> GetBioHojaCProyCodi(int proyCodi);

        bool SaveBioHojaC(BioHojaCDTO BioHojaCDTO);

        bool DeleteBioHojaCById(int id, string usuario);

        int GetLastBioHojaCId();

        BioHojaCDTO GetBioHojaCById(int id);

        bool UpdateBioHojaC(BioHojaCDTO BioHojaCDTO);

        List<DetBioHojaCDTO> GetBioHojaCByFilter(string plancodi, string empresa, string estado);
    }
}
