using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_GRUPO_EXCLUIDO
    /// </summary>
    public interface IVceCfgValidaConceptoRepository
    {
        VceCfgValidaConceptoDTO GetById(int crcvalcodi);
        List<VceCfgValidaConceptoDTO> ListValidaConceptos();
        
    }
}
