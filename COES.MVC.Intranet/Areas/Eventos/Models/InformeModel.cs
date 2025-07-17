using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class InformeModel
    {
        public EveEventoDTO Entidad { get; set; }
        public List<EqEquipoDTO> ListaEquipos { get; set; }
        public List<EveInformeItemDTO> ListaItems { get; set; }
        public List<EveInformeFileDTO> ListaFiles { get; set; }
        public List<EventoInformeReporte> ListaEmpresaCopia { get; set; }
        public int IdInformePreliminar { get; set; }
        public int IdInformeFinal { get; set; }
        public int IdInformeComplementario { get; set; }
        public int IdInformePreliminarInicial { get; set; }
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
        public string PlazoPreliminarInicial { get; set; }
        public string PlazoPreliminar { get; set; }
        public string PlazoFinal { get; set; }
        public string EmpresaReporta { get; set; }
        public int IdEmpresa { get; set; }
        public string IndicadorCopia { get; set; }
        public string IndicadorRevision { get; set; }
        public string IndicadorFinalizar { get; set; }
        public List<ListaSelect> ListaEquipoGenerador { get; set; }
        
        /// <summary>
        /// Datos para la auditoria del informe
        /// </summary>
        public string EstadoInforme { get; set; }
        public string PlazoInforme { get; set; }
        public string UsuarioInforme { get; set; }
        public string FechaInforme { get; set; }
        public string UsuarioRevisionInforme { get; set; }
        public string FechaRevisionInforme { get; set; }
        public string IndicadorCompletoInforme { get; set; }
        
        public string[][] Datos { get; set; }
        public MergeModel[] Merge { get; set; }
        public int[] IndicesTitulo { get; set; }
        public int[] IndicesSubtitulo { get; set; }
        public int[] IndicesAgrupacion { get; set; }
        public int[] IndicesComentario { get; set; }
        public int[] IndicesAdicional { get; set; }
        public int[] IndicesFinal { get; set; }
        public int[] IndiceMantenimiento { get; set; }
        public int? IdPersona { get; set; }
        public List<ValidacionListaCelda> Validaciones { get; set; }
        public List<CeldaValidacionLongitud> Longitudes { get; set; }
        public int Indicador1 { get; set; }
        public List<AlineacionCelda> Centros { get; set; }
        public List<AlineacionCelda> Derechos { get; set; }
        public List<TipoCelda> Tipos { get; set; }
        public List<ValidacionListaCelda> FechaHora { get; set; }
        
    }

    public class ElementoModel
    {
        public List<EqEquipoDTO> ListaGeneradores { get; set; }
        public List<EqEquipoDTO> ListaCeldas { get; set; }
        public List<EqEquipoDTO> ListaInterruptores { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
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

    public class DatoInformeModel
    {
        public List<EventoInformeReporte> Reporte { get; set; }
        public List<EventoInformeReporte> ReporteSCO { get; set; }
        public string Indicador { get; set; }
        public int IdEvento { get; set; }
        public string ExistenciaInformeConsolidado { get; set; }
        public bool IndicadorConsolidado { get; set; }
    }

    public class MergeModel
    {
        public int row { get; set; }
        public int col { get; set; }
        public int rowspan { get; set; }
        public int colspan { get; set; }
    }

    public class AlineacionCelda
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public class ValidacionListaCelda
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public List<string> Elementos { get; set; }
    }

    public class CeldaValidacionLongitud
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Longitud { get; set; }
    }
    
    public class ListaSelect
    {
        public int id { get; set; }
        public string text { get; set; }
    }

    public class TipoCelda
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string Tipo { get; set; }
    }   


    /// <summary>
    /// Secciones del informe
    /// </summary>
    public class SubSeccionesCambio
    {
        /// <summary>
        /// Regulación secundaria de frecuencia
        /// </summary>
        public const int Seccion11 = 11;

        /// <summary>
        /// Reprogramas
        /// </summary>
        public const int Seccion12 = 12;

        /// <summary>
        /// Mantenimientos relevantes
        /// </summary>
        public const int Seccion21 = 21;

        /// <summary>
        /// Observaciones de los mantenimientos relevantes
        /// </summary>
        public const int Seccion22 = 22;

        /// <summary>
        /// Suministro
        /// </summary>
        public const int Seccion31 = 31;

        /// <summary>
        /// Operación de centrales
        /// </summary>
        public const int Seccion41 = 41;

        /// <summary>
        /// Lineas desconectadas
        /// </summary>
        public const int Seccion42 = 42;

        /// <summary>
        /// Mantenimientos fuera del pdo
        /// </summary>
        public const int Seccion43 = 43;

        /// <summary>
        /// Eventos importantes
        /// </summary>
        public const int Seccion44 = 44;

        /// <summary>
        /// Informes 
        /// </summary>
        public const int Seccion45 = 45;
    }
}