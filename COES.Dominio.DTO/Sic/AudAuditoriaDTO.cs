using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_AUDITORIA
    /// </summary>
    public class AudAuditoriaDTO : EntityBase
    {
        public int Audicodi { get; set; }
        public int Audicodigenerado { get; set; }
        public int Tabcdestadocodi { get; set; } 
        public string Audicodigo { get; set; } 
        public string Audinombre { get; set; } 
        public string Audiobjetivo { get; set; }
        public string Audialcance { get; set; }
        public DateTime? Audifechainicio { get; set; }
        public DateTime? Audifechafin { get; set; }
        public string Audiactivo { get; set; } 
        public string Audihistorico { get; set; } 
        public string Audiusucreacion { get; set; } 
        public DateTime? Audifeccreacion { get; set; } 
        public string Audiusumodificacion { get; set; } 
        public DateTime? Audifecmodificacion { get; set; }

        public List<int> Procareacodi { get; set; }
        public List<int> Proccodi { get; set; }
        public List<int> Elemproccodi { get; set; }
        public List<int> Audppcodis { get; set; }
        public string Audiesplanificado { get; set; }
        public int Audpcodi { get; set; }
        public int Plancodi { get; set; }
        public int AnioVigencia { get; set; }
        public int Areacodi { get; set; }
        public int AnioInicio { get; set; }

        public string Procdescripcion { get; set; }
        public string Tabcddescripcion { get; set; }
        public int Tabcdcodi { get; set; }
        public string Progahdescripcion { get; set; }
        public string Elemdescripcion { get; set; }
        public string Elemcodigo { get; set; }
        public string Areaabrev { get; set; }
        public string Estadodescripcion { get; set; }
        public string Progahaccionmejora { get; set; }
        

    }
}
