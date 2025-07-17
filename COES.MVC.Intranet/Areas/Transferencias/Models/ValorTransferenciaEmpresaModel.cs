using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class ValorTransferenciaEmpresaModel
    {

        public List<ValorTransferenciaEmpresaDTO> ListaValorTransferenciaEmpresa { get; set; }
        public ValorTransferenciaEmpresaDTO Entidad { get; set; }
        public int IdValorTransferenciaEmpresa { get; set; }
    }
}