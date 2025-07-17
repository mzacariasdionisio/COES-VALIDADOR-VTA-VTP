using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class CodigoInfoBaseModel
    {
        public List<CodigoInfoBaseDTO> ListaCodigoInfoBase { get; set; }
        public CodigoInfoBaseDTO Entidad { get; set; }
        public int IdcodInfoBase { get; set; }
    }
}