using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Medidores.Models
{
    public class ComparativoRPFModel
    {
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public string FechaConsulta { get; set; }
    }
}