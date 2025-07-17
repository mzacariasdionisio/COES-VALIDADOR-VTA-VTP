using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Yupana.Models
{
    public class ResultadoModel
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<VariableModel> ListaVariable { get; set; }
        public List<VariableModel> ListaEcuacion { get; set; }
        public List<VariableModel> ListaCosto { get; set; }
        public List<CpTopologiaDTO> ListaTopologia { get; set; }
        public string Resultado { get; set; }
        public List<string> ListaFecha { get; set; }
    }
}