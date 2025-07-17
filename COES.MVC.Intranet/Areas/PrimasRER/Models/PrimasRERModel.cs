using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Sic.PowerBI;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.FormatoMedicion;

using COES.Servicios.Aplicacion.PrimasRER;

namespace COES.MVC.Intranet.Areas.PrimasRER.Models
{
    public class PrimasRERModel
    {
        public bool bAgregarArchivo { get; set; }

        #region REQ001
        public List<VtpRecalculoPotenciaDTO> ListaRecalculoPotencia { get; set; }
        public List<RerCentralDTO> ListCentralRER { get; set; }
        public RerCentralDTO CentralRER { get; set; }
        public int IdCentralRER { get; set; }

        public string codigosLVTEA { get; set; }
        public string codigosPMPO { get; set; }

        public List<RerCentralLvtpDTO> ListRerCentralLvtp { get; set; }
        public RerCentralLvtpDTO RerCentralLvtp { get; set; }
        public int idRerCentralLvtp { get; set; }

        public List<RerCentralPmpoDTO> ListRerCentralPmpo { get; set; }
        public RerCentralPmpoDTO RerCentralPmpo { get; set; }
        public int idRerCentralPmpo { get; set; }


        public List<MeMedicionxintervaloDTO> ListCentralMedicion { get; set; }
        public MeMedicionxintervaloDTO CentralMedicion { get; set; }
        public int idCentralMedicion { get; set; }

        public List<MeMedicionxintervaloDTO> ListBarraMedicion { get; set; }
        public MeMedicionxintervaloDTO BarraMedicion { get; set; }
        public int idBarraMedicion { get; set; }

        public List<COES.Dominio.DTO.Transferencias.EmpresaDTO> ListEmpresas { get; set; }
        public COES.Dominio.DTO.Transferencias.EmpresaDTO Empresa { get; set; }
        public int IdEmpresa { get; set; }

        public List<CentralGeneracionDTO> ListCentralGeneracion { get; set; }
        public CentralGeneracionDTO CentralGeneracion { get; set; }
        public int IdCentralGene { get; set; }

        public List<CentralGeneracionDTO> ListCentralUnidad { get; set; }
        public CentralGeneracionDTO CentralUnidad { get; set; }
        public int IdCentralUnidad { get; set; }

        public List<VtpIngresoPotefrDetalleDTO> ListCentralUnidadLVTP { get; set; }

        public List<BarraDTO> ListBarras { get; set; }
        public BarraDTO Barra { get; set; }
        public int IdBarra { get; set; }
        #endregion

        #region REQ002

        public List<VtpPeajeIngresoDTO> ListVtpPeajeIngreso { get; set; }
        public VtpPeajeIngresoDTO VtpPeajeIngreso { get; set; }
        public List<RecalculoDTO> ListaRecalculo { get; set; }
        public string Version { get; set; }
        public string Descripcion { get; set; }
        public int EmprCodi { get; set; }
        public List<CodigoEntregaDTO> ListaCodigoEntrega { get; set; }
        public CodigoEntregaDTO CodigoEntrega { get; set; }

        public List<RerParametroPrimaDTO> ListParametroDTO { get; set; }
        public RerParametroPrimaDTO ParametroRER { get; set; }
        public int IdParametroRER { get; set; }
        public int? Rerpprinflacion { get; set; }

        public string codigosRetiroRelaciones { get; set; }

        public List<RerCentralCodRetiroDTO> ListRerCentralCodRetiro { get; set; }
        public RerCentralCodRetiroDTO RerCentralCodRetiro { get; set; }
        public int IdRerCentralCodRetiro { get; set; }
        //atributos adicionales
        public List<CodigoRetiroDTO> ListaCodigoRetiro { get; set; }
        public CodigoRetiroDTO Entidad { get; set; }

        public List<CodigoRetiroDTO> ListaCodigoRetiroTabla { get; set; }

        public string Rercenfechainicio { get; set; }
        public string Rercenfechafin { get; set; }

        public List<RerFacPerMedDTO> ListFacPerMedDTO { get; set; }
        public RerFacPerMedDTO FacPerMedDTO { get; set; }
        public int IdFacPerMedDTO { get; set; }

