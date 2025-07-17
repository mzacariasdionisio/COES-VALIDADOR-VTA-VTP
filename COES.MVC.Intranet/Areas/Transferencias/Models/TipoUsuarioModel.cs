using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class TipoUsuarioModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public List<TipoUsuarioDTO> ListaTipoUsuario { get; set; }
        public TipoUsuarioDTO Entidad { get; set; }
        public int idTipoUsuario { get; set; }
    }
}