using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_PROGRAMAAUDITORIA
    /// </summary>
    public class AudProgramaauditoriaDTO : EntityBase
    {
        public int Progacodi { get; set; }
        public int Audicodi { get; set; }
        public int Tabcdcoditipoactividad { get; set; }
        public int Tabcdcodiestadoactividad { get; set; }
        public DateTime? Progafecha { get; set; }
        public string Progahorainicio { get; set; }
        public string Progahorafin { get; set; }
        public string Progaactivo { get; set; }
        public string Progahistorico { get; set; }
        public string Progausucreacion { get; set; }
        public DateTime? Progafeccreacion { get; set; }
        public string Progausumodificacion { get; set; }
        public DateTime? Progafecmodificacion { get; set; }

        public List<int> Elemcodi { get; set; }
        public List<int> Percodiequipo { get; set; }
        public List<int> Percodiinvolucrado { get; set; }
        public List<int> Audpproceso { get; set; }

        public string Tipoactividad { get; set; }
        public string Tipoelemento { get; set; }
        public int Tipoelementocodi { get; set; }
        public int Elemcod { get; set; }
        public string Elemdescripcion { get; set; }
        public string Equipo { get; set; }
        public string Responsables { get; set; }
        public string Elemcodigo { get; set; }

        public int Progaecodi { get; set; }
        

        public int Areacodi { get; set; }
    }
}
