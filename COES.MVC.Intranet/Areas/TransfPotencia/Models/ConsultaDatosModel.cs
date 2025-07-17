using COES.Dominio.DTO.Transferencias;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class ConsultaDatosModel: BaseModel
    {
        public VtpIngresoPotUnidPromdDTO Entidad { get; set; }
        public List<VtpIngresoPotUnidPromdDTO> ListaConsulta { get; set; }
        public List<VtpEmpresaPagoDTO> ListaConsultaValorizacionPotencia { get; set; }

        public List<VtpPeajeEmpresaPagoDTO> ListaConsultaPeajeEmpresaPago { get; set; }
        public List<string> ListaPeriodos { get; set; }
        public int TipoComp { get; set; }
    }
}