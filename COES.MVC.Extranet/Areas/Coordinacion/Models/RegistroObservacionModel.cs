using COES.Dominio.DTO.Scada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Coordinacion.Models
{
    public class RegistroObservacionModel
    {
        public List<ScEmpresaDTO> ListaEmpresas { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int IdEmpresa { get; set; }
        public List<TrObservacionDTO> ListadoObservacion { get; set; }
        public TrObservacionDTO Entidad { get; set; }
        public List<TrZonaSp7DTO> ListaZonas { get; set; }
        public List<TrCanalSp7DTO> ListadoCanal { get; set; }
        public List<TrObservacionItemEstadoDTO> ListaHistoriaItem { get; set; }
        public List<TrObservacionItemDTO> ListadoSeniales { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int IdObservacion { get; set; }
        public bool IndicadorEmpresa { get; set; }

        public string ComentariosAgente { get; set; }
        public string Estado { get; set; }

        public string[][] Datos { get; set; }
        public string[] Estados { get; set; }
    }
}