using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_YUPCON_ENVIO
    /// </summary>
    public partial class CpYupconEnvioDTO : EntityBase
    {
        public int Cyupcodi { get; set; }
        public DateTime Cyupfecha { get; set; }
        public int Cyupbloquehorario { get; set; }
        public int Topcodi { get; set; }
        public int Tyupcodi { get; set; }
        public string Cyupusuregistro { get; set; }
        public DateTime Cyupfecregistro { get; set; }
    }

    public partial class CpYupconEnvioDTO
    {
        public string CyupfechaDesc { get; set; }
        public string CyupbloquehorarioDesc { get; set; }
        public string CyupfecregistroDesc { get; set; }

    }
}
