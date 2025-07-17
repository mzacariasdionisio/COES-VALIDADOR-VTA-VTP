using System;
using System.Collections.Generic;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.MVC.Intranet.Areas.Siosein.Models;

namespace COES.MVC.Intranet.Areas.Mape.Models
{
    public class Siosein2Model
    {

        public List<ListaSelect> Meses;
        public int NRegistros;
        public string Fecha { get; set; }
        public DateTime Fecha2 { get; set; }
        public string Resultado { get; set; } 
        public List<string> Resultados { get; set; } 
        public GraficoWeb Grafico { get; set; }
         
        public List<GraficoWeb> Graficos { get; set; }
        public string Anho { get; set; }
        public string Mes { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string NroReporte { get; set; }
         
        public dynamic ResultadoDynamic { get; set; }
        public string Titulo { get; set; }
        public List<Servicios.Aplicacion.IEOD.SerieDuracionCarga> ListaGrafico { get; set; }
        public int Total { get; set; }
        public List<SioseinModel.ListaGenerica> SeriesPie { get; set; }
        public decimal TotalMesAct { get; set; }
        public decimal TotalMesAnt { get; set; }
        public decimal Variacion { get; set; }
        public string MesAnio { get; set; }
        public List<ListaSelect> ListaSelect { get; internal set; }
    }
}  