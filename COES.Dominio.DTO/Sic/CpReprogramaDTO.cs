using System;

namespace COES.Dominio.DTO.Sic
{
    public class CpReprogramaDTO
    {
        public int? Reprogorden { get; set; }
        public int? Topcodi2 { get; set; }
        public int? Topcodi1 { get; set; }
        public int Reprogcodi { get; set; }

        public int Topiniciohora { get; set; }
        public string Topnombre { get; set; }
        public string Ordenreprog { get; set; }
        public string Lastuser { get; set; }
        public string Lastdate { get; set; }
        public string Hora { get; set; }
        public DateTime Topfecha { get; set; }
        public int Tophorareprog { get; set; }
        public string Topouserdespacho { get; set; }
        public int HoraReprograma { get; set; }
    }
}
