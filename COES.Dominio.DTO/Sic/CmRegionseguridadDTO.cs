using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_REGIONSEGURIDAD
    /// </summary>
    public class CmRegionseguridadDTO : EntityBase
    {
        public int Regsegcodi { get; set; } 
        public string Regsegnombre { get; set; }
        public decimal? Regsegvalorm { get; set; } 
        public string Regsegdirec { get; set; } 
        public string Regsegestado { get; set; } 
        public string Regsegusucreacion { get; set; } 
        public DateTime? Regsegfeccreacion { get; set; } 
        public string Regsegusumodificacion { get; set; } 
        public DateTime? Regsegfecmodificacion { get; set; } 
        //Movisoft 26-04-2022
        public int RegsegcodiExcel { get; set; }
    }
}
