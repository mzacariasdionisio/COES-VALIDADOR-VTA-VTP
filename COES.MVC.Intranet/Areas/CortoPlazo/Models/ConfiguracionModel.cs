using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    public class ConfiguracionModel
    {
    }


    /// <summary>
    /// Model para el tratamiento de equivalencia de codigos
    /// </summary>
    public class RelacionModel
    {
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqRelacionDTO> ListaRelacion { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public EqRelacionDTO Entidad { get; set; }
        public int Equicodi { get; set; }
        public string Nombarra { get; set; }
        public string Idgener { get; set; }
        public int Codncp { get; set; }
        public string Nombrencp { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int Relacioncodi { get; set; }
        public string[][] Datos { get; set; }
        public int Registro { get; set; }
        public string NombreTna { get; set; }

        //- Modificado movisot 25.02.2021
        public string RER { get; set; }

        #region Ticket 2022-004245
        public string IndNoModedada { get; set; }
        public List<CmGeneradorPotenciagenDTO> ListaPotencia { get; set; }
        public List<CmGeneradorBarraemsDTO> ListaBarra { get; set; }
        public List<CmConfigbarraDTO> ListaBarraEMS { get; set; }
        public string CodigosBarra { get; set; }
        public string CodigosPotencia { get; set; }

        #endregion

        public List<EqRelacionTnaDTO> ListaAdicionalTNA { get; set; }
        public string EquipoAdicional { get; set; }
        public string MasEquiposTNA { get; set; }
    }

    /// <summary>
    /// Model para la relacion de lineas
    /// </summary>
    public class LineaModel
    {
        public List<EqGrupoLineaDTO> ListGrupoLinea { get; set; }
        public List<SiEmpresaDTO> ListEmpresa { get; set; }
        public List<EqCongestionConfigDTO> ListLinea { get; set; }
        public EqCongestionConfigDTO Entidad { get; set; }
        public List<EqEquipoDTO> ListEquipo { get; set; }
        public List<EqEquipoDTO> ListLineaEquipo { get; set; }
        public List<EqEquipoDTO> ListTrafo2dEquipo { get; set; }
        public List<EqEquipoDTO> ListTrafo3dEquipo { get; set; }
        public List<string> ListConjuntoLinea { get; set; }

        public int Equicodi { get; set; }
        public int Codncp { get; set; }
        public string Nombrencp { get; set; }
        public string Estado { get; set; }
        public int Lineacodi { get; set; } 
        public int? Grupolineacodi { get; set; }     
        public string Nodobarra1 { get; set; }
        public string Nodobarra2 { get; set; }
        public string Nodobarra3 { get; set; }
        public string Idems { get; set; }
        public string[][] Datos { get; set; }
        public int Registro { get; set; }
        public string Nombretna1 { get; set; }
        public string Nombretna2 { get; set; }
        public string Nombretna3 { get; set; }

        #region Mejoras CMgN	Nueva region
        public string Mensaje { get; set; }       
        public string Resultado { get; set; }
        public string Detalle { get; set; }
        public int? Canalcodi { get; set; }
        #endregion

        #region CMgCP_PR07
        public string TipoGrupo { get; set; }

        #endregion
    }

    /// <summary>
    /// Model para los grupos de lineas
    /// </summary>
    public class GrupoLineaModel
    {
        public EqGrupoLineaDTO Entidad { get; set; }
        public List<EqCongestionConfigDTO> ListaLinea { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal Limite { get; set; }
        public decimal Porcentaje { get; set; }
        public string Estado { get; set; }
        public string NombreNcp { get; set; }
        public int CodigoNcp { get; set; }
        public string TipoGrupo { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public int? Equipo { get; set; }
    }

    /// <summary>
    /// Model para las restricciones operativas
    /// </summary>
    public class RestriccionOperativaModel
    {
        public List<EveIeodcuadroDTO> ListaIeodcuadro { get; set; }
        public string Fecha { get; set; }
        public string[][] Datos { get; set; }
        public List<EqEquipoDTO> ListGeneradorEquipo { get; set; }        
        public List<string> ListaEvensubcausa { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
    }

    #region Regiones_seguridad

    public class RegionSeguridadModel
    {
        public CmRegionseguridadDTO Entidad { get; set; }
        public List<CmRegionseguridadDTO> Listado { get; set; }
        public List<CmRegionseguridadDetalleDTO> ListaDetalle { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal ValorM { get; set; }
        public string Direccion { get; set; }
        public string Estado { get; set; }
    }

    #endregion

}