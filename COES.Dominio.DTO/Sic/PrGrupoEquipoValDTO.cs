using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_GRUPO_EQUIPO_VAL
    /// </summary>
    public partial class PrGrupoEquipoValDTO : EntityBase
    {
        public int Grupocodi { get; set; } 
        public int Concepcodi { get; set; } 
        public int Equicodi { get; set; } 
        public DateTime Greqvafechadat { get; set; } 
        public string Greqvaformuladat { get; set; } 
        public int Greqvadeleted { get; set; } 
        public string Greqvausucreacion { get; set; } 
        public DateTime? Greqvafeccreacion { get; set; } 
        public string Greqvausumodificacion { get; set; } 
        public DateTime? Greqvafecmodificacion { get; set; }

        //mejoras Ficha Técnica
        public string Greqvacomentario { get; set; }
        public string Greqvasustento { get; set; }
        public int? Greqvacheckcero { get; set; }
    }

    public partial class PrGrupoEquipoValDTO
    {
        #region MigracionSGOCOES-GrupoB
        public string GreqvafechadatDesc { get; set; }
        public string EstadoDesc { get; set; }
        public string FechaactDesc { get; set; }
        public string Lastuser { get; set; }
        public int Greqvadeleted2 { get; set; }
        public decimal? ValorDecimal { get; set; }
        public string Concepabrev { get; set; }
        public int Conceppadre { get; set; }
        #endregion

        public bool EsSustentoConfidencial { get; set; }

        //INICIO: IND.PR25.2022
        public string Tipocombustible { get; set; }
        //FIN: IND.PR25.2022
    }
}
