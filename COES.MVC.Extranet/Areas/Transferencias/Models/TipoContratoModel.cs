using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class TipoContratoModel
    {
        public List<TipoContratoDTO> ListaTipoContrato { get; set; }
        public TipoContratoDTO Entidad { get; set; }
        public int idTipoContrato { get; set; }
    }
}