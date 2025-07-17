using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class EmpresaModel
    {

        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public EmpresaDTO Entidad { get; set; }
        public int IdEmpresa { get; set; }
    }
}