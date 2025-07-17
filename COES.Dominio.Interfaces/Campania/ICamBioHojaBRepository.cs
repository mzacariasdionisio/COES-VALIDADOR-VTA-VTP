using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamBioHojaBRepository
    {
        List<BioHojaBDTO> GetBioHojaBProyCodi(int proyCodi);

        bool SaveBioHojaB(BioHojaBDTO BioHojaBDTO);

        bool DeleteBioHojaBById(int id, string usuario);

        int GetLastBioHojaBId();

        BioHojaBDTO GetBioHojaBById(int id);

        bool UpdateBioHojaB(BioHojaBDTO BioHojaBDTO);

        List<BioHojaBDTO> GetBioHojaBByFilter(string plancodi, string empresa, string estado);
    }
}
