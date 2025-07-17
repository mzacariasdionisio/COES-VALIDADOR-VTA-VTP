using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_RSFHORA
    /// </summary>
    public class EveRsfhoraDTO : EntityBase
    {
        public int Rsfhorcodi { get; set; }
        public string Rsfhorindman { get; set; }
        public string Rsfhorindaut { get; set; }
        public string Rsfhorcomentario { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public DateTime? Rsfhorfecha { get; set; }
        public DateTime? Rsfhorinicio { get; set; }
        public DateTime? Rsfhorfin { get; set; }
        public decimal? Rsfhormaximo { get; set; }
        public string Ursnomb { get; set; }
        public decimal Valorautomatico { get; set; }
        public int Indicador { get; set; }
        public string Rsfhorindxml { get; set; }
        public int rsfadd { get; set; }
        public int rsfdel { get; set; }
    }
}
