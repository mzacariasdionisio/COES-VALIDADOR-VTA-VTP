using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_HO_UNIDAD
    /// </summary>
    public partial class EveHoUnidadDTO : EntityBase
    {
        public int Hopunicodi { get; set; }
        public int Hopcodi { get; set; }
        public int Equicodi { get; set; }
        public string Equiabrev { get; set; }
        
        public DateTime? Hopunihorordarranq { get; set; }
        public DateTime? Hopunihorini { get; set; }
        public DateTime? Hopunihorfin { get; set; }
        public DateTime? Hopunihorarranq { get; set; }
        public DateTime? Hopunihorparada { get; set; }
        public int Hopuniactivo { get; set; }
        public string Hopuniusucreacion { get; set; }
        public DateTime? Hopunifeccreacion { get; set; }
        public string Hopuniusumodificacion { get; set; }
        public DateTime? Hopunifecmodificacion { get; set; }

        #region Titularidad-Instalaciones-Empresas

        public int Emprcodi { get; set; }

        #endregion

    }

    public partial class EveHoUnidadDTO
    {
        public DateTime? Hophorfin { get; set; }
        public DateTime? Hophorini { get; set; }
        public DateTime? Hophorordarranq { get; set; }
        public DateTime? Hophorparada { get; set; }

        public int OpcionCrud { get; set; }
    }
}
