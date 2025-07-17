using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class IngresoRetiroSCModel
    {
        public List<IngresoRetiroSCDTO> ListaIngresoRetiroSC { get; set; }
        public IngresoRetiroSCDTO Entidad { get; set; }
        public int IdIngresoRetiroSC { get; set; }
    }
}