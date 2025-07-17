using COES.Dominio.DTO.Transferencias;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Models
{
    public class AjusteEmpresaModel : BaseModel
    {
        public List<CaiAjusteempresaDTO> ListaAjusteEmpresa { get; set; }
        public CaiAjusteempresaDTO EntidadAjusteEmpresa { get; set; }
        public int Caiajecodi { get; set; }
        public string Caiajetipoinfo { get; set; }

        //atributos para el manejo de fecha
        public string Caiajereteneejeini { get; set; }
        public string Caiajereteneejefin { get; set; }
        public string Caiajeretenepryaini { get; set; }
        public string Caiajeretenepryafin { get; set; }
        public string Caiajereteneprybini { get; set; }
        public string Caiajereteneprybfin { get; set; }

    }
}