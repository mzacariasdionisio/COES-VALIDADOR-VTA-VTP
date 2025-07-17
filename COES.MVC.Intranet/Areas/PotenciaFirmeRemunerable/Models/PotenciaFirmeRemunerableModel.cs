using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.PotenciaFirmeRemunerable.Models
{
    public class PotenciaFirmeRemunerableModel
    {
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }
        public int? Version { get; set; }

        public bool TienePermiso { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }
        public bool TienePermisoGuardar { get; set; }

        public bool UsarLayoutModulo { get; set; }
        public bool TieneRegistroPrevio { get; set; }

        public List<GenericoDTO> ListaAnio { get; set; }
        public List<GenericoDTO> ListaMes { get; set; }
        public int AnioActual { get; set; }
        public int MesActual { get; set; }
        public int? NumVersion { get; set; }
        public string Fecha { get; internal set; }
        public bool AccesoEditar { get; internal set; }
        public int NRegistros { get; internal set; }

        public int IdPeriodo { get; set; }
        public int IdReporte { get; set; }
        public int IdReporteDatos { get; set; }
        public int IdRecalculo { get; set; }
        public PfrPeriodoDTO PfrPeriodo { get; set; }
        public PfrRecalculoDTO PfrRecalculo { get; set; }

        public List<PfrPeriodoDTO> ListaPeriodo { get; set; }
        public List<PfrRecalculoDTO> ListaRecalculo { get; set; }
        public List<PfRecalculoDTO> ListaRevisionesPF{ get; set; }
        public List<IndRecalculoDTO> ListaRevisionesIndMesAnterior { get; set; }        
        public List<VtpRecalculoPotenciaDTO> ListaRecalculoTransferencia { get; set; }

        public List<PrGrupodatDTO> ListaParametros { get; set; }

        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public List<PfrEntidadDTO> ListaBarra { get; internal set; }
        public List<BarraDTO> ListaBarraVtp { get; internal set; }
        public List<EqEquipoDTO> ListaUnidad { get; internal set; }
        public List<PrGrupoDTO> ListaBarrasnomb { get; set; }

        public string TipoRecalculo { get; internal set; }
        public bool TieneReportePF { get; set; }
        public string PeriodoActual { get; set; }
        public HandsonModel HandsonModel { get; set; }

        /* ultimo codigos de equipos GAMS */
        public string CodigoDisponibleEquipo { get; set; }
        public string CodigoDisponibleBarra { get; set; }
        public string CodigoDisponibleLinea { get; set; }
        public string CodigoDisponibleTrafo2 { get; set; }
        public string CodigoDisponibleTrafo3 { get; set; }
        public string CodigoDisponibleCompDinamica { get; set; }
        public string CodigoDisponibleCongestion { get; set; }
        public string CodigoDisponibleGamsequipo { get; set; }
        public string CodigoDisponiblePenalidad { get; set; }


        public int FamiliaEquipo { get; set; }

        public List<FileData> ListaDocumentos { get; set; }
        public string DiagramaUnifilar { get; set; }
        public string FuenteGams1 { get; set; }
        public string FuenteGams2 { get; set; }
        public int Conceptocodi { get; set; }
        public string ValorActualCR { get; set; }
        public string ValorActualCA { get; set; }
        public string ValorActualMR { get; set; }

        public string IdEquipo { get; set; }
        public string ListadoPeriodosFechasInicio { get; set; }

        public string HtmlBarras { get; set; }
        public string HtmlBarras1 { get; set; }
        public string HtmlBarras2 { get; set; }
        public string HtmlEBarras { get; set; }
        public string HtmlEBarras1 { get; set; }
        public string HtmlEBarras2 { get; set; }

        public List<PfrEntidadDatDTO> ListaPropiedadVigente { get; set; } = new List<PfrEntidadDatDTO>();
        public List<PfrEntidadDTO> ListaLineas { get; internal set; }
    }
}