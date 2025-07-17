using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Evaluacion.Models
{
    public class Respuesta
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public object Datos { get; set; }

        public int CodigoPrograma { get; set; }

        public int CodigoCuadroPrograma { get; set; }

        public int RegistrosObservados { get; set; }

        public int RegistrosProcesados { get; set; }
        public List<EprCargaMasivaLineaDTO> ListaErrores { get; set; }

        public List<EprCargaMasivaCeldaAcoplamientoDTO> ListaErroresCeldasAcoplamiento { get; set; }

        public List<EprCargaMasivaTransformadorDTO> ListaErroresTransformadores { get; set; }
    }
}