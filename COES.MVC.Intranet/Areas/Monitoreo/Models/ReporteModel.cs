using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Monitoreo;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Monitoreo.Models
{
    public class ReporteModel
    {
        public int Id { get; set; }
        public string FechaInicio { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public string Resultado { get; set; }
        public string Resultado2 { get; set; }
        public List<EveIeodcuadroDTO> ListaIeodcuadro { get; set; }
        public string Motivo { get; set; }
        public MmmVersionDTO Generador { get; set; }
        public GraficoMedidorGeneracionMoni GraficoMedidorCurva { get; set; }
        public GraficoMedidorGeneracionMoni GraficoMedidorCurva2 { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public GraficoWeb Grafico { get; set; }
        public GraficoWeb GraficoCurva { get; set; }
        public GraficoWeb GraficoBarra { get; set; }
        public List<GraficoWeb> ListGrafico { get; set; }
        public GraficoWeb GraficoBarraGen { get; set; }
        public List<TipoInformacion3> ListaSemanas { get; set; }
        public List<BarraDTO> ListaBarra { get; set; }
        public int Indicador { get; set; }
    }

    public class TipoInformacion3
    {
        public string IdTipoInfo { get; set; }
        public string NombreTipoInfo { get; set; }
    }

}