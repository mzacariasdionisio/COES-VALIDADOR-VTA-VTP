using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_AUDITORIAPLANIFICADA
    /// </summary>
    public class AudAuditoriaplanificadaDTO : EntityBase
    {
        public int Audpcodi { get; set; } 
        public int Plancodi { get; set; } 
        public string Audpnombre { get; set; }
        public string Audpcodigo { get; set; }
        public string Audpmesinicio { get; set; } 
        public string Audpmesfin { get; set; } 
        public string Audpdactivo { get; set; } 
        public string Audphistorico { get; set; } 
        public string Audpusucreacion { get; set; } 
        public DateTime? Audpfeccreacion { get; set; } 
        public string Audpusumodificacion { get; set; } 
        public DateTime? Audpfecmodificacion { get; set; } 

        public List<int> Proccodi { get; set; }
        public List<int> Audiproccodi { get; set; }

        public string Procesos { get; set; }
        public string ProcesoAreas { get; set; }

        public List<int> Areacodi { get; set; }
        public List<int> Audiareacodi { get; set; }

        public int Aniovigencia { get; set; }

        public int Existeaudiproceso { get; set; }
        
    }
}
