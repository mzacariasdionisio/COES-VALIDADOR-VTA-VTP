using COES.Dominio.DTO.Transferencias;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Models
{
    public class UnidadesVsBarrasModel : BaseModel
    {
        public List<CaiEquiunidbarrDTO> ListaEquiunidbarr { get; set; }
        public CaiEquiunidbarrDTO Entidad { get; set; }
        public int IdUnidadesVsBarras { get; set; }
        public string Caiunbbarra { get; set; }
        public string Caiunbfecvigencia { get; set; }  
    }
}