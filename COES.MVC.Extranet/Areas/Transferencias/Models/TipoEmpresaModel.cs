using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class TipoEmpresaModel
    {
        public List<TipoEmpresaDTO> ListaTipoEmpresas { get; set; }
        public TipoEmpresaDTO Entidad { get; set; }
        public int IdTipoEmpresa { get; set; }
    }
}