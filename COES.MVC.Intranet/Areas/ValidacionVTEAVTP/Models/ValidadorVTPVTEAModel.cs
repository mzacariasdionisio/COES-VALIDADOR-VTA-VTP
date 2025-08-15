using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.DTO.ValidacionVTEAVTP;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Models
{
    public class ValidadorVTPVTEAModel
    {
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }

        public string StrMensajeError {  get; set; }

        public List<TrnPeriodoDTO> ListPeriodos { get; set; }
        public List<VtpVersionDTO> ListVersiones { get; set; }
        public List<VteaVersionDTO> ListVersionsVTEA { get; set; }

        public VtpVteaDTO VtpVteaDatos { get; set; }

        public List<BarraDTO> ListBarrasBrg { get; set; }

        public List<BarraDTO> ListBarrasNoBrg { get; set; }       

        public string VistaComparacionDiferencia { get; set; }

        public string VistaComparacionVTEA{ get; set; }

        public string VistaComparacionVTP { get; set; }
    }
}