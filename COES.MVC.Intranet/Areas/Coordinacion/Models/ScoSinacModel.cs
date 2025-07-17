using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Coordinacion.Models
{
    public class ScoSinacModel
    {
        public string FechaMaximo { get; set; }
        public List<PrGrupodatDTO> ListaConfiguracion { get; set; }
        public bool IndGrabar { get; set; }
    }

    public class EntidadConfiguracionModel
    {
            public DateTime? Fechadat { get; set; }
            public int Concepcodi { get; set; }
            public int Grupocodi { get; set; }            
            public int Deleted { get; set; }
            public string Formuladat { get; set; }
    }

        public class PaginadoModel
    {
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
    }

}