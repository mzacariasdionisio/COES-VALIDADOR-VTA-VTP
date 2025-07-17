using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.DemandaMaxima.Models
{
    public class AmpliacionPlazoModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }

        public List<MeAmpliacionfechaDTO> ListaEmpresasPlazo { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }
}