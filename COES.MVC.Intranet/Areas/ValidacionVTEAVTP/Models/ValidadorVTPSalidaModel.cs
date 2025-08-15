using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.ValidacionVTEAVTP;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Models
{
    public class ValidadorVTPSalidaModel
    {
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }

        public string StrMensajeError {  get; set; }

        public TrnPeriodoDTO PeriodoValorizacion { get; set; }
        public VtpVersionDTO VersionesVtp { get; set; }

        public VtpValidacionDTO VtpValidacion { get; set; }      

        public string VistaValorizacion { get; set; }

        public string VistaCompensacion { get; set; }
    }
}