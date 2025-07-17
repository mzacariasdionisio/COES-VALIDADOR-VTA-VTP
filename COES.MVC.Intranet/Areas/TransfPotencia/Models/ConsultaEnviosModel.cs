using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class ConsultaEnviosModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public string sError { get; set; }
        public string sMensaje { get; set; }
        public int iNumReg { get; set; }

        
        //Lista de tablas
        public List<PeriodoDTO> ListaPeriodos { get; set; }
        public List<VtpRecalculoPotenciaDTO> ListaRecalculos { get; set; }
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<VtpPeajeEgresoDTO> ListaPeajeEgreso { get; set; }
        public List<ExportExcelDTO> ListaEntregReti { get; set; }

        //Entidades de Tablas
        public PeriodoDTO EntidadPeriodo { get; set; }
        public VtpRecalculoPotenciaDTO EntidadRecalculo { get; set; }
        public EmpresaDTO EntidadEmpresa { get; set; }
        
        //Identificadores a tablas
        public int pericodi { get; set; }
        public int recpotcodi { get; set; }
        public int emprcodi { get; set; }
        public string plazo { get; set; }
        public string liquidacion { get; set; }
        
    }

    
}