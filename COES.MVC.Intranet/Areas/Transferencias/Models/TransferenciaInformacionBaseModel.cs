using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class TransferenciaInformacionBaseModel
    {
        public List<TransferenciaInformacionBaseDTO> ListaInformacionBase { get; set; }
        public TransferenciaInformacionBaseDTO Entidad { get; set; }
        public int idInformacionBase { get; set; }
    }
}