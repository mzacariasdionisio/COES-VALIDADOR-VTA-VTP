using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using System;
using System.Collections.Generic;
using System.Linq;

namespace COES.MVC.Intranet.Areas.Equipamiento.Models
{
    public class FichaTecnicaModel
    {
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<PrCategoriaDTO> ListaCategoria { get; set; }
        public List<PrConceptoDTO> ListaConcepto { get; set; }
        public List<EqPropiedadDTO> ListaPropiedad { get; set; }
        public List<FtFictecPropDTO> ListaFichaPropiedad { get; set; }
        public List<PrAgrupacionDTO> ListaAgrupacion { get; set; }
        public PrAgrupacionDTO Agrupacion { get; set; }
        public List<PrAgrupacionConceptoDTO> ListaAgrupacionConcepto { get; set; }

        public int NumOrigenpadre { get; set; }
        public List<FtFictecXTipoEquipoDTO> ListaFichaTecnica { get; set; }
        public List<FtFictecXTipoEquipoDTO> ListaFichaTecnicaPadre { get; set; }
        public FtFictecXTipoEquipoDTO FichaTecnica { get; set; }
        public List<FtFictecItemDTO> ListaItems { get; set; }
        public List<TreeItemFichaTecnica> ListaItemsJson { get; set; }
        public string TreeJson { get; set; }
        public int Origen { get; set; }

        public FtFichaTecnicaDTO FichaMaestra { get; set; }
        public List<FtFichaTecnicaDTO> ListaFichaMaestra { get; set; }
        public List<FtFictecXTipoEquipoDTO> ListaFichaTecnicaNoSelec { get; set; }
        public List<FtFictecXTipoEquipoDTO> ListaFichaTecnicaSelec { get; set; }
        public List<EstadoParametro> ListaEstado { get; set; }

        public List<FtFictecNotaDTO> ListaNota { get; set; }
        public string NotaJson { get; set; }

        public List<FtExtCorreoareaDTO> ListaAreas { get; set; }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////              
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public int TipoElemento { get; set; }
        public int TipoElementoId { get; set; }
        public int Codigo { get; set; }
        public ElementoFichaTecnica Elemento { get; set; }
        public string DetalleHtml { get; set; }
        public int IdFicha { get; set; }

        public List<ElementoFichaTecnica> ListaElemento { get; set; }
        public int NroPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////   

        public string Resultado { get; set; }
        public string Resultado2 { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public int FlagExisteComentario { get; set; }
        public string FechaConsulta { get; set; }

        public int? FlagCheckComent { get; set; }
        public int? FlagCheckSust { get; set; }
        public int? FlagCheckFech { get; set; }
        public int TienePermisoAdmin { get; set; }

        //LISTA PARA VISTA PREVIA
        public List<FtFictecItemDTO> ListaAllItems { get; set; }
        public List<FtFictecItemDTO> ListaTreeItems { get; set; }
        public List<FtFictecXTipoEquipoDTO> ListaHijo { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<PrGrupoDTO> ListaGrupo { get; set; }

        //reportes histórico
        public List<FtExtEtapaDTO> ListaEtapas { get; set; }
        public List<FTFilaReporteHistParametro> ListaDataRptHist { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }

    public class FtPlantillaCorreo
    {
        public List<SiPlantillacorreoDTO> ListadoPlantillasCorreo { get; set; }
        public SiPlantillacorreoDTO PlantillaCorreo { get; set; }
        public List<SiTipoplantillacorreoDTO> ListaTipoPlantilla { get; set; }
        public List<FtExtEtapaDTO> ListaEtapa { get; set; }

        public string LogoEmail { get; set; }
        public int TipoCorreo { get; set; }
        public bool AccionGrabar { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
    }
}