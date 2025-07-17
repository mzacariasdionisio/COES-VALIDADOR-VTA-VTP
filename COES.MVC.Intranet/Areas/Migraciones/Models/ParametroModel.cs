using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Migraciones.Models
{
    public class ParametroModel
    {
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<PrGrupoDTO> ListaGrupo { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<PrCategoriaDTO> ListaCategoria { get; set; }
        public List<EqAreaDTO> ListaUbicacion { get; set; }
        public List<SiFuenteenergiaDTO> ListaFenerg { get; set; }
        public List<SiEmpresaDTO> ListaEmpresaForm { get; set; }

        public List<PrGrupodatDTO> ListaParametros { get; set; }
        public bool AccesoNuevo { get; set; }
        public bool AccesoEditar { get; set; }
        public bool AccesoEditarOsignermin { get; set; }
        public bool AccesoEditarSddp { get; set; }
        public bool TienePermisoImportar { get; set; }

        public List<PrAgrupacionDTO> ListaAgrupacion { get; set; }
        public PrAgrupacionDTO Agrupacion { get; set; }
        public List<PrConceptoDTO> ListaConcepto { get; set; }
        public List<PrAgrupacionConceptoDTO> ListaAgrupacionConcepto { get; set; }

        public string Fecha { get; set; }
        public string FechaFull { get; set; }
        public PrGrupoDTO Grupo { get; set; }
        public List<PrGrupodatDTO> ListaGrupodat { get; set; }
        public List<PrGrupoEquipoValDTO> ListaGrupoEquipoVal { get; set; }
        public List<string> ListaUnidad { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<PrGrupoeqDTO> ListaRelacionEquipo { get; set; }

        public List<FwAreaDTO> ListaArea { get; set; }
        public List<UsuarioParametro> ListaUsuario { get; set; }
        public List<PrAreaConceptoDTO> ListaAreaConcepto { get; set; }
        public List<int> ListaUsuariocodi { get; set; }
        public string ListaUsuarioJson { get; set; }
        public string ListaAreaJson { get; set; }

        public int NroPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public int EquicodiFiltro { get; set; }
        public int IdAgrupacion { get; set; }
        public int Grupocodi { get; set; }

        public PrRepcvDTO RepCV { get; set; }
        public int Repcodi { get; set; }
        public bool HabilitaCargaMasiva { get; set; }

        //Parametros gruposMop 
        public string NombreArchivo { get; set; }
        public List<PrGrupodatDTO> ListaGrupoDatErrores { get; set; }
        public List<PrGrupodatDTO> ListaGrupoDatCorrectos { get; set; }
        public List<FileData> ListaDocumentos { get; set; }
        public FileData Documento { get; set; }
        public string FileName { get; set; } //nombre archivo
        public int TipoExportacion { get; set; }

        #region Ficha tecnica 2023         
        public List<FtExtReleqpryDTO> ListaProyectosGrupo { get; set; }        
        public List<FtExtProyectoDTO> ListadoProyectos { get; set; }
        public List<EpoEstudioEoDTO> ListadoEstudiosEo { get; set; }        
        public FtExtProyectoDTO Proyecto { get; set; }
        public FtExtReleqpryDTO RelEquipoProyecto { get; set; }
        #endregion
    }
}