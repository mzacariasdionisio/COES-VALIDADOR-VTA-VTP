using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.TransfPotencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class CodigoRetiroGeneradoModel: BaseModel
    {
        public List<CodigoRetiroGeneradoDTO> ListaCodigoRetiroGenerado { get; set; }
        public CodigoRetiroGeneradoDTO EntidadCodigoRetiroGenerado { get; set; }
        public VtpPeajeEgresoDTO EntidadPeajeEgreso { get; set; }
        //Objetos del Modelo PeajeEgresoDetalle
        public VtpPeajeEgresoDetalleDTO EntidadDetalle { get; set; }
        public List<VtpPeajeEgresoDetalleDTO> ListaPeajeEgresoDetalle { get; set; }
        public string sError { get; set; }
    }
}