using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Medidores.Models
{
    public class MedidorGeneracionModel : FormatoModel
    {
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<SiTipoinformacionDTO> ListaTipoInformacion { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public string Fecha { get; set; }

        public string TituloCargaCentralPotActiva { get; set; }
        public int IdFormatoCargaCentralPotActiva { get; set; }

        public List<FuenteInformacion> ListaFuente1 { get; set; }
        public List<FuenteInformacion> ListaFuente2 { get; set; }

        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
    }

    public class GraficoMedidorGeneracion
    {
        public int TipoUsuario { get; set; }
        public string Nombre { get; set; }
        public List<PuntoGraficoMedidorGeneracion> ListaPunto { get; set; }
        public string TituloGrafico { get; set; }
        public string TituloFuente1 { get; set; }
        public string TituloFuente2 { get; set; }
        public string LeyendaFuente1 { get; set; }
        public string LeyendaFuente2 { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string DescPeriodo { get; set; }
        public string DescCentral { get; set; }
        public string ValorFuente1 { get; set; }
        public string ValorFuente2 { get; set; }
        public int TipoGrafico { get; set; }
    }

    public class FuenteInformacion
    {
        public int Codigo { get; set; }
        public string Valor { get; set; }
        public string Nombre { get; set; }
        public string Titulo { get; set; }
        public string Leyenda { get; set; }
    }

    public class PeriodoDato
    {
        public int Codigo { get; set; }
        public string Valor { get; set; }
    }

    public class PuntoGraficoMedidorGeneracion
    {
        public DateTime Fecha { get; set; }
        public string FechaString { get; set; }
        public decimal? ValorFuente1 { get; set; }
        public decimal? ValorFuente2 { get; set; }
    }

    public class PuntoFuenteInformacion
    {
        public DateTime Fecha { get; set; }
        public DateTime FechaFiltro { get; set; }
        public decimal? ValorFuente { get; set; }
        public int NumeroTiempo { get; set; }
    }
}