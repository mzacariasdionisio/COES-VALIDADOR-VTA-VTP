using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    public class VolumenEmbalseModel
    {
        public bool TienePermisoNuevo { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public HandsonModel Handson { get; set; }
        public int Mtopcodi { get; set; }
        public string Titulo { get; set; }
        public bool UsarLayoutModulo { get; set; }
    }
}