using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Yupana.Helper;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Yupana.Models
{
    public class GeneracionMetaModel
    {
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public string Anho { get; set; }
        public string Mes { get; set; }
        public string Semana { get; set; }
        public string Dia { get; set; }
        public int NroSemana { get; set; }
        public List<TipoInformacion> ListaSemana { get; set; }
        public List<EnvioRestriccion> ListaEnvios { get; set; }
        public string FechaIniSem { get; set; }
        public string FechaFinSem { get; set; }
        public int IdEnvio { get; set; }
        public DatoRestricciones DataRestricciones { get; set; }
        public string FechaProceso { get; set; }
    }
}