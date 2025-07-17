using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_GRUPOEQ
    /// </summary>
    public partial class PrGrupoeqDTO : EntityBase
    {
        public int Geqcodi { get; set; }
        public int Grupocodi { get; set; }
        public int Equicodi { get; set; }
        public DateTime? Geqfeccreacion { get; set; }
        public string Gequsucreacion { get; set; }
        public DateTime? Geqfecmodificacion { get; set; }
        public string Gequsumodificacion { get; set; }
        public int Geqactivo { get; set; }
    }

    public partial class PrGrupoeqDTO
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }

        public string Central { get; set; }
        public int Equipadre { get; set; }

        public string Equinomb { get; set; }
        public string Equiabrev { get; set; }
        public string Equiestado { get; set; }

        public string Grupotipomodo { get; set; }
        public string Gruponomb { get; set; }
        public string Grupoestado { get; set; }
        public string Grupoabrev { get; set; }
        public string Osinergcodi { get; set; }

        public int Fenergcodi { get; set; }
        public string Fenergnomb { get; set; }
    }
}