        public List<RerFacPerMedDetDTO> ListFacPerMedDetDTO { get; set; }
        public RerFacPerMedDetDTO FacPerMedDetDTO { get; set; }
        public int IdFacPerMedDetDTO { get; set; }


        #endregion

        #region CUS21
        public RerAnioVersionDTO entidadRerAnioVersion { get; set; }
        public List<RerParametroPrimaDTO> ListaParametroPrima { get; set; }
        #endregion

        public List<PmoPeriodoDTO> ListaMesPmo { get; set; } = new List<PmoPeriodoDTO>();
        public string Mensaje { get; set; }
        public int RegError { get; set; }
        public string MensajeError { get; set; }
        public string Resultado { get; set; }
        public string Detalle { get; set; }
        public int IdRegistro { get; set; }
        public bool UsarLayoutModulo { get; set; } = true;
        public string NombreArchivo { get; set; }
        public string RutaArchivosSddp { get; set; }

        public int AnioActual { get; set; }
        public int MesActual { get; set; }
        public List<GenericoDTO> ListaAnio { get; set; }
        public List<GenericoDTO> ListaMes { get; set; }

        public int IdPeriodo { get; set; }
        public int IdRevision { get; set; }
        public int IdEvaluacion { get; set; }
        public int IdComparativoCab { get; set; }

        public decimal EnergiaEstimadaCentral { get; set; }
        public decimal EnergiaEstimada15Min { get; set; }
        public decimal EnergiaEstimadaUnidad { get; set; }
        public decimal EnergiaSolicitadaUnidad { get; set; }
        public IndPeriodoDTO IndPeriodo { get; set; }
        public RerRevisionDTO RerRevision { get; set; }
        public RerEvaluacionDTO RerEvaluacion { get; set; }
        public List<IndPeriodoDTO> ListaPeriodo { get; set; }
        public List<RerRevisionDTO> ListaRevision { get; set; }
        public List<RerEvaluacionDTO> ListaEvaluaciion { get; set; }
        public List<RerEvaluacionEnergiaUnidadDTO> ListaEvaluacionEnergiaUnidad { get; set; }
        public HandsonModel HandsonTable { get; set; }
        public bool EsEditable { get; set; }
        public List<GenericoDTO> ListaGenerica { get; set; }
        public GraficoWeb Grafico { get; set; }
        public int CantidadEvaluacionValidado { get; set; }
        public int CantidadRevisionesTipoRevision { get; set; }

        public bool TienePermiso { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }
        public bool TienePermisoGuardar { get; set; }

        #region REQ004
        public string File { get; set; }
        InsumoDTO Insumo { get; set; }
        public List<InsumoDTO> ListaInsumos { get; set; }

        public List<RerAnioTarifarioDTO> ListaAniosTarifario { get; set; }
        public RerAnioVersionDTO AnioVersion { get; set; }
        public List<RerAnioVersionDTO> ListaAniosVersion { get; set; }

        public RerParametroRevisionDTO RevisionVTEA { get; set; }
        public List<RerParametroRevisionDTO> ListaRevisionVTEA { get; set; }

        public RerParametroRevisionDTO RevisionVTP { get; set; }
        public List<RerParametroRevisionDTO> ListaRevisionVTP { get; set; }

        public List<RerVersionDTO> ListaVersiones { get; set; }

        MesAnioTarifarioDTO MesAnioTarifario { get; set; }
        public List<MesAnioTarifarioDTO> ListarMesesAnioTarifario { get; set; }

        ReporteDTO Descarga { get; set; }
        public List<ReporteDTO> ListaReportes { get; set; }
        #endregion
    }

    public class PrimasRERResultado
    {
        public string sMensaje { get; set; }

        public int iResultado { get; set; }
        public List<PowerBIReportIntranetDTO> list { get; set; }
        
    }

    public class PrimasRERBI
    {
        public string sMensaje { get; set; }

        public int iResultado { get; set; }
        public string sId { get; set; }
        public string sName { get; set; }
    }
}