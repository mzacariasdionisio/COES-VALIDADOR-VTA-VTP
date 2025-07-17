using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class ConceptoModel
    {
        public List<EmpresaDTO> ListaEmpresa { get; set; }
        public List<TipoEmpresaDTO> ListaTipoEmpresa { get; set; }

        public TrnInfoadicionalDTO EntidadConcepto { get; set; }
    }
}