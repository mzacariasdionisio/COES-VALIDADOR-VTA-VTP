using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Models
{
    public class GeneracionProyectadaModel : BaseModel
    {
        public List<CaiSddpDuracionDTO> ListaDuracion { get; set; }
        public CaiSddpDuracionDTO EntidadDuracion { get; set; }

        public List<CaiSddpGenmargDTO> ListaGen { get; set; }
        public CaiSddpGenmargDTO EntidadGen { get; set; }

        public CaiSddpParamsemDTO EntidadSddpSemana { get; set; }
        public List<CaiSddpParamsemDTO> ListNumeroSemana { get; set; }

        public CaiSddpParametroDTO EntidadSddpParametro { get; set; }
        public List<CaiSddpParametroDTO> ListaParametro { get; set; }

        public CaiSddpParamintDTO EntidadSddpIntervalo { get; set; }
        public List<CaiSddpParamintDTO> ListNumeroBloque { get; set; }
        
        public CaiSddpParamdiaDTO EntidadSddpDia { get; set; }
        public List<CaiSddpParamdiaDTO> ListDiaLaboral { get; set; }

        public CaiGenerdemanDTO EntidadGenerDemanda { get; set; }
        public List<CaiGenerdemanDTO> ListGenerDemanda { get; set; }

        public List<CaiSddpDuracionDTO> ListaDuracionPorEtapa { get; set; }
    }
}