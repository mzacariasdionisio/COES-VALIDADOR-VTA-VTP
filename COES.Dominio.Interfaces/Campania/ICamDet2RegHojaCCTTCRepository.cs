using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamDet2RegHojaCCTTCRepository
    {

        List<Det2RegHojaCCTTCDTO> GetDet2RegHojaCCTTCCentralCodi(int fichaCCodi);

        bool SaveDet2RegHojaCCTTC(Det2RegHojaCCTTCDTO detRegHojaCCTTCDTO);

        bool DeleteDet2RegHojaCCTTCById(int id, string usuario);

        int GetLastDet2RegHojaCCTTCId();

        Det2RegHojaCCTTCDTO GetDet2RegHojaCCTTCById(int id);

        bool UpdateDet2RegHojaCCTTC(Det2RegHojaCCTTCDTO detRegHojaCCTTCDTO);


    }
}
