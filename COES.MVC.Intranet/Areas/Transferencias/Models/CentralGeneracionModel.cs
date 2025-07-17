using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class CentralGeneracionModel
    {
        public List<CentralGeneracionDTO> ListaCentralGeneracion { get; set; }
        public CentralGeneracionDTO Entidad { get; set; }
        public int IdCentralGene { get; set; }
    }
}