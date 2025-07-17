using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_TIPO_PROCESO
    /// </summary>
    [Serializable]
    public class VtpTipoAplicacionDTO: EntityBase
    {
            
        public string Tipaplinombre { get; set; }
        public int Tipaplicodi { get; set; }

    }
}
