using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea una consulta
    /// </summary>
    public class ReporteCostoIncrementalDTO
    {
        public string Empresa { get; set; }
        public string GrupoModoOperacion { get; set; }
        public string CEC { get; set; }
        public string Pe { get; set; }
        public string Rendimiento { get; set; }
        public string Precio { get; set; }
        public string CVNC { get; set; }
        public string CVC { get; set; }
        public string CV { get; set; }
        public string Tramo1 { get; set; }
        public double Cincrem1 { get; set; }
        public string Tramo2 { get; set; }
        public double Cincrem2 { get; set; }
        public string Tramo3 { get; set; }
        public double Cincrem3 { get; set; }
        public string TipoCombustible { get; set; }
        public int Grupocodi { get; set; }
        public string TipoGenerRer { get; set; }
        public string Grupotipocogen { get; set; }
        public int Ptomedicodi { get; set; }
        public int? Grupopadre { get; set; }
        public double CCombXArr { get; set; }
        public double CCombXPar { get; set; }
        public double TCambio { get; set; }

        public decimal Pe1 { get; set; }
        public decimal Pe2 { get; set; }
        public decimal Pe3 { get; set; }
        public decimal Pe4 { get; set; }
    }
}
