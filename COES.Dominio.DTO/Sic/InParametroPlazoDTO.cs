using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_PARAMETROPLAZO
    /// </summary>
    public class InParametroPlazoDTO : EntityBase
    {
        public int Parplacodi { get; set; }
        public string Parpladesc { get; set; }
        public DateTime? Parplafecdesde { get; set; }
        public DateTime Parplafechasta { get; set; }
        public string Parplahora { get; set; }
        public string Parplasucreacion { get; set; }
        public DateTime? Parplafeccreacion { get; set; }
        public string Parplausumodificacion { get; set; }
        public DateTime? Parplafecmodificacion { get; set; }

        public int Progrcodi { get; set; }
    }
}
