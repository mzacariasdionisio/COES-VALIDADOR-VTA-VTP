using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class BarraModel
    {
        public List<BarraDTO> ListaBarras { get; set; }       
        public BarraDTO Entidad { get; set; }
        public int IdBarra { get; set; }
    }
}