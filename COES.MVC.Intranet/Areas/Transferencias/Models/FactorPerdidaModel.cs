using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class FactorPerdidaModel
    {
        public List<FactorPerdidaDTO> ListaFactoresPerdida { get; set; }
        public FactorPerdidaDTO Entidad { get; set; }
        public int IdFactorPerdida { get; set; }
    }
}