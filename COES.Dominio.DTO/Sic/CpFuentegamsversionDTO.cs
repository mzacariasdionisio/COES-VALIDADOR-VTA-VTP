using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_FUENTEGAMS
    /// </summary>
    public class CpFuentegamsversionDTO : EntityBase
    {
        public int Fverscodi { get; set; } 
        public int Ftegcodi { get; set; }
        public int Fversnum { get; set; } 
        public string Fversdescrip { get; set; }
        public string Fversusumodificacion { get; set; } 
        public DateTime Fversfecmodificacion { get; set; }
        public int Fversestado { get; set; }
        public string Fversinputdata { get; set; }
        public string Fversruncase { get; set; }
        public byte[] Fverscodigoencrip { get; set; }
    }
}
