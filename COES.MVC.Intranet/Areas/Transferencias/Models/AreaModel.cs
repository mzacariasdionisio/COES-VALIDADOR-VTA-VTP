using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class AreaModel
    {
        public List<AreaDTO> ListaAreas { get; set; }
        public AreaDTO Entidad { get; set; }
        public int IdArea { get; set; }
    }
}