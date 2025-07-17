using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_MODELO_AGRUPACION
    /// </summary>
    public partial class CmModeloAgrupacionDTO : EntityBase
    {
        public int Modagrcodi { get; set; }
        public int Modcomcodi { get; set; }
        public int Modagrorden { get; set; }
    }

    public partial class CmModeloAgrupacionDTO
    {
        public List<CmModeloConfiguracionDTO> ListaConfiguracion { get; set; }
    }
}
