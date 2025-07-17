using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class RepaRecaPeajeModel : BaseModel
    {
        //Objetos del Modelo RepaRecaPeaje
        public int Rrpecodi { get; set; }
        public VtpRepaRecaPeajeDTO Entidad { get; set; }
        public List<VtpRepaRecaPeajeDTO> ListaRepaRecaPeaje { get; set; }

    }
}