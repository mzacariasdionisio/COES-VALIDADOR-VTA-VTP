
using COES.Dominio.DTO.Campania;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Campanias.Models
{
    public class CampaniasModel
    {
        public long Proycodi { get; set; }
        public int Plancodi { get; set; }
        public string Modo { get; set; }

        public TransmisionProyectoDTO TransmisionProyecto { get; set; }

        public List<int> ListaHojas { get; set; }

        public List<PlanTransmisionDTO> ListaPlanTransmicion { get; set; }

        public List<ObservacionDTO> ListaObservacion { get; set; }
    }
}