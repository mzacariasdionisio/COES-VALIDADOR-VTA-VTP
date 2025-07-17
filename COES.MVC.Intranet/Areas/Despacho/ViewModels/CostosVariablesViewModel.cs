using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;

namespace COES.MVC.Intranet.Areas.Despacho.ViewModels
{
    public class CostosVariablesViewModel
    {
    }

    public class IndexCostosVariablesViewModel
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<PrRepcvDTO> ListadoCostos { get; set; }
        public List<DatoComboBox> ListaTipoPrograma { get; set; }
        public List<DatoComboBox> ListaSemana { get; set; }
        public List<DocFlujoDTO> ListaDocumentosCV { get; set; }
        public string Anho { get; set; }
        public string SemanaActual { get; set; }
        public string FechaDia { get; set; }
        public string FechaDiaEmision { get; set; }
    }
    public class DetalleCostoVariableViewModel
    {
        public int iRepCodi { get; set; }
        public DateTime dFechaRepCV { get; set; }
        public string sNombreRepCV { get; set; }
        public string sTipoRepCV { get; set; }
        public string sDetalleRepCV { get; set; }
        public string sObservaciones { get; set; }
        public List<string> lsEscenarios { get; set; }
        public List<DatoComboBox> lsTipo { get; set; }
        public string Enabled { get; set; }
        public DateTime dFechaEmision { get; set; }
        public List<DatoComboBox> ListaTipoPrograma { get; set; }
        public string NombreGenerarReporte { get; set; }
        public string Anho { get; set; }
        public string SemanaActual { get; set; }
        public string FechaDia { get; set; }
        public string FechaDiaEmision { get; set; }
    }

    public class ParametrosRepCvViewModel
    {
        public int iRepCodi { get; set; }
        public List<PrGrupodatDTO> listaParametros { get; set; }
    }

    public class CostosVariableRepCvViewModel
    {
        public int iRepCodi { get; set; }
        public List<PrCvariablesDTO> listaCostosVariables { get; set; }
    }
}