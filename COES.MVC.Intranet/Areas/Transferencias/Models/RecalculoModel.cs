using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class RecalculoModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public bool bEjecutar { get; set; }
        public List<RecalculoDTO> ListaRecalculo { get; set; }
        public RecalculoDTO Entidad { get; set; }
        public int IdRecalculo { get; set; }

        public string Recafechavalorizacion { get; set; }
        public string Recafechalimite { get; set; }
        public string Recafechaobservacion { get; set; }
        public int ?pericodidestino { get; set; } 
        public string sError { get; set; } 

        //Modificacion Calculo Rentas Congestion

        public int NumeroRegistros { get; set; }
        public string UltimaFechaActualizacion { get; set; } 
    }
}