using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_FORMATO
    /// </summary>
    public interface IPmoFormatoRepository
    {
        List<PmoFormatoDTO> List();

        #region SIOSEIN
        List<PmoFormatoDTO> GetFormatPtomedicion(int pmftabcodi);
        #endregion
    }
}
