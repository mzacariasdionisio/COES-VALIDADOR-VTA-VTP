using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class ValorTotalEmpresaModel
    {

        public List<ValorTotalEmpresaDTO> ListaValorTotalEmpresa { get; set; }
        public ValorTotalEmpresaDTO Entidad { get; set; }
        public int IdValorTotalEmpresa { get; set; }
    }
}