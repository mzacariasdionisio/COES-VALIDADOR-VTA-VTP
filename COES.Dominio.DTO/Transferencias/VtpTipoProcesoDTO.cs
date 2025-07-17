using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_TIPO_PROCESO
    /// </summary>
    [Serializable]
    public class VtpTipoProcesoDTO: EntityBase
    {
        public string Tipprodescripcion { get; set; }
        public int Tipprocodi { get; set; }

        

    }
}
