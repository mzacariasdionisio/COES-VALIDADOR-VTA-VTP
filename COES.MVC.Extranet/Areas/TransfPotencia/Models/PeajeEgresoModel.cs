using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.TransfPotencia.Models
{
    public class PeajeEgresoModel : BaseModel
    {
        //Objetos del Modelo PeajeEgreso
        public int Pegrcodi { get; set; }
        public VtpPeajeEgresoDTO Entidad { get; set; }
        public VtpPeajeEgresoDTO EntidadPeajeEgreso { get; set; }
        public List<VtpPeajeEgresoDTO> ListaPeajeEgreso { get; set; }

        //Objetos del Modelo PeajeEgresoDetalle
        public VtpPeajeEgresoDetalleDTO EntidadDetalle { get; set; }
        public List<VtpPeajeEgresoDetalleDTO> ListaPeajeEgresoDetalle { get; set; }

        //Objetos complementarios al Modelo
        public string Emprnomb { get; set; }
        public string sFecha { get; set; }
        public string sPlazo { get; set; }
        public int Emprnumero { get; set; }
        public int NumRegistros { get; set; }
    }
}
