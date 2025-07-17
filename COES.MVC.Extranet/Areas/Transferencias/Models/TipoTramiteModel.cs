using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class TipoTramiteModel
    {
        public List<TipoTramiteDTO> ListaTipoTramites { get; set; }
        public TipoTramiteDTO Entidad { get; set; }
        public int IdTipoTramite { get; set; }
    }
}