using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class AuditoriaModel
    {
        //Objetos del Modelo RangoValDatos
        public VtpTipoAplicacionDTO Entidad { get; set; }
        public List<VtpTipoAplicacionDTO> ListaTipoAplicacion{ get; set; }

        public List<VtpTipoProcesoDTO> ListaTipoProceso { get; set; }

        public List<VtpAuditoriaProcesoDTO> ListaAuditoriaProceso { get; set; }



        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int NroRegistros { get; set; }


    }
}