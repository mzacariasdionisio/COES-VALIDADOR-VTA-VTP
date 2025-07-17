using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class EnvioInformacionModel
    {
        public List<ExportExcelDTO> ListaEntregReti{ get; set; }
        public ExportExcelDTO Entidad { get; set; }

        //ASSETEC 202001
        //Listas de tablas
        public List<PeriodoDTO> ListaPeriodos { get; set; }
        //public List<VtpRecalculoPotenciaDTO> ListaRecalculoPotencia { get; set; }
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<BarraDTO> ListaBarras { get; set; }
        public List<RecalculoDTO> ListaRecalculo { get; set; }
        public List<TipoInformacionDTO> ListaTipoInfo { get; set; }
        public List<TransferenciaEntregaDetalleDTO> ListaEntregaDetalle { get; set; }
        public List<TransferenciaRetiroDetalleDTO> ListaRetiroDetalle { get; set; }
        public List<TransferenciaInformacionBaseDetalleDTO> ListaInformacionBaseDetalle { get; set; }
        public List<TrnEnvioDTO> ListaEnvios { get; set; }

        //Entidades de tablas
        public RecalculoDTO EntidadRecalculo { get; set; }
        public PeriodoDTO EntidadPeriodo { get; set; }
        public EmpresaDTO EntidadEmpresa { get; set; }
        public TransferenciaEntregaDTO EntidadEntrega { get; set; }
        public TransferenciaRetiroDTO EntidadRetiro { get; set; }
        public TransferenciaInformacionBaseDTO EntidadInformacionBase { get; set; }
        public TrnEnvioDTO EntidadEnvio { get; internal set; }

        //Acciones de permisos y validación
        public bool bNuevo { get; set; }
        public bool bEditar { get; set; }
        public bool bGrabar { get; set; }
        public bool bEliminar { get; set; }
        public string sError { get; set; }
        public string sMensaje { get; set; }
        public string sFecha { get; set; }
        public string sPlazo { get; set; }

        //Identificadores a tablas
        public int Pericodi { get; set; }
        public int Recacodi { get; set; }
        public int Emprcodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public int Trnenvcodi { get; set; }
        public int Trnmodcodi { get; set; }
        public List<TrnModeloDTO> ListaModelo { get; internal set; }
    }
}