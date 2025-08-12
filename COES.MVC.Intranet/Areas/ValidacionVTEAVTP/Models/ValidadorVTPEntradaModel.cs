using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.ValidacionVTEAVTP;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Models
{
    public class ValidadorVTPEntradaModel
    {
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }

        public string StrMensajeError {  get; set; }

        public List<TrnPeriodoDTO> ListPeriodos { get; set; }
        public List<VteVersionDTO> ListVersiones { get; set; }

        public VtpDTO DatosVTP { get; set; }
        
        public string VistaBarrasBrg {  get; set; }
        public string VistaBarrasNoBrg { get; set; }

        public string VistaBarrasSinAnalizar { get; set; }

        public string VistaBarrasDiferencia { get; set; }

        public List<string> EmpresasBarra { get; set; }
      
    }
}