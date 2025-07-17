using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class PrnConfiguracionDTO : EntityBase
    {
        public int Ptomedicodi { get; set; }
        public DateTime Prncfgfecha { get; set; }
        public decimal? Prncfgporcerrormin { get; set; }
        public decimal? Prncfgporcerrormax { get; set; }
        public decimal? Prncfgmagcargamin { get; set; }
        public decimal? Prncfgmagcargamax { get; set; }
        public decimal? Prncfgporcdsvptrn { get; set; }
        public decimal? Prncfgporcmuestra { get; set; }
        public decimal? Prncfgporcdsvcnsc { get; set; }
        public decimal? Prncfgnrocoincidn { get; set; }
        public string Prncfgflagveda { get; set; }
        public string Prncfgflagferiado { get; set; }
        public string Prncfgflagatipico { get; set; }
        public string Prncfgflagdepauto { get; set; }
        public string Prncfgtipopatron { get; set; }
        public int? Prncfgnumdiapatron { get; set; }
        public string Prncfgusucreacion { get; set; }
        public DateTime Prncfgfeccreacion { get; set; }
        public string   Prncfgusumodificacion { get; set; }
        public DateTime Prncfgfecmodificacion { get; set; }
        public string Prncfgflagdefecto { get; set; }

        //08032020
        public decimal? Prncfgpse { get; set; }
        public decimal? Prncfgfactorf { get; set; }

        //Adicionales
        public string Ptomedidesc { get; set; }

        //Auxiliares
        public string StrPrncfgfecha { get; set; }
        public int Prncfgtiporeg { get; set; }
    }
}
