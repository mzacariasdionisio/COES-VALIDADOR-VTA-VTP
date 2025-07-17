using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class InfoDesbalanceModel
    {
        public List<InfoDesbalanceDTO> ListaInfodesbalance { get; set; }
        public InfoDesbalanceDTO Entidad { get; set; }
        public int IdInfoDesbalance { get; set; }
    }
}