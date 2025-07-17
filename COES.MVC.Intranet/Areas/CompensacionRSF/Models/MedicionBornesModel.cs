using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Models
{
    public class MedicionBornesModel : BaseModel
    {

        //Lista de tablas
        public List<VcrMedborneDTO> ListaMedBorne { get; set; }
        public List<VcrMedbornecargoincpDTO> ListaMedBorneCargoIncp { get; set; }
        
    }
}