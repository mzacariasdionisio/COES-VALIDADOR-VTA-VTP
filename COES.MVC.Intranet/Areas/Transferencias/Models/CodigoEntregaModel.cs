using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class CodigoEntregaModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }

        public List<CodigoEntregaDTO> ListaCodigoEntrega { get; set; }
        public CodigoEntregaDTO Entidad { get; set; }
        public int IdCodigoEntrega { get; set; }

        public string Codientrfechainicio { get; set; }
        public string Codientrfechafin { get; set; }
        public string sError { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int NroRegistros { get; set; }
        
    }
}