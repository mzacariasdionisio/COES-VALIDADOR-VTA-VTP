using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.GMM.Models
{
    public class IncumplimientoModel
    {

        public List<GmmIncumplimientoDTO> listadoIncumplimientos { get; set; }
        public List<GmmDetIncumplimientoDTO> listadoArchivos { get; set; }
        public List<GmmIncumplimientoDTO> listadoMontos { get; set; }
        public List<GmmEmpresaDTO> ListEmpresaCliente { get; set; }
        public List<GmmEmpresaDTO> ListEmpresaAgente { get; set; }


        public IncumplimientoModel()
        {
            listadoIncumplimientos = new List<GmmIncumplimientoDTO>();
            listadoArchivos = new List<GmmDetIncumplimientoDTO>();
            ListEmpresaCliente = new List<GmmEmpresaDTO>();
            ListEmpresaAgente = new List<GmmEmpresaDTO>();
        }
    }
}