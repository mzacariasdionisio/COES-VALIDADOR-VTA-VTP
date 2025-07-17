using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.CPPA;

namespace COES.MVC.Intranet.Areas.CPPA.Models
{
    public class CPPAModel
    {
        #region General
        //Acciones de permisos y validación
        public bool bNuevo { get; set; }
        public bool bEditar { get; set; }
        public bool bGrabar { get; set; }
        public bool bEliminar { get; set; }
        public string sError { get; set; }
        public string sMensaje { get; set; }
        public string sTipo { get; set; }
        public string sResultado { get; set; }
        public string sDetalle { get; set; }

        #endregion

        #region REQ002
        public List<GenericoDTO> ListaAnio { get; set; }
        public List<CpaAjustePresupuestalDTO> ListAjustePresupuestal { get; set; }
        public CpaAjustePresupuestalDTO AjustePresupuestal { get; set; }
        public List<CpaRevisionDTO> ListRevision { get; set; }
        public CpaRevisionDTO Revision { get; set; }
        public List<CpaHistoricoDTO> ListHistorico { get; set; }
        public CpaHistoricoDTO Historico { get; set; }
        #endregion

        #region REQ007
        public List<CpaInsumoDTO> ListInsumo { get; set; }
        public CpaInsumoDTO Insumo { get; set; }
        MesAnioPresupuestalDTO MesAnioTarifario { get; set; }
        public List<MesAnioPresupuestalDTO> ListarMesesAnioPresupuestal { get; set; }
        public string NombInsumo { get; set; }
        public string[] NombMeses { get; set; }

        public string[] ListInsumos { get; set; }
        #endregion

        #region REQ008
        public int iNumMes { get; set; }
        public string sLog { get; set; }

        #endregion

        #region REQ010
        public CpaCalculoDTO Calculo { get; set; }
        public string sEstadoRevision { get; set; }
        public string sExiteCentrales { get; set; }
        public string sSeProcesoCalculo { get; set; }

        public string sRutaArchivosSddp { get; set; }
        #endregion

        #region REQ03
        public List<SiEmpresaDTO> ListEmpresasGeneracion { get; set; }
        public List<SiEmpresaDTO> ListEmpresasDistribucion { get; set; }
        public List<SiEmpresaDTO> ListEmpresasLibres { get; set; }
        public List<SiEmpresaDTO> ListEmpresasTransmision { get; set; }
        public List<EqEquipoDTO> ListCentralesGeneradoras { get; set; }
        public List<CpaEmpresaDTO> ListEmpresasIntegrantes{ get; set; }
        public List<CpaCentralDTO> ListCentralesIntegrantes { get; set; }
        #endregion

        #region REQ04
        public List<CpaEmpresaDTO> filtroEmpresasIntegrantes { get; set; }
        public List<CpaCentralDTO> filtroCentralesIntegrantes { get; set; }
        public List<BarraDTO> filtroBarrasTransIntegrantes { get; set; }
        public List<CpaCentralDTO> GrillaPrincipal { get; set; }
        public List<BarraDTO> popupfiltroBarrasTransferencia { get; set; }
        public List<MePtomedicionDTO> popupfiltroBarrasPMPO { get; set; }
        public List<MePtomedicionDTO> popupfiltroCentralesPMPO { get; set; }
        public List<CpaCentralPmpoDTO> popupGrillaCentralesPMPO { get; set; }
        #endregion

        #region REQ05
        public List<CpaParametroDTO> GrillaPrincipalParametros { get; set; }   
        public List<CpaParametroHistoricoDTO> GrillaHistoricoParametros { get; set; }
        #endregion

        #region REQ011
        public List<GenericoDTO> ListaReporte { get; set; }
        #endregion

        #region REQ12 y REQ13
        public List<CpaDocumentosDTO> ListaDocumentosGrilla { get; set; }
        public List<CpaDocumentosDetalleDTO> ListaDocumentosDetalleGrilla { get; set; }
        public List<SiEmpresaDTO> ListaEmpresasDescarga { get; set; }
        #endregion

        #region REQ016
        public string EstadoPublicacion { get; set; }
        public List<GenericoDTO> ListaReporte2 { get; set; }
        #endregion
    }
}