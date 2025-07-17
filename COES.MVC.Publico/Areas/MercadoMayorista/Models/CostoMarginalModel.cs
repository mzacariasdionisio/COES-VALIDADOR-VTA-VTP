using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.MercadoMayorista.Models
{
    public class CostoMarginalModel
    {
        public string FechaConsulta { get; set; }
        public List<CmCostomarginalDTO> Listado { get; set; }
        public string PathResultado { get; set; }
        public string PathPrincipal { get; set; }
        public string ListaCoordenada { get; set; }
        public List<CmParametroDTO> ListaColores { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }

    public class ListaParticipantesModel
    {
        public List<SiEmpresaMMEDTO> Listado { get; set; }
    }
}