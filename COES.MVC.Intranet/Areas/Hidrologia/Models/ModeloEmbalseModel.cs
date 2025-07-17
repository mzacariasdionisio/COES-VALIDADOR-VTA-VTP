using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Hidrologia;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Hidrologia.Models
{
    public class ModeloEmbalseModel
    {

        public string Fecha { get; set; }
        public CmModeloEmbalseDTO ModeloEmbalse { get; set; }
        public List<CmModeloEmbalseDTO> ListaEmbalse { get; set; }
        public ConfiguracionDefaultEmbalseHidrologia ConfiguracionDefault { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Detalle2 { get; set; }

    }

}