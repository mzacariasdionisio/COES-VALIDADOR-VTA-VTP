using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    public class UmbralModel
    {
        public CmUmbralComparacionDTO Entidad { get; set; }
        public int Codigo { get; set; }
        public decimal UmbralHOP { get; set; }
        public decimal UmbralGeneracionEMS { get; set; }
        public decimal UmbralDemandaEMS { get; set; }
        public decimal UmbralCI { get; set; }
        public decimal UmbralNumIter { get; set; }
        public decimal UmbralVarAng { get; set; }
        public int NumTab { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
    }
}