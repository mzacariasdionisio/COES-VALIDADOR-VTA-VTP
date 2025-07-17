using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AF_CONDICIONES
    /// </summary>
    public class AfCondicionesDTO : EntityBase
    {
        public DateTime? Afcondfecmodificacion { get; set; }
        public string Afcondusumodificacion { get; set; }
        public DateTime? Afcondfeccreacion { get; set; }
        public string Afcondusucreacion { get; set; }
        public int? Afcondestado { get; set; }
        public string Afcondzona { get; set; }
        public int? Afcondnumetapa { get; set; }
        public string Afcondfuncion { get; set; }
        public int Afcondcodi { get; set; }
        public int Afecodi { get; set; }

        //
        public string Afcondnumetapadescrip { get; set; }
        public string AfcondzonaDesc { get; set; }
        public string Evencodi { get; set; }
    }
}
