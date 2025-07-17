using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Models
{
    public class BaseModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public string sError { get; set; }
        public string sMensaje { get; set; }
        public int iNumReg { get; set; }
        public int PeriAnioMes { get; set; }

        //Identificadores a tablas
        public int Pericodi { get; set; }
        public int Vcrecacodi { get; set; }
        public int Equicodi { get; set; }
        public int Emprcodi { get; set; }

        //Lista de tablas
        public List<PeriodoDTO> ListaPeriodos { get; set; }
        public List<VcrRecalculoDTO> ListaRecalculo { get; set; }
        public List<EmpresaDTO> ListaEmpresa { get; set; }
        public List<BarraDTO> ListaBarras { get; set; }
        //public List<StCompensacionDTO> ListaCompensacion { get; set; }
        //public List<StBarraDTO> ListaSTBarra { get; set; }

        //Entidades de Tablas
        public PeriodoDTO EntidadPeriodo { get; set; }
        public VcrRecalculoDTO EntidadRecalculo { get; set; }
        public EmpresaDTO EntidadEmpresa { get; set; }

    }
}