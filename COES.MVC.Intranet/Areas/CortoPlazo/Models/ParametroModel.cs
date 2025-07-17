using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    public class ParametroModel
    {
        public List<CmParametroDTO> ListaParametro { get; set; }
        public CmParametroDTO Entidad { get; set; }
        public string Descripcion { get; set; }
        public decimal ValorInferior { get; set; }
        public decimal ValorSuperior { get; set; }
        public string Color { get; set; }
        public int Codigo { get; set; }
    }
}