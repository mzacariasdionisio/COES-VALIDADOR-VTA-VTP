using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.Operacion.Models
{
    public class PrecioCombustibleModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<PrGrupodatDTO> ListaFormula { get; set; }
    }
}