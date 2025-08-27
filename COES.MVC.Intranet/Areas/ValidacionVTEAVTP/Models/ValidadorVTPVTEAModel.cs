using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.DTO.ValidacionVTEAVTP;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Models
{
    public class ValidadorVtpvteaModel
    {
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }

        public string StrMensajeError {  get; set; }

        public TrnPeriodoDTO PeriodoValorizacion { get; set; }
        public VtpVersionDTO VersionesVtp { get; set; }
        public VteaVersionDTO VersionesVtea { get; set; }

        public VtpVteaDTO VtpVteaDatos { get; set; }

        public List<BarraDTO> ListBarrasBrg { get; set; }

        public List<BarraDTO> ListBarrasNoBrg { get; set; }       

        public string VistaComparacionDiferencia { get; set; }

        public string VistaComparacionVTEA{ get; set; }

        public string VistaComparacionVTP { get; set; }
    }
}