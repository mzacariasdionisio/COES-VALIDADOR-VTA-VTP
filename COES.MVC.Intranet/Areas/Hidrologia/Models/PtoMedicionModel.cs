using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static COES.Dominio.DTO.Sic.EqEquipoDTO;

namespace COES.MVC.Intranet.Areas.Hidrologia.Models
{
    public class PtoMedicionModel
    {
        public PtoMedicionModel()
        {
            ListaAreasSuministro= new List<EqAreaDTO>();
            ListaEmpresasSuministradoras= new List<SiEmpresaDTO>();
        }

        public List<MePtomedicionDTO> ListaPtoMedicion { get; set; }
        public List<Dominio.DTO.Sic.EmpresaDTO> ListaEmpresas { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<MeTipopuntomedicionDTO> ListaTipoPuntoMedicion { get; set; }
        public List<MeOrigenlecturaDTO> ListaOrigenLectura { get; set; }
        public MePtomedicionDTO Ptomedicion { get; set; }

        public List<TipoSerie> ListaTipoSerie { get; set; }
        public int IdPtomedicodi { get; set; }
        public int IdEquipo { get; set; }
        public int IdEmpresa { get; set; }
        public int IdOrigenLectura { get; set; }
        public short IdTipoPtoMedicion { get; set; }
        public string IdOsicodi { get; set; }
        public int IdOrden { get; set; }
        public string IdPtomedibarranomb { get; set; }
        public string IdPtomedielenomb { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool OpcionEditar { get; set; }
        public bool OpcionEspecial { get; set; }
        public decimal? TensionBarra { get; set; }
        public string AreaOperativaEquipo { get; set; }
        public string Ptomediestado { get; set; }
        //PtoMedicion PR16
        public List<SiEmpresaDTO> ListaEmpresasSuministradoras { get; set; }
        public decimal? TensionSuministro { get; set; }
        public int IdAreaCodiSuministro { get; set; }

        public int TipoSerie { get; set; }
        public List<EqAreaDTO> ListaAreasSuministro { get; set; }
        public int IdSuministrador { get; set; }
        //inicio modificado
        /// <summary>
        /// propiedades familia embalse
        /// </summary>
        public int IdCategoriaPadre { get; set; }
        public List<EqCategoriaDTO> ListaCategoriaSuperior { get; set; }
        public List<EqCategoriaDTO> ListaCategoria { get; set; }
        public List<EqCategoriaDetDTO> ListaSubclasificacion { get; set; }
        //fin modificado

        public List<PrGrupoDTO> ListaGrupo { get; set; }
        public List<PrCategoriaDTO> ListaTipoGrupo { get; set; }
        public int IndicadorFuente { get; set; }
        public int IdCategoria { get; set; }

        #region FIT-Aplicativo VTD
        public List<BarraDTO> ListBarra { get; set; }
        #endregion
    }


    public class BusquedaPtoMedicionModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<MeTipopuntomedicionDTO> ListaTipoPuntoMedicion { get; set; }
        public List<SiTipoinformacionDTO> ListaTipoInformacion { get; set; }
        public List<MeOrigenlecturaDTO> ListaOrigenLectura { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<PrCategoriaDTO> ListaTipoGrupo { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<EqAreaDTO> ListaAreas { get; set; }
        public bool OpcionEditar { get; set; }
        public bool OpcionEspecial { get; set; }
        public string FiltroPto { get; set; }
        public int TipoFuente { get; set; }
        public List<GenericoDTO> ListaFuente { get; set; }

        #region FIT-Aplicativo VTD
        public List<BarraDTO> ListBarra { get; set; }
        #endregion
    }
}