using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Mediciones;
namespace COES.MVC.Intranet.Areas.Medidores.Models
{
    public class MaximaDemanda
    {
        public List<MaximaDemandaDia> ListaDemandaDia;
        public List<MaximaDemandaDia> ListaDemandaDiaTotalResumen;
        public List<DemandadiaDTO> ListaDemandaDia_HFP_HP;
        public int ndiasXMes;
        public int IndexHFP { get; set; }
        public int IndexHP { get; set; }
    }

    //inicio agregado
    public class ReporteMaximaDemandaModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<MaximaDemandaDTO> ListaResumenDemanda { get; set; }
        public List<ConsolidadoEnvioDTO> ListaConsolidadoDemanda { get; set; }
        public string Mes { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string FechaConsulta { get; set; }
        public List<Integrante> ListaIntegrante { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public string Titulo { get; set; }
        public string NombreHoja { get; set; }
        public int IdParametro { get; set; }
        public List<Normativa> ListaNormativa { get; set; }
        public List<string> ListaDescripcionNormativa { get; set; }
        public string MensajeFechaConsulta { get; set; }
        public string MensajePorcentajeConsulta { get; set; }
        public bool EsPortal { get; set; }

        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
    }

    public class ReporteDemandaPeriodoModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<MaximaDemandaDTO> ListaResumenDemanda { get; set; }
        public List<ConsolidadoEnvioDTO> ListaConsolidadoDemanda { get; set; }
        public string Mes { get; set; }
        public string FechaConsulta { get; set; }
        public List<Integrante> ListaIntegrante { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public List<BloqueHorario> ListaBloqueHorario { get; set; }
        public string Titulo { get; set; }
        public string NombreHoja { get; set; }
        public string JsonBloqueHorario { get; set; }
        public List<Normativa> ListaNormativa { get; set; }
        public List<string> ListaDescripcionNormativa { get; set; }
        public string MensajeFechaConsulta { get; set; }
        public string MensajePorcentajeConsulta { get; set; }
        public bool EsPortal { get; set; }
    }

    public class Integrante
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
    }

    public class BloqueHorario
    {
        public int Tipo { get; set; }
        public string Bloque { get; set; }
        public string Descripcion { get; set; }
    }

    public class DiagramaCargaMaximaDemandaModel
    {
        public MaximaDemandaDTO MaximaDemanda { get; set; }
        public List<MaximaDemandaDTO> ListaDemandaCuartoHora { get; set; }
        public string Mes { get; set; }
        public string FechaConsulta { get; set; }
        public string Titulo { get; set; }
        public string Leyenda { get; set; }
        public string DescripcionSerie { get; set; }
        public string Json { get; set; }
        public List<Normativa> ListaNormativa { get; set; }
        public List<string> ListaDescripcionNormativa { get; set; }
    }

    public class ReporteRecursoEnergeticoModel
    {
        public List<MaximaDemandaDTO> ListaResumenDemanda { get; set; }
        public List<ConsolidadoEnvioDTO> ListaConsolidadoRecursoEnergetico { get; set; }
        public string Mes { get; set; }
        public string FechaConsulta { get; set; }
        public string Titulo { get; set; }
        public string Leyenda { get; set; }
        public string DescripcionSerie { get; set; }
        public string JsonGraficoPipe { get; set; }
        public int BloqueHorario { get; set; }
        public List<Normativa> ListaNormativa { get; set; }
        public List<string> ListaDescripcionNormativa { get; set; }
        public MaximaDemandaDTO DemandaHP { get; set; }
    }

