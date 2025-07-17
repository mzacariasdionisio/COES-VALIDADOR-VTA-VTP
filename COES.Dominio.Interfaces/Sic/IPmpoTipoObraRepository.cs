using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMPO_TIPO_OBRA
    /// </summary>
    public interface IPmpoTipoObraRepository
    {
        List<PmpoTipoObraDTO> List();
    }
}
