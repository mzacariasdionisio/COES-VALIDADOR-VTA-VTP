using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Models
{
    public class GruposVsUnidadesModel : BaseModel
    {
        public List<CaiEquisddpuniDTO> ListaEquisddpuni { get; set; }
        public CaiEquisddpuniDTO Entidad { get; set; }

        public string Casddufecvigencia { get; set; }


    }
}