using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using System.Collections.Generic;


namespace COES.MVC.Intranet.Areas.Equipamiento.Models
{    
    public class FTAreasModel
    {
        public bool TienePermisoAdmin { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public bool Grabar { get; set; }

        public FtExtCorreoareaDTO Areas { get; set; }
        public List<FtExtCorreoareaDTO> ListadoAreas { get; set; }
        public List<UserCorreo> ListaCorreos { get; set; }
        public List<string> ListaCorreosPorArea { get; set; }
        public string correos { get; set; }
        public bool ExisteCorreosAdminFT { get; set; }

        public string correosRolPermisoTotal { get; set; }
        public string correosRolSoloNoConfidenciales { get; set; }

    }

}