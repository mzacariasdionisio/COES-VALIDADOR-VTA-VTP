using System;

namespace COES.Dominio.DTO.Sic
{
    public class CmDemandatotalDTO
    {
        public int Demacodi { get; set; }
        public DateTime? Demafecha { get; set; }
        public int Demaintervalo { get; set; }
        public decimal Dematermica { get; set; }
        public decimal Demahidraulica { get; set; }
        public decimal Dematotal { get; set; }
        public string Demasucreacion { get; set; }
        public DateTime? Demafeccreacion { get; set; }
        public string Demausumodificacion { get; set; }
        public DateTime? Demafecmodificacion { get; set; }


    }
}
