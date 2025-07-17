using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Models
{
    public class MaximaDemandaModel :  BaseModel
    {
        public List<CaiMaxdemandaDTO> ListaMaximaDemanda { get; set; }
        public CaiMaxdemandaDTO EntidadMaximaDemanda { get; set; }
        public int Caimdecodi { get; set; }
        public string Codfech { get; set; }
        public string Codhor { get; set; }

    }
}