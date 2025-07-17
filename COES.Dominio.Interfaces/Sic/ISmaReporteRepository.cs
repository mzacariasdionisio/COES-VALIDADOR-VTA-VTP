using System;
using System.Collections.Generic;

using System.Data; //STS
using System.Data.Common; //STS

using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de SMA_REPORTE
    /// </summary>
    public interface ISmaReporteRepository
    {
        List<SmaReporteDTO> List();
        
    }
}
