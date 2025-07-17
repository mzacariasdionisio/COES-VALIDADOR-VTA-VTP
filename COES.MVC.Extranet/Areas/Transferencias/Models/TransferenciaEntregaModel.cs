using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class TransferenciaEntregaModel
    {

        public List<TransferenciaEntregaDTO> ListaTransferenciaEntrega { get; set; }
        public TransferenciaEntregaDTO Entidad { get; set; }
        public int idTransferenciaEntrega { get; set; }
    }
}