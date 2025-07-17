using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Models
{
    public class ProvisionBaseModel : BaseModel
    {
        public VcrProvisionbaseDTO EntidadProvisionbase { get; set; }
        public List<VcrProvisionbaseDTO> ListaProvisionbase { get; set; }
        public CentralGeneracionDTO EntidadCentralGeneracion { get; set; }
        public List<CentralGeneracionDTO> ListaCentralGeneracion { get; set; }
        public TrnBarraursDTO EntidadURS { get; set; }
        public List<TrnBarraursDTO> ListaURS { get; set; }
        
        //atributos adicionales
        public string Vcrpbperiodoini { get; set; }
        public string Vcrpbperiodofin { get; set; } 
        public bool bEjecutar { get; set; }
    }
}