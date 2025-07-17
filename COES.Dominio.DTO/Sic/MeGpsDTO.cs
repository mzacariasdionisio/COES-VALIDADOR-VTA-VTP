using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_GPS
    /// </summary>
    public partial class MeGpsDTO : EntityBase
    {
        public int Gpscodi { get; set; }
        public int? Emprcodi { get; set; }
        public int? Equicodi { get; set; }
        public string Nombre { get; set; }
        public string Gpsoficial { get; set; }
        public string Gpsestado { get; set; }
        public string Gpsosinerg { get; set; }
        public string Gpsindieod { get; set; }
    }

    public partial class MeGpsDTO
    {
        public string NombreYEstado { get; set; }
        public string Emprnomb { get; set; }
    }
}
