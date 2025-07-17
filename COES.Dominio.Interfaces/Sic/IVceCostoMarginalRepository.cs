using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_COSTO_MARGINAL
    /// </summary>
    public interface IVceCostoMarginalRepository
    {
        List<string> LstBodyCostoMarginal(int pericodi, int cosmarversion);
        List<string> LstHeadCostoMarginal(int pericodi);

        List<int> LstGrupos();
    }
}
