using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Models
{
    public class ElementoStModel : BaseModel
    {
        public List<StSistematransDTO> ListaSistema { get; set; }
        public StSistematransDTO EntidadSistema { get; set; }

        //Para Grabar directo en Carga de Archivos ST_FACTOR
        public StFactorDTO EntidadFactorActualizacion { get; set; }
    }
}