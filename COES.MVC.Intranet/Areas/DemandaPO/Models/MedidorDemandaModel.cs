using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.DPODemanda.Helper;

namespace COES.MVC.Intranet.Areas.DemandaPO.Models
{
    public class MedidorDemandaModel
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public string Fecha { get; set; }
        public List<DpoBarraSplDTO> ListaBarraSPL { get; set; }
        public List<DpoVersionRelacionDTO> ListaVersion { get; set; }
        public List<MeJustificacionDTO> Bitacora { get; set; }
        public List<Tuple<int, string, bool>> ListTipoEmpresa { get; set; }
        public List<Tuple<int, string, bool>> ListTipoDemanda { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }

    }
}