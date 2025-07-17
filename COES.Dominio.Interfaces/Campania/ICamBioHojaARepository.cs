using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamBioHojaARepository
    {

        List<BioHojaADTO> GetBioHojaAProyCodi(int proyCodi);

        bool SaveBioHojaA(BioHojaADTO BioHojaADTO);

        bool DeleteBioHojaAById(int id, string usuario);

        int GetLastBioHojaAId();

        BioHojaADTO GetBioHojaAById(int id);

        bool UpdateBioHojaA(BioHojaADTO BioHojaADTO);

        List<BioHojaADTO> GetBioHojaAByFilter(string plancodi, string empresa, string estado);
    }
}
