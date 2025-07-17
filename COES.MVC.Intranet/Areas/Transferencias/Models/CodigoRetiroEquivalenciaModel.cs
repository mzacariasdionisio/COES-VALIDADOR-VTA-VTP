using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class CodigoRetiroEquivalenciaModel
    {
        public List<CodigoRetiroRelacionDTO> ListaCodigoRetiroRelacion { get; set; }
        public CodigoRetiroRelacionDTO Entidad { get; set; }
        public List<CodigoRetiroRelacionDetalleDTO> ListaEquivalencia { get; set; }
        public List<CodigoRetiroRelacionDetalleDTO> ListaEquivalenciaVtea { get; set; }
        public List<CodigoRetiroRelacionDetalleDTO> ListaEquivalenciaVtp { get; set; }
        public int IdCodigoRetiroRelacion { get; set; }

        public string Solicodiretifechainicio { get; set; }
        public string Solicodiretifechafin { get; set; }
        public string sError { get; set; }
    }
}