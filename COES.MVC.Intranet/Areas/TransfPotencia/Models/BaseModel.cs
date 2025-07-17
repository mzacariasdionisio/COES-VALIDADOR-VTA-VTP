using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class BaseModel
    {
        //Acciones de permisos y validación
        public bool bNuevo { get; set; }
        public bool bEditar { get; set; }
        public bool bGrabar { get; set; }
        public bool bEliminar { get; set; }
        public string sError { get; set; }
        public string sMensaje { get; set; }

        //Identificadores a tablas
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public int Pericodi2 { get; set; }
        public int Recpotcodi2 { get; set; }
        public int Emprcodi { get; set; }

        //Listas de tablas
        public List<PeriodoDTO> ListaPeriodos { get; set; }
        public List<VtpRecalculoPotenciaDTO> ListaRecalculoPotencia { get; set; }
        public List<VtpRecalculoPotenciaDTO> ListaRecalculoPotencia2 { get; set; }
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<BarraDTO> ListaBarras { get; set; }
        public List<TrnMigracionDTO> ListaTrnMigracion { get; set; }

        //Entidades de tablas
        public VtpRecalculoPotenciaDTO EntidadRecalculoPotencia { get; set; }
        public PeriodoDTO EntidadPeriodo { get; set; }
        public EmpresaDTO EntidadEmpresa { get; set; }
        public List<CentralGeneracionDTO> ListaCentrales { get; set; }
    }
}