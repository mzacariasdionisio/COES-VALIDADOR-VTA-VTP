using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class InfoDesviacionModel
    {
        public List<InfoDesviacionDTO> ListaInfoDesviacion { get; set; }
        public InfoDesviacionDTO Entidad { get; set; }
    }
}