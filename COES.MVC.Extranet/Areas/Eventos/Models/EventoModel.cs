using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Eventos.Models
{
    public class EventoModel
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<EveTipoeventoDTO> ListaTipoEvento { get; set; }
        public List<EveEventoDTO> ListaEvento { get; set; }
        public List<SeguridadServicio.EmpresaDTO> ListaEmpresa { get; set; }
        public int IndicadorEmpresa { get; set; }
        public int IdEmpresa { get; set; }
    }

    public class DatoInformeModel
    {
        public List<EventoInformeReporte> ReporteSCO { get; set; }
        public List<EventoInformeReporte> Reporte { get; set; }
        public string Indicador { get; set; }
    }

    public class PaginadoModel
    {
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
    }

    public class InformeModel
    {
        public EveEventoDTO Entidad { get; set; }
        public List<EqEquipoDTO> ListaEquipos { get; set; }
        public List<EveInformeItemDTO> ListaItems { get; set; }
        public List<EveInformeFileDTO> ListaFiles { get; set; }
        public int IdInformePreliminarInicial { get; set; }
        public int IdInformePreliminar { get; set; }
        public int IdInformeFinal { get; set; }
        public int IdInformeComplementario { get; set; }
        public int IdInformeFile { get; set; }
        public int IdInforme { get; set; }
        public string Indicador { get; set; }
        public int IdItemDescripcion { get; set; }
        public int IdItemAnalisis { get; set; }
        public string DesItemDescripcion { get; set; }
        public string DesItemAnalisis { get; set; }
        public string TipoInforme { get; set; }
        public string AsuntoEvento { get; set; }
        public string FechaEvento { get; set; }
        public string HoraEvento { get; set; }
        public string PlazoMaximo { get; set; }
        public string IndicadorPlazo { get; set; }
        public string IndicadorEdicion { get; set; }
        public string PlazoPreliminar { get; set; }
        public string PlazoPreliminarInicial { get; set; }
        public string PlazoFinal { get; set; }
        public string EmpresaReporta { get; set; }
        public int IdEmpresa { get; set; }
        public string LogoEmpresa { get; set; }
        public string IndicaShowPlazo { get; set; }
    }

    public class ElementoModel
    {
        public List<EqEquipoDTO> ListaGeneradores { get; set; }
        public List<EqEquipoDTO> ListaCeldas { get; set; }
        public List<EqEquipoDTO> ListaInterruptores { get; set; }
        public int ItemInforme { get; set; }
        public int SubItemInforme { get; set; }
        public string TipoInforme { get; set; }
        public int IdEmpresa { get; set; }
        public int IdEvento { get; set; }
        public int IdInforme { get; set; }
        public int IdItemInforme { get; set; }
        public int Indicador { get; set; }
        public EveInformeItemDTO Entidad { get; set; }


        /// <summary>
        /// Campos para captar los datos de los elementos
        /// </summary>
        public decimal? Potactiva { get; set; }
        public decimal? Potreactiva { get; set; }
        public int? Equicodi { get; set; }
        public decimal? Niveltension { get; set; }
        public string Desobservacion { get; set; }
        public string Itemhora { get; set; }
        public string Senializacion { get; set; }
        public int? Interrcodi { get; set; }
        public string Ac { get; set; }
        public int? Ra { get; set; }
        public int? Sa { get; set; }
        public int? Ta { get; set; }
        public int? Rd { get; set; }
        public int? Sd { get; set; }
        public int? Td { get; set; }
        public string Descomentario { get; set; }
        public string Sumininistro { get; set; }
        public decimal? Potenciamw { get; set; }
        public string Proteccion { get; set; }
        public string Intinicio { get; set; }
        public string Intfin { get; set; }
        public string Subestacionde { get; set; }
        public string Subestacionhasta { get; set; }

    }
    #region Mejoras CTAF
    public class EventoScoModel
    {
        public int IdEvento { get; set; }
        public int? TipoInforme { get; set; }
        public int? Emprcodi { get; set; }
        public string DI { get; set; }
        public string DF { get; set; }
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public string FechaEvento { get; set; }
        public bool? EnPlazoIPI { get; set; }
        public bool? EnPlazoIF { get; set; }
        public string PlazoEnvio { get; set; }
        public string Descripcion { get; set; }
        public List<EveEventoDTO> LstEvento { get; set; }
        public List<EveInformesScoDTO> LstInformes { get; set; }
    }
    #endregion
}