using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Models
{
    public class SolicitudPendienteModel
    {
        public List<SiTipoempresaDTO> ListaTipoEmpresa;
        public List<RiSolicitudDTO> ListaSolicitudes;
        public List<EstadoSolicitudDTO> ListaEstadoSolicitud;
        public bool esUsuarioDirectivo = false;
        public SolicitudPendienteModel()
        {
            ListaTipoEmpresa = new List<SiTipoempresaDTO>();
            ListaSolicitudes = new List<RiSolicitudDTO>();
            ListaEstadoSolicitud = new List<EstadoSolicitudDTO>();
            
        }
    }

    public class PaginadoModel
    {
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
    }

    public class SolicitudDetalleModel
    {
        RiSolicitudDTO objSolicitud;
        RiSolicituddetalleDTO objDetalle;

        public SolicitudDetalleModel()
        {
            objSolicitud = new RiSolicitudDTO();
            objDetalle = new RiSolicituddetalleDTO();
        }
    }
}