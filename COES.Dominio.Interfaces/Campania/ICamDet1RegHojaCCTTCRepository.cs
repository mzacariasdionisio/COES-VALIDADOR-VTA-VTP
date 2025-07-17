using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Campania;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamDet1RegHojaCCTTCRepository
    {

        List<Det1RegHojaCCTTCDTO> GetDet1RegHojaCCTTCCentralCodi( int fichaCCodi);
        
        bool SaveDet1RegHojaCCTTC(Det1RegHojaCCTTCDTO detRegHojaCCTTCDTO);

        bool DeleteDet1RegHojaCCTTCById(int id, string usuario);

        int GetLastDet1RegHojaCCTTCId();

        Det1RegHojaCCTTCDTO GetDet1RegHojaCCTTCById(int id);

        bool UpdateDet1RegHojaCCTTC(Det1RegHojaCCTTCDTO detRegHojaCCTTCDTO);


    }
}