    public class Normativa
    {
        public string DescripcionFull { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
    //fin agregado

    public class BusquedaMaximaDemandaModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public int IdTipoGeneracion { get; set; }
        public int IdEmpresa { get; set; }
        public int IdCentral { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int ParametroDefecto { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public List<Normativa> ListaNormativa { get; set; }
    }

    public class MaximaDemandaDia
    {
        public string Empresanomb { get; set; }
        public string Centralnomb { get; set; }
        public string Gruponomb { get; set; }
        public string Tipogeneracion { get; set; }
        public List<decimal> valores { get; set; }
        public List<string> horamin { get; set; }
    }

    public class ConsolidadoModel
    {
        public string FechaConsulta { get; set; }
        public string FechaHasta { get; set; }
        public string FechaDesde { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public List<Normativa> ListaNormativa { get; set; }
    }



    public class RankingModel
    {
        public string FechaConsulta { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public List<DemandadiaDTO> ListaDemandaDia { get; set; }
        public List<DemandadiaDTO> ListaDemandaDiaGeneracion { get; set; }
        public int IndexHFP { get; set; }
        public int IndexHP { get; set; }
        public decimal MaximaDemanda { get; set; }
        public string FechaMD { get; set; }
        public string HoraMD { get; set; }
        public decimal ProduccionEnergia { get; set; }
        public decimal FactorCarga { get; set; }
        public List<Normativa> ListaNormativa { get; set; }
    }

    /// <summary>
    /// Maneja los datos del reporte de medidores
    /// </summary>
    public class ValidacionMedidoresModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public List<SiFuenteenergiaDTO> ListaFuenteEnergia { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string FechaConsulta { get; set; }
        public List<ReporteValidacionMedidor> ListaReporte { get; set; }
        public decimal TotalDespacho { get; set; }
        public decimal TotalMedidor { get; set; }
        public decimal TotalMDDespacho { get; set; }
        public decimal TotalMDMedidor { get; set; }
        public string FechaMDDespacho { get; set; }
        public string FechaMDMedidor { get; set; }
        public string IndConfiguracion { get; set; }
        public string MensajeValidacion { get; set; }
        public List<WbMedidoresValidacionDTO> ListadoGeneral { get; set; }
        public List<string> Listado { get; set; }
    }


    public class DuracionCargaModel
    {
        public string FechaConsulta { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public List<DemandadiaDTO> ListaDemandaDia { get; set; }
        public List<SerieDuracionCarga> ListaGrafico { get; set; }
        public decimal Gwh { get; set; }
        public decimal Maximo { get; set; }
        public decimal Minimo { get; set; }
        public decimal Fc { get; set; }
        public string Titulo { get; set; }
        public List<Normativa> ListaNormativa { get; set; }
    }

    /// <summary>
    /// Elementos de la serie
    /// </summary>
    public class SerieMedicionEvolucion
    {
        public string SerieName { get; set; }
        public List<decimal> ListaValores { get; set; }
        public string SerieColor { get; set; }

    }

    /// <summary>
    /// Objeto para almacenar las series de duración de carga
    /// </summary>
    public class SerieDuracionCarga
    {
        public string SerieName { get; set; }
        public List<decimal> ListaValores { get; set; }
        public string SerieColor { get; set; }
    }

    /// <summary>
    /// Obtjeto para almacena las series de duracion de carga
    /// </summary>
    public class DuracionCargaSerie
    {
        public decimal Agua { get; set; }
        public decimal Gas { get; set; }
        public decimal Diesel { get; set; }
        public decimal Residual { get; set; }
        public decimal Carbon { get; set; }
        public decimal Bagazo { get; set; }
        public decimal Biogas { get; set; }
        public decimal Solar { get; set; }
        public decimal Eolica { get; set; }
        public decimal R500 { get; set; }
        public decimal R6 { get; set; }

        public const int IdAgua = 1;
        public const int IdGas = 2;
        public const int IdDiesel = 3;
        public const int IdResidual = 4;
        public const int IdCarbon = 5;
        public const int IdBagazo = 6;
        public const int IdBiogas = 7;
        public const int IdSolar = 8;
        public const int IdEolica = 9;
        public const int IdR500 = 10;
        public const int IdR6 = 11;

        public const string ColorAgua = "#4682B4";
        public const string ColorGas = "#FF0000";
        public const string ColorDiesel = "#006400";
        public const string ColorResidual = "#8B4513";
        public const string ColorCarbon = "#696969";
        public const string ColorBagazo = "#FFA500";
        public const string ColorBiogas = "#98FB98";
        public const string ColorSolar = "#FFFF00";
        public const string ColorEolica = "#87CEEB";
        public const string ColorR500 = "#FFFFFF";
        public const string ColorR6 = "#FFFFFF";

        public const string NombreAgua = "AGUA";
        public const string NombreGas = "GAS";
        public const string NombreDiesel = "DIESEL";
        public const string NombreResidual = "RESIDUAL";
        public const string NombreCarbon = "CARBON";
        public const string NombreBagazo = "BAGAZO";
        public const string NombreBiogas = "BIOGAS";
        public const string NombreSolar = "SOLAR";
        public const string NombreEolica = "EOLICA";
        public const string NombreR500 = "R500";
        public const string NombreR6 = "R6";


        public decimal Total { get; set; }
    }

    public class ListaMesesRE
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }

    /// <summary>
    /// Almacena la lista que contendra la lista
    /// </summary>
    public class EntidadSerieMedicionEvolucion
    {
        public List<SerieMedicionEvolucion> ListaSerie { get; set; }
        public int IndiceMaximaDemanda { get; set; }
        public int IndiceMinimaDemanda { get; set; }
        public decimal ValorMaximaDemanda { get; set; }
        public decimal ValorMinimaDemanda { get; set; }
        public string FechaMaximaDemanda { get; set; }
        public string FechaMinimaDemanda { get; set; }
        public string Titulo { get; set; }
    }

    public class ComparativoItemModel
    {
        public string Hora { get; set; }
        public decimal ValorMedidor { get; set; }
        public decimal ValorDespacho { get; set; }
        public decimal Desviacion { get; set; }
    }

    public class ComparativoMasivoModel
    {
        public string Central { get; set; }
        public List<ComparativoItemModel> ListaGrafico { get; set; }
    }

}