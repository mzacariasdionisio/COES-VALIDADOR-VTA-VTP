using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class PrGrupoConceptoDato : EntityBase
    {
        public int? ConceptoCatecodi { get; set; }
        public int Concepcodi { get; set; }
        public string Concepabrev { get; set; }
        public string Concepdesc { get; set; }
        public string Concepunid { get; set; }
        public string Conceptipo { get; set; }
        public int? Conceporden { get; set; }
        public int Grupocodi { get; set; }
        public int? GrupoCatecodi { get; set; }
        public DateTime Fechadat { get; set; }
        public string Lastuser { get; set; }
        public string Formuladat { get; set; }
        public int Deleted { get; set; }
        public DateTime? Fechaact { get; set; }
        public Decimal? Potefectiva { get; set; }
        public Decimal? Potminima { get; set; }
        public Decimal? PorcentajeRPF { get; set; }
        public Decimal? Vartermica { get; set; }
        public Decimal? PrecioOsinergmin { get; set; }
        public Decimal? TipoCambio { get; set; }
        public string Moneda { get; set; }
        public string UnidadPotencia { get; set; }
        
    }
}
