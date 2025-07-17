using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.StockCombustibles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.StockCombustibles.Models
{
    public class NotificacionModel
    {
        public bool TienePermisoAdmin { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public SiPlantillacorreoDTO PlantillaCorreo { get; set; }
        public List<VariableCorreo> ListaVariables { get; set; }
        public bool Grabar { get; set; }
        public int IdPlantillaCorreo { get; set; }

        public MeEnvcorreoConfDTO ConfiguracionCorreo { get; set; }

        public string Firmante { get; set; }
        public string Cargo { get; set; }
        public string Anexo { get; set; }
        public string HoraEjecucion { get; set; }
        public string EstadoEjecucion { get; set; }

        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<MeFormatoDTO> ListaFormato { get; set; }
    }
}