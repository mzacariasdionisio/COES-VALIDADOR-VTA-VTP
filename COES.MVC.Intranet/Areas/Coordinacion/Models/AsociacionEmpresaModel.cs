using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Coordinacion.Models
{
    public class AsociacionEmpresanModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<TrEmpresaSp7DTO> ListaEmpresasSp7 { get; set; } 
        public List<ScEmpresaDTO> Listado { get; set; }

        public string Nombre { get; set; }
       
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }


    }
}