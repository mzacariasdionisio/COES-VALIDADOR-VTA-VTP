using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_FUENTEDATOS
    /// </summary>
    public interface ISiFuentedatosRepository
    {
        SiFuentedatosDTO GetById(int fdatcodi);

        #region PR5
        List<SiFuentedatosDTO> GetByModulo(int ModCodi);
        List<SiFuentedatosDTO> List();
        #endregion
    }
}
