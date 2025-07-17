using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Campania;

namespace COES.Dominio.Interfaces.Campania
{
    public interface IcamITCFE01Repository
    {

        List<ITCFE01DTO> GetRegITCFE01ProyCodi(int proyCodi);

        bool SaveRegITCFE01(ITCFE01DTO itcFE01);

        bool DeleteRegITCFE01ById(int id, string usuario);

        int GetLastRegITCFE01Id();

        ITCFE01DTO GetRegITCFE01ById(int id);

        bool UpdateRegITCFE01(ITCFE01DTO itcFE01);


    }
}
