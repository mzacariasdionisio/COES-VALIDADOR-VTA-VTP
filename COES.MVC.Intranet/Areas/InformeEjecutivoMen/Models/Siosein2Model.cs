using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.MVC.Intranet.Areas.Siosein.Models;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.IEOD;

namespace COES.MVC.Intranet.Areas.InformeEjecutivoMen.Models
{
    public class Siosein2Model
    {

        public List<ListaSelect> Meses;
        public int NRegistros;
        public string Fecha { get; set; }
        public DateTime Fecha2 { get; set; }
        public string Resultado { get; set; }
        public string Resultado2 { get; set; }
        public string Mensaje { get; set; }
        public List<string> Resultados { get; set; } 
        public GraficoWeb Grafico { get; set; }
         
        public List<GraficoWeb> Graficos { get; set; }
        public string Anho { get; set; }
        public string Mes { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
         
        public dynamic ResultadoDynamic { get; set; }
        public string Titulo { get; set; }
        public string TituloWeb { get; set; }
        public List<Servicios.Aplicacion.IEOD.SerieDuracionCarga> ListaGrafico { get; set; }
        public int Total { get; set; }
        public List<SioseinModel.ListaGenerica> SeriesPie { get; set; }
        public decimal TotalMesAct { get; set; }
        public decimal TotalMesAnt { get; set; }
        public decimal Variacion { get; set; }
        public string MesAnio { get; set; }
        public List<ListaSelect> ListaSelect { get; internal set; }

        ///Fuente de Datos
        public int Idnumeral { get; set; }
        public int Tiporeporte { get; set; }
        public int Repcodi { get; set; }

        //informes SGI
        public string MesActual { get; set; }
        public string Detalle { get; set; }
        public List<SiVersionDTO> ListaVersion { get; set; }
        public FechasPR5 ObjFecha { get; set; }
        public int Verscodi { get; set; }

        public string Url { get; set; }
        public enum TipoEstado
        {
            Error = 0,
            Ok = 1
        }

        public int Estado { get; set; }

        //Graficos en Reporte
        public List<MeReporteDTO> ListaGraficosReporte { get; set; }
    }
}  