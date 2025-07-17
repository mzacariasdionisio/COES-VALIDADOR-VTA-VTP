using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RCG_COSTOMARGINAL_CAB
    /// </summary>
    public interface IRcgCostoMarginalCabRepository
    {
        int Save(RcgCostoMarginalCabDTO entity);
        int Update(RcgCostoMarginalCabDTO entity);
        List<RcgCostoMarginalCabDTO> ListCostoMarginalCab(int pericodi, int recacodi);
        
    }
}

