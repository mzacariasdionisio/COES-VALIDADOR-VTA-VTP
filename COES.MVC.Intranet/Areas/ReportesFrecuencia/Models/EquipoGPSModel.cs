using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Models
{
    public class EquipoGPSModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public List<EquipoGPSDTO> ListaEquipos { get; set; }
        public EquipoGPSDTO Entidad { get; set; }
        public int IdEquipo { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public string sError { get; set; }
        public string sAccion { get; set; }
    }
}