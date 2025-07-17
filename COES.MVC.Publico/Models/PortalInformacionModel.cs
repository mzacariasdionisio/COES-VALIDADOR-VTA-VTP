using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Models
{
    /// <summary>
    /// Model para manejo del portal
    /// </summary>
    public class PortalInformacionModel
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public List<FamiliaDTO> ListaFamilias { get; set; }
        public List<EveCausaeventoDTO> ListaCausaEvento { get; set; }

        public string FechaFinSemanaSiguienteOperativa { get; set; }

    }


    public class GeneracionScadaModel
    {
        public List<MeMedicion48DTO> ListadoPorEmpresa { get; set; }        
        public ChartGeneracion GraficoPorEmpresa { get; set; }
        public ChartStock GraficoTipoCombustible { get; set; }
        public int Indicador { get; set; }
    }

    public class EventoFallaModel
    {
        public List<EventoDTO> ListaEventos { get; set; }

        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int IdFallaCier { get; set; }
        public int TipoEmpresa { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoEquipo { set; get; }
        public string Interrupcion { get; set; }
        public int IdLeyendaCier { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
    }

    #region Modelos para los Graficos de los Eventos Falla

    public class FallaModel
    {
        public List<FallaCier> FallaCier { get; set; }

        public List<string> NombresDuracionxTipoEquipoYCier { get; set; }
        public List<Series> DuracionxTipoEquipoYCier { get; set; }

        public List<string> NombresDuracionxCierYTension { get; set; }
        public List<Series> DuracionxCierYTension { get; set; }

        public List<string> NombresEnergiaInterrumpidaxEquipo { get; set; }
        public Series EnergiaInterrumpidaxEquipo { get; set; }
    }

    // Data para el grafico de pastel
    public class FallaCier
    {
        public string name { get; set; }
        public double y { get; set; }
    }

    // Data para el grafico de columnas
    public class Series
    {
        public string name { get; set; }
        public List<double> data { get; set; }
    }

    #endregion

    public class HidrologiaModel
    {
        public List<string> Puntos { get; set; }
        public List<DataHidrologia> Data { get; set; }
    }
}