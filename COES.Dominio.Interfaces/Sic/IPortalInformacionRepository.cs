using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de metodos del portal de informacion
    /// /// </summary>
    public interface IPortalInformacionRepository
    {
        DemandadiaDTO ObtenerResumenMaximaDemanda();
        List<MeMedicion48DTO> ProduccionxTipoCombustible(DateTime fechaInicial, DateTime fechaFinal);
    }
}
