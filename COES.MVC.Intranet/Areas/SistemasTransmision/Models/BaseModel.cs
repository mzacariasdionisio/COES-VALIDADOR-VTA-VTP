using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Models
{
    public class BaseModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public string sError { get; set; }
        public string sMensaje { get; set; }

        //Identificadores a tablas
        public int Stpercodi { get; set; }
        public int Strecacodi { get; set; }
        public int Emprcodi { get; set; }

        //Identificadores de Tablas
        public int IdPeriodo { get; set; }
        public int IdRecalculo { get; set; }
        public int IdEmpresa { get; set; }
        public int IdSistema { get; set; }
        //Lista de tablas
        public List<StPeriodoDTO> ListaPeriodos { get; set; }
        public List<StRecalculoDTO> ListaRecalculo { get; set; }
        public List<EmpresaDTO> ListaEmpresa { get; set; }
        public List<BarraDTO> ListaBarras { get; set; }
        public List<StCompensacionDTO> ListaCompensacion { get; set; }
        public List<StBarraDTO> ListaSTBarra { get; set; }

        //Entidades de Tablas
        public StPeriodoDTO EntidadPeriodo { get; set; }
        public StRecalculoDTO EntidadRecalculo { get; set; }
        public EmpresaDTO EntidadEmpresa { get; set; }

    }
}