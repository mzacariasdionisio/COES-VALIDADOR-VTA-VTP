using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Rpf.Model
{
    public class EnvioAGCModel
    {
        public List<SeguridadServicio.EmpresaDTO> ListaEmpresa { get; set; }
        public string Fecha { get; set; }
    }
}