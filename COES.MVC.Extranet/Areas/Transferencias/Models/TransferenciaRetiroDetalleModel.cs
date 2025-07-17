using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class TransferenciaRetiroDetalleModel
    {
        public List<TransferenciaRetiroDetalleDTO> ListaTransferenciaRetiroDetalle { get; set; }
        public TransferenciaRetiroDetalleDTO Entidad { get; set; }
        public int idTransferenciaRetiroDetalle { get; set; }
    }
}