using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class RetiroPotenciaSCModel : BaseModel
    {
        //Objetos del Modelo RetiroPotenciaSC
        public int Rpsccodi { get; set; }
        public VtpRetiroPotescDTO Entidad { get; set; }
        public List<VtpRetiroPotescDTO> ListaRetiroPotenciaSC { get; set; }

    }
}
