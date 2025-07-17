using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class TipoInformacionDTO
    {
        public System.Int32 TipoInfoCodi { get; set; }
        public System.String TipoInfoCodigo { get; set; }
        public System.String TipoInfoNombre { get; set; }

    }

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
        public List<RecalculoDTO> ListaRecalculos { get; set; }
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<TipoInformacionDTO> ListaTipoInfo { get; set; }
        public List<TrnEnvioDTO> ListaTrnEnvios { get; set; }
        public List<ExportExcelDTO> ListaEntregReti { get; set; }

        public List<RecalculoDTO> ListaRecalculos2 { get; set; }

        //Entidades de Tablas
        public PeriodoDTO EntidadPeriodo { get; set; }
        public RecalculoDTO EntidadRecalculo { get; set; }
        public EmpresaDTO EntidadEmpresa { get; set; }
        
        //Identificadores a tablas
        public int pericodi { get; set; }
        public int recacodi { get; set; }
        public int emprcodi { get; set; }
        public int trnenvtipinf { get; set; }
        public string trnenvplazo { get; set; }
        public string trnenvliqvt { get; set; }

        public int pericodi2 { get; set; }
        public int recacodi2 { get; set; }

        //para consultas y comparaciones
        public List<BarraDTO> ListaBarras { get; set; }

        public List<ValorTransferenciaDTO> ListaCodigos { get; set; }

    }

    
}