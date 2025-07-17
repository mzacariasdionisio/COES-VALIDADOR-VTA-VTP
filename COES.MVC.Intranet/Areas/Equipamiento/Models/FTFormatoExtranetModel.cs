using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using System.Collections.Generic;


namespace COES.MVC.Intranet.Areas.Equipamiento.Models
{    
    public class FTFormatoExtranetModel
    {
        public bool TienePermisoAdmin { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string FechaActual { get; set; }
        public FtExtEventoDTO FTExtEvento { get; set; }               

        public List<FtExtEventoDTO> ListadoFtExtEventos { get; set; }
        public List<FtExtEventoReqDTO> ListaDetalleFTExtEvento { get; set; }

        public string Fecha { get; set; }
        public int? Famcodi { get; set; }
        public int? Catecodi { get; set; }

        public List<FtFictecXTipoEquipoDTO> ListaFichaTecnica { get; set; }
        public List<FtExtEtapaDTO> ListaEtapas { get; set; }
        public int ExisteInfoGuardada { get; set; }
        public FTFormatoExtranet FormatoExtranet { get; set; }
    }

}