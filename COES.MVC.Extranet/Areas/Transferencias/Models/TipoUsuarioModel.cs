using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class TipoUsuarioModel
    {
        public List<TipoUsuarioDTO> ListaTipoUsuario { get; set; }
        public TipoUsuarioDTO Entidad { get; set; }
        public int idTipoUsuario { get; set; }
    }
}