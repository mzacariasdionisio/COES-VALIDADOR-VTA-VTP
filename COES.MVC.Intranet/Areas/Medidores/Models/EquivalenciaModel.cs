using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Medidores.Models
{
    public class EquivalenciaModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<WbMedidoresValidacionDTO> ListaMedicion { get; set; }
        public List<WbMedidoresValidacionDTO> ListaDespacho { get; set; }
        public List<WbMedidoresValidacionDTO> ListaRelaciones { get; set; }
    }
}