using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Models
{
    public class DemandaUsuariosModel : BaseModel
    {
        public string sPeriodoInicio { get; set; }
        public string sPeriodoFinal { get; set; }

        public List<CaiGenerdemanDTO> ListaDemandaUsuarios { get; set; }
    }
}