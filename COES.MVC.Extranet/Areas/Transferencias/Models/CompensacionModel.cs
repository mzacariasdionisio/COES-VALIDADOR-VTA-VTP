using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class CompensacionModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public List<CompensacionDTO> ListaCompensacion { get; set; }
        public CompensacionDTO Entidad { get; set; }
        public int IdCompensacion { get; set; }

        public string sError { get; set; }
    }
}