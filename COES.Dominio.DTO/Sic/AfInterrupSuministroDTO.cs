using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AF_INTERRUP_SUMINISTRO
    /// </summary>
    public class AfInterrupSuministroDTO : EntityBase
    {
        public int Enviocodi { get; set; }
        public decimal? Intsummwred { get; set; }
        public decimal? Intsummwfin { get; set; }
        public string Intsumsuministro { get; set; }
        public string Intsumobs { get; set; }
        public int Intsumnumetapa { get; set; }
        public decimal? Intsumduracion { get; set; }
        public string Intsumfuncion { get; set; }
        public DateTime? Intsumfechainterrfin { get; set; }
        public DateTime? Intsumfechainterrini { get; set; }
        public decimal? Intsummw { get; set; }
        public string Intsumsubestacion { get; set; }
        public string Intsumempresa { get; set; }
        public string Intsumzona { get; set; }
        public int Intsumcodi { get; set; }
        public int Afecodi { get; set; }
        public DateTime? Intsumfeccreacion { get; set; }
        public string Intsumusucreacion { get; set; }
        public int Intsumestado { get; set; }

        //
        public int Eracmfcodi { get; set; }
        public string Intsummw2 { get; set; }
        public string Intsummwfin2 { get; set; }
        public string Intsumfechainterrfin2 { get; set; }
        public string Intsumfechainterrini2 { get; set; }

        public int Fdatcodi { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string CodigoOsinergmin { get; set; }
        public int EVENCODI { get; set; }
        public DateTime EVENINI { get; set; }
        public string EmpresaSuministradora { get; set; }
        public string Afemprnomb { get; set; }       
    }
}
