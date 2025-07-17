using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Models
{
    public class IncumpPR21Model : BaseModel
    {
        //master
        public VcrVersionincplDTO EntidadIncumpPR21 { get; set; }
        public List<VcrVersionincplDTO> ListaIncumpPR21 { get; set; }
        //hijo
        public VcrVerincumplimDTO EntidadDetalle { get; set; }
        public List<VcrVerincumplimDTO> ListaDetalle { get; set; }

        public VcrVerporctreservDTO EntidadDetalleRPNS { get; set; }
        public List<VcrVerporctreservDTO> ListaDetalleRPNS { get; set; }
        public string Vcrincfeccreacion { get; set; }
        public int vcrinccodi { get; set; }
        //Atributos Adicionales
        public int idCentral { get; set; }
        public int idUnidad { get; set; }
        
    }
}