using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class TransferenciaEntregaDetalleModel
    {

        public List<TransferenciaEntregaDetalleDTO> ListaTransferenciaEntregaDetalle { get; set; }
        public TransferenciaEntregaDetalleDTO Entidad { get; set; }
        public int idTransferenciaEntregaDeta { get; set; }
    }
}