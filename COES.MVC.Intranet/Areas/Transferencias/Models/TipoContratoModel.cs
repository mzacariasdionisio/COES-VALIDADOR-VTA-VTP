using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class TipoContratoModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public List<TipoContratoDTO> ListaTipoContrato { get; set; }
        public TipoContratoDTO Entidad { get; set; }
        public int idTipoContrato { get; set; }

    }
}