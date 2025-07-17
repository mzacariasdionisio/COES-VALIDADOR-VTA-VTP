using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Models
{
    public class CargaArchivosModel : BaseModel
    {
        //Objetos del Modelo Distancia Electrica
        public StDistelectricaDTO EntidadDistelectrica { get; set; }
        public List<StDistelectricaDTO> ListaDistelectrica { get; set; }
        public StDsteleBarraDTO EntidadDistelecBarra { get; set; }
        public List<StDsteleBarraDTO> ListaDistelecBarra { get; set; }

        //Objetos del Modelo Energias Netas
        public StEnergiaDTO EntidadEnergiaNeta { get; set; }
        public List<StEnergiaDTO> ListaEnergiaNeta { get; set; }

        //Objetos del Modelo Responsabilidad de Pagos
        public StRespagoDTO EntidadCentralesResp { get; set; }
        public List<StRespagoDTO> ListaCentralesResp { get; set; }
        public StRespagoeleDTO EntidadCentralesRespEle { get; set; }
        public List<StRespagoeleDTO> ListaCentralesRespEle { get; set; }

        //Objetos del Modelo Compensacion Mensual
        public StCompmensualDTO EntidadCompMensual { get; set; }
        public List<StCompmensualDTO> ListaCompMensual { get; set; }
        public StCompmensualeleDTO EntidadCompMensualEle { get; set; }
        public List<StCompmensualeleDTO> ListaCompMensualEle { get; set; }

        //Objetos del Modelo Factor Actualizacion
        public StFactorDTO EntidadFactorActualizacion { get; set; }
        public List<StFactorDTO> ListaFactorActualizacion { get; set; }

    }
}