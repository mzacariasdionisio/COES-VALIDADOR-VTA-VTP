using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Medidores.Models
{
    public class ConsolidadoEnvioModel
    {
        public string Central { get; set; }
        public string GrupSSAA { get; set; }
        public decimal Total { get; set; }
        public DateTime fecha { get; set; }
        public short tipoGeneracion { get; set; }
    }

    public class GrupoSSAA
    {
        public string Nombre { get; set; }
        public decimal SubTotal { get; set; }
        public short tipoG { get; set; }
    }


    public class ConsolidadoCentral
    {
        public string Empresa { get; set; }
        public string CentralHead { get; set; }
        public decimal total { get; set; }
        public int Ngrupo { get; set; }
        public DateTime AnoMes { get; set; }
        public IList<GrupoSSAA> listaGrupo { get; set; }
    }
}