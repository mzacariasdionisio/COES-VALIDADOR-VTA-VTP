using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_FUENTEGAMS
    /// </summary>
    public class CpFuentegamsDTO : EntityBase
    {
        public int Ftegcodi { get; set; }
        public string Ftegnombre { get; set; }
        public byte[] Ftegcodigoencrip { get; set; }
        public string Ftegcentrales { get; set; }
        public string Ftegadmitancia { get; set; }
        public string Fteganexo { get; set; }
        public int? Ftegdefault { get; set; }
        public string Ftegusumodificacion { get; set; }
        public DateTime? Ftegfecmodificacion { get; set; }
        public int? Ftegestado { get; set; }
        public int? Ftemetodo { get; set; }
        public string Fteginputdata { get; set; }
        public string Ftegruncase { get; set; }

        public string Oficial { get; set; }
        public string Fversinputdata { get; set; }
        public string Fversruncase { get; set; }
        public byte[] Fverscodigoencrip { get; set; }
        public int Fversnum { get; set; }
        public int Fverscodi { get; set; }
        public string Fversdescrip { get; set; }
        public DateTime Fversfecmodificacion { get; set; }
    }
}
