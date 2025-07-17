using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class PeajeIngresoModel:BaseModel
    {
        //Objetos del Modelo PeajeIngreso
        public int Pingcodi { get; set; }
        public VtpPeajeIngresoDTO Entidad { get; set; }
        public List<VtpPeajeIngresoDTO> ListaPeajeIngreso { get; set; }

        //Objetos complementarios al Modelo
        public List<VtpRepaRecaPeajeDTO> ListaRepaRecaPeaje { get; set; }
    }
}