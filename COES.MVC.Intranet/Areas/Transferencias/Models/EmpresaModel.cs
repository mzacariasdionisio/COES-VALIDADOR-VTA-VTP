using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class EmpresaModel
    {
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public EmpresaDTO Entidad { get; set; }
        public int IdEmpresa { get; set; }
        public string nombreEmpresa { get; set; }
    }
}