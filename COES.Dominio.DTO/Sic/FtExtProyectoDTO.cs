using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_PROYECTO
    /// </summary>
    public partial class FtExtProyectoDTO : EntityBase
    {
        public int Ftprycodi { get; set; }
        public string Ftprynombre { get; set; }
        public string Ftpryeonombre { get; set; }
        public string Ftpryeocodigo { get; set; }
        public int Emprcodi { get; set; }
        public int? Esteocodi { get; set; }
        public string Ftpryestado { get; set; }
        public string Ftpryusucreacion { get; set; }
        public DateTime? Ftpryfeccreacion { get; set; }
        public string Ftpryusumodificacion { get; set; }
        public DateTime? Ftpryfecmodificacion { get; set; }
    }

    public partial class FtExtProyectoDTO
    {
        public string Esteocodiusu { get; set; }
        public string Emprnomb { get; set; }
        public int ConEstudioEo { get; set; }

        public string FtpryestadoDesc { get; set; }
        public string FechaCreaciónDesc { get; set; }
        public string FechaModificacionDesc { get; set; }
    }

}
