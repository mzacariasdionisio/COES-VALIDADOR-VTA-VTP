
namespace COES.MVC.Intranet.Areas.Proteccion.Models
{
    public class Respuesta
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public object Datos { get; set; }

        public int CodigoPrograma { get; set; }

        public int CodigoCuadroPrograma { get; set; }

        public int RegistrosObservados { get; set; }
    }
}