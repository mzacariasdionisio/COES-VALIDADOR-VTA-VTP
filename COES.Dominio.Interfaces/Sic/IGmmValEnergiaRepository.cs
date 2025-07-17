using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IGmmValEnergiaRepository
    {
        int Save(GmmValEnergiaDTO entity);
        List<GmmValEnergiaDTO> ListarValores96Originales(GmmValEnergiaDTO valEnergiaDTO);
        List<GmmValEnergiaDTO> ListarValoresCostoMarginal(GmmValEnergiaDTO valEnergiaDTO, int anio, int mes);
        
    }

    
    

}
