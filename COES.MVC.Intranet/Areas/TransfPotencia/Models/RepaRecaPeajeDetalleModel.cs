using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class RepaRecaPeajeDetalleModel : BaseModel
    {
        //Objetos del Modelo  RepaRecaPeajeDetalle
        public int Rrpdcodi { get; set; }
        public VtpRepaRecaPeajeDetalleDTO Entidad { get; set; }
        public List<VtpRepaRecaPeajeDetalleDTO> ListaRepaRecaPeajeDetalle { get; set; }
        
        //Objetos complementarios al Modelo
        public int Rrpecodi { get; set; }
        public int Cantidad { get; set; } //Objeto para obtener la cantidad de columnas
        
    }

}