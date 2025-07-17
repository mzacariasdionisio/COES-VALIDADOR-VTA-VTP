using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class CodigoRetiroModel
    {
        public string estadoPeriodo { get; set; }
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
         
        public List<CodigoRetiroDTO> ListaCodigoRetiro { get; set; }
        public CodigoRetiroDTO Entidad { get; set; }
        public int IdcodRetiro { get; set; }

        public string Solicodiretifechainicio { get; set; }
        public string Solicodiretifechafin { get; set; }
        public string sError { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int PeriCodi { get; set; }
        public int NroRegistros { get; set; }
        public string CodigoAnterior { get; set; }
        public string SolicitudCambio { get; set; }
    }
}