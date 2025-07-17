using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class AjusteSaldosModel : BaseModel
    {
        //Objetos del Modelo AjusteSaldo
        public PeriodoDTO EntidadPeriodo { get; set; }
        public VtpPeajeCargoAjusteDTO EntidadPeajeCargoAjuste { get; set; }
        public VtpPeajeEmpresaAjusteDTO EntidadPeajeEmpresaAjuste { get; set; }
        public VtpSaldoEmpresaAjusteDTO EntidadSaldoEmpresaAjuste { get; set; }
        public VtpIngresoTarifarioAjusteDTO EntidadIngresoTarifarioAjuste { get; set; }
        public List<VtpPeajeCargoAjusteDTO> ListaPeajeCargoAjuste { get; set; }
        public List<VtpPeajeEmpresaAjusteDTO> ListaPeajeEmpresaAjuste { get; set; }
        public List<VtpSaldoEmpresaAjusteDTO> ListaSaldoEmpresaAjuste { get; set; }
        public List<VtpIngresoTarifarioAjusteDTO> ListaIngresoTarifarioAjuste { get; set; }
    }
}