using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Despacho.Models
{
    public class GrupoDespachoModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<PrGrupoDTO> ListaCentral { get; set; }
        public string Fecha { get; set; }
        public bool IndicadorConfigCMgN { get; set; }
        public bool Editar { get; set; }
    }
}