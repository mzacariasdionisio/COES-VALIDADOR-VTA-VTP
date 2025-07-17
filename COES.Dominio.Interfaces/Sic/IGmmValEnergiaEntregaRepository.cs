using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IGmmValEnergiaEntregaRepository
    {
        int Save(GmmValEnergiaEntregaDTO entity);
        List<GmmValEnergiaEntregaDTO> ListarValores96Originales(GmmValEnergiaEntregaDTO valEnergiaDTO);
        List<GmmValEnergiaEntregaDTO> ListarValoresCostoMarginal(GmmValEnergiaEntregaDTO valEnergiaDTO, int anio, int mes);
        
    }

    
    

}
