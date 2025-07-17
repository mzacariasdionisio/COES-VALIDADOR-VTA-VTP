using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class DemandaMercadoLibreModel
    {
        public List<DemandaMercadoLibreDTO> ListaInformacionAgentes { get; set; }
        public List<DemandaMercadoLibreDTO> ListaInformacionOsinergmin { get; set; }

        public DateTime[] PeriodosEvaluados { get; set; }
    }
}