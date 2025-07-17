using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class BarraursModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }

        public List<TrnBarraursDTO> ListaBarraURS { get; set; }
        public TrnBarraursDTO EntidadBarraURS { get; set; }

        public List<BarraDTO> ListaBarra { get; set; }
        public BarraDTO EntidadBarra { get; set; }

        public List<CentralGeneracionDTO> listaUnidadGen { get; set; }
        public List<EmpresaDTO> listaEmpresas { get; set; }

        public int IdBarra { get; set; }
        public string sError { get; set; }
    }

    public class PrGrupoModel
    {
        public int IdBarra { get; set; }
        public string UrsNombre { get; set; }
    }
}