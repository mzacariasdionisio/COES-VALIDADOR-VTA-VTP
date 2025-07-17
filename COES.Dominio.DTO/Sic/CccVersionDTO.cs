using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CCC_VERSION
    /// </summary>
    public partial class CccVersionDTO : EntityBase
    {
        public int Cccvercodi { get; set; }
        public string Cccverhorizonte { get; set; }
        public DateTime Cccverfecha { get; set; }
        public int Cccvernumero { get; set; }
        public string Cccverestado { get; set; }
        public string Cccverobs { get; set; }
        public string Cccverrptcodis { get; set; }
        public string Cccverusucreacion { get; set; }
        public DateTime? Cccverfeccreacion { get; set; }
        public string Cccverusumodificacion { get; set; }
        public DateTime? Cccverfecmodificacion { get; set; }

    }

    public partial class CccVersionDTO
    {
        public string CccverfechaDesc { get; set; }
        public string CccverfeccreacionDesc { get; set; }
        public string CccverfecmodificacionDesc { get; set; }

        public List<string> ListaObs { get; set; } = new List<string>();
    }
}
