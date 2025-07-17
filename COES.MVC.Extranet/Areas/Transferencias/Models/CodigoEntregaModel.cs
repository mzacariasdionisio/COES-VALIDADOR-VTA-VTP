using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class CodigoEntregaModel
    {
        public List<CodigoEntregaDTO> ListaCodigoEntrega { get; set; }
        public CodigoEntregaDTO Entidad { get; set; }
        public int IdcodEntrega { get; set; }
    }
}