using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_MES
    /// </summary>
    public partial class PmoMesDTO : EntityBase
    {
        public int Pmmescodi { get; set; }
        public int Pmmesaniomes { get; set; }
        public int Pmmesestado { get; set; }
        public int? Pmmesprocesado { get; set; }
        public DateTime Pmmesfecini { get; set; }
        public DateTime Pmmesfecfin { get; set; }
        public DateTime Pmmesfecinimes { get; set; }
        public int Pmanopcodi { get; set; }
        public string Pmmesusucreacion { get; set; }
        public DateTime? Pmmesfeccreacion { get; set; }
        public string Pmusumodificacion { get; set; }
        public DateTime? Pmfecmodificacion { get; set; }
    }

    public partial class PmoMesDTO
    {
        public string NombreMes { get; set; }
        public int NroSemana { get; set; }
        public string FechaIniDesc { get; set; }

        public int? Pmanopanio { get; set; }
        public DateTime? Pmanopfecini { get; set; }

        public string PmmesfeccreacionDesc { get; set; }
        public string PmfecmodificacionDesc { get; set; }
        public string ProcesadoDesc { get; set; }
    }
}
