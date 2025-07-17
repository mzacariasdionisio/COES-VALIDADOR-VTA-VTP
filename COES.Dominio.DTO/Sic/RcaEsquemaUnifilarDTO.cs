using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RCA_ESQUEMA_UNIFILAR
    /// </summary>
    public class RcaEsquemaUnifilarDTO : EntityBase
    {
        public int Rcesqucodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int? Equicodi { get; set; } 
        public string Rcesqudocumento { get; set; } 
        public DateTime? Rcesqufecharecepcion { get; set; } 
        public string Rcesquestado { get; set; } 
        public string Rcesqunombarchivo { get; set; } 
        public string Rcesquestregistro { get; set; } 
        public string Rcesquusucreacion { get; set; } 
        public DateTime Rcesqufeccreacion { get; set; } 
        public string Rcesquusumodificacion { get; set; } 
        public DateTime? Rcesqufecmodificacion { get; set; }

        public string Emprrazsocial { get; set; }
        public string Equinomb { get; set; }
        public string Areaabrev { get; set; }

        public string Areanomb { get; set; }

        public string Osinergcodi { get; set; }

        public int Tipoemprcodi { get; set; }

        public int Rcesquorigen { get; set; }

        public string Rcesqunombarchivodatos { get; set; }
    }
}
