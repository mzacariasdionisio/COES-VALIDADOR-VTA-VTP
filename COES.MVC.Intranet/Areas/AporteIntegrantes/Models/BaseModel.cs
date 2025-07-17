using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Models
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

        //Identificadores a tablas
        public int Caiprscodi { get; set; }
        public int Caiajcodi { get; set; }
        public int Equicodi { get; set; }
        public int Emprcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Equicodicen { get; set; }
        public int Equicodiuni { get; set; }
        public int Barrcodi { get; set; }
        public int Tipoemprcodi { get; set; }

        //Lista de tablas
        public List<CaiPresupuestoDTO> ListaPresupuesto { get; set; }
        public List<CaiAjusteDTO> ListaAjuste { get; set; }
        public List<COES.Dominio.DTO.Transferencias.EmpresaDTO> ListaEmpresa { get; set; }
        public List<BarraDTO> ListaBarras { get; set; }
        public List<CentralGeneracionDTO> ListaEquiCen { get; set; }
        public List<CentralGeneracionDTO> ListaEquiUni { get; set; }
        public List<MePtomedicionDTO> ListaPtoMedicion { get; set; }
        public List<SiTipoempresaDTO> ListaTipoempresa { get; set; }

        //Entidades de Tablas
        public CaiPresupuestoDTO EntidadPresupuesto { get; set; }
        public CaiAjusteDTO EntidadAjuste { get; set; }
        public COES.Dominio.DTO.Transferencias.EmpresaDTO EntidadEmpresa { get; set; }
        public CentralGeneracionDTO EntidadCentral { get; set; }
        public CentralGeneracionDTO EntidadUnidad { get; set; }
        public BarraDTO EntidadBarra { get; set; }
        public MePtomedicionDTO EntidadPtoMedicion { get; set; }
    }
}