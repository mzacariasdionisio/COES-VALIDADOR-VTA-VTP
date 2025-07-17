using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_FUENTEENERGIA
    /// </summary>
    public interface ISiFuenteenergiaRepository
    {
        void Update(SiFuenteenergiaDTO entity);
        SiFuenteenergiaDTO GetById(int fenergcodi);
        List<SiFuenteenergiaDTO> List();
        List<SiFuenteenergiaDTO> GetByCriteria();
        List<SiFuenteenergiaDTO> ListTipoCombustibleXTipoCentral(string famcodi, string idEmpresa);

        #region PR5
        List<SiFuenteenergiaDTO> ListTipoCombustibleXEquipo(string equicodi);
        #endregion

        #region SIOSEIN        
        List<SiFuenteenergiaDTO> PromedioEnergiaActivaPorTipodeRecursoYrangoFechas(string fechaI, string fechaF);
        #endregion
    }
}
