using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace COES.MVC.Intranet.Areas.StockCombustibles.Models
{
    public class StockCombustiblesModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<MeEnvioDTO> ListaEnvio { get; set; }
        public List<MeEstadoenvioDTO> ListaEstadoEnvio { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int HoraPlazo { get; set; }
        public string Fecha { get; set; }
        public string FechaPlazo { get; set; }
        public HandsonModel Handson { get; set; }
        public string Resultado { get; set; }
        public string NombreFortmato { get; set; }
        public GraficoWeb Grafico { get; set; }
        public List<TipoInformacion2> ListaTipoAgente { get; set; }
        public List<TipoInformacion> ListaCentralIntegrante { get; set; }
        public List<EqEquipoDTO> ListaCentral { get; set; }
        public List<TipoInformacion> ListaEstFisCombustible { get; set; }
        public List<MeTipopuntomedicionDTO> ListaRecursoEnergetico { get; set; }
        public List<TipoInformacion> ListaParametros { get; set; }
        public List<MeAmpliacionfechaDTO> ListaAmpliacion { get; set; }
        public List<MeTipopuntomedicionDTO> ListaTipoPtoMedicion { get; set; }
        public List<MeValidacionDTO> ListaValidacion { get; set; }
        public List<MeMedicionxintervaloDTO> ListaAcumulado { get; set; }
        public List<SiFuenteenergiaDTO> ListaCombustible { get; set; }
        public string Equinomb { get; set; }
        public string Emprnomb { get; set; }
        public string TituloReporte { get; set; }


        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }

        public int codReporte { get; set; }
        public int ragFechas { get; set; }

        public List<EqCategoriaDetDTO> ListaYacimientos { get; set; }

        public List<MeFormatoDTO> ListaCombo { get; set; }
    }


    public class StockCombustibleFormatoModel : FormatoModel
    {
        public Boolean EnabledStockInicio { get; set; }
        public List<decimal> ListaStockInicio { get; set; }
    }

    public class FiltroStockCombustibleModel
    {
        public List<EqEquipoDTO> ListaCentral { get; set; }

    }
}