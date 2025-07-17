using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Equipamiento.Models
{
    public class EquipamientoModel
    {
        public string Fecha { get; set; }
        public int Count { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public List<string> ListaResultado { get; set; }

        public List<int> ListaCount { get; set; }
        public List<EqPropequiDTO> ListaPropiedad { get; set; }
        public HandsonModel Handson { get; set; }
        public List<int> ListaFlagVigenciaCorrecta { get; set; }
        public EqEquipoDTO EquipoSeleccionado { get; set; }
    }

    public class DatoComboBox
    {
        public string Descripcion { get; set; }
        public string Valor { get; set; }
    }
    #region Area
    public class AreaModel
    {
        public List<EqTipoareaDTO> ListaTipoArea { get; set; }
        public int idTipoArea { get; set; }
        public string NombreArea { get; set; }
        public List<EqAreaDTO> ListaArea { get; set; }
        //Paginado
        public int NroPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public List<EstadoModel> ListaEstados { get; set; }
        public string EstadoCodigo { get; set; }
        public string EnableNuevo { get; set; }
        public string EnableEditar { get; set; }
    }

    public class AreaDetalleModel
    {
        public List<EqTipoareaDTO> ListaTipoArea { get; set; }
        public decimal Areacodi { get; set; }
        public decimal? Tareacodi { get; set; }
        public string Areaabrev { get; set; }
        public string Areanomb { get; set; }
        public decimal Areapadre { get; set; }
        public string AreaUsuIns { get; set; }
        public string FechaIns { get; set; }
        public string AreaUsuUpd { get; set; }
        public string FechaUpd { get; set; }
        public List<EstadoModel> lsEstado { get; set; }
        public string EstadoDesc { get; set; }
        public string EstadoCodigo { get; set; }

    }
    #endregion
    #region Familia
    public class FamiliaListaModel
    {
        public List<EqFamiliaDTO> listaFamilia { get; set; }
        public int NroPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public string EnableEditar { get; set; }
    }

    public class FamiliaDetalleModel
    {
        public int Famcodi { get; set; }
        public string Famabrev { get; set; }
        public int? Tipoecodi { get; set; }
        public int? Tareacodi { get; set; }
        public string Famnomb { get; set; }
        public int? Famnumconec { get; set; }
        public string Famnombgraf { get; set; }
        public List<EqTipoareaDTO> ListaTipoArea { get; set; }
        public string EstadoDescripcion { get; set; }
        public List<EstadoModel> ListaEstados { get; set; }
        public string Famestado { get; set; }
        public string UsuarioCreacion { get; set; }
        public string FechaCreacion { get; set; }
        public string UsuarioUpdate { get; set; }
        public string FechaUpdate { get; set; }

    }

    public class FamiliaIndexModel
    {
        public List<EstadoModel> ListaEstados { get; set; }

        public string EnableNuevo { get; set; }

        public string EnableEditar { get; set; }
    }
    #endregion
    #region Propiedad

    public class PropiedadModel
    {
        public int IdPropiedad { get; set; }
        public EqPropiedadDTO Entidad { get; set; }
        public List<DatoComboBox> ListaFichatecnica { get; set; }
        public List<DatoComboBox> ListaTipoDato { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<EqPropiedadDTO> ListaPropiedad { get; set; }

        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public bool TienePermiso { get; set; }

        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        //public bool AccionEliminar { get; set; }
        //public bool AccionGrabar { get; set; }

        public string NombreArchivo { get; set; }
        public List<EqPropiedadDTO> ListaPropiedadesErrores { get; set; }
        public List<EqPropiedadDTO> ListaPropiedadesCorrectas { get; set; }
        public List<FileData> ListaDocumentos { get; set; }
        public FileData Documento { get; set; }
        public string FileName { get; set; } //nombre archivo

        public List<EprPropCatalogoDataDTO> ListaFunciones { get; set; }
    }
    #endregion
    #region Equipos

    public class IndexEquipoModel
    {
        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
        public int iTipoEmpresa { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public int iEmpresa { get; set; }
        public List<EqFamiliaDTO> ListaTipoEquipo { get; set; }
        public int iTipoEquipo { get; set; }
        public List<EstadoModel> ListaEstados { get; set; }
        public string sEstadoCodi { get; set; }
        public string CodigoEquipo { get; set; }
        public int NroPagina { get; set; }
        public List<EqEquipoDTO> ListadoEquipamiento { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public string NombreEquipo { get; set; }

        public string EnableNuevo { get; set; }

        public string EnableEditar { get; set; }

        public bool AccesoNuevo { get; set; }
        public bool AccesoEditar { get; set; }

        public bool TienePermiso { get; set; }
        public bool TienePermisoAdminFT { get; set; }
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public string NombreArchivo { get; set; }
        public List<EqEquipoDTO> ListaEquiposErrores { get; set; }
        public List<EqEquipoDTO> ListaEquiposCorrectos { get; set; }
        public List<FileData> ListaDocumentos { get; set; }
        public FileData Documento { get; set; }
        public string FileName { get; set; } //nombre archivo
        public int TipoExportacion { get; set; }

        public List<EqPropequiDTO> ListaPropEquiErrores { get; set; }
        public List<EqPropequiDTO> ListaPropEquiCorrectos { get; set; }

        #region Ficha tecnica 2023

        public EqEquipoDTO Equipo { get; set; }
        public List<FtExtReleqpryDTO> ListaProyectosEquipo { get; set; }
        public List<FtExtReleqempltDTO> ListadoEmpresasCopropietarias { get; set; }
        public List<EmpresaCoes> ListaEmpresas { get; set; }

        public List<FtExtReleqpryDTO> ListaReleqpryErrores { get; set; }
        public List<FtExtReleqpryDTO> ListaReleqpryCorrectos { get; set; }

        #endregion
    }

    public class DetalleEquipoModel
    {
        public int Equicodi { get; set; }
        public int? Emprcodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Elecodi { get; set; }
        public int? Areacodi { get; set; }
        public int? Famcodi { get; set; }
        public string Equiabrev { get; set; }
        public string Equinomb { get; set; }
        public string Equiabrev2 { get; set; }
        public decimal? Equitension { get; set; }
        public int? Equipadre { get; set; }
        public string EquipadreDesc { get; set; }
        public decimal? Equipot { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Ecodigo { get; set; }
        public string Equiestado { get; set; }
        public string EquiestadoDesc { get; set; }
        public string Osinergcodi { get; set; }
        public string OsinergcodiGen { get; set; }         // ticket-6068
        public int? Lastcodi { get; set; }
        public string Equifechiniopcom { get; set; }
        public string Equifechfinopcom { get; set; }
        public string UsuarioUpdate { get; set; }
        public DateTime? FechaUpdate { get; set; }
        public string EquiManiobra { get; set; }
        public string EquiManiobraDesc { get; set; }
        public string EMPRNOMB { get; set; }
        public string AREANOMB { get; set; }
        public string Famnomb { get; set; }
        public int Operadoremprcodi { get; set; }
        public int TipoEmpresa { get; set; }

        //Datos para listas
        public List<DatoComboBox> ListaProcManiobras { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqFamiliaDTO> ListaTipoEquipo { get; set; }
        public List<EstadoModel> ListaEstados { get; set; }
        public List<EqAreaDTO> ListaUbicaciones { get; set; }
        public List<EqEquipoDTO> ListaEquipos { get; set; }
        public List<SiEmpresaDTO> ListaOperadores { get; set; }
        public List<PrGrupoDTO> ListaGrupo { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }

        #region Equipos sin datos de ficha técnica
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string PropVaciasCount { get; set; }
        public List<EqPropequiDTO> ListadoValoresVacios { get; set; }
        public List<EqPropiedadDTO> ListaPropValidas { get; set; }
        public List<List<string>> ListaConstantes { get; set; }
        #endregion
    }

    #endregion
    #region Equipo-Propiedad

    public class EquipoPropiedadesModel
    {
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public string Equiabrev { get; set; }
        public string Emprnomb { get; set; }
        public string Areanomb { get; set; }
        public string Famnomb { get; set; }
        public string PropiedadNombre { get; set; }
        public List<EqPropequiDTO> ListadoValoresPropiedades { get; set; }
        public int NroPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public string Fecha { get; set; }

        public string EnableEditar { get; set; }

        public string EnableNuevo { get; set; }

        public bool AccesoNuevo { get; set; }
        public bool AccesoEditar { get; set; }
    }

    public class HistoricoPropiedadModel
    {
        public int Equicodi { get; set; }
        public int Propcodi { get; set; }
        public string PropNomb { get; set; }
        public string Equinomb { get; set; }
        public List<EqPropequiDTO> ListaValoresHistoricos { get; set; }
        public bool bExistenDatos { get; set; }
        public bool MostrarColAdicional { get; set; }
    }

    public class ValorPropiedadModel
    {
        public int Equicodi { get; set; }
        public int Propcodi { get; set; }
        public string PropNomb { get; set; }
        public string Valor { get; set; }
        public bool File { get; set; }
    }

    #endregion

    #region TIPOREL
    public class TipoRelModel
    {
        public int idTipoRel { get; set; }
        public string NombreTiporel { get; set; }
        public List<EqTiporelDTO> ListaTiporel { get; set; }
        //Paginado
        public int NroPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public List<EstadoModel> ListaEstados { get; set; }
        public string EstadoCodigo { get; set; }
        public string EnableNuevo { get; set; }
        public string EnableEditar { get; set; }
    }

    public class TipoRelDetalleModel
    {
        public int Tiporelcodi { get; set; }
        public string Tiporelnomb { get; set; }
        public string TiporelEstado { get; set; }
        public string TiporelUsuarioCreacion { get; set; }
        public string TiporelFechaCreacion { get; set; }
        public string TiporelUsuarioUpdate { get; set; }
        public string TiporelFechaUpdate { get; set; }
        public string EstadoDesc { get; set; }
        public List<EstadoModel> lsEstado { get; set; }
    }

    public class FamRelModel
    {
        public int Tiporelcodi { get; set; }
        public int Famcodi1 { get; set; }
        public int Famcodi2 { get; set; }
        public int? Famnumconec { get; set; }
        public string Famreltension { get; set; }
        public string Famrelestado { get; set; }
        public string Famrelusuariocreacion { get; set; }
        public string Famrelfechacreacion { get; set; }
        public string Famrelusuarioupdate { get; set; }
        public string Famrelfechaupdate { get; set; }
        public List<EqFamiliaDTO> LsTipoEquipo { get; set; }
        public List<DatoComboBox> LsTension { get; set; }
        public List<EstadoModel> lsEstado { get; set; }
        public string Famnomb1 { get; set; }
        public string Famnomb2 { get; set; }
        public string EstadoDesc { get; set; }
        public string EnableNuevo { get; set; }
        public string EnableEditar { get; set; }
    }

    public class FamRelTipoRelIndexModel
    {
        public int Tiporelcodi { get; set; }
        public string Tiporelnomb { get; set; }
        public List<EstadoModel> lsEstado { get; set; }
        public List<FamRelModel> lsFamRel { get; set; }
        public string EnableNuevo { get; set; }
        public string EnableEditar { get; set; }
    }

    #endregion

    #region EQ_EQUIREL

    public class EquipoRelModel
    {
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<AreaDTO> ListaArea { get; set; }

        public List<EqFamrelDTO> ListaFamiliaRel { get; set; }
        public EqEquirelDTO EquipoTopologia { get; set; }
        public List<EqEquirelDTO> ListaEquiposRel { get; set; }

        public int Tiporelcodi { get; set; }
        public string NombreTiporel { get; set; }
        public string FamcodiDefault { get; set; }

        public int Equicodi1 { get; set; }
        public int Equicodi2 { get; set; }
        public int Famcodi1 { get; set; }
        public int Famcodi2 { get; set; }
        public string Famnomb1 { get; set; }
        public string Famnomb2 { get; set; }

        public int IdEquipo { get; set; }
        public string FiltroFamilia { get; set; }
        public int TipoFormulario { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        // -------------------------------------------------------------------------------------
        // Propiedades de Paginado
        // -------------------------------------------------------------------------------------
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int NroRegistros { get; set; }
    }

    #endregion

    //inicio modificado
    #region Categoria
    public class CategoriaIndexModel
    {
        public string EnableNuevo { get; set; }
        public string EnableEditar { get; set; }
        public bool AccesoNuevo { get; set; }
        public bool AccesoEditar { get; set; }
        public List<EstadoModel> ListaEstados { get; set; }
        public List<EqFamiliaDTO> ListaTipoEquipo { get; set; }
    }

    public class CategoriaListaModel
    {
        public List<EqCategoriaDTO> listaCategoria { get; set; }
        public int NroPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public string EnableEditar { get; set; }
    }

    public class CategoriaViewModel
    {
        public int Ctgcodi { get; set; }
        public int? Ctgpadre { get; set; }
        public int Famcodi { get; set; }
        public string Ctgnomb { get; set; }
        public string CtgFlagExcluyente { get; set; }
        public string Ctgestado { get; set; }

        public string UsuarioCreacion { get; set; }
        public string FechaCreacion { get; set; }
        public string UsuarioUpdate { get; set; }
        public string FechaUpdate { get; set; }

        public string Famnomb { get; set; }
        public string Ctgpadrenomb { get; set; }
        public string CtgestadoDescripcion { get; set; }
        public string CtgFlagExcluyenteDescripcion { get; set; }
        public List<EqFamiliaDTO> ListaTipoEquipo { get; set; }
        public List<EstadoModel> ListaEstadoFlag { get; set; }
        public List<EqCategoriaDTO> listaCategoria { get; set; }
        public List<EstadoModel> ListaEstados { get; set; }
    }
    #endregion

    #region Categoria Detalle
    public class CategoriaDetIndexModel
    {
        public int Ctgcodi { get; set; }
        public string Ctgnomb { get; set; }
        public string EnableNuevo { get; set; }
        public string EnableEditar { get; set; }
        public bool AccesoNuevo { get; set; }
        public bool AccesoEditar { get; set; }
    }

    public class CategoriaDetListaModel
    {
        public List<EqCategoriaDetDTO> listaCategoriaDetalle { get; set; }
        public int NroPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public string EnableEditar { get; set; }
    }

    public class CategoriaDetViewModel
    {
        public int Ctgdetcodi { get; set; }
        public string Ctgdetnomb { get; set; }
        public int Ctgcodi { get; set; }
        public int? Ctgpadre { get; set; }
        public int Famcodi { get; set; }
        public string Ctgnomb { get; set; }
        public string Ctgestado { get; set; }
        public string Ctgdetestado { get; set; }

        public string UsuarioCreacion { get; set; }
        public string FechaCreacion { get; set; }
        public string UsuarioUpdate { get; set; }
        public string FechaUpdate { get; set; }

        public string Famnomb { get; set; }
        public string Ctgpadrenomb { get; set; }
        public string CtgdetestadoDescripcion { get; set; }
        public List<EstadoModel> ListaEstados { get; set; }
    }
    #endregion


    #region Categoria Equipo
    public class CategoriaEquipoIndexModel
    {
        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
        public int iTipoEmpresa { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public int iEmpresa { get; set; }
        public int iCategoria { get; set; }
        public int iSubclasificacion { get; set; }
        public List<EqFamiliaDTO> ListaTipoEquipo { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<EqCategoriaEquipoDTO> ListaCategoriaEquipo { get; set; }
        public List<EqCategoriaDTO> ListaCategoria { get; set; }
        public List<EqCategoriaDetDTO> ListaSubclasificacion { get; set; }
        public int iTipoEquipo { get; set; }
        public List<EstadoModel> ListaEstados { get; set; }
        public string sEstadoCodi { get; set; }
        public string CodigoEquipo { get; set; }
        public int NroPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public string NombreEquipo { get; set; }

        public string EnableNuevo { get; set; }
        public string EnableEditar { get; set; }
        public bool AccesoNuevo { get; set; }
        public bool AccesoEditar { get; set; }
    }

    public class CategoriaEquipoViewModel
    {
        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqFamiliaDTO> ListaTipoEquipo { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<EqCategoriaDTO> ListaCategoria { get; set; }
        public List<EqCategoriaDetDTO> ListaSubclasificacion { get; set; }
        public int Ctgdetcodi { get; set; }
        public int Equicodi { get; set; }
        //inicio agregado
        public int CtgdetcodiOld { get; set; }
        public int EquicodiOld { get; set; }
        //fin agregado
        public string Ctgequiestado { get; set; }
        public string Ctgequiestadodescripcion { get; set; }

        public string Ctgdetnomb { get; set; }
        public int Ctgcodi { get; set; }
        public int? Ctgpadre { get; set; }
        public int Famcodi { get; set; }
        public string Ctgnomb { get; set; }
        public string Ctgestado { get; set; }
        public string Ctgdetestado { get; set; }

        public string UsuarioCreacion { get; set; }
        public string FechaCreacion { get; set; }
        public string UsuarioUpdate { get; set; }
        public string FechaUpdate { get; set; }

        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Famnomb { get; set; }
        public string Ctgpadrenomb { get; set; }
        public string CtgdetestadoDescripcion { get; set; }
        public List<EstadoModel> ListaEstados { get; set; }
    }
    #endregion
    //fin modificado
}