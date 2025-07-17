using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Compensacion.Models
{
    public class MesValorizacionGeneralModel
    {
        public List<PeriodoDTO> ListTrnPeriodo { get; set; }
        public List<VcePeriodoCalculoDTO> ListVcePeriodoCalculo { get; set; }

        public PeriodoDTO TrnPeriodoDTO { get; set; }

        public VcePeriodoCalculoDTO VcePeriodoCalculoDTO { get; set; }

        // DSH 05-07-2017 : Se modifico por requerimiento
        public List<ComboCompensaciones> ListTrnCostoMarginal { get; set; }
        public List<VceLogCargaDetDTO> ListEntidades { get; set; }
        
    }
}