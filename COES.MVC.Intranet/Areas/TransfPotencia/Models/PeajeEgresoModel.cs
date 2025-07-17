using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class PeajeEgresoModel:BaseModel
    {
        //Objetos del Modelo RetiroPotenciaSC
        public int Pegrcodi { get; set; }
        public VtpPeajeEgresoDTO Entidad { get; set; }
        public VtpPeajeEgresoDTO EntidadPeajeEgreso { get; set; }
        public List<VtpPeajeEgresoDTO> ListaPeajeEgreso { get; set; }
        public List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoMinfo { get; set; }
        public List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoEmpresa { get; set; }

        //Objetos complementarios al Modelo
        public string Emprnomb { get; set; }
        public int Emprnumero { get; set; }

        //Detalle
        public VtpPeajeEgresoDetalleDTO EntidadDetalle { get; set; }
        public List<VtpPeajeEgresoDetalleDTO> ListaPeajeEgresoDetalle { get; set; }
        
    }
}