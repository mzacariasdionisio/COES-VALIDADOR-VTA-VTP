using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class ValorTransferenciaModel
    {
        public List<ValorTransferenciaDTO> ListaValorTransferencia { get; set; }
        public ValorTransferenciaDTO Entidad { get; set; }
        public int IdValorTransferencia { get; set; }
    }
}