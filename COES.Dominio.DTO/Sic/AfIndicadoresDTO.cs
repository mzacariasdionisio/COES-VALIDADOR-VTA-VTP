using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class AfIndicadoresDTO : EntityBase
    {
        public int Afindicadorcodi { get; set; }
        public int Afecodi { get; set; }
        public int Mes { get; set; }
        public string MesNombre { get; set; }
        public int TotalEventosMes { get; set; }
        public int Diasinfctaf { get; set; }
        public int Diasinftec { get; set; }
        public int Limctaf { get; set; }
        public int Limit { get; set; }
        public decimal Indctaf { get; set; }
        public decimal Indit { get; set; }
        public DateTime Lastdate { get; set; }
        public string Lastuser { get; set; }
        public int Anio { get; set; }
    }
}
