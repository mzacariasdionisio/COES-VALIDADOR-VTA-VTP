using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Mediciones;

namespace COES.MVC.Intranet.Areas.Medidores.Models
{
    public class ExcesoPotencReactivaModel
    {
        public List<ExcesoPotenciaReacMes> ListaExcPotencReac;
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public string Mes { get; set; }
        public string FechaConsulta { get; set; }
        public List<Integrante> ListaIntegrante { get; set; }

        public int IdTipoGeneracion { get; set; }
        public int IdEmpresa { get; set; }
        public int IdCentral { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
    }

    public class ReporteExcesoPotenciaReacModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<MaximaDemandaDTO> ListaResumenDemanda { get; set; }
        public List<ConsolidadoEnvioDTO> ListaConsolidadoDemanda { get; set; }
        public string Mes { get; set; }
        public string FechaConsulta { get; set; }
        public List<Integrante> ListaIntegrante { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public string Titulo { get; set; }
        public string TituloDet { get; set; }
        public string NombreHoja { get; set; }
        public string NombreHojaDet { get; set; }
        public int IdParametro { get; set; }
        public List<MeMedicion96DTO> DetActiva { get; set; }
        public List<MeMedicion96DTO> DetInductiva { get; set; }
        public List<MeMedicion96DTO> DetCapacitiva { get; set; }
        public string ResultadoHtml { get; set; }
    }

    public class ExcesoPotenciaReacMes
    {
        public string Empresanomb { get; set; }
        public string Centralnomb { get; set; }
        public string Gruponomb { get; set; }
        public string Tipogeneracion { get; set; }
        public decimal PotInductiva { get; set; }
        public decimal PotCapacitiva { get; set; }
        public string ModoOperacion { get; set; }
        public List<decimal> valores { get; set; }
        public List<string> horamin { get; set; }
    }
}