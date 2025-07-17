using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class TipoEmpresaModel
    {

        public List<TipoEmpresaDTO> ListaTipoEmpresas { get; set; }
        public TipoEmpresaDTO Entidad { get; set; }
        public int IdTipoEmpresa { get; set; }
    }
}