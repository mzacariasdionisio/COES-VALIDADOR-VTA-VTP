using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.MarcoNormativo.Models
{
    /// <summary>
    /// Model para la seccion de decisiones ejecutivas y notas tecnicas
    /// </summary>
    public class DecisionEjecutivaModel
    {
        public List<WbDecisionejecutivaDTO> ListaDecisiones { get; set; }
        public WbDecisionejecutivaDTO Entidad { get; set; }
        public string Indicador { get; set; }
        public List<WbDecisionejecutivaDTO> ListaNotas { get; set; }
    }
}