using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPO_CONFIGURA
    /// </summary>
    public class EpoConfiguraDTO : EntityBase
    {
        public int Confplazorevcoesporv { get; set; } 
        public int Confplazorevcoesvenc { get; set; } 
        public int Confplazolevobsporv { get; set; } 
        public int Confplazolevobsvenc { get; set; } 
        public int Confplazoalcancesvenc { get; set; } 
        public int Confplazoverificacionvenc { get; set; } 
        public int Confplazorevterceroinvporv { get; set; } 
        public int Confplazorevterceroinvvenc { get; set; } 
        public int Confplazoenvestterceroinvporv { get; set; } 
        public int Confplazoenvestterceroinvvenc { get; set; } 
        public int? Confindigestionsnpepo { get; set; } 
        public int? Confindiporcatencionepo { get; set; } 
        public int? Confindigestionsnpeo { get; set; } 
        public int? Confindiporcatencioneo { get; set; } 
        public int Confcodi { get; set; }
        public string  Confdescripcion { get; set; }

        public int Confplazoverificacionvencabs { get; set; }
        

    }
}
