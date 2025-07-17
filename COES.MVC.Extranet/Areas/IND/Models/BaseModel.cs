using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.IND.Models
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

        public int Emprcodi { get; set; }
        public int Pericodi { get; set; }

        //Listas de tablas
        public List<COES.Dominio.DTO.Transferencias.EmpresaDTO> ListaEmpresas { get; set; }
        public List<IndPeriodoDTO> ListaPeriodos { get; set; }
        public List<IndRelacionEmpresaDTO> ListaRelacionEmpresa { get; set; }

        //Entidades
        public COES.Dominio.DTO.Transferencias.EmpresaDTO EntidadEmpresa { get; set; }
        public IndPeriodoDTO EntidadPeriodo { get; set; }


    }
}
