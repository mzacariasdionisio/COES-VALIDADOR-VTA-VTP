using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.InformesOsinergmin.Models
{
    public class IndicadoresRPFModel
    {
        public List<PsuRpfhidDTO> Listado { get; set; }
        public PsuRpfhidDTO Entidad { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string IndNuevo { get; set; }

        public string Rpfhidfecha { get; set; }
        public decimal? Rpfenetotal { get; set; }
        public decimal? Rpfpotmedia { get; set; }
        public decimal? Eneindhidra { get; set; }
        public decimal? Potindhidra { get; set; }
    }
}