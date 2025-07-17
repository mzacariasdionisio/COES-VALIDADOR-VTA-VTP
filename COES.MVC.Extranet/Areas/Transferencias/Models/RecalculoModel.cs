using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class RecalculoModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public bool bEjecutar { get; set; }
        public List<RecalculoDTO> ListaRecalculos { get; set; }
        public RecalculoDTO Entidad { get; set; }
        public int IdRecalculo { get; set; }
        public string sError { get; set; }
    }
}