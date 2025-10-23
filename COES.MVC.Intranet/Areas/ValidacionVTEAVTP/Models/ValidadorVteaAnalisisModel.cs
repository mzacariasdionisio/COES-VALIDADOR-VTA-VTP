using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.ValidacionVTEAVTP;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Models
{
    public class ValidadorVteaAnalisisModel
    {
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }

        public string StrMensajeError {  get; set; }       

        public TrnPeriodoDTO PeriodoValorizacion { get; set; }
        
        public VteaVersionDTO VersionesVtea { get; set; }

        public VteaValidadorDTO DatosValidadorVTEA { get; set; }
        
        public string VistaRolEmpresa {  get; set; }
        public string VistaEnergia { get; set; }    

        public VteaHistRolDTO DatosHisRol { get; set; }

        public VteaDcUnitDTO DetalleDiasPeriodo {  get; set; }

        public VteaDetailDTO DetalleEmpresaEnergiaDia { get; set; }
        
        public string PeriodoSeleccionado { get; set; }
        public string VersionSeleccionado { get; set; }
    }
}