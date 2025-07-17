using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class IngresoPotefrDetalleModel : BaseModel
    {

        //Objetos del Modelo IngresoPotefr
        public int Ipefrcodi { get; set; }
        public int Intervalo { get; set; }

        public VtpIngresoPotefrDetalleDTO Entidad { get; set; }
        public List<VtpIngresoPotefrDetalleDTO> ListaIngresoPotefrDetalle { get; set; }
        
    }
}