using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class TransferenciaInformacionBaseDetalleModel
    {
        public List<TransferenciaInformacionBaseDetalleDTO> ListaInformacionBaseDetalle { get; set; }
        public TransferenciaInformacionBaseDetalleDTO Entidad { get; set; }
        public int idInformacionBaseDetalle { get; set; }
    }
}